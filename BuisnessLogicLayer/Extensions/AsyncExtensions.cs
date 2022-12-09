using System.Collections.Concurrent;

namespace BuisnessLogicLayer.Extensions
{
    public static class AsyncExtensions
    {
        public static async Task<IEnumerable<T>> Where<T>(
             this IEnumerable<T> source, Func<T, Task<bool>> predicate)
        {
            var results = new ConcurrentQueue<T>();
            var tasks = source.Select(
                async x =>
                {
                    if (await predicate(x))
                        results.Enqueue(x);
                });
            await Task.WhenAll(tasks);
            return results;
        }
    }
}
