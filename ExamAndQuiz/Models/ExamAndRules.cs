using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExamAndQuiz.Models
{
    public class ExamAndRules
    {
        public Exam MyExam { get; set; }
        public IEnumerable<ExamRule> MyExamRule { get; set; }
    }
}