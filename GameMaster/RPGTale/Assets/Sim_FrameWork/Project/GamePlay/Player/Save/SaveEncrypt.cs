using System;
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
        /// <summary>
        /// Encrypt Data
        /// </summary>
        /// <param name="str"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string Encrypt(string str, string key)
        {
            ///key
            byte[] keyArray = UTF8Encoding.UTF8.GetBytes(key);

            ///Data
            byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(str);

            RijndaelManaged ri = new RijndaelManaged();
            ri.Key = keyArray;
            ri.Mode = CipherMode.ECB;
            ri.Padding = PaddingMode.PKCS7;
            ICryptoTransform cTrans = ri.CreateEncryptor();

            byte[] result = cTrans.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            return Convert.ToBase64String(result, 0, result.Length);
        }


        public static string Decrypt(string str,string key)
        {
            ///key
            byte[] keyArray = UTF8Encoding.UTF8.GetBytes(key);

            ///Data
            byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(str);

            RijndaelManaged rdel = new RijndaelManaged();
            rdel.Key = keyArray;
            rdel.Mode = CipherMode.ECB;
            rdel.Padding = PaddingMode.PKCS7;
            ICryptoTransform cTrans = rdel.CreateDecryptor();
            try
            {
                byte[] result = cTrans.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
                return UTF8Encoding.UTF8.GetString(result);
            }
            catch(Exception e)
            {
                DebugPlus.LogError(e);
                DebugPlus.LogError("[GameSaveData] : Decryptor ERROR!");
                return string.Empty;
            }
            
        }


    }
}