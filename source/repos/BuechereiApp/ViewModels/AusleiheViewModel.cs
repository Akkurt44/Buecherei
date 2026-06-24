using BuechereiApp.Models;
using BuechereiApp.Services;
using System.Collections.ObjectModel;

namespace BuechereiApp.ViewModels
{
    public class AusleiheViewModel
    {
        private readonly ApiService _api;
        public ObservableCollection<Ausleihe> Ausleihen { get; set; } = new();

        public AusleiheViewModel()
        {
            _api = new ApiService();
        }

        public async Task LadeAusleihen()
        {
            var liste = await _api.GetAusleihen();
            Ausleihen.Clear();
            foreach (var a in liste)
                Ausleihen.Add(a);
        }

        public async Task AusleiheBuch(int buchId, int mitgliedId)
        {
            await _api.AusleiheBuch(buchId, mitgliedId);
        }

        public async Task IadeBuch(int ausleiheId)
        {
            await _api.IadeBuch(ausleiheId);
        }
    }
}