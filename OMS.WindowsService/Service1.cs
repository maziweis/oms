using BoxOms.Database;
using OMS.Common;
using OMS.WindowsService.BLL;
using OMS.WindowsService.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace OMS.WindowsService
{
    public partial class Service1 : ServiceBase
    {
        public Service1()
        {
            InitializeComponent();

            System.Timers.Timer timer = new System.Timers.Timer();
            timer.Elapsed += new System.Timers.ElapsedEventHandler(TimedEvent);
            timer.Interval = 60 * 1000;//每1分钟执行一次
            timer.Enabled = true;

            System.Timers.Timer timer1 = new System.Timers.Timer();
            timer1.Elapsed += new System.Timers.ElapsedEventHandler(TimedEventByRp);
            timer1.Interval = 60 * 1000;
            timer1.Enabled = true;

        }

        #region 盒子定时任务
        /// <summary>
        /// 盒子定时任务
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TimedEvent(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                string omsUrl = System.Configuration.ConfigurationManager.AppSettings["OMS_URL"].ToString();
                string boxLogUrl = string.Format("{0}\\Files\\Box\\Logs", omsUrl);
                if (Directory.Exists(boxLogUrl))
                {
                    var files = Directory.GetFiles(boxLogUrl, "*.xml");
                    if (files != null && files.Length > 0)
                    {
                        foreach (var file in files)
                        {
                            XmlDocument doc = new XmlDocument();
                            doc.Load(file);

                            XmlNode logs = doc.SelectSingleNode("Logs");
                            var datas = logs.SelectNodes("Datas");
                            if (datas != null && datas.Count > 0)
                            {
                                string mac = "";
                                string gcode = "";
                                DateTime? date = null;

                                using (var db = new box_omsEntities())
                                {
                                    foreach (XmlNode data in datas)
                                    {
                                        if (mac == "") mac = data["Mac"].InnerText;
                                        if (gcode == "") gcode = db.box_good.Where(w => w.Mac == mac).Select(s => s.Code).FirstOrDefault();
                                        if (date == null) date = DateTime.Parse(data["Time"].InnerText).Date;




                                        //01.操作oms_box_resource_statist表
                                        box_resource_statist m = new box_resource_statist();
                                        m.Id = Guid.NewGuid().ToString();
                                        m.Mac = mac;
                                        var good = db.box_good.Where(w => w.Mac == mac).FirstOrDefault();
                                        if (good != null)
                                            m.BoxId = good.BoxId;
                                        m.CreateDate = DateTime.Parse(data["Time"].InnerText);
                                        if (!string.IsNullOrWhiteSpace(data["Subject"].InnerText)) m.Subject = int.Parse(data["Subject"].InnerText);
                                        if (!string.IsNullOrWhiteSpace(data["Edition"].InnerText)) m.Edition = int.Parse(data["Edition"].InnerText);
                                        m.Type = int.Parse(data["Type"].InnerText);
                                        if (good != null)
                                        {
                                            if (good.FirstRunTime == null)
                                                good.FirstRunTime = DateTime.Parse(data["Time"].InnerText);
                                            good.RunTime = DateTime.Parse(data["Time"].InnerText);
                                        }
                                        if (data["TypeName"] != null)
                                            m.TypeName = data["TypeName"].InnerText;
                                        db.box_resource_statist.Add(m);
                                        db.SaveChanges();
                                    }
                                }

                                File.Delete(file);//删除文件
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                WriteErrLog(ex.ToString());
            }
        }
        #endregion

        #region 资源平台定时任务
        /// <summary>
        /// 资源平台定时任务
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TimedEventByRp(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                string omsUrl = System.Configuration.ConfigurationManager.AppSettings["OMS_URL"].ToString();
                string boxLogUrl = string.Format("{0}\\Files\\Rp\\Logs", omsUrl);
                if (Directory.Exists(boxLogUrl))
                {
                    var files = Directory.GetFiles(boxLogUrl, "*.xml");
                    if (files != null && files.Length > 0)
                    {
                        foreach (var file in files)
                        {
                            XmlDocument doc = new XmlDocument();
                            doc.Load(file);

                            XmlNode logs = doc.SelectSingleNode("Logs");
                            var datas = logs.SelectNodes("Datas");
                            if (datas != null && datas.Count > 0)
                            {
                                using (var db = new box_omsEntities())
                                {
                                    foreach (XmlNode data in datas)
                                    {
                                        ResMod rm = JsonHelper.DeserializeJsonToObject<ResMod>(data["Content"].InnerText);
                                        string id = DataHelper.BackId(rm.ActiveCode);
                                        if (string.IsNullOrWhiteSpace(id))
                                            continue;
                                        if (rm.ResourceType == "1" || rm.ResourceType == "3")
                                        {
                                            db.rp_resource_statist.Add(new rp_resource_statist()
                                            {
                                                Id = Guid.NewGuid().ToString(),
                                                Cd_key = id,
                                                EditionId = Convert.ToInt32(rm.EditionId),
                                                EditionName = rm.EditionName,
                                                ResourceType = Convert.ToInt32(rm.ResourceType),
                                                SubjectId = Convert.ToInt32(rm.SubjectId),
                                                Type = Convert.ToInt32(data["Type"].InnerText),
                                                CreateTime = Convert.ToDateTime(data["Time"].InnerText),
                                                RoleName = rm.RoleName,
                                                SchoolName = rm.SchoolName,
                                                UserName = rm.Account
                                            });
                                        }
                                        else if (rm.ResourceType == "2")
                                        {
                                            int _type = 0;
                                            switch (rm.EditionName)
                                            {
                                                case "英语一年级":
                                                    _type = 1;
                                                    break;
                                                case "英语二年级":
                                                    _type = 2;
                                                    break;
                                                case "英语三年级":
                                                    _type = 3;
                                                    break;
                                                case "英语四年级":
                                                    _type = 4;
                                                    break;
                                                case "英语五年级":
                                                    _type = 5;
                                                    break;
                                                case "英语六年级":
                                                    _type = 6;
                                                    break;
                                            }
                                            db.rp_resource_statist.Add(new rp_resource_statist()
                                            {
                                                Id = Guid.NewGuid().ToString(),
                                                Cd_key = id,
                                                EditionName = rm.EditionName,
                                                ResourceType = Convert.ToInt32(rm.ResourceType),
                                                Type = _type,
                                                CreateTime = Convert.ToDateTime(data["Time"].InnerText),
                                                RoleName = rm.RoleName,
                                                SchoolName = rm.SchoolName,
                                                UserName = rm.Account
                                            });
                                        }
                                        db.SaveChanges();
                                    }
                                }

                                File.Delete(file);//删除文件
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                WriteErrLog("资源平台定时任务出错：" + ex.ToString());
            }
        }

        #endregion


        protected override void OnStart(string[] args)
        {
        }

        protected override void OnStop()
        {
        }

        #region 记录错误日志
        /// <summary>
        /// 记录错误日志
        /// </summary>
        /// <param name="msg"></param>
        private void WriteErrLog(string msg)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "\\ErrLog.txt";//该日志文件会存在windows服务程序目录下
            FileInfo file = new FileInfo(path);
            if (!file.Exists)
            {
                FileStream fs;
                fs = File.Create(path);
                fs.Close();
            }

            using (FileStream fs = new FileStream(path, FileMode.Append, FileAccess.Write))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    sw.WriteLine(DateTime.Now.ToString() + "   " + msg);
                }
            }
        }
        #endregion

        #region 记录成功日志
        /// <summary>
        /// 记录成功日志
        /// </summary>
        /// <param name="msg"></param>
        private void WriteSucceedLog(string msg)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "\\SucceedLog.txt";//该日志文件会存在windows服务程序目录下
            FileInfo file = new FileInfo(path);
            if (!file.Exists)
            {
                FileStream fs;
                fs = File.Create(path);
                fs.Close();
            }

            using (FileStream fs = new FileStream(path, FileMode.Append, FileAccess.Write))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    sw.WriteLine(DateTime.Now.ToString() + "   " + msg);
                }
            }
        }
        #endregion

        #region 记录监控日志
        /// <summary>
        /// 记录监控日志
        /// </summary>
        private void WriteTempLog()
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "\\TempLog.txt";//该日志文件会存在windows服务程序目录下
            FileInfo file = new FileInfo(path);
            if (!file.Exists)
            {
                FileStream fs;
                fs = File.Create(path);
                fs.Close();
            }

            using (FileStream fs = new FileStream(path, FileMode.Append, FileAccess.Write))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    sw.WriteLine(DateTime.Now.ToString());
                }
            }
        }
        #endregion
    }
}
