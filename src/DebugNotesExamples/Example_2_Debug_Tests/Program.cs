using Example_2_Debug;

namespace Example_2_Debug_Tests
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Tests tests = new Tests();

            tests.RunMultiple(100);
        }
    }

    class Tests
    {
        public void RunSingle()
        {
            int armyCount = 10;

            WarField warField = new WarField(armyCount, "First", "Second");

            warField.BeginBattle();
        }

        public void RunMultiple(int amount)
        {
            for (int i = 0; i < amount; i++) 
            {
                RunSingle();
            }
        }
    }
}
