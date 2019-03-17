using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System;

namespace Mutanium
{
    [Serializable]
    public class Global
    {
        public static string savePath = "save.dat";
        public float GameTime { get; set; }
        public int LastUniqueId { get; set; }

        public void Save()
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Create(savePath);
            bf.Serialize(file, this);
            file.Close();
        }

        public static Global Load()
        {
            if (File.Exists(savePath))
            {
                BinaryFormatter bf = new BinaryFormatter();
                FileStream file = File.Open(savePath, FileMode.Open);
                Global gameSave = (Global)bf.Deserialize(file);
                file.Close();
                return gameSave;
            }
            return new Global();
        }

        public static Global Instance
        {
            get
            {
                if (Current == null)
                {
                    Current = Load();
                }
                return Current;
            }
        }

        public static Global Current { get; set; }
    }
}