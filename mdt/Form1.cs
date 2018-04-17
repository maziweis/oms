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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Dictionary<int, string> _dicChinese = Helper.BackEditions(1);
            foreach (var item in _dicChinese)
            {
                this.cklChinese.Items.Add(item.Value);
            }
            Dictionary<int, string> _dicMath = Helper.BackEditions(2);
            foreach (var item in _dicMath)
            {
                this.cklMath.Items.Add(item.Value);
            }
            Dictionary<int, string> _dicEnglish = Helper.BackEditions(3);
            foreach (var item in _dicEnglish)
            {
                this.cklEnglish.Items.Add(item.Value);
            }
        }

        private void btnok_Click(object sender, EventArgs e)
        {
            //电子书资源存放位置
            string _ResourceUrl = ConfigurationManager.AppSettings["ResourceUrl"].ToString();
            //资源文件存放目录
            string _OtherUrl = ConfigurationManager.AppSettings["OtherUrl"].ToString();
            //缩略图存放位置
            string _ImgUrl = ConfigurationManager.AppSettings["ImgUrl"].ToString();
            string _directory = System.IO.Directory.GetCurrentDirectory() + "/" + DateTime.Now.ToString("yyyyMMddHHmmss");
            Dictionary<int, string> _dicChinese = new Dictionary<int, string>();
            Dictionary<int, string> _dicMath = new Dictionary<int, string>();
            Dictionary<int, string> _dicEnglish = new Dictionary<int, string>();
            Dictionary<int, string> _dicC = Helper.BackEditions(1);
            Dictionary<int, string> _dicM = Helper.BackEditions(2);
            Dictionary<int, string> _dicE = Helper.BackEditions(3);
            bool _chinese = this.chkChinese.Checked;
            bool _math = this.chkMath.Checked;
            bool _english = this.chkEnglish.Checked;

            #region 验证信息
            if (!_chinese && !_math && !_english)
            {
                MessageBox.Show("请至少勾选一个科目！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            _dicChinese = Helper.Vali(cklChinese, _chinese);
            _dicMath = Helper.Vali(cklMath, _math);
            _dicEnglish = Helper.Vali(cklEnglish, _english);
            if (_chinese)
            {
                if (!Helper.ValiData(_dicC, _dicChinese))
                {
                    MessageBox.Show("请至少勾选一个语文版本！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            if (_math)
            {
                if (!Helper.ValiData(_dicM, _dicMath))
                {
                    MessageBox.Show("请至少勾选一个数学版本！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            if (_english)
            {
                if (!Helper.ValiData(_dicE, _dicEnglish))
                {
                    MessageBox.Show("请至少勾选一个英语版本！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            #endregion

            this.btnok.Visible = false;//隐藏导出按钮，避免用户在前一次没完成之前，开始第二次

            StringBuilder _strb = new StringBuilder();
            StringBuilder _subject = new StringBuilder();
            if (_chinese)
                Helper.JoinData(_dicChinese, _strb, 1, _subject);
            if (_math)
                Helper.JoinData(_dicMath, _strb, 2, _subject);
            if (_english)
                Helper.JoinData(_dicEnglish, _strb, 3, _subject);

            List<Editions> _list = new List<Editions>();
            string _url = _directory + "/Data/Common/";
            if (!Directory.Exists(_url))
            {
                Directory.CreateDirectory(_url);
            }
            List<Books> _lb = new List<Books>();
            string _bookids = string.Empty;//记录书本
            List<Resources> _lrs = new List<Resources>();//记录碎片化资源
            StringBuilder _where = new StringBuilder();//查询条件
            using (var db = new MOD_MetaDatabase_BranchEntities())
            {

                var query = db.tb_Code_ListTable3.SqlQuery(string.Format(@"  SELECT * FROM [dbo].[tb_Code_ListTable3]
                WHERE Deleted = 0 AND CodeName IN({0})", _strb.ToString().TrimEnd(','))).ToList();
                if (query.Count > 0)
                {
                    #region 导出版本
                    int i = 1;
                    foreach (var item in query)
                    {

                        _list.Add(new Editions
                        {
                            EditionId = item.ID,
                            EditionName = item.CodeName,
                            Sort = i
                        });
                        i++;
                    }
                    Helper.CreateXML(_url + "/edition.xml", _list);
                    #endregion

                    #region 拼接查询条件
                    if (_chinese)
                        Helper.JoinCondi(_dicChinese, _where, 1);
                    if (_math)
                        Helper.JoinCondi(_dicMath, _where, 2);
                    if (_english)
                        Helper.JoinCondi(_dicEnglish, _where, 3);
                    #endregion

                    var query1 = db.tb_StandardBook.SqlQuery(string.Format(@"  SELECT * FROM [dbo].[tb_StandardBook]
                    WHERE Deleted=0 AND ({0})", _where.ToString().Remove(_where.ToString().Length - 3, 3))).ToList();
                    List<tb_TextBook> _ltb = Helper.BackTB();
                    if (query1.Count > 0 && _ltb.Count > 0)
                    {

                        var _list_result = (from t in query1
                                            from r in _ltb
                                            where t.ID == r.ClssId
                                            select t).ToList();
                        if (_list_result.Count == 0)
                            return;

                        #region 导出书
                        foreach (var item in _list_result)
                        {

                            _lb.Add(new Books
                            {
                                BookId = item.ID,
                                Booklet = item.Booklet,
                                BookName = item.BooKName,
                                Subject = item.Subject,
                                Grade = item.Grade,
                                Edition = item.Edition,
                                Remark = item.Remark,
                            });
                        }
                        Helper.CreateXML(_url + "/book.xml", _lb);
                        #endregion


                        _list_result.ForEach(_ => _bookids += _.ID + ",");
                        var query2 = db.tb_StandardCatalog.SqlQuery(string.Format(@" SELECT * FROM [dbo].[tb_StandardCatalog]
                        WHERE Deleted = 0 AND BookID IN({0})", _bookids.TrimEnd(','))).ToList();
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
                            Helper.CreateXML(_url + "/bookcatalog.xml", _lbc);
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
                        _lrty.Add(new ResourceTypes()
                        {
                            ID = item.ID,
                            CodeName = item.CodeName,
                            ParentID = item.ParentID,
                            Sort = item.Seq == null ? "" : item.Seq.ToString()
                        });
                    }
                    Helper.CreateXML(_url + "/resourcetype.xml", _lrty);
                    #endregion
                }

            }

            #region 导出教材每页默认关联资源
            using (var db = new fz_basicEntities())
            {
                List<BookRes> _lbr = new List<BookRes>();
                var query = db.dict_textbook_resource.SqlQuery(string.Format(@"  SELECT * FROM [dbo].[dict_textbook_resource]
                WHERE BookId IN({0})", _bookids.TrimEnd(','))).ToList();
                if (query.Count > 0)
                {
                    foreach (var item in query)
                    {
                        _lbr.Add(new BookRes
                        {
                            BookId = item.BookId,
                            Content = item.Content,
                            IsSys = 1,
                            PageIndex = item.PageIndex
                        });

                    }
                    Helper.CreateXML(_url + "/bookres.xml", _lbr);
                }
            }
            #endregion


            using (var db = new MOD_kingfiles_ZYC_BranchEntities())
            {

                var query = db.Database.SqlQuery<Files>(string.Format(@"   SELECT a.*,b.Catalog,b.SchoolStage,b.Grade,b.Subject,
                   b.Edition,b.BookReel,b.ResourceClass,b.ResourceStyle,b.ResourceType,b.ComeFrom,b.KeyWords,b.Title,b.Description,c.BookID 
                   FROM [MOD_kingfiles_ZYC_Branch].[dbo].[tb_Files] a
                  INNER JOIN [MOD_Resource_Branch].[dbo].[tb_Resource] b ON a.ID=b.FileID AND b.IsDelete=0
                  INNER JOIN [MOD_Meta_Branch].[dbo].[tb_StandardCatalog] c ON b.Catalog=c.ID AND c.Deleted=0
                  WHERE a.ID IN(SELECT FileID FROM  [MOD_Resource_Branch].[dbo].[tb_Resource] WHERE IsDelete=0)
                  AND c.BookID IN({0})", _bookids.TrimEnd(','))).ToList();

                if (query.Count > 0)
                {
                    #region 导出碎片化资源
                    foreach (var item in query)
                    {
                        _lrs.Add(new Resources
                        {
                            BookId = item.BookID.ToString(),
                            Catalog1 = item.Catalog.ToString(),
                            ResId = item.ID.ToString(),
                            Url = item.FilePath,
                            FileName = item.FileName,
                            BookReel = item.BookReel == null ? "" : item.BookReel.ToString(),
                            ComeFrom = item.ComeFrom,
                            Cover = "/Resources/KingsunFiles/" + item.FilePath.Replace("\\", "/") + "/thumb/" + item.ID + ".jpg",
                            Edition = item.Edition == null ? "" : item.Edition.ToString(),
                            ResourceClass = item.ResourceClass == null ? "" : item.ResourceClass.ToString(),
                            ResourceStyle = item.ResourceStyle == null ? "" : item.ResourceStyle.ToString(),
                            ResourceType = item.ResourceType == null ? "" : item.ResourceType.ToString(),
                            Title = item.Title,
                            KeyWords = item.KeyWords,
                            Description = item.Description,
                            SchoolStage = item.SchoolStage == null ? "" : item.SchoolStage.ToString(),
                            Subject = item.Subject == null ? "" : item.Subject.ToString(),
                            Grade = item.Grade == null ? "" : item.Grade.ToString(),
                            Extension = item.FileExtension
                        });
                        if (this.chkResource.Checked)
                            Helper.CopyImg(_ImgUrl + "/" + item.ID + ".jpg", _directory + "/KingsunFiles/" + item.FilePath + "/thumb");
                    }
                    Helper.CreateXML(_url + "/resources.xml", _lrs);
                    #endregion
                }
            }

            #region 复制资源到指定目录
            if (this.chkBook.Checked)
            {
                List<tb_TextBook> _listtb = Helper.BackTB();
                if (_lb.Count > 0 && _listtb.Count > 0)
                {
                    var _list_result = (from t in _listtb
                                        from r in _lb
                                        where t.ClssId == r.BookId
                                        select t).ToList();
                    if (_list_result.Count == 0)
                        return;
                    Dictionary<int, string> _dicBook = new Dictionary<int, string>();
                    _list_result.ForEach(_ =>
                    {
                        if (!_dicBook.ContainsKey(Convert.ToInt32(_.ClssId)))
                            _dicBook.Add(Convert.ToInt32(_.ClssId), _.TextBookPath.Split('/')[2].ToString());
                    });
                    foreach (var item in _dicBook)
                    {
                        Helper.CopyFile(_ResourceUrl + "/" + item.Value, _directory + "/TextBook/" + item.Key);
                    }
                    //导出json数据
                    Helper.CreateJson(_directory + "/TextBook/", _lb);
                }
            }
            #endregion

            #region 复制碎片化资源到指定文件夹
            if (this.chkResource.Checked)
            {
                if (_lrs.Count > 0)
                {
                    ArrayList _arr = new ArrayList();
                    foreach (var item in _lrs)
                    {
                        Helper.CopyFile(_OtherUrl + "/" + item.Url + "/" + item.ResId, _directory + "/KingsunFiles/" + item.Url + "/" + item.ResId);
                        if (_arr.Contains(item.Url))
                            continue;
                        Helper.CopyOtherFile(_OtherUrl + "/" + item.Url, _directory + "/KingsunFiles/" + item.Url, _lrs);
                        _arr.Add(item.Url);
                    }
                }
            }
            #endregion

            if (this.chkDec.Checked)
            {
                //加密数据
                Helper.EncryData(_directory, _directory + "Encry");
            }

            MessageBox.Show("导出成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.None);
            this.btnok.Visible = true;//导出完成后，显示导出按钮
        }
    }
}
