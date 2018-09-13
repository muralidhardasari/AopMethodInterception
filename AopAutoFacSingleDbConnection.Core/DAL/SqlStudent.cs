using System;
using System.Collections.Generic;

namespace AopMethodInterception.Core
{
    public class SqlStudent : IStudentDao
    {
        public void AddStudent(Student student)
        {
            using (var cmd = DbCommand.GetCommand())
            {
                cmd.CommandText =
                   @"insert into dbo.Student(Name,Marks,Email) values(@Name,@Marks,@Email)
                    select  CAST(scope_identity() AS int)";

                cmd.Parameters.AddWithValue("@Name", student.Name);
                cmd.Parameters.AddWithValue("@Marks", student.Marks);
                cmd.Parameters.AddWithValue("@Email", student.Email);
                student.Id = Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

        public IList<Student> GetStudents()
        {
            throw new NotImplementedException();
        }
    }
}
