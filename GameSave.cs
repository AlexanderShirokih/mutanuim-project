using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System;
using UnityEngine;

namespace Mutanium
{
    [Serializable]
    public class GameSave
    {
        public static string savePath = "save.dat";
        public float GameTime { get; set; }
        public int LastUniqueId { get; set; }

        public void Save(GameSave gameSave)
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Create(savePath);
            bf.Serialize(file, gameSave);
            file.Close();
        }

        public static GameSave Load()
        {
            if (File.Exists(savePath))
            {
                BinaryFormatter bf = new BinaryFormatter();
                FileStream file = File.Open(savePath, FileMode.Open);
                GameSave gameSave = (GameSave)bf.Deserialize(file);
                file.Close();
                return gameSave;
            }
            return new GameSave();
        }

        public static GameSave Instance
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

        public static GameSave Current { get; set; }
    }
}