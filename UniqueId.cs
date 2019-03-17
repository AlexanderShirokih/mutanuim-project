namespace Mutanium
{
    [System.Serializable]
    public class UniqueId
    {
        public int id;

        public static UniqueId GetNextUniqueId()
        {
            GameSave save = GameSave.Current;
            UniqueId hid = new UniqueId
            {
                id = save.LastUniqueId + 1
            };
            save.LastUniqueId = hid.id;
            return hid;
        }
    }
}
