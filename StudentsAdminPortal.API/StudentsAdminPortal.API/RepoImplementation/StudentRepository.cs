using Microsoft.CodeAnalysis.Elfie.Diagnostics;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using StudentsAdminPortal.API.Models;
using StudentsAdminPortal.API.Repositories;
using System;

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
        public async Task<List<Student>> GetAllStudentsAsync()
        {
            List<Student> students = new List<Student>();
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
                        Student st = new Student();
                        st.Id = Convert.ToInt32(rda["Id"]);
                        st.firstName = rda["firstName"].ToString();
                        st.lastName = rda["lastName"].ToString();
                        st.DOB = DateTime.Parse(rda["DOB"].ToString()).Date;
                        st.Email = rda["Email"].ToString();
                        st.Mobile = Convert.ToInt64(rda["Mobile"]);
                        st.profileImageURL = rda["profileImageURL"] == DBNull.Value ? null : rda["profileImageURL"].ToString();
                        st.genderID = Convert.ToInt32(rda["genderID"]);

                        Gender gender = new Gender();
                        gender.ID = Convert.ToInt32(rda["studentID"]);
                        gender.Description = rda["Description"].ToString();
                        st.gender = gender;

                        Address address = new Address();
                        if (rda["address"] != DBNull.Value)
                        {
                            address = new Address();
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
        public async Task<Student> GetStudentAsync(int id)
        {
            Student student = new Student();
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
                        student.DOB = DateTime.Parse(reader["DOB"].ToString()).Date;
                        student.Mobile = Convert.ToInt64(reader["Mobile"]);
                        student.Email = reader["Email"].ToString();
                        student.profileImageURL = reader["profileImageURL"] == DBNull.Value ? null : reader["profileImageURL"].ToString();
                        student.genderID = Convert.ToInt32(reader["genderID"]);

                        Gender gender = new Gender();
                        gender.ID = Convert.ToInt32(reader["StudentID"]);
                        gender.Description = reader["Description"].ToString();
                        student.gender = gender;

                        Address address = new Address();
                        if (reader["address"] != DBNull.Value)
                        {
                            address = new Address();
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

    }
}

