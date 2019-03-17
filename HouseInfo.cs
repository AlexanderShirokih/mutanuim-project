using System;
using System.Collections.Generic;

using UnityEngine;

namespace Mutanium
{
    [Serializable]
    public class HouseInfo
    {
        public UniqueId id;
        public byte type;
        public Vector3 position;
        public float health = 1.0f;
        public HashSet<UniqueId> humans = new HashSet<UniqueId>();
    }
}
