
using System.IO;
using System.Collections;
using System.Xml.Serialization;
using System.Linq;

using UnityEngine;
using Mutanium.Human;
using System;

namespace Mutanium
{
    public class SceneSerializator : MonoBehaviour
    {
        private const string SAVE_FILENAME = "save.xml";
        private const float AUTOSAVE_PERIOD = 15f;
        private readonly XmlSerializer SERIALIZER = new XmlSerializer(typeof(SaveFile));

        public int levelId;

        public Spawner spawner;

        void Awake()
        {
            //Application.persistentDataPath +"/save.dat";
            Global.LoadOrCreate("game.xml");
            UniqueIdDatabase.Clear();
            Load();
        }

        void Start()
        {
            StartCoroutine(AutoSave());
        }

        IEnumerator AutoSave()
        {
            while (true)
            {
                yield return new WaitForSeconds(AUTOSAVE_PERIOD);
                Debug.Log("Saving...");
                Save();
                Global.Instance.Save();
            }
        }

        public void Save()
        {
            SaveFile save = new SaveFile
            {
                HouseInfos = GetHouseInfos(),
                Humans = UniqueIdDatabase.FindByType(typeof(HumanInfo)).Cast<HumanInfo>().ToArray()
            };

            using (FileStream fs = new FileStream(SAVE_FILENAME, FileMode.Create))
            {
                SERIALIZER.Serialize(fs, save);
            }
        }

        public void Load()
        {
            if (File.Exists(SAVE_FILENAME))
                using (FileStream fs = new FileStream(SAVE_FILENAME, FileMode.Open))
                {
                    SaveFile save = (SaveFile)SERIALIZER.Deserialize(fs);
                    SetHouseInfos(save.HouseInfos);
                    SetHumanInfos(save.Humans);
                }
        }

        internal HouseInfo[] GetHouseInfos()
        {
            GameObject[] houses = GameObject.FindGameObjectsWithTag("House");
            HouseInfo[] houseInfos = new HouseInfo[houses.Length];

            for (int i = 0; i < houses.Length; i++)
            {
                houseInfos[i] = houses[i].GetComponent<HouseController>().House;
            }
            return houseInfos;
        }

        private void SetHouseInfos(HouseInfo[] houseInfos)
        {
            foreach (HouseInfo house in houseInfos)
            {
                if (house == null) continue;
                spawner.SpawnHouse(house);
            }
        }

        private void SetHumanInfos(HumanInfo[] humans)
        {
            foreach (var human in humans)
            {
                if (human == null) continue;
                spawner.SpawnHuman(human);
            }
        }
    }
}