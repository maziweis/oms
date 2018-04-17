using mdt.BLL;
using mdt.Common;
using mdt.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace mdt
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            C_E _dn = new C_E();
            CacheData(System.IO.Directory.GetCurrentDirectory() + "/DirectoryName.xml", _dn, "DirectoryName");
            Edition _edi = new Edition();
            CacheData(System.IO.Directory.GetCurrentDirectory() + "/Edition.xml", _edi, "Edition");
            BindTree();
        }

        #region 绑定树
        public void BindTree()
        {
            Dictionary<int, string> _dicSubject = new Dictionary<int, string>();
            _dicSubject.Add(1, "语文");
            _dicSubject.Add(2, "数学");
            _dicSubject.Add(3, "英语");
            foreach (var item in _dicSubject)
            {
                TreeNode rootNode = new TreeNode();
                rootNode.Tag = item;
                rootNode.Text = item.Value;
                treeView1.Nodes.Add(rootNode);

                BindChilds(rootNode);
            }
        }

        private void BindChilds(TreeNode fNode)
        {
            KeyValuePair<int, string> _kvp = (KeyValuePair<int, string>)fNode.Tag;
            Dictionary<int, string> _dicEdit = BackEditions(_kvp.Key);
            foreach (var item in _dicEdit)
            {
                TreeNode node = new TreeNode();
                node.Tag = item;
                node.Text = item.Value;

                //添加子节点
                fNode.Nodes.Add(node);
                BindChi(_kvp.Key, node);
            }
        }

        private void BindChi(int subject, TreeNode fNode)
        {
            KeyValuePair<int, string> _kvp = (KeyValuePair<int, string>)fNode.Tag;
            Dictionary<int, string> _dicEdit = BackBooks(subject, _kvp.Key);
            foreach (var item in _dicEdit)
            {
                TreeNode node = new TreeNode();
                node.Tag = item;
                node.Text = item.Value;

                //添加子节点
                fNode.Nodes.Add(node);
            }
        }

        public Dictionary<int, string> BackEditions(int subject)
        {
            Dictionary<int, string> _dic = new Dictionary<int, string>();
            using (var db = new MOD_MetaDatabase_BranchEntities())
            {
                string _edit = BackEdition(subject.ToString());
                if (string.IsNullOrEmpty(_edit))
                    return _dic;
                _dic = db.tb_Code_ListTable3.SqlQuery(string.Format(@"  SELECT * FROM [dbo].[tb_Code_ListTable3]
                  WHERE Deleted=0 AND ID IN(
                  SELECT Edition FROM [dbo].[tb_StandardBook]
                  WHERE Deleted=0 AND Subject={0} AND Edition IN({1}) GROUP BY Edition)", subject, _edit)).ToDictionary(_ => _.ID, _ => _.CodeName);
            }
            return _dic;
        }

        public Dictionary<int, string> BackBooks(int subject, int edition)
        {
            List<C_E> _listdn = OMS.Common.CacheHelper.GetCache("DirectoryName") as List<C_E>;
            Dictionary<int, string> _dic = new Dictionary<int, string>();
            using (var db = new MOD_MetaDatabase_BranchEntities())
            {
                //_dic = db.tb_StandardBook.SqlQuery(string.Format(@"  SELECT * FROM [dbo].[tb_StandardBook]
                //WHERE Deleted=0 AND Subject={0} AND Edition={1}", subject, edition)).ToDictionary(_ => _.ID, _ => _.BooKName);
                var _list = db.tb_StandardBook.SqlQuery(string.Format(@"  SELECT *,
                RANK() OVER(PARTITION BY Edition, Subject, Grade, Booklet ORDER BY ID asc) AS Rank
                FROM [dbo].[tb_StandardBook] where Deleted = 0 AND Subject={0} AND Edition={1}", subject, edition)).ToList();
                if (_list.Count > 0)
                {
                    foreach (var item in _list)
                    {
                        var _ce = _listdn.Where(_ => _.ID == item.ID.ToString()).FirstOrDefault();
                        if (_ce == null)
                            continue;
                        _dic.Add(item.ID, item.BooKName + "|" + _ce.Name);
                    }
                }
            }
            return _dic;
        }
        #endregion

        #region 根据科目编号返回版本编号
        /// <summary>
        /// 根据科目编号返回版本编号
        /// </summary>
        /// <returns></returns>
        public string BackEdition(string subjectid)
        {
            string _edit = string.Empty;
            List<Edition> _listedi = OMS.Common.CacheHelper.GetCache("Edition") as List<Edition>;
            var _qu = _listedi.Where(_ => _.SubjectID == subjectid).ToList();
            foreach (var item in _qu)
            {
                _edit += item.EditionID + ",";
            }
            return _edit.TrimEnd(',');
        }
        #endregion

        #region 全选、反选
        private void treeView1_AfterCheck(object sender, TreeViewEventArgs e)
        {
            if (e.Action == TreeViewAction.ByMouse)
            {
                if (e.Node.Checked == true)
                {
                    //选中节点之后，选中该节点所有的子节点
                    setChildNodeCheckedState(e.Node, true);
                }
                else if (e.Node.Checked == false)
                {
                    //取消节点选中状态之后，取消该节点所有子节点选中状态
                    setChildNodeCheckedState(e.Node, false);
                    //如果节点存在父节点，取消父节点的选中状态
                    if (e.Node.Parent != null)
                    {
                        setParentNodeCheckedState(e.Node, false);
                    }
                }
            }
        }
        ////取消节点选中状态之后，取消所有父节点的选中状态
        private void setParentNodeCheckedState(TreeNode currNode, bool state)
        {
            TreeNode parentNode = currNode.Parent;
            parentNode.Checked = state;
            if (currNode.Parent.Parent != null)
            {
                setParentNodeCheckedState(currNode.Parent, state);
            }
        }
        //选中节点之后，选中节点的所有子节点
        private void setChildNodeCheckedState(TreeNode currNode, bool state)
        {
            TreeNodeCollection nodes = currNode.Nodes;
            if (nodes.Count > 0)
            {
                foreach (TreeNode tn in nodes)
                {
                    tn.Checked = state;
                    setChildNodeCheckedState(tn, state);
                }
            }
        }
        #endregion

        List<TreeNode> _list = new List<TreeNode>();
        private void btnExport_Click(object sender, EventArgs e)
        {
            CallRecursive(this.treeView1);
            if (_list.Count == 0)
            {
                MessageBox.Show("请至少勾选一本书！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            this.btnExport.Visible = false;//隐藏导出按钮，避免用户在前一次没完成之前，开始第二次


            //电子书资源存放位置
            string _ResourceUrl = ConfigurationManager.AppSettings["ResourceUrl"].ToString();
            //资源文件存放目录
            string _OtherUrl = ConfigurationManager.AppSettings["OtherUrl"].ToString();
            //缩略图存放位置
            string _ImgUrl = ConfigurationManager.AppSettings["ImgUrl"].ToString();
            string _directory = System.IO.Directory.GetCurrentDirectory() + "/" + DateTime.Now.ToString("yyyyMMddHHmmss");
            List<C_E> _listdn = OMS.Common.CacheHelper.GetCache("DirectoryName") as List<C_E>;
            foreach (var items in _list)
            {
                if (items.Text.Split('|').Count() <= 1)
                    continue;

                int _bookId = Convert.ToInt32(_listdn.Where(_ => _.Name == items.Text.Split('|')[1]).FirstOrDefault().ID);
                if (_bookId == 0)
                    continue;
                List<Editions> _listediti = new List<Editions>();
                List<Books> _lb = new List<Books>();
                List<SE> _lse = new List<SE>();//用于记录用户所勾选的科目、版本
                List<Resources> _lrs = new List<Resources>();//记录碎片化资源
                string _url = _directory + "/" + _listdn.Where(_ => _.ID == _bookId.ToString()).FirstOrDefault().Name + "/Applications/";
                if (!Directory.Exists(_url))
                {
                    Directory.CreateDirectory(_url);
                }
                string _xmlurl = _url + "/Data";
                if (!Directory.Exists(_xmlurl))
                {
                    Directory.CreateDirectory(_xmlurl);
                }
                using (var db = new MOD_MetaDatabase_BranchEntities())
                {

                    var query = db.tb_Code_ListTable3.SqlQuery(string.Format(@"  SELECT * FROM [dbo].[tb_Code_ListTable3]
                WHERE Deleted = 0 AND ID IN(SELECT Edition FROM [dbo].[tb_StandardBook] WHERE Deleted=0 AND ID IN({0}))", _bookId)).ToList();
                    if (query.Count > 0)
                    {
                        var query1 = db.tb_StandardBook.SqlQuery(string.Format(@"  SELECT * FROM [dbo].[tb_StandardBook]
                    WHERE Deleted=0 AND ID IN ({0})", _bookId)).ToList();

                        //    _lse = db.Database.SqlQuery<SE>(string.Format(@"SELECT Subject,Edition FROM [dbo].[tb_StandardBook]
                        //WHERE Deleted=0 AND ID IN({0})
                        //GROUP BY Subject,Edition", _bookId)).ToList();

                        if (query1.Count > 0)
                        {

                            var query2 = db.tb_StandardCatalog.SqlQuery(string.Format(@" SELECT * FROM [dbo].[tb_StandardCatalog]
                        WHERE Deleted = 0 AND BookID IN(SELECT ID FROM [dbo].[tb_StandardBook] WHERE Deleted=0 AND ID IN({0}))", _bookId)).ToList();
                            if (query2.Count > 0)
                            {
                                #region 导出目录
                                List<BookCatalogs> _lbc = new List<BookCatalogs>();
                                foreach (var item in query2)
                                {

                                    _lbc.Add(new BookCatalogs
                                    {
                                        CatalogId = item.ID,
                                        BookId = item.BookID,
                                        PId = item.ParentID,
                                        CatalogName = item.FolderName,
                                        Sort = item.Seq
                                    });
                                }
                                Helper.CreateXML(_xmlurl + "/bookcatalog.xml", _lbc);
                                #endregion
                            }
                        }
                    }

                    var query3 = db.tb_Code_TreeTable2.Where(_ => _.Deleted == 0).ToList();
                    if (query3.Count > 0)
                    {
                        #region 导出资源类型
                        List<ResourceTypes> _lrty = new List<ResourceTypes>();
                        foreach (var item in query3)
                        {
                            if (string.IsNullOrEmpty(mdt.dict.ResType.GetVal(Convert.ToInt32(item.ID))))
                                continue;
                            _lrty.Add(new ResourceTypes()
                            {
                                ID = item.ID,
                                CodeName = item.CodeName,
                                ParentID = item.ParentID,
                                Sort = item.Seq == null ? "" : item.Seq.ToString()
                            });
                        }
                        Helper.CreateXML(_xmlurl + "/resourcetype.xml", _lrty);
                        #endregion
                    }

                }

                List<Resource> _list_resource = new List<Resource>();
                using (var db = new MOD_Resource_BranchEntities())
                {
                    _list_resource = db.Database.SqlQuery<Resource>(string.Format(@"SELECT b.FileID,b.Catalog,b.SchoolStage,b.Grade,b.Subject,b.Edition,b.BookReel,b.ResourceClass,b.ResourceStyle,b.ResourceType,b.ComeFrom,b.KeyWords,b.Title,b.Description,c.ParentID,c.BookID 
                    FROM [MOD_ResourceLibrary].[dbo].[tb_Resource] b
                    INNER JOIN [MOD_MetaDatabase].[dbo].[tb_StandardCatalog] c ON b.Catalog=c.ID AND c.Deleted=0
                    WHERE c.BookID= {0} and b.IsDelete= 0", _bookId)).ToList();
                }
                if (_list_resource.Count > 0)
                {
                    using (var db = new MOD_kingfiles_ZYC_BranchEntities())
                    {                        
                        foreach (var res in _list_resource)
                        {
                            if (string.IsNullOrEmpty(mdt.dict.ResType.GetVal(Convert.ToInt32(res.ResourceStyle))) && res.ResourceType != 10)
                            {
                                continue;
                            }
                            
                            var _qu = db.tb_Files.Where(_ => _.ID == res.FileID).ToList();
                            if (_qu.Count > 0)
                            {
                                foreach (var itqu in _qu)
                                {
                                    _lrs.Add(new Resources
                                    {
                                        BookId = res.BookID.ToString(),
                                        Catalog1 = res.Catalog.ToString(),
                                        ResId = itqu.ID.ToString(),
                                        Url = itqu.FilePath,
                                        FileName = itqu.FileName,
                                        BookReel = res.BookReel == null ? "" : res.BookReel.ToString(),
                                        ComeFrom = res.ComeFrom,
                                        Cover = Helper.IsExist(_ImgUrl + "/" + itqu.ID + ".jpg") == true ? _listdn.Where(_ => _.ID == _bookId.ToString()).FirstOrDefault().Name + "/Applications/KingsunFiles/" + itqu.FilePath.Replace("\\", "/") + "/thumb/" + itqu.ID + ".jpg" : "",
                                        Edition = res.Edition == null ? "" : res.Edition.ToString(),
                                        ResourceClass = res.ResourceClass == null ? "" : res.ResourceClass.ToString(),
                                        ResourceStyle = res.ResourceStyle == null ? "" : res.ResourceStyle.ToString(),
                                        ResourceType = res.ResourceType == null ? "" : res.ResourceType.ToString(),
                                        Title = res.Title,
                                        KeyWords = res.KeyWords,
                                        Description = res.Description,
                                        SchoolStage = res.SchoolStage == null ? "" : res.SchoolStage.ToString(),
                                        Subject = res.Subject == null ? "" : res.Subject.ToString(),
                                        Grade = res.Grade == null ? "" : res.Grade.ToString(),
                                        Extension = itqu.FileExtension,
                                        ParentID = res.ParentID
                                    });
                                    if (this.chkResource.Checked)
                                        Helper.CopyImg(_ImgUrl + "/" + itqu.ID + ".jpg", _url + "/KingsunFiles/" + itqu.FilePath + "/thumb");
                                }
                            }
                        }
                    }
                }
                if (_lrs.Count > 0)
                    Helper.CreateXML(_xmlurl + "/resources.xml", _lrs);

                //using (var db = new MOD_kingfiles_ZYC_BranchEntities())
                //{
                //    var query = db.Database.SqlQuery<Files>(string.Format(@"   SELECT a.*,b.Catalog,b.SchoolStage,b.Grade,b.Subject,
                //    b.Edition,b.BookReel,b.ResourceClass,b.ResourceStyle,b.ResourceType,b.ComeFrom,b.KeyWords,b.Title,b.Description,c.ParentID,c.BookID 
                //    FROM [MOD_KingsunFiles].[dbo].[tb_Files] a
                //    INNER JOIN [MOD_ResourceLibrary].[dbo].[tb_Resource] b ON a.ID=b.FileID AND b.IsDelete=0
                //    INNER JOIN [MOD_MetaDatabase].[dbo].[tb_StandardCatalog] c ON b.Catalog=c.ID AND c.Deleted=0
                //    WHERE a.ID IN(SELECT FileID FROM  [MOD_ResourceLibrary].[dbo].[tb_Resource] WHERE IsDelete=0)
                //    AND c.BookID= {0}", _bookId)).ToList();

                //    if (query.Count > 0)
                //    {
                //        #region 导出碎片化资源
                //        foreach (var item in query)
                //        {
                //            if (item.ParentID == 0)
                //            {
                //                if (item.ResourceType != 10)
                //                    continue;
                //            }
                //            else
                //            {
                //                if (string.IsNullOrEmpty(mdt.dict.ResType.GetVal(Convert.ToInt32(item.ResourceStyle))))
                //                    continue;
                //            }
                //            _lrs.Add(new Resources
                //            {
                //                BookId = item.BookID.ToString(),
                //                Catalog1 = item.Catalog.ToString(),
                //                ResId = item.ID.ToString(),
                //                Url = item.FilePath,
                //                FileName = item.FileName,
                //                BookReel = item.BookReel == null ? "" : item.BookReel.ToString(),
                //                ComeFrom = item.ComeFrom,
                //                Cover = Helper.IsExist(_ImgUrl + "/" + item.ID + ".jpg") == true ? "/Resources/KingsunFiles/" + item.FilePath.Replace("\\", "/") + "/thumb/" + item.ID + ".jpg" : "",
                //                Edition = item.Edition == null ? "" : item.Edition.ToString(),
                //                ResourceClass = item.ResourceClass == null ? "" : item.ResourceClass.ToString(),
                //                ResourceStyle = item.ResourceStyle == null ? "" : item.ResourceStyle.ToString(),
                //                ResourceType = item.ResourceType == null ? "" : item.ResourceType.ToString(),
                //                Title = item.Title,
                //                KeyWords = item.KeyWords,
                //                Description = item.Description,
                //                SchoolStage = item.SchoolStage == null ? "" : item.SchoolStage.ToString(),
                //                Subject = item.Subject == null ? "" : item.Subject.ToString(),
                //                Grade = item.Grade == null ? "" : item.Grade.ToString(),
                //                Extension = item.FileExtension,
                //                ParentID = item.ParentID
                //            });
                //            if (this.chkResource.Checked)
                //                Helper.CopyImg(_ImgUrl + "/" + item.ID + ".jpg", _url + "/KingsunFiles/" + item.FilePath + "/thumb");
                //        }
                //        if (_lrs.Count > 0)
                //            Helper.CreateXML(_xmlurl + "/resources.xml", _lrs);
                //        #endregion
                //    }
                //    //}
                //}

                #region 复制碎片化资源到指定文件夹
                if (this.chkResource.Checked)
                {
                    if (_lrs.Count > 0)
                    {
                        ArrayList _arr = new ArrayList();
                        foreach (var item in _lrs)
                        {
                            //Helper.CopyFile(_OtherUrl + "/" + item.Url + "/" + item.ResId, _url + "/KingsunFiles/" + item.Url + "/" + item.ResId);
                            if (_arr.Contains(item.Url))
                                continue;
                            Helper.CopyOtherFile(_OtherUrl + "/" + item.Url, _url + "/KingsunFiles/" + item.Url, _lrs);
                            _arr.Add(item.Url);
                        }
                    }
                }
                #endregion




            }






            if (this.chkDec.Checked)
            {
                //加密数据
                Helper.EncryData(_directory, _directory + "Encry");
            }

            MessageBox.Show("导出成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.None);
            this.btnExport.Visible = true;//导出完成后，显示导出按钮

        }

        #region 遍历出所有选择的书
        private void CallRecursive(TreeView treeView)
        {
            TreeNodeCollection nodes = treeView.Nodes;
            foreach (TreeNode n in nodes)
            {
                PrintRecursive(n);
            }
        }

        private void PrintRecursive(TreeNode treeNode)
        {
            if (treeNode.Checked)
            {
                _list.Add(treeNode);
            }

            foreach (TreeNode tn in treeNode.Nodes)
            {
                PrintRecursive(tn);
            }
        }
        #endregion

        #region 缓存数据
        public static void CacheData<T>(string url, T t, string name)
        {
            List<T> _list = new List<T>();

            var fileUrl = url;
            if (File.Exists(fileUrl))
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(fileUrl);
                Type type = typeof(T);
                XmlNode datas = doc.SelectSingleNode("Datas");
                var rows = datas.SelectNodes("Row");

                if (rows != null && rows.Count > 0)
                {
                    foreach (XmlNode row in rows)
                    {

                        if (t != null)
                            t = System.Activator.CreateInstance<T>();
                        PropertyInfo[] pi = type.GetProperties();
                        XmlNodeList childs = row.ChildNodes;
                        for (int i = 0; i < childs.Count; i++)
                        {
                            XmlNode n = childs[i];
                            //查找属性名称和xml文档中一致的节点名称，并且设置属性值
                            var list = pi.Where(_ => _.Name == n.Name).ToList();
                            //注释掉的这种写法可以不必拘束于所有的变量都是string类型
                            //if (list[0].GetValue(t, null) == null)
                            list[0].SetValue(t, n.InnerText.ToString(), null);
                            //else
                            //    list[0].SetValue(t, null, null);
                        }
                        _list.Add(t);

                    }
                }
            }
            if (_list.Count > 0)
                OMS.Common.CacheHelper.SetCache(name, _list);
        }
        #endregion
    }
}
