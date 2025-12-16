using WasteManagementConsole.Data;
using WasteManagementConsole.Models;
using WasteManagementConsole.Factories;
using System.Net.Mail;

namespace WasteManagementConsole.Services;

// Service class for managing users
public class UserService
{
    private readonly UserRepository _userRepository; // Reference to the UserRepository

    public UserService(UserRepository userRepository) 
    {
        _userRepository = userRepository;
    }

    // Method to register a new user
    public User Register(string name, string email, string password, bool wantAdmin, string? adminPassword)
    {
        // Email validation using built-in .NET class
        if (!IsValidEmail(email))
            throw new Exception("Email format is not valid.");

        // Repository instead of direct database access
        if (_userRepository.EmailExists(email))
            throw new Exception("Email already registered.");

        // Validate password
        if (string.IsNullOrWhiteSpace(password) || password.Length < 4)
            throw new Exception("Password must be at least 4 characters.");

        // Use Factory Pattern to create User objects
        int userId = _userRepository.GetNextUserId();
        User user = UserFactory.CreateUser(userId, name, email, password, wantAdmin, adminPassword);

        // Use Repository to add user
        _userRepository.Add(user);
        return user;
    }

    // Method for user login
    public User Login(string email, string password)
    {
        // Use Repository to find user
        var user = _userRepository.GetByEmail(email);

        // Check if user exists and password matches
        if (user == null)
            throw new Exception("Email not found.");

        if (user.Password != password)
            throw new Exception("Incorrect password.");

        return user;
    }

    // Helper method to validate email format
    public bool IsValidEmail(string email)
    {
        try
        {
            var addr = new MailAddress(email);
            return true;
        }
        catch
        {
            return false;
        }
    }
}