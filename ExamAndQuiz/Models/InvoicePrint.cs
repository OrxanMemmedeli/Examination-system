using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExamAndQuiz.Models
{
    public class InvoicePrint
    {
        public IEnumerable<StudentAction> MyStudentActions { get; set; }
        public About MyAbout { get; set; }
        public IEnumerable<ExamRule> MyExamRules { get; set; }
    }
}