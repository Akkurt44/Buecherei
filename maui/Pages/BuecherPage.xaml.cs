using BuechereiApp.ViewModels;




namespace BuechereiApp.Pages
{
    public partial class BuecherPage : ContentPage
    {
        private readonly BuecherViewModel _vm;

        public BuecherPage()
        {
            InitializeComponent();
            _vm = new BuecherViewModel();
        }

        private async void OnLadenClicked(object sender, EventArgs e)
        {
            await _vm.LadeBuecher();
            BuecherListe.ItemsSource = _vm.Buecher;
        }

        private async void OnHinzufuegenClicked(object sender, EventArgs e)
        {
            var titel = TitelEntry.Text;
            var autor = AutorEntry.Text;
            var isbn = IsbnEntry.Text;
            var menge = int.Parse(MengeEntry.Text);

            await _vm.BuchHinzufuegen(titel, autor, isbn, menge);
            await _vm.LadeBuecher();
            BuecherListe.ItemsSource = null;
            BuecherListe.ItemsSource = _vm.Buecher;
        }



    }
}


