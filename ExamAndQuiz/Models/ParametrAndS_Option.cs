using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExamAndQuiz.Models
{
    public class ParametrAndS_Option
    {
        public StudentOption MyStudentOption { get; set; }
        public IEnumerable<ExamParameter> MyExamParameter { get; set; }
    }
}