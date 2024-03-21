using System.Collections.Generic;
using System.Linq;

namespace BagOfTricks.Extensions 
{
    public static class ArrayExtensions 
    {
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> arr) 
        {
            return arr == null || arr.Any();
        }
    }
}
