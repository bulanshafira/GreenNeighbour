using WasteManagementConsole.Models;

namespace WasteManagementConsole.Data;

public class PickupRepository : IRepository<Pickup> 
{
    private readonly Database _db; // Reference to the singleton database instance

    public PickupRepository(Database db)
    {
        _db = db;
    }

    // Implementing IRepository methods
    public void Add(Pickup entity) 
    {
        _db.Pickups.Add(entity);
    }

    // Retrieve a pickup by its ID
    public Pickup? GetById(int id)
    {
        return _db.Pickups.FirstOrDefault(p => p.PickupId == id);
    }

    // Retrieve all pickups
    public List<Pickup> GetAll()
    {
        return _db.Pickups.ToList();
    }

    // Updating a pickup. For simplicity, this method is left empty
    public void Update(Pickup entity)
    {

    }

    // Deleting a pickup by its ID
    public void Delete(int id)
    {
        var pickup = GetById(id);
        if (pickup != null)
        {
            _db.Pickups.Remove(pickup);
        }
    }

    // Additional methods specific to PickupRepository
    public List<Pickup> GetByUserId(int userId)
    {
        return _db.Pickups.Where(p => p.UserId == userId).ToList();
    }

    // Retrieve pickups by their status
    public List<Pickup> GetByStatus(string status)
    {
        return _db.Pickups.Where(p => p.Status == status).ToList();
    }

    // Count of pending pickups for a specific user
    public int GetPendingCountByUserId(int userId)
    {
        return _db.Pickups.Count(p => p.UserId == userId && p.Status == "Pending");
    }

    // Get the next Pickup ID
    public int GetNextPickupId()
    {
        return _db.GetNextPickupId();
    }
}