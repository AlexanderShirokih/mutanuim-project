using System;

namespace Mutanium.Animal
{
    [Serializable]
    public class AnimalInfo
    {
        /// <summary>
        /// Animal health level. Ranged [0..1]. If less then 0 animal dies.
        /// </summary>
        public float health = 1.0f;

    }
}
