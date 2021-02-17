using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace dgfbhsd
{
    public partial class MainPage : ContentPage
    {
        public string SelectedBthDevice { get; set; }
        public MainPage()
        {
            InitializeComponent();
            
        }      

        private void AddPlayers(object sender, EventArgs e)
        {
            EntryCell cell = new EntryCell { Placeholder = "Enter Players Name" };
            TableSection tablesec = (TableSection)FindByName("tablesection");
            tablesec.Add(cell);
        }

        private void ConnectBluetooth(object sender, EventArgs e)
        {
            try
            {
                DependencyService.Get<IBth>().Connect("robot1");
                Button BTButt = (Button)FindByName("BTButt");
                BTButt.IsEnabled = false;
            }
            catch (Exception ex)
            {
                Application.Current.MainPage.DisplayAlert("Attention", ex.Message, "Ok");
            }
        }

        async private void startGame(object sender, EventArgs e)
        {
            TableSection tablesec = (TableSection)FindByName("tablesection");
            string[] playersName = new string[tablesec.Count];
            int n = 0;

            foreach (EntryCell el in tablesec)
            {
                playersName[n] = el.Text;
                n++;
            }
            var gamePage = new StartedPage(playersName);
            await Navigation.PushModalAsync(gamePage);
        }
    }
}
