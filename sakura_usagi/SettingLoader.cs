using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace sakura_usagi
{
    public class SettingLoader
    {
        private string settingFilePath = "setting.json";
        public string CachePath { get; set; }
        public string UserDataPath { get; set; }
        public string HomePage { get; set; }

        public SettingLoader()
        {
            if (File.Exists(settingFilePath))
            {
                string setting_string = File.ReadAllText(settingFilePath);
                var settings = JsonConvert.DeserializeObject<Settings>(setting_string);
                
                UserDataPath = settings.UserDataPath;
                HomePage = settings.HomePage;
                CachePath = settings.CachePath;
            }
            else
            {
                Settings settings = new Settings
                {
                    CachePath = "Caches",
                    HomePage = "https://google.com",
                    UserDataPath = "UserDatas"
                };

                string default_json = JsonConvert.SerializeObject(settings, Formatting.Indented);
                File.WriteAllText(settingFilePath, default_json);

                UserDataPath = settings.UserDataPath;
                HomePage = settings.HomePage;
                CachePath = settings.CachePath;
            }
        }
    }

    [JsonObject]
    class Settings
    {
        [JsonProperty("CachePath")]
        public string CachePath { get; set; }

        [JsonProperty("UserDataPath")]
        public string UserDataPath { get; set; }

        [JsonProperty("HomePage")]
        public string HomePage { get; set; }
    }
}
