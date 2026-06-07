# Task12_LibraryOOP

Текстовое описание диаграммы классов:

```text
LibraryItem (abstract)
├── Book
└── Magazine

Library (partial)
├── Library.Data.cs: Add, PrintAll
└── Library.Search.cs: FindByTitle, FindBooks, FindMagazines

LibraryItemExtensions
├── IsNew
├── ToCsvLine
└── PrintCard
```
