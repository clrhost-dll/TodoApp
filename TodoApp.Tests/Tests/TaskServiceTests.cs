using FluentAssertions;
using Moq;
using TodoApp.BLL.DTOs.Tasks;
using TodoApp.BLL.Exceptions;
using TodoApp.BLL.Services;
using TodoApp.DAL.Entities;
using TodoApp.DAL.Repositories.Interfaces;

namespace TodoApp.Tests.Tests
{
    public class TaskServiceTests
    {
        private readonly Mock<ITaskRepository> _taskRepositoryMock;
        private readonly TaskService _taskService;

        public TaskServiceTests()
        {
            _taskRepositoryMock = new Mock<ITaskRepository>();
            _taskService = new TaskService(_taskRepositoryMock.Object);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnTask_WhenTaskExists()
        {
            // Arrange
            var taskId = Guid.NewGuid();
            var task = new TaskItem
            {
                Id = taskId,
                Title = "Test Task",
                Description = "Test Description",
                IsCompleted = false,
                CreatedAt = DateTime.UtcNow,
                CategoryId = Guid.NewGuid(),
                Category = new Category { Name = "Work" }
            };

            _taskRepositoryMock
                .Setup(x => x.GetByIdAsync(taskId))
                .ReturnsAsync(task);

            // Act
            var result = await _taskService.GetByIdAsync(taskId);

            // Assert
            result.Should().NotBeNull();
            result!.Title.Should().Be(task.Title);
            result.CategoryName.Should().Be("Work");
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnNull_WhenTaskNotFound()
        {
            // Arrange
            _taskRepositoryMock
                .Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync((TaskItem?)null);

            // Act
            var result = await _taskService.GetByIdAsync(Guid.NewGuid());

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task CreateAsync_ShouldCallRepository_WhenDtoIsValid()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var dto = new CreateTaskDto
            {
                Title = "New Task",
                Description = "Description",
                CategoryId = Guid.NewGuid(),
                DueDate = DateTime.UtcNow.AddDays(7)
            };

            _taskRepositoryMock
                .Setup(x => x.CreateAsync(It.IsAny<TaskItem>()))
                .Returns(Task.CompletedTask);

            _taskRepositoryMock
                .Setup(x => x.SaveChangesAsync())
                .Returns(Task.CompletedTask);

            // Act
            await _taskService.CreateAsync(userId, dto);

            // Assert
            _taskRepositoryMock.Verify(
                x => x.CreateAsync(It.Is<TaskItem>(t =>
                    t.Title == dto.Title &&
                    t.UserId == userId)),
                Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_ShouldThrowNotFoundException_WhenTaskNotFound()
        {
            // Arrange
            _taskRepositoryMock
                .Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync((TaskItem?)null);

            // Act
            var act = async () => await _taskService.DeleteAsync(Guid.NewGuid());

            // Assert
            await act.Should().ThrowAsync<NotFoundException>()
                .WithMessage("Task not found");
        }

        [Fact]
        public async Task UpdateAsync_ShouldThrowNotFoundException_WhenTaskNotFound()
        {
            // Arrange
            _taskRepositoryMock
                .Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync((TaskItem?)null);

            var dto = new UpdateTaskDto
            {
                Title = "Updated",
                IsCompleted = true,
                CategoryId = Guid.NewGuid()
            };

            // Act
            var act = async () => await _taskService.UpdateAsync(Guid.NewGuid(), dto);

            // Assert
            await act.Should().ThrowAsync<NotFoundException>()
                .WithMessage("Task not found");
        }
    }
}