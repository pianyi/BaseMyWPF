using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace Sample.MainWindow
{
    public class MainWindowModule : IModule
    {
        /// <summary>
        /// DIコントローラ
        /// </summary>
        private readonly IRegionManager _regionManager;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="regionManager"></param>
        public MainWindowModule(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }

        /// <summary>
        /// 初期化処理
        /// </summary>
        /// <param name="containerProvider"></param>
        public void OnInitialized(IContainerProvider containerProvider)
        {
        }

        /// <summary>
        /// データ登録処理
        /// </summary>
        /// <param name="containerRegistry"></param>
        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterDialog<Views.MainWindow>();
        }
    }
}
