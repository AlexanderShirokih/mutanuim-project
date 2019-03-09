using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System;
using UnityEngine;

namespace Mutanium
{
    [Serializable]
    public class GameSave
    {
        public float GameTime { get; set; }

        public void SaveGame(GameSave gameSave)
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Create(Application.persistentDataPath + "/save.dat");
            bf.Serialize(file, gameSave);
            file.Close();
        }

        public static GameSave LoadGame()
        {
            if (File.Exists(Application.persistentDataPath + "/save.dat"))
            {
                BinaryFormatter bf = new BinaryFormatter();
                FileStream file = File.Open(Application.persistentDataPath + "/save.dat", FileMode.Open);
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
                    Current = LoadGame();
                }
                return Current;
            }
        }

        public static GameSave Current { get; private set; }
    }
}