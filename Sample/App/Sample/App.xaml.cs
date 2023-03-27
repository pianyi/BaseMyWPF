using NLog;
using NLog.Config;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using Prism.Unity;
using PrismExpansion.Services.Dialogs;
using Sample.Core;
using Sample.Core.Enums;
using Sample.MainWindow;
using Sample.NormalPage;
using Sample.Services.MainWindows;
using Sample.Services.NormalPages;
using System;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using Unity;

namespace Sample
{
    /// <summary>
    /// アプリケーションの起動・終了等の処理
    /// </summary>
    public partial class App : PrismApplication
    {
        /// <summary>
        /// ログ出力設定ファイルのパス
        /// </summary>
        private const string _nLogConfigFile = "./nlog.config";

        /// <summary>
        /// ログ出力クラス
        /// </summary>
        private readonly Logger _logger;

        /// <summary>
        /// 二重起動防止用
        /// </summary>
        private Mutex _mutex;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public App()
        {
            InitializeComponent();

            // ログ情報変更
            SetLogConfig();
            _logger = LogManager.GetCurrentClassLogger();

            // 2重起動チェック
            if (CanStarted())
            {
                // SHIFT-JISを読み込み可能にする
                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

                // 言語に合わせてリソースの更新
                UpdateResources();
            }
        }

        /// <summary>
        /// ログ出力情報変更
        /// </summary>
        private void SetLogConfig()
        {
            // ログ情報の指定
            if (File.Exists(_nLogConfigFile))
            {
                // ログファイルが存在する場合は指定する
                LogManager.Configuration = new XmlLoggingConfiguration(_nLogConfigFile);
            }

            var logger = LogManager.GetLogger("LogFile");
            logger.Factory.Configuration.Variables.Add("CompanyName", SampleConstants.CompanyName);
            logger.Factory.Configuration.Variables.Add("AssemblyName", Assembly.GetExecutingAssembly().GetName().Name);
            logger.Factory.ReconfigExistingLoggers();
        }

        /// <summary>
        /// 起動可能かどうかを確認します
        /// </summary>
        /// <returns>true:起動OK、false：起動済みなので終了します</returns>
        private bool CanStarted()
        {
            try
            {
                // 二重起動チェック(アセンブリ情報が取れないので、作成したGUIDを指定)
                _mutex = new Mutex(false, "{b5aacf47-993b-4db8-8267-547f348ff09b}");
                if (!_mutex.WaitOne(0, false))
                {
                    _mutex.Close();
                    _mutex = null;
                    Shutdown();
                    _logger.Warn("既に起動されています。");
                    return false;
                }
            }
            catch
            {
                _mutex.Close();
                _mutex = null;
                Shutdown();
                _logger.Warn("既に起動されています。");
                return false;
            }

            return true;
        }

        /// <summary>
        /// 使用言語に合わせてリソースを変更します
        /// </summary>
        private void UpdateResources()
        {
            try
            {
                // OSの言語に合わせてメッセージファイルを変更
                string uiCulture = CultureInfo.CurrentCulture.Name;

                _logger.Info($"読込言語：{uiCulture}");
                var split = uiCulture.Split("-");
                ResourceDictionary dic = new()
                {
                    Source = new Uri($"pack://application:,,,/Sample.Resources;component/Resources/Message/Message.{split[0]}.xaml", UriKind.Absolute)
                };
                // App.xaml 記載の言語(英語)を基準とし、各国の言語を上書きする(指定があるもののみ)
                Resources.MergedDictionaries.Add(dic);
            }
            catch (Exception ex)
            {
                _logger.Warn(ex, "言語ファイルが存在しません。デフォルト言語(英語)を使用します。");
            }
        }

        /// <summary>
        /// 実行初期に動作します。
        /// </summary>
        /// <param name="sender">イベントオブジェクト</param>
        /// <param name="e">イベント引数</param>
        protected override void OnStartup(StartupEventArgs e)
        {
            _logger.Info("システム起動");

            // バージョン情報の指定
            Resources["COM-00-000"] = string.Format(Resources["COM-00-000"].ToString(),
                                                  Assembly.GetExecutingAssembly().GetName().Version.Major,
                                                  Assembly.GetExecutingAssembly().GetName().Version.Minor,
                                                  Assembly.GetExecutingAssembly().GetName().Version.Build,
                                                  Assembly.GetExecutingAssembly().GetName().Version.Revision);

            base.OnStartup(e);
        }

        /// <summary>
        /// 未処理の例外処理を処理します。
        /// </summary>
        /// <param name="sender">イベントオブジェクト</param>
        /// <param name="e">イベント引数</param>
        private void ApplicationDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            _logger.Error(e.Exception, "処理されないシステム例外発生のため、強制終了されました。");

            string title = "Error";
            string message = "A system error has occurred. Exit the application.";
            try
            {
                message = Resources["COM-00-M004-00"].ToString();
            }
            catch
            {
                // どうしようもないので例外は無視
            }
            try
            {
                title = Resources["COM-00-S111-00"].ToString();
            }
            catch
            {
                // どうしようもないので例外は無視
            }

            MessageBox.Show(message,
                            title,
                            MessageBoxButton.OK,
                            MessageBoxImage.Error,
                            MessageBoxResult.OK,
                            MessageBoxOptions.DefaultDesktopOnly);

            Shutdown();
        }

        /// <summary>
        /// システム終了
        /// </summary>
        /// <param name="sender">イベントオブジェクト</param>
        /// <param name="e">イベント引数</param>
        private void ApplicationExit(object sender, ExitEventArgs e)
        {
            _logger.Info("システム停止");

            if (_mutex != null)
            {
                _mutex.ReleaseMutex();
                _mutex.Close();
                _mutex = null;
            }
        }

        /// <summary>
        /// モジュールクラスを初期化します
        /// </summary>
        /// <param name="moduleCatalog">モジュール制御クラス</param>
        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            moduleCatalog.AddModule<NormalPageModule>();
            moduleCatalog.AddModule<MainWindowModule>();
        }

        /// <summary>
        /// インスタンスのDI登録(インスタンスの生成を任せる)
        /// </summary>
        /// <param name="containerRegistry">インスタンス登録クラス</param>
        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            // 複数画面で共通利用されるサービス(モデル)はシングルトンで作成するらしい
            // 本来はインターフェースを作成すること
            containerRegistry.RegisterSingleton<MainWindowControlService>();
            containerRegistry.RegisterSingleton<NormalPageControlService>();

            // ダイアログ制御
            containerRegistry.RegisterDialog<Dialog>(nameof(DialogType.ModelessFirst));
            containerRegistry.RegisterDialog<Dialog>(nameof(DialogType.ModelessSecond));

            // ダイアログ表示クラスをカスタマイズ(基本は元のコピー)
            containerRegistry.RegisterSingleton<IDialogService, CustomizeDialogService>();
        }

        /// <summary>
        /// アプリケーション起動時の初期ウィンドウ
        /// </summary>
        /// <returns>初期ウィンドウ</returns>
        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow.Views.MainWindow>();
        }

        /// <summary>
        /// ViewとViewModelの関連付けを行います
        /// </summary>
        protected override void ConfigureViewModelLocator()
        {
            base.ConfigureViewModelLocator();

            // 紐づけのカスタマイズ
            ViewModelLocationProvider.SetDefaultViewTypeToViewModelTypeResolver(GetViewModelName);

            // /Views/XXView と /ViewModels/XXViewModel は勝手に紐づく
            // ViewとViewModel の関連付け
            // ルールから外れる場合はここで強制的に紐づける
            //ViewModelLocationProvider.Register<MainWindow, MainWindowViewModel>();
        }

        /// <summary>
        /// ルートディレクトリ以外でも、ViewsとViewModels の自動紐づけを行うようにカスタマイズする
        /// </summary>
        /// <param name="viewType">View側のデータ</param>
        /// <returns>ViewModelのタイプ</returns>
        private Type GetViewModelName(Type viewType)
        {
            // Viewに対応するViewModelの名前を生成
            // viewType.FullNameで取得されるのは名前空間も含めた完全名.
            // その名前空間の｢Views｣の部分を｢ViewModels｣に置換している.
            var viewModelName = viewType.FullName.Replace("Views", "ViewModels") + "ViewModel";
            var viewAssemblyName = viewType.GetTypeInfo().Assembly.FullName;
            return Type.GetType($"{viewModelName}, {viewAssemblyName}");
        }
    }
}
