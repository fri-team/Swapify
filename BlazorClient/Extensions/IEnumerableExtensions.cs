using System;
using System.Collections.Generic;
using System.Linq;

namespace BlazorClient.Extensions
{
    public static class IEnumerableExtensions
    {
        public static int Gcd(this IEnumerable<int> numbers)
        {            
            return numbers.Aggregate(Gcd);
        }

        private static int Gcd(int a, int b)
        {
            return b == 0 ? a : Gcd(b, a % b);
        }

        public static int Lcm(this IEnumerable<int> numbers)
        {
            return numbers.Aggregate(Lcm);
        }

        private static int Lcm(int a, int b)
        {
            return Math.Abs(a * b / Gcd(new int[] { a, b }));
        }

        public static IEnumerable<(T item, int index)> WithIndex<T>(this IEnumerable<T> self)
            => self.Select((item, index) => (item, index));
    }
}
