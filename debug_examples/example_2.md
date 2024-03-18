
# Использование отладки для разбора логики работающей программы.

Бывают ситуации когда нужно работать с чужим кодом с целью его дальнейшей поддержки - внесение новых функций или исправления  работы старых. 

Иногда начинающие разработчики пишут совершенно неразборчивый код, используют неудачные названия для переменных (они не передают назначения/смысл переменных) или используют "магические числа" - литералы со значениями которые неочевидны - все это не улучшает читабельность кода.

В качестве примера возьмем пример исходного кода который выводит таблицу умножения в таком виде

```
2 x 2= 4        3 x 2= 6        4 x 2= 8        5 x 2= 10
2 x 3= 6        3 x 3= 9        4 x 3= 12       5 x 3= 15
2 x 4= 8        3 x 4= 12       4 x 4= 16       5 x 4= 20
2 x 5= 10       3 x 5= 15       4 x 5= 20       5 x 5= 25
2 x 6= 12       3 x 6= 18       4 x 6= 24       5 x 6= 30
2 x 7= 14       3 x 7= 21       4 x 7= 28       5 x 7= 35
2 x 8= 16       3 x 8= 24       4 x 8= 32       5 x 8= 40
2 x 9= 18       3 x 9= 27       4 x 9= 36       5 x 9= 45

6 x 2= 12       7 x 2= 14       8 x 2= 16       9 x 2= 18
6 x 3= 18       7 x 3= 21       8 x 3= 24       9 x 3= 27
6 x 4= 24       7 x 4= 28       8 x 4= 32       9 x 4= 36
6 x 5= 30       7 x 5= 35       8 x 5= 40       9 x 5= 45
6 x 6= 36       7 x 6= 42       8 x 6= 48       9 x 6= 54
6 x 7= 42       7 x 7= 49       8 x 7= 56       9 x 7= 63
6 x 8= 48       7 x 8= 56       8 x 8= 64       9 x 8= 72
6 x 9= 54       7 x 9= 63       8 x 9= 72       9 x 9= 81
```


# Исходные коды

## Ссылки

Оригинал

https://github.com/konstantine2121/debug_notes/blob/main/src/DebugNotesExamples/Example_2_Original/Program.cs

Отладка(Рефакторинг)

https://github.com/konstantine2121/debug_notes/blob/main/src/DebugNotesExamples/Example_2_Debug/Program.cs

Листинг оригинала

```cs
namespace Example_2_Original
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

```


# Начало отладки

## Визуальная оценка кода 

**Переменные**

Все переменные имеют неочевидные названия

```
i, j, z, orgi, ij
```

**Литералы**

```
10, 33, 6, 2
```

Являются магическими переменными - ибо не ясно что эти значения обозначают, более того они используются в разных местах программы в разных обстоятельствах.

## Первая точка останова

Ставим точку останова в начале программы и все значения интересующих нас переменных закрепляем в окне IDE

![](attachments/Pasted%20image%2020240317223259.png)

Начинаем обход программы с помощью пошаговой отладки - клавиша F10

Продолжая отладку - замечаем некоторые нюансы работы приложения

![](attachments/Pasted%20image%2020240317224959.png)

Например
```cs
z = i * j;
Console.Write("{0} x {1}= {2}\t", i, j, z);
```

i = 2; 
j = 2
z = 2 * 2

То есть данные переменные используются непосредственно для рассчетов очередной операции умножения и вывода ее на экран.

Сделаем предположение, что переменная z нужна только в блоке кода

```cs
while (i < ogri)
{
    z = i * j;
    Console.Write("{0} x {1}= {2}\t", i, j, z);
    ++i;

    ++ij;
}
```

Проверяем это предположение - наводим курсор мыши на нее и нажимаем ПКМ, в открывшемся контекстном меню мы выбираем пункт найти все ссылки

![](attachments/Pasted%20image%2020240317225511.png)

IDE должна отобразить окно Ссылки

![](attachments/Pasted%20image%2020240317225944.png)

в нем видны строки в программе где используются данная переменная - стоит обратить внимание как на контекст/ код - так и на значение в столбце **Строка** 
У нас z используется только на 13 и на 14 строке.

## Первый шаг по рефакторингу (i, j, z)

Переименовываем переменные i, j, z в firstMultiplier, secondMultiplier, multiplicationResult соответственно.
Кроме того, переменную можем объявить непосредственно в месте использования


```cs
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
```

После изменений фрагмент кода выглядит вот так.

## Второй шаг по рефакторингу (ij)

Также на глаза попадается строка

```cs
++ij;
```

которая вызывается сразу после вывода информации об умножении на экран

проверим, что это значения изменяется только в этом месте ("Найти все ссылки")

![](attachments/Pasted%20image%2020240317235018.png)

По использованию видно что только 17 строка изменяет значение переменной, а в 2х других строках только чтение значения.

Исходя из контекста, можем предположить что в переменной **ij** храниться значение, сколько раз происходит вывод в консоль операции умножения

Переименовываем **ij** в **outputCounter**.

## Продолжаем исследовать код

В настоящий момент он имеет вот такой вид

```cs
static void Main(string[] args)
{
    int firstMultiplier = 2, secondMultiplier = 1, ogri = 0, outputCounter = 1;

    while (secondMultiplier < 10)
    {
        while (firstMultiplier < ogri)
        {
            int multiplicationResult = firstMultiplier * secondMultiplier;

            Console.Write("{0} x {1}= {2}\t", firstMultiplier, secondMultiplier, multiplicationResult);
            
            ++firstMultiplier;
            ++outputCounter;
        }

        if (outputCounter < 33)
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
            if (outputCounter == 33)
            {
                Console.WriteLine();
                secondMultiplier = 2;
            }
        }
    }

    Console.ReadKey();
}
```

## Работа с окном "Контрольные значения"

### Добавление переменных для отслеживания

Для удобства наблюдения добавим все интересующие нас переменные в контрольные значения

Для этого во время отладки нужно навести курсор на интересующую переменную нажать ПКМ и в контекстном меню выбрать "Добавить контрольное значение"

![](attachments/Pasted%20image%2020240319000355.png)
После этого значение этой переменной должно отобразиться в окне "Контрольные значения 1"

![](attachments/Pasted%20image%2020240319000544.png)



### Где найти окно "Контрольные значения"
Обычно VS сама делает данное окно активным и выводит его на передний план, но это окно также можно отобразить самому

**Отладка -> Окна -> Контрольные значения -> Контрольные значения 1**

![](attachments/Pasted%20image%2020240319000709.png)

Добавим все интересующие нас переменные для отслеживания

```
firstMultiplier secondMultiplier ogri outputCounter multiplicationResult
```

![](attachments/Pasted%20image%2020240319001423.png)


Как мы видим для переменной multiplicationResult выводится ошибка, 

| Имя                  | Значение                                                                    | Тип |
| -------------------- | --------------------------------------------------------------------------- | --- |
| multiplicationResult | error CS0103: Имя "multiplicationResult" не существует в текущем контексте. |     |

Если посмотреть на желтую стрелку - то она указывает на строку 13, а переменная multiplicationResult объявляется только на 16 строке.

Нажмем пару раз F10

![](attachments/Pasted%20image%2020240319001716.png)

Ошибка пропала

| Имя                  | Значение | Тип |
| -------------------- | -------- | --- |
| multiplicationResult | 0        | int |

Нажмем пару раз F10

![](attachments/Pasted%20image%2020240319001819.png)

||Имя|Значение|Тип|
|---|---|---|---|
||firstMultiplier|2|int|
||secondMultiplier|2|int|
||multiplicationResult|4|int|

И мы замечаем что ее значение изменилось на 4

### Добавление произвольного кода в окно контрольных значений

Если присмотреться - то на последней строке таблицы виднеется какая то надпись

![](attachments/Pasted%20image%2020240319002028.png)

Расширим столбец **Имя**, чтобы ее прочитать

![](attachments/Pasted%20image%2020240319002146.png)

```
Добавить  элемент в контрольное значение
```

Если по данной строке щелкнуть 2 раза мышью - то надпись пропадет  и мы сможем набрать текст

![](attachments/Pasted%20image%2020240319002311.png)

Наберем `2 * 2` и нажмем **Enter**

![](attachments/Pasted%20image%2020240319002415.png)

Видим, что среда вычислила результат выражения и отобразила его в столбце Значение

Также мы можем набирать длинные выражения, использовать переменные из контекста или даже вызывать функции в этой строке

![](attachments/Pasted%20image%2020240319002707.png)

А во время набора текста доступна среда с подсказками

![](attachments/Pasted%20image%2020240319002753.png)

||Имя|Значение|Тип|
|---|---|---|---|
||firstMultiplier * secondMultiplier|4|int|

**Примечание** Можно в любой момент поправить текст в любой строке таблицы окна **Контрольные значения** или удалить ненужные строки (клавиша Delete)

Мы добавили нужные нам переменные.

Также для нас представляет большой интерес и условия которые есть в условных операторах и циклах

```cs
while (secondMultiplier < 10) //Это
{
    while (firstMultiplier < ogri) //Это
    {
        int multiplicationResult = firstMultiplier * secondMultiplier;

        Console.Write("{0} x {1}= {2}\t", firstMultiplier, secondMultiplier, multiplicationResult);
        
        ++firstMultiplier;
        ++outputCounter;
    }

    if (outputCounter < 33) //Это

```

Добавим же их тоже в число отслеживаемых - просто выделяем нужные куски кода и копируем в окно Контрольные значения

Вот что мы отслеживаем в данный момент

||Имя|Значение|Тип|
|---|---|---|---|
||firstMultiplier|2|int|
||secondMultiplier|2|int|
||multiplicationResult|4|int|
||ogri|6|int|
||outputCounter|1|int|
||firstMultiplier * secondMultiplier|4|int|
||secondMultiplier < 10|true|bool|
||firstMultiplier < ogri|true|bool|
||outputCounter < 33|true|bool|

Во время пошаговой отладки значения в этой таблице будут постоянно обновляться и это сильно поможет в дальнейшем разборе.

Перезапускаем отладку - идем пошагово - пытаемся обращать внимание на значения переменных и на то при каких их значениях куда желтая стрелка заходит и куда не заходит

Во время отладки окно IDE  выходит на передний план и перекрывает окно консоли - можно просто разместить их поблизости на одном экране, чтобы было удобнее наблюдать, что в настоящий момент происходит в консоли


![](attachments/Pasted%20image%2020240319004328.png)

# Логирование отладки - окно вывода

Правда в том, что приведенный пример кода довольно запутан и даже если следовать рекомендациям выше - довольно сложно уловить все нюансы происходящего. 

Придется напрягать память или вести дополнительные записи куда либо в те моменты - когда происходит переключения значений переменных или когда указатель текущей команды(желтая стрелка) приближаются к условными операторам или циклам / входит или не входит в их блоки кода.

Чтобы досконально уловить все эти переходы - можно вести логи прямо в программе, при этом не изменяя исходный код самой программы.

Для этого нам необходимо установить в нужных местах точки останова с особыми настройками - нажать галочку "Действия" - станет доступна опция "Показать сообщение в окне вывода" - в этом поле мы может определить формат лога для этой точки останова.
Также Следует установить галочку


![](attachments/Pasted%20image%2020240319005256.png)

## Формат логов для точки останова

```
Текст {код1} еще текст {код2}
```

Принцип тот же, что и при интерполяции строк 

```cs
string message = $"Text {variable1}";
```

Составим первый вариант строки лога

```
firstMultiplier = {firstMultiplier}, secondMultiplier = {secondMultiplier}, ogri = {ogri}, outputCounter = {outputCounter}
```

![](attachments/Pasted%20image%2020240319010602.png)

Запускаем отладку

![](attachments/Pasted%20image%2020240319010718.png)

Мы видим, что как только желтая стрелка указала на 15 строку где стоит точка останова - то в окне вывод добавилась новая запись

![](attachments/Pasted%20image%2020240319010739.png)

## Окно вывода

Присмотримся к нему поближе

![](attachments/Pasted%20image%2020240319011045.png)

Сюда стекается самая разная информация во время работы нашей программы - в том числе и логи с наших точек останова

## Где найти окно "Вывод"

Вид -> Вывод

![](attachments/Pasted%20image%2020240319011300.png)

На выходе получим вот такие логи

### Логи c первой точки останова

```
firstMultiplier = 2, secondMultiplier = 2, ogri = 6, outputCounter = 1
firstMultiplier = 3, secondMultiplier = 2, ogri = 6, outputCounter = 2
firstMultiplier = 4, secondMultiplier = 2, ogri = 6, outputCounter = 3
firstMultiplier = 5, secondMultiplier = 2, ogri = 6, outputCounter = 4
firstMultiplier = 2, secondMultiplier = 3, ogri = 6, outputCounter = 5
firstMultiplier = 3, secondMultiplier = 3, ogri = 6, outputCounter = 6
firstMultiplier = 4, secondMultiplier = 3, ogri = 6, outputCounter = 7
firstMultiplier = 5, secondMultiplier = 3, ogri = 6, outputCounter = 8
firstMultiplier = 2, secondMultiplier = 4, ogri = 6, outputCounter = 9
firstMultiplier = 3, secondMultiplier = 4, ogri = 6, outputCounter = 10
firstMultiplier = 4, secondMultiplier = 4, ogri = 6, outputCounter = 11
firstMultiplier = 5, secondMultiplier = 4, ogri = 6, outputCounter = 12
firstMultiplier = 2, secondMultiplier = 5, ogri = 6, outputCounter = 13
firstMultiplier = 3, secondMultiplier = 5, ogri = 6, outputCounter = 14
firstMultiplier = 4, secondMultiplier = 5, ogri = 6, outputCounter = 15
firstMultiplier = 5, secondMultiplier = 5, ogri = 6, outputCounter = 16
firstMultiplier = 2, secondMultiplier = 6, ogri = 6, outputCounter = 17
firstMultiplier = 3, secondMultiplier = 6, ogri = 6, outputCounter = 18
firstMultiplier = 4, secondMultiplier = 6, ogri = 6, outputCounter = 19
firstMultiplier = 5, secondMultiplier = 6, ogri = 6, outputCounter = 20
firstMultiplier = 2, secondMultiplier = 7, ogri = 6, outputCounter = 21
firstMultiplier = 3, secondMultiplier = 7, ogri = 6, outputCounter = 22
firstMultiplier = 4, secondMultiplier = 7, ogri = 6, outputCounter = 23
firstMultiplier = 5, secondMultiplier = 7, ogri = 6, outputCounter = 24
firstMultiplier = 2, secondMultiplier = 8, ogri = 6, outputCounter = 25
firstMultiplier = 3, secondMultiplier = 8, ogri = 6, outputCounter = 26
firstMultiplier = 4, secondMultiplier = 8, ogri = 6, outputCounter = 27
firstMultiplier = 5, secondMultiplier = 8, ogri = 6, outputCounter = 28
firstMultiplier = 2, secondMultiplier = 9, ogri = 6, outputCounter = 29
firstMultiplier = 3, secondMultiplier = 9, ogri = 6, outputCounter = 30
firstMultiplier = 4, secondMultiplier = 9, ogri = 6, outputCounter = 31
firstMultiplier = 5, secondMultiplier = 9, ogri = 6, outputCounter = 32
firstMultiplier = 6, secondMultiplier = 2, ogri = 10, outputCounter = 33
firstMultiplier = 7, secondMultiplier = 2, ogri = 10, outputCounter = 34
firstMultiplier = 8, secondMultiplier = 2, ogri = 10, outputCounter = 35
firstMultiplier = 9, secondMultiplier = 2, ogri = 10, outputCounter = 36
firstMultiplier = 6, secondMultiplier = 3, ogri = 10, outputCounter = 37
firstMultiplier = 7, secondMultiplier = 3, ogri = 10, outputCounter = 38
firstMultiplier = 8, secondMultiplier = 3, ogri = 10, outputCounter = 39
firstMultiplier = 9, secondMultiplier = 3, ogri = 10, outputCounter = 40
firstMultiplier = 6, secondMultiplier = 4, ogri = 10, outputCounter = 41
firstMultiplier = 7, secondMultiplier = 4, ogri = 10, outputCounter = 42
firstMultiplier = 8, secondMultiplier = 4, ogri = 10, outputCounter = 43
firstMultiplier = 9, secondMultiplier = 4, ogri = 10, outputCounter = 44
firstMultiplier = 6, secondMultiplier = 5, ogri = 10, outputCounter = 45
firstMultiplier = 7, secondMultiplier = 5, ogri = 10, outputCounter = 46
firstMultiplier = 8, secondMultiplier = 5, ogri = 10, outputCounter = 47
firstMultiplier = 9, secondMultiplier = 5, ogri = 10, outputCounter = 48
firstMultiplier = 6, secondMultiplier = 6, ogri = 10, outputCounter = 49
firstMultiplier = 7, secondMultiplier = 6, ogri = 10, outputCounter = 50
firstMultiplier = 8, secondMultiplier = 6, ogri = 10, outputCounter = 51
firstMultiplier = 9, secondMultiplier = 6, ogri = 10, outputCounter = 52
firstMultiplier = 6, secondMultiplier = 7, ogri = 10, outputCounter = 53
firstMultiplier = 7, secondMultiplier = 7, ogri = 10, outputCounter = 54
firstMultiplier = 8, secondMultiplier = 7, ogri = 10, outputCounter = 55
firstMultiplier = 9, secondMultiplier = 7, ogri = 10, outputCounter = 56
firstMultiplier = 6, secondMultiplier = 8, ogri = 10, outputCounter = 57
firstMultiplier = 7, secondMultiplier = 8, ogri = 10, outputCounter = 58
firstMultiplier = 8, secondMultiplier = 8, ogri = 10, outputCounter = 59
firstMultiplier = 9, secondMultiplier = 8, ogri = 10, outputCounter = 60
firstMultiplier = 6, secondMultiplier = 9, ogri = 10, outputCounter = 61
firstMultiplier = 7, secondMultiplier = 9, ogri = 10, outputCounter = 62
firstMultiplier = 8, secondMultiplier = 9, ogri = 10, outputCounter = 63
firstMultiplier = 9, secondMultiplier = 9, ogri = 10, outputCounter = 64
```

Глядя на их уже можно увидеть некоторые закономерности - но картину полностью это все еще не проясняет

Что делать?

Сделать больше точек останова с логами и для каждой скорректировать запись

**Примечание** точки останова можно устанавливать только в строках, где есть какие либо операторы - если требуется добавить большее количество - то можно прибегнуть к хитрости и добавить пустых операторов (;)

```cs
 static void Main(string[] args)
 {
     int firstMultiplier = 2, secondMultiplier = 1, ogri = 0, outputCounter = 1;
     ; //например вот сюда
     while (secondMultiplier < 10)
```

Расположение точек останова у нас будет выглядеть вот так

![](attachments/Pasted%20image%2020240319013318.png)

Точка останова на строке с пустым оператором `;` будет выводить значения переменных после инициализации (Строка 11).

```
init | firstMultiplier = {firstMultiplier}, secondMultiplier = {secondMultiplier}, ogri = {ogri}, outputCounter = {outputCounter}
```

Точки останова которые находятся на строке с открывающим блоком кода (`{`) - будут нести информацию об условии которое проверялось выше (строки 13, 15, 25, 32 ).

```
while (secondMultiplier < 10) | {secondMultiplier < 10}
while (firstMultiplier < ogri) | {firstMultiplier < ogri}
if (outputCounter < 33) | {outputCounter < 33}
else (outputCounter < 33) | {outputCounter < 33}
```

Точки останова которые находятся на строке с закрывающим блоком кода (`}`) - будут выводить текущие значения переменных (строки 22, 43).

```
inner while | firstMultiplier = {firstMultiplier}, secondMultiplier = {secondMultiplier}, multiplicationResult = {multiplicationResult}, outputCounter = {outputCounter}
outer while | firstMultiplier = {firstMultiplier}, secondMultiplier = {secondMultiplier}, ogri = {ogri}, outputCounter = {outputCounter}
```


## Листинг логов из окна вывод


Лишние сообщения можно удалить

```
"Example_2_Debug.exe" (CoreCLR: clrhost). Загружено "C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.25\System.Text.Encoding.Extensions.dll". Загрузка символов пропущена. Модуль оптимизирован, включен параметр отладчика "Только мой код".

"Example_2_Debug.exe" (CoreCLR: clrhost). Загружено "C:\Program Files\dotnet\shared\Microsoft.NETCore.App\6.0.25\System.Collections.Concurrent.dll". Загрузка символов пропущена. Модуль оптимизирован, включен параметр отладчика "Только мой код".
```


Для удобства их разбора можно их скопировать например в редактор Notepad++ (бесплатный) - и раскрасить в нем цветами текст, чтобы было проще его читать


![](attachments/Pasted%20image%2020240319015503.png)

**Примечание** если хотите покрасить все вхождения данного текста используйте кнопку **Style all occurrences of token** (она на позицию выше - и именно ее использовал автор в данном примере)

![](attachments/Pasted%20image%2020240319020529.png)

Если грамотно использовать цвета - то проще буде разбирать записи. 

Вывод данных я закрашивать не стал, условные и циклы я покрасил в разные цвета


![](attachments/Pasted%20image%2020240319020816.png)

При отдалении становится заметно - что внутренний цикл отрабатывает 4 раза подряд, при этом в нем меняется первый множитель, но второй остается неизменным

![](attachments/Pasted%20image%2020240319021105.png)

Далее отрабатывает условие которое проверяет (outputCounter < 33) и сбрасывает первый множитель в 2, а второй инкрементирует при этом ogri = 6

```cs
if (outputCounter < 33)
{
    ogri = 6;
    Console.WriteLine();
    secondMultiplier++;
    firstMultiplier = 2;
}
```

Переменная ogri все еще остается загадкой - можно и ее покрасить `ogri = 6`


![](attachments/Pasted%20image%2020240319022321.png)

Листаем вниз и видим, что на 92 строке `ogri = 10`

Смотрим на 90 строку `outputCounter = 33` а `firstMultiplier = 6`

```
inner while | firstMultiplier = 6, secondMultiplier = 9, multiplicationResult = 45, outputCounter = 33
```

Подсветим также и `inner while | firstMultiplier = 6`

![](attachments/Pasted%20image%2020240319022949.png)

А если смотреть ниже 90 строчки - то она больше нигде и не встречается

![](attachments/Pasted%20image%2020240319023047.png)

Еще раз посмотрим за поведением переменной firstMultiplier до 90 строки

Значения 2 3 4 5 6

![](attachments/Pasted%20image%2020240319023238.png)

Значения 2 3 4 5 6 и опять по новой, при этом каждый раз происходив вход в условный оператор, где значение сбрасывается в 2

```cs
if (outputCounter < 33)
{
    ogri = 6;
    Console.WriteLine();
    secondMultiplier++;
    firstMultiplier = 2;
}
```

![](attachments/Pasted%20image%2020240319023327.png)

Эта картина повторяется вплоть до 90 строчки, при этом переменная ogri = 6

![](attachments/Pasted%20image%2020240319023419.png)

Посмотрим как дела обстоят дальше

Подсветим `inner while | firstMultiplier = 10` и `ogri = 10`

![](attachments/Pasted%20image%2020240319024008.png)

Собственно та же картина наблюдается только значения в диапазоне от 6 включительно до 10 исключительно

**Вывод**

ogri  является ограничением для переменной firstMultiplier - в последствии переименуем ее в firstMultiplierLimit

Следующий вопрос - зачем нужен outputCounter?

Если присмотреться - то видно что до 90 строки вызывался блок кода






----

| Навигация                 |                                             |                           |
| ------------------------- | ------------------------------------------- | ------------------------- |
| [example_1](example_1.md) | [К списку примеров](debug_examples_list.md) | [example_3](example_3.md) |
