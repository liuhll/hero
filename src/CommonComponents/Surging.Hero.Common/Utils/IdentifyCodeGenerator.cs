using System;
using System.Text;

namespace Surging.Hero.Common.Utils
{
    public static class IdentifyCodeGenerator
    {
        private const string numLine = "0123456789";

        private const string charStr = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";

        public static string Create(int length, IdentifyCodeType identifyCodeType = IdentifyCodeType.Number)
        {
            var line = string.Empty;
            switch (identifyCodeType)
            {
                case IdentifyCodeType.Letter:
                    line = charStr;
                    break;
                case IdentifyCodeType.Number:
                    line = numLine;
                    break;
                case IdentifyCodeType.MixNumberLetter:
                    line = charStr + numLine;
                    break;
            }
            var lineArray = line.ToCharArray();
            var sb = new StringBuilder();
            var random = new Random(unchecked((int) DateTime.Now.Ticks));
            for (var i = 0; i < length; i++)
            {
                sb.Append(lineArray[random.Next(lineArray.Length)]);
            }

            return sb.ToString();
        }
    }
}