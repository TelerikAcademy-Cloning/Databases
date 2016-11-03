namespace StudentSystem.Data
{
    using StudentSystem.Data.Repositories;
    using StudentSystem.Models;
    using System;

    public interface IStudentSystemData : IDisposable
    {
        void Commit();
    }
}
