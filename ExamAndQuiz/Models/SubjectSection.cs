
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
    
public partial class SubjectSection
{

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public SubjectSection()
    {

        this.QuestionBanks = new HashSet<QuestionBank>();

    }


    public int ID { get; set; }

    public string Movzu { get; set; }

    public int FennID { get; set; }



    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<QuestionBank> QuestionBanks { get; set; }

    public virtual Subject Subject { get; set; }

}

}
