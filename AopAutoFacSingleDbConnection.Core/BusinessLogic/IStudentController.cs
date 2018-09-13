namespace AopMethodInterception.Core
{
    public interface IStudentController
    {
        void AddStudent(Student student);
    }
    public class StudentController : IStudentController
    {
        private readonly IStudentDao _studentDao;
        public StudentController(IStudentDao studentDao)
        {
            _studentDao = studentDao;
        }

        [SqlConnectionAspect]
        public void AddStudent(Student student)
        {
            _studentDao.AddStudent(student);
        }
    }
}
