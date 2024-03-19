using System.Text;

namespace Example_2_Debug
{  

    internal class Program
    {
        private const int firstGroupLimit = 6;

        static void Main(string[] args)
        {
            const int multiplierStartValue = 2;

            int firstMultiplier = 2, secondMultiplier = 1, firstMultiplierLimit = 0, outputCounter = 1;
            ;
            while (secondMultiplier < 10)
            {
                while (firstMultiplier < firstMultiplierLimit)
                {
                    int multiplicationResult = firstMultiplier * secondMultiplier;

                    Console.Write("{0} x {1} = {2}\t", firstMultiplier, secondMultiplier, multiplicationResult);
                    
                    ++firstMultiplier;
                    ++outputCounter;
                }

                if (outputCounter < 33)
                {
                    firstMultiplierLimit = firstGroupLimit;
                    Console.WriteLine();
                    secondMultiplier++;
                    firstMultiplier = multiplierStartValue;
                }
                else
                {
                    firstMultiplierLimit = 10;
                    Console.WriteLine();
                    secondMultiplier++;
                    firstMultiplier = 6;
                    if (outputCounter == 33)
                    {
                        Console.WriteLine();
                        secondMultiplier = 2;
                    }
                }
            }

            Console.ReadKey();
        }
    }
}
