namespace Example_2_Debug
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int armyCount = 10;

            WarField warField = new WarField(armyCount, "First", "Second");

            warField.BeginBattle();
        }
    }

    public class WarField
    {
        private Squad _firstArmy;
        private Squad _secondArmy;
        private ArmyFactory _armyFactory = new ArmyFactory();

        public WarField(int armyCount, string firstArmyName, string secondArmyName)
        {
            _firstArmy = _armyFactory.Create(firstArmyName, armyCount);
            _secondArmy = _armyFactory.Create(secondArmyName, armyCount);
        }

        public void BeginBattle()
        {
            while (_firstArmy.GetUnitsCount() != 0 && _secondArmy.GetUnitsCount() != 0)
            {
                _firstArmy.AttackEnemySquad(_secondArmy);
                _secondArmy.CleanSquad();
                _secondArmy.AttackEnemySquad(_firstArmy);
                _firstArmy.CleanSquad();
            }

            if (_firstArmy.GetUnitsCount() == 0 && _secondArmy.GetUnitsCount() == 0)
            {
                Console.WriteLine("No one is alive");
            }
            else if (_firstArmy.GetUnitsCount() == 1 && _secondArmy.GetUnitsCount() == 0)
            {
                Console.WriteLine("first army is win");
            }
            else
            {
                Console.WriteLine("second army is win");
            }
        }
    }

    public class Squad
    {
        private List<Unit> _units;

        public Squad(string name, List<Unit> units)
        {
            Name = name;
            _units = units;
        }

        public int GetUnitsCount()
        {
            return _units.Count;
        }

        public void AttackEnemySquad(Squad enemySquad)
        {
            Unit currentUnit = null;
            Unit enemyUnit = null;

            if (!TryGetRamdomUnit(out currentUnit) ||
                !enemySquad.TryGetRamdomUnit(out enemyUnit))
            {
                return;
            }

            if (currentUnit is Damager)
            {
                currentUnit.TakeAction(enemyUnit);
            }

            if (currentUnit is Suporter)
            {
                if (TryGetRamdomUnit(out Unit ally))
                {
                    currentUnit.TakeAction(ally);
                }    
            }
        }

        public void CleanSquad()
        {
            for (int i = _units.Count - 1; i >= 0; i--)
            {
                if (_units[i].IsDead)
                {
                    _units.RemoveAt(i);
                }
            }
        }

        private bool TryGetRamdomUnit(out Unit unit)
        {
            if (_units.Count == 0)
            {
                unit = null;
                return false;
            }

            unit = _units[UserUtils.GetRandomValue(_units.Count)];
            return true;
        }

        public string Name { get; private set; }
    }

    public interface IUnit 
    {
        int MaxHealth { get; }

        int CurrentHealth { get; }

        int CurrentArmor { get; }

        int MaxArmor { get; }
    }

    public abstract class Unit : IUnit
    {
        protected int maxHealth;
        protected int currentHealth;
        protected int currentArmor;
        protected int maxArmor;

        public Unit()
        {
            IsDead = false;
        }

        public bool IsDead { get; private set; }

        public int MaxHealth => maxHealth;

        public int CurrentHealth => currentHealth;

        public int CurrentArmor => currentArmor;

        public int MaxArmor => maxArmor;

        public void TakeDamage(int damage)
        {
            if (damage > currentArmor)
            {
                currentHealth -= damage - currentArmor;
            }

            if (currentHealth <= 0)
            {
                IsDead = true;
            }
        }

        public virtual void TakeAction(Unit unit)
        {
            ClearBuffs();
        }

        public abstract Unit Clone();

        public void GetBuff(int armor)
        {
            currentArmor += armor;
        }

        public void ClearBuffs()
        {
            currentArmor = maxArmor;
        }
    }

    public abstract class Damager : Unit
    {
        protected int currentDamage;

        public void Attack(Unit unit)
        {
            currentDamage = GetDamage();

            unit.TakeDamage(currentDamage);
        }

        public override void TakeAction(Unit unit)
        {
            base.TakeAction(unit);
            Attack(unit);
        }

        protected abstract int GetDamage();
    }

    public abstract class Suporter : Unit
    {
        protected int maxMana;
        protected int currentMana;
        protected int currentManaRegen;

        public void RegenMana()
        {
            currentMana += currentManaRegen;

            if (currentMana > maxMana)
            {
                currentMana = maxMana;
            }
        }

        public abstract void UseBuffSkill(Unit unit);
    }

    public class ArmorBuffer : Suporter
    {
        int armorBuffPoints = 5;

        public ArmorBuffer()
        {
            maxArmor = 1;
            currentArmor = maxArmor;
            maxHealth = 100;
            currentHealth = maxHealth;
            maxMana = 100;
            currentMana = maxMana;
            currentManaRegen = 25;
        }

        public override Unit Clone()
        {
            return new ArmorBuffer();
        }

        public override void TakeAction(Unit ally)
        {
            base.TakeAction(ally);
            UseBuffSkill(ally);
        }

        public override void UseBuffSkill(Unit unit)
        {
            unit.GetBuff(armorBuffPoints);
        }
    }

    public class ShieldMan : Damager
    {
        public ShieldMan()
        {
            maxHealth = 100;
            currentHealth = maxHealth;
            currentDamage = 20;
            maxArmor = 10;
            currentArmor = maxArmor;
        }

        public override Unit Clone()
        {
            return new ShieldMan();
        }

        protected override int GetDamage()
        {
            currentDamage = currentDamage + currentArmor;

            return currentDamage;
        }
    }

    public class Warior : Damager
    {
        public Warior()
        {
            maxHealth = 100;
            currentHealth = maxHealth;
            currentDamage = 25;
            maxArmor = 5;
            currentArmor = maxArmor;
        }

        public override Unit Clone()
        {
            return new Warior();
        }

        protected override int GetDamage()
        {
            currentDamage = currentDamage + (int)currentDamage / currentHealth;

            return currentDamage;
        }
    }

    public class ArmyFactory
    {
        private int _maxPercent = 100;

        private float _percentOfWarior = 30;
        private float _percentOfShieldMan = 35;
        private float _percentOfArmorBuffer = 35;

        private int _countOfWarior;
        private int _countOfShieldMan;
        private int _countOfArmorBuffer;

        public Squad Create(string name, int unitsCount)
        {
            List<Unit> units = new List<Unit>();

            ArmorBuffer buffer = new ArmorBuffer();
            ShieldMan shieldMan = new ShieldMan();
            Warior warior = new Warior();

            _countOfWarior = (int)Math.Round(unitsCount / (float)_maxPercent * _percentOfWarior);
            _countOfShieldMan = (int)Math.Round(unitsCount / (float)_maxPercent * _percentOfShieldMan);

            int[] createdUnits = { _countOfWarior, _countOfShieldMan };

            _countOfArmorBuffer = unitsCount - createdUnits.Sum();

            AddUnits(units, warior, _countOfWarior);
            AddUnits(units, shieldMan, _countOfShieldMan);
            AddUnits(units, buffer, _countOfArmorBuffer);

            return new Squad(name, units);
        }

        private void AddUnits(List<Unit> units, Unit armyUnit, int count)
        {
            for (int i = 0; i < count; i++)
            {
                units.Add(armyUnit.Clone());
            }
        }
    }

    public class UserUtils
    {
        private static Random s_random = new Random();

        public static int GetRandomValue(int maxValue)
        {
            return s_random.Next(maxValue);
        }
    }
}