using System;
using System.IO;
using System.Threading;

namespace Sample.Core
{
    /// <summary>
    /// 定数クラス
    /// </summary>
    public static class SampleConstants
    {
        /// <summary>
        /// 会社名(フォルダ名称に使う)
        /// </summary>
        public const string CompanyName = "会社名";

        /// <summary>
        /// メイン画面の切り替え領域名称
        /// </summary>
        public const string ChangeMainScreenAreaName = "ModeChangeArea";

        /// <summary>
        /// 設定情報のデフォルトファイル名称
        /// </summary>
        public const string DefaultSettingFileName = "Default";

        /// <summary>
        /// 改行コードCR(\r)
        /// </summary>
        public const string Cr = "\r";

        /// <summary>
        /// 改行コードLF(\n)
        /// </summary>
        public const string Lf = "\n";

        /// <summary>
        /// 改行文字(\r\n)
        /// </summary>
        public const string CrLf = Cr + Lf;

        /// <summary>
        /// CSVファイル読み書きエンコード
        /// </summary>
        public const string EncodingShiftJIS = "UTF_8";

        #region 拡張子系(Extension)
        /// <summary>
        /// CSVファイルの拡張子
        /// </summary>
        public const string FileExtensionCSV = ".csv";

        /// <summary>
        /// CSVファイルの全ファイル検索用
        /// </summary>
        public const string FileNameSearchCSV = "*" + FileExtensionCSV;

        /// <summary>
        /// JSONファイルの拡張子
        /// </summary>
        public const string FileExtensionJSON = ".json";

        /// <summary>
        /// JSONファイルの全ファイル検索用
        /// </summary>
        public const string FileNameSearchJSON = "*" + FileExtensionJSON;

        /// <summary>
        /// PDFファイルの拡張子
        /// </summary>
        public const string FileExtensionPDF = ".pdf";

        /// <summary>
        /// PDFファイルの全ファイル検索用
        /// </summary>
        public const string FileNameSearchPDF = "*" + FileExtensionPDF;

        /// <summary>
        /// ビットマップファイルの拡張子
        /// </summary>
        public const string FileExtensionBMP = ".bmp";

        /// <summary>
        /// Jpegファイルの拡張子
        /// </summary>
        public const string FileExtensionJPEG = ".jpg";
        #endregion 拡張子系(Extension)

        /// <summary>
        /// アプリケーション設定ファイル名称
        /// </summary>
        public static string ApplicationSettingFileName = "ApplicationSettings" + FileExtensionJSON;
        /// <summary>
        /// どのユーザでも共通で使用するフォルダ
        /// </summary>
        private static string _allUserFolder = null;
        /// <summary>
        /// どのユーザでも共通で使用するフォルダ
        /// </summary>
        public static string AllUserFolder
        {
            get
            {
                if (string.IsNullOrEmpty(_allUserFolder))
                {
                    string tmp = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);

                    _allUserFolder = Path.Join(tmp, $@"{CompanyName}\Sample");
                }

                return _allUserFolder;
            }
        }

        /// <summary>
        /// アプリケーション設定ファイルパスを取得します
        /// </summary>
        public static string ApplicationSettingFilePath
        {
            get
            {
                return Path.Join(AllUserFolder, ApplicationSettingFileName);
            }
        }

        /// <summary>
        /// データ出力先フォルダ
        /// </summary>
        public static string DataDefaultPath
        {
            get
            {
                return "./Data";
            }
        }

        /// <summary>
        /// <para>現在日時を文字列として取得します。</para>
        /// <para>yyyyMMddHHmmssfff</para>
        /// </summary>
        public static string NowDateFormatms
        {
            get
            {
                return $"{DateTime.Now:yyyyMMddHHmmssfff}";
            }
        }

        /// <summary>
        /// <para>現在日時を文字列として取得します。</para>
        /// <para>yyyyMMddHHmmss</para>
        /// </summary>
        public static string NowDateFormatSec
        {
            get
            {
                return $"{DateTime.Now:yyyyMMddHHmmss}";
            }
        }

        /// <summary>
        /// <para>現在日時を文字列として取得します。</para>
        /// <para>yyyyMMdd_HHmmss</para>
        /// </summary>
        public static string NowDateFormatSecSeparator
        {
            get
            {
                return $"{DateTime.Now:yyyyMMdd_HHmmss}";
            }
        }

        /// <summary>
        /// <para>現在日時を文字列として取得します。</para>
        /// <para>yyyyMMdd</para>
        /// </summary>
        public static string NowDateFormatDay
        {
            get
            {
                return $"{DateTime.Now:yyyyMMdd}";
            }
        }

        /// <summary>
        /// 実行可能スレッド数
        /// </summary>
        public static int threadCount = 0;
        /// <summary>
        /// <para>実行PCのCPUに合わせて実行スレッド数を取得します</para>
        /// <para>Parallelで使用する事を想定しています</para>
        /// <para>PCスレッド数 ÷ 2 (最低1、最大12)</para>
        /// </summary>
        /// <returns>スレッド数</returns>
        public static int GetThreadCount()
        {
            if (threadCount <= 0)
            {
                ThreadPool.GetMinThreads(out _, out int ioMin);
                threadCount = (int)Math.Ceiling((double)ioMin / 2);
                if (threadCount <= 0)
                {
                    threadCount = 1;
                }
                else if (12 < threadCount)
                {
                    threadCount = 12;
                }
            }

            return threadCount;
        }
    }
}
