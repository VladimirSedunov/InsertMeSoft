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

Используется для заливки данных со стендов Заказчика на наши тестовые стенды или между нашими стендами.


#### _Примечание:_ _в настоящее время существует WEB-версия этой утилиты_
<hr>

## Внешний вид:
* Многие визуальные элементы (кнопки, поля, чекбоксы и т.п.) снабжены всплывающими подсказками.
* Для работы используются вкладки "V5" и "Копия".
* Вкладка "V5", предназначенная для обработки выборки с последующим формированием скрипта заливки на стенд либо заливки на стенд напрямую из выборки.
* Также здесь расположена панель визуальных элементов "Запрос выборки для АСТП" для формирования и последующей отправки Заказчику скрипта, посредством которого будет сформирована выборка.
* Слева от текстового поля/окна ("Окно 1", "Окно 2") есть кнопки: с веником - очистить окно, с блокнотиком - скопировать в буфер текст в окне, а также чекбокс для переноса строк, которые не умещаются по ширине окна.
  
![ ](/design/images/vid1.png)

### Панель "V5" является основной в работе со СНИЛСами.

Для обработки выборки необходимо 

* Выбрать БД тестового стенда. При этом будет автоматически показана текущая версия выбранной БД. Если вместо версии БД появится "v. неизвестно", необходимо проверить наличие коннекта с БД (включен ли VPN, есть ли доступ к БД, в порядке ли интернет и т.д.).
* Выбрать режим работы с помощью набора чекбоксов:
  * "не все doc_mass" - не загружать "лишние" записи (для ИП). Некоторые выборки по СНИЛСу ИП или крупного предприятия содержат в таблице SPU_DOC_MASS записи с рег.номером этого ИП, но относящиеся к другим ИП (другим снилсам). \
Иногда их очень много (сотни и даже тысячи, если предприятие большое), и они фактически не нужны. В этом случае лучше такие записи не загружать.
  * "Случайные ФИО" - Для СНИЛСов ФИО выбираются случайным образом из файлов, лежащих в папке FIO_Men_Women.
  * "Загружать буферные таблицы" - Инсерты по всем схемам, включая буферные таблицы. Если выключен - только по SPUMST и USPN.
  * "Автозаливка в БД" - После формирования инсёртов они будут автоматически выполнены. То есть, заливка в БД произойдёт без участия пользователя. Работает только в случае, когда заливается ровно один СНИЛС.
* Нажать на кнопку старта (с рисунком зелёной ступни). \
Появится диалоговая панель для выбора файла с выборкой по СНИЛСу. \
Кодировка файла определяется автоматически, но предпочтительна "utf-8 без BOM". \
По мере работы утилиты будут появляться сообщения в Окне 1. \
Если счёт уже есть в БД, прога запрашивает пользователя о дальнейших действиях. При согласии на перезаливку СНИЛС автоматически удаляется из БД и заливается заново.

![ ](/design/images/panelV5.png)

После формирования инсёртов в поле под чекбоксами пишется ACC_ID из обработанной выборки (если счетов несколько, пишутся все через запятую).
Если в БД уже есть загружаемые счета, в отчёт в Окне 1 выводится сообщение об этом.

![ ](/design/images/insert1.png)

Кнопка с синим профилем (см. рис. выше) формирует в окне 2 инсёрты для стандартных анкетных данных по снилсу, указанному в поле справа от этой кнопки (анкета "болванчика").
По умолчанию болванчика зовут Петров Петр Петрович, пол мужской. Если поставить галочку справа, то болванчик будет Ольгина Ольга Олеговна, пол женский.
Дополнительно проверяется, есть ли этот снилс в выбранной БД:

![ ](/design/images/insert2.png)

### Панель "Запрос выборки для АСТП"

![ ](/design/images/zapros.png)

В ней можно:

* Ввести acc_id, и в поле ниже будет рассчитанный СНИЛС (с КС / КЧ). В поле "acc_id" можно копировать и СНИЛС, в этом случае он автоматически сменит формат на acc_id. \
В поле acc_id можно указать множество счетов, разделенных запятыми. В результате сформируются скрипты для всех счетов сразу. Это может понадобиться для заливки СНИЛСов, логически связанных слиянием/преемственностью.
Но лучше не злоупотреблять и больше десятка счетов за раз не указывать (ибо замучаетесь потом заливать).
* Скопировать СНИЛС в буфер, нажав на кнопку с маленьким зеленым плюсиком (если лень жать Ctrl+C)
* Кнопки "Select" и "Delete" формируют в Окне 2 скрипты для выборки либо удаления данных по СНИЛСУ. Если выставлена галка рядом с "Select", которая называется "без ФИО", то скрипт формируется для выборки конфиденциальной, т.е. ФИО заменяются на стандартные ФИО болванчика. \
Если вкючен чекбокс "Для Об/Раз", то кнопки "Select" и "Delete" формируют скрипты для СНИЛСов, по которым есть операции Объединения/Разделения (см. ниже).
* Если нужны данные буфера, можно выбрать в меню "Буфер" нужный вариант скрипта (для общей части, для спец.части, для ДСВ+Софин).
* "РЗ" - указать номер рабочего задания из АСТП (или любую заметку).
* Чекбокс "Для Об/Раз" формирует скрипт для выборки по СНИЛСу, по которому есть операции Объединения/Разделения. Скрипт сам определяет набор СНИЛСов, участвующих в Об/Раз, и по всем им формирует единую выборку. Полученная выборка загружается как обычно.
* Логин/Пароль указать для входа в Почту Софткомпани
* "От" - адрес юзера. "Кому" - выбрать адрес Получателя. Адреса настраиваются в "mail.xml" (в том числе можно указать пустой адрес). \
Если стоит галка, то копия отсылается ещё одному Получателю. Кнопка с конвертом - отправка почты (через браузер).

По запросу формируются селекты на основе файла "v5 sql_SNILS.sql" и дополняются селектами из файла "Буфер_ОЧ.sql", "Буфер_СЧ.sql" или "Буфер_ДСВ.sql" соответственно. \
При удалении СНИЛСа используются скрипты из всех трёх файлов "Буфер_ОЧ delete.sql", "Буфер_СЧ delete.sql", "Буфер_ДСВ delete.sql".

### Копирование СНИЛС

Закладка "Копия". Можно СНИЛС, имеющийся в БД, скопировать с другим номером счета на эту же БД. Или с любым номером на другую БД. \
Копируется вся информация, которая обычно присылается в стандартной выборке.


Для копирования нужно указать БД (откуда и куда) и номера СНИЛСов. Нажать на кнопку. \
Также можно поставить галочку и Создать нужное количество копий СНИЛСа в указанном интервале acc_id.

![ ](/design/images/copy.png)

<hr>

## Примечание:

Проект не может быть выложен на публичный ресурс по соображениям конфиденциальности.
