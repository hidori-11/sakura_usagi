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
        public Settings Settings { set; get; }

        public SettingLoader()
        {

            if (File.Exists(settingFilePath))
            {
                string setting_string = File.ReadAllText(settingFilePath);
                Settings = JsonConvert.DeserializeObject<Settings>(setting_string);
            }
            else
            {
                Settings = new Settings
                {
                    CachePath = "Caches",
                    HomePage = "https://google.com",
                    UserDataPath = "UserDatas",
                    Favorites = new Dictionary<string, string> { }
                };

                string default_json = JsonConvert.SerializeObject(Settings, Formatting.Indented);
                File.WriteAllText(settingFilePath, default_json);
            }
        }

        public void SaveSetting()
        {
            string default_json = JsonConvert.SerializeObject(Settings, Formatting.Indented);
            File.WriteAllText(settingFilePath, default_json);
        }
    }

    [JsonObject]
    public class Settings
    {
        [JsonProperty("CachePath")]
        public string CachePath { get; set; }

        [JsonProperty("UserDataPath")]
        public string UserDataPath { get; set; }

        [JsonProperty("HomePage")]
        public string HomePage { get; set; }

        [JsonProperty("Favorite")]
        public Dictionary<string, string> Favorites { get; set; }

    }
}
