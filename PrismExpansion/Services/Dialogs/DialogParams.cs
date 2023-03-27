namespace Sample.ViewModels.Common.Enum
{
    /// <summary>
    /// ダイアログ画面へのパラメータ
    /// </summary>
    public static class DialogParams
    {
        /// <summary>
        /// ダイアログを指定するキー
        /// </summary>
        public const string Key = "Key";
        /// <summary>
        /// タイトルメッセージ
        /// </summary>
        public const string Title = "Title";
        /// <summary>
        /// メインメッセージ
        /// </summary>
        public const string Message = "Message";

        /// <summary>
        /// <para>初期ウィンドウの幅を指定します</para>
        /// <para>SizeToContentがManual指定時に動作します</para>
        /// </summary>
        public const string Width = "Width";
        /// <summary>
        /// <para>初期ウィンドウの高さを指定します</para>
        /// <para>SizeToContentがManual指定時に動作します</para>
        /// </summary>
        public const string Height = "Height";
        /// <summary>
        /// <para>初期ウィンドウの左の位置を指定します</para>
        /// <para>prism:Dialog.WindowStartupLocationがManual指定時に動作します</para>
        /// </summary>
        public const string Left = "Left";
        /// <summary>
        /// <para>初期ウィンドウの上の位置を指定します</para>
        /// <para>prism:Dialog.WindowStartupLocationがManual指定時に動作します</para>
        /// </summary>
        public const string Top = "Top";

        /// <summary>
        /// フォルダパス
        /// </summary>
        public const string FolderPath = "FolderPath";

        /// <summary>
        /// 設定データ変更フラグ
        /// </summary>
        public const string IsSettingChange = "IsttingChange";

        /// <summary>
        /// 表示するウィンドウのオーナーウィンドウの指定を拒否します
        /// </summary>
        public const string UnsetOwner = "UnsetOwner";

        /// <summary>
        /// はいボタン表示
        /// </summary>
        public const string IsShowYes = "IsShowYes";
        /// <summary>
        /// いいえボタン表示
        /// </summary>
        public const string IsShowNo = "IsShowNo";
        /// <summary>
        /// OKボタン表示
        /// </summary>
        public const string IsShowOk = "IsShowOk";
        /// <summary>
        /// キャンセルボタン表示
        /// </summary>
        public const string IsShowCancel = "IsShowCancel";
    }
}
