
# Целевая задача

Вывести на экран 10 раз надпись `Hello, World!`

# Пример кода

```cs

namespace Example_1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int times = 10;

            for (int i = 0; i > times; i++)
            {
                Console.WriteLine("Hello, World!");
            }
        }
    }
}
```

# Результат работы

![](attachments/Pasted%20image%2020240301183048.png)

Можно заметить что программа ничего не вывела

# Процесс отладки

## Поиск проблемы

Запустить отладку и определить заходит ли исполнение кода в тело цикла.

Поставить точку останова в теле цикла


![](attachments/Pasted%20image%2020240301184434.png)

Перед запуском убеждаемся что исходный код сохранен и программа пересобрана с использованием актуальных исходников - кнопки **Собрать решение** или **Пересобрать решение** в разделе **Сборка**.
![](attachments/Pasted%20image%2020240301185201.png)

Проверяем, что выбрана отладочная сборка (слева) и запускаем с использованием отладки

![](attachments/Pasted%20image%2020240301184711.png)

На зеленую стрелку (справа)

![](attachments/Pasted%20image%2020240301184838.png)

Или на кнопку начать отладку.

Остановки в точке останова не произошло
Смотрим на вывод программы

![](attachments/Pasted%20image%2020240301185918.png)

Добавим точку останова в начале

![](attachments/Pasted%20image%2020240301190242.png)

Запускаем

![](attachments/Pasted%20image%2020240301190329.png)

Видим появление желтой стрелки - это указатель на текущий выполняемый оператор

Нажимаем F10(шаг с обходом) или F11(шаг с заходом) для перехода к исполнению к следующей команды.

![](attachments/Pasted%20image%2020240301190516.png)

Как видим указатель команды переместился на строку ниже

Нажмем  еще пару раз F10

![](attachments/Pasted%20image%2020240301193313.png)

Вот на этом моменте задержимся поподробнее

### Закрепление значения переменных в IDE

![](attachments/Pasted%20image%2020240301193604.png)

Если навести курсор мыши на переменную - то можно увидеть ее текущее значение
Справа от значения есть кнопка "кнопка" - если на нее нажать то значение переменной будет закреплено на экране.
В данном случае значения i и times закреплены справа от объявления функции Main

Как мы видим

```cs
0 > 10
```

будет возвращать нам false

Если мы еще раз нажмем F10 то желтая стрелка уйдет ниже к закрывающей скобке метода - пропуская при этом тело цикла 

![](attachments/Pasted%20image%2020240301194749.png)


Останавливаем отладку нажав на иконку с красным квадратом - Остановить отладку (Shift + F5)

![](attachments/Pasted%20image%2020240301195139.png)
Тут
![](attachments/Pasted%20image%2020240301195224.png)
Или тут

------

## Исправление

Заменим оператор > на < в строке с оператором цикла
Было
```cs
for (int i = 0; i > times; i++)
```

Стало

```cs
for (int i = 0; i < times; i++)
```

Сохранимся и пересоберем программу

Результат следующего запуска

![](attachments/Pasted%20image%2020240301200521.png)

Как мы видим - ошибку удалось локализовать и исправить

----

| Навигация |                                             |                           |
| --------- | ------------------------------------------- | ------------------------- |
|           | [К списку примеров](debug_examples_list.md) | [example_2](example_2.md) |
