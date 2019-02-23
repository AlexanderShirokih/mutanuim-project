using System.Collections;
using UnityEngine;

namespace Mutanium
{

    public class AnimalSpawnZone : MonoBehaviour
    {
        private readonly Color GizmosColor = new Color(1, 1, 0, 0.75F);
        private const float CHECKING_BASE_DELAY = 5f;
        private bool coroutineStarted;

        public int animalsCount = 10;
        public GameObject animalGameObject;

        private ArrayList spawnedAnimals;

        public void Start()
        {
            Load();
        }

        public void Update()
        {
            if (!coroutineStarted)
            {
                coroutineStarted = true;
                StartCoroutine(CheckForSpawn());
            }
        }

        private IEnumerator CheckForSpawn()
        {
            yield return new WaitForSeconds(CHECKING_BASE_DELAY + Random.value);
            if (spawnedAnimals.Count < animalsCount)
            {
                Instantiate(animalGameObject, GetSpawnPosition(), Quaternion.identity);
            }
            coroutineStarted = false;
        }

        private Vector3 GetSpawnPosition()
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
            return 4.0f;
        }

        private void Store()
        {

        }

        private void Load()
        {
            spawnedAnimals = new ArrayList(animalsCount);
        }

        void OnDrawGizmosSelected()
        {
            Gizmos.color = GizmosColor;
            Gizmos.DrawCube(transform.position, transform.localScale);
        }
    }
}
