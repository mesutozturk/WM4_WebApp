using System;
using System.Diagnostics;

namespace ItServiceApp.InjectOrnek
{
    public class MyDependency : IMyDependency
    {
        public void Log(string message)
        {
            Debug.WriteLine(message);
        }
    }
}
