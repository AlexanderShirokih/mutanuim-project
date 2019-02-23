namespace Mutanium
{
    public enum Relationship
    {
        PEACEFUL_HUMAN,
        HOSTILE_HUMAN,
        NEUTRAL_HUMAN,
        MUTATED_HUMAN,
        PEACEFUL_ANIMAL,
        PREDATORY_ANIMAL,
        MUTATED_ANIMAL
    };

    public static class RelationshipResolver
    {
        public const int GOOD = 1;
        public const int NEUTRAL = 0;
        public const int BAD = -1;

        private static int[,] relations = {
            {1, -1, 0, -1, 1, -1, -1}, //P_H
            {-1, 1, 0, -1, 1, 0, -1}, //H_H
            {0, 0, 1, -1, 1, 0, -1}, //N_H
            {-1, -1, -1, 1, 1, 1,1}, //M_H
            {1, 1, 1, 1, 1, 1, 1}, //P_A
            {0, 0, 0, 1, 1, 1, 1},//PR_A
            {-1, -1, -1, 1, 1, 1, 1}//M_A
         };

        /// <summary>
        /// Return relation between two Relationship objects
        /// </summary>
        /// <returns>1 - for good relationship,
        ///          0 - for neutral relationship,
        ///          -1 - for bad relationship</returns>
        /// <param name="from">From.</param>
        /// <param name="to">To.</param>
        public static int Resolve(Relationship from, Relationship to)
        {
            return relations[(int)from, (int)to];
        }
    }
}
