using System.Reflection;

namespace Aspenlaub.Net.GitHub.CSharp.Tash.Extensions {
    public static class ModelExtensions {
        public static bool MemberwiseEquals<T>(this T item, T otherItem) {
            foreach (var property in typeof(T).GetProperties(BindingFlags.Instance | BindingFlags.Public)) {
                if (!property.GetValue(item).Equals(property.GetValue(otherItem))) { return false; }
            }

            return true;
        }
    }
}
