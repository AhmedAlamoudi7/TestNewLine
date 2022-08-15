using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestNewLine.Exceptions
{
    public class DuplicateEmailOrPhoneException : Exception
    {
        public DuplicateEmailOrPhoneException() : base("Duplicate Email Or Phone Exception")
        {

        }
    }
}
