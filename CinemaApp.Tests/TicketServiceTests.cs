using CinemaApp.Data.Models;
using CinemaApp.Services.Implementations;
using NUnit.Framework;

namespace CinemaApp.Tests
{
    /// <summary>
    /// Компонентни тестове за <see cref="TicketService"/>.
    /// </summary>
    [TestFixture]
    public class TicketServiceTests : TestBase
    {
        [Test]
        public async Task GetAllAsync_ReturnsAllTickets()
        {
            using var context = CreateSeededContext();
            var service = new TicketService(context);

            var result = (await service.GetAllAsync()).ToList();

            Assert.That(result, Has.Count.EqualTo(1));
            Assert.That(result[0].TicketPrice, Is.EqualTo(15m));
        }

        [Test]
        public async Task GetAllAsync_EmptyDatabase_ReturnsEmptyList()
        {
            using var context = CreateContext();
            var service = new TicketService(context);

            Assert.That(await service.GetAllAsync(), Is.Empty);
        }

        [Test]
        public async Task GetByIdAsync_ExistingId_ReturnsTicket()
        {
            using var context = CreateSeededContext();
            var service = new TicketService(context);

            var ticket = await service.GetByIdAsync(1);

            Assert.That(ticket, Is.Not.Null);
            Assert.That(ticket!.TicketPrice, Is.EqualTo(15m));
        }

        [Test]
        public async Task GetByIdAsync_NonExistingId_ReturnsNull()
        {
            using var context = CreateSeededContext();
            var service = new TicketService(context);

            Assert.That(await service.GetByIdAsync(999), Is.Null);
        }

        [Test]
        public async Task AddAsync_ValidTicket_IncreasesCount()
        {
            using var context = CreateSeededContext();
            var service = new TicketService(context);
            var newTicket = new Ticket { TicketPrice = 20m };

            await service.AddAsync(newTicket);

            Assert.That((await service.GetAllAsync()).Count(), Is.EqualTo(2));
        }

        [Test]
        public void AddAsync_NullTicket_ThrowsArgumentNullException()
        {
            using var context = CreateSeededContext();
            var service = new TicketService(context);

            Assert.ThrowsAsync<ArgumentNullException>(() => service.AddAsync(null!));
        }

        [Test]
        public async Task UpdateAsync_ValidTicket_UpdatesPrice()
        {
            using var context = CreateSeededContext();
            var service = new TicketService(context);
            var ticket = await service.GetByIdAsync(1);
            ticket!.TicketPrice = 20m;

            await service.UpdateAsync(ticket);
            var updated = await service.GetByIdAsync(1);

            Assert.That(updated!.TicketPrice, Is.EqualTo(20m));
        }

        [Test]
        public void UpdateAsync_NullTicket_ThrowsArgumentNullException()
        {
            using var context = CreateSeededContext();
            var service = new TicketService(context);

            Assert.ThrowsAsync<ArgumentNullException>(() => service.UpdateAsync(null!));
        }

        [Test]
        public async Task DeleteAsync_ExistingId_RemovesTicket()
        {
            using var context = CreateContext();
            context.Tickets.Add(new Ticket { TicketId = 1, TicketPrice = 15m });
            await context.SaveChangesAsync();

            var service = new TicketService(context);
            await service.DeleteAsync(1);

            Assert.That(await service.GetAllAsync(), Is.Empty);
        }

        [Test]
        public void DeleteAsync_NonExistingId_ThrowsKeyNotFoundException()
        {
            using var context = CreateSeededContext();
            var service = new TicketService(context);

            Assert.ThrowsAsync<KeyNotFoundException>(() => service.DeleteAsync(999));
        }

        [Test]
        public async Task GetTotalTicketsSumAsync_ReturnsSumOfPurchasedTickets()
        {
            using var context = CreateSeededContext();
            var service = new TicketService(context);

            decimal total = await service.GetTotalTicketsSumAsync();

            Assert.That(total, Is.EqualTo(15m));
        }

        [Test]
        public async Task GetTotalTicketsSumAsync_NoClients_ReturnsZero()
        {
            using var context = CreateContext();
            var service = new TicketService(context);

            decimal total = await service.GetTotalTicketsSumAsync();

            Assert.That(total, Is.EqualTo(0));
        }

        [Test]
        public async Task GetFreePlacesAsync_ReturnsPlacesWithoutClient()
        {
            using var context = CreateContext();
            // Seat + Row + Room + Ticket без клиент
            context.Seats.Add(new Seat { SeatId = 1 });
            context.Rows.Add(new Row { RowsId = 1, RowsType = "normal" });
            context.Rooms.Add(new Room { RoomId = 1, RoomType = "2D" });
            context.Tickets.Add(new Ticket { TicketId = 1, TicketPrice = 15m });
            context.Places.Add(new Place { SeatId = 1, RowsId = 1, RoomId = 1, TicketId = 1, PlaceType = "normal/normal" });
            await context.SaveChangesAsync();

            var service = new TicketService(context);
            var free    = (await service.GetFreePlacesAsync()).ToList();

            Assert.That(free, Has.Count.EqualTo(1));
        }
    }
}
