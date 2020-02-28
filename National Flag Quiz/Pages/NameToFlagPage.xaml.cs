using System;
using System.Collections.Generic;
using System.Collections.Specialized;
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
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using Windows.Storage;

namespace National_Flag_Quiz
{
    public sealed partial class NameToFlagPage : Page
    {
        FlagCollection flag = new FlagCollection();
        DispatcherTimer Timer { get; set; }
        int TimeLimit { get; set; }
        int TimeElpased
        {
            get { return timeElpased; }
            set
            {
                timeElpased = value;
                txtTime.Text = (TimeLimit - value).ToString();
            }
        } int timeElpased;

        int Flag1
        {
            get { return flag1; }
            set
            {
                flag1 = value;
                imgFlag1.Source = new BitmapImage(flag[value].ImageUri);
                imgFlag1.Tag = value;
            }
        } int flag1;
        int Flag2
        {
            get { return flag2; }
            set
            {
                flag2 = value;
                imgFlag2.Source = new BitmapImage(flag[value].ImageUri);
                imgFlag2.Tag = value;
            }
        } int flag2;
        int Flag3
        {
            get { return flag3; }
            set
            {
                flag3 = value;
                imgFlag3.Source = new BitmapImage(flag[value].ImageUri);
                imgFlag3.Tag = value;
            }
        } int flag3;
        int Flag4
        {
            get { return flag4; }
            set
            {
                flag4 = value;
                imgFlag4.Source = new BitmapImage(flag[value].ImageUri);
                imgFlag4.Tag = value;
            }
        } int flag4;
        int FlagTarget
        {
            get { return flagTarget; }
            set
            {
                flagTarget = value;
                txtCountry.Text = flag[value].Name;
                flag[value].Used = true;
            }
        } int flagTarget;
        int Correct
        {
            get { return correct; }
            set
            {
                correct = value;
                txtCorrect.Text = "Correct: " + value.ToString();
                txtFlags.Text = "Flags: " + value + "/" + flag.Count;
            }
        } int correct;
        int Wrong
        {
            get { return wrong; }
            set
            {
                wrong = value;
                txtWrong.Text = "Wrong: " + value.ToString();
            }
        } int wrong;
        int Score
        {
            get { return 3 * correct - 2 * wrong; }
        }
        bool Finished { get; set; }

        public NameToFlagPage()
        {
            this.InitializeComponent();
            TimeLimit = 5;

            txtCountry.Text = "";
            txtTime.Text = TimeLimit.ToString();

            LoadFlags();
        }

        private async void LoadFlags()
        {
            StorageFolder InstallationFolder = Windows.ApplicationModel.Package.Current.InstalledLocation;
            StorageFolder flagsFolder = await InstallationFolder.GetFolderAsync("Assets\\Flags");
            var files = await flagsFolder.GetFilesAsync();

            flag = new FlagCollection();
            for (int i = 0; i < files.Count; i++)
                flag.Add(new Flag(Path.GetFileNameWithoutExtension(files[i].Name)));

            //Flag[] f = flag.Flag;
            //Array.Resize<Flag>(ref f, 12);
            //flag.Flag = f;

            txtFlags.Text = "Flags: 0/" + flag.Count;

            Start();
        }
        private void GetRandomFlags()
        {
            Random rnd = new Random();
            int[] flg = new int[5];

            for (int i = 1; i <= 4; i++)
                flg[i] = rnd.Next(0, flag.Count);


            for (int i = 1; i <= 4; i++)
                if (flag[flg[i]].Used == false)
                    break;
                else if (i == 4)
                {
                    int j = rnd.Next(1, 5);
                    while (flag[flg[j]].Used)
                        flg[j] = rnd.Next(0, flag.Count);
                }

            for (int i = 1; i <= 4; i++)
                for (int j = 1; j <= 4; j++)
                    if (i != j)
                        while (flg[i] == flg[j])
                            flg[i] = rnd.Next(0, flag.Count);

            flg[0] = flg[rnd.Next(1, 5)];
            while (flag[flg[0]].Used)
            {
                int i = rnd.Next(1, 5);
                flg[i] = rnd.Next(0, flag.Count);

                for (int j = 1; j <= 4; j++)
                    if (i != j)
                        while (flg[i] == flg[j])
                        {
                            flg[i] = rnd.Next(0, flag.Count);
                            j = 1;
                        }

                flg[0] = flg[i];
            }

            FlagTarget = flg[0];
            Flag1 = flg[1];
            Flag2 = flg[2];
            Flag3 = flg[3];
            Flag4 = flg[4];
        }

        private void Start()
        {
            GetRandomFlags();

            Timer = new DispatcherTimer();
            Timer.Interval = new TimeSpan(0, 0, 1);
            Timer.Tick += Timer_Tick;

            Timer.Start();
            TimeElpased = 0;
        }
        private void Stop()
        {
            Finished = true;
            Timer.Stop();

            this.Frame.Navigate(typeof(ScorePage), new int[] { Correct, Wrong });
        }

        private void Timer_Tick(object sender, object e)
        {
            TimeElpased += 1;
            txtTime.Text = (TimeLimit - TimeElpased).ToString();
 
            if (TimeElpased == TimeLimit)
                Stop();
        }
        private void imgFlag_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (Finished)
                return;

            Image imgFlag = (Image)sender;

            if (Convert.ToInt32(imgFlag.Tag) == FlagTarget)
                Correct += 1;
            else
            {
                Wrong += 1;
                flag[FlagTarget].Used = false;
            }

            if(correct == flag.Count)
            {
                Stop();
                return;
            }

            GetRandomFlags();
            txtScore.Text = "Score: " + Score;
        }

        private void btnBack_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage), null);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            Timer.Stop();
        }

        private void btnRetry_Tapped(object sender, TappedRoutedEventArgs e)
        {
        }
    }
}
