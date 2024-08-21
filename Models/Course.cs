using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LMS.Models
{
    public class Course
    {
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public string CourseCode { get; set; }
        public List<Module> Modules { get; set; } = new List<Module>();
    }

    public class Module
    {
        public int ModuleId { get; set; }
        public string ModuleName { get; set; }
        public string ModuleLink { get; set; }
    }

}