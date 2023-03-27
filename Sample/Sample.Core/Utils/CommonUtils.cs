using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Sapmel.Core.Utils
{
    /// <summary>
    /// システム共通メソッド
    /// </summary>
    public static class CommonUtils
    {
        /// <summary>
        /// SHA512 ハッシュコード生成プロバイダ
        /// </summary>
        private static readonly SHA512 _hashProvider = SHA512.Create();

        /// <summary>
        /// SHA512 ハッシュコードを生成します
        /// </summary>
        /// <param name="value">ハッシュ元</param>
        /// <returns>ハッシュコード</returns>
        public static string GetSHAHashed(string value)
        {
            return string.Join("", _hashProvider.ComputeHash(Encoding.UTF8.GetBytes($"{value}")).Select(x => $"{x:X2}"));
        }

        /// <summary>
        /// 引数の文字列に、ファイル名に利用できない文字列があるかを確認します
        /// </summary>
        /// <param name="value">チェック文字列</param>
        /// <returns>true:エラー文字あり、false:エラー文字無し</returns>
        public static bool IsErrorFileName(string value)
        {
            var invalid = Path.GetInvalidFileNameChars();
            bool isError = false;

            if (0 <= value.IndexOfAny(invalid))
            {
                isError = true;
            }

            return isError;
        }
    }
}
