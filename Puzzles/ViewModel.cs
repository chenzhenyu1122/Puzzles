using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Puzzles 
{
    class ViewModel : INotifyPropertyChanged
    {
        class Puzzle
        {
            public string PuzzleText;
            public string Answer;
            public Puzzle(string text, string ans)
            {
                PuzzleText = text;
                Answer = ans;
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string NewProp)
        {
            if (PropertyChanged != null)
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(NewProp));
        }
        public Commands Check { get; set; } = new Commands();
        private string _puzzleText;
        public string PuzzleText
        {
            get { return _puzzleText; }
            set
            {
                _puzzleText = value;
                OnPropertyChanged("PuzzleText");
            }
        }
        private string _answer;
        public string Answer
        {
            get { return _answer; }
            set
            {
                _answer = value;
                OnPropertyChanged("Answer");
            }
        }
        List<Puzzle> Puzzles = new List<Puzzle>();
        Puzzle CurrentPuzzle;
        Random r = new Random();
        public ViewModel()
        {
            Check.Com = fSend;
            using (SqlConnection db = new SqlConnection("Data Source=DESKTOP-A0N5237;Initial Catalog=Puzzles;Integrated Security=True"))
            {
                SqlCommand query = new SqlCommand("select PuzzleText, Answer from Puzzles", db);
                SqlDataReader reader;
                try
                {
                    db.Open();
                    reader = query.ExecuteReader();
                    SqlDataAdapter adapter = new SqlDataAdapter(query);
                    DataTable table = new DataTable("Puzzles");
                    db.Close();
                    adapter.Fill(table);
                    foreach (DataRow row in table.Rows)
                    {
                        Puzzles.Add(new Puzzle((string)row.ItemArray[0], (string)row.ItemArray[1]));
                    }
                }
                catch
                {
                    MessageBox.Show("Нет доступа к базе данных");
                    return;
                }
            }
            CurrentPuzzle = Puzzles[r.Next(Puzzles.Count)];
            PuzzleText = CurrentPuzzle.PuzzleText;
        }
        public void fSend(object o)
        {
            if ((!string.IsNullOrEmpty(Answer)) && (Answer.ToLower() == CurrentPuzzle.Answer.ToLower()))
            {
                MessageBox.Show("Вы угадали!");
                CurrentPuzzle = Puzzles[r.Next(Puzzles.Count)];
                PuzzleText = CurrentPuzzle.PuzzleText;
                Answer = "";
            }
            else MessageBox.Show("Неверно! Попробуйте еще!");
        }
    }
    public class Commands : ICommand
    {
        public event EventHandler CanExecuteChanged;
        public Action<object> Com { get; set; }
        public bool CanExecute(object parameter) { return true; }
        public void Execute(object parameter)
        {
            Com(parameter);
        }
    }
}
