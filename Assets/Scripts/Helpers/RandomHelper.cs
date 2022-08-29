using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace Helpers
{
    [UsedImplicitly]
    public class RandomHelper<T>
    {
        private RandomHelper()
        {
        }

        public static T GetRandomOutOfList(IList<T> tList)
        {
            int randomIndex = Random.Range(0, tList.Count);
            return tList[randomIndex];
        }
        
        public static T GetRandomOutOfReadOnlyList(IReadOnlyList<T> tList)
        {
            int randomIndex = Random.Range(0, tList.Count);
            return tList[randomIndex];
        }
    }
}