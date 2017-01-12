using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResizeSprites {
    class Program {
        static void Main(string[] args) {

            // Get our paths and locations for easy access later.
            var curpath = Environment.CurrentDirectory;
            var inpath = Path.Combine(curpath, "input");
            var outpath = Path.Combine(curpath, "output");

            // Make sure everything exists.
            if (!Directory.Exists(inpath)) Directory.CreateDirectory(inpath);
            if (!Directory.Exists(outpath)) Directory.CreateDirectory(outpath);

            // Tell our user what to do next.
            Console.WriteLine("Please place your RPG Maker VX Formatted sprites in the input folder where this application resides.");
            Console.WriteLine("Once you've done this, hit any key to continue.");
            Console.WriteLine(String.Empty);
            Console.ReadKey();

            // Ask our user what to use to resize it.
            Console.WriteLine("How much larger do our sprites need to be become? (1 = 100%, 2 = 200%, 2.5 = 250% etc.)");
            var modifier = Single.Parse(Console.ReadLine());
            Console.WriteLine(String.Empty);

            // Process our images.
            foreach (var file in new DirectoryInfo(inpath).EnumerateFiles("*.png")) {
                using (var tmp = Image.FromFile(file.FullName)) {
           
                    using (var newimage = new Bitmap((Int32)(tmp.Width * modifier), (Int32)(tmp.Height * modifier), PixelFormat.Format32bppArgb)) {
                        using (var g = Graphics.FromImage(newimage)) {
                            g.InterpolationMode = InterpolationMode.NearestNeighbor;
                            g.CompositingMode = CompositingMode.SourceCopy;
                            g.CompositingQuality = CompositingQuality.HighQuality;
                            g.PixelOffsetMode = PixelOffsetMode.HighQuality;

                            g.Clear(Color.Transparent);
                            g.DrawImage(tmp, new Rectangle(0, 0, newimage.Width, newimage.Height), new Rectangle(0, 0, tmp.Width, tmp.Height), GraphicsUnit.Pixel);
                            g.Flush();
                        }
                        newimage.Save(Path.Combine(outpath, file.Name), ImageFormat.Png);
                        Console.WriteLine(String.Format("Processed {0}.", file.Name));
                    }
                }
            }

            // notify the user we're done.
            Console.WriteLine(String.Empty);
            Console.WriteLine("All files have been processed, hit any key to close the application.");
            Console.ReadKey();

        }
    }
}
