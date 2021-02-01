using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExamAndQuiz.Models
{
    public class Profil
    {
        public Student student { get; set; }
        public IEnumerable<StudentAction> studentActionsBeAttended { get; set; }
        public IEnumerable<Exam> exams { get; set; }
        public IEnumerable<StudentOption> studentOption { get; set; }
    }
}