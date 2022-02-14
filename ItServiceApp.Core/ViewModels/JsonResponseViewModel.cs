using System;

namespace ItServiceApp.Core.ViewModels
{
    public class JsonResponseViewModel
    {
        public bool IsSuccess { get; set; } = true;
        public object Data { get; set; }
        public string ErrorMessage { get; set; }
        public DateTime ResponseTime { get; set; } = DateTime.UtcNow;

    }
}
