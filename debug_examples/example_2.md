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


# Отладка

# No one is alive

![](attachments/Pasted%20image%2020240307162601.png)

При нескольких запусках приложения мы видим одну и ту же картину - никто из бойцов не выжил.

Нужно убедиться в том что эта картина не происходит при каждом запуске.

## Как выбрать какой из проектов запускать?

В решении в котором есть множество проектов со своими точками входа - можно выбрать текущий запускаемый проект следующим образом

Выбрать нужный проект из иерархии проектов и в контекстном меню нажать "Назначить в качестве запускаемого проекта"

![](attachments/Pasted%20image%2020240307180836.png)

----

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

Можно в контрольный значения добавлять не только переменные но и целые выражения - для этого достаточно их выделить и в контекстном меню нажать `Добавить контрольное значение`

![](attachments/Pasted%20image%2020240307171809.png)
![](attachments/Pasted%20image%2020240307172134.png)


||Имя|Значение|Тип|
|---|---|---|---|
||unitsCount|10|int|
||_maxPercent|100|int|
||_percentOfWarior|30|int|
||_countOfWarior|0|int|
||unitsCount / _maxPercent|0|int|

```
unitsCount / _maxPercent == 0
10 / 100 == 0
```
А ожидаем

```
10 / 100 == 0.1
```


Нужно присмотреться повнимательнее к переменным `unitsCount` и `_maxPercent`

unitsCount - это входной аргумент функции типа int

```cs
public Squad Create(string name, int unitsCount)
```

`_maxPercent` - это поле класса типа int

```cs
public class ArmyFactory
{
    private int _maxPercent = 100;
```

Как мы знаем при делении int на int на выходе мы получаем тоже int.
int не может в себе хранить дробные значения - поэтому там и оказывается 0
исправляем это в несколько

1) мы изменим типы полей (`_percentOfWarior, _percentOfShieldMan, _percentOfArmorBuffer`) на float, чтобы можно было в них хранить и дробные значения процентов (33,5 и тп.)
2) В формулах перед `_maxPercent` мы добавим явное преобразование типа во float, чтобы конструкции вида `int / int = int` мы пришли к `int / float = float`

После первых 2х шагов видим следующую картину

![](attachments/Pasted%20image%2020240307173850.png)

в переменную `_countOfWarrior` мы не можем положить тип float т.к. она является типом int
следовательно нужно результат выражения привести к виду в котором будет округлен результат(Math.Round) и преобразован к типу int.

Для этого используем функцию 

конечный результат выглядит вот так

```cs
_countOfWarior = (int)Math.Round( unitsCount / (float)_maxPercent * _percentOfWarior);
_countOfShieldMan = (int)Math.Round( unitsCount / (float)_maxPercent * _percentOfShieldMan);
_countOfArmorBuffer = (int)Math.Round( unitsCount / (float)_maxPercent * _percentOfArmorBuffer);
```

Так как мы поменяли исходный код - то нам нужно сохранить результат и пересобрать приложение.

Запускаем отладку

![](attachments/Pasted%20image%2020240307174913.png)

||Имя|Значение|Тип|
|---|---|---|---|
||unitsCount|10|int|
||_countOfWarior|3|int|
||_countOfShieldMan|4|int|
||_countOfArmorBuffer|4|int|
|▶|units|Count = 11|System.Collections.Generic.List<Example_2_Debug.Unit>|

Видим что ситуация изменилась и на выходе у нас есть список на 11 юнитов.

Стоп! 11?

Перепроверим себя - в окне контрольных значений нажимаем дважды на строчку 
"Добавить элемент в контрольное значение" 


![](attachments/Pasted%20image%2020240307175441.png)

и начинаем печатать код

```cs
_countOfWarior + _countOfShieldMan + _countOfArmorBuffer
```

![](attachments/Pasted%20image%2020240307175730.png)
||Имя|Значение|Тип|
|---|---|---|---|
||_countOfWarior + _countOfShieldMan + _countOfArmorBuffer|11|int|

Сумма = 11

А сколько передали в метод? 10!

```
11 != 10
```

Похоже придется немного изменить логику 

Например сделать так, чтобы количество юнитов последнего типа считалось по другому например как

```
[кол-во юнитов вида х] = [нужно создать юнитов всего] - [сумма уже созданных видов юнитов]
```

итоговый вид

```cs
_countOfWarior = (int)Math.Round( unitsCount / (float)_maxPercent * _percentOfWarior);
_countOfShieldMan = (int)Math.Round( unitsCount / (float)_maxPercent * _percentOfShieldMan);

int[] createdUnits = { _countOfWarior, _countOfShieldMan };

_countOfArmorBuffer = unitsCount - createdUnits.Sum();
```

Пересобираем проект и запускаем отладку

![](attachments/Pasted%20image%2020240307181654.png)

||Имя|Значение|Тип|
|---|---|---|---|
||unitsCount|10|int|
||_countOfWarior|3|int|
||_countOfShieldMan|4|int|
||_countOfArmorBuffer|3|int|
|▶|units|Count = 10|System.Collections.Generic.List<Example_2_Debug.Unit>|
||_countOfWarior + _countOfShieldMan + _countOfArmorBuffer|10|int|

На выходе мы получили список из 10 бойцов как и задумали.

## Убеждаемся что метод Create работает стабильно на большом разбросе входных данных.

Основной критерий для проверки, что мы на выходе получаем в отряде то же количество юнитов, что запросили при создании.

Добавим следующие методы в класс Tests для проверки

```cs
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
```

Добавим запуск этого теста
```cs
  static void Main(string[] args)
  {
      Tests tests = new Tests();

      //tests.RunMultiple(100);// Это нам сейчас не нужно

      tests.TestCreateSquadMultiple();
  }
```

Запускаем приложение

![](attachments/Pasted%20image%2020240307191341.png)

Видим что количество ошибок равно 0

## Итоги по методу Create

1) Мы нашли типовую ошибку вида `int / int = int` при выполнение которой программа отбрасывает значение дробной части и дальнейшие результаты становятся недействительными.
2) После исправления первой ошибки мы заметили, что на выходе может создаться не то количество бойцов которое мы запрашивали и нам также пришлось скорректировать логику.

----

Можно продолжать работу с программой - в надежде что все работает.

В классе для отладки изменим код

```cs
  static void Main(string[] args)
{
    Tests tests = new Tests();

    tests.RunMultiple(1);

    //tests.TestCreateSquadMultiple();
}
```


![](attachments/Pasted%20image%2020240307191750.png)

Видим что при одном запуске приложение отрабатывает корректно.

Запускаем его еще раз

![](attachments/Pasted%20image%2020240307191955.png)

Ошибка плавающая - то есть выявляется не всегда.

Для надежности запустим его несколько раз - например 100.

```cs
 static void Main(string[] args)
 {
     Tests tests = new Tests();

     tests.RunMultiple(100);

     //tests.TestCreateSquadMultiple();
 }
```

Ловим опять исключение

![](attachments/Pasted%20image%2020240307185156.png)

Пробуем перезапустить

![](attachments/Pasted%20image%2020240307192247.png)

Видим черное окно - в котором ничего не происходит

При этом если открыть Диспетчер задач - то можно увидеть, что программа работает и при этом активно потребляет ресурсы процессора (если точнее - то полностью выжирает все ресурсы одного процессора)

![](attachments/Pasted%20image%2020240307192339.png)


![](attachments/Pasted%20image%2020240307192547.png)

Вывод - есть ситуация в которой приложение входит в бесконечный цикл и не заканчивает свою работу

### Промежуточный итог

Мы увидели три ситуации

1) Приложение может отработать корректно
2) Приложение может выкинуть исключение "вылет за границы массива"
3) Приложение может войти в бесконечный цикл и не заканчивать свою работу
# Как читать логи исключений

Копия логов
```
Unhandled exception. System.ArgumentOutOfRangeException: Index was out of range. Must be non-negative and less than the size of the collection. (Parameter 'index')
   at System.Collections.Generic.List`1.get_Item(Int32 index)
   at Example_2_Debug.Squad.GetRamdomUnit() in D:\Obsidian\debug_notes\src\DebugNotesExamples\Example_2_Debug\Program.cs:line 99
   at Example_2_Debug.Squad.AttackEnemySquad(Squad enemySquad) in D:\Obsidian\debug_notes\src\DebugNotesExamples\Example_2_Debug\Program.cs:line 72
   at Example_2_Debug.WarField.BeginBattle() in D:\Obsidian\debug_notes\src\DebugNotesExamples\Example_2_Debug\Program.cs:line 36
   at Example_2_Debug_Tests.Tests.RunSingle() in D:\Obsidian\debug_notes\src\DebugNotesExamples\Example_2_Debug_Tests\Program.cs:line 23
   at Example_2_Debug_Tests.Tests.RunMultiple(Int32 amount) in D:\Obsidian\debug_notes\src\DebugNotesExamples\Example_2_Debug_Tests\Program.cs:line 30
   at Example_2_Debug_Tests.Program.Main(String[] args) in D:\Obsidian\debug_notes\src\DebugNotesExamples\Example_2_Debug_Tests\Program.cs:line 11

D:\Obsidian\debug_notes\src\DebugNotesExamples\Example_2_Debug_Tests\bin\Debug\net6.0\Example_2_Debug_Tests.exe (процесс 23472) завершил работу с кодом -532462766.
```

Логи читаем сверху вниз

## Конкретная ошибка
```
Unhandled exception. System.ArgumentOutOfRangeException: Index was out of range. Must be non-negative and less than the size of the collection. (Parameter 'index')
   at System.Collections.Generic.List`1.get_Item(Int32 index)
   at Example_2_Debug.Squad.GetRamdomUnit() in D:\Obsidian\debug_notes\src\DebugNotesExamples\Example_2_Debug\Program.cs:line 99
```

По простому - у нас произошел вылет за границы коллекции при попытке доступа к элементу коллекции в методе
```
at Example_2_Debug.Squad.GetRamdomUnit()
```

Тут конкретно указана строка в которой эта ошибка произошла
```
D:\Obsidian\debug_notes\src\DebugNotesExamples\Example_2_Debug\Program.cs:line 99
```

----
## Далее вниз

Далее, если идти вниз по цепочки то мы видим, что обращение к **GetRamdomUnit**
 было в методе **AttackEnemySquad**

```
   at Example_2_Debug.Squad.AttackEnemySquad(Squad enemySquad) in D:\Obsidian\debug_notes\src\DebugNotesExamples\Example_2_Debug\Program.cs:line 72
```

 и так далее.

----

## Лайфхак по быстрому перемещению к месту ошибки

Если ошибка стабильно воспроизводится - то мы можем быстро попасть в место, где ошибка происходит

Нажимаем F5 чтобы начать исполнение программы в режиме отладки и IDE нас сразу перенесет куда нужно

![](attachments/Pasted%20image%2020240307190150.png)


# Пытаемся исправить вылет за границы массивы

```cs
private Unit GetRamdomUnit()
{
    return _units[UserUtils.GetRandomValue(_units.Count)];
}
```

![](attachments/Pasted%20image%2020240307193530.png)



||Имя|Значение|Тип|
|---|---|---|---|
||UserUtils.GetRandomValue(_units.Count)|0|int|
||_units.Count|0|int|
||UserUtils.GetRandomValue(0)|0|int|

Как мы видим если в списке 0 элементов - то

```cs
GetRandomValue(0) == 0
```

А при попытке обращения к нулевому элементу коллекции
```
_units[0]
```

в которой нет элементов (`_units.Count == 0`) - мы и получаем исключение **System.ArgumentOutOfRangeException**

### Как исправлять

Есть 2 варианта 
1) если в коллекции нет элементов - то возвращать значение null
2) или переделать функцию таким образом чтобы при ее вызове программист получал доп. инфу о том, что получить инфу не удалось.

Первый вариант опасен - он может спровоцировать появление ошибки в тех местах, где разработчик не предполагает появление null, а также заставляет проводить проверку на null при обращении к данному методу.

Поэтому реализуем второй вариант.

Перепишем функцию следующим образом

```cs
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
```

Соответственно в местах вызова метода код также будет изменен

```cs
 public void AttackEnemySquad(Squad enemySquad)
 {
     Unit currentUnit = null;
     Unit enemyUnit = null;

     if (!TryGetRamdomUnit(out currentUnit) ||
         !enemySquad.TryGetRamdomUnit(out enemyUnit))
     {
         return;
     }

     if (currentUnit is Warior)
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
```

Пробуем запускать программу множество раз

![](attachments/Pasted%20image%2020240307200932.png)
![](attachments/Pasted%20image%2020240307201030.png)

![](attachments/Pasted%20image%2020240307201052.png)

### Вывод

Ошибки вылета за границу массива мы более не наблюдаем - но остается беда с зацикливанием программы.

# Отлаживаем проблему с зацикливанием программы

Запуск программы выглядит так

```cs
 public void RunSingle()
 {
     int armyCount = 10;

     WarField warField = new WarField(armyCount, "First", "Second");

     warField.BeginBattle();
 }
```

```cs
 public WarField(int armyCount, string firstArmyName, string secondArmyName)
 {
     _firstArmy = _armyFactory.Create(firstArmyName, armyCount);
     _secondArmy = _armyFactory.Create(secondArmyName, armyCount);
 }
```

В конструкторе WarField ничего странного не происходит - более того метод 
`_armyFactory.Create` мы уже прогнали через множество тестов и убедились что он работает как надо

изучаем метод

```cs
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
```


В нем есть один цикл **while** -- скорее всего проблема именно в нем.

Ставим в нем точку останова и начинаем отлаживаться.

![](attachments/Pasted%20image%2020240307203302.png)

Чтобы лишний раз не нажимать F10 и F11 - можно нажимать клавишу F5 - тогда программа исполнит итерацию тела цикла и вернется опять к точке останова

Понажимали F5 довольно много раз - но картина не меняется количество бойцов не уменьшается.

Присмотримся поближе к нашим бойцам

В окне контрольных значений для юнитов можно закреплять интересующие нас значения для вывода их на экран "Закрепить элемент в избранном"

![](attachments/Pasted%20image%2020240307203808.png)


![](attachments/Pasted%20image%2020240307204426.png)

Значения очень медленно изменяются

![](attachments/Pasted%20image%2020240307204512.png)

Один боец помер

Еще немного погоняем отладку

![](attachments/Pasted%20image%2020240307204934.png)

Значения здоровья бойцов перестали изменяться вообще

Заглянем внутрь юнитов и увидим что значения брони стали аномально высокими

![](attachments/Pasted%20image%2020240307205056.png)

![](attachments/Pasted%20image%2020240307205146.png)

Возможно проблема в этом - нужно проверить как работает логика нанесения урона

Во время очередного цикла отладки используем кнопку F11 для того, чтобы заходить внутрь тела вызываемых методов

Мы оказались внутри метода  AttackEnemySquad

![](attachments/Pasted%20image%2020240307205414.png)

Едем и далее по шагам через F11

Доходим до логики

![](attachments/Pasted%20image%2020240307205554.png)

Во время отладки желтая стрелка не вошла не в блок
```cs
if (currentUnit is Warior)
{
    currentUnit.TakeAction(enemyUnit);
}
```

```cs

if (currentUnit is Suporter)
{
    if (TryGetRamdomUnit(out Unit ally))
    {
        currentUnit.TakeAction(ally);
    }    
}
```

||Имя|Значение|Тип|
|---|---|---|---|
|▶|currentUnit|currentHealth = 75, currentArmor = 240|Example_2_Debug.Unit {Example_2_Debug.ShieldMan}|

А если посмотреть внимательно на тип юнита - то мы увидим что он **ShieldMan** то логика для юнита с щитом не определена вообще

Проверим цепочку наследования

```cs
 public class ShieldMan : Damager
 public abstract class Damager : Unit
```

Соответственно `ShieldMan Damager Unit` не входят в группу наследников `Warior Suporter`

При этом

```
Warior : Damager
```

Warior наследуется от Damager

```cs
if (currentUnit is Warior)
{
    currentUnit.TakeAction(enemyUnit);
}
```

исправим на 

```cs
if (currentUnit is Damager)
{
    currentUnit.TakeAction(enemyUnit);
}
```

Перезапустим программу

![](attachments/Pasted%20image%2020240307211058.png)

Видим, что программа начала работать более стабильно - то явление зацикливание все еще имеет место быть.

Памятуя о том что были случаи большого количества брони нужно еще раз проверить нанесение урона при большом количестве брони

Поставим точку останова в блоке 
```cs
if (currentUnit is Damager)
{
    currentUnit.TakeAction(enemyUnit);
}
```
![](attachments/Pasted%20image%2020240307211401.png)

Запускаем отладку

![](attachments/Pasted%20image%2020240307211521.png)

И попадаем в точку останова на первой же итерации - но нам это не нужно.

Исходя из отладки мы знаем что беды начинаются именно на больших значениях брони

||Имя|Значение|Тип|
|---|---|---|---|
|◢|currentUnit|currentHealth = 100, currentArmor = 5|Example_2_Debug.Unit {Example_2_Debug.Warior}|
||currentHealth|100|int|
||currentArmor|5|int|
||IsDead|false|bool|
||currentDamage|25|int|
||maxArmor|5|int|
||maxHealth|100|int|
|◢|enemyUnit|currentHealth = 100, currentArmor = 1|Example_2_Debug.Unit {Example_2_Debug.ArmorBuffer}|
||currentHealth|100|int|
||currentArmor|1|int|
||IsDead|false|bool|
||armorBuffPoints|5|int|
||currentMana|100|int|
||currentManaRegen|25|int|
||maxArmor|1|int|
||maxHealth|100|int|
||maxMana|100|int|

1) currentArmor == 5
2) currentArmor == 1

Это очень маленькие значения

Произошло попадание в точку останова в тот момент когда нужные нам условия еще не соблюдены

# Как вызвать прерывание отладкой при определенных условиях

Для этого можно модифицировать условия срабатывания точки останова

Наводим курсор мыши на точку останова и жмем кнопку параметры

![](attachments/Pasted%20image%2020240307212138.png)

Увидим такое окно

![](attachments/Pasted%20image%2020240307212207.png)

Нажмем галочку напротив пункта условия

![](attachments/Pasted%20image%2020240307212246.png)

Начинаем вводить код

```
enemyUnit.a
```

![](attachments/Pasted%20image%2020240307212340.png)

И тут мы понимаем - что публичного свойства "текущая броня" у нас нету - соответственно сейчас мы не сможем получить текущее значение из кода

Придется модифицировать код таким образом, чтобы мы могли посмотреть нужные нам значения во время отладки

Добавим интерфейс для просмотра значений юнита

```cs
    public interface IUnit 
    {
        int MaxHealth { get; }

        int CurrentHealth { get; }

        int CurrentArmor { get; }

        int MaxArmor { get; }
    }
```

Теперь сделаем так чтобы наш класс Unit имплементировал этот интерфейс

```cs
public abstract class Unit : IUnit
```

Возвращаемся к нашей точке останова и прописываем ей условия

```cs
enemyUnit.CurrentArmor > 50
```

![](attachments/Pasted%20image%2020240307213152.png)


Сохраняемся и перезапускаем программу в режиме отладки

![](attachments/Pasted%20image%2020240307213326.png)

Как мы видим программа выполнится много раз прежде чем произойдет попадание в точку останова

![](attachments/Pasted%20image%2020240307213454.png)

||Имя|Значение|Тип|
|---|---|---|---|
|▶|currentUnit|CurrentArmor = 20, CurrentHealth = 20|Example_2_Debug.Unit {Example_2_Debug.Warior}|
|▶|enemyUnit|CurrentArmor = 56, CurrentHealth = 21|Example_2_Debug.Unit {Example_2_Debug.ArmorBuffer}|

Нажимаем F11 и проваливаемся в метод TakeAction

![](attachments/Pasted%20image%2020240307213530.png)

Еще раз и провалимся в метод

![](attachments/Pasted%20image%2020240307213604.png)

![](attachments/Pasted%20image%2020240307213628.png)

Еще раз и проваливаемся в TakeDamage

```cs
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
```

Как мы видим

||Имя|Значение|Тип|
|---|---|---|---|
||damage|36|int|
||currentArmor|56|int|
||damage > currentArmor|false|bool|

урон меньше брони - поэтому атака не проходит.

Очевидно, что нужно либо менять условие проверки либо сделать ограничение по количеству максимальной брони

Еще раз перезапустим отладку

![](attachments/Pasted%20image%2020240307214726.png)


||Имя|Значение|Тип|
|---|---|---|---|
|◢|currentUnit|currentHealth = 20, currentArmor = 20|Example_2_Debug.Unit {Example_2_Debug.ShieldMan}|
||currentHealth|20|int|
||currentArmor|20|int|
||CurrentArmor|20|int|
||CurrentHealth|20|int|
||IsDead|false|bool|
||MaxArmor|10|int|
||MaxHealth|100|int|
||currentDamage|120|int|
||maxArmor|10|int|
||maxHealth|100|int|
|◢|enemyUnit|CurrentArmor = 51, CurrentHealth = 39|Example_2_Debug.Unit {Example_2_Debug.ArmorBuffer}|
||CurrentArmor|51|int|
||CurrentHealth|39|int|
||IsDead|false|bool|
||MaxArmor|1|int|
||MaxHealth|100|int|
||armorBuffPoints|5|int|
||currentArmor|51|int|
||currentHealth|39|int|
||currentMana|100|int|
||currentManaRegen|25|int|
||maxArmor|1|int|
||maxHealth|100|int|
||maxMana|100|int|

И внимательно изучаем статусы юнитов

||Имя|Значение|Тип|
|---|---|---|---|
|◢|enemyUnit|CurrentArmor = 51, CurrentHealth = 39|Example_2_Debug.Unit {Example_2_Debug.ArmorBuffer}|
||CurrentArmor|51|int|
||MaxArmor|1|int|

```cs
CurrentArmor > MaxArmor
51 > 1
```

Очевидно, что есть ошибка при наложении бафа на броню


.
----

| Навигация                 |                                             |     |
| ------------------------- | ------------------------------------------- | --- |
| [example_1](example_1.md) | [К списку примеров](debug_examples_list.md) |     |
