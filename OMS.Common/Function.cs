using System;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace OMS.Common
{
    public static class Function
    {
        /// <summary>
        /// 获取访问者IP
        /// </summary>
        /// <returns></returns>
        public static string GetIp(HttpRequestBase Request)
        {
            string ip = "";

            if (Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != "")// 如果使用代理，获取真实IP  
            {
                ip = Request.ServerVariables["REMOTE_ADDR"];
            }
            else
            {
                ip = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            }

            if (ip == null || ip == "")
            {
                ip = Request.UserHostAddress;
            }

            return ip;
        }

        /// <summary>
        /// 给一个字符串进行MD5加密
        /// </summary>
        /// <param name="strText">待加密字符串</param>
        /// <returns>加密后的字符串</returns>
        public static string MD5Encrypt(string strText)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] result = md5.ComputeHash(System.Text.Encoding.Default.GetBytes(strText));
            return System.Text.Encoding.Default.GetString(result);
        }

        /// <summary>
        /// 格式化转换日期时间到字符串
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string ConvertDateTime(DateTime? dt)
        {
            return dt == null ? "" : ((DateTime)dt).ToString("yyyy-MM-dd HH:mm:ss");
        }

        /// <summary>
        /// 格式化转换日期到字符串
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string ConvertDate(DateTime? dt)
        {
            return dt == null ? "" : ((DateTime)dt).ToString("yyyy-MM-dd");
        }

        /// <summary>
        /// 获取文件MD5值
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string GetMD5HashFromFile(string fileName)
        {
            try
            {
                FileStream file = new FileStream(fileName, FileMode.Open);
                MD5 md5 = new MD5CryptoServiceProvider();
                byte[] retVal = md5.ComputeHash(file);
                file.Close();

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < retVal.Length; i++)
                {
                    sb.Append(retVal[i].ToString("x2"));
                }
                return sb.ToString();
            }
            catch (Exception ex)
            {
                throw new Exception("GetMD5HashFromFile() fail,error:" + ex.Message);
            }
        }
    }
}
