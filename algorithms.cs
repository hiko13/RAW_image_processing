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

        public int progress { get; set; }


        public algorithms(int image_width_o, int image_height_o, int dwnscale_o) {
            Console.Write("algorithms クラスのコンストラクターが呼ばれました\n");
            image_width = image_width_o;
            image_height = image_height_o;
            dwnscale = dwnscale_o;
        }

        ~algorithms(){
         Console.Write("algorithms クラスのデストラクターが呼ばれました\n");
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

        public ushort[] Interpolation_R(ushort[] Rdata)
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

        public ushort[] Interpolation_Gr(ushort[] Grdata)
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

        public ushort[] Interpolation_Gb(ushort[] Gbdata)
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

        public ushort[] Interpolation_B(ushort[] Bdata)
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

        public ushort[] Combine_GrGb(ushort[] Grdemos, ushort[] Gbdemos)
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

        public ushort[] Create_Color_Array(ushort[] Rdemos, ushort[] Gdemos, ushort[] Bdemos)
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
            Rdemos = Interpolation_R(Rdata);
            Grdemos = Interpolation_Gr(Grdata);
            Gbdemos = Interpolation_Gb(Gbdata);
            Bdemos = Interpolation_B(Bdata);
            Console.Write("補間完了\n");

            //Combine GrGb
            Gdemos = Combine_GrGb(Grdemos, Gbdemos);

            //Set to Color Image Array
            color = Create_Color_Array(Rdemos,Gdemos,Bdemos);
            Console.Write("カラー配列格納完\n");

            return color;
            }

        public ushort[] ComplessColorImage()
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

            public void Save_Demosaiced_Imege()
            {
                SaveImage("Rdemos.tif", Rdemos, image_width, image_height, PixelFormats.Gray16);
                SaveImage("Gdemos.tif", Gdemos, image_width, image_height, PixelFormats.Gray16);
                SaveImage("Bdemos.tif", Bdemos, image_width, image_height, PixelFormats.Gray16);
                SaveImage("color.tif", color, image_width, image_height, PixelFormats.Gray16);
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

                // ファイル名の拡張子
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

    //public class algorithms_unsafe
    //{
    //    static private int image_width;
    //    static private int image_height;
    //    static private int dwnscale;
    //    private ushort[] Rdata, Grdata, Gbdata, Bdata;
    //    private ushort[] color, color_small;

    //    private ushort[] Rdemos;
    //    private ushort[] Grdemos;
    //    private ushort[] Gbdemos;
    //    private ushort[] Bdemos;
    //    private ushort[] Gdemos;

    //    public int progress { get; set; }


    //    public algorithms_unsafe(int image_width_o, int image_height_o, int dwnscale_o)
    //    {
    //        Console.Write("algorithms クラスのコンストラクターが呼ばれました\n");
    //        image_width = image_width_o;
    //        image_height = image_height_o;
    //        dwnscale = dwnscale_o;
    //    }

    //    ~algorithms_unsafe()
    //    {
    //        Console.Write("algorithms クラスのデストラクターが呼ばれました\n");
    //        Rdemos = null;
    //        Grdemos = null;
    //        Gbdemos = null;
    //        Bdemos = null;
    //        Gdemos = null;
    //        color = null;
    //        GC.Collect();
    //    }

    //    public void Ch_split(Array raw_image)
    //    {
    //        Rdata = new ushort[image_width * image_height / 4];
    //        Grdata = new ushort[image_width * image_height / 4];
    //        Gbdata = new ushort[image_width * image_height / 4];
    //        Bdata = new ushort[image_width * image_height / 4];

    //        for (int y = 0; y < image_height; y = y + 2)
    //        {
    //            for (int x = 0; x < image_width; x = x + 2)
    //            {
    //                Array.Copy(raw_image, x + y * image_width, Rdata, x / 2 + y / 2 * image_width / 2, 1);
    //                Array.Copy(raw_image, (x + 1) + y * image_width, Grdata, x / 2 + y / 2 * image_width / 2, 1);
    //                Array.Copy(raw_image, x + (y + 1) * image_width, Gbdata, x / 2 + y / 2 * image_width / 2, 1);
    //                Array.Copy(raw_image, (x + 1) + (y + 1) * image_width, Bdata, x / 2 + y / 2 * image_width / 2, 1);
    //            }
    //        }

    //    }

    //    public ushort[] Interpolation_R(ushort* Rdata)
    //    {
    //        Rdemos = new ushort[image_width * image_height];
    //        int h1, h2, v1, v2;

    //        for (int y = 0; y < image_height; y = y + 2)
    //        {
    //            if (y == 0) { v1 = 1; v2 = 1; }//first row
    //            else if (y == image_height - 2) { v1 = -1; v2 = -1; }//last row
    //            else { v1 = -1; v2 = 1; };//other row

    //            for (int x = 0; x < image_width; x = x + 2)
    //            {
    //                if (x == 0) { h1 = 1; h2 = 1; }//first column
    //                else if (x == image_width - 2) { h1 = -1; h2 = -1; }//last column
    //                else { h1 = -1; h2 = 1; };//other column

    //                Rdemos[x + image_width * y]
    //                                = Rdata[x / 2 + (image_width / 2) * y / 2];
    //                Rdemos[x + 1 + image_width * y]
    //                   = (ushort)((Rdata[x / 2 + (image_width / 2) * y / 2] + Rdata[x / 2 + h2 + (image_width / 2) * y / 2]) / 2);
    //                Rdemos[x + image_width * (y + 1)]
    //                   = (ushort)((Rdata[x / 2 + (image_width / 2) * y / 2] + Rdata[x / 2 + (image_width / 2) * (y / 2 + v2)]) / 2);
    //                Rdemos[x + 1 + image_width * (y + 1)]
    //                   = (ushort)((Rdata[x / 2 + (image_width / 2) * y / 2] + Rdata[x / 2 + h2 + (image_width / 2) * y / 2] + Rdata[x / 2 + (image_width / 2) * (y / 2 + v2)] + Rdata[x / 2 + h2 + (image_width / 2) * (y / 2 + v2)]) / 4);
    //            }
    //        }
    //        return Rdemos;
    //    }


    //    public ushort[] Interpolation_Gr(ushort[] Grdata)
    //    {
    //        Grdemos = new ushort[image_width * image_height];
    //        int h1, h2, v1, v2;

    //        for (int y = 0; y < image_height; y = y + 2)
    //        {
    //            if (y == 0) { v1 = 1; v2 = 1; }//first row
    //            else if (y == image_height - 2) { v1 = -1; v2 = -1; }//last row
    //            else { v1 = -1; v2 = 1; };//other row

    //            for (int x = 0; x < image_width; x = x + 2)
    //            {
    //                if (x == 0) { h1 = 1; h2 = 1; }//first column
    //                else if (x == image_width - 2) { h1 = -1; h2 = -1; }//last column
    //                else { h1 = -1; h2 = 1; };//other column


    //                Grdemos[x + image_width * y]
    //                    = (ushort)((Grdata[x / 2 + h1 + (image_width / 2) * y / 2] + Grdata[x / 2 + h2 + (image_width / 2) * y / 2]) / 2);
    //                Grdemos[x + 1 + image_width * y]
    //                    = Grdata[x / 2 + (image_width / 2) * y / 2];
    //                Grdemos[x + image_width * (y + 1)]
    //                    = (ushort)((Grdata[x / 2 + h1 + (image_width / 2) * (y / 2)] + Grdata[x / 2 + (image_width / 2) * y / 2] + Grdata[x / 2 + h1 + (image_width / 2) * (y / 2 + v2)] + Grdata[x / 2 + (image_width / 2) * (y / 2 + v2)]) / 4);
    //                Grdemos[x + 1 + image_width * (y + 1)]
    //                    = (ushort)((Grdata[x / 2 + (image_width / 2) * y / 2] + Grdata[x / 2 + (image_width / 2) * y / 2 + v2]) / 2);
    //            }
    //        }
    //        return Grdemos;
    //    }

    //    public ushort[] Interpolation_Gb(ushort[] Gbdata)
    //    {
    //        Gbdemos = new ushort[image_width * image_height];
    //        int h1, h2, v1, v2;

    //        for (int y = 0; y < image_height; y = y + 2)
    //        {
    //            if (y == 0) { v1 = 1; v2 = 1; }//first row
    //            else if (y == image_height - 2) { v1 = -1; v2 = -1; }//last row
    //            else { v1 = -1; v2 = 1; };//other row

    //            for (int x = 0; x < image_width; x = x + 2)
    //            {
    //                if (x == 0) { h1 = 1; h2 = 1; }//first column
    //                else if (x == image_width - 2) { h1 = -1; h2 = -1; }//last column
    //                else { h1 = -1; h2 = 1; };//other column

    //                Gbdemos[x + image_width * y]
    //                    = (ushort)((Gbdata[x / 2 + (image_width / 2) * y / 2] + Gbdata[x / 2 + (image_width / 2) * y / 2 + v2]) / 2);
    //                Gbdemos[x + 1 + image_width * y]
    //                    = (ushort)((Gbdata[x / 2 + (image_width / 2) * y / 2 + v1] + Gbdata[x / 2 + h2 + (image_width / 2) * y / 2 + v1] + Gbdata[x / 2 + (image_width / 2) * y / 2] + Gbdata[x / 2 + h2 + (image_width / 2) * y / 2]) / 4);
    //                Gbdemos[x + image_width * (y + 1)]
    //                    = Gbdata[x / 2 + (image_width / 2) * y / 2];
    //                Gbdemos[x + 1 + image_width * (y + 1)]
    //                    = (ushort)((Gbdata[x / 2 + (image_width / 2) * y / 2] + Gbdata[x / 2 + h2 + (image_width / 2) * y / 2]) / 2);
    //            }
    //        }

    //        return Gbdemos;
    //    }

    //    public ushort[] Interpolation_B(ushort[] Bdata)
    //    {
    //        Bdemos = new ushort[image_width * image_height];
    //        int h1, h2, v1, v2;

    //        for (int y = 0; y < image_height; y = y + 2)
    //        {
    //            if (y == 0) { v1 = 1; v2 = 1; }//first row
    //            else if (y == image_height - 2) { v1 = -1; v2 = -1; }//last row
    //            else { v1 = -1; v2 = 1; };//other row

    //            for (int x = 0; x < image_width; x = x + 2)
    //            {
    //                if (x == 0) { h1 = 1; h2 = 1; }//first column
    //                else if (x == image_width - 2) { h1 = -1; h2 = -1; }//last column
    //                else { h1 = -1; h2 = 1; };//other column

    //                Bdemos[x + image_width * y]
    //                     = (ushort)((Bdata[x / 2 + h1 + (image_width / 2) * y / 2 + v1] + Bdata[x / 2 + (image_width / 2) * y / 2 + v1] + Bdata[x / 2 + h1 + (image_width / 2) * y / 2] + Bdata[x / 2 + (image_width / 2) * y / 2]) / 4);
    //                Bdemos[x + 1 + image_width * y]
    //                    = (ushort)((Bdata[x / 2 + (image_width / 2) * y / 2 + v1] + Bdata[x / 2 + (image_width / 2) * y / 2]) / 2);
    //                Bdemos[x + image_width * (y + 1)
    //                    ] = (ushort)((Bdata[x / 2 + h1 + (image_width / 2) * y / 2] + Bdata[x / 2 + (image_width / 2) * y / 2]) / 2);
    //                Bdemos[x + 1 + image_width * (y + 1)
    //                    ] = Bdata[x / 2 + (image_width / 2) * y / 2];

    //            }
    //        }
    //        return Bdemos;
    //    }

    //    public ushort[] Combine_GrGb(ushort[] Grdemos, ushort[] Gbdemos)
    //    {
    //        Gdemos = new ushort[image_width * image_height];

    //        for (int y = 0; y < image_height; ++y)
    //        {
    //            for (int x = 0; x < image_width; ++x)
    //            {
    //                Gdemos[x + image_width * y] = (ushort)((Grdemos[x + image_width * y] + Gbdemos[x + image_width * y]) / 2);
    //            }
    //        }

    //        return Gdemos;
    //    }

    //    public ushort[] Create_Color_Array(ushort[] Rdemos, ushort[] Gdemos, ushort[] Bdemos)
    //    {
    //        color = new ushort[image_width * 3 * image_height];

    //        for (int j = 0; j < image_height; j++)
    //        {
    //            for (int i = 0; i < image_width; i++)
    //            {
    //                color[i * 3 + j * image_width * 3] = Rdemos[i + image_width * j];
    //                color[i * 3 + 1 + j * image_width * 3] = Gdemos[i + image_width * j];
    //                color[i * 3 + 2 + j * image_width * 3] = Bdemos[i + image_width * j];
    //            }
    //        }

    //        return color;
    //    }

    //    public ushort[] Demosaic()
    //    {
    //        //Interpolation
    //        Rdemos = Interpolation_R(Rdata);
    //        Grdemos = Interpolation_Gr(Grdata);
    //        Gbdemos = Interpolation_Gb(Gbdata);
    //        Bdemos = Interpolation_B(Bdata);
    //        Console.Write("補間完了\n");

    //        //Combine GrGb
    //        Gdemos = Combine_GrGb(Grdemos, Gbdemos);

    //        //Set to Color Image Array
    //        color = Create_Color_Array(Rdemos, Gdemos, Bdemos);
    //        Console.Write("カラー配列格納完\n");

    //        return color;
    //    }

    //    public ushort[] ComplessColorImage()
    //    {
    //        color_small = new ushort[image_width / dwnscale * 3 * image_height / dwnscale];

    //        for (int j = 0; j < image_height / dwnscale; j++)
    //        {
    //            for (int i = 0; i < image_width / dwnscale; i++)
    //            {
    //                color_small[i * 3 + j * image_width / dwnscale * 3] = color[i * 3 * dwnscale + image_width * j * 3 * dwnscale];
    //                color_small[i * 3 + 1 + j * image_width / dwnscale * 3] = color[i * 3 * dwnscale + 1 + image_width * j * 3 * dwnscale];
    //                color_small[i * 3 + 2 + j * image_width / dwnscale * 3] = color[i * 3 * dwnscale + 2 + image_width * j * 3 * dwnscale];
    //            }
    //        }

    //        return color_small;
    //    }

    //    public void Save_Demosaiced_Imege()
    //    {
    //        SaveImage("Rdemos.tif", Rdemos, image_width, image_height, PixelFormats.Gray16);
    //        SaveImage("Gdemos.tif", Gdemos, image_width, image_height, PixelFormats.Gray16);
    //        SaveImage("Bdemos.tif", Bdemos, image_width, image_height, PixelFormats.Gray16);
    //        SaveImage("color.tif", color, image_width, image_height, PixelFormats.Gray16);
    //    }

    //    public void SaveImage(string filename, Array bmpData, int width, int height, System.Windows.Media.PixelFormat format, double dpi = 96)
    //    {

    //        int stride = ((width * format.BitsPerPixel) / 8);

    //        System.Windows.Media.Imaging.BitmapSource bs = System.Windows.Media.Imaging.BitmapSource.Create(
    //            width,
    //            height,
    //            dpi,
    //            dpi,
    //            format,
    //            null,
    //            bmpData,
    //            stride);

    //        // ファイル名の拡張子
    //        var ext = System.IO.Path.GetExtension(filename).ToLower();

    //        BitmapEncoder encoder;

    //        switch (ext)
    //        {
    //            case ".bmp":
    //                encoder = new BmpBitmapEncoder();
    //                break;

    //            case ".tif":
    //                encoder = new TiffBitmapEncoder { Compression = TiffCompressOption.None };
    //                break;

    //            case ".png":
    //                encoder = new PngBitmapEncoder();
    //                break;
    //            default:
    //                encoder = null;
    //                break;
    //        }

    //        using (Stream stream = new FileStream(filename, FileMode.Create))
    //        {
    //            var bf = BitmapFrame.Create(bs);
    //            encoder.Frames.Add(bf);
    //            encoder.Save(stream);
    //        }

    //    }

    //}



    public class basic_process
    {

        static private int image_width;
        static private int image_height;
        static private int dwnscale;
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

        public ushort[] GainOffset(ushort[] color, int dwnscale, double Offset, double R_Offset, double G_Offset, double B_Offset
                  , double Gain, double R_Gain, double G_Gain, double B_Gain, double gamma)
        {
            double added_R, added_G, added_B;

            for (int y = 0; y <= image_height/ dwnscale - 1; ++y)
            {
                for (int x = 0; x <= image_width / dwnscale - 1; ++x)
                {
                    checked
                    {
                        added_R = (double)color[x * 3 + image_width / dwnscale * 3 * y] * Gain * R_Gain + Offset + R_Offset;//R
                        added_G = (double)color[x * 3 + 1 + image_width / dwnscale * 3 * y] * Gain * G_Gain + Offset + G_Offset;//G
                        added_B = (double)color[x * 3 + 2 + image_width / dwnscale * 3 * y] * Gain * B_Gain + Offset + B_Offset;//B

                        added_R = Clip_16bit(added_R);
                        added_G = Clip_16bit(added_G);
                        added_B = Clip_16bit(added_B);

                        added_R = GammaCorrection(added_R, gamma);
                        added_G = GammaCorrection(added_G, gamma);
                        added_B = GammaCorrection(added_B, gamma);

                        color2_small[x * 3 + image_width / dwnscale * 3 * y] = (ushort)(added_R);
                        color2_small[x * 3 + 1 + image_width / dwnscale * 3 * y] = (ushort)(added_G);
                        color2_small[x * 3 + 2 + image_width / dwnscale * 3 * y] = (ushort)(added_B);

                    }
                }

            }
            return color2_small;
        }

        public ushort[] GainOffset2(ushort[] color, int dwnscale, double Offset, double R_Offset, double G_Offset, double B_Offset
                  , double Gain, double R_Gain, double G_Gain, double B_Gain, double gamma)
        {
            double added_R, added_G, added_B;

            for (int y = 0; y <= image_height / dwnscale - 1; ++y)
            {
                for (int x = 0; x <= image_width / dwnscale - 1; ++x)
                {
                    checked
                    {
                        added_R = (double)color[x * 3 + image_width / dwnscale * 3 * y] * Gain * R_Gain + Offset + R_Offset;//R
                        added_G = (double)color[x * 3 + 1 + image_width / dwnscale * 3 * y] * Gain * G_Gain + Offset + G_Offset;//G
                        added_B = (double)color[x * 3 + 2 + image_width / dwnscale * 3 * y] * Gain * B_Gain + Offset + B_Offset;//B

                        added_R = Clip_16bit(added_R);
                        added_G = Clip_16bit(added_G);
                        added_B = Clip_16bit(added_B);

                        added_R = GammaCorrection(added_R, gamma);
                        added_G = GammaCorrection(added_G, gamma);
                        added_B = GammaCorrection(added_B, gamma);

                        color2[x * 3 + image_width / dwnscale * 3 * y] = (ushort)(added_R);
                        color2[x * 3 + 1 + image_width / dwnscale * 3 * y] = (ushort)(added_G);
                        color2[x * 3 + 2 + image_width / dwnscale * 3 * y] = (ushort)(added_B);

                    }
                }

            }
            return color2;
        }

        public ushort[] GetColor()
        {
            return color2;
        }

        public async Task GetColor_Process(ushort[] color, int dwnscale, double Offset, double R_Offset, double G_Offset, double B_Offset
                  , double Gain, double R_Gain, double G_Gain, double B_Gain, double gamma)
        {
            await GainOffset_RGB(color, dwnscale, Offset, R_Offset, G_Offset, B_Offset, Gain, R_Gain, G_Gain, B_Gain, gamma);
            Console.Write("タスク完了になってる?\n");
        }

        public async Task GainOffset_RGB(ushort[] color, int dwnscale, double Offset, double R_Offset, double G_Offset, double B_Offset
                  , double Gain, double R_Gain, double G_Gain, double B_Gain, double gamma)
        {
            double C_Offset=0;
            double C_Gain = 0;
            int addr_offset = 0;

            var tasks = new List<Task>();
            for (int i = 0; i < 3; i++)
            {
                if (i == 0) {C_Offset = R_Offset; C_Gain = R_Gain; addr_offset = 0; }
                else if (i == 1) { C_Offset = G_Offset; C_Gain = G_Gain; addr_offset = 1; }
                else { C_Offset = B_Offset; C_Gain = B_Gain; addr_offset = 2; }

                var prcTask = Task.Run(async() =>
                     await GainOffset_loop(addr_offset, color, dwnscale, Offset, C_Offset, Gain, C_Gain, gamma));
                tasks.Add(prcTask);                
            }
            await Task.WhenAll(tasks);
            Console.Write("タスク完了\n");
        }


        private async Task GainOffset_loop(int addr_offset, ushort[] color, int dwnscale, double Offset, double C_Offset, double Gain, double C_Gain, double gamma)
        {
            
            for (int y = 0; y <= image_height / dwnscale - 1; ++y)
            {
                for (int x = 0; x <= image_width / dwnscale - 1; ++x)
                {
                    color2[x * 3 + addr_offset + image_width / dwnscale * 3 * y]
                        = GainOffset_U(color[x * 3 + addr_offset + image_width / dwnscale * 3 * y],dwnscale 
                                           ,Offset, C_Offset, Gain, C_Gain, gamma);
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


        public ushort[] Pixel2ColorArray(double R, double G, double B)
        {
            ushort[] color_image = new ushort[image_width / dwnscale * 3 * image_height / dwnscale];
            for (int y = 0; y <= image_height / dwnscale - 1; ++y)
            {
                for (int x = 0; x <= image_width / dwnscale - 1; ++x)
                {
                    color_image[x * 3 + image_width / dwnscale * 3 * y] = (ushort)(R);
                    color_image[x * 3 + 1 + image_width / dwnscale * 3 * y] = (ushort)(G);
                    color_image[x * 3 + 2 + image_width / dwnscale * 3 * y] = (ushort)(B);
                }
            }

            return color_image;
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

        static private int image_width;
        static private int image_height;
        static private int dwnscale;
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
       
        public unsafe ushort GainOffset(ushort* color, int dwnscale, double Offset, double C_Offset
                  , double Gain, double C_Gain, double gamma)

        {
            double added;
            ushort added_16bit;
            checked
            {
                added = color[0] * Gain * C_Gain + Offset + C_Offset;
                added = Clip_16bit(added);
                added = GammaCorrection(added, gamma);
                added = Clip_16bit(added);
                added_16bit = (ushort)added;
            }
            return added_16bit;
        }

        public ushort[] GainOffset2(ushort[] color, int dwnscale, double Offset, double R_Offset, double G_Offset, double B_Offset
                  , double Gain, double R_Gain, double G_Gain, double B_Gain, double gamma)
        {
            double added_R, added_G, added_B;

            for (int y = 0; y <= image_height / dwnscale - 1; ++y)
            {
                for (int x = 0; x <= image_width / dwnscale - 1; ++x)
                {
                        added_R = (double)color[x * 3 + image_width / dwnscale * 3 * y] * Gain * R_Gain + Offset + R_Offset;//R
                        added_G = (double)color[x * 3 + 1 + image_width / dwnscale * 3 * y] * Gain * G_Gain + Offset + G_Offset;//G
                        added_B = (double)color[x * 3 + 2 + image_width / dwnscale * 3 * y] * Gain * B_Gain + Offset + B_Offset;//B

                        added_R = Clip_16bit(added_R);
                        added_G = Clip_16bit(added_G);
                        added_B = Clip_16bit(added_B);

                        added_R = GammaCorrection(added_R, gamma);
                        added_G = GammaCorrection(added_G, gamma);
                        added_B = GammaCorrection(added_B, gamma);

                        color2[x * 3 + image_width / dwnscale * 3 * y] = (ushort)(added_R);
                        color2[x * 3 + 1 + image_width / dwnscale * 3 * y] = (ushort)(added_G);
                        color2[x * 3 + 2 + image_width / dwnscale * 3 * y] = (ushort)(added_B);
                }

            }
            return color2;
        }

        public ushort[] Pixel2ColorArray(double R, double G, double B)
        {
            ushort[] color_image = new ushort[image_width / dwnscale * 3 * image_height / dwnscale];
            for (int y = 0; y <= image_height / dwnscale - 1; ++y)
            {
                for (int x = 0; x <= image_width / dwnscale - 1; ++x)
                {
                    color_image[x * 3 + image_width / dwnscale * 3 * y] = (ushort)(R);
                    color_image[x * 3 + 1 + image_width / dwnscale * 3 * y] = (ushort)(G);
                    color_image[x * 3 + 2 + image_width / dwnscale * 3 * y] = (ushort)(B);
                }
            }

            return color_image;
        }

        public double GammaCorrection(double val, double gamma)
        {
            double gamma_val = Math.Pow(val, gamma) / (Math.Pow(65535, gamma)) * 65535;
            double gamma_val_clip = Clip_16bit(gamma_val);
            return gamma_val;
        }

        public double Clip_16bit(double val)
        {

            if (val > 65535) val = 65535;
            else if (val < 0) val = 0;

            return val;
        }


    }
}
