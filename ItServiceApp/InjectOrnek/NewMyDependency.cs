using System;
using System.Diagnostics;

namespace ItServiceApp.InjectOrnek
{
    public class NewMyDependency : IMyDependency
    {
        public void Log(string message)
        {
            Debug.WriteLine($"{DateTime.Now:T} - {message}");
        }
    }
}