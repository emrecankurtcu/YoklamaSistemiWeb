using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YoklamaSistemi1.Models
{
    public class User
    {
        public int id { get; set; }
        public string ino { get; set; }
        public string iname { get; set; }
        public string isurname { get; set; }
        public string ipassword { get; set; }
        public string imail { get; set; }
        public string imacadr { get; set; }
    }
}