using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Surging.Hero.Common.Utils
{
    public class EncryptHelper
    {
        /// <summary>
        /// 用MD5加密字符串，可选择生成16位或者32位的加密字符串
        /// </summary>
        /// <param name="line">待加密的字符串</param>
        /// <param name="bit">位数，一般取值16 或 32</param>
        /// <returns>返回的加密后的字符串</returns>
        public static string Md5(string line, int bit)
        {
            MD5 md5 = MD5.Create();//实例化一个md5对像
            byte[] hashedDataBytes = md5.ComputeHash(Encoding.UTF8.GetBytes(line));
            StringBuilder tmp = new StringBuilder();
            foreach (byte i in hashedDataBytes)
            {
                tmp.Append(i.ToString("x2"));
            }
            if (bit == 16)
                return tmp.ToString().Substring(8, 16);
            else if (bit == 32)

                return tmp.ToString(); //默认情况
            else return string.Empty;
        }

        /// <summary>
        /// 用MD5加密字符串
        /// </summary>
        /// <param name="line">待加密的字符串</param>
        /// <returns></returns>
        public static string Md5(string line, MD5Length length = MD5Length.L32)
        {

            string str_md5_out = string.Empty;
            using (MD5 md5 = MD5.Create())
            {
                byte[] bytes_md5_in = Encoding.UTF8.GetBytes(line);
                byte[] bytes_md5_out = md5.ComputeHash(bytes_md5_in);

                str_md5_out = length == MD5Length.L32
                    ? BitConverter.ToString(bytes_md5_out)
                    : BitConverter.ToString(bytes_md5_out, 4, 8);

                str_md5_out = str_md5_out.Replace("-", "");
                return str_md5_out.ToLower();
            }

        }
    }

    public enum MD5Length
    {
        L16,
        L32
    }
}
