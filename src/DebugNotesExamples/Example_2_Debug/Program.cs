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

            int firstMultiplier = 2, secondMultiplier = 1, firstMultiplierLimit = 0, outputCounter = 0;
            ;
            while (secondMultiplier < groupLimit)
            {
                while (firstMultiplier < firstMultiplierLimit)
                {
                    int multiplicationResult = firstMultiplier * secondMultiplier;

                    Console.Write("{0} x {1} = {2}\t", firstMultiplier, secondMultiplier, multiplicationResult);
                    
                    ++firstMultiplier;
                    ++outputCounter;
                }

                if (outputCounter < 32)
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
                    if (outputCounter == 32)
                    {
                        Console.WriteLine();
                        secondMultiplier = multiplierStartValue;
                    }
                }
            }

            Console.ReadKey();
        }
    }
}
