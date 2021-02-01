
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


namespace ExamAndQuiz.Models
{

    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;

    public partial class QuestionBank
    {

        public int ID { get; set; }
        [AllowHtml]
        public string Sual { get; set; }
        [AllowHtml]
        public string Cavab { get; set; }

        public Nullable<int> Cetinlik { get; set; }

        public Nullable<int> Movzu { get; set; }

        public int FennID { get; set; }

        public bool Status { get; set; }

        public System.DateTime YaranmaTarixi { get; set; }

        public int TipID { get; set; }

        public int ImtahanID { get; set; }

        public Nullable<int> AdminID { get; set; }



        public virtual Admin Admin { get; set; }

        public virtual Exam Exam { get; set; }

        public virtual QuestionType QuestionType { get; set; }

        public virtual Subject Subject { get; set; }

        public virtual SubjectSection SubjectSection { get; set; }

    }

}
