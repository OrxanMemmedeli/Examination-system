using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExamAndQuiz.Models
{
    public class İnvoice
    {
        public StudentAction MyStudentAction { get; set; }
        public About MyAbout { get; set; }
        public IEnumerable<ExamRule> MyExamRule { get; set; }
    }
}