using System.IO;
using System.Xml.Serialization;
using System;

namespace Mutanium
{
    [Serializable]
    public class Global
    {
        [NonSerialized]
        public string savePath = "save.dat";
        public float GameTime { get; set; }
        public int LastUniqueId { get; set; }

        public void Save()
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(Global));
            FileStream file = File.Create(savePath);
            xmlSerializer.Serialize(file, this);
            file.Close();
        }

        public static void LoadOrCreate(string savePath)
        {
            if (File.Exists(savePath))
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(Global));
                FileStream file = File.Open(savePath, FileMode.Open);
                Global gameSave = (Global)xmlSerializer.Deserialize(file);
                file.Close();
                Instance = gameSave;
            }
            else
            {
                Instance = new Global();
            }

            Instance.savePath = savePath;
        }

        public static Global Instance { get; private set; } = new Global();

        public static void SetCurrent(Global global)
        {
            Instance = global;
        }
    }
}