using WasteManagementConsole.Models;

namespace WasteManagementConsole.Factories;

public class UserFactory // Factory class for creating User instances
{
    private const string AdminPassword = "12345"; // Predefined admin password

    // Factory method to create User instances
    public static User CreateUser(int id, string name, string email, string password, bool wantAdmin, string? adminPassword)
    {
        if (wantAdmin && adminPassword == AdminPassword)
        {
            return new Admin(id, name, email, password);
        }
        else
        {
            return new Citizen(id, name, email, password);
        }
    }

    // Specific factory methods for each user type
    public static User CreateCitizen(int id, string name, string email, string password)
    {
        return new Citizen(id, name, email, password);
    }

    // Specific factory method for Admin
    public static User CreateAdmin(int id, string name, string email, string password)
    {
        return new Admin(id, name, email, password);
    }

    // Method to validate admin password
    public static bool ValidateAdminPassword(string? password)
    {
        return password == AdminPassword;
    }
}