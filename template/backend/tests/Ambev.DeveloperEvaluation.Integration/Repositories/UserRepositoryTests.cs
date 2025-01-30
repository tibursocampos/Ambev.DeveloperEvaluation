using Ambev.DeveloperEvaluation.Integration.TestData;
using Ambev.DeveloperEvaluation.ORM;
using Ambev.DeveloperEvaluation.ORM.Repositories;

using Microsoft.EntityFrameworkCore;

using Xunit;

namespace Ambev.DeveloperEvaluation.Integration.Repositories;

/// <summary>
/// Integration tests for the <see cref="UserRepository"/> class.
/// </summary>
public class UserRepositoryTests
{
    /// <summary>
    /// The options for the in-memory database context.
    /// </summary>
    private readonly DbContextOptions<DefaultContext> _dbContextOptions;

    /// <summary>
    /// Initializes a new instance of the <see cref="UserRepositoryTests"/> class.
    /// </summary>
    public UserRepositoryTests()
    {
        _dbContextOptions = new DbContextOptionsBuilder<DefaultContext>()
            .UseInMemoryDatabase(databaseName: "UserTestDatabase")
            .Options;
    }

    [Fact(DisplayName = "Should add complete user to database when valid user is provided")]
    public async Task GivenValidUser_WhenCreateAsyncCalled_ThenPersistsAllUserProperties()
    {
        // Arrange
        using var context = new DefaultContext(_dbContextOptions);
        var _sut = new UserRepository(context);
        var newUser = UserTestData.GenerateValidUser();

        // Act
        var createdUser = await _sut.CreateAsync(newUser);

        // Assert
        using var assertContext = new DefaultContext(_dbContextOptions);
        var persistedUser = await assertContext.Users.FindAsync(newUser.Id);

        Assert.NotNull(persistedUser);
        Assert.Equal(newUser.Email, persistedUser.Email);
        Assert.Equal(newUser.Username, persistedUser.Username);
        Assert.Equal(newUser.Phone, persistedUser.Phone);
        Assert.Equal(newUser.Role, persistedUser.Role);
        Assert.Equal(newUser.Status, persistedUser.Status);
    }

    [Fact(DisplayName = "Should return null when searching for non-existent user ID")]
    public async Task GivenInvalidUserId_WhenGetByIdAsyncCalled_ThenReturnsNull()
    {
        // Arrange
        using var context = new DefaultContext(_dbContextOptions);
        var _sut = new UserRepository(context);
        var nonExistentId = Guid.NewGuid();

        // Act
        var result = await _sut.GetByIdAsync(nonExistentId);

        // Assert
        Assert.Null(result);
    }

    [Fact(DisplayName = "Should retrieve user by email ignoring case sensitivity")]
    public async Task GivenExistingEmailWithDifferentCase_WhenGetByEmailAsyncCalled_ThenReturnsUser()
    {
        // Arrange
        using var context = new DefaultContext(_dbContextOptions);
        var _sut = new UserRepository(context);
        var originalEmail = "TestUser@Example.com";
        var existingUser = UserTestData.GenerateValidUser(email: originalEmail);
        await context.Users.AddAsync(existingUser);
        await context.SaveChangesAsync();

        // Act
        var retrievedUser = await _sut.GetByEmailAsync(originalEmail.ToUpper());

        // Assert
        Assert.NotNull(retrievedUser);
        Assert.Equal(originalEmail, retrievedUser?.Email);
    }

    [Fact(DisplayName = "Should permanently delete user when existing ID is provided")]
    public async Task GivenExistingUserId_WhenDeleteAsyncCalled_ThenRemovesUserPermanently()
    {
        // Arrange
        using var context = new DefaultContext(_dbContextOptions);
        var _sut = new UserRepository(context);
        var userToDelete = UserTestData.GenerateValidUser();
        await context.Users.AddAsync(userToDelete);
        await context.SaveChangesAsync();

        // Act
        var deleteResult = await _sut.DeleteAsync(userToDelete.Id);

        // Assert
        Assert.True(deleteResult);
        var deletedUser = await context.Users.IgnoreQueryFilters().FirstOrDefaultAsync(u => u.Id == userToDelete.Id);
        Assert.Null(deletedUser);
    }

    [Fact(DisplayName = "Should maintain data integrity when creating user with all fields populated")]
    public async Task GivenCompleteUserData_WhenCreated_ThenPersistsAllFieldsCorrectly()
    {
        // Arrange
        using var context = new DefaultContext(_dbContextOptions);
        var _sut = new UserRepository(context);
        var complexUser = UserTestData.GenerateValidUser();

        // Act
        var createdUser = await _sut.CreateAsync(complexUser);

        // Assert
        using var assertContext = new DefaultContext(_dbContextOptions);
        var persistedUser = await assertContext.Users.AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == complexUser.Id);

        Assert.NotNull(persistedUser);
        Assert.Equal(complexUser.Password, persistedUser.Password);
        Assert.Equal(complexUser.Phone, persistedUser.Phone);
        Assert.Equal(complexUser.CreatedAt, persistedUser.CreatedAt);
        Assert.Null(persistedUser.UpdatedAt);
    }
}
