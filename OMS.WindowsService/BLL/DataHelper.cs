using BoxOms.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace OMS.WindowsService.BLL
{
    public class DataHelper
    {
        /// <summary>
        /// 根据激活码得到主键id
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string BackId(string key)
        {
            string id = string.Empty;
            using (var db = new box_omsEntities())
            {
                string data = Decrypt(key, "a%k8h5.o");
                if (data == null)
                    return id;

                string[] strs = data.Split('|');
                if (strs.Length > 2)
                {
                    id = strs[0];
                }

            }
            return id;
        }

        public static string Decrypt(string pToDecrypt, string sKey)
        {
            byte[] inputByteArray = Convert.FromBase64String(pToDecrypt);
            using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
            {
                des.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
                des.IV = ASCIIEncoding.ASCII.GetBytes(sKey);
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                using (CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(inputByteArray, 0, inputByteArray.Length);
                    cs.FlushFinalBlock();
                    cs.Close();
                }
                string str = Encoding.UTF8.GetString(ms.ToArray());
                ms.Close();
                return str;
            }
        }
    }
}
