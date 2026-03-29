using CinemaApp.Data.Models;
using CinemaApp.Services.Implementations;
using NUnit.Framework;

namespace CinemaApp.Tests
{
    /// <summary>
    /// Компонентни тестове за <see cref="ActorService"/>.
    /// </summary>
    [TestFixture]
    public class ActorServiceTests : TestBase
    {
        [Test]
        public async Task GetAllAsync_ReturnsAllActors()
        {
            using var context = CreateSeededContext();
            var service = new ActorService(context);

            var result = (await service.GetAllAsync()).ToList();

            Assert.That(result, Has.Count.EqualTo(1));
            Assert.That(result[0].LastName, Is.EqualTo("Харди"));
        }

        [Test]
        public async Task GetAllAsync_EmptyDatabase_ReturnsEmptyList()
        {
            using var context = CreateContext();
            var service = new ActorService(context);

            var result = await service.GetAllAsync();

            Assert.That(result, Is.Empty);
        }

        [Test]
        public async Task GetByIdAsync_ExistingId_ReturnsActor()
        {
            using var context = CreateSeededContext();
            var service = new ActorService(context);

            var actor = await service.GetByIdAsync(1);

            Assert.That(actor, Is.Not.Null);
            Assert.That(actor!.FirstName, Is.EqualTo("Том"));
        }

        [Test]
        public async Task GetByIdAsync_NonExistingId_ReturnsNull()
        {
            using var context = CreateSeededContext();
            var service = new ActorService(context);

            var actor = await service.GetByIdAsync(999);

            Assert.That(actor, Is.Null);
        }

        [Test]
        public async Task AddAsync_ValidActor_IncreasesCount()
        {
            using var context = CreateSeededContext();
            var service = new ActorService(context);
            var newActor = new Actor { FirstName = "Анджелина", LastName = "Джоли", Age = 50 };

            await service.AddAsync(newActor);
            var all = (await service.GetAllAsync()).ToList();

            Assert.That(all, Has.Count.EqualTo(2));
        }

        [Test]
        public void AddAsync_NullActor_ThrowsArgumentNullException()
        {
            using var context = CreateSeededContext();
            var service = new ActorService(context);

            Assert.ThrowsAsync<ArgumentNullException>(() => service.AddAsync(null!));
        }

        [Test]
        public async Task UpdateAsync_ValidActor_UpdatesName()
        {
            using var context = CreateSeededContext();
            var service = new ActorService(context);
            var actor = await service.GetByIdAsync(1);
            actor!.FirstName = "Том Бекери";

            await service.UpdateAsync(actor);
            var updated = await service.GetByIdAsync(1);

            Assert.That(updated!.FirstName, Is.EqualTo("Том Бекери"));
        }

        [Test]
        public void UpdateAsync_NullActor_ThrowsArgumentNullException()
        {
            using var context = CreateSeededContext();
            var service = new ActorService(context);

            Assert.ThrowsAsync<ArgumentNullException>(() => service.UpdateAsync(null!));
        }

        [Test]
        public async Task DeleteAsync_ExistingId_RemovesActor()
        {
            using var context = CreateContext();
            context.Actors.Add(new Actor { ActorId = 1, FirstName = "Test", LastName = "Actor", Age = 30 });
            await context.SaveChangesAsync();

            var service = new ActorService(context);
            await service.DeleteAsync(1);

            Assert.That(await service.GetAllAsync(), Is.Empty);
        }

        [Test]
        public void DeleteAsync_NonExistingId_ThrowsKeyNotFoundException()
        {
            using var context = CreateSeededContext();
            var service = new ActorService(context);

            Assert.ThrowsAsync<KeyNotFoundException>(() => service.DeleteAsync(999));
        }

        [Test]
        public async Task GetByMinAgeAsync_ReturnsOnlyActorsAboveMin()
        {
            using var context = CreateSeededContext();
            context.Actors.Add(new Actor { ActorId = 2, FirstName = "Млад", LastName = "Актьор", Age = 20 });
            await context.SaveChangesAsync();

            var service = new ActorService(context);
            var result  = (await service.GetByMinAgeAsync(35)).ToList();

            Assert.That(result, Has.Count.EqualTo(1));
            Assert.That(result[0].Age, Is.GreaterThanOrEqualTo(35));
        }

        [Test]
        public async Task GetActorsWithFilmsAsync_ReturnsActorsWithFilms()
        {
            using var context = CreateSeededContext();
            var service = new ActorService(context);

            var actors = (await service.GetActorsWithFilmsAsync()).ToList();

            Assert.That(actors, Is.Not.Empty);
            Assert.That(actors[0].Films, Is.Not.Null);
        }
    }
}
