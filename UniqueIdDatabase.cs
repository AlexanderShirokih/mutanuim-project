using System;
using System.Linq;
using System.Collections.Generic;
using Mutanium.Human;

namespace Mutanium
{
    public class UniqueIdDatabase
    {
        private Dictionary<int, UniqueElement> uniqueIds = new Dictionary<int, UniqueElement>();

        private UniqueIdDatabase()
        { }

        public static UniqueIdDatabase Instance { get; } = new UniqueIdDatabase();

        internal static void Link(UniqueElement uniqueElement, UniqueId id)
        {
            Instance.uniqueIds.Add(id.UId, uniqueElement);
        }

        internal static void UnLink(UniqueId id)
        {
            Instance.uniqueIds.Remove(id.UId);
        }

        public static UniqueElement Find(UniqueId uniqueId)
        {
            Instance.uniqueIds.TryGetValue(uniqueId.UId, out UniqueElement val);
            return val;
        }

        public static void Clear()
        {
            Instance.uniqueIds.Clear();
        }

        public static int Size => Instance.uniqueIds.Count;

        public static IEnumerable<UniqueElement> FindByType(Type type)
        {
            return Instance.uniqueIds.Where(t => type.IsInstanceOfType(t.Value))
                .Select(t => t.Value);
        }
    }
}
