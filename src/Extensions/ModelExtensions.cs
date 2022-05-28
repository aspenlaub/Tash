using System.Linq;
using System.Reflection;

namespace Aspenlaub.Net.GitHub.CSharp.Tash.Extensions;

public static class ModelExtensions {
    public static bool MemberwiseEquals<T>(this T item, T otherItem) {
        // ReSharper disable once PossibleNullReferenceException
        return typeof(T).GetProperties(BindingFlags.Instance | BindingFlags.Public).All(property => property.GetValue(item).Equals(property.GetValue(otherItem)));
    }
}