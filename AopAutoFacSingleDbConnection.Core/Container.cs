using Unity;

namespace AopMethodInterception.Core
{
    public class Container
    {
        public static void RegisterControllers(UnityContainer unityContainer)
        {
           unityContainer.RegisterType<IStudentController, StudentController>();
            unityContainer.RegisterType<IStudentDao, SqlStudent>();
        }
    }
}
