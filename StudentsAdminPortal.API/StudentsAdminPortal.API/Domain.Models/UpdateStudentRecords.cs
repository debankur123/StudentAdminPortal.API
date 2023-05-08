namespace StudentsAdminPortal.API.Domain.Models
{
    public class UpdateStudentRecords
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DOB { get; set; }
        public string Email { get; set; }
        public long Mobile { get; set; }
        public int GenderID { get; set; }
        public Address Address { get; set; }
    }
}
