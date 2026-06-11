using BuechereiApp.Models;
using BuechereiApp.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace BuechereiApp.ViewModels
{

    public class BuecherViewModel
    {
        private readonly ApiService _api;
        public ObservableCollection<Buch> Buecher { get; set; } = new();

        public BuecherViewModel()
        {
            _api = new ApiService();
        }

        public async Task LadeBuecher()
        {
            var liste = await _api.GetBuecher();
            Buecher.Clear();
            foreach (var b in liste)
                Buecher.Add(b);
        }

        public async Task BuchHinzufuegen(string titel, string autor, string isbn, int menge)
        {
            await _api.BuchHinzufuegen(titel, autor, isbn, menge);
        }
    }





}
