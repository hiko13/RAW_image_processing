using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.IO;
using System.ComponentModel;
using System.Threading;

namespace image_processing
{

    public class algorithms
    {
        static private int image_width;
        static private int image_height;
        static private int dwnscale;
        private ushort[] Rdata, Grdata, Gbdata, Bdata;
        private ushort[] color, color_small;

        private ushort[] Rdemos;
        private ushort[] Grdemos;
        private ushort[] Gbdemos;
        private ushort[] Bdemos;
        private ushort[] Gdemos;

        public algorithms(int image_width_o, int image_height_o, int dwnscale_o) {
            Console.Write("Called constructor of the class algorithms\n");
            image_width = image_width_o;
            image_height = image_height_o;
            dwnscale = dwnscale_o;
        }

        ~algorithms(){
         Console.Write("Called destructor of the class algorithms\n");
            Rdemos = null;
            Grdemos = null;
            Gbdemos = null;
            Bdemos = null;
            Gdemos = null;
            color = null;
            GC.Collect();
        }

        public void Ch_split(Array raw_image)
            {
            Rdata = new ushort[image_width * image_height / 4];
            Grdata = new ushort[image_width * image_height / 4];
            Gbdata = new ushort[image_width * image_height / 4];
            Bdata = new ushort[image_width * image_height / 4];

            for (int y = 0; y < image_height; y = y + 2)
            {
                for (int x = 0; x < image_width; x = x + 2)
                {
                    Array.Copy(raw_image, x + y * image_width, Rdata, x / 2 + y / 2 * image_width / 2, 1);
                    Array.Copy(raw_image, (x + 1) + y * image_width, Grdata, x / 2 + y / 2 * image_width / 2, 1);
                    Array.Copy(raw_image, x + (y + 1) * image_width, Gbdata, x / 2 + y / 2 * image_width / 2, 1);
                    Array.Copy(raw_image, (x + 1) + (y + 1) * image_width, Bdata, x / 2 + y / 2 * image_width / 2, 1);
                }
            }
            
        }

        private ushort[] Interpolation_R(ushort[] Rdata)
        {
            Rdemos = new ushort[image_width * image_height];
            int h1, h2, v1, v2;

            for (int y = 0; y < image_height; y = y + 2)
            {
                if (y == 0) { v1 = 1; v2 = 1; }//first row
                else if (y == image_height - 2) { v1 = -1; v2 = -1; }//last row
                else { v1 = -1; v2 = 1; };//other row

                for (int x = 0; x < image_width; x = x + 2)
                {
                    if (x == 0) { h1 = 1; h2 = 1; }//first column
                    else if (x == image_width - 2) { h1 = -1; h2 = -1; }//last column
                    else { h1 = -1; h2 = 1; };//other column

                    Rdemos[x + image_width * y]
                                    = Rdata[x / 2 + (image_width / 2) * y / 2];
                    Rdemos[x + 1 + image_width * y]
                       = (ushort)((Rdata[x / 2 + (image_width / 2) * y / 2] + Rdata[x / 2 + h2 + (image_width / 2) * y / 2]) / 2);
                    Rdemos[x + image_width * (y + 1)]
                       = (ushort)((Rdata[x / 2 + (image_width / 2) * y / 2] + Rdata[x / 2 + (image_width / 2) * (y / 2 + v2)]) / 2);
                    Rdemos[x + 1 + image_width * (y + 1)]
                       = (ushort)((Rdata[x / 2 + (image_width / 2) * y / 2] + Rdata[x / 2 + h2 + (image_width / 2) * y / 2] + Rdata[x / 2 + (image_width / 2) * (y / 2 + v2)] + Rdata[x / 2 + h2 + (image_width / 2) * (y / 2 + v2)]) / 4);
                }
            }
            return Rdemos;
        }

        private ushort[] Interpolation_Gr(ushort[] Grdata)
        {
            Grdemos = new ushort[image_width * image_height];
            int h1, h2, v1, v2;

            for (int y = 0; y < image_height; y = y + 2)
            {
                if (y == 0) { v1 = 1; v2 = 1; }//first row
                else if (y == image_height - 2) { v1 = -1; v2 = -1; }//last row
                else { v1 = -1; v2 = 1; };//other row

                for (int x = 0; x < image_width; x = x + 2)
                {
                    if (x == 0) { h1 = 1; h2 = 1; }//first column
                    else if (x == image_width - 2) { h1 = -1; h2 = -1; }//last column
                    else { h1 = -1; h2 = 1; };//other column


                    Grdemos[x + image_width * y]
                        = (ushort)((Grdata[x / 2 + h1 + (image_width / 2) * y / 2] + Grdata[x / 2 + h2 + (image_width / 2) * y / 2]) / 2);
                    Grdemos[x + 1 + image_width * y]
                        = Grdata[x / 2 + (image_width / 2) * y / 2];
                    Grdemos[x + image_width * (y + 1)]
                        = (ushort)((Grdata[x / 2 + h1 + (image_width / 2) * (y / 2)] + Grdata[x / 2 + (image_width / 2) * y / 2] + Grdata[x / 2 + h1 + (image_width / 2) * (y / 2 + v2)] + Grdata[x / 2 + (image_width / 2) * (y / 2 + v2)]) / 4);
                    Grdemos[x + 1 + image_width * (y + 1)]
                        = (ushort)((Grdata[x / 2 + (image_width / 2) * y / 2] + Grdata[x / 2 + (image_width / 2) * y / 2 + v2]) / 2);
                }
            }
            return Grdemos;
        }

        private ushort[] Interpolation_Gb(ushort[] Gbdata)
        {
            Gbdemos = new ushort[image_width * image_height];
            int h1, h2, v1, v2;

            for (int y = 0; y < image_height; y = y + 2)
            {
                if (y == 0) { v1 = 1; v2 = 1; }//first row
                else if (y == image_height - 2) { v1 = -1; v2 = -1; }//last row
                else { v1 = -1; v2 = 1; };//other row

                for (int x = 0; x < image_width; x = x + 2)
                {
                    if (x == 0) { h1 = 1; h2 = 1; }//first column
                    else if (x == image_width - 2) { h1 = -1; h2 = -1; }//last column
                    else { h1 = -1; h2 = 1; };//other column

                    Gbdemos[x + image_width * y]
                        = (ushort)((Gbdata[x / 2 + (image_width / 2) * y / 2] + Gbdata[x / 2 + (image_width / 2) * y / 2 + v2]) / 2);
                    Gbdemos[x + 1 + image_width * y]
                        = (ushort)((Gbdata[x / 2 + (image_width / 2) * y / 2 + v1] + Gbdata[x / 2 + h2 + (image_width / 2) * y / 2 + v1] + Gbdata[x / 2 + (image_width / 2) * y / 2] + Gbdata[x / 2 + h2 + (image_width / 2) * y / 2]) / 4);
                    Gbdemos[x + image_width * (y + 1)]
                        = Gbdata[x / 2 + (image_width / 2) * y / 2];
                    Gbdemos[x + 1 + image_width * (y + 1)]
                        = (ushort)((Gbdata[x / 2 + (image_width / 2) * y / 2] + Gbdata[x / 2 + h2 + (image_width / 2) * y / 2]) / 2);
                }
            }

            return Gbdemos;
        }

        private ushort[] Interpolation_B(ushort[] Bdata)
        {
            Bdemos = new ushort[image_width * image_height];
            int h1, h2, v1, v2;

            for (int y = 0; y < image_height; y = y + 2)
            {
                if (y == 0) { v1 = 1; v2 = 1; }//first row
                else if (y == image_height - 2) { v1 = -1; v2 = -1; }//last row
                else { v1 = -1; v2 = 1; };//other row

                for (int x = 0; x < image_width; x = x + 2)
                {
                    if (x == 0) { h1 = 1; h2 = 1; }//first column
                    else if (x == image_width - 2) { h1 = -1; h2 = -1; }//last column
                    else { h1 = -1; h2 = 1; };//other column

                    Bdemos[x + image_width * y]
                         = (ushort)((Bdata[x / 2 + h1 + (image_width / 2) * y / 2 + v1] + Bdata[x / 2 + (image_width / 2) * y / 2 + v1] + Bdata[x / 2 + h1 + (image_width / 2) * y / 2] + Bdata[x / 2 + (image_width / 2) * y / 2]) / 4);
                    Bdemos[x + 1 + image_width * y]
                        = (ushort)((Bdata[x / 2 + (image_width / 2) * y / 2 + v1] + Bdata[x / 2 + (image_width / 2) * y / 2]) / 2);
                    Bdemos[x + image_width * (y + 1)
                        ] = (ushort)((Bdata[x / 2 + h1 + (image_width / 2) * y / 2] + Bdata[x / 2 + (image_width / 2) * y / 2]) / 2);
                    Bdemos[x + 1 + image_width * (y + 1)
                        ] = Bdata[x / 2 + (image_width / 2) * y / 2];

                }
            }
            return Bdemos;
        }

        private ushort[] Combine_GrGb(ushort[] Grdemos, ushort[] Gbdemos)
        {
            Gdemos = new ushort[image_width * image_height];

            for (int y = 0; y < image_height; ++y)
            {
                for (int x = 0; x < image_width; ++x)
                {
                    Gdemos[x + image_width * y] = (ushort)((Grdemos[x + image_width * y] + Gbdemos[x + image_width * y]) / 2);
                }
            }

            return Gdemos;
        }

        private ushort[] Create_Color_Array(ushort[] Rdemos, ushort[] Gdemos, ushort[] Bdemos)
        {
            color = new ushort[image_width * 3 * image_height];

            for (int j = 0; j < image_height; j++)
            {
                for (int i = 0; i < image_width; i++)
                {
                    color[i * 3 + j * image_width * 3] = Rdemos[i + image_width * j];
                    color[i * 3 + 1 + j * image_width * 3] = Gdemos[i + image_width * j];
                    color[i * 3 + 2 + j * image_width * 3] = Bdemos[i + image_width * j];
                }
            }

            return color;
        }

        public ushort[] Demosaic()
            {
            //Interpolation

            var options = new ParallelOptions();
            Parallel.Invoke(options,
                () => Rdemos = Interpolation_R(Rdata),
                () => Grdemos = Interpolation_Gr(Grdata),
                () => Gbdemos = Interpolation_Gb(Gbdata),
                () => Bdemos = Interpolation_B(Bdata)
                );
            
            Console.Write("補間完了\n");

            //Combine GrGb
            Gdemos = Combine_GrGb(Grdemos, Gbdemos);

            //Set to Color Image Array
            color = Create_Color_Array(Rdemos,Gdemos,Bdemos);
            Console.Write("カラー配列格納完\n");

            return color;
            }

        public ushort[] ComplessColorImage()//resize the color image
        {
            color_small = new ushort[image_width / dwnscale * 3 * image_height / dwnscale];

            for (int j = 0; j < image_height/ dwnscale; j++)
            {
                for (int i = 0; i < image_width/ dwnscale; i++)
                {
                    color_small[i * 3 + j * image_width/ dwnscale * 3] = color[i*3 * dwnscale + image_width * j * 3 * dwnscale];
                    color_small[i * 3 + 1 + j * image_width / dwnscale * 3] = color[i * 3 * dwnscale + 1 + image_width * j * 3 * dwnscale];
                    color_small[i * 3 + 2 + j * image_width / dwnscale * 3] = color[i * 3 * dwnscale + 2 + image_width * j * 3 * dwnscale];
                }
            }

            return color_small;
        }

        public void Save_Demosaiced_Imege()//Save images(R-ch image, G-ch image, B-ch image, color image)
            {
                SaveImage("Rdemos.tif", Rdemos, image_width, image_height, PixelFormats.Gray16);
                SaveImage("Gdemos.tif", Gdemos, image_width, image_height, PixelFormats.Gray16);
                SaveImage("Bdemos.tif", Bdemos, image_width, image_height, PixelFormats.Gray16);
                SaveImage("color.tif", color, image_width, image_height, PixelFormats.Rgb48);
        }

        public void SaveImage(string filename, Array bmpData, int width, int height, System.Windows.Media.PixelFormat format, double dpi = 96)
            {

                int stride = ((width * format.BitsPerPixel) / 8);

                System.Windows.Media.Imaging.BitmapSource bs = System.Windows.Media.Imaging.BitmapSource.Create(
                    width,
                    height,
                    dpi,
                    dpi,
                    format,
                    null,
                    bmpData,
                    stride);

                // file extention
                var ext = System.IO.Path.GetExtension(filename).ToLower();

                BitmapEncoder encoder;

                switch (ext)
                {
                    case ".bmp":
                        encoder = new BmpBitmapEncoder();
                        break;

                    case ".tif":
                        encoder = new TiffBitmapEncoder { Compression = TiffCompressOption.None };
                        break;

                    case ".png":
                        encoder = new PngBitmapEncoder();
                        break;
                    default:
                        encoder = null;
                        break;
                }

                using (Stream stream = new FileStream(filename, FileMode.Create))
                {
                    var bf = BitmapFrame.Create(bs);
                    encoder.Frames.Add(bf);
                    encoder.Save(stream);
                }

            }

    }


    //public class Convolution
    //{

    //    public void Convolution(ushort[] image, int size, int pattern, double param, int C_offset)
    //    {
    //        // ushort[] new_image = new ushort[image_width * image_height];
    //        ushort[,] calc_area = new ushort[size, size];
    //        int[,] check_array = new int[size, size];
    //        int[,] Kernel = new int[size, size];


    //        Kernel = KernelSelect(size, pattern, param);

    //        for (int x = 0; x < image_width; x++)
    //        {
    //            for (int y = 0; y < image_height; y++)
    //            {
    //                check_array = OverCheck(size, x, y);
    //                calc_area = ZeroPadding(check_array, image, size, x, y);
    //                color2[x + C_offset + y * image_width] = KernelCalc(calc_area, Kernel, size);
    //            }
    //        }
    //        Console.Write("END OD CONVOLUTION\n");

    //    }

    //    private int[,] OverCheck(int size, int x, int y)
    //    {
    //        int[,] check_array = new int[size, size];
    //        int x_addr, y_addr;
    //        for (int i = 0; i < size; i++)
    //        {
    //            for (int j = 0; j < size; j++)
    //            {
    //                x_addr = x - (size - 1) / 2 + i;
    //                y_addr = y - (size - 1) / 2 + j;

    //                if (x_addr < 0 || x_addr > image_width || y_addr < 0 || y_addr > image_height)
    //                {
    //                    check_array[i, j] = 1;
    //                }
    //                else
    //                {
    //                    check_array[i, j] = 0;
    //                }
    //            }
    //        }
    //        return check_array;
    //    }

    //    private ushort[,] ZeroPadding(int[,] checkarray, ushort[] image, int size, int x, int y)
    //    {
    //        ushort[,] calc_area = new ushort[size, size];

    //        for (int i = 0; i < size; i++)
    //        {
    //            for (int j = 0; j < size; j++)
    //            {
    //                if (checkarray[i, j] == 1)
    //                {
    //                    calc_area[i, j] = 0;
    //                }
    //                else
    //                {
    //                    calc_area[i, j] = image[(x - (size - 1) / 2 + i)
    //                        + (y - (size - 1) / 2 + j) * image_width];
    //                }

    //            }
    //        }

    //        return calc_area;
    //    }

    //    private ushort KernelCalc(ushort[,] calc_area, int[,] Kernel, int size)
    //    {
    //        int val = 0;
    //        int val1;
    //        ushort val_ushort;

    //        for (int x = 0; x < size; x++)
    //        {
    //            for (int y = 0; y < size; y++)
    //            {
    //                val1 = calc_area[x, y] * Kernel[x, y];
    //                val = val + val1;
    //            }
    //        }
    //        val_ushort = (ushort)val;
    //        return val_ushort;
    //    }

    //    private int[,] KernelSelect(int size, int pattern, double param)
    //    {
    //        int[,] Kernel = new int[size, size];

    //        switch (pattern)
    //        {
    //            case 1://mean filter
    //                Kernel = MeanKernelGen(size);
    //                break;
    //            case 2://Gaussian Filter
    //                Kernel = GaussianKernelGen(size, param);
    //                break;
    //            case 3://Lapracian Filter
    //                Kernel = LaplacianKernelGen(size, param);
    //                break;
    //            default:
    //                break;
    //        }
    //        return Kernel;
    //    }

    //    private int[,] MeanKernelGen(int size)
    //    {
    //        int[,] Kernel = new int[size, size];

    //        for (int x = 0; x < size; x++)
    //        {
    //            for (int y = 0; y < size; y++)
    //            {
    //                Kernel[x, y] = 1;
    //            }
    //        }
    //        return Kernel;
    //    }

    //    private int[,] GaussianKernelGen(int size, double sigma)
    //    {
    //        int[,] Kernel = new int[size, size];
    //        double[,] val = new double[size, size];
    //        int[,] val_int = new int[size, size];
    //        double val1, val2;
    //        double sum = 0;
    //        double k;
    //        int x_addr, y_addr;

    //        for (int y = 0; y < size; y++)
    //        {
    //            for (int x = 0; x < size; x++)
    //            {
    //                x_addr = x - (size - 1) / 2;
    //                y_addr = y - (size - 1) / 2;

    //                val1 = -((Math.Pow(x_addr, 2) + Math.Pow(y_addr, 2)) / (2 * Math.Pow(sigma, 2)));
    //                val2 = 1 / (2 * Math.PI * Math.Pow(sigma, 2));
    //                val[x, y] = (val2) * Math.Exp(val1);

    //                sum = sum + val[x, y];
    //            }
    //        }

    //        Console.Write("SUM = " + sum + "\n");

    //        k = 256 / sum;

    //        for (int y = 0; y < size; y++)
    //        {
    //            for (int x = 0; x < size; x++)
    //            {
    //                val_int[x, y] = (int)(val[x, y] * k);
    //            }
    //        }

    //        Console.Write("SUM = " + sum + "\n");

    //        return val_int;
    //    }

    //    private int[,] LaplacianKernelGen(int size, double intensity)
    //    {
    //        int[,] Kernel = new int[size, size];

    //        for (int x = 0; x < size; x++)
    //        {
    //            for (int y = 0; y < size; y++)
    //            {
    //                if (x == y)
    //                {
    //                    Kernel[x, y] = -(int)((Math.Pow(size, 2) - 1) * intensity);
    //                }
    //                Kernel[x, y] = (int)intensity;
    //            }
    //        }
    //        return Kernel;
    //    }

    //    private int CheckKarnelSum(int[,] Kernel, int size)
    //    {
    //        int sum = 0;
    //        for (int y = 0; y < size; y++)
    //        {
    //            for (int x = 0; x < size; x++)
    //            {
    //                sum = sum + Kernel[x, y];
    //            }
    //        }
    //        return sum;
    //    }

    //}

    public class basic_process
    {

        private int image_width;
        private int image_height;
        private int dwnscale;
        private ushort[] color2;
        private ushort[] color2_small;

        public basic_process(int image_width_o, int image_height_o, int dwnscale_o)
        {
            image_width = image_width_o;
            image_height = image_height_o;
            dwnscale = dwnscale_o;
            color2 = new ushort[image_width * 3 * image_height];
            color2_small = new ushort[image_width/dwnscale * 3 * image_height/ dwnscale];
        }

        public ushort[] GetColor()
        {
            return color2;
        }

        public void GetColor_Process(ushort[] color, int dwnscale, double Offset, double R_Offset, double G_Offset, double B_Offset
                  , double Gain, double R_Gain, double G_Gain, double B_Gain, double gamma)
        {

                GainOffset_RGB(color, dwnscale, Offset, R_Offset, G_Offset, B_Offset,
                 Gain, R_Gain, G_Gain, B_Gain, gamma);

            Console.Write("タスク完了になってる?\n");
        }



        public void  GainOffset_RGB(ushort[] color, int dwnscale, double Offset, double R_Offset, double G_Offset, double B_Offset
                  , double Gain, double R_Gain, double G_Gain, double B_Gain, double gamma)
        {
            var options = new ParallelOptions();
            Parallel.Invoke(options,
                () => GainOffset_loop(0, color, dwnscale, Offset, R_Offset, Gain, R_Gain, gamma),
                () => GainOffset_loop(1, color, dwnscale, Offset, G_Offset, Gain, G_Gain, gamma),
                () => GainOffset_loop(2, color, dwnscale, Offset, B_Offset, Gain, B_Gain, gamma)
                );
        }

        private void GainOffset_loop(int addr_offset, ushort[] color, int dwnscale, double Offset, double C_Offset, double Gain, double C_Gain, double gamma)
        {
            for (int y = 0; y <= image_height / dwnscale - 1; ++y)
            {
                for (int x = 0; x <= image_width / dwnscale - 1; ++x)
                {
                    color2[x * 3 + addr_offset + image_width / dwnscale * 3 * y]
                        = GainOffset_U(color[x * 3 + addr_offset + image_width / dwnscale * 3 * y], dwnscale
                                            , Offset, C_Offset, Gain, C_Gain, gamma);
                }
            }
            Console.Write("ループ完\n");
        }

        private ushort GainOffset_U(ushort color_in, int dwnscale, double Offset, double C_Offset, double Gain, double C_Gain, double gamma)
        {
            double added;
            ushort color_prc;
            added = (double)color_in * Gain * C_Gain + Offset + C_Offset;
            added = Clip_16bit(added);
            added = GammaCorrection(added, gamma);
            added = Clip_16bit(added);

            color_prc= (ushort)(added);

            return color_prc;
        }

        private double GammaCorrection(double val, double gamma)
        {
            double gamma_val = Math.Pow(val, gamma) / (Math.Pow(65535, gamma)) * 65535;
            double gamma_val_clip = Clip_16bit(gamma_val);
            return gamma_val;
        }

        private double Clip_16bit(double val)
        {

            if (val > 65535) val = 65535;
            else if (val < 0) val = 0;

            return val;
        }


    }



    public class basic_process_unsafe
    {

        private int image_width;
        private int image_height;
        private int dwnscale;
        private ushort[] color2;
        private ushort[] color2_small;

        public basic_process_unsafe(int image_width_o, int image_height_o, int dwnscale_o)
        {
            image_width = image_width_o;
            image_height = image_height_o;
            dwnscale = dwnscale_o;
            color2 = new ushort[image_width * 3 * image_height];
            color2_small = new ushort[image_width / dwnscale * 3 * image_height / dwnscale];
            
        }

        public ushort[] GetColor()
        {
            return color2;
        }

        public unsafe void GetColor_Process(ushort* color, ushort* color_buf, int dwnscale, double Offset, double R_Offset, double G_Offset, double B_Offset
                  , double Gain, double R_Gain, double G_Gain, double B_Gain, double gamma)
        {

            GainOffset_RGB(color, color_buf, dwnscale, Offset, R_Offset, G_Offset, B_Offset,
             Gain, R_Gain, G_Gain, B_Gain, gamma);

            Console.Write("タスク完了になってる?\n");
        }



        public unsafe void GainOffset_RGB(ushort* color, ushort* color_buf, int dwnscale, double Offset, double R_Offset, double G_Offset, double B_Offset
                  , double Gain, double R_Gain, double G_Gain, double B_Gain, double gamma)
        {
            var options = new ParallelOptions();
            Parallel.Invoke(options,
                () => GainOffset_loop(0, color, color_buf, dwnscale, Offset, R_Offset, Gain, R_Gain, gamma),
                () => GainOffset_loop(1, color, color_buf, dwnscale, Offset, G_Offset, Gain, G_Gain, gamma),
                () => GainOffset_loop(2, color, color_buf, dwnscale, Offset, B_Offset, Gain, B_Gain, gamma)
                );          
        }

        private unsafe void GainOffset_loop(int addr_offset, ushort* color, ushort* color_buf, int dwnscale, double Offset, double C_Offset, double Gain, double C_Gain, double gamma)
        {
                for (int y = 0; y <= image_height / dwnscale - 1; ++y)
                {
                    for (int x = 0; x <= image_width / dwnscale - 1; ++x)
                    {
                    color_buf[x * 3 + addr_offset + image_width / dwnscale * 3 * y]
                        = GainOffset_U(color[x * 3 + addr_offset + image_width / dwnscale * 3 * y], dwnscale
                                            , Offset, C_Offset, Gain, C_Gain, gamma);
                }
                }
            Console.Write("ループ完\n");
        }

        private ushort GainOffset_U(ushort color_in, int dwnscale, double Offset, double C_Offset, double Gain, double C_Gain, double gamma)
        {
            double added;
            ushort color_prc;
            added = (double)color_in * Gain * C_Gain + Offset + C_Offset;
            added = Clip_16bit(added);
            added = GammaCorrection(added, gamma);
            added = Clip_16bit(added);

            color_prc = (ushort)(added);

            return color_prc;
        }

        private double GammaCorrection(double val, double gamma)
        {
            double gamma_val = Math.Pow(val, gamma) / (Math.Pow(65535, gamma)) * 65535;
            double gamma_val_clip = Clip_16bit(gamma_val);
            return gamma_val;
        }

        private double Clip_16bit(double val)
        {
            if (val > 65535) val = 65535;
            else if (val < 0) val = 0;

            return val;
        }


    }


}
