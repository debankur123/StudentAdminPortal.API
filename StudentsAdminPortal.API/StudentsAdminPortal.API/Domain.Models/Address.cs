namespace StudentsAdminPortal.API.Domain.Models
{
    public class Address
    {
        public int ID { get; set; }
        public string? physicalAddress { get; set; }
        public string? postalAddress { get; set; }
        public int studentID { get; set; }
    }
}
