
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
    
public partial class Admin
{

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public Admin()
    {

        this.Exams = new HashSet<Exam>();

        this.QuestionBanks = new HashSet<QuestionBank>();

    }


    public int ID { get; set; }

    public string Ad { get; set; }

    public string Soyad { get; set; }

    public string Email { get; set; }

    public string Parol { get; set; }
    public virtual string TekrarParol { get; set; }

    public System.DateTime QeydiyyatTarixi { get; set; }



    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<Exam> Exams { get; set; }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<QuestionBank> QuestionBanks { get; set; }

}

}