using UnityEngine;

namespace BagOfTricks.Extensions
{
    internal static class ComponentExtensions
    {
        public static bool TryGetComponent<T>(this Component @object, out T component) where T : Component
        {
            component = @object.GetComponent<T>();
            
            if (component == null)
                return false;

            return true;
        }

        public static bool TryGetComponent<T>(this GameObject @object, out T component) where T : Component
        {
            component = @object.GetComponent<T>();

            if (component == null)
                return false;

            return true;
        }
    }
}
