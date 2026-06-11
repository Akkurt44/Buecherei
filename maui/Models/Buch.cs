using System;
using System.Collections.Generic;
using System.Text;

namespace BuechereiApp.Models
{


    public class Buch
    {
        public int Id { get; set; }
        public string Titel { get; set; }
        public string Autor { get; set; }
        public string Isbn { get; set; }
        public int Menge { get; set; }
    }




}
