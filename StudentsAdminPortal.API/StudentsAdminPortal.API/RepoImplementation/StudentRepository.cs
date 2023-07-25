using Microsoft.Data.SqlClient;
using StudentsAdminPortal.API.Domain.Models;
using StudentsAdminPortal.API.Repositories;
using System.Data;
using System.Transactions;

namespace StudentsAdminPortal.API.RepoImplementation
{
    public class StudentRepository : IStudentRepository
    {
        public IConfiguration Configuration { get; set; }
        private readonly string connectionString;
        public StudentRepository(IConfiguration _configuration)
        {
            Configuration = _configuration;
            connectionString = Configuration.GetSection("ConnectionStrings").Get<ConnectionStrings>().StudentAdminPortal;
        }
        public async Task<List<Domain.Models.Student>> GetAllStudentsAsync()
        {
            List<Domain.Models.Student> students = new List<Domain.Models.Student>();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("USP_GetAllStudents", con);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    await con.OpenAsync();
                    SqlDataReader rda = await cmd.ExecuteReaderAsync();
                    while (await rda.ReadAsync())
                    {
                        Domain.Models.Student st = new Domain.Models.Student();
                        st.Id = Convert.ToInt32(rda["Id"]);
                        st.firstName = rda["firstName"].ToString();
                        st.lastName = rda["lastName"].ToString();
                        st.DOB = DateTime.Parse(rda["DOB"].ToString());
                        st.Email = rda["Email"].ToString();
                        st.Mobile = rda["Mobile"].ToString();
                        st.profileImageURL = rda["profileImageURL"] == DBNull.Value ? null : rda["profileImageURL"].ToString();
                        st.genderID = Convert.ToInt32(rda["genderID"]);

                        Domain.Models.Gender gender = new Domain.Models.Gender();
                        gender.ID = Convert.ToInt32(rda["studentID"]);
                        gender.Description = rda["Description"].ToString();
                        st.gender = gender;

                        Domain.Models.Address address = new Domain.Models.Address();
                        if (rda["address"] != DBNull.Value)
                        {
                            address = new Domain.Models.Address();
                            address.ID = Convert.ToInt32(rda["studentID"]);
                            address.postalAddress = rda["postalAddress"] == DBNull.Value ? null : rda["postalAddress"].ToString();
                            address.physicalAddress = rda["physicalAddress"] == DBNull.Value ? null : rda["physicalAddress"].ToString();
                            address.studentID = Convert.ToInt32(rda["studentID"]);
                        }
                        st.address = address;
                        students.Add(st);
                    }
                    await rda.CloseAsync();
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
            return students;
        }
        public async Task<Domain.Models.Student> GetStudentAsync(int id)
        {
            Domain.Models.Student student = new Domain.Models.Student();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("USP_GetStudentByID", con);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    await con.OpenAsync();
                    cmd.Parameters.AddWithValue("@ID", id);
                    SqlDataReader reader = await cmd.ExecuteReaderAsync();
                    while (await reader.ReadAsync())
                    {
                        student.Id = id;
                        student.firstName = reader["firstName"].ToString();
                        student.lastName = reader["lastName"].ToString();
                        student.DOB = DateTime.Parse(reader["DOB"].ToString());
                        student.Mobile = reader["Mobile"].ToString();
                        student.Email = reader["Email"].ToString();
                        student.profileImageURL = reader["profileImageURL"] == DBNull.Value ? null : reader["profileImageURL"].ToString();
                        student.genderID = Convert.ToInt32(reader["genderID"]);

                        Domain.Models.Gender gender = new Domain.Models.Gender();
                        gender.ID = Convert.ToInt32(reader["StudentID"]);
                        gender.Description = reader["Description"].ToString();
                        student.gender = gender;

                        Domain.Models.Address address = new Domain.Models.Address();
                        if (reader["address"] != DBNull.Value)
                        {
                            address = new Domain.Models.Address();
                            address.ID = Convert.ToInt32(reader["StudentID"]);
                            address.postalAddress = reader["postalAddress"] == DBNull.Value ? null : reader["postalAddress"].ToString();
                            address.physicalAddress = reader["physicalAddress"] == DBNull.Value ? null : reader["physicalAddress"].ToString();
                            address.studentID = Convert.ToInt32(reader["studentID"]);
                        }
                        student.address = address;
                    }
                    await reader.CloseAsync();
                }
                catch (Exception)
                {
                    throw;
                }
                return student;
            }
        }

        public async Task<List<Domain.Models.Gender>> GetGenderAsync()
        {
            List<Domain.Models.Gender> gender = new List<Domain.Models.Gender>();
            string msg = "";
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("USP_GetAllGenders", con);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    await con.OpenAsync();
                    SqlDataReader reader = await cmd.ExecuteReaderAsync();
                    while (await reader.ReadAsync())
                    {
                        Domain.Models.Gender gdn = new Domain.Models.Gender();
                        gdn.ID = Convert.ToInt32(reader["ID"]);
                        gdn.Description = reader["Description"].ToString();
                        gender.Add(gdn);
                    }
                    await reader.CloseAsync();
                }
                catch (Exception ex)
                {
                    msg = ex.Message;
                }
                return gender;
            }
        }

        public async Task<UpdateStudentRecords> UpdateStudentDetails(int studentId, UpdateStudentRecords _records)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand("USP_UpdateStudentAndAddressDetails", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    await connection.OpenAsync();
                    command.Parameters.AddWithValue("@ID", studentId);
                    command.Parameters.AddWithValue("@firstName", _records.FirstName);
                    command.Parameters.AddWithValue("@lastName", _records.LastName);
                    command.Parameters.AddWithValue("@DOB", _records.DOB);
                    command.Parameters.AddWithValue("@Email", _records.Email);
                    command.Parameters.AddWithValue("@Mobile", _records.Mobile);
                    command.Parameters.AddWithValue("@GenderID", _records.GenderID);
                    command.Parameters.AddWithValue("@PostalAddress", _records.Address.postalAddress ?? string.Empty);
                    command.Parameters.AddWithValue("@PhysicalAddress", _records.Address.physicalAddress ?? string.Empty);
                    await command.ExecuteNonQueryAsync();
                    return _records;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void DeleteSingleStudent(int studentId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("USP_DELETE_SINGLE_STUDENT", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add("@Id", SqlDbType.Int).Value = studentId;
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }


        public void AddStudentDetails(AddStudentRecords student)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                try
                {
                    using (SqlCommand studentCommand = new SqlCommand("USP_InsertStudentRecord", connection))
                    {
                        studentCommand.CommandType = CommandType.StoredProcedure;
                        studentCommand.Parameters.AddWithValue("@FirstName", student.FirstName);
                        studentCommand.Parameters.AddWithValue("@LastName", student.LastName);
                        studentCommand.Parameters.AddWithValue("@DOB", student.DOB);
                        studentCommand.Parameters.AddWithValue("@Email", student.Email);
                        studentCommand.Parameters.AddWithValue("@PhoneNo", student.Mobile);
                        studentCommand.Parameters.AddWithValue("@GenderID", student.GenderID);
                        studentCommand.Parameters.AddWithValue("@PhysicalAddress", student.Address.physicalAddress);
                        studentCommand.Parameters.AddWithValue("@PostalAddress", student.Address.postalAddress);
                        studentCommand.Parameters.AddWithValue("@ProfileImageURL", student.ProfileImageURL);
                        studentCommand.ExecuteNonQuery();
                    }
                }
                catch(Exception ex)
                {
                    throw ex;
                }
            }

        }

        public async Task<bool> UpdateProfileImage(int studentId,string ProfileImageURL)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    await connection.OpenAsync();
                    using (SqlCommand command = new SqlCommand("USP_UpdateProfileImageURL", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@ProfileImageURL",ProfileImageURL);
                        command.Parameters.AddWithValue("@studentId",studentId);
                        int rowsAffected = await command.ExecuteNonQueryAsync();

                        return rowsAffected > 0;
                    }
                }
                catch (Exception ex)
                {
                   return false;
                }
            }
        }
    }
}



