using System;
using System.Collections.Generic;

using UnityEngine;
using Mutanium.Human;

namespace Mutanium
{
    [Serializable]
    public class HouseInfo : UniqueElement
    {
        public byte type;
        public Vector3 position;
        public float health = 1.0f;
        public HashSet<ReferencedId<HumanInfo>> humans = new HashSet<ReferencedId<HumanInfo>>();

        public ReferencedId<HouseInfo> ReferencedId => new ReferencedId<HouseInfo>
        {
            RefId = Id
        };
    }
}
