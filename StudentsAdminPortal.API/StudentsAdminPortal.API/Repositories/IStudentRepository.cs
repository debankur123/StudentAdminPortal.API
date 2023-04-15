﻿using StudentsAdminPortal.API.Models;

namespace StudentsAdminPortal.API.Repositories
{
    public interface IStudentRepository
    {
        Task<List<Student>> GetAllStudentsAsync();
    }
}