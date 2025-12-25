using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Aspenlaub.Net.GitHub.CSharp.Tash.Extensions;

// ReSharper disable once UnusedMember.Global
public static class BlockingCollectionExtension {
    public static void Remove<T>(this BlockingCollection<T> collection, T item) {
        lock (collection) {
            bool found;
            var itemsTakenOut = new List<T>();
            do {
                bool result = collection.TryTake(out T comparedItem);
                if (!result) { return; }

                found = comparedItem.Equals(item);
                if (!found) {
                    itemsTakenOut.Add(comparedItem);
                }
            } while (!found);

            Parallel.ForEach(itemsTakenOut, collection.Add);
        }
    }
}