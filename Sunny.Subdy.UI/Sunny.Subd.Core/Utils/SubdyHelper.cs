namespace Sunny.Subd.Core.Utils
{
    public class SubdyHelper
    {
        private static readonly Random _random = new Random();
        public static List<string> Shuffle(List<string> inputList, int count = -1)
        {
            List<string> shuffledList = new List<string>(inputList);
            int n = shuffledList.Count;
            for (int i = n - 1; i > 0; i--)
            {
                int j = _random.Next(i + 1);
                string temp = shuffledList[i];
                shuffledList[i] = shuffledList[j];
                shuffledList[j] = temp;
            }
            if (count == -1 || count > n)
            {
                count = n;
            }

            return shuffledList.Take(count).ToList();
        }
    }
}
