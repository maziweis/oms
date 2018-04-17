using mdt.Common;
using mdt.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace mdt.BLL
{
    public class Helper
    {
        //加密时，以下后缀的就不加密
        private static List<string> noencry = new List<string> { ".kino", ".kbase", ".json", ".xml" };
        //用于排除掉要复制的文件名的后缀
        private static List<string> outfile = new List<string> { ".zip", ".swf", ".rar" };

        #region 实体转换成字典
        /// <summary>
        /// 实体转换成字典
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public static Dictionary<object, object> GetProperties<T>(T t)
        {
            var ret = new Dictionary<object, object>();

            if (t == null) { return null; }
            PropertyInfo[] properties = t.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
            if (properties.Length <= 0) { return null; }
            foreach (PropertyInfo item in properties)
            {
                try
                {
                    string name = item.Name;
                    object value = item.GetValue(t, null);
                    if (item.PropertyType.IsValueType || item.PropertyType.Name.StartsWith("String"))
                    {
                        ret.Add(name, value);
                    }
                }
                catch (Exception ex)
                {

                    Log.WriteLog("实体转换成字典出错：" + ex.Message);
                }

            }

            return ret;
        }
        #endregion

        #region 导出xml文件
        /// <summary>
        /// 导出xml文件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fileName"></param>
        /// <param name="list"></param>
        public static void CreateXML<T>(string fileName, List<T> list)
        {

            try
            {
                FileStream fileStream = new FileStream(fileName, FileMode.Create);
                XmlTextWriter writer = new XmlTextWriter(fileStream, Encoding.UTF8);
                if (list != null && list.Count > 0)
                {
                    writer.WriteStartDocument();
                    writer.WriteStartElement("Datas");    //创建父节点  

                    foreach (T it in list)
                    {
                        writer.WriteStartElement("Row");

                        Dictionary<object, object> _dic = GetProperties(it);
                        foreach (var item in _dic)
                        {
                            writer.WriteStartElement(item.Key.ToString());
                            writer.WriteValue(item.Value == null ? "" : item.Value);
                            writer.WriteEndElement();
                        }

                        writer.WriteEndElement();    //子节点结束  
                    }
                    writer.WriteEndElement();    //父节点结束  
                }
                writer.WriteEndDocument();
                writer.Close();
                fileStream.Close();
            }
            catch (Exception ex)
            {
                Log.WriteLog("导出xml文件出错：" + ex.Message);
            }
        }
        #endregion

        #region 导出json数据
        /// <summary>
        /// 导出json数据
        /// </summary>
        /// <param name="url"></param>
        /// <param name="list"></param>
        public static void CreateJson(string url, List<Books> list)
        {
            foreach (var item in list)
            {
                try
                {
                    TextBoxDataJson _tbdj = new TextBoxDataJson();
                    List<TextBoxPageDataJson> _listtbpdj = new List<TextBoxPageDataJson>();

                    string _path = url + item.BookId;
                    if (Directory.Exists(_path))
                    {
                        _tbdj.book = item.BookName;
                        List<string> folders = new List<string>(Directory.GetDirectories(_path));
                        List<string> fol = new List<string>();
                        folders.ForEach(c =>
                        {
                            if (!fol.Contains(Path.GetFileName(c)))
                                fol.Add(Path.GetFileName(c));
                        });

                        var fmlist = fol.Where(_ => _.Contains("fm")).ToList();
                        var fylist = fol.Where(_ => _.Contains("fy")).ToList();
                        var mllist = fol.Where(_ => _.Contains("ml")).ToList();
                        var pagelist = fol.Where(_ => _.Contains("page")).ToList();
                        var zfdlist = fol.Where(_ => _.Contains("zfd")).ToList();
                        //获取第一页的页码
                        int _firstPage = 0;
                        if (pagelist.Count > 0)
                            _firstPage = Convert.ToInt32(pagelist.First().ToString().Replace("page", ""));

                        #region fm
                        if (fmlist.Count > 0)
                        {
                            for (int i = 0; i < fmlist.Count; i++)
                            {
                                TextBoxPageDataJson _tbpdj = new TextBoxPageDataJson();
                                _tbpdj.pageId = (_firstPage - (fmlist.Count + fylist.Count + mllist.Count)) + i;
                                _tbpdj.pageImg = fmlist[i].ToString() + "/bg.jpg";
                                _tbpdj.buttons = new List<TextBoxPageButtonDataJson>();
                                _listtbpdj.Add(_tbpdj);
                            }
                        }
                        #endregion

                        #region fy
                        if (fylist.Count > 0)
                        {
                            for (int i = 0; i < fylist.Count; i++)
                            {
                                TextBoxPageDataJson _tbpdj = new TextBoxPageDataJson();
                                _tbpdj.pageId = (_firstPage - (fylist.Count + mllist.Count)) + i;
                                _tbpdj.pageImg = fylist[i].ToString() + "/bg.jpg";
                                _tbpdj.buttons = new List<TextBoxPageButtonDataJson>();
                                _listtbpdj.Add(_tbpdj);
                            }
                        }
                        #endregion

                        #region ml
                        if (mllist.Count > 0)
                        {
                            for (int i = 0; i < mllist.Count; i++)
                            {
                                TextBoxPageDataJson _tbpdj = new TextBoxPageDataJson();
                                _tbpdj.pageId = (_firstPage - mllist.Count) + i;
                                _tbpdj.pageImg = mllist[i].ToString() + "/bg.jpg";
                                _tbpdj.buttons = new List<TextBoxPageButtonDataJson>();
                                _listtbpdj.Add(_tbpdj);
                            }
                        }
                        #endregion

                        #region page
                        if (pagelist.Count > 0)
                        {
                            for (int i = 0; i < pagelist.Count; i++)
                            {
                                try
                                {
                                    TextBoxPageDataJson _tbpdj = new TextBoxPageDataJson();
                                    _tbpdj.pageId = Convert.ToInt32(pagelist[i].Replace("page", ""));
                                    _tbpdj.pageImg = pagelist[i].ToString() + "/bg.jpg";
                                    var files = Directory.GetFiles(_path + "/" + pagelist[i].ToString(), "*.xml");
                                    if (files != null && files.Length > 0)
                                    {
                                        foreach (var file in files)
                                        {
                                            List<TextBoxPageButtonDataJson> _list = new List<TextBoxPageButtonDataJson>();
                                            XmlDocument doc = new XmlDocument();
                                            doc.Load(file);
                                            XmlNode buts = doc.SelectSingleNode("buttons");
                                            var datas = buts.SelectNodes("button");
                                            if (datas != null && datas.Count > 0)
                                            {
                                                int j = 1;
                                                foreach (XmlNode data in datas)
                                                {
                                                    TextBoxPageButtonDataJson _btn = new TextBoxPageButtonDataJson();
                                                    _btn.id = j;
                                                    _btn.x = Convert.ToDouble(data["x"].InnerText);
                                                    _btn.y = Convert.ToDouble(data["y"].InnerText);
                                                    _btn.height = Convert.ToDouble(data["height"].InnerText);
                                                    _btn.width = Convert.ToDouble(data["width"].InnerText);
                                                    _btn.soundsrc = data["soundpath"].InnerText;
                                                    _btn.eventtype = 5;
                                                    _list.Add(_btn);
                                                    j++;
                                                }
                                            }
                                            _tbpdj.buttons = _list;
                                        }
                                    }
                                    else
                                        _tbpdj.buttons = new List<TextBoxPageButtonDataJson>();
                                    _listtbpdj.Add(_tbpdj);
                                }
                                catch (Exception)
                                {
                                    Log.WriteLog(string.Format("导出json数据出错，书的名称为：{0},编号为：{1}，页码为：{2}", item.BookName, item.BookId, pagelist[i]));
                                }

                            }
                        }
                        #endregion

                        #region zfd
                        if (zfdlist.Count > 0)
                        {
                            for (int i = 0; i < zfdlist.Count; i++)
                            {
                                TextBoxPageDataJson _tbpdj = new TextBoxPageDataJson();
                                _tbpdj.pageId = pagelist.Count + i + 1;
                                _tbpdj.pageImg = zfdlist[i].ToString() + "/bg.jpg";
                                _tbpdj.buttons = new List<TextBoxPageButtonDataJson>();
                                _listtbpdj.Add(_tbpdj);
                            }
                        }
                        #endregion

                        _tbdj.pageSource = _listtbpdj;
                        using (StreamWriter sw = new StreamWriter(_path + "/data.json", true, Encoding.UTF8))
                        {
                            sw.WriteLine(JsonHelper.SerializeObject(_tbdj));

                        }
                    }
                }
                catch (Exception ex)
                {
                    Log.WriteLog("导出json数据出错：" + ex.Message);
                }
            }


        }
        #endregion

        #region 加密数据
        public static void EncryData(string filePath, string fileOutPath)
        {
            if (!Directory.Exists(filePath))
            {
                MessageBox.Show("所选目录不存在!", "提示");
                return;
            }

            if (!Directory.Exists(fileOutPath))
            {
                if (fileOutPath.Trim() != "")
                {
                    Directory.CreateDirectory(fileOutPath);
                }
                else
                {
                    fileOutPath = new DirectoryInfo(filePath).Parent.FullName;
                }
            }
            try
            {
                DirectoryInfo objDir = null;
                DirectoryInfo sourceDir = new DirectoryInfo(filePath);
                if (sourceDir.Parent.FullName == fileOutPath)
                {
                    objDir = new DirectoryInfo(Path.Combine(fileOutPath, "Encrypt_" + sourceDir.Name));
                }
                else
                {
                    objDir = new DirectoryInfo(Path.Combine(fileOutPath, sourceDir.Name));
                }
                EncryptDir(sourceDir, sourceDir, objDir);
            }
            catch (Exception x)
            {

                MessageBox.Show(x.Message, "提示");
            }
        }
        private static void EncryptDir(DirectoryInfo currentDir, DirectoryInfo sourceDir, DirectoryInfo objDir)
        {
            DirectoryInfo[] childsDir = currentDir.GetDirectories();
            FileInfo[] childsFile = currentDir.GetFiles();
            for (int i = 0; i < childsFile.Length; i++)
            {
                EncryptFile(sourceDir, objDir, childsFile[i]);
            }
            for (int j = 0; j < childsDir.Length; j++)
            {
                EncryptDir(childsDir[j], sourceDir, objDir);
            }
        }

        private static void EncryptFile(DirectoryInfo sourceDir, DirectoryInfo objDir, FileInfo file)
        {
            if (noencry.Contains(file.Extension.ToLower()))
            {
                //方直内部数据文件不加密
                string objFilePath = file.FullName.Replace(sourceDir.FullName, objDir.FullName);
                FileInfo objCopyFile = new FileInfo(objFilePath);
                if (!objCopyFile.Directory.Exists)
                {
                    Directory.CreateDirectory(objCopyFile.Directory.FullName);
                }
                File.Copy(file.FullName, objFilePath);
                return;
            }
            string message = string.Empty;
            FileInfo objFile = new FileInfo(file.FullName.Replace(sourceDir.FullName, objDir.FullName));
            if (!objFile.Directory.Exists)
            {
                Directory.CreateDirectory(objFile.Directory.FullName);
            }
            DESEncrypt.EncryptFile(file.FullName, file.FullName.Replace(sourceDir.FullName, objDir.FullName), ref message);
        }
        #endregion

        #region 拼接条件（版本）
        /// <summary>
        /// 拼接条件（版本）
        /// </summary>
        /// <param name="_dic"></param>
        /// <param name="_strb"></param>
        public static void JoinData(Dictionary<int, string> _dic, StringBuilder _strb, int type, StringBuilder subject)
        {
            subject.Append(type + ",");
            foreach (var item in _dic)
            {
                _strb.Append(string.Format("'{0}',", item.Value));
            }
        }
        #endregion

        #region 拼接查询条件(导出书)
        /// <summary>
        /// 拼接查询条件(导出书)
        /// </summary>
        /// <param name="dic"></param>
        /// <param name="str"></param>
        /// <param name="subject"></param>
        public static void JoinCondi(Dictionary<int, string> dic, StringBuilder str, int subject)
        {
            string _ids = string.Empty;
            dic.ToList().ForEach(_ => _ids += string.Format("'{0}',", _.Value));
            str.Append(string.Format(@"(Subject={0} AND Edition IN(SELECT ID FROM [MOD_Meta_Branch].[dbo].[tb_Code_ListTable3]
                        WHERE Deleted = 0 AND CodeName IN({1}))) OR ", subject, _ids.TrimEnd(',')));
        }
        #endregion

        #region 验证数据
        /// <summary>
        /// 验证数据
        /// </summary>
        /// <param name="_dic"></param>
        /// <param name="ckl"></param>
        public static Dictionary<int, string> Vali(CheckedListBox ckl, bool isTrue)
        {
            Dictionary<int, string> _dic = new Dictionary<int, string>();
            if (!isTrue)
                return _dic;
            for (int i = 0; i < ckl.Items.Count; i++)
            {
                if (ckl.GetItemChecked(i))
                {
                    _dic.Add(i, ckl.GetItemText(ckl.Items[i]));
                }

            }
            return _dic;
        }
        #endregion

        #region 验证比较数据
        /// <summary>
        /// 验证比较数据
        /// </summary>
        /// <param name="_dic1"></param>
        /// <param name="_dic2"></param>
        /// <returns></returns>
        public static bool ValiData(Dictionary<int, string> _dic1, Dictionary<int, string> _dic2)
        {
            foreach (var item in _dic1)
            {
                foreach (var it in _dic2)
                {
                    if (item.Value == it.Value)
                        return true;
                }
            }
            return false;
        }
        #endregion

        #region 复制文件
        /// <summary>
        /// 复制文件
        /// </summary>
        /// <param name="sourcePath">数据源文件夹</param>
        /// <param name="destPath">目标文件夹</param>
        public static void CopyFile(string sourcePath, string destPath)
        {
            if (Directory.Exists(sourcePath))
            {
                if (!Directory.Exists(destPath))//目标目录不存在则创建
                {
                    try
                    {
                        Directory.CreateDirectory(destPath);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("创建目标目录失败！" + ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
                //获得源文件下所有文件
                List<string> files = new List<string>(Directory.GetFiles(sourcePath));
                files.ForEach(c =>
                {
                    string destFile = Path.Combine(new string[] { destPath, Path.GetFileName(c) });
                    File.Copy(c, destFile, true);//覆盖模式
                });
                //获得源文件下所有目录文件
                List<string> folders = new List<string>(Directory.GetDirectories(sourcePath));
                folders.ForEach(c =>
                {
                    string destDir = Path.Combine(new string[] { destPath, Path.GetFileName(c) });
                    //采用递归的方法实现
                    CopyFile(c, destDir);
                });
            }
            else
            {
                //MessageBox.Show("源目录不存在！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }
        #endregion

        #region 复制图片
        /// <summary>
        /// 复制图片
        /// </summary>
        /// <param name="sourceImg"></param>
        /// <param name="destpath"></param>
        /// <param name="fileName"></param>
        public static void CopyImg(string sourceImg, string destpath)
        {
            try
            {
                if (File.Exists(sourceImg))
                {
                    if (!Directory.Exists(destpath))
                    {
                        Directory.CreateDirectory(destpath);
                    }
                    string destFile = Path.Combine(new string[] { destpath, Path.GetFileName(sourceImg) });
                    File.Copy(sourceImg, destFile, true);
                }
            }
            catch (Exception ex)
            {

                Log.WriteLog("复制图片出错：" + ex.Message);
            }

        }
        #endregion

        #region 复制其他的文件
        /// <summary>
        /// 复制其他的文件
        /// </summary>
        /// <param name="source"></param>
        /// <param name="destpath"></param>
        /// <param name="list"></param>
        public static void CopyOtherFile(string source, string destpath, List<Resources> list)
        {
            if (Directory.Exists(source))
            {
                DirectoryInfo TheFolder = new DirectoryInfo(source);
                if (TheFolder.GetFiles().Length > 0)
                {
                    foreach (FileInfo NextFile in TheFolder.GetFiles())
                    {
                        try
                        {
                            //if (outfile.Contains(NextFile.Extension.ToLower()))
                            //    continue;
                            list.ForEach(_ =>
                            {
                                if (NextFile.Name == _.ResId + NextFile.Extension)
                                {
                                    CopyImg(source + "/" + NextFile.Name, destpath);
                                }
                            });
                        }
                        catch (Exception ex)
                        {

                            Log.WriteLog("复制其他的文件出错：" + ex.Message);
                        }

                    }
                }
            }
        }
        #endregion

        #region 返回版本
        /// <summary>
        /// 返回版本
        /// </summary>
        /// <param name="subject"></param>
        /// <returns></returns>
        public static Dictionary<int, string> BackEditions(int subject)
        {
            Dictionary<int, string> _dic = new Dictionary<int, string>();
            List<tb_TextBook> _list = BackTB();
            if (_list.Count == 0)
                return _dic;
            string _ids = string.Empty;
            _list.ForEach(_ => _ids += _.ClssId + ",");
            using (var db = new MOD_MetaDatabase_BranchEntities())
            {
                _dic = db.tb_Code_ListTable3.SqlQuery(string.Format(@"  SELECT * FROM [dbo].[tb_Code_ListTable3]
                  WHERE Deleted=0 AND ID IN(
                  SELECT Edition FROM [dbo].[tb_StandardBook]
                  WHERE Deleted=0 AND Subject={0} AND ID IN({1}) GROUP BY Edition)", subject, _ids.TrimEnd(','))).ToDictionary(_ => _.ID, _ => _.CodeName);
            }
            return _dic;
        }
        #endregion

        #region 返回书本
        /// <summary>
        /// 返回书本
        /// </summary>
        /// <returns></returns>
        public static List<tb_TextBook> BackTB()
        {
            List<tb_TextBook> _list = new List<tb_TextBook>();
            using (var db = new MOD_Resource_BranchEntities())
            {
                _list = db.tb_TextBook.AsQueryable().ToList();
            }
            return _list;
        }
        #endregion

        #region 判断图片是否存在
        /// <summary>
        /// 判断图片是否存在
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static bool IsExist(string url)
        {
            bool _result = false;
            try
            {
                if (File.Exists(url))
                    _result = true;

            }
            catch (Exception ex)
            {

                Log.WriteLog("判断图片是否存在出错：" + ex.Message);
            }
            return _result;
        }
        #endregion
    }
}
