using System.Collections.Generic;

namespace Sinostar.API
{
    public class ErrorMsg
    {
        public string Message { get; set; }
        public List<string> Causes { get; set; }
        public string Error { get; set; }
        public string Status { get; set; }
    }
}