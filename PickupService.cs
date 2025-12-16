using WasteManagementConsole.Data;
using WasteManagementConsole.Models;

namespace WasteManagementConsole.Services;

// Service class to handle Pickup-related operations
public class PickupService
{
    private readonly PickupRepository _pickupRepository;
    private readonly UserRepository _userRepository;

    // Constructor to initialize repositories
    public PickupService(PickupRepository pickupRepository, UserRepository userRepository)
    {
        _pickupRepository = pickupRepository;
        _userRepository = userRepository;
    }

    // Method to request a new pickup
    public Pickup RequestPickup(User user, string address, string trashType, double weight)
    {
        int pendingCount = _pickupRepository.GetPendingCountByUserId(user.Id);

        if (pendingCount >= 3)
            throw new Exception("You already have 3 pending pickup requests.");

        // Validate trash type
        if (string.IsNullOrWhiteSpace(trashType))
            throw new Exception("Trash type is required.");

        // Validate weight
        if (weight <= 0)
            throw new Exception("Weight must be greater than 0.");

        // Use Repository to get next ID
        var pickup = new Pickup(
            _pickupRepository.GetNextPickupId(),
            user.Id,
            address,
            trashType,
            weight
        );

        _pickupRepository.Add(pickup);
        return pickup;
    }

    // Retrieve pickups for a specific user
    public List<Pickup> GetUserPickups(int userId)
    {
        return _pickupRepository.GetByUserId(userId);
    }

    // Retrieve all pickups
    public List<Pickup> GetAllPickups()
    {
        return _pickupRepository.GetAll();
    }

    // Retrieve all pending pickups
    public List<Pickup> GetPendingPickups()
    {
        return _pickupRepository.GetByStatus("Pending");
    }

    // Approve a pending pickup
    public void ApprovePickup(int pickupId)
    {
        var pickup = _pickupRepository.GetById(pickupId);

        if (pickup == null)
            throw new Exception("Pickup not found.");

        if (pickup.Status != "Pending")
            throw new Exception($"Pickup is already {pickup.Status}.");

        pickup.Status = "Approved";
    }

    // Reject a pending pickup
    public User? GetUserById(int userId)
    {
        return _userRepository.GetById(userId);
    }
}