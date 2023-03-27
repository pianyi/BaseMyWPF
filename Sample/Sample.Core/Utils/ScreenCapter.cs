using Sample.Core.Enums;
using System;
using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Sample.Core.Utils
{
    /// <summary>
    /// 画面キャプチャ
    /// </summary>
    public static class ScreenCapter
    {
        /// <summary>
        /// 画面をBMP形式で保存します
        /// </summary>
        /// <param name="filePath">保存ファイルパス</param>
        /// <param name="imageType">保存画像区分</param>
        public static void SaveMainWindow(string filePath, SaveImageType imageType)
        {
            // メイン画面の操作を行うため、メインスレッドにディスパッチする
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                var bmpRen = GetScreenImage(Application.Current.MainWindow);

                if (imageType == SaveImageType.Bitmap)
                {
                    SaveBitmap(bmpRen, filePath);
                }
                else
                {
                    SaveJpeg(bmpRen, filePath);
                }
            }));
        }

        /// <summary>
        /// 解析画面イメージ画像を取得します
        /// </summary>
        /// <typeparam name="T">取得するコントロール</typeparam>
        /// <param name="owner">検索対象のウィンドウオブジェクト</param>
        /// <param name="name">x:Name に指定されている値</param>
        /// <param name="filePath">保存ファイルパス</param>
        /// <param name="imageType">保存画像区分</param>
        public static void SaveImage<T>(Window owner, string name, string filePath, SaveImageType imageType) where T : DependencyObject
        {
            // メイン画面の操作を行うため、メインスレッドにディスパッチする
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                var bmpRen = GetControlImage<T>(owner, name);

                if (imageType == SaveImageType.Bitmap)
                {
                    SaveBitmap(bmpRen, filePath);
                }
                else
                {
                    SaveJpeg(bmpRen, filePath);
                }
            }));
        }

        /// <summary>
        /// メインウィンドウから指定コントロールの画像を取得します
        /// </summary>
        /// <typeparam name="T">取得するコントロール</typeparam>
        /// <param name="controlName">コントロール名(x:Nameで指定された名前)</param>
        /// <returns>イメージオブジェクト(取得できない場合はnull)</returns>
        public static RenderTargetBitmap GetControlImage<T>(string controlName) where T : DependencyObject
        {
            RenderTargetBitmap result = null;

            // メイン画面の操作を行うため、メインスレッドにディスパッチする
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                result = GetControlImage<T>(Application.Current.MainWindow, controlName);
            }));

            return result;
        }

        /// <summary>
        /// 指定のウィンドウから指定コントロールの画像を取得します
        /// </summary>
        /// <typeparam name="T">取得するコントロール</typeparam>
        /// <param name="controlName">コントロール名(x:Nameで指定された名前)</param>
        /// <returns>イメージオブジェクト(取得できない場合はnull)</returns>
        public static RenderTargetBitmap GetControlImage<T>(Window orner, string controlName) where T : DependencyObject
        {
            RenderTargetBitmap result = null;

            // メイン画面の操作を行うため、メインスレッドにディスパッチする
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                result = GetScreenImage(FindChild<T>(orner, controlName));
            }));

            return result;
        }

        /// <summary>
        /// 座標を指定して、指定の方式で保存します
        /// </summary>
        /// <param name="ctrl">キャプチャ元のコントロール(取得できない場合はnull)</param>
        private static RenderTargetBitmap GetScreenImage(DependencyObject obj)
        {
            try
            {
                if (obj is Control ctrl)
                {
                    var bounds = VisualTreeHelper.GetDescendantBounds(ctrl);
                    var bitmap = new RenderTargetBitmap((int)bounds.Width,
                                                        (int)bounds.Height,
                                                        0,
                                                        0,
                                                        PixelFormats.Pbgra32);
                    var dv = new DrawingVisual();
                    using (var dc = dv.RenderOpen())
                    {
                        var vb = new VisualBrush(ctrl);
                        dc.DrawRectangle(vb, null, bounds);
                    }
                    bitmap.Render(dv);

                    return bitmap;
                }
            }
            catch (Exception)
            {
            }

            return null;
        }

        /// <summary>
        /// Finds a Child of a given item in the visual tree. 
        /// </summary>
        /// <param name="parent">A direct parent of the queried item.</param>
        /// <typeparam name="T">The type of the queried item.</typeparam>
        /// <param name="childName">x:Name or Name of child. </param>
        /// <returns>The first parent item that matches the submitted type parameter. 
        /// If not matching item can be found, 
        /// a null parent is being returned.</returns>
        private static T FindChild<T>(DependencyObject parent, string childName)
           where T : DependencyObject
        {
            // Confirm parent and childName are valid. 
            if (parent == null)
            {
                return null;
            }

            T foundChild = null;

            int childrenCount = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < childrenCount; i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);

                // If the child is not of the request child type child
                if (child is not T)
                {
                    // recursively drill down the tree
                    foundChild = FindChild<T>(child, childName);

                    // If the child is found, break so we do not overwrite the found child. 
                    if (foundChild != null)
                    {
                        break;
                    }
                }
                else if (!string.IsNullOrEmpty(childName))
                {
                    // If the child's name is set for search
                    if (child is FrameworkElement frameworkElement && frameworkElement.Name == childName)
                    {
                        // if the child's name is of the request name
                        foundChild = (T)child;
                        break;
                    }
                }
                else
                {
                    // child element found.
                    foundChild = (T)child;
                    break;
                }
            }

            return foundChild;
        }

        /// <summary>
        /// 画像をBitmap形式で保存します
        /// </summary>
        /// <param name="bmpRen">イメージ情報</param>
        /// <param name="filePath">保存先</param>
        private static void SaveBitmap(RenderTargetBitmap bmpRen, string filePath)
        {
            if (bmpRen != null && !string.IsNullOrEmpty(filePath))
            {
                // 1度メモリ内でBitmapに変換
                using MemoryStream stream = new();
                BitmapEncoder encoder = new BmpBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(bmpRen));
                encoder.Save(stream);

                // 保存
                Bitmap bitmap = new(stream);
                bitmap.Save(filePath);
            }
        }

        /// <summary>
        /// 画像をJpeg形式で保存します
        /// </summary>
        /// <param name="bmpRen">イメージ情報</param>
        /// <param name="filePath">保存先</param>
        private static void SaveJpeg(RenderTargetBitmap bmpRen, string filePath)
        {
            if (bmpRen != null && !string.IsNullOrEmpty(filePath))
            {
                // Jpegにそのまま保存
                JpegBitmapEncoder encoder = new();
                encoder.Frames.Add(BitmapFrame.Create(bmpRen));
                using Stream stm = File.Create(filePath);
                encoder.Save(stm);
            }
        }
    }
}
