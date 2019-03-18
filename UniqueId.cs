using System;

namespace Mutanium
{
    [Serializable]
    public class UniqueId
    {
        private int _id;

        public int UId
        {
            get
            {
                if (_id == 0)
                    _id = GetNextId();
                return _id;
            }
            set => _id = value;
        }

        private static int GetNextId()
        {
            Global save = Global.Instance;
            var id = save.LastUniqueId + 1;
            save.LastUniqueId = id;
            return id;
        }
    }
}
