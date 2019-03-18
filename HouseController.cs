using System.Collections;
using System.Linq;
using UnityEngine;

using Mutanium.Utils;
using Mutanium.Human;

namespace Mutanium
{
    public class HouseController : MonoBehaviour
    {
        private Spawner spawner;
        private const float LAZY_UPDATE_PERIOD = 4f;

        public HouseInfo House { get; set; }

        public float maxHumanCapacity = 3;

        void Start()
        {
            spawner = GameObject.Find("Spawner").GetComponent<Spawner>();
            StartCoroutine(_LazyCoroutine());
        }

        IEnumerator _LazyCoroutine()
        {
            while (House.health > 0.0f)
            {
                OnLazyUpdate();
                yield return new WaitForSeconds(LAZY_UPDATE_PERIOD);
            }
        }

        void OnLazyUpdate()
        {
            if (House.humans.Count < maxHumanCapacity)
            {
                //TODO: Пересмотреть алогоритм спавнинга
                //Определить когда спавнить новых humans

                //Сперва заселяем жителей без домов
                HumanInfo unsetted = UniqueIdDatabase.FindByType(typeof(HumanInfo)).Cast<HumanInfo>().FirstOrDefault(h => h.AssignedHouse == null);
                if (unsetted != null)
                {
                    House.humans.Add(unsetted.ReferencedId);
                    unsetted.AssignedHouse = House.ReferencedId;
                }
                else if (RandomUtils.GetRandomBool())
                {
                    //Если жителей без домов нет, то спавним нового жителя
                    SpawnHuman();
                }
            }
        }

        private void SpawnHuman()
        {
            float rad = MeasurementUtils.GetModelRadius(gameObject);
            Vector3 spawnPosition = transform.position + (transform.rotation * Vector3.forward * rad);

            HumanInfo human = new HumanInfo
            {
                BirthDate = new Date(),
                IsMen = RandomUtils.GetRandomBool(),
                EulerRotation = Vector3.zero,
                Position = spawnPosition,
                AssignedHouse = House.ReferencedId
            };
            spawner.SpawnHuman(human);

            House.humans.Add(human.ReferencedId);
        }
    }
}
