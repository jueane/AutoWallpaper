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
        public static void Begin(int interval)
        {
            var files = Directory.GetFiles("wallpaper", "*", SearchOption.AllDirectories);

            string path = Directory.GetCurrentDirectory();
            int cur = 0;
            while (true)
            {
                SetWallPaper.Set(path + "/" + files[cur]);
                cur++;
                if (cur >= files.Length)
                {
                    cur = 0;
                }
                Thread.Sleep(interval);
            }

        }


    }
}
