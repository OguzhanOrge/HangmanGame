using System.Diagnostics;

namespace HangmanGame
{
    public partial class MainPage : ContentPage
    {
        #region UI Properties
        public string Spotlight
        {
            get => spotlight; set
            {
                spotlight = value;
                OnPropertyChanged();
            }

        }
        public string Message
        {
            get => message; set
            {
                message = value;
                OnPropertyChanged();
            }
        }
        public string GameStatus
        {
            get => gameStatus; set
            {
                gameStatus = value;
                OnPropertyChanged();
            }
        }
        #endregion
        public List<char> Letters { get => letters; set { letters = value; OnPropertyChanged(); } }
        #region Fields
        List<string> word = new List<string>
        {
            "python",
            "dotnetmauı",
            "aspnet",
            "dotnet",
            "java",
            "javascript",
            "csharp",
            "cplusplus",
        };
        string answer = string.Empty;
        private List<char> letters = new List<char>();
        private string spotlight = string.Empty;
        List<char> guessed = new List<char>();
        private string message = string.Empty;
        int mistakes = 0;
        private string gameStatus = string.Empty;
        private string currentImage1 = "img0.jpg";

        public string currentImage { get => currentImage1; set { currentImage1 = value;OnPropertyChanged(); } }
        #endregion

        public MainPage()
        {
            InitializeComponent();
            Letters.AddRange("qwertyuıopğüasdfghjklşizxcvbnmöç");
            BindingContext = this;
            PickWord();
            CalculateWord(answer,guessed);
        }
        #region GameEngine
        public void PickWord()
        {
            answer = word[new Random().Next(0,word.Count)]; 
            Debug.WriteLine(answer);
        }
        public void CalculateWord(string answer, List<char> guessed)
        {
            var temp = answer.Select(x => (guessed.IndexOf(x) >= 0 ? x : '_')).ToArray();
            Spotlight = string.Join(' ',temp);
        }
        #endregion

        private void Button_Clicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            if (button != null)
            {
                var letter = button.Text;
                button.IsEnabled = false;
                HandleGues(letter[0]);
            }
        }

        private void HandleGues(char letter)
        {
            if (guessed.IndexOf(letter)== -1)
            {
                guessed.Add(letter);
            }
            if (answer.IndexOf(letter) >=0)
            {
                CalculateWord(answer, guessed);
                CheckIfGameWon();
            }
            else if(answer.IndexOf(letter) == -1)
            {
                mistakes++;
                UpdateStatus();
                CheckIfGameLost();
                currentImage = $"img{mistakes}.jpg";
            }
        }

        private void CheckIfGameLost()
        {
            if (mistakes == 6)
            {
                Message = "You Lost";
                DisableLetters();
            }
        }

        private void DisableLetters()
        {
            foreach (var children in LettersContainer.Children)
            {
                var btn = children as Button;
                if(btn != null)
                {
                    btn.IsEnabled = false;
                }
            }
        }

        private void CheckIfGameWon()
        {
            if (Spotlight.Replace(" ", "") == answer)
            {
                Message = "You Win";
            }
        }
        public void UpdateStatus()
        {
            GameStatus = $"Errors : {mistakes} / 6";
        }

        private void Button_Clicked_1(object sender, EventArgs e)
        {
            mistakes = 0 ;
            guessed = new List<char>();
            currentImage = "img0.jpg";
            PickWord();
            CalculateWord(answer, guessed);
            UpdateStatus();
            EnableLetters(); 
        }

        private void EnableLetters()
        {
            foreach (var children in LettersContainer.Children)
            {
                var btn = children as Button;
                if (btn != null)
                {
                    btn.IsEnabled = true;
                }
            }
        }
    }

}
