using System.Text;

namespace Example_2_Debug
{
    class ConsoleOutputDisplay
    {
        private readonly StringBuilder _builder = new StringBuilder();

        public string Content => _builder.ToString();

        public void Append(string message)
        {
            _builder.Append(message);
        }

        public void AppendLine() 
        {
            _builder.AppendLine();
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            ConsoleOutputDisplay consoleOutput = new ConsoleOutputDisplay();

            int firstMultiplier = 2, secondMultiplier = 1, ogri = 0, outputCounter = 1;

            while (secondMultiplier < 10)
            {
                while (firstMultiplier < ogri)
                {
                    int multiplicationResult = firstMultiplier * secondMultiplier;
                    var message = string.Format("{0} x {1}= {2}\t", firstMultiplier, secondMultiplier, multiplicationResult);
                    Console.Write(message);
                    consoleOutput.Append(message);
                    ++firstMultiplier;

                    ++outputCounter;
                }

                if (outputCounter < 33)
                {
                    ogri = 6;
                    Console.WriteLine();
                    consoleOutput.AppendLine();
                    secondMultiplier++;
                    firstMultiplier = 2;
                }
                else
                {
                    ogri = 10;
                    Console.WriteLine();
                    consoleOutput.AppendLine();
                    secondMultiplier++;
                    firstMultiplier = 6;
                    if (outputCounter == 33)
                    {
                        Console.WriteLine();
                        consoleOutput.AppendLine();
                        secondMultiplier = 2;
                    }
                }
            }

            Console.ReadKey();
        }
    }
}
