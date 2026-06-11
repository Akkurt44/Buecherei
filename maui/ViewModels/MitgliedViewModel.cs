using BuechereiApp.Models;
using BuechereiApp.Services;
using System.Collections.ObjectModel;

namespace BuechereiApp.ViewModels
{
    public class MitgliedViewModel
    {
        private readonly ApiService _api;
        public ObservableCollection<Mitglied> Mitglieder { get; set; } = new();

        public MitgliedViewModel()
        {
            _api = new ApiService();
        }

        public async Task LadeMitglieder()
        {
            var liste = await _api.GetMitglieder();
            Mitglieder.Clear();
            foreach (var m in liste)
                Mitglieder.Add(m);
        }

        public async Task MitgliedHinzufuegen(string name, string email)
        {
            await _api.MitgliedHinzufuegen(name, email);
        }
    }
}