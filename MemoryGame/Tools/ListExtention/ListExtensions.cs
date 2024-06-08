namespace MemoryGame.Tools.ListExtention
{
    using System;
    using System.Collections.Generic;
    public static class ListExtensions
    {
        private static Random rng = new Random();
        

        /// <summary>
        /// Extention on List Object to shuffle the objects in a list
        /// </summary>
        /// <typeparam name="T">List with a every class</typeparam>
        /// <param name="list">Returns the class</param>
        public static void Shuffle<T>(this IList<T> list)
        {
            int amount = list.Count;
            while (amount > 1)
            {
                amount--;
                int randomObject = rng.Next(amount + 1);
                T value = list[randomObject];
                list[randomObject] = list[amount];
                list[amount] = value;
            }
        }
    }

}
