using BuechereiApp.ViewModels;

namespace BuechereiApp.Pages
{
    public partial class MitgliedPage : ContentPage
    {
        private readonly MitgliedViewModel _vm;

        public MitgliedPage()
        {
            InitializeComponent();
            _vm = new MitgliedViewModel();
        }

        private async void OnLadenClicked(object sender, EventArgs e)
        {
            await _vm.LadeMitglieder();
            MitgliederListe.ItemsSource = _vm.Mitglieder;
        }

        private async void OnHinzufuegenClicked(object sender, EventArgs e)
        {
            var name = NameEntry.Text;
            var email = EmailEntry.Text;

            await _vm.MitgliedHinzufuegen(name, email);
            await _vm.LadeMitglieder();
            MitgliederListe.ItemsSource = null;
            MitgliederListe.ItemsSource = _vm.Mitglieder;
        }
    }
}