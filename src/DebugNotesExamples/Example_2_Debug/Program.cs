﻿namespace Example_2_Debug
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int firstMultiplier = 2, secondMultiplier = 1, ogri = 0, ij = 1;

            while (secondMultiplier < 10)
            {
                while (firstMultiplier < ogri)
                {
                    int multiplicationResult = firstMultiplier * secondMultiplier;
                    Console.Write("{0} x {1}= {2}\t", firstMultiplier, secondMultiplier, multiplicationResult);
                    ++firstMultiplier;

                    ++ij;
                }

                if (ij < 33)
                {
                    ogri = 6;
                    Console.WriteLine();
                    secondMultiplier++;
                    firstMultiplier = 2;
                }
                else
                {
                    ogri = 10;
                    Console.WriteLine();
                    secondMultiplier++;
                    firstMultiplier = 6;
                    if (ij == 33)
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
