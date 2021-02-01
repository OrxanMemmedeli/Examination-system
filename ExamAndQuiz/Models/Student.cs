
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

    public partial class Student
    {

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Student()
        {

            this.ExamResults = new HashSet<ExamResult>();

            this.StudentActions = new HashSet<StudentAction>();

            this.StudentOptions = new HashSet<StudentOption>();

        }


        public int ID { get; set; }

        public string Ad { get; set; }

        public string Soyad { get; set; }

        public string AtaAdi { get; set; }

        public string SVNomresi { get; set; }

        public Nullable<int> SagirdKodu { get; set; }

        public int SinifID { get; set; }

        public string Cins { get; set; }

        public System.DateTime DogumTarixi { get; set; }

        public string Email { get; set; }

        public string WhatsApp { get; set; }

        public string MobilNomre { get; set; }

        public Nullable<int> QrupID { get; set; }

        public string Parol { get; set; }
        public virtual string TekrarParol { get; set; }


        public System.DateTime QeydiyyatTarixi { get; set; }

        public int SeherID { get; set; }

        public string FotoURL { get; set; }



        public virtual City City { get; set; }

        public virtual CourseGroup CourseGroup { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

        public virtual ICollection<ExamResult> ExamResults { get; set; }

        public virtual Grade Grade { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

        public virtual ICollection<StudentAction> StudentActions { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

        public virtual ICollection<StudentOption> StudentOptions { get; set; }

    }

}
