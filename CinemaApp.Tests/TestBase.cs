using CinemaApp.Data.Context;
using CinemaApp.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace CinemaApp.Tests
{
    /// <summary>
    /// Базов клас за тестове — създава изолирана in-memory база данни с тестови данни.
    /// </summary>
    public abstract class TestBase
    {
        /// <summary>Създава нова in-memory CinemaDbContext с уникално име за изолация между тестове.</summary>
        protected static CinemaDbContext CreateContext(string dbName = "")
        {
            var options = new DbContextOptionsBuilder<CinemaDbContext>()
                .UseInMemoryDatabase(string.IsNullOrEmpty(dbName) ? Guid.NewGuid().ToString() : dbName)
                .Options;
            return new CinemaDbContext(options);
        }

        /// <summary>Сийдва базата с тестови данни и връща контекст.</summary>
        protected static CinemaDbContext CreateSeededContext()
        {
            var context = CreateContext();

            var actor = new Actor { ActorId = 1, FirstName = "Том", LastName = "Харди", Age = 48 };
            context.Actors.Add(actor);

            var film = new Film
            {
                FilmId    = 1,
                FilmName  = "Венъм",
                FilmGenre = "Екшън",
                FilmTime  = 109,
                ActorId   = 1,
            };
            context.Films.Add(film);

            var room = new Room { RoomId = 1, RoomType = "3D" };
            context.Rooms.Add(room);

            var project = new Project
            {
                ProjectId = 1,
                DayOfWeek = "Понеделник",
                StartTime = "14:00",
                EndTime   = "16:00",
                RoomId    = 1,
                FilmId    = 1,
            };
            context.Projects.Add(project);

            var type = new TypeDrinkFood { TypeId = 1, TypeTitle = "Средно", TypePrice = 10m };
            context.TypeDrinkFoods.Add(type);

            var food = new Food { FoodId = 1, FoodTitle = "Пуканки", TypeId = 1 };
            context.Foods.Add(food);

            var drink = new Drink { DrinkId = 1, DrinkTitle = "Кока-Кола", TypeId = 1 };
            context.Drinks.Add(drink);

            var ticket = new Ticket { TicketId = 1, TicketPrice = 15m };
            context.Tickets.Add(ticket);

            var client = new Client
            {
                ClientId  = 1,
                DayOfWeek = "Понеделник",
                TicketId  = 1,
                FoodId    = 1,
                DrinkId   = 1,
            };
            context.Clients.Add(client);

            context.SaveChanges();
            return context;
        }
    }
}
