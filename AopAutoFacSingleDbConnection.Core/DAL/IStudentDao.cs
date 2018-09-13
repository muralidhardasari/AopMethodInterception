using System.Collections.Generic;

namespace AopMethodInterception.Core
{
    public  interface IStudentDao
    {
        void AddStudent(Student student);
        IList<Student> GetStudents();
    }

   
}
