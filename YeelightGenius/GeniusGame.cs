using System;
using System.Collections.Generic;
using System.Windows.Media;

namespace YeelightGenius
{
    public class GeniusGame
    {
        private readonly Color[] COLORS = new Color[]
        {
            Constants.RED,
            Constants.YELLOW,
            Constants.GREEN,
            Constants.BLUE,
        };

        private Random rand;

        public GeniusGame()
        {
            this.GameSequence = new List<Color>();
            this.GuessSequence = new List<Color>();
            this.rand = new Random(DateTime.Now.Millisecond);
        }

        public EventHandler OnCorrectGuess { get; set; }

        public EventHandler OnWrongGuess { get; set; }

        public List<Color> GameSequence { get; private set; }

        public List<Color> GuessSequence { get; private set; }

        public int Score { get => this.GameSequence.Count; }

        public void ResetGame()
        {
            this.GameSequence.Clear();
            this.GuessSequence.Clear();
        }

        public void AddNewItemToSequence()
        {
            Color c = this.COLORS[this.rand.Next(this.COLORS.Length)];
            this.GameSequence.Add(c);
        }

        public void GuessNextColor(Color c)
        {
            this.GuessSequence.Add(c);

            if (this.GuessSequence[this.GuessSequence.Count-1] != this.GameSequence[this.GuessSequence.Count-1])
            {
                this.OnWrongGuess?.Invoke(this, EventArgs.Empty);
            }
            else if (this.GuessSequence.Count == this.GameSequence.Count)
            {
                this.GuessSequence.Clear();
                this.OnCorrectGuess?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
