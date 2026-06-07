# Домашнее задание №2, вариант 22

Консольное приложение находится в папке `Homework-2`. Оно написано на C#/.NET и работает с базой ресторанов и блюд в меню. Данные хранятся в SQLite, начальная загрузка выполняется из CSV-файлов, а отчёты строятся классом `ReportBuilder` по паттерну Fluent Interface.

## Структура проекта

```text
.
├── Homework-2/
│   ├── ASOIU-2.csproj
│   ├── ASOIU-2.sln
│   ├── Program.cs
│   ├── DatabaseManager.cs
│   ├── ReportBuilder.cs
│   ├── Restaurant.cs
│   ├── MenuItem.cs
│   ├── restaurants.csv
│   ├── menu_items.csv
│   └── homework2.pdf
├── .gitignore
└── README.md
```

## Предметная область

- Справочная таблица: рестораны.
- Основная таблица: блюда в меню.
- Числовое поле: `price`.
- Расшифровка: цена блюда в рублях.

## Структура таблиц

`restaurants`:

- `restaurant_id INTEGER PRIMARY KEY`
- `restaurant_name TEXT NOT NULL`

`menu_items`:

- `dish_id INTEGER PRIMARY KEY`
- `restaurant_id INTEGER NOT NULL`
- `dish_name TEXT NOT NULL`
- `price INTEGER NOT NULL`
- `FOREIGN KEY (restaurant_id) REFERENCES restaurants(restaurant_id)`

## CSV-файлы

В проекте есть стартовые файлы:

- `restaurants.csv` — список ресторанов.
- `menu_items.csv` — список блюд.

Разделитель в CSV-файлах — `;`, первая строка содержит заголовки.

## Запуск

```bash
cd ./Homework-2/Homework-2 && dotnet run
```

При первом запуске в папке `Homework-2` создаётся база `restaurants.db`, таблицы и начальные данные из CSV.

## Пункты меню

1. Показать все рестораны.
2. Показать все блюда.
3. Добавить блюдо.
4. Редактировать блюдо.
5. Удалить блюдо.
6. Отчёты.
7. Экспорт в CSV.
0. Выход.

## Отчёты

В подменю отчётов реализованы:

1. Полный список блюд с названиями ресторанов.
2. Количество блюд в каждом ресторане.
3. Средняя цена блюд по ресторанам.

Отчёты используют SQL-запросы с `JOIN`, `GROUP BY`, `COUNT(*)`, `AVG(price)` и сортировкой.

## Файлы в results

Папка `Homework-2/results` создаётся автоматически. В неё сохраняются:

- `report1.txt`
- `report2.txt`
- `report3.txt`
- `restaurants_export.csv`
- `menu_items_export.csv`

## Основные классы

- `Restaurant` — модель ресторана.
- `MenuItem` — модель блюда с валидацией цены.
- `DatabaseManager` — работа с SQLite, CSV, CRUD и SQL-запросами.
- `ReportBuilder` — построение, вывод и сохранение отчётов.
