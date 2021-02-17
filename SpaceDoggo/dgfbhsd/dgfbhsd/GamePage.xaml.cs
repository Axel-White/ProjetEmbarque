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
    public partial class GamePage : ContentView
    {
        Dictionary<string, int> playersScore = new Dictionary<string, int>();
        public GamePage(string[] playersName)
        {
            foreach (string el in playersName)
            {
                playersScore.Add(el, 0);
            }
            InitializeComponent();
        }
    }
}