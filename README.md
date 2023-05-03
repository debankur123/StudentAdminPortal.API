# StudentAdminPortal.API
Student Administration Management Portal 
1. Created a Database
2. Created Tables and migrated Data
3. Added stored procedure for GetAllStudent records.
4. Added stored procedure and Repository methods for GetStudentById .

# A controller method for using Automapper to map the models .(Added for clarity purpose)

      [HttpPut("{id}")]
      public async Task<IActionResult> UpdateStudentDetails(int id, [FromBody] UpdateStudentRequest request)
      {
          try
          {
              using (SqlConnection con = new SqlConnection(connectionString))
              {
                  var student = _mapper.Map<Domain.Models.Student>(request);
                  student.Id = id;

                  var updatedStudent = await _studentService.UpdateStudentDetails(id, student, con);

                  if (updatedStudent == null)
                  {
                      return NotFound();
                  }

                  return Ok(updatedStudent);
              }
          }
          catch (Exception ex)
          {
              return StatusCode(500, ex.Message);
          }
      }

