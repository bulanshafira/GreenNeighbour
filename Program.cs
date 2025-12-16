using WasteManagementConsole.Data;
using WasteManagementConsole.Services;
using WasteManagementConsole.Models;

var database = Database.Instance; // Get the singleton database instance

// Initialize repositories
var userRepository = new UserRepository(database); 
var pickupRepository = new PickupRepository(database);

// Initialize services
var userService = new UserService(userRepository);
var pickupService = new PickupService(pickupRepository, userRepository);

User? currentUser = null; // Logged-in user

// Main application loop
while (true)
{
    Console.Clear();
    Console.WriteLine("=== Waste Management System ===");
    Console.WriteLine("1. Login");
    Console.WriteLine("2. Register");
    Console.WriteLine("3. Exit");
    Console.Write("Choose: ");
    var input = Console.ReadLine();

    if (input == "1")
    {
        // Login flow
        Console.Write("Email: ");
        string email = Console.ReadLine() ?? "";

        Console.Write("Password: ");
        string password = Console.ReadLine() ?? "";

        try
        {
            currentUser = userService.Login(email, password);
            Console.WriteLine($"Welcome back, {currentUser.Name}!");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
            Console.WriteLine("Press any key...");
            Console.ReadKey();
        }
    }
    else if (input == "2")
    {
        // Registration flow
        Console.Write("Name: ");
        string name = Console.ReadLine() ?? "";

        Console.Write("Email: ");
        string email = Console.ReadLine() ?? "";

        Console.Write("Password (min 4 characters): ");
        string password = Console.ReadLine() ?? "";

        Console.Write("Register as admin? (y/n): ");
        string adminChoice = Console.ReadLine()?.ToLower() ?? "";
        bool wantAdmin = adminChoice == "y" || adminChoice == "yes";

        string? adminPass = null;
        if (wantAdmin)
        {
            Console.Write("Admin password (hint: 12345): ");
            adminPass = Console.ReadLine();
        }

        try
        {
            currentUser = userService.Register(name, email, password, wantAdmin, adminPass);
            Console.WriteLine($"\n✓ Successfully registered as {currentUser.Role}!");
            if (currentUser.Role == "Admin")
            {
                Console.WriteLine("You have admin privileges.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
            if (wantAdmin && ex.Message.Contains("Email already registered") == false)
            {
                Console.WriteLine("Note: Admin password is required to register as admin.");
            }
            Console.WriteLine("Press any key...");
            Console.ReadKey();
        }
    }
    else if (input == "3")
    {
        break;
    }

    // Keep user logged in until they choose to logout
    while (currentUser != null)
    {
        Console.Clear();

        // Show different menus based on role
        if (currentUser.Role == "Admin")
        {
            // Admin Menu
            Console.WriteLine($"=== Admin Menu === (Logged in as: {currentUser.Name})");
            Console.WriteLine("1. View All Pickups");
            Console.WriteLine("2. View Pending Pickups");
            Console.WriteLine("3. Approve Pickup");
            Console.WriteLine("4. View Profile");
            Console.WriteLine("5. Logout");
            Console.Write("Choose: ");
            var menu = Console.ReadLine();

            try
            {
                if (menu == "1")
                {
                    // View all pickups
                    var allPickups = pickupService.GetAllPickups();
                    if (allPickups.Count == 0)
                    {
                        Console.WriteLine("No pickups in the system.");
                    }
                    else
                    {
                        Console.WriteLine("\n=== All Pickups ===");
                        foreach (var p in allPickups)
                        {
                            var user = pickupService.GetUserById(p.UserId);
                            string userName = user?.Name ?? "Unknown";
                            Console.WriteLine($"ID: {p.PickupId} | Citizen: {userName} | Address: {p.Address}");
                            Console.WriteLine($"  Type: {p.TrashType} | Weight: {p.Weight} kg | Status: {p.Status}");
                        }
                    }
                }
                else if (menu == "2")
                {
                    // View pending pickups
                    var pendingPickups = pickupService.GetPendingPickups();
                    if (pendingPickups.Count == 0)
                    {
                        Console.WriteLine("No pending pickups.");
                    }
                    else
                    {
                        Console.WriteLine("\n=== Pending Pickups ===");
                        foreach (var p in pendingPickups)
                        {
                            var user = pickupService.GetUserById(p.UserId);
                            string userName = user?.Name ?? "Unknown";
                            Console.WriteLine($"ID: {p.PickupId} | Citizen: {userName} | Address: {p.Address}");
                            Console.WriteLine($"  Type: {p.TrashType} | Weight: {p.Weight} kg");
                        }
                    }
                }
                else if (menu == "3")
                {
                    // Approve pickup
                    Console.Write("Enter Pickup ID to approve: ");
                    if (int.TryParse(Console.ReadLine(), out int pickupId))
                    {
                        pickupService.ApprovePickup(pickupId);
                        Console.WriteLine($"✓ Pickup #{pickupId} has been approved!");
                    }
                    else
                    {
                        Console.WriteLine("Error: Invalid Pickup ID.");
                    }
                }
                else if (menu == "4")
                {
                    // View profile
                    Console.WriteLine("\n=== User Profile ===");
                    Console.WriteLine($"Name: {currentUser.Name}");
                    Console.WriteLine($"Email: {currentUser.Email}");
                    Console.WriteLine($"User Role: {currentUser.Role}");
                    Console.WriteLine("\n" + currentUser.GetRoleInfo());
                }
                else if (menu == "5")
                {
                    Console.WriteLine("Logging out...");
                    currentUser = null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }
        else
        {
            // Citizen Menu
            Console.WriteLine($"=== Citizen Menu === (Logged in as: {currentUser.Name})");
            Console.WriteLine("1. Request Pickup");
            Console.WriteLine("2. View My Pickups");
            Console.WriteLine("3. View Profile");
            Console.WriteLine("4. Logout");
            Console.Write("Choose: ");
            var menu = Console.ReadLine();

            try
            {
                if (menu == "1")
                {
                    Console.Write("Pickup address: ");
                    string address = Console.ReadLine() ?? "";

                    Console.Write("Trash type (e.g., Plastic, Paper, Glass, Metal, Organic): ");
                    string trashType = Console.ReadLine() ?? "";

                    Console.Write("Weight (kg): ");
                    string weightInput = Console.ReadLine() ?? "";

                    if (!double.TryParse(weightInput, out double weight))
                    {
                        Console.WriteLine("Error: Invalid weight format.");
                    }
                    else
                    {
                        var pickup = pickupService.RequestPickup(currentUser, address, trashType, weight);
                        Console.WriteLine($"✓ Pickup requested successfully!");
                        Console.WriteLine($"Pickup ID: {pickup.PickupId}");
                        Console.WriteLine($"Type: {pickup.TrashType}, Weight: {pickup.Weight} kg");
                        Console.WriteLine($"Status: {pickup.Status}");
                    }
                }
                else if (menu == "2")
                {
                    var pickups = pickupService.GetUserPickups(currentUser.Id);
                    if (pickups.Count == 0)
                    {
                        Console.WriteLine("No pickups found.");
                    }
                    else
                    {
                        Console.WriteLine("\n=== My Pickups ===");
                        foreach (var p in pickups)
                        {
                            Console.WriteLine($"ID: {p.PickupId} | Address: {p.Address}");
                            Console.WriteLine($"  Type: {p.TrashType} | Weight: {p.Weight} kg | Status: {p.Status}");
                        }
                    }
                }
                else if (menu == "3")
                {
                    // Display detailed profile information
                    Console.WriteLine("\n=== User Profile ===");
                    Console.WriteLine($"Name: {currentUser.Name}");
                    Console.WriteLine($"Email: {currentUser.Email}");
                    Console.WriteLine($"User Role: {currentUser.Role}");

                    // Calculate current pending pickups
                    int pendingCount = pickupService.GetUserPickups(currentUser.Id)
                        .Count(p => p.Status == "Pending");
                    Console.WriteLine($"Current Pending Pickups: {pendingCount}/3");
                    Console.WriteLine($"Max Pickup Requests: 3 pending at a time");

                    Console.WriteLine("\n" + currentUser.GetRoleInfo());
                }
                else if (menu == "4")
                {
                    Console.WriteLine("Logging out...");
                    currentUser = null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }

        if (currentUser != null)
        {
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }
    }
}