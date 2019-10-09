using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MedNote.API.Helpers.Globalsys
{
    public class SuccessResult
    {
        public string Message { get; set; }
        public bool Success { get; set; }
        public Object Data { get; set; }
    }
}