
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
    
public partial class ExamParameter
{

    public int ID { get; set; }

    public int TipParametrID { get; set; }

    public int Sira { get; set; }

    public int FenninID { get; set; }

    public int Bal { get; set; }

    public int UmumiSualSayi { get; set; }

    public Nullable<int> AciqSualSayi { get; set; }

    public string AciqSuallar { get; set; }

    public Nullable<int> SehvBal { get; set; }

    public Nullable<int> AciqSehvBal { get; set; }



    public virtual ExamTypeParameter ExamTypeParameter { get; set; }

    public virtual Subject Subject { get; set; }

}

}
