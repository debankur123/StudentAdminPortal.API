using Microsoft.AspNetCore.Mvc;
using StudentsAdminPortal.API.Domain.Models;
using StudentsAdminPortal.API.Repositories;

namespace StudentsAdminPortal.API.Controllers
{
    [ApiController]
    public class StudentController : Controller
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IImageRepository _imageRepository;
        public StudentController(IStudentRepository studentRepository, IImageRepository imageRepository)
        {
            _studentRepository = studentRepository;
            _imageRepository = imageRepository;
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
        [HttpPut]
        [Route("students/{id:int}")]
        public async Task<IActionResult> UpdateStudentDetails(int id, [FromBody] UpdateStudentRecords records)
        {
            if (id != records.Id)
            {
                return BadRequest();
            }
            var updatedStudent = await _studentRepository.UpdateStudentDetails(id, records);

            if (updatedStudent == null)
            {
                return NotFound();
            }

            return Ok(updatedStudent);
        }
        [HttpDelete]
        [Route("students/{studentId:int}")]
        public ActionResult DeleteSingleStudentRecord(int studentId)
        {
            var studentPresent =  _studentRepository.GetStudentAsync(studentId);
            if (studentPresent == null)
            {
                return NotFound();
            }
            else
            {
                 _studentRepository.DeleteSingleStudent(studentId);
                return Ok(studentId);
            }
        }
        [HttpPost]
        [Route("students/Add")]
        public IActionResult InsertStudent([FromBody] AddStudentRecords details)
        {
            try
            {
                _studentRepository.AddStudentDetails(details);
                return Ok(new { message = "Student record inserted successfully!" });
            }
            catch (Exception ex)
            {
                return BadRequest("Error occurred: " + ex.Message);
            }
        }

        [HttpPost]
        [Route("students/{studentId:int}/upload-image")]
        public async Task<IActionResult> UploadImage(int studentId,IFormFile profileImage)
        {
            // Check student exists or not?
            var studentPresent = await _studentRepository.GetStudentAsync(studentId);
            if(studentPresent == null)
                return NotFound();

            //store the image in local storage
            var fileName = Guid.NewGuid() + Path.GetExtension(profileImage.FileName);
            var fileImagePath = await _imageRepository.UploadImage(profileImage, fileName);

            //update profile image path in database
            if (await _studentRepository.UpdateProfileImage(studentId, fileImagePath))
                return Ok(fileImagePath);
            else
                return StatusCode(StatusCodes.Status500InternalServerError, "Error uploading Image");

        }
    }
}

        

