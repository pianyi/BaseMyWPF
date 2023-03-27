using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;
using System.IO;
using System.Windows;

namespace Sample.Core.Utils
{
    /// <summary>
    /// ファイル、フォルダ選択処理用クラス
    /// </summary>
    public static class FileOpenProcess
    {
        /// <summary>
        /// 出力フォルダ選択
        /// </summary>
        /// <param name="DirectoryPath">初期参照先</param>
        /// <param name="FileName">出力ファイル</param>
        /// <returns>選択フォルダパス(キャンセルボタン押下時はnull)</returns>
        public static string GetDirectoryPath(string DirectoryPath, string FileName = "")
        {
            using var ofdlg = new CommonOpenFileDialog();

            // 指定先がフォルダの場合
            if (!string.IsNullOrEmpty(DirectoryPath) && File.GetAttributes(DirectoryPath).HasFlag(FileAttributes.Directory))
            {
                ofdlg.InitialDirectory = DirectoryPath;
            }
            // 指定先がファイルの場合
            else
            {
                ofdlg.InitialDirectory = Path.GetDirectoryName(DirectoryPath);
            }

            ofdlg.Title = $"{FileName} {Application.Current.Resources["COM-00-S008-00"]}";

            ofdlg.IsFolderPicker = true;

            if (ofdlg.ShowDialog() == CommonFileDialogResult.Ok)
            {
                return ofdlg.FileName;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// ファイル選択
        /// </summary>
        /// <param name="FilePath">初期参照先</param>
        /// <param name="FileName">選択ファイル</param>
        /// <returns>選択ファイルパス(キャンセルボタン押下時はnull)</returns>
        public static string GetFilePath(string FilePath, string FileName = "")
        {
            var dialog = new OpenFileDialog();

            // 指定先がフォルダの場合
            if (!string.IsNullOrEmpty(FilePath) && File.GetAttributes(FilePath).HasFlag(FileAttributes.Directory))
            {
                dialog.InitialDirectory = FilePath;
            }
            // 指定先がファイルの場合
            else
            {
                dialog.InitialDirectory = Path.GetDirectoryName(FilePath);
            }

            dialog.Title = $"{FileName} {Application.Current.Resources["COM-00-S009-00"]}";

            dialog.Filter = $"{Application.Current.Resources["COM-00-S010-00"]}({SampleConstants.FileNameSearchCSV})|{SampleConstants.FileNameSearchCSV}";

            if (dialog.ShowDialog() == true)
            {
                return dialog.FileName;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// ファイル保存
        /// </summary>
        /// <param name="FilePath">初期参照先</param>
        /// <param name="FileName">選択ファイル</param>
        /// <returns>選択ファイルパス(キャンセルボタン押下時はnull)</returns>
        public static string GetSaveFilePath(string FilePath, string FileName = "")
        {
            var dialog = new SaveFileDialog();

            // 指定先がフォルダの場合
            if (!string.IsNullOrEmpty(FilePath) && File.GetAttributes(FilePath).HasFlag(FileAttributes.Directory))
            {
                dialog.InitialDirectory = FilePath;
            }
            // 指定先がファイルの場合
            else
            {
                dialog.InitialDirectory = Path.GetDirectoryName(FilePath);
            }

            dialog.Title = $"{Application.Current.Resources["COM-00-S011-00"]}";

            dialog.Filter = $"{Application.Current.Resources["COM-00-S010-00"]}({SampleConstants.FileNameSearchCSV})|{SampleConstants.FileNameSearchCSV}";

            dialog.FileName = FileName;

            if (dialog.ShowDialog() == true)
            {
                return dialog.FileName;
            }
            else
            {
                return null;
            }
        }
    }
}
