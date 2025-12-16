using WasteManagementConsole.Models;

namespace WasteManagementConsole.Data;

public class UserRepository : IRepository<User>
{
    // Reference to the singleton database instance
    private readonly Database _db;

    public UserRepository(Database db) 
    {
        _db = db;
    }

    // Implementing IRepository methods
    public void Add(User entity)
    {
        _db.Users.Add(entity);
    }

    // Retrieve a user by their ID
    public User? GetById(int id)
    {
        return _db.Users.FirstOrDefault(u => u.Id == id);
    }

    // Retrieve all users
    public List<User> GetAll()
    {
        return _db.Users.ToList();
    }

    // Updating a user. For simplicity, this method is left empty. In a real database, this would update the entity
    public void Update(User entity)
    {

    }

    // Deleting a user by their ID
    public void Delete(int id)
    {
        var user = GetById(id);
        if (user != null)
        {
            _db.Users.Remove(user);
        }
    }

    // Additional User-specific methods
    public User? GetByEmail(string email)
    {
        return _db.Users.FirstOrDefault(u => u.Email == email);
    }

    // Check if an email already exists in the database
    public bool EmailExists(string email)
    {
        return _db.Users.Any(u => u.Email == email);
    }

    // Get the next User ID
    public int GetNextUserId()
    {
        return _db.GetNextUserId();
    }
}