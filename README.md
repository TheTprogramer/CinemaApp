# Cinema App — Трислойно приложение

## Описание
Приложение за управление на кино, разработено като трислойна архитектура в C# / .NET 8. Включва конзолен интерфейс с луксозен дизайн и уеб интерфейс (ASP.NET Razor Pages).

---

## Структура на проекта

```
CinemaApp/
├── CinemaApp.sln
├── CinemaApp.Data/                  ← Слой за данни
│   ├── Models/                      ← Entity класове
│   │   ├── Actor.cs
│   │   ├── Film.cs
│   │   ├── Room.cs
│   │   ├── Row.cs
│   │   ├── Seat.cs
│   │   ├── Ticket.cs
│   │   ├── Place.cs
│   │   ├── Project.cs
│   │   ├── TypeDrinkFood.cs
│   │   ├── Food.cs
│   │   ├── Drink.cs
│   │   └── Client.cs
│   └── Context/
│       └── CinemaDbContext.cs       ← EF DbContext + Seed Data
│
├── CinemaApp.Services/              ← Слой за услуги (бизнес логика)
│   ├── Interfaces/
│   │   ├── IActorService.cs
│   │   ├── IFilmService.cs
│   │   ├── IProjectService.cs
│   │   ├── IClientService.cs
│   │   └── ITicketService.cs
│   └── Implementations/
│       ├── ActorService.cs
│       ├── FilmService.cs
│       ├── ProjectService.cs
│       ├── ClientService.cs
│       └── TicketService.cs
│
├── CinemaApp.ConsoleUI/             ← Конзолен презентационен слой
│   ├── Program.cs
│   ├── ConsoleHelper.cs             ← Луксозен дизайн (злато на тъмно)
│   └── Menus/
│       ├── ActorsMenu.cs
│       ├── FilmsMenu.cs
│       ├── ProjectsMenu.cs
│       ├── ClientsMenu.cs
│       └── TicketsMenu.cs
│
├── CinemaApp.Web/                   ← Уеб презентационен слой (Razor Pages)
│   ├── Program.cs
│   ├── Pages/
│   │   ├── Index.cshtml
│   │   ├── Actors/        (Index, Create, Edit, Delete, Details, WithFilms)
│   │   ├── Films/         (Index, Create, Edit, Delete, Details, Shortest, Longest, WithDetails)
│   │   ├── Projects/      (Index, Create, Edit, Delete, Details, ByFilm, ByDay)
│   │   ├── Clients/       (Index, Create, Edit, Delete, Details, Queries)
│   │   ├── Tickets/       (Index, Create, Edit, Delete, Details, FreePlaces, TotalSum)
│   │   └── Shared/
│   │       └── _Layout.cshtml
│   └── wwwroot/
│       └── css/
│           └── site.css
│
└── CinemaApp.Tests/                 ← Компонентни тестове (NUnit)
    ├── TestBase.cs
    ├── ActorServiceTests.cs
    ├── FilmServiceTests.cs
    ├── ProjectServiceTests.cs
    ├── ClientServiceTests.cs
    └── TicketServiceTests.cs
```

---

## Технологии

| Технология | Версия | Цел |
|---|---|---|
| .NET | 8.0 | Основна платформа |
| Entity Framework Core | 8.0 | ORM за база данни |
| Pomelo.EFCore.MySql | 8.0 | MySQL провайдър за EF |
| Microsoft.Extensions.DependencyInjection | 8.0 | Dependency Injection |
| ASP.NET Razor Pages | 8.0 | Уеб интерфейс |
| Bootstrap | 5.3 | CSS рамка за уеб UI |
| NUnit | 4.1 | Unit тестове |
| EFCore.InMemory | 8.0 | In-memory DB за тестове |
| Moq | 4.20 | Mocking за тестове |

---

## База данни
Приложението използва MySQL база данни **cinema** с 12 таблици.

> ⚠️ **Не е нужен SQL файл!** Базата се създава автоматично при първо стартиране чрез `EnsureCreated()` и Seed Data в `CinemaDbContext`.

| Таблица | Описание | Записи |
|---|---|---|
| **actors** | Актьори | 6 |
| **films** | Филми | 10 |
| **rooms** | Зали (2D, 3D, 4DX, ScreenX) | 4 |
| **rowss** | Редове в зала (normal / VIP) | 9 |
| **seats** | Седалки | 15 |
| **tickets** | Билети | 354 |
| **places** | Места (зала + ред + седалка + билет) | 354 |
| **projects** | Прожекции (филм + зала + ден + час) | 112 |
| **types_drinks_foods** | Типове (Малко / Средно / Голямо) | 3 |
| **foods** | Храни от менюто | 30 |
| **drinks** | Напитки от менюто | 30 |
| **clients** | Клиенти (билет + храна + напитка) | 150 |

---

## Инсталация и стартиране

### 1. Изисквания
- .NET 8 SDK
- MySQL Server (5.7+ или 8.x)
- Visual Studio 2022 / VS Code / Rider

### 2. Базата данни се създава автоматично
Не е необходим SQL файл. При първо стартиране приложението автоматично:
- Създава базата данни **cinema**
- Създава всичките 12 таблици
- Вмъква всичките данни (Seed Data)

### 3. Въвеждане на парола
При стартиране приложението пита за MySQL парола:
```
╔══════════════════════════════════════════╗
║         CINEMA APP — СВЪРЗВАНЕ           ║
╠══════════════════════════════════════════╣
║  MySQL парола › ********
╚══════════════════════════════════════════╝
```
Не е нужно да редактирате никакъв файл.

### 4. Стартиране на конзолния UI
```bash
cd CinemaApp/CinemaApp.ConsoleUI
dotnet run
```

### 5. Стартиране на уеб UI
```bash
cd CinemaApp/CinemaApp.Web
dotnet run
# Отворете браузъра на https://localhost:5001
```

### 6. Стартиране на тестовете
```bash
cd CinemaApp/CinemaApp.Tests
dotnet test
```

---

## Функционалности

### Актьори
- Преглед на всички актьори
- Търсене по ID
- Добавяне / Редактиране / Изтриване
- Филтриране по минимална възраст
- Преглед с техните филми

### Филми
- Пълен CRUD
- Филтриране по жанр
- Филтриране по минимално времетраене
- Най-кратките 3 филма
- Най-дългият филм
- Детайлен преглед с актьори и прожекции

### Прожекции
- Пълен CRUD
- Търсене по заглавие на филм
- Търсене по ден от седмицата
- Детайлен преглед с филм и зала

### Клиенти
- Пълен CRUD
- Филтриране по ден
- Брой продадени билети за ден
- Приходи за ден (билет + храна + напитка)
- Приходи за цялата седмица
- Клиенти с еднакви разходи

### Билети
- Пълен CRUD
- Обща сума на продадените билети
- Преглед на свободните места

---

## Архитектурни решения

**Трислойна архитектура:**
1. **Data Layer** — EF модели, DbContext, навигационни свойства, composite PK за `places`, пълен Seed Data за автоматично създаване на базата
2. **Service Layer** — Всяка услуга имплементира интерфейс; async/await навсякъде
3. **Presentation Layer** — Два независими UI-а (ConsoleUI с луксозен дизайн + Razor Pages с Bootstrap 5)

**Автоматично създаване на базата** — чрез `EnsureCreated()` и `HasData()` в `OnModelCreating()`. При първо стартиране базата, таблиците и данните се създават без нужда от SQL файл.

**Въвеждане на парола при стартиране** — паролата не е хардкодирана в кода; въвежда се интерактивно при всяко стартиране.

**Dependency Injection** — чрез `Microsoft.Extensions.DependencyInjection` в ConsoleUI и вградения DI на ASP.NET в Web проекта.

**Тестове** — използват in-memory база данни за пълна изолация; тестват реалните имплементации.

---

## Покритие с тестове

| Клас | Брой тестове |
|---|---|
| ActorService | 10 |
| FilmService | 13 |
| ProjectService | 12 |
| ClientService | 13 |
| TicketService | 11 |
| **Общо** | **59** |

Покритие: **> 79%** от Service Layer кода.

---

## Статистика на проекта

| Метрика | Стойност |
|---|---|
| Общо файлове | ~139 |
| Общо редове код | ~7 228 |
| Проекти в solution | 5 |
| Таблици в базата | 12 |
| Общо записи (Seed Data) | 1 081 |
| Unit тестове | 59 |

---

## Автори
Разработено като курсов проект по C# / .NET от Аслъ Караджа, Денислав Стаменов и Тодор Димитров. 
Проектът демонстрира умения в трислойна архитектура, EF Core, ASP.NET Razor Pages и Unit Testing.
