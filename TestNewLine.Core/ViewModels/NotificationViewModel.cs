﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestNewLine.Core.ViewModels
{
    public class NotificationViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string CreateAt { get; set; }
        public string img { get; set; }
        public string Href { get; set; }
        public bool isCheked { get; set; }
        public string UserFrom { get; set; }
        public string UserTo { get; set; }
        public string Author { get; set; }

    }
}
