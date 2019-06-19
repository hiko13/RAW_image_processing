using System;
using System.IO;
//using System.Timers;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Input;
using System.ComponentModel;
using System.Collections.Generic;
using System.Threading;
using System.Drawing;
using System.Diagnostics;
//using System.Drawing.Imaging;



namespace image_processing
{
    public partial class MainWindow : Window
    {
        WriteableBitmap original;
        WriteableBitmap processed;

        ushort[] color;
        ushort[] color2;
        ushort[] color_small;
        ushort[] color2_small;
        //ushort[] color_resized;

        public static double Offset;
        public static double R_Offset;
        public static double G_Offset;
        public static double B_Offset;
        public static double Gain;
        public static double R_Gain;
        public static double G_Gain;
        public static double B_Gain;
        public static double gamma;

        public Array original_data;

        public basic_process basic_process;

        public static int width, height;
        public static int dwnscale = 1;


        public MainWindow()
        {
            InitializeComponent();
        }

        //// MainWindow の Closed 時などに、先に生成した Windwow をクローズします。
        //private void Window_Closed(object sender, EventArgs e)
        //{
        //    App app = (App)App.Current;
        //    app.m_wnd.Close();
        //}

        private void event_DragEnter(object sender, DragEventArgs e)
        {
            e.Effects = DragDropEffects.Copy; //change the cursor when dragging
        }

        private async void event_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) // confirm it's file or not
            {

                //Title_Border.Visibility = Visibility.Visible;
                //Dragdrop.Visibility = Visibility.Hidden;

                //ProgressBar_wait.IsIndeterminate = true;
                //ProgressBar_wait.Visibility = Visibility.Visible;

                await Task.Run(() =>
                {

                    string filename = ((string[])e.Data.GetData(DataFormats.FileDrop))[0]; // get the file name of dragged file

                    BitmapImage bitmap = new BitmapImage(); // create the instance of the decoded bitmap image
                    bitmap.BeginInit();
                    FileStream stream = File.OpenRead(filename);
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.CreateOptions = BitmapCreateOptions.None;
                    bitmap.StreamSource = stream;
                    //bitmap.UriSource = new Uri(filename); // specify the file for the source of bitmap image
                    bitmap.EndInit();
                    stream.Close();
                    //image_original.Source = bitmap; // binding to image control

                    original = BitmapFactory.ConvertToPbgra32Format(bitmap);

                    // prepare the instance for processed image
                    processed = BitmapFactory.New(original.PixelWidth, original.PixelHeight);

                    width = original.PixelWidth;
                    height = original.PixelHeight;

                    if (width * height > 1024 * 1024 * 2)
                    {
                        dwnscale = 8;
                    }
                    else if (width * height > 1024 * 1024)
                    {
                        dwnscale = 4;
                    }
                    else
                    {
                        dwnscale = 1;
                    }


                    algorithms algorithmes2 = new algorithms(width, height, dwnscale);
                    color = new ushort[width * 3 * height];
                    color2 = new ushort[width * 3 * height];
                    color_small = new ushort[width * 3 / dwnscale * height / dwnscale];
                    color2_small = new ushort[width * 3 / dwnscale * height / dwnscale];
                    //color_resized = new ushort[width/16 * dwnscale * height/16];

                    PixelFormat format;

                    original_data = FileConvertArray(filename, out format);

                    //Split in Color Channel
                    algorithmes2.Ch_split(original_data);

                    //Demosaic
                    color = algorithmes2.Demosaic();

                    //Small Color Image
                    color_small = algorithmes2.ComplessColorImage();

                    //Save Images
                    algorithmes2.Save_Demosaiced_Imege();





                    image_processed.Dispatcher.BeginInvoke(
                    new Action(() =>
                    {
                        //Show Color Image
                        var colorBitmap = BitmapSource.Create(width / dwnscale, height / dwnscale, 96, 96, PixelFormats.Rgb48, null, color_small, 16 * 3 * width / dwnscale / 8);

                        //image_processed.Source = resized_colorBitmap;
                        image_processed.Source = colorBitmap;

                    }));

                    basic_process = new basic_process(width, height, dwnscale);

                    algorithmes2 = null;


                });


                //Title_Border.Visibility = Visibility.Hidden;
                //ProgressBar_wait.Visibility = Visibility.Hidden;
                GC.Collect();

            }

        }



        private Array FileConvertArray(string filename, out PixelFormat format)

        {
            Array arr;
            using (FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read))
            {
                BitmapFrame bitmapFrame = BitmapFrame.Create(
                    fs,
                    BitmapCreateOptions.PreservePixelFormat,
                    BitmapCacheOption.Default
                    );
                //BitmapDecoder decoder = new TiffBitmapDecoder(fs, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
                //BitmapDecoder decoder = new BmpBitmapDecoder(fs, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
                //BitmapDecoder decoder = new PngBitmapDecoder(fs, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);

                //BitmapFrame bitmapFrame = decoder.Frames[0];

                int width = bitmapFrame.PixelWidth;
                int height = bitmapFrame.PixelHeight;
                format = bitmapFrame.Format;

                int stride = ((width * format.BitsPerPixel + 31) / 32) * 4;

                if (format == PixelFormats.Gray16)
                {
                    arr = new ushort[width * height];
                }

                else if (format == PixelFormats.Rgb48)
                {
                    arr = new ushort[width * 3 * height];
                }

                else
                {
                    arr = new byte[stride * height];
                }

                // 輝度データを配列へコピー
                bitmapFrame.CopyPixels(arr, stride, 0);

                fs.Dispose();
            }



            GC.Collect();
            return arr;
        }


        private void Total_Offset_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Offset = Total_Offset_slider.Value;
            Task.Run(() =>
            {
                Color_Small_Image_Update();
            });

        }

        private void R_Offset_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            R_Offset = R_Offset_slider.Value;
            Task.Run(() =>
            {
                Color_Small_Image_Update();
            });
        }

        private void G_Offset_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            G_Offset = G_Offset_slider.Value;
            Task.Run(() =>
            {
                Color_Small_Image_Update();
            });
        }

        private void B_Offset_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            B_Offset = B_Offset_slider.Value;
            Task.Run(() =>
            {
                Color_Small_Image_Update();
            });
        }


        private void Total_Gain_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Gain = Total_Gain_slider.Value;
            Task.Run(() =>
            {
                Color_Small_Image_Update();
            });

        }

        private void R_Gain_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            R_Gain = R_Gain_slider.Value;
            Task.Run(() =>
            {
                Color_Small_Image_Update();
            });
        }

        private void G_Gain_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            G_Gain = G_Gain_slider.Value;
            Task.Run(() =>
            {
                Color_Small_Image_Update();
            });
        }

        private void B_Gain_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            B_Gain = B_Gain_slider.Value;
            Task.Run(() =>
            {
                Color_Small_Image_Update();
            });
        }

        private void Gamma_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            gamma = Gamma_slider.Value;
            Task.Run(() =>
            {
                Color_Small_Image_Update();
            });
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            //MessageBox.Show("タスクが完了しました。");
            //vm.Progress = 0;
        }


        //// 現在値の更新
        //private void Worker_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        //{
        //    // プログレスバーの現在値を更新する
        //    //ProgressBar1.Value = e.ProgressPercentage;
        //    // 進捗率を表示する
        //    TextBlock1.Text = ProgressBar1.Value.ToString() + "%";
        //}

        static object lockObject = new object();

        private void Color_Small_Image_Update()
        {



            if (color != null)
            {

                basic_process.GetColor_Process(color_small, dwnscale, Offset, R_Offset, G_Offset, B_Offset, Gain, R_Gain, G_Gain, B_Gain, gamma);
                Console.Write("タスク完了になってるよねええ?\n");

                lock (lockObject)
                {
                    color2_small = basic_process.GetColor();

                }

                image_processed.Dispatcher.BeginInvoke(new Action(() =>
                {
                    var bitmap = new WriteableBitmap(width / dwnscale, height / dwnscale, 96, 96, PixelFormats.Rgb48, null);
                    bitmap.Lock();
                    Console.Write("タスク完了になってるよねええaaaaa?\n");
                    unsafe
                    {
                        ushort* Ptr = (ushort*)bitmap.BackBuffer;
                        for (int y = 0; y < bitmap.PixelHeight; y++)
                        {
                            for (int x = 0; x < bitmap.PixelWidth; x++)
                            {
                                Ptr[0] = color2_small[3 * x + width / dwnscale * 3 * y];
                                Ptr[1] = color2_small[3 * x + 1 + width / dwnscale * 3 * y];
                                Ptr[2] = color2_small[3 * x + 2 + width / dwnscale * 3 * y];
                                Ptr += 3;
                            }
                        }
                        bitmap.AddDirtyRect(new Int32Rect(0, 0, (int)bitmap.Width, (int)bitmap.Height));
                        bitmap.Unlock();

                        image_processed.Source = bitmap;
                    }
                }));
            }
            GC.Collect();
            Console.Write("カラー配列格納完\n");

        }








        /// ////////////ZOOM and MOVE///////////

        private System.Windows.Point _start;
            private double size_ratio = 1;

            private void Image_MouseWheel(object sender, MouseWheelEventArgs e)
            {
                //Set scale
                const double scale = 1.2;
                double new_size_ratio;

                var matrix = image_processed.RenderTransform.Value;

                if (e.Delta > 0)
                {
                    new_size_ratio = size_ratio * scale;
                    // 拡大処理
                    matrix.ScaleAt(scale, scale, e.GetPosition(image_processed).X, e.GetPosition(image_processed).Y);
                    size_ratio = new_size_ratio;
                    image_processed.RenderTransform = new MatrixTransform(matrix);

                }
                else
                {
                    new_size_ratio = size_ratio / scale;

                    if (new_size_ratio >= 1)
                    {
                        // 縮小処理
                        matrix.ScaleAt(1.0 / scale, 1.0 / scale, e.GetPosition(image_processed).X, e.GetPosition(image_processed).Y);
                        size_ratio = new_size_ratio;
                        image_processed.RenderTransform = new MatrixTransform(matrix);
                    }
                    else
                    {
                        PositionReset();
                        size_ratio = 1;
                    }
                }
            }

            private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
            {
                if (e.ChangedButton == MouseButton.Left && e.ClickCount == 2)
                {
                    PositionReset();
                }
                else
                {
                    image_processed.CaptureMouse();
                    _start = e.GetPosition(image_processed);
            }
            }

            private void Image_MouseMove(object sender, MouseEventArgs e)
            {
                if (image_processed.IsMouseCaptured)
                {
                    var matrix = image_processed.RenderTransform.Value;

                    Vector v = _start - e.GetPosition(image_processed);
                    PositionGet_start(v.X, v.X);
                    matrix.Translate(-v.X, -v.Y);
                    image_processed.RenderTransform = new MatrixTransform(matrix);
                }
                PositionShow(e.GetPosition(image_processed).X / image_processed.ActualWidth*width
                    , e.GetPosition(image_processed).Y / image_processed.ActualHeight * height);
            //PositionShow(image_processed.ActualHeight, image_processed.ActualWidth);
        }

        private void Image_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
            {
                image_processed.ReleaseMouseCapture();
            }

            private void Window_KeyDown(object sender, KeyEventArgs e)
            {
                if (e.Key == Key.Escape)
                {
                    PositionReset();
                }
            }

            private void PositionReset()
            {
                var matrix = image_processed.RenderTransform.Value;
                matrix.M11 = 1.0;
                matrix.M12 = 0.0;
                matrix.M21 = 0.0;
                matrix.M22 = 1.0;
                matrix.OffsetX = 0.0;
                matrix.OffsetY = 0.0;
                image_processed.RenderTransform = new MatrixTransform(matrix);
            }

            private void PositionShow(double X, double Y)
        {
            int Xint = (int)X;
            int Yint = (int)Y;
            string Xpos = Xint.ToString();
            string Ypos = Yint.ToString();

            string pos_print = "(x,y)=(" + Xpos + "," + Ypos + ")";
            Pixel_Position.Text = pos_print;
        }

        private void PositionGet_start(double X, double Y)
        {
            string Xpos = X.ToString();
            string Ypos = Y.ToString();

            string pos_print = "(x,y)=(" + Xpos + "," + Ypos + ")";
            Pixel_Position_Start.Text = pos_print;
        }

        private void FileSaveButton_Click(object sender, RoutedEventArgs e)
        {
            var win = new SaveImageWindow();
            win.Show();
        }

     }
    

}
