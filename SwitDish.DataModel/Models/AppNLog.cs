using System;
using System.Collections.Generic;

namespace SwitDish.DataModel.Models
{
    public class AppNLog
    {
        public int Id { get; set; }
        public DateTime Timestamp { get; set; }
        public string Level { get; set; }
        public string Logger { get; set; }
        public string Message { get; set; }
        public int? Userid { get; set; }
        public string Exception { get; set; }
        public string Stacktrace { get; set; }
    }
}
