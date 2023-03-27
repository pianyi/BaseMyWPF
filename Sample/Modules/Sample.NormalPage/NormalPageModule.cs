using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using Sample.Core;
using System;
using System.Windows;

namespace Sample.NormalPage
{
    /// <summary>
    /// ノーマルモード用の画面初期化処理
    /// </summary>
    public class NormalPageModule : IModule
    {
        /// <summary>
        /// DIコントローラ
        /// </summary>
        private readonly IRegionManager _regionManager;

        /// <summary>
        /// アプリケーションデータ
        /// </summary>
        /// <summary>
        public NormalPageModule(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }

        /// <summary>
        /// 初期化処理
        /// </summary>
        /// <param name="containerProvider"></param>
        public void OnInitialized(IContainerProvider containerProvider)
        {
            // メイン画面がノーマルモードの身になったので有効にする
            _regionManager.RequestNavigate(SampleConstants.ChangeMainScreenAreaName, nameof(Views.NormalPageMain));
        }

        /// <summary>
        /// データ登録処理
        /// </summary>
        /// <param name="containerRegistry"></param>
        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<Views.NormalPageMain>();

            ResourceDictionary dic = new()
            {
                Source = new Uri(@$"pack://application:,,,/Sample.NormalPage;component/Resources/Style/SampleStyle.xaml", UriKind.Absolute)
            };

            // App.xaml 記載の言語(英語)を基準とし、各国の言語を上書きする(指定があるもののみ)
            Application.Current.Resources.MergedDictionaries.Add(dic);
        }
    }
}