using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LMS.Models
{
    public class MasterRecord
    {
        public int InstructorId {  get; set; }  

        public string InstructorName {  get; set; }

        public int  LearnerId { get; set; }

        public string LearnerName { get; set; } 

        public string Coursecode {  get; set; }
        public string Coursename { get; set; }  

    }
}