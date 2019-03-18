namespace Mutanium
{
    [System.Serializable]
    public class ReferencedId<T> where T : UniqueElement
    {
        public UniqueId RefId { get; set; }

        public T Get()
        {
            return (T)UniqueIdDatabase.Find(RefId);
        }
    }
}
