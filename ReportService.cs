using WasteManagementConsole.Data;
using WasteManagementConsole.Models;

namespace WasteManagementConsole.Services;

// Service class for managing reports
public class ReportService
{
    private readonly Database _db; // Reference to the singleton database instance

    public ReportService(Database db) // Constructor to initialize the ReportService with the database
    {
        _db = db;
    }

}