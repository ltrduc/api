using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace docnet_final.Models
{
    public class Wrapper<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
    }
}