using System;
using UnityEngine;

namespace Mutanium
{
    [Serializable]
    public class UniqueElement
    {
        private UniqueId _uniqueId;

        public UniqueId Id
        {
            get
            {
                if (_uniqueId == null)
                {
                    _uniqueId = new UniqueId();
                    LinkUniqueId(_uniqueId);
                }
                return _uniqueId;
            }
            set
            {
                _uniqueId = value;
                LinkUniqueId(_uniqueId);
            }
        }

        protected void LinkUniqueId(UniqueId id)
        {
            UniqueIdDatabase.Link(this, id);
        }

        public void Destroy()
        {
            UniqueIdDatabase.UnLink(Id);
        }
    }
}
