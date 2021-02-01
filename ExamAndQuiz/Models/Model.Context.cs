﻿

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
using System.Data.Entity;
using System.Data.Entity.Infrastructure;


public partial class OnlineExamEntities : DbContext
{
    public OnlineExamEntities()
        : base("name=OnlineExamEntities")
    {

    }

    protected override void OnModelCreating(DbModelBuilder modelBuilder)
    {
        throw new UnintentionalCodeFirstException();
    }


    public virtual DbSet<About> Abouts { get; set; }

    public virtual DbSet<Admin> Admins { get; set; }

    public virtual DbSet<Blog> Blogs { get; set; }

    public virtual DbSet<BookletOption> BookletOptions { get; set; }

    public virtual DbSet<City> Cities { get; set; }

    public virtual DbSet<CourseGroup> CourseGroups { get; set; }

    public virtual DbSet<Exam> Exams { get; set; }

    public virtual DbSet<ExamQuestion> ExamQuestions { get; set; }

    public virtual DbSet<ExamOption> ExamOptions { get; set; }

    public virtual DbSet<ExamParameter> ExamParameters { get; set; }

    public virtual DbSet<ExamResult> ExamResults { get; set; }

    public virtual DbSet<ExamRule> ExamRules { get; set; }

    public virtual DbSet<ExamType> ExamTypes { get; set; }

    public virtual DbSet<ExamTypeParameter> ExamTypeParameters { get; set; }

    public virtual DbSet<ExamVideo> ExamVideos { get; set; }

    public virtual DbSet<FreeTicket> FreeTickets { get; set; }

    public virtual DbSet<Grade> Grades { get; set; }

    public virtual DbSet<QuestionBank> QuestionBanks { get; set; }

    public virtual DbSet<QuestionType> QuestionTypes { get; set; }

    public virtual DbSet<PDFFile> PDFFiles { get; set; }

    public virtual DbSet<Student> Students { get; set; }

    public virtual DbSet<StudentAction> StudentActions { get; set; }

    public virtual DbSet<StudentOption> StudentOptions { get; set; }

    public virtual DbSet<Subject> Subjects { get; set; }

    public virtual DbSet<SubjectSection> SubjectSections { get; set; }

    public virtual DbSet<sysdiagram> sysdiagrams { get; set; }

}

}
