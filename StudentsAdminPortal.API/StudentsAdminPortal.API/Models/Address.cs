namespace StudentsAdminPortal.API.Models;

public class Address
{
    public int ID { get; set; }
    public string? physicalAddress { get; set; }
    public string? postalAddress { get; set; }
    public int studentID { get; set; }
}