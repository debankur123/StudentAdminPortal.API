namespace StudentsAdminPortal.API.Domain.Models
{
    public class AddStudentRecords
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DOB { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public int GenderID { get; set; }
        public string ProfileImageURL { get; set; }
        public Address Address { get; set; }
        




    }
}
