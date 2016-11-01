namespace StudentSystem.Data
{
    using StudentSystem.Data.Repositories;
    using StudentSystem.Models;
    using System;
    using System.Collections.Generic;

    public class StudentsSystemData : IStudentSystemData, IDisposable
    {
        private IStudentSystemDbContext context;

        public StudentsSystemData(IStudentSystemDbContext context)
        {
            if(context == null)
            {
                throw new ArgumentException();
            }

            this.context = context;
        }

        
        public void Commit()
        {
            this.context.SaveChanges();
        }

        public void Dispose()
        {
        }
    }
}
