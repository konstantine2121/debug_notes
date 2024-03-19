using System.Text;

namespace Example_2_Debug
{  

    internal class Program
    {

        static void Main(string[] args)
        {
            const int firstGroupLimit = 6;
            const int groupLimit = 10;
            const int multiplierStartValue = 2;
            const int secondGroupStartValue = firstGroupLimit;

            PrintMultiplicationGroup(multiplierStartValue, firstGroupLimit, multiplierStartValue, groupLimit);
            Console.WriteLine();
            PrintMultiplicationGroup(secondGroupStartValue, groupLimit, multiplierStartValue, groupLimit);

            Console.ReadKey();
        }

        private static void PrintMultiplicationGroup(int firstMultiplierStart, int firstMultiplierEnd, int secondMultiplierStart, int secondMultiplierEnd)
        {
            for (int secondMultiplier = secondMultiplierStart; secondMultiplier < secondMultiplierEnd; secondMultiplier++) 
            {
                PrintMultiplicationRow(firstMultiplierStart, firstMultiplierEnd, secondMultiplier);
                Console.WriteLine();
            }
        }

        private static void PrintMultiplicationRow(int firstMultiplierStart, int firstMultiplierEnd, int secondMultiplier)
        {
            for (int firstMultiplier = firstMultiplierStart; firstMultiplier < firstMultiplierEnd; firstMultiplier++)
            {
                int result = firstMultiplier * secondMultiplier;

                Console.Write("{0} x {1} = {2}\t", firstMultiplier, secondMultiplier, result);
            }
        }
    }
}
