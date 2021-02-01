using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExamAndQuiz.Models
{
    public class OptionAndParameter
    {
        private string[] inputCavablar;
        public ExamOption MyExamOption { get; set; }
        public IEnumerable<ExamParameter> MyExamParameter { get; set; }

        public string[] InputCavablar { get => inputCavablar; set => inputCavablar = value; }
    }
}