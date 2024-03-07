using Example_2_Debug;

namespace Example_2_Debug_Tests
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Tests tests = new Tests();

            tests.RunMultiple(10000);

            //tests.TestCreateSquadMultiple();
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

        public void TestCreateSquadMultiple()
        {
            int errorCounter = 0;
            int minValue = 1;
            int maxValue = 10000;

            ArmyFactory armyFactory = new ArmyFactory();

            for (int numberOfUnitsToCreate = minValue; numberOfUnitsToCreate <= maxValue; numberOfUnitsToCreate++)
            {
                if (!TestCreateSquad(armyFactory, numberOfUnitsToCreate))
                {
                    errorCounter++;
                }
            }

            Console.WriteLine("Тестирование создания юнитов");
            Console.WriteLine($"Минимальное количество юнитов для создания {minValue}");
            Console.WriteLine($"Максимальное количество юнитов для создания {maxValue}");
            Console.WriteLine($"Количество ошибок при создании {errorCounter}");
        }

        public bool TestCreateSquad(ArmyFactory armyFactory, int numberOfUnitsToCreate)
        {
            Squad squad = armyFactory.Create(numberOfUnitsToCreate.ToString(), numberOfUnitsToCreate);
            return squad.GetUnitsCount() == numberOfUnitsToCreate;
        }
    }
}
