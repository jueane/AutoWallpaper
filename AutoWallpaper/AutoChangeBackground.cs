using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO;

namespace AutoWallpaper
{
    class AutoChangeBackground
    {
        static string[] files;
        //static int cur = 0;
        public static void Begin()
        {
            string path = Directory.GetCurrentDirectory();
            while (true)
            {
                var ret = RenewFiles();
                if (!ret)
                {
                    return;
                }
                var data = DataStore.Read();
                if (data.stop)
                {
                    Thread.Sleep(1);
                    continue;
                }
                var cur = data.currentNumber;
                if (cur >= files.Length)
                {
                    cur = files.Length - 1;
                }
                SetWallPaper.Set(path + "/" + files[cur]);
                data.lastImage = data.currentImage;
                data.currentImage = files[cur];
                DataStore.Write(data);
                cur++;
                if (cur >= files.Length)
                {
                    RenewFiles();
                    ret = RenewFiles();
                    if (!ret)
                    {
                        return;
                    }
                    cur = 0;
                }
                data.currentNumber = cur;
                DataStore.Write(data);
                Thread.Sleep(1000 * DataStore.Read().interval);
            }

        }

        static bool RenewFiles()
        {
            try
            {
                files = Directory.GetFiles("wallpaper", "*", SearchOption.AllDirectories);
                if (files.Length < 0)
                {
                    Console.WriteLine("No images");
                    return false;
                }
                return true;
            }
            catch (DirectoryNotFoundException ex)
            {
                Console.WriteLine("Not wallpaper folder");
                return false;
            }
        }


    }
}
