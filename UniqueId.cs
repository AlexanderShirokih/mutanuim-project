namespace Mutanium
{
    [System.Serializable]
    public class UniqueId
    {
        public int id;

        public static UniqueId GetNextUniqueId()
        {
            Global save = Global.Current;
            UniqueId hid = new UniqueId
            {
                id = save.LastUniqueId + 1
            };
            save.LastUniqueId = hid.id;
            return hid;
        }
    }
}
