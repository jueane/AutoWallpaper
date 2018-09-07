using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using Newtonsoft.Json;

namespace AutoWallpaper
{
    class DataStore
    {
        static string settingFilename = "setting.ini";
        public static SimpleSetting Read()
        {
            SimpleSetting ss = null;
            try
            {
                string json = File.ReadAllText(settingFilename);
                try
                {
                    ss = JsonConvert.DeserializeObject<SimpleSetting>(json);
                }
                catch (Exception ex)
                {
                    ss = new SimpleSetting();
                    Write(ss);
                }
            }
            catch (FileNotFoundException ex)
            {
                ss = new SimpleSetting();
                Write(ss);
            }
            return ss;
        }

        public static void Write(SimpleSetting ss)
        {
            var json = JsonConvert.SerializeObject(ss);
            File.WriteAllText(settingFilename, json);
        }
    }

    [Serializable]
    class SimpleSetting
    {
        public bool stop = false;
        public int interval = 5;
        public int currentNumber = 0;
        public string lastImage;
        public string currentImage;
    }

}
