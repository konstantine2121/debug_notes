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

            const int splitStep = (firstGroupLimit - multiplierStartValue) * (groupLimit - multiplierStartValue);

            int firstMultiplier = multiplierStartValue;
            int secondMultiplier = multiplierStartValue;
            int firstMultiplierLimit = firstGroupLimit;
            int outputCounter = 0;

            while (secondMultiplier < groupLimit)
            {
                while (firstMultiplier < firstMultiplierLimit)
                {
                    int multiplicationResult = firstMultiplier * secondMultiplier;
                    PrintMultiplicationResult(firstMultiplier, secondMultiplier, multiplicationResult);

                    ++firstMultiplier;
                    ++outputCounter;
                }

                if (outputCounter < splitStep)
                {
                    firstMultiplierLimit = firstGroupLimit;
                    Console.WriteLine();
                    secondMultiplier++;
                    firstMultiplier = multiplierStartValue;
                }
                else
                {
                    firstMultiplierLimit = groupLimit;
                    Console.WriteLine();
                    secondMultiplier++;
                    firstMultiplier = secondGroupStartValue;

                    if (outputCounter == splitStep)
                    {
                        Console.WriteLine();
                        secondMultiplier = multiplierStartValue;
                    }
                }
            }

            Console.ReadKey();
        }

        private static void PrintMultiplicationResult(int firstMultiplier, int secondMultiplier, int multiplicationResult)
        {
            Console.Write("{0} x {1} = {2}\t", firstMultiplier, secondMultiplier, multiplicationResult);
        }
    }
}
