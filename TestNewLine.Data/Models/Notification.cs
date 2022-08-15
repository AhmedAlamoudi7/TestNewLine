using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestNewLine.Data.Models
{
    public class Notification :BaseEntity
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public string Href { get; set; }
        public bool isCheked { get; set; }
        public string UserFrom { get; set; }
        public string UserTo { get; set; }
    }
}
