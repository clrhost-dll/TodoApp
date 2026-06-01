using FluentAssertions;
using Moq;
using TodoApp.BLL.DTOs.Categories;
using TodoApp.BLL.Exceptions;
using TodoApp.BLL.Services;
using TodoApp.DAL.Entities;
using TodoApp.DAL.Repositories.Interfaces;

namespace TodoApp.Tests.Tests
{
    public class CategoryServiceTests
    {
        private readonly Mock<ICategoryRepository> _categoryRepositoryMock;
        private readonly CategoryService _categoryService;

        public CategoryServiceTests()
        {
            _categoryRepositoryMock = new Mock<ICategoryRepository>();
            _categoryService = new CategoryService(
                _categoryRepositoryMock.Object);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnCategories()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var categories = new List<Category>
            {
                new() { Id = Guid.NewGuid(), Name = "Work", UserId = userId },
                new() { Id = Guid.NewGuid(), Name = "Personal", UserId = userId }
            };

            _categoryRepositoryMock
                .Setup(x => x.GetAllAsync(userId))
                .ReturnsAsync(categories);

            // Act
            var result = await _categoryService.GetAllAsync(userId);

            // Assert
            result.Should().HaveCount(2);
            result.Should().Contain(x => x.Name == "Work");
            result.Should().Contain(x => x.Name == "Personal");
        }

        [Fact]
        public async Task CreateAsync_ShouldCallRepository_WhenDtoIsValid()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var dto = new CreateCategoryDto { Name = "Work" };

            _categoryRepositoryMock
                .Setup(x => x.CreateAsync(It.IsAny<Category>()))
                .Returns(Task.CompletedTask);

            _categoryRepositoryMock
                .Setup(x => x.SaveChangesAsync())
                .Returns(Task.CompletedTask);

            // Act
            await _categoryService.CreateAsync(userId, dto);

            // Assert
            _categoryRepositoryMock.Verify(
                x => x.CreateAsync(It.Is<Category>(c =>
                    c.Name == dto.Name &&
                    c.UserId == userId)),
                Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_ShouldThrowNotFoundException_WhenCategoryNotFound()
        {
            // Arrange
            _categoryRepositoryMock
                .Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync((Category?)null);

            // Act
            var act = async () => await _categoryService.DeleteAsync(Guid.NewGuid());

            // Assert
            await act.Should().ThrowAsync<NotFoundException>()
                .WithMessage("Category not found");
        }

        [Fact]
        public async Task DeleteAsync_ShouldCallDelete_WhenCategoryExists()
        {
            // Arrange
            var category = new Category
            {
                Id = Guid.NewGuid(),
                Name = "Work",
                UserId = Guid.NewGuid()
            };

            _categoryRepositoryMock
                .Setup(x => x.GetByIdAsync(category.Id))
                .ReturnsAsync(category);

            _categoryRepositoryMock
                .Setup(x => x.SaveChangesAsync())
                .Returns(Task.CompletedTask);

            // Act
            await _categoryService.DeleteAsync(category.Id);

            // Assert
            _categoryRepositoryMock.Verify(
                x => x.Delete(category), 
                Times.Once);
        }
    }
}