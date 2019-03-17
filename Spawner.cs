using UnityEngine;

using Mutanium.Human;
using Random = UnityEngine.Random;

namespace Mutanium
{
    public class Spawner : MonoBehaviour
    {
        private readonly Color GizmosColor = new Color(1, 1, 0, 0.75F);

        public GameObject[] houseTypes;
        public GameObject[] humanTypes;
        public Transform playersRoot;

        void Awake()
        {
            HumanManager.Instance.Spawner = this;
        }

        public void SpawnHouse(HouseInfo house)
        {
            GameObject housePrefab = houseTypes[house.type];
            GameObject instance = Instantiate(housePrefab, house.position, Quaternion.identity);
            instance.GetComponent<HouseController>().House = house;
        }

        public void SpawnHuman(HumanInfo human)
        {
            GameObject instance = Instantiate(humanTypes[0], human.position, Quaternion.Euler(human.eulerRotation), playersRoot);
            instance.GetComponent<HumanController>().Human = human;
        }

        internal Vector3 RandomSpawningPosition()
        {
            Vector3 v = new Vector3();
            float t;

            t = transform.localScale.x / 2f;
            v.x = Random.Range(transform.position.x - t, t + transform.position.x);
            t = transform.localScale.y / 2f;
            v.z = Random.Range(transform.position.z - t, t + transform.position.z);
            v.y = GetTerrainHeightAt(v.x, v.z);
            return v;
        }


        private float GetTerrainHeightAt(float x, float z)
        {
            //TODO: Temp varaint
            return 7.9f;
        }

        void OnDrawGizmosSelected()
        {
            Gizmos.color = GizmosColor;
            Gizmos.DrawCube(transform.position, transform.localScale);
        }
    }
}