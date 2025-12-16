namespace WasteManagementConsole.Models;

// Report model class
public class Report
{
    public int ReportId { get; }
    public int UserId { get; }
    public string Title { get; }
    public string Description { get; }
    public string Location { get; }

    // Constructor to initialize Report properties
    public Report(int reportId, int userId, string title, string description, string location)
    {
        ReportId = reportId;
        UserId = userId;
        Title = title;
        Description = description;
        Location = location;
    }
}