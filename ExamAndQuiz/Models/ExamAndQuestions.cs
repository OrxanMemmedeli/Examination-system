using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExamAndQuiz.Models
{
    public class ExamAndQuestions
    {
        public Exam MyExam { get; set; }
        public IEnumerable<ExamQuestion> MyExamQuestion { get; set; }
    }
}