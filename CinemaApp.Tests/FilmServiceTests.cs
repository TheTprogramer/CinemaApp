using CinemaApp.Data.Models;
using CinemaApp.Services.Implementations;
using NUnit.Framework;

namespace CinemaApp.Tests
{
    /// <summary>
    /// Компонентни тестове за <see cref="FilmService"/>.
    /// </summary>
    [TestFixture]
    public class FilmServiceTests : TestBase
    {
        [Test]
        public async Task GetAllAsync_ReturnsAllFilms()
        {
            using var context = CreateSeededContext();
            var service = new FilmService(context);

            var result = (await service.GetAllAsync()).ToList();

            Assert.That(result, Has.Count.EqualTo(1));
            Assert.That(result[0].FilmName, Is.EqualTo("Венъм"));
        }

        [Test]
        public async Task GetAllAsync_EmptyDatabase_ReturnsEmptyList()
        {
            using var context = CreateContext();
            var service = new FilmService(context);

            Assert.That(await service.GetAllAsync(), Is.Empty);
        }

        [Test]
        public async Task GetByIdAsync_ExistingId_ReturnsFilm()
        {
            using var context = CreateSeededContext();
            var service = new FilmService(context);

            var film = await service.GetByIdAsync(1);

            Assert.That(film, Is.Not.Null);
            Assert.That(film!.FilmGenre, Is.EqualTo("Екшън"));
        }

        [Test]
        public async Task GetByIdAsync_NonExistingId_ReturnsNull()
        {
            using var context = CreateSeededContext();
            var service = new FilmService(context);

            Assert.That(await service.GetByIdAsync(999), Is.Null);
        }

        [Test]
        public async Task AddAsync_ValidFilm_IncreasesCount()
        {
            using var context = CreateSeededContext();
            var service = new FilmService(context);
            var newFilm = new Film { FilmName = "Аватар", FilmGenre = "Екшън", FilmTime = 162, ActorId = 1 };

            await service.AddAsync(newFilm);

            Assert.That((await service.GetAllAsync()).Count(), Is.EqualTo(2));
        }

        [Test]
        public void AddAsync_NullFilm_ThrowsArgumentNullException()
        {
            using var context = CreateSeededContext();
            var service = new FilmService(context);

            Assert.ThrowsAsync<ArgumentNullException>(() => service.AddAsync(null!));
        }

        [Test]
        public async Task UpdateAsync_ValidFilm_UpdatesName()
        {
            using var context = CreateSeededContext();
            var service = new FilmService(context);
            var film = await service.GetByIdAsync(1);
            film!.FilmName = "Венъм 2";

            await service.UpdateAsync(film);
            var updated = await service.GetByIdAsync(1);

            Assert.That(updated!.FilmName, Is.EqualTo("Венъм 2"));
        }

        [Test]
        public void UpdateAsync_NullFilm_ThrowsArgumentNullException()
        {
            using var context = CreateSeededContext();
            var service = new FilmService(context);

            Assert.ThrowsAsync<ArgumentNullException>(() => service.UpdateAsync(null!));
        }

        [Test]
        public async Task DeleteAsync_ExistingId_RemovesFilm()
        {
            using var context = CreateContext();
            context.Actors.Add(new Actor { ActorId = 1, FirstName = "T", LastName = "T", Age = 30 });
            context.Films.Add(new Film { FilmId = 1, FilmName = "Test", FilmGenre = "Test", FilmTime = 100, ActorId = 1 });
            await context.SaveChangesAsync();

            var service = new FilmService(context);
            await service.DeleteAsync(1);

            Assert.That(await service.GetAllAsync(), Is.Empty);
        }

        [Test]
        public void DeleteAsync_NonExistingId_ThrowsKeyNotFoundException()
        {
            using var context = CreateSeededContext();
            var service = new FilmService(context);

            Assert.ThrowsAsync<KeyNotFoundException>(() => service.DeleteAsync(999));
        }

        [Test]
        public async Task GetByGenreAsync_ReturnsOnlyMatchingGenre()
        {
            using var context = CreateSeededContext();
            context.Films.Add(new Film { FilmId = 2, FilmName = "Комедия", FilmGenre = "Комедия", FilmTime = 90, ActorId = 1 });
            await context.SaveChangesAsync();

            var service = new FilmService(context);
            var result  = (await service.GetByGenreAsync("Екшън")).ToList();

            Assert.That(result, Has.Count.EqualTo(1));
            Assert.That(result[0].FilmGenre, Is.EqualTo("Екшън"));
        }

        [Test]
        public void GetByGenreAsync_EmptyString_ThrowsArgumentException()
        {
            using var context = CreateSeededContext();
            var service = new FilmService(context);

            Assert.ThrowsAsync<ArgumentException>(() => service.GetByGenreAsync(""));
        }

        [Test]
        public async Task GetByMinDurationAsync_ReturnsFilmsAboveMin()
        {
            using var context = CreateSeededContext();
            context.Films.Add(new Film { FilmId = 2, FilmName = "Кратък", FilmGenre = "Комедия", FilmTime = 80, ActorId = 1 });
            await context.SaveChangesAsync();

            var service = new FilmService(context);
            var result  = (await service.GetByMinDurationAsync(100)).ToList();

            Assert.That(result, Has.Count.EqualTo(1));
            Assert.That(result[0].FilmTime, Is.GreaterThanOrEqualTo(100));
        }

        [Test]
        public async Task GetShortestAsync_ReturnsCorrectCount()
        {
            using var context = CreateSeededContext();
            context.Films.Add(new Film { FilmId = 2, FilmName = "Втори", FilmGenre = "Комедия", FilmTime = 80, ActorId = 1 });
            context.Films.Add(new Film { FilmId = 3, FilmName = "Трети", FilmGenre = "Трилър",  FilmTime = 95, ActorId = 1 });
            await context.SaveChangesAsync();

            var service = new FilmService(context);
            var result  = (await service.GetShortestAsync(2)).ToList();

            Assert.That(result, Has.Count.EqualTo(2));
            Assert.That(result[0].FilmTime, Is.LessThanOrEqualTo(result[1].FilmTime));
        }

        [Test]
        public async Task GetLongestAsync_ReturnsFilmWithMaxDuration()
        {
            using var context = CreateSeededContext();
            context.Films.Add(new Film { FilmId = 2, FilmName = "Аватар", FilmGenre = "Екшън", FilmTime = 162, ActorId = 1 });
            await context.SaveChangesAsync();

            var service = new FilmService(context);
            var longest = await service.GetLongestAsync();

            Assert.That(longest, Is.Not.Null);
            Assert.That(longest!.FilmTime, Is.EqualTo(162));
        }

        [Test]
        public async Task GetFilmsWithDetailsAsync_IncludesActorAndProjects()
        {
            using var context = CreateSeededContext();
            var service = new FilmService(context);

            var films = (await service.GetFilmsWithDetailsAsync()).ToList();

            Assert.That(films, Is.Not.Empty);
            Assert.That(films[0].Actor, Is.Not.Null);
            Assert.That(films[0].Projects, Is.Not.Null);
        }
    }
}
