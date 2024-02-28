# Термины

![](/attachments/Pasted%20image%2020240228220745.png)

**Баг** (_bug_ — жук, мелкое насекомое) — распространенное среди программистов название _ошибок_ в программах.

Следует отличать баг от глюка, который есть не что иное как симптом бага, вызывающий у пользователя нервно-истерическую реакцию.

----

**Отладка** — этап разработки компьютерной программы, на котором обнаруживают, локализуют и устраняют ошибки. Чтобы понять, где возникла ошибка, приходится:

Существуют две взаимодополняющие технологии отладки:

- **Использование отладчиков** 
- **Вывод текущего состояния программы** с помощью расположенных в критических точках программы операторов вывода — на экран или в файл. Вывод отладочных сведений в файл называется журналированием (логированием).

----

**Отладчик** (debugger от bug) — компьютерная программа для автоматизации процесса отладки: поиска ошибок в других программах, ядрах операционных систем, SQL-запросах и других видах кода. 

# Базовые принципы

## Представление - реальность

![](/attachments/1588521136128931146.png)

[Оригинал](https://pikabu.ru/story/otladka_programmyi_7596295)

Для начала мы попытаемся сформулировать ряд базовых принципов, связанных с отладкой, и сделаем это в такой форме, что они, возможно, покажутся вам шуткой; вы вскоре сами убедитесь, что в данной шутке доля шутки совсем незначительна, а всё остальное - самая настоящая правда. 

Итак:

- ошибка всегда есть;

- ошибка всегда не там;

- если вы точно знаете, где ошибка, то у ошибки может оказаться другое мнение;

- если вы считаете, что программа должна работать, то самое время вспомнить, что "должен" - это когда взял взаймы и не отдал;

- если отладка - это процесс исправления ошибок, то написание программы - это процесс их внесения;

- сразу после обнаружения ошибки дело всегда выглядит безнадёжным;

- найденная ошибка всегда кажется глупой;

- чем безнадёжнее всё выглядело, тем глупее кажется найденная ошибка;

- компьютер делает не то, что вы хотите, а то, о чём вы попросили;

- корректная программа работает правильно в любых условиях, некорректная - тоже иногда работает;

- и лучше бы она не работала;

- если программа работает, то это ещё ничего не значит;

- если программа "свалилась", надо радоваться: ошибка себя проявила, значит её теперь можно найти;

- чем громче грохот и ярче спецэффекты при "падении" программы, тем лучше - заметную ошибку искать гораздо проще;

- если ошибка в программе точно есть, а программа все-таки работает, вам не повезло - это самый противный случай;

- ни компилятор, ни библиотека, ни операционная система ни в чем не виноваты;

- никто не хочет вашей смерти, но если что - никто не расстроится;

- на самом деле всё совсем не так плохо - всё гораздо хуже;

- первая написанная строчка текста будущей программы делает этап отладки неизбежным;

- если вы не готовы к отладке - не начинайте программировать;

- компьютер не взорвётся; но большего вам никто не обещал.

# Навигация

![](/attachments/Pasted%20image%2020240228220902.png)

| Разделы                                             |
| --------------------------------------------------- |
| [Что такое отладка](what_is_debug/what_is_debug.md) |
| [Полезные ссылки](useful_links/useful_links.md)     |
