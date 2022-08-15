using System;
using System.Collections.Generic;
using System.Text;
using TestNewLine.Core.ViewModels;

namespace TestNewLine.Core.ViewModel
{
    public class LoginResponseViewModel
    {
        public UserViewModel User { get; set; }

        public string AccessToken { get; set; }

    }
}
