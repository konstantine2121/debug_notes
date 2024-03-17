namespace Example_2_Debug
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int i = 2, j = 1, z = 0, ogri = 0, ij = 1;

            while (j < 10)
            {
                while (i < ogri)
                {
                    z = i * j;
                    Console.Write("{0} x {1}= {2}\t", i, j, z);
                    ++i;

                    ++ij;
                }

                if (ij < 33)
                {
                    ogri = 6;
                    Console.WriteLine();
                    j++;
                    i = 2;
                }
                else
                {
                    ogri = 10;
                    Console.WriteLine();
                    j++;
                    i = 6;
                    if (ij == 33)
                    {
                        Console.WriteLine();
                        j = 2;
                    }
                }
            }

            Console.ReadKey();
        }
    }
}
