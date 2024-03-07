# Исходники

Оригинальная версия исходников лежит тут

https://github.com/konstantine2121/debug_notes/blob/main/src/DebugNotesExamples/Example_2_Original/Program.cs

Модифицированная лежит тут

https://github.com/konstantine2121/debug_notes/blob/main/src/DebugNotesExamples/Example_2_Debug/Program.cs

Код тестов лежит тут

https://github.com/konstantine2121/debug_notes/blob/main/src/DebugNotesExamples/Example_2_Debug_Tests/Program.cs

----

Листинг оригинальной версии
```cs
using System;
using System.Collections.Generic;

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

    class WarField
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

    class Squad
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
            Unit currentUnit = GetRamdomUnit();
            Unit enemyUnit = enemySquad.GetRamdomUnit();

            if (currentUnit is Warior)
            {
                currentUnit.TakeAction(enemyUnit);
            }

            if (currentUnit is Suporter)
            {
                currentUnit.TakeAction(GetRamdomUnit());
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

        private Unit GetRamdomUnit()
        {
            return _units[UserUtils.GetRandomValue(_units.Count)];
        }

        public string Name { get; private set; }
    }

    abstract class Unit
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

        public abstract void TakeAction(Unit unit);
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

    abstract class Damager : Unit
    {
        protected int currentDamage;

        public void Attack(Unit unit)
        {
            currentDamage = GetDamage();

            unit.TakeDamage(currentDamage);
        }

        public override void TakeAction(Unit unit)
        {
            Attack(unit);
        }

        protected abstract int GetDamage();
    }

    abstract class Suporter : Unit
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

    class ArmorBuffer : Suporter
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
            UseBuffSkill(ally);
        }

        public override void UseBuffSkill(Unit unit)
        {
            unit.GetBuff(armorBuffPoints);
        }
    }

    class ShieldMan : Damager
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

    class Warior : Damager
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

    class ArmyFactory
    {
        private int _maxPercent = 100;

        private int _percentOfWarior = 30;
        private int _percentOfShieldMan = 35;
        private int _percentOfArmorBuffer = 35;

        private int _countOfWarior;
        private int _countOfShieldMan;
        private int _countOfArmorBuffer;

        public Squad Create(string name, int unitsCount)
        {
            List<Unit> units = new List<Unit>();

            ArmorBuffer buffer = new ArmorBuffer();
            ShieldMan shieldMan = new ShieldMan();
            Warior warior = new Warior();

            _countOfWarior = unitsCount / _maxPercent * _percentOfWarior;
            _countOfShieldMan = unitsCount / _maxPercent * _percentOfShieldMan;
            _countOfArmorBuffer = unitsCount / _maxPercent * _percentOfArmorBuffer;

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

    class UserUtils
    {
        private static Random s_random = new Random();

        public static int GetRandomValue(int maxValue)
        {
            return s_random.Next(maxValue);
        }
    }
}
```


# Запуск приложения

![](attachments/Pasted%20image%2020240307162601.png)

При нескольких запусках приложения мы видим одну и ту же картину - никто из бойцов не выжил.

Нужно убедиться в том что эта картина не происходит при каждом запуске.
## Создание отдельного проекта для тестирования


Для этого создадим отдельный проект **Example_2_Debug_Tests**, в котором напишем простые вспомогательные классы для тестирования.

Структура проектов будет выглядеть так

![](attachments/Pasted%20image%2020240307163521.png)


Укажем как источник кода ссылку на первый проект

![](attachments/Pasted%20image%2020240307163616.png)

![](attachments/Pasted%20image%2020240307163638.png)

Теперь мы можем ссылаться на первый проект используя  соответствующее пространство имен

```cs
using Example_2_Debug;
```



![](attachments/Pasted%20image%2020240307163758.png)

Но как мы видим IDE подчеркивает нам код как ошибку идем в класс **WarField**

![](attachments/Pasted%20image%2020240307163941.png)

мы видим что у класса не прописан модификатор доступа - те по умолчанию он private, а чтобы его использовать мы должны указать public

```cs
    public class WarField
```

и так нужно поступить со всеми классами которые мы собираемся тестировать извне

Запустим нашу программу 100 раз

```cs
Tests tests = new Tests();

tests.RunMultiple(100);
```

![](attachments/Pasted%20image%2020240307164741.png)

Как мы видим - никто никогда не выживает.

Значит нужно разбираться глубже.

Идем последовательно сверху вниз

Первое нужно убедится, что армии вообще создаются.

Добавим точку останова в конструктор WarField

![](attachments/Pasted%20image%2020240307165503.png)


Нажмем несколько раз F10 

Для переменных `_firstArmy` и `_secondArmy` добавим их в список контрольных значений

Для этого ставим курсор на переменную и кликаем правуюю кнопку мыши - в контекстном меню выбираем кнопку добавить в контрольные значения

![](attachments/Pasted%20image%2020240307165835.png)

Окно с контрольными значениями можно найти тут

Отладка - Окна - Контрольные значения

![](attachments/Pasted%20image%2020240307170050.png)

Теперь можно увидеть текущее состояние нашей программы

(Как можно зафиксировать значения переменных рядом с кодом - см [example_1](example_1.md))

![](attachments/Pasted%20image%2020240307170156.png)

Как мы видим 

```cs
        public WarField(int armyCount, string firstArmyName, string secondArmyName)
        {
            _firstArmy = _armyFactory.Create(firstArmyName, armyCount);
            _secondArmy = _armyFactory.Create(secondArmyName, armyCount);
        }
```

||Имя|Значение|Тип|
|---|---|---|---|
|◢|_firstArmy|{Example_2_Debug.Squad}|Example_2_Debug.Squad|
||Name|"First"|string|
||▶ _units|Count = 0|System.Collections.Generic.List<Example_2_Debug.Unit>|
|◢|_secondArmy|{Example_2_Debug.Squad}|Example_2_Debug.Squad|
||Name|"Second"|string|
||▶ _units|Count = 0|System.Collections.Generic.List<Example_2_Debug.Unit>|

Значение `Count` у полей `_units` переменных `_firstArmy _secondArmy` равно 0 -- соответственно никто ни с кем не воюет и все изначально мертвы

Соответственно проблема скорее всего в методе Create класса ArmyFactory


Листинг класса ArmyFactory
```cs
public class ArmyFactory
{
    private int _maxPercent = 100;

    private int _percentOfWarior = 30;
    private int _percentOfShieldMan = 35;
    private int _percentOfArmorBuffer = 35;

    private int _countOfWarior;
    private int _countOfShieldMan;
    private int _countOfArmorBuffer;

    public Squad Create(string name, int unitsCount)
    {
        List<Unit> units = new List<Unit>();

        ArmorBuffer buffer = new ArmorBuffer();
        ShieldMan shieldMan = new ShieldMan();
        Warior warior = new Warior();

        _countOfWarior = unitsCount / _maxPercent * _percentOfWarior;
        _countOfShieldMan = unitsCount / _maxPercent * _percentOfShieldMan;
        _countOfArmorBuffer = unitsCount / _maxPercent * _percentOfArmorBuffer;

        AddUnits(units, warior, _countOfWarior);
        AddUnits(units, shieldMan, _countOfShieldMan);
        AddUnits(units, buffer, _countOfArmorBuffer);

        return new Squad(name, units);
    }

```

Нужно отлаживать его - останавливаем текущую отладку убираем старые точки останова и ставим точку останова уже в методе Create

![](attachments/Pasted%20image%2020240307171247.png)

Как мы видим значения переменных 

||Имя|Значение|Тип|
|---|---|---|---|
||_countOfWarior|0|int|
||_countOfShieldMan|0|int|
||_countOfArmorBuffer|0|int|

Равны 0 -- вот в этом и есть ошибка.

присмотримся повнимательнее к строкам

```cs
_countOfWarior = unitsCount / _maxPercent * _percentOfWarior;
_countOfShieldMan = unitsCount / _maxPercent * _percentOfShieldMan;
_countOfArmorBuffer = unitsCount / _maxPercent * _percentOfArmorBuffer;
```

например к строке

```cs
_countOfWarior = unitsCount / _maxPercent * _percentOfWarior;
```

![](attachments/Pasted%20image%2020240307171620.png)

Почему же так происходит что на выходе 0?

Можно в контрольный значения добавля

231--



----

| Навигация                 |                                             |     |
| ------------------------- | ------------------------------------------- | --- |
| [example_1](example_1.md) | [К списку примеров](debug_examples_list.md) |     |
