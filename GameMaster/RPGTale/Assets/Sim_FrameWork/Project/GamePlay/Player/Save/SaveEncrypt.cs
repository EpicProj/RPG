using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
/*
* 存档加密
* 
*/
namespace Sim_FrameWork
{
    public class SaveEncrypt
    {
        private static string sKEY = "ZTdkNTNmNDE2NTM3MWM0NDFhNTEzNzU1";
        private static string sIV = "4rZymEMfa/PpeJ89qY4gyA==";

        /// <summary>
        /// Encrypt Data
        /// </summary>
        /// <param name="str"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string Encrypt(string str)
        {
            RijndaelManaged rijndaelManaged = new RijndaelManaged
            {
                Padding = PaddingMode.Zeros,
                Mode = CipherMode.CBC,
                KeySize = 128,
                BlockSize = 128
            };

            byte[] bytes = Encoding.UTF8.GetBytes(sKEY);
            byte[] rgbIV = Convert.FromBase64String(sIV);

            ICryptoTransform transform = rijndaelManaged.CreateEncryptor(bytes, rgbIV);

            MemoryStream memoryStream = new MemoryStream();
            CryptoStream cryptoStream = new CryptoStream(memoryStream, transform, CryptoStreamMode.Write);

            byte[] bytes2 = Encoding.UTF8.GetBytes(str);
            cryptoStream.Write(bytes2, 0, bytes2.Length);
            cryptoStream.FlushFinalBlock();
            byte[] inArray = memoryStream.ToArray();

            return Convert.ToBase64String(inArray);
        }


        public static string Decrypt(string str)
        {
            RijndaelManaged rijndaelManaged = new RijndaelManaged
            {
                Padding = PaddingMode.Zeros,
                Mode = CipherMode.CBC,
                KeySize = 128,
                BlockSize = 128
            };
            byte[] bytes = Encoding.UTF8.GetBytes(sKEY);
            byte[] rgbIV = Convert.FromBase64String(sIV);

            ICryptoTransform transform = rijndaelManaged.CreateDecryptor(bytes, rgbIV);

            byte[] array = Convert.FromBase64String(str);
            byte[] array2 = new byte[array.Length];
            MemoryStream stream = new MemoryStream(array);
            CryptoStream cryptoStream = new CryptoStream(stream, transform, CryptoStreamMode.Read);
            cryptoStream.Read(array2, 0, array2.Length);

            return Encoding.UTF8.GetString(array2).TrimEnd(new char[1]);

        }


    }
}