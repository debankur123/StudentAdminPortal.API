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
                        gender.ID = Convert.ToInt32(rda["ID"]);
                        gender.Description = rda["Description"].ToString();
                        st.gender = gender;

                        Address address = new Address();
                        if (rda["address"] != DBNull.Value)
                        {
                            address = new Address();
                            address.ID = Convert.ToInt32(rda["ID"]);
                            address.postalAddress = rda["postalAddress"] == DBNull.Value ? null : rda["postalAddress"].ToString();
                            address.physicalAddress = rda["physicalAddress"] == DBNull.Value ? null : rda["physicalAddress"].ToString();
                            address.studentID = Convert.ToInt32(rda["studentID"]);
                        }
                        st.address = address;
                        students.Add(st);
                    }
                    rda.CloseAsync();
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
            return students;
        }

    }
}
