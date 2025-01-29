# InsertMeSoft

<h1 align="center">Утилита для заливки в тестовую Базу данных выборки по СНИЛС из другой БД.</h1>

<hr>

## Проект реализован с использованием
C# = Java = DB2 for z/OS

![](/design/icons/cs.png)&emsp;![](/design/icons/Java.png)&emsp;![ ](/design/icons/db2.png)

<hr>

## Назначение утилиты: 

1. Создание скрипта для получения выборки по СНИЛС / нескольких СНИЛС.
2. Обработка выборки данных по СНИЛС из одной БД, создание скрипта для заливки (набор Insert'ов) этих данных на другую БД.
3. Заливка выборки на стенд.
4. Копирование (а также множественное клонирование) СНИЛС с одного стенда на другой (или на тот же стенд) с возможностью изменения номера СНИЛС.
5. Используется для заливки данных со стендов ПФР на наши тестовые стенды или между нашими стендами.

<hr>
