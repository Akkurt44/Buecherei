using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace BuechereiApp.Models
{

    public class Ausleihe
    {
        public int Id { get; set; }

        [JsonPropertyName("buch_id")]
        public int BuchId { get; set; }

        [JsonPropertyName("mitglied_id")]
        public int MitgliedId { get; set; }

        [JsonPropertyName("ausleihe_datum")]
        public string AusleiheDatum { get; set; }

        [JsonPropertyName("rueckgabe_datum")]
        public string RueckgabeDatum { get; set; }
    }




}
