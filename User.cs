namespace WasteManagementConsole.Models;

// Base User model class
public abstract class User
{
    public int Id { get; }
    public string Name { get; }
    public string Email { get; }
    public string Password { get; }
    public string Role { get; }

    // Constructor to initialize User properties
    protected User(int id, string name, string email, string password, string role)
    {
        Id = id;
        Name = name;
        Email = email;
        Password = password;
        Role = role;
    }

    // Abstract method to get role-specific information
    public abstract string GetRoleInfo();
}

// Citizen subclass
public class Citizen : User
{
    public Citizen(int id, string name, string email, string password)
        : base(id, name, email, password, "Citizen") { }

    // Implementation of abstract method
    public override string GetRoleInfo()
    {
        return "Citizen can request pickups (max 3 pending).";
    }
}

// Admin subclass
public class Admin : User
{
    public Admin(int id, string name, string email, string password)
        : base(id, name, email, password, "Admin") { }

    public override string GetRoleInfo()
    {
        return "Admin can view all pickup requests.";
    }
}