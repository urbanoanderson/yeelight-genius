using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace YeelightGenius
{
    public partial class MainWindow : Window
    {
        private YeelightManager yeelightManager;

        private GeniusGame geniusGame;

        private SoundPlayer sfxRed;

        private SoundPlayer sfxYellow;

        private SoundPlayer sfxGreen;

        private SoundPlayer sfxBlue;

        public MainWindow()
        {
            InitializeComponent();
            this.sfxRed = new SoundPlayer(Constants.PATH_FX_DO);
            this.sfxYellow = new SoundPlayer(Constants.PATH_FX_RE);
            this.sfxGreen = new SoundPlayer(Constants.PATH_FX_MI);
            this.sfxBlue = new SoundPlayer(Constants.PATH_FX_FA);
            this.geniusGame = new GeniusGame();
            this.geniusGame.OnCorrectGuess += this.HandleCorrectGuess;
            this.geniusGame.OnWrongGuess += this.HandleWrongGuess;
            this.ResetUI();
            this.ButtonStart.IsEnabled = false;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.sfxRed.Dispose();
            this.sfxYellow.Dispose();
            this.sfxGreen.Dispose();
            this.sfxBlue.Dispose();
        }

        private void ResetUI()
        {
            this.ButtonRed.IsEnabled = false;
            this.ButtonYellow.IsEnabled = false;
            this.ButtonGreen.IsEnabled = false;
            this.ButtonBlue.IsEnabled = false;
            this.ButtonCancel.IsEnabled = false;
            this.ButtonStart.IsEnabled = true;
            this.ButtonStart.Content = "Start Game";
        }

        private async void StatusBarLampState_Initialized(object sender, EventArgs e)
        {
            this.yeelightManager = new YeelightManager();

            this.yeelightManager.OnConnectionAttemptEnd += (o, connected) =>
            {
                if (connected)
                {
                    this.StatusBarLogText.Text = "Let's play genius!";
                    this.StatusBarLampState.Text = "Connected";
                    this.StatusBarLampState.Foreground = Brushes.Green;
                    this.ButtonStart.IsEnabled = true;
                }
                else
                {
                    this.StatusBarLampState.Text = "Connection Failed";
                    this.StatusBarLampState.Foreground = Brushes.Red;
                }
            };

            await this.yeelightManager.Connect();
        }

        private async void ButtonStart_Click(object sender, RoutedEventArgs e)
        {
            this.ButtonRed.IsEnabled = true;
            this.ButtonYellow.IsEnabled = true;
            this.ButtonGreen.IsEnabled = true;
            this.ButtonBlue.IsEnabled = true;
            this.ButtonStart.IsEnabled = false;
            this.ButtonCancel.IsEnabled = true;
            this.StatusBarLogText.Text = string.Empty;
            this.LabelGameOver.Visibility = Visibility.Hidden;

            this.geniusGame.AddNewItemToSequence();

            foreach (Color c in this.geniusGame.GameSequence)
            {
                await this.FlashColor(c);
            }
        }

        private async void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            this.ResetUI();
            this.geniusGame.ResetGame();

            await this.yeelightManager.SetRgbColor(Constants.WHITE);
        }

        private async void ButtonRed_Click(object sender, RoutedEventArgs e)
        {
            this.geniusGame.GuessNextColor(Constants.RED);
            await this.FlashColor(Constants.RED);
        }

        private async void ButtonYellow_Click(object sender, RoutedEventArgs e)
        {
            this.geniusGame.GuessNextColor(Constants.YELLOW);
            await this.FlashColor(Constants.YELLOW);
        }

        private async void ButtonGreen_Click(object sender, RoutedEventArgs e)
        {
            this.geniusGame.GuessNextColor(Constants.GREEN);
            await this.FlashColor(Constants.GREEN);
        }

        private async void ButtonBlue_Click(object sender, RoutedEventArgs e)
        {
            this.geniusGame.GuessNextColor(Constants.BLUE);
            await this.FlashColor(Constants.BLUE);
        }

        private void HandleWrongGuess(object sender, EventArgs e)
        {
            this.LabelGameOver.Visibility = Visibility.Visible;
            this.StatusBarLogText.Text = $"Wrong answer! Your score: {this.geniusGame.Score}";
            this.ResetUI();
        }

        private void HandleCorrectGuess(object sender, EventArgs e)
        {
            this.ButtonRed.IsEnabled = false;
            this.ButtonYellow.IsEnabled = false;
            this.ButtonGreen.IsEnabled = false;
            this.ButtonBlue.IsEnabled = false;
            this.ButtonStart.IsEnabled = true;
            this.ButtonCancel.IsEnabled = false;

            this.StatusBarLogText.Text = $"Correct answer! Current score: {this.geniusGame.Score}";
            this.ButtonStart.Content = $"Start Level {this.geniusGame.Score+1}";
        }

        private async Task FlashColor(Color c)
        {
            this.PlayColorSound(c);
            await this.yeelightManager.SetRgbColor(c);
            await Task.Delay(Constants.LIGHT_DELAY);
            await this.yeelightManager.SetRgbColor(Constants.WHITE);
            await Task.Delay(Constants.LIGHT_DELAY);
        }

        private void PlayColorSound(Color c)
        {
            if (c == Constants.RED)
                this.sfxRed.Play();
            else if (c == Constants.YELLOW)
                this.sfxYellow.Play();
            else if (c == Constants.GREEN)
                this.sfxGreen.Play();
            else if (c == Constants.BLUE)
                this.sfxBlue.Play();
            else
                SystemSounds.Exclamation.Play();
        }
    }
}
