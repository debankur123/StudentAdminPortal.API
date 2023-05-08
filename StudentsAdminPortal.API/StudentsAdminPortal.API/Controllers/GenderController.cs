using Microsoft.AspNetCore.Mvc;
using StudentsAdminPortal.API.Domain.Models;
using StudentsAdminPortal.API.RepoImplementation;
using StudentsAdminPortal.API.Repositories;

namespace StudentsAdminPortal.API.Controllers
{
    [ApiController]
    public class GenderController : Controller
    {
        private readonly IStudentRepository _studentRepository;
        public GenderController(IStudentRepository studentRepository)
        {
            this._studentRepository = studentRepository;
        }
        [HttpGet]
        [Route("[controller]")]
        public async Task<IActionResult> GetGenderAsync()
        {
            var genderList = await _studentRepository.GetGenderAsync();
            if(genderList == null || !genderList.Any())
            {
                return NotFound();
            }
            return Ok(genderList);
        }
    }
}
