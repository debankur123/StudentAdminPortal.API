using StudentsAdminPortal.API.Domain.Models;

namespace StudentsAdminPortal.API.Domain.Models
{
    public class Student
    {
        public int Id { get; set; }
        public string? firstName { get; set; }
        public string? lastName { get; set; }
        public DateTime? DOB { get; set; }
        public string? Email { get; set; }
        public string? Mobile { get; set; }
        public string? profileImageURL { get; set; }
        public int genderID { get; set; }
        public Gender? gender { get; set; }
        public Address? address { get; set; }

    }
}
