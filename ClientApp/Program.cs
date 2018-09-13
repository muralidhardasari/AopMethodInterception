using AopMethodInterception.Core;
using System;
using Unity;

namespace ClientApp
{
    public  class Program
    {
       public static void Main(string[] args)
        {
            UnityContainer unityContainer = new UnityContainer();
            Container.RegisterControllers(unityContainer);
            Student student = new Student { Name = "Laxmi", Marks = 100, Email = "laxmi.dasari@gmail.com" };
            var studentController = unityContainer.Resolve<IStudentController>();
            studentController.AddStudent(student);
            Console.WriteLine(student.Id);
        }
    }
}
