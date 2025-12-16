using WasteManagementConsole.Models;

namespace WasteManagementConsole.Data;

// Singleton Database class
public sealed class Database
{
    private static Database? _instance;

    // Object is locked for thread safety
    private static readonly object _lock = new object();

    // Simple auto-increment simulation
    private int _userId = 1;
    private int _pickupId = 1;

    // In-memory storage
    public List<User> Users { get; } = new();
    public List<Pickup> Pickups { get; } = new();

    // Using a private constructor to prevent instantiation from outside. Left empty 
    private Database()
    {

    }

    // Public property to get the singleton instance
    public static Database Instance
    {
        get
        {
            // For thread safety, double-check locking is used
            if (_instance == null) 
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new Database();
                    }
                }
            }
            return _instance;
        }
    }

    // Get the next User ID
    public int GetNextUserId()
    {
        return _userId++;
    }

    // Get the next Pickup ID
    public int GetNextPickupId()
    {
        return _pickupId++;
    }

    // Method to reset the singleton instance (for testing purposes)
    public static void ResetInstance()
    {
        lock (_lock)
        {
            _instance = null;
        }
    }
}