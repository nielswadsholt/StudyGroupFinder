using Graph;
using StudyGroupFinder;
using StudyGroupFinderWeb.Storage;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Util;

namespace StudyGroupFinderWeb.Models
{
    public class StudentViewModel
    {
        [RegularExpression(@"^[A-ZÆØÅ][a-zA-Z0-9æøåÆØÅ]*([-][A-ZÆØÅ][a-zA-Z0-9æøåÆØÅ]+)*$", 
            ErrorMessage = "Navnet skal starte med stort bogstav og må ikke indeholde specialtegn eller mellemrum.")]
        [Required(ErrorMessage = "Du skal indtaste dit navn.")]
        [StringLength(60, MinimumLength = 3, 
            ErrorMessage = "Navnet skal være på mellem 3 og 60 tegn.")]
        [DisplayName("Navn")]
        public string Name { get; set; }

        [RegularExpression(@"^[A-ZÆØÅ][a-zA-Z0-9æøåÆØÅ]*([-][A-ZÆØÅ][a-zA-Z0-9æøåÆØÅ]+)*$", 
            ErrorMessage = "Studienavnet skal starte med stort bogstav og må ikke indeholde specialtegn eller mellemrum.")]
        [Required(ErrorMessage = "Du skal indtaste dit studie.")]
        [StringLength(60, MinimumLength = 3, 
            ErrorMessage = "Studienavnet skal være på mellem 3 og 60 tegn.")]
        [DisplayName("Studie")]
        public string Study { get; set; }
        
        [Required(ErrorMessage = "Du skal angive, om du søger studiegruppe.")]
        [DisplayName("Søger studiegruppe?")]
        public bool SeeksGroup { get; set; }

        [RegularExpression(@"^[a-zA-Z0-9æøåÆØÅ]*([\s][a-zA-Z0-9æøåÆØÅ]+)*$", 
            ErrorMessage = "Specialtegn er ikke tilladt og mellemrum skal efterfølges af tegn.")]
        [DisplayName("Personlige egenskaber")]
        public string Attributes { get; set; }

        [RegularExpression(@"^[a-zA-Z0-9æøåÆØÅ]*([\s][a-zA-Z0-9æøåÆØÅ]+)*$", 
            ErrorMessage = "Specialtegn er ikke tilladt og mellemrum skal efterfølges af tegn.")]
        [DisplayName("Studierelaterede egenskaber")]
        public string StudyAttributes { get; set; }

        [RegularExpression(@"^[A-ZÆØÅ][a-zA-Z0-9æøåÆØÅ-]*([\s][A-ZÆØÅ][a-zA-Z0-9æøåÆØÅ-]+)*$", 
            ErrorMessage = "Navnene skal starte med stort bogstav og må ikke indeholde andre specialtegn end bindestreg. Mellemrum skal efterfølges af tegn.")]
        [DisplayName("Naboer")]
        public string Neighbors { get; set; }
        public List<Node<Student>> AllNodes { get; set; }

        public StudentViewModel()
        {
            Name = "";
            Study = "";
            Attributes = "";
            StudyAttributes = "";
            SeeksGroup = true;
            Neighbors = "";
            AllNodes = StudentGraph.GetAll();
        }

        public Student ToStudent()
        {
            Student student = new Student((Name ?? " "), (Study ?? " "));
            student.Attributes = new HashSet<string>((Attributes ?? " ").Split(' '));
            student.StudyAttributes = new HashSet<string>((StudyAttributes ?? " ").Split(' '));
            student.SeeksGroup = SeeksGroup;
            return student;
        }

        public Node<Student> ToStudentNode()
        {
            var studentNode = new Node<Student>(ToStudent());

            foreach (string n in (Neighbors ?? " ").Split(' '))
            {
                studentNode.AddNeighbor(AllNodes.FirstOrDefault(node => node.Data.Name == n));
            }

            return studentNode;
        }
    }
}