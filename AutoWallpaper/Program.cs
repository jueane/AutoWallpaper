using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoWallpaper
{
    class Program
    {
        static void Main(string[] args)
        {
            ImageHunter ih = new ImageHunter();
            ih.Hunter().Wait();

        }
    }
}
