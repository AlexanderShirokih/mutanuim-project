using System;
using Mutanium.Human;

namespace Mutanium
{
    [Serializable]
    public class SaveFile
    {
        public HouseInfo[] HouseInfos;
        public HumanInfo[] Humans;
    }
}
