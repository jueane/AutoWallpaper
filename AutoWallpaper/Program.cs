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
            string url = "https://image.baidu.com/search/index?ct=&z=&tn=baiduimage&ipn=r&word=%E5%A3%81%E7%BA%B8&pn=0&istype=2&ie=utf-8&oe=utf-8&cl=&lm=-1&st=-1&fr=&fmq=1536207041692_R&ic=0&se=&sme=&width=1920&height=1080&face=0";

            var baidu = new CollectFromWeb();
            var urls = baidu.GetURLs(url, WebRegex.BAIDU);

            SimpleAsyncDownload down = new SimpleAsyncDownload();
            var files = down.Download(urls);

        }
    }
}
