using CinemaApp.Data.Models;
using CinemaApp.Services.Implementations;
using NUnit.Framework;

namespace CinemaApp.Tests
{
    /// <summary>
    /// Компонентни тестове за <see cref="ProjectService"/>.
    /// </summary>
    [TestFixture]
    public class ProjectServiceTests : TestBase
    {
        [Test]
        public async Task GetAllAsync_ReturnsAllProjects()
        {
            using var context = CreateSeededContext();
            var service = new ProjectService(context);

            var result = (await service.GetAllAsync()).ToList();

            Assert.That(result, Has.Count.EqualTo(1));
            Assert.That(result[0].DayOfWeek, Is.EqualTo("Понеделник"));
        }

        [Test]
        public async Task GetAllAsync_EmptyDatabase_ReturnsEmptyList()
        {
            using var context = CreateContext();
            var service = new ProjectService(context);

            Assert.That(await service.GetAllAsync(), Is.Empty);
        }

        [Test]
        public async Task GetByIdAsync_ExistingId_ReturnsProject()
        {
            using var context = CreateSeededContext();
            var service = new ProjectService(context);

            var project = await service.GetByIdAsync(1);

            Assert.That(project, Is.Not.Null);
            Assert.That(project!.StartTime, Is.EqualTo("14:00"));
        }

        [Test]
        public async Task GetByIdAsync_NonExistingId_ReturnsNull()
        {
            using var context = CreateSeededContext();
            var service = new ProjectService(context);

            Assert.That(await service.GetByIdAsync(999), Is.Null);
        }

        [Test]
        public async Task AddAsync_ValidProject_IncreasesCount()
        {
            using var context = CreateSeededContext();
            var service = new ProjectService(context);
            var newProject = new Project
            {
                DayOfWeek = "Вторник",
                StartTime = "18:00",
                EndTime   = "20:00",
                RoomId    = 1,
                FilmId    = 1,
            };

            await service.AddAsync(newProject);

            Assert.That((await service.GetAllAsync()).Count(), Is.EqualTo(2));
        }

        [Test]
        public void AddAsync_NullProject_ThrowsArgumentNullException()
        {
            using var context = CreateSeededContext();
            var service = new ProjectService(context);

            Assert.ThrowsAsync<ArgumentNullException>(() => service.AddAsync(null!));
        }

        [Test]
        public async Task UpdateAsync_ValidProject_UpdatesDay()
        {
            using var context = CreateSeededContext();
            var service = new ProjectService(context);
            var project = await service.GetByIdAsync(1);
            project!.DayOfWeek = "Сряда";

            await service.UpdateAsync(project);
            var updated = await service.GetByIdAsync(1);

            Assert.That(updated!.DayOfWeek, Is.EqualTo("Сряда"));
        }

        [Test]
        public void UpdateAsync_NullProject_ThrowsArgumentNullException()
        {
            using var context = CreateSeededContext();
            var service = new ProjectService(context);

            Assert.ThrowsAsync<ArgumentNullException>(() => service.UpdateAsync(null!));
        }

        [Test]
        public async Task DeleteAsync_ExistingId_RemovesProject()
        {
            using var context = CreateContext();
            context.Projects.Add(new Project { ProjectId = 1, DayOfWeek = "Петък", StartTime = "10:00", EndTime = "12:00" });
            await context.SaveChangesAsync();

            var service = new ProjectService(context);
            await service.DeleteAsync(1);

            Assert.That(await service.GetAllAsync(), Is.Empty);
        }

        [Test]
        public void DeleteAsync_NonExistingId_ThrowsKeyNotFoundException()
        {
            using var context = CreateSeededContext();
            var service = new ProjectService(context);

            Assert.ThrowsAsync<KeyNotFoundException>(() => service.DeleteAsync(999));
        }

        [Test]
        public async Task GetByFilmNameAsync_ReturnsCorrectProjects()
        {
            using var context = CreateSeededContext();
            var service = new ProjectService(context);

            var result = (await service.GetByFilmNameAsync("Венъм")).ToList();

            Assert.That(result, Has.Count.EqualTo(1));
            Assert.That(result[0].Film!.FilmName, Is.EqualTo("Венъм"));
        }

        [Test]
        public void GetByFilmNameAsync_EmptyString_ThrowsArgumentException()
        {
            using var context = CreateSeededContext();
            var service = new ProjectService(context);

            Assert.ThrowsAsync<ArgumentException>(() => service.GetByFilmNameAsync(""));
        }

        [Test]
        public async Task GetByDayAsync_ReturnsProjectsForDay()
        {
            using var context = CreateSeededContext();
            var service = new ProjectService(context);

            var result = (await service.GetByDayAsync("Понеделник")).ToList();

            Assert.That(result, Has.Count.EqualTo(1));
            Assert.That(result[0].DayOfWeek, Is.EqualTo("Понеделник"));
        }

        [Test]
        public async Task GetProjectionsWithDetailsAsync_IncludesFilmAndRoom()
        {
            using var context = CreateSeededContext();
            var service = new ProjectService(context);

            var projects = (await service.GetProjectionsWithDetailsAsync()).ToList();

            Assert.That(projects, Is.Not.Empty);
            Assert.That(projects[0].Film, Is.Not.Null);
            Assert.That(projects[0].Room, Is.Not.Null);
        }
    }
}
