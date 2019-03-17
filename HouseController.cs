using System.Collections;
using System.Linq;
using UnityEngine;

using Mutanium.Utils;
using Mutanium.Human;

namespace Mutanium
{
    public class HouseController : MonoBehaviour
    {
        private const float LAZY_UPDATE_PERIOD = 4f;

        public HouseInfo House { get; set; }

        public float maxHumanCapacity = 3;

        void Start()
        {
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
                var query = from HumanInfo human in HumanManager.Instance.GetHumanList()
                            where human.AssignedHouse == null
                            select human;
                HumanInfo unsetted = query.FirstOrDefault();
                if (unsetted != null)
                    AddHuman(unsetted);
                else if (RandomUtils.GetRandomBool())
                {
                    //Если жителей без домов нет, то спавним нового жителя
                    HumanInfo human = HumanManager.Instance.Spawn();
                    AddHuman(human);
                }
            }
        }

        private void AddHuman(HumanInfo hi)
        {
            hi.AssignedHouse = House.id;
            House.humans.Add(hi.Id);
        }
    }
}
