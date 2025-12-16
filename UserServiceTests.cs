using WasteManagementConsole.Data;
using WasteManagementConsole.Services;
using Xunit;

namespace WasteManagementConsole.Tests;

// Unit tests for UserService
public class UserServiceTests
{
    // Helper method to create a UserService with a fresh database instance
    private UserService CreateUserService()
    {
        Database.ResetInstance(); 
        var db = Database.Instance;
        var userRepository = new UserRepository(db);
        return new UserService(userRepository);
    }

    [Fact]
    public void Valid_Email_Should_Pass() // Test for valid email format
    {
        var service = CreateUserService();

        bool result = service.IsValidEmail("student@email.com");

        Assert.True(result);
    }

    [Fact]
    public void Invalid_Email_Should_Fail() // Test for invalid email format
    {
        var service = CreateUserService();

        bool result = service.IsValidEmail("studentemail.com");

        Assert.False(result);
    }

    [Fact]
    public void Register_With_Invalid_Email_Should_Throw_Error() // Test registration with invalid email
    {
        var service = CreateUserService();

        Assert.Throws<Exception>(() =>
            service.Register("Test", "bad-email", "password123", false, null));
    }

    [Fact]
    public void Register_And_Login_Should_Work() // Test successful registration and login
    {
        var service = CreateUserService();

        // Register a user
        var user = service.Register("John Doe", "john@email.com", "pass1234", false, null);
        Assert.NotNull(user);
        Assert.Equal("john@email.com", user.Email);

        // Login with correct credentials
        var loggedInUser = service.Login("john@email.com", "pass1234");
        Assert.NotNull(loggedInUser);
        Assert.Equal("John Doe", loggedInUser.Name);
    }

    [Fact]
    public void Login_With_Wrong_Password_Should_Throw_Error() // Test login with incorrect password
    {
        var service = CreateUserService();

        // Register a user
        service.Register("Jane Doe", "jane@email.com", "pass1234", false, null);

        // Try to login with wrong password
        Assert.Throws<Exception>(() =>
            service.Login("jane@email.com", "wrongpass"));
    }

    [Fact]
    public void Login_With_Nonexistent_Email_Should_Throw_Error() // Test login with unregistered email
    {
        var service = CreateUserService();

        Assert.Throws<Exception>(() =>
            service.Login("notfound@email.com", "anypass"));
    }

    [Fact]
    public void Register_With_Duplicate_Email_Should_Throw_Error() // Test registration with duplicate email
    {
        var service = CreateUserService();

        // Register first user
        service.Register("User One", "duplicate@email.com", "pass1234", false, null);

        // Try to register with same email
        Assert.Throws<Exception>(() =>
            service.Register("User Two", "duplicate@email.com", "pass5678", false, null));
    }
}