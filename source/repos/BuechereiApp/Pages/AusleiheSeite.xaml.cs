using BuechereiApp.ViewModels;

namespace BuechereiApp.Pages
{
    public partial class AusleiheSeite : ContentPage
    {
        private readonly AusleiheViewModel _vm;

        public AusleiheSeite()
        {
            InitializeComponent();
            _vm = new AusleiheViewModel();
        }

        private async void OnAusleihenClicked(object sender, EventArgs e)
        {
            int buchId = int.Parse(BuchIdEntry.Text);
            int mitgliedId = int.Parse(MitgliedIdEntry.Text);
            await _vm.AusleiheBuch(buchId, mitgliedId);
            await DisplayAlert("Erfolg", "Buch ausgeliehen!", "OK");
        }

        private async void OnIadeClicked(object sender, EventArgs e)
        {
            int ausleiheId = int.Parse(AusleiheIdEntry.Text);
            await _vm.IadeBuch(ausleiheId);
            await DisplayAlert("Erfolg", "Buch zurückgegeben!", "OK");
        }

        private async void OnLadenClicked(object sender, EventArgs e)
        {
            await _vm.LadeAusleihen();
            AusleihenListe.ItemsSource = _vm.Ausleihen;
        }
    }
}