
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BrickBreaker
{
    internal class ReadGameFile
    {
        public GameInfo ReadSettingFile()
        {
            GameInfo gameInfo;
            string path = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "\\gameInfo.txt";
            using (StreamReader reader = new StreamReader(path))
            {
                string str = reader.ReadToEnd();
                //gameInfo = JsonConvert.DeserializeObject<GameInfo>(str);
                gameInfo = JsonSerializer.Deserialize<GameInfo>(str);

            }
            return gameInfo;
        }
        public void WriteSettingFile(GameInfo gameInfo)
        {
            string path = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "\\gameInfo.txt";
            //string settingString = JsonConvert.SerializeObject(gameInfo);
            string settingString = JsonSerializer.Serialize(gameInfo);
            using (StreamWriter writer = new StreamWriter(path))
            {
                writer.Write(settingString);               
            }
        }
    }
}
