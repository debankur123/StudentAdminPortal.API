using Microsoft.AspNetCore.Mvc;
using StudentsAdminPortal.API.RepoImplementation;
using StudentsAdminPortal.API.Repositories;

namespace StudentsAdminPortal.API.Controllers
{
    [ApiController]
    public class StudentController : Controller
    {
        private readonly IStudentRepository _studentRepository;
        public StudentController(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }
        [HttpGet]
        [Route("students")]
        public async Task<IActionResult> getStudentsList()
        {
            var students = await _studentRepository.GetAllStudentsAsync();
            var domainModelsStudents = new List<Domain.Models.Student>();
            foreach (var studentModel in students)
            {
                var domainModelStudent = new Domain.Models.Student()
                {
                    Id = studentModel.Id,
                    firstName = studentModel.firstName,
                    lastName = studentModel.lastName,
                    DOB = studentModel.DOB,
                    Email = studentModel.Email,
                    Mobile = studentModel.Mobile,
                    profileImageURL = studentModel.profileImageURL,
                    genderID = studentModel.genderID
                };
                if (studentModel.address != null)
                {
                    domainModelStudent.address = new Domain.Models.Address()
                    {
                        ID = studentModel.address.ID,
                        physicalAddress = studentModel.address.physicalAddress,
                        postalAddress = studentModel.address.postalAddress,
                        studentID = studentModel.address.studentID,
                    };
                }
                if (studentModel.gender != null)
                {
                    domainModelStudent.gender = new Domain.Models.Gender()
                    {
                        ID = studentModel.gender.ID,
                        Description = studentModel.gender.Description
                    };
                }
                domainModelsStudents.Add(domainModelStudent);
            }
            return Ok(domainModelsStudents);
        }
        [HttpGet]
        [Route("students/{id:int}")]
        public async Task<IActionResult> GetStudentAsync(int id)
        {
            var student = await _studentRepository.GetStudentAsync(id);
            if (student == null)
            {
                return NotFound();
            }
            return new JsonResult(student);
        }


    }
}
