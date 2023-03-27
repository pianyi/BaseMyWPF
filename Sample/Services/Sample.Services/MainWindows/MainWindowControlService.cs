using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using Sample.Core.Mvvm;
using System;
using System.Windows;

namespace Sample.Services.MainWindows
{
    /// <summary>
    /// メインウィンドウの制御を行います
    /// </summary>
    public class MainWindowControlService : DisposableBindableViewModelBase, IDisposable
    {
        /// <summary>
        /// メインウィンドウの最小幅
        /// </summary>
        private const int DefaultMinWidth = 300;

        /// <summary>
        /// メインウィンドウの最小高さ
        /// </summary>
        private const int DefaultMinHight = 100;

        /// <summary>
        /// メインウィンドウ操作可否
        /// </summary>
        public ReactiveProperty<bool> IsEnable { set; get; }

        /// <summary>
        /// メインウィンドウの幅
        /// </summary>
        public ReactiveProperty<double> Width { get; set; }

        /// <summary>
        /// メインウィンドウの最小幅
        /// </summary>
        public ReactiveProperty<double> MinWidth { get; set; }

        /// <summary>
        /// メインウィンドウの高さ
        /// </summary>
        public ReactiveProperty<double> Height { get; set; }

        /// <summary>
        /// メインウィンドウの最小高
        /// </summary>
        public ReactiveProperty<double> MinHeight { get; set; }

        /// <summary>
        /// メインウィンドウのステータス
        /// </summary>
        public ReactiveProperty<WindowState> State { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MainWindowControlService()
        {
            Width = new ReactiveProperty<double>(DefaultMinWidth).AddTo(Disposables);
            MinWidth = new ReactiveProperty<double>(DefaultMinWidth).AddTo(Disposables);
            Height = new ReactiveProperty<double>(DefaultMinHight).AddTo(Disposables);
            MinHeight = new ReactiveProperty<double>(DefaultMinHight).AddTo(Disposables);

            State = new ReactiveProperty<WindowState>(WindowState.Normal).AddTo(Disposables);
            IsEnable = new ReactiveProperty<bool>(true).AddTo(Disposables);
        }
    }
}
