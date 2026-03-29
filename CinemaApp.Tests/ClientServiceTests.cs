using CinemaApp.Data.Models;
using CinemaApp.Services.Implementations;
using NUnit.Framework;

namespace CinemaApp.Tests
{
    /// <summary>
    /// Компонентни тестове за <see cref="ClientService"/>.
    /// </summary>
    [TestFixture]
    public class ClientServiceTests : TestBase
    {
        [Test]
        public async Task GetAllAsync_ReturnsAllClients()
        {
            using var context = CreateSeededContext();
            var service = new ClientService(context);

            var result = (await service.GetAllAsync()).ToList();

            Assert.That(result, Has.Count.EqualTo(1));
            Assert.That(result[0].DayOfWeek, Is.EqualTo("Понеделник"));
        }

        [Test]
        public async Task GetAllAsync_EmptyDatabase_ReturnsEmptyList()
        {
            using var context = CreateContext();
            var service = new ClientService(context);

            Assert.That(await service.GetAllAsync(), Is.Empty);
        }

        [Test]
        public async Task GetByIdAsync_ExistingId_ReturnsClient()
        {
            using var context = CreateSeededContext();
            var service = new ClientService(context);

            var client = await service.GetByIdAsync(1);

            Assert.That(client, Is.Not.Null);
            Assert.That(client!.TicketId, Is.EqualTo(1));
        }

        [Test]
        public async Task GetByIdAsync_NonExistingId_ReturnsNull()
        {
            using var context = CreateSeededContext();
            var service = new ClientService(context);

            Assert.That(await service.GetByIdAsync(999), Is.Null);
        }

        [Test]
        public async Task AddAsync_ValidClient_IncreasesCount()
        {
            using var context = CreateSeededContext();
            var service = new ClientService(context);
            var newClient = new Client { DayOfWeek = "Вторник", TicketId = 1, FoodId = 1, DrinkId = 1 };

            await service.AddAsync(newClient);

            Assert.That((await service.GetAllAsync()).Count(), Is.EqualTo(2));
        }

        [Test]
        public void AddAsync_NullClient_ThrowsArgumentNullException()
        {
            using var context = CreateSeededContext();
            var service = new ClientService(context);

            Assert.ThrowsAsync<ArgumentNullException>(() => service.AddAsync(null!));
        }

        [Test]
        public async Task UpdateAsync_ValidClient_UpdatesDay()
        {
            using var context = CreateSeededContext();
            var service = new ClientService(context);
            var client = await service.GetByIdAsync(1);
            client!.DayOfWeek = "Сряда";

            await service.UpdateAsync(client);
            var updated = await service.GetByIdAsync(1);

            Assert.That(updated!.DayOfWeek, Is.EqualTo("Сряда"));
        }

        [Test]
        public void UpdateAsync_NullClient_ThrowsArgumentNullException()
        {
            using var context = CreateSeededContext();
            var service = new ClientService(context);

            Assert.ThrowsAsync<ArgumentNullException>(() => service.UpdateAsync(null!));
        }

        [Test]
        public async Task DeleteAsync_ExistingId_RemovesClient()
        {
            using var context = CreateSeededContext();
            var service = new ClientService(context);

            await service.DeleteAsync(1);

            Assert.That(await service.GetAllAsync(), Is.Empty);
        }

        [Test]
        public void DeleteAsync_NonExistingId_ThrowsKeyNotFoundException()
        {
            using var context = CreateSeededContext();
            var service = new ClientService(context);

            Assert.ThrowsAsync<KeyNotFoundException>(() => service.DeleteAsync(999));
        }

        [Test]
        public async Task GetByDayAsync_ReturnsClientsForDay()
        {
            using var context = CreateSeededContext();
            var service = new ClientService(context);

            var result = (await service.GetByDayAsync("Понеделник")).ToList();

            Assert.That(result, Has.Count.EqualTo(1));
            Assert.That(result[0].DayOfWeek, Is.EqualTo("Понеделник"));
        }

        [Test]
        public void GetByDayAsync_EmptyString_ThrowsArgumentException()
        {
            using var context = CreateSeededContext();
            var service = new ClientService(context);

            Assert.ThrowsAsync<ArgumentException>(() => service.GetByDayAsync(""));
        }

        [Test]
        public async Task GetSoldTicketsCountByDayAsync_ReturnsCorrectCount()
        {
            using var context = CreateSeededContext();
            var service = new ClientService(context);

            int count = await service.GetSoldTicketsCountByDayAsync("Понеделник");

            Assert.That(count, Is.EqualTo(1));
        }

        [Test]
        public async Task GetRevenueByDayAsync_ReturnsCorrectRevenue()
        {
            using var context = CreateSeededContext();
            var service = new ClientService(context);

            // билет 15 + храна тип 10 + напитка тип 10 = 35
            decimal revenue = await service.GetRevenueByDayAsync("Понеделник");

            Assert.That(revenue, Is.EqualTo(35m));
        }

        [Test]
        public async Task GetWeeklyRevenueAsync_IsGreaterThanOrEqualToDailyRevenue()
        {
            using var context = CreateSeededContext();
            var service = new ClientService(context);

            decimal weekly = await service.GetWeeklyRevenueAsync();
            decimal daily  = await service.GetRevenueByDayAsync("Понеделник");

            Assert.That(weekly, Is.GreaterThanOrEqualTo(daily));
        }

        [Test]
        public async Task GetClientsWithSameExpensesAsync_GroupsCorrectly()
        {
            using var context = CreateSeededContext();
            // Добавяме втори клиент с еднакви разходи
            context.Clients.Add(new Client { ClientId = 2, DayOfWeek = "Вторник", TicketId = 1, FoodId = 1, DrinkId = 1 });
            await context.SaveChangesAsync();

            var service = new ClientService(context);
            var groups  = (await service.GetClientsWithSameExpensesAsync()).ToList();

            Assert.That(groups, Is.Not.Empty);
            Assert.That(groups[0].Count, Is.GreaterThan(1));
        }
    }
}
