namespace WasteManagementConsole.Models;

// Pickup model class
public class Pickup
{
    public int PickupId { get; set; }
    public int UserId { get; }
    public string Address { get; }
    public string TrashType { get; }
    public double Weight { get; }
    public string Status { get; set; }

    // Constructor to initialize Pickup properties
    public Pickup(int pickupId, int userId, string address, string trashType, double weight)
    {
        PickupId = pickupId;
        UserId = userId;
        Address = address;
        TrashType = trashType;
        Weight = weight;
        Status = "Pending";
    }
}