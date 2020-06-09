using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
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

namespace TafelTester
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        Random random = new Random();

        int maxB = 100;

        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Prevents the user from entering any invalid text into the textbox.
        /// In this case all non-integer text will not be entered.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void IntOnlyText(object sender, TextChangedEventArgs e)
        {
            if (!(sender is TextBox))
            {
                return;
            }

            TextBox box = sender as TextBox;

            ///If it doesn't parse, remove the last changes.
            if (!BigInteger.TryParse(box.Text, out BigInteger result))
            {
                TextChange[] test = e.Changes.ToArray();

                if (test[0].AddedLength > 0)
                {
                    box.Text = box.Text.Remove(test[0].Offset, test[0].AddedLength);
                    box.SelectionStart = test[0].Offset;
                }
            }
        }

        /// <summary>
        /// Fills in the different labels with sums to complete.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TafelButton_Click(object sender, RoutedEventArgs e)
        {
            //There should be a maximum number entered in the TafelTextBox
            if (TafelTextBox.Text.Length == 0)
            {
                return;
            }
            //Since our textboxes take any integer (e.g. integers larger then Int32.MaxValue we need to make sure it is an int for our randomizer.
            if (!int.TryParse(TafelTextBox.Text, out int result))
            {
                TafelTextBox.Text = string.Empty;
                ScoreLabel.Content = "Ben je echt zó goed in rekenen dat je tafels van miljarden wilt hebben? Dit kan ik helaas niet voor je genereren.";
                return;
            }
            int tafel = int.Parse(TafelTextBox.Text);

            //Fill in all the labels with equations to solve.
            foreach (Label label in SomGrid.Children.OfType<Label>())
            {
                int randomNumber = random.Next(2, tafel + 1);
                label.Content = $"{label.Name.Last()} x {randomNumber} =";
            }
        }

        /// <summary>
        /// Calculates and displays the score if all answers have been filled in.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ShowScoreButton_Click(object sender, RoutedEventArgs e)
        {
            int score = 0;
            foreach (TextBox box in SomGrid.Children.OfType<TextBox>())
            {
                if (box.Text != string.Empty)
                {
                    string matchName = $"SomLabel{box.Name.Last()}";

                    //find the matching equation
                    Label label = SomGrid.Children.OfType<Label>().Where<Label>(xs => xs.Name == matchName).FirstOrDefault();
                    string[] som = label.Content.ToString().Split('x');
                    BigInteger rand = BigInteger.Parse(som[1].Remove(som[1].Length - 1));
                    //calculate the answer and convert it to a string so we can compare it to the user input.
                    string answer = (int.Parse(box.Name.Last().ToString()) * rand).ToString();
                    if (answer == box.Text)
                    {
                        score++;
                    }
                }
                else
                {
                    //One of the boxes has been empty.
                    ScoreLabel.Content = "Vul eerst alle sommen in!";
                    return;
                }
            }
            //If all boxes were filled in, we print the score (correct answers * 2)

            ScoreLabel.Content = $"Je score is: {score * 2} (งಠᨎಠ)ง";//(\u0E07\u0CA0\u1A0E
        }
    }
}
