using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace dgfbhsd
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StartedPage : ContentPage
    {
        Dictionary<string, int> playersScore = new Dictionary<string, int>();
        int turn;
        string key;
        int nbrMaxPlayer;
        int nbr = 0;
        Task wtScore;
        bool wait = true;
        bool gameIsFinished = false;

        public StartedPage(string[] playersName)
        {
            InitializeComponent();
            
            this.turn = 1;
            this.nbrMaxPlayer = playersName.Length;
            turnLabel.Text = "Turn " + turn + " / 10";
            this.key = playersName[0];
            playerLabel.Text = this.key + " is preparing is shoot ....";
            foreach (string el in playersName)
            {
                playersScore.Add(el, 0);
            }
            //waitforscore();
        }
        async void win(object sender, EventArgs e)
        {
            if (nbr >= nbrMaxPlayer - 1)
            {
                nbr = 0;
                turn++;
                turnLabel.Text = "Turn " + turn + " / 10";
                playersScore[key]++;
            }
            else
            {
                playersScore[key]++;
                nbr++;
            }
            key = playersScore.ElementAt(nbr).Key;
            playerLabel.Text = this.key + " is preparing is shoot ....";

            if (nbr >= nbrMaxPlayer - 1 && turn == 10)
            {
                await DisplayAlert("And The Winner is ....", this.findWinner(), "OK");
                var mainPage = new MainPage();
                await Navigation.PushModalAsync(mainPage);
            }
        }

        async void fail(object sender, EventArgs e)
        {
            if (nbr >= nbrMaxPlayer - 1)
            {
                nbr = 0;
                turn++;
                turnLabel.Text = "Turn " + turn + " / 10";
            }
            else
            {
                nbr++;
            }
            key = playersScore.ElementAt(nbr).Key;
            playerLabel.Text = this.key + " is preparing is shoot ....";
            if (nbr >= nbrMaxPlayer - 1 && turn == 10)
            {
                await DisplayAlert("And The Winner is ....", this.findWinner(), "OK");
                var mainPage = new MainPage();
                await Navigation.PushModalAsync(mainPage);
            }
        }

        string findWinner()
        {
            string res = "";
            int score = 0;

            foreach (KeyValuePair<string, int> el in this.playersScore)
            {
                if (el.Value > score)
                {
                    res = el.Key;
                }
            }

            return res;
        }

        private async void waitforscore()
        {
            try
            {
                while (!gameIsFinished)
                {
                    wait = false;
                    wtScore = Task.Run(async () => DependencyService.Get<IBth>().WaitForScore());
                    while (wait)
                    {
                        if (wtScore.IsCompleted)
                        {
                            if (nbr >= nbrMaxPlayer - 1)
                            {
                                nbr = 0;
                                turn++;
                                turnLabel.Text = "Turn " + turn + " / 10";
                                playersScore[key]++;
                            }
                            else
                            {
                                playersScore[key]++;
                                nbr++;
                            }
                            key = playersScore.ElementAt(nbr).Key;
                            playerLabel.Text = this.key + " is preparing is shoot ....";

                            if (nbr >= nbrMaxPlayer - 1 && turn == 10)
                            {
                                gameIsFinished = true;
                                await DisplayAlert("And The Winner is ....", this.findWinner(), "OK");
                                var mainPage = new MainPage();
                                await Navigation.PushModalAsync(mainPage);
                            }
                        }
                        wait = false;
                    }
                }
            }
            catch
            {

            }
        }


    }
}