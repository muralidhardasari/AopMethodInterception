using Cauldron.Interception;
using System;
using System.Reflection;

namespace AopMethodInterception.Core
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
    public class SqlConnectionAspectAttribute : Attribute, IMethodInterceptor
    {
        SqlConnectionContext sqlContext = null;
        public void OnEnter(Type declaringType, object instance, MethodBase methodbase, object[] values)
        {
                sqlContext = new SqlConnectionContext();
         }

        public void OnException(Exception e)
        {
            throw new NotImplementedException();
        }

        public void OnExit()
        {
            sqlContext.Commit();
            sqlContext.Dispose();
        }
    }
}
