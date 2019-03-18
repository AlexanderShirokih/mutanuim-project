using System.Collections;
using Mutanium.Utils;
using UnityEngine;

namespace Mutanium.Human
{
    public class HumanManager
    {
        private static HumanManager instance;
        private ArrayList humansList = new ArrayList();

        internal Spawner Spawner { private get; set; }

        public static HumanManager Instance
        {
            get { if (instance == null) instance = new HumanManager(); return instance; }

        }

        public HumanInfo[] GetHumanInfos()
        {
            return (HumanInfo[])humansList.ToArray(typeof(HumanInfo));
        }

        public ArrayList GetHumanList()
        {
            return ArrayList.ReadOnly(humansList);
        }

        public void SetHumanInfos(HumanInfo[] infos)
        {
            humansList.Clear();
            humansList.AddRange(infos);
            foreach (var human in infos)
            {
                Spawner.SpawnHuman(human);
            }
        }

        public HumanInfo Spawn()
        {
            HumanInfo humanInfo = new HumanInfo
            {
                BirthDate = new Date(),
                IsMen = RandomUtils.GetRandomBool(),
                eulerRotation = Vector3.zero,
                position = Spawner.RandomSpawningPosition()
            };
            humansList.Add(humanInfo);
            Spawner.SpawnHuman(humanInfo);
            return humanInfo;
        }
    }
}

