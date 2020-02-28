using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace National_Flag_Quiz
{
    public sealed partial class ScorePage : Page
    {
        public ScorePage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            int[] score = e.Parameter as int[];

            int correct = score[0];
            int wrong = score[1];

            txtCorrect.Text = "3 x " + correct + " = " + (3 * correct);
            txtWrong.Text = "-2 x " + wrong + " = " + (-2 * wrong);
            txtScore.Text = (3 * correct) + " - " + (2 * wrong) + " = " + (3 * correct - 2 * wrong);
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage), null);
        }
    }
}
