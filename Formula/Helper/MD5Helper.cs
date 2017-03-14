using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Formula.Helper
{
    public static class MD5Helper
    {
        private static object obj = new object();

        /// <summary>
        /// 获取文件的MD5值
        /// </summary>
        /// <param name="FilePath"></param>
        /// <returns></returns>
        public static string GetFileMD5String(string FilePath)
        {
            string strResult = "";
            string strHashData = "";
            byte[] arrbytHashValue;
            System.IO.FileStream oFileStream = null;
            System.Security.Cryptography.MD5CryptoServiceProvider oMD5Hasher =new System.Security.Cryptography.MD5CryptoServiceProvider();
            try
            {
                oFileStream = new System.IO.FileStream(FilePath, System.IO.FileMode.Open,
                      System.IO.FileAccess.Read, System.IO.FileShare.ReadWrite);
                arrbytHashValue = oMD5Hasher.ComputeHash(oFileStream);//计算指定Stream 对象的哈希值
                oFileStream.Close();
                //由以连字符分隔的十六进制对构成的String，其中每一对表示value 中对应的元素；例如“F-2C-4A”
                strHashData = System.BitConverter.ToString(arrbytHashValue);
                //替换-
                strHashData = strHashData.Replace("-", "");
                strResult = strHashData;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return strResult;
        }


       
        // a 32 character hexadecimal string.
        public static string getMd5Hash(string input)
        {
            // Create a new instance of the MD5CryptoServiceProvider object.
            lock (obj)
            {
                MD5 md5Hasher = MD5.Create();

                // Convert the input string to a byte array and compute the hash.
                byte[] data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(input));

                // Create a new Stringbuilder to collect the bytes
                // and create a string.
                StringBuilder sBuilder = new StringBuilder();

                // Loop through each byte of the hashed data 
                // and format each one as a hexadecimal string.
                for (int i = 0; i < data.Length; i++)
                {
                    sBuilder.Append(data[i].ToString("x2"));
                }

                // Return the hexadecimal string.
                return sBuilder.ToString();
            }
        }
    }
}
