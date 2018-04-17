using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace mdt.Common
{
    public class DESEncrypt
    {
        private const string _iv = "A$4^G@br";
        private const string _key = "Kings";
        private const string _fileKey = "KingsFEN";
        public const int FileEncryptStep = 2097152 * 20;//4M

        TripleDESCryptoServiceProvider _des = new TripleDESCryptoServiceProvider();
        private Encoding _encoding = Encoding.Unicode;

        /// <summary>
        /// 
        /// </summary>
        /// <value>The encoding mode.</value>
        public Encoding EncodingMode
        {
            get
            {
                return _encoding;
            }
            set
            {
                _encoding = value;
            }
        }

        /// <summary>
        /// Encrypt
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <returns></returns>
        public byte[] Encrypt(string str, string encryptyKey)
        {
            encryptyKey = encryptyKey.Replace("-", "");
            if (encryptyKey.Length < 19)
            {
                encryptyKey = encryptyKey.PadRight(19, '0');
            }
            string iv = _iv;
            string key = _key + encryptyKey.Substring(0, 19);
            if (String.IsNullOrEmpty(key))
                return null;
            var ivb = Encoding.ASCII.GetBytes(iv);
            var keyb = Encoding.ASCII.GetBytes(key);
            var tob = EncodingMode.GetBytes(str);
            byte[] encrypted;

            try
            {
                // Create a MemoryStream.
                MemoryStream mStream = new MemoryStream();

                TripleDESCryptoServiceProvider tdsp = new TripleDESCryptoServiceProvider();
                tdsp.Mode = CipherMode.CBC;             //默认值
                tdsp.Padding = PaddingMode.PKCS7;       //默认值

                // Create a CryptoStream using the MemoryStream 
                // and the passed key and initialization vector (IV).
                CryptoStream cStream = new CryptoStream(mStream,
                    _des.CreateEncryptor(keyb, ivb),
                    CryptoStreamMode.Write);

                // Write the byte array to the crypto stream and flush it.
                cStream.Write(tob, 0, tob.Length);
                cStream.FlushFinalBlock();

                // Get an array of bytes from the 
                // MemoryStream that holds the 
                // encrypted data.
                encrypted = mStream.ToArray();

                // Close the streams.
                cStream.Close();
                mStream.Close();

                return encrypted;
                // Return the encrypted buffer.
                //return EncodingMode.GetString(encrypted);
            }
            catch (CryptographicException e)
            {
                Console.WriteLine("A Cryptographic error occurred: {0}", e.Message);
                return null;
            }
        }

        /// <summary>
        /// Decript
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public string Decrypt(byte[] tob, string encryptyKey)
        {
            encryptyKey = encryptyKey.Replace("-", "");
            if (encryptyKey.Length < 19)
            {
                encryptyKey = encryptyKey.PadRight(19, '0');
            }
            string iv = _iv;
            string key = _key + encryptyKey.Substring(0, 19);

            if (String.IsNullOrEmpty(key))
                return null;

            var ivb = Encoding.ASCII.GetBytes(iv);
            var keyb = Encoding.ASCII.GetBytes(key);
            // var tob = EncodingMode.GetBytes(str);
            byte[] encrypted;
            try
            {
                // Create a new MemoryStream using the passed 
                // array of encrypted data.
                if (tob == null)
                    return null;
                MemoryStream msDecrypt = new MemoryStream();

                TripleDESCryptoServiceProvider tdsp = new TripleDESCryptoServiceProvider();
                tdsp.Mode = CipherMode.CBC;
                tdsp.Padding = PaddingMode.PKCS7;

                // Create a CryptoStream using the MemoryStream 
                // and the passed key and initialization vector (IV).
                CryptoStream csDecrypt = new CryptoStream(msDecrypt,
                    tdsp.CreateDecryptor(keyb, ivb),
                    CryptoStreamMode.Read);

                ///////////
                ICryptoTransform desdecrypt123 = tdsp.CreateDecryptor(keyb, ivb);
                MemoryStream writerS123 = new MemoryStream();
                CryptoStream cryptostreamDecr123 = new CryptoStream(writerS123, desdecrypt123, CryptoStreamMode.Write);
                cryptostreamDecr123.Write(tob, 0, tob.Length);
                cryptostreamDecr123.FlushFinalBlock();
                encrypted = writerS123.ToArray();
                /////////////

                //// Create buffer to hold the decrypted data.
                //encrypted = new byte[msDecrypt.Length];

                //// Read the decrypted data out of the crypto stream
                //// and place it into the temporary buffer.
                //csDecrypt.Read(encrypted, 0, encrypted.Length);
                //// csDecrypt.FlushFinalBlock();

                ////Convert the buffer into a string and return it.
                return EncodingMode.GetString(encrypted);
            }
            catch (CryptographicException e)
            {
                Console.WriteLine("A Cryptographic error occurred: {0}", e.Message);
                return null;
            }
        }


        /// <summary>
        /// 文件加密
        /// </summary>
        /// <param name="inputFilePath">明文输入文件路径</param>
        /// <param name="outputPath">密文输出文件路径(为空则替换明文输入文件)</param>
        /// <param name="message">消息</param>
        /// <returns>是否成功</returns>
        public static bool EncryptFile(string inputFilePath, string outputPath, ref string message)
        {
            FileStream fsInput = null;
            FileStream fsEncrypted = null;
            CryptoStream cryptostream = null;
            try
            {
                FileInfo sourceFile = new FileInfo(inputFilePath);
                if (!sourceFile.Exists)
                {
                    message = "对象文件不存在!";
                    return false;
                }
                if (string.IsNullOrEmpty(outputPath))
                {
                    outputPath = sourceFile.DirectoryName + "\\" + Guid.NewGuid().ToString() + sourceFile.Extension;
                    //return EncryptFile(inputFilePath, outputPath, true, ref message);
                }

                fsInput = new FileStream(inputFilePath, FileMode.Open, FileAccess.Read);
                fsEncrypted = new FileStream(outputPath, FileMode.Create, FileAccess.Write);
                DESCryptoServiceProvider DES = new DESCryptoServiceProvider();
                DES.Key = ASCIIEncoding.ASCII.GetBytes(_fileKey);
                DES.IV = ASCIIEncoding.ASCII.GetBytes(_iv);

                ICryptoTransform desencrypt = DES.CreateEncryptor();
                cryptostream = new CryptoStream(fsEncrypted, desencrypt, CryptoStreamMode.Write);

                byte[] bytearrayinput = new byte[FileEncryptStep];
                int count = 0;
                count = fsInput.Read(bytearrayinput, 0, FileEncryptStep);
                while (count > 0)
                {
                    cryptostream.Write(bytearrayinput, 0, count);
                    count = fsInput.Read(bytearrayinput, 0, FileEncryptStep);
                }
                cryptostream.FlushFinalBlock();
                cryptostream.Close();
                cryptostream = null;
                fsInput.Close();
                fsInput = null;
                fsEncrypted.Close();
                fsEncrypted = null;
                //替换原来的文件
                //File.Copy(outputPath, inputFilePath, true);
                //File.Delete(outputPath);
                message = "";
                return true;
            }
            catch (Exception e)
            {
                message = e.Message;
                return false;
            }
            finally
            {
                try
                {
                    if (cryptostream != null)
                    {
                        cryptostream.Close();
                        cryptostream = null;
                    }
                    if (fsInput != null)
                    {
                        fsInput.Close();
                        fsInput = null;
                    }
                    if (fsEncrypted != null)
                    {
                        fsEncrypted.Close();
                        fsEncrypted = null;
                    }
                }
                catch
                {
                }
            }
        }
        /// <summary>
        /// 文件加密
        /// </summary>
        /// <param name="inputFilePath">明文输入文件路径</param>
        /// <param name="outputPath">密文输出文件路径</param>
        /// <param name="replace">是否替换原文件</param>
        /// <param name="message">消息</param>
        /// <returns>是否成功</returns>
        public static bool EncryptFile(string inputFilePath, string outputPath, bool replace, ref string message)
        {
            try
            {
                bool result = EncryptFile(inputFilePath, outputPath, ref message);
                if (result)
                {
                    if (replace)
                    {
                        //替换原来的文件
                        File.Copy(outputPath, inputFilePath, true);
                        File.Delete(outputPath);
                    }
                    message = "";
                    return true;
                }
                else
                {
                    return result;
                }
            }
            catch (Exception e)
            {
                message = e.Message;
                return false;
            }
        }

        /// <summary>
        /// 文件解密
        /// </summary>
        /// <param name="inputFilePath">密文输入文件路径</param>
        /// <param name="outputPath">明文输出文件路径</param>
        /// <param name="message">消息</param>
        /// <returns>是否成功</returns>
        public static bool DecryptFile(string inputFilePath, string outputPath, ref string message)
        {
            try
            {

                FileStream fsread = new FileStream(inputFilePath, FileMode.Open, FileAccess.Read);
                File.Delete(outputPath);
                int count = 0;
                while (fsread.Position < fsread.Length)
                {
                    byte[] bytearrayinput = new byte[FileEncryptStep];
                    count = fsread.Read(bytearrayinput, 0, FileEncryptStep);

                    DESCryptoServiceProvider DES = new DESCryptoServiceProvider();
                    DES.Key = ASCIIEncoding.ASCII.GetBytes(_fileKey);
                    DES.IV = ASCIIEncoding.ASCII.GetBytes(_iv);
                    if (fsread.Position != fsread.Length)
                    {
                        DES.Padding = PaddingMode.None;
                    }
                    else
                    {
                        DES.Padding = PaddingMode.PKCS7;
                    }

                    ICryptoTransform desdecrypt = DES.CreateDecryptor();
                    FileStream writerS = new FileStream(outputPath, FileMode.Append);
                    UnicodeEncoding encoding = new UnicodeEncoding();
                    CryptoStream cryptostreamDecr = new CryptoStream(writerS, desdecrypt, CryptoStreamMode.Write);
                    cryptostreamDecr.Write(bytearrayinput, 0, count);
                    cryptostreamDecr.FlushFinalBlock();
                    writerS.Flush();
                    cryptostreamDecr.Close();
                    writerS.Close();
                }
                fsread.Close();
                message = "";
                return true;
            }
            catch (Exception e)
            {
                message = "失败！提示:" + e.Message;
                return false;
            }
        }

        /// <summary>
        /// 文件解密
        /// </summary>
        /// <param name="inputFilePath">密文输入文件路径</param>
        /// <param name="outputPath">明文输出文件路径</param>
        /// <param name="message">消息</param>
        /// <returns>是否成功</returns>
        public static bool DecryptFile(string inputFilePath, ref string message)
        {
            try
            {
                string outputPath = null;
                bool replace = false;
                FileInfo sourceFile = new FileInfo(inputFilePath);
                if (!sourceFile.Exists)
                {
                    message = "对象文件不存在!";
                    return false;
                }

                if (String.IsNullOrEmpty(outputPath) || inputFilePath.Trim().ToLower() == outputPath.Trim().ToLower())
                {
                    replace = true;
                    outputPath = sourceFile.DirectoryName + "\\" + Guid.NewGuid().ToString() + sourceFile.Extension;
                }
                bool result = DecryptFile(inputFilePath, outputPath, ref message);
                if (result)
                {
                    if (replace)
                    {
                        //替换原来的文件
                        File.Copy(outputPath, inputFilePath, true);
                        File.Delete(outputPath);
                    }
                    message = "";
                    return true;
                }
                else
                {
                    return result;
                }
            }
            catch (Exception e)
            {
                message = "失败！提示:" + e.Message;
                return false;
            }
        }


        /// <summary>
        /// 根据指定位置读取指定长度的加密文件解密后的二进制数组
        /// </summary>
        /// <param name="filePath">密文文件</param>
        /// <param name="startIndex">起始位置</param>
        /// <param name="length">长度</param>
        /// <param name="message">消息</param>
        /// <returns>解密后的二进制数组</returns>
        public static byte[] DecryptFile(string filePath, long startIndex, int length, ref string message)
        {
            try
            {
                DESCryptoServiceProvider DES = new DESCryptoServiceProvider();
                DES.Key = ASCIIEncoding.ASCII.GetBytes(_fileKey);
                DES.IV = ASCIIEncoding.ASCII.GetBytes(_iv);
                DES.Padding = PaddingMode.None;

                FileStream fsread = new FileStream(filePath, FileMode.Open, FileAccess.Read);

                ICryptoTransform desdecrypt = DES.CreateDecryptor();

                int pageIndex = (int)(startIndex / FileEncryptStep);
                int pageCount = (int)((length - 1) / FileEncryptStep) + 1;
                long arrayIndex = startIndex - pageIndex * FileEncryptStep;


                MemoryStream writerS = new MemoryStream();
                byte[] bytearrayinput = new byte[FileEncryptStep];
                CryptoStream cryptostreamDecr = new CryptoStream(writerS, desdecrypt, CryptoStreamMode.Write);
                fsread.Position = pageIndex * FileEncryptStep;

                int count = 0;
                for (int i = 0; i < pageCount; i++)
                {
                    count = fsread.Read(bytearrayinput, 0, FileEncryptStep);
                    if (count < FileEncryptStep)
                    {
                        //如果是文件尾，则做特殊处理
                        DES.Padding = PaddingMode.PKCS7;
                        ICryptoTransform desdecrypt123 = DES.CreateDecryptor();
                        MemoryStream writerS123 = new MemoryStream();
                        CryptoStream cryptostreamDecr123 = new CryptoStream(writerS123, desdecrypt123, CryptoStreamMode.Write);
                        cryptostreamDecr123.Write(bytearrayinput, 0, count);
                        cryptostreamDecr123.FlushFinalBlock();
                        byte[] endBytes = writerS123.ToArray();
                        writerS.Write(endBytes, 0, endBytes.Length);
                    }
                    else
                    {
                        cryptostreamDecr.Write(bytearrayinput, 0, count);
                    }
                }
                byte[] result;// = new byte[length];
                writerS.Position = arrayIndex;
                if (writerS.Length > arrayIndex + length)
                {
                    result = new byte[length];
                    writerS.Read(result, 0, length);
                }
                else
                {
                    length = (int)(writerS.Length - arrayIndex);
                    result = new byte[length];
                    writerS.Read(result, 0, length);
                }
                fsread.Close();
                writerS.Close();
                message = "";
                return result;
            }
            catch (Exception e)
            {
                message = "失败！提示:" + e.Message;
                return null;
            }
        }
    }
}
