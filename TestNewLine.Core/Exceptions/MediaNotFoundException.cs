using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestNewLine.Exceptions
{
    public class MediaNotFoundException : Exception
    {
        public MediaNotFoundException() : base("Media Not Found Exception")
        {

        }
    }
}
