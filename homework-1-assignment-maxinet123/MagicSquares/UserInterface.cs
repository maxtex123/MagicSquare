/* 
 * UserInterface.cs
 * By: Maxine Teixeira
 */
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MagicSquares
{
    public partial class UserInterface : Form
    {
        TextBox[,] _textBoxGrid = new TextBox[Square.SIZE, Square.SIZE];                    //textbox array
        Label[] _labelRows = new Label[Square.SIZE];                                        //label row array
        Label[] _labelColumns = new Label[Square.SIZE];                                     //label column array
        Square _currentPuzzle = new Square();                                               //Square object
        Square _solution = null;                                                            //Square object declared null
        /// <summary>
        /// Declares all the textboxes into an array. Does the same for the labels for rows in one and columns in another.
        /// Implements the leave and HandleUserInput method for all textboxes.
        /// 
        /// (help from office hours --Josh Weese)
        /// </summary>
        public UserInterface()
        {
            InitializeComponent();
            _textBoxGrid[0, 0] = ux0_0;
            _textBoxGrid[0, 1] = ux0_1;
            _textBoxGrid[0, 2] = ux0_2;
            _textBoxGrid[0, 3] = ux0_3;
            _textBoxGrid[1, 0] = ux1_0;
            _textBoxGrid[1, 1] = ux1_1;
            _textBoxGrid[1, 2] = ux1_2;
            _textBoxGrid[1, 3] = ux1_3;
            _textBoxGrid[2, 0] = ux2_0;
            _textBoxGrid[2, 1] = ux2_1;
            _textBoxGrid[2, 2] = ux2_2;
            _textBoxGrid[2, 3] = ux2_3;
            _textBoxGrid[3, 0] = ux3_0;
            _textBoxGrid[3, 1] = ux3_1;
            _textBoxGrid[3, 2] = ux3_2;
            _textBoxGrid[3, 3] = ux3_3;

            _labelRows[0] = uxRow0;
            _labelRows[1] = uxRow1;
            _labelRows[2] = uxRow2;
            _labelRows[3] = uxRow3;

            _labelColumns[0] = uxCol0;
            _labelColumns[1] = uxCol1;
            _labelColumns[2] = uxCol2;
            _labelColumns[3] = uxCol3;

            for (int i = 0; i < Square.SIZE; i++)
            {
                for (int j = 0; j < Square.SIZE; j++)
                {
                    _textBoxGrid[i, j].Leave += new EventHandler(HandleUserInput);
                }
            }
        }

        /// <summary>
        /// Finds the textbox that the user would like to enter information into as long as the textbox is not empty.
        /// Users is then only allowed to enter an integer between teh numbers of 1 and 16. If anything else is entered then a message box will display the message and the textbox will be clear to allow new entries.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HandleUserInput(object sender, EventArgs e)
        {
            TextBox txtBox = (TextBox)sender;
            int row = 0;
            int col = 0;
            bool found = false;
            for (int i = 0; i < Square.SIZE; i++)
            {
                for (int j = 0; j < Square.SIZE; j++)
                {
                    if (txtBox == _textBoxGrid[i, j])
                    {
                        row = i;
                        col = j;
                        found = true;
                        break;
                    }
                }
                if (found) break;
            }
            if (txtBox.Text != "")
            {
                try
                {
                    int val = Convert.ToInt32(txtBox.Text);
                    if (val >= 1 && val <= 16)
                    {
                        _currentPuzzle[row, col] = val;
                        if (_solution != null && !_solution.Complete())
                            _solution = null;
                    }
                    else
                    {
                        MessageBox.Show("Entries must be numbers 1-16");
                        _currentPuzzle[row, col] = 0;
                        txtBox.Text = "";
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Entries must be numbers 1-16");
                    _currentPuzzle[row, col] = 0;
                    txtBox.Text = "";
                }
            }
            else
            {
                _currentPuzzle[row, col] = 0;
            }
            UpdateGUI(false);
        }

        /// <summary>
        /// Updates the labels with the sums for all the rows, columns, and both diagonals. 
        /// If the current puzzle is not equal to 0 then it places the current puzzle in the textboxes.
        /// </summary>
        /// <param name="disableAll"></param>
        private void UpdateGUI(bool disableAll)
        {
            for (int i = 0; i < Square.SIZE; i++)
            {
                _labelRows[i].Text = _currentPuzzle.SumRow(i).ToString();
                _labelColumns[i].Text = _currentPuzzle.SumColumn(i).ToString();
                for (int j = 0; j < Square.SIZE; j++)
                {
                    if (_currentPuzzle[i, j] != 0)
                    {
                        _textBoxGrid[i, j].Text = _currentPuzzle[i, j].ToString();
                        if (disableAll)
                        {
                            _textBoxGrid[i, j].Enabled = false;
                        }
                    }
                }
            }

            uxDiagonalTopBottom.Text = _currentPuzzle.SumTopBottom().ToString();
            uxDiagonalBottomTop.Text = _currentPuzzle.SumBottomTop().ToString();
        }

        /// <summary>
        /// Triggered when the user clicks on the check button.
        /// If the conflict method is true then an error message will display in the textbox. If the conflict method is false, then the current puzzle will be checked using the compelte method.
        /// If the complete method is true then the hint and solve button will be disabled, the textboxes will all be disabled and a message stating the solved the puzzle will be displayed in a messagebox.
        /// if the current puzzle is not complete then a message will display saying the puzzle is not solved.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uxCheck_Click(object sender, EventArgs e)
        {
            if (_currentPuzzle.Conflict())
            {
                MessageBox.Show("There are duplicate values.");
            }
            else
            {
                if (_currentPuzzle.Complete())
                {
                    uxHint.Enabled = false;
                    uxSolve.Enabled = false;
                    DisableTextBoxes();
                    MessageBox.Show("Puzzle is solved!");

                }
                else
                {
                    MessageBox.Show("Puzzle is not solved.");
                }
            }
        }
        /// <summary>
        /// Triggered when the user clicks on the hint button. Only gives one hint per click.
        /// If the solution is null the program will find a solution using the MagicStack method. If the solution is still null it will display an error message saying that no solution exists.
        /// If the solution is not null then the will run through a for loop. The loop checks whether the textbox at that point is empty or not.
        /// If the textbox isnt empty then the entry remains and the textbox is diabled, but if the text box is empty it will reveal the correct number for the next text box going from right to left and top to bottom.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uxHint_Click(object sender, EventArgs e)
        {
            if (_solution == null)
            {
                _solution = MagicStack();
            }
            if (_solution != null)
            {
                for (int i = 0; i < Square.SIZE; i++)
                {
                    for (int j = 0; j < Square.SIZE; j++)
                    {
                        if (_textBoxGrid[i, j].Text == "")
                        {
                            _textBoxGrid[i, j].Enabled = false;
                            _textBoxGrid[i, j].Text = _solution[i, j].ToString();
                            _currentPuzzle[i, j] = _solution[i, j];
                            UpdateGUI(true);
                            j = Square.SIZE;
                            i = Square.SIZE;
                        }
                    }
                }
                UpdateGUI(false);
            }
            else
            {
                MessageBox.Show("No solution exists.");
            }
        }

        /// <summary>
        /// Triggered when the user clicks on the solve button.
        /// If the solution is null the program will find a solution using the MagicStack method. If the solution is still full then an messagebox will display that there is no solution for the entries they are trying to use.
        /// If the solution is not null, then the buttons with be diabled, the solution is displayed in the textboxes and all labels are updated.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uxSolve_Click(object sender, EventArgs e)
        {
            //_solution = MagicStack();
            if (_solution == null)
            {
                _solution = MagicStack();
            }
            if (_solution == null)
            {
                MessageBox.Show("No solution exists.");
            }
            else
            {
                PlacePuzzle();
                UpdateGUI(true);
                uxSolve.Enabled = false;
                uxHint.Enabled = false;
                uxCheck.Enabled = false;
            }
            Console.WriteLine(_solution.NextAvailable());
        }
        /// <summary>
        /// Resets the entire game so all textboxes are enabled and empty and all labels reset to 0. All button are then enabled as well.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UxNewGame_Click(object sender, EventArgs e)
        {
            _currentPuzzle = new Square();
            _solution = null;
            for (int i = 0; i < Square.SIZE; i++)
            {
                for (int j = 0; j < Square.SIZE; j++)
                {
                    _textBoxGrid[i, j].Text = "";
                    _labelRows[i].Text = "0";
                    _labelColumns[j].Text = "0";
                    _textBoxGrid[i, j].Enabled = true;
                }
            }
            uxDiagonalBottomTop.Text = "";
            uxDiagonalTopBottom.Text = "";
            uxCheck.Enabled = true;
            uxSolve.Enabled = true;
            uxHint.Enabled = true;
        }
        /// <summary>
        /// Finds all the possible solutions using the users entries.
        /// </summary>
        /// <returns>Null if there is no solution with the given entries, otherwise it returns a Square object with the solution for those entries.</returns>
        private Square MagicStack()
        {
            Stack<Square> stack = new Stack<Square>();
            stack.Push(_currentPuzzle);
            while (stack.Count > 0)
            {
                Square box = stack.Pop();
                if (box.Complete())
                {
                    return box;
                }
                else
                {
                    int val = box.NextAvailable();
                    Console.WriteLine(_currentPuzzle.NextAvailable());
                    if (val == -1)
                    {
                        continue;
                    }
                    for (int i = 0; i < Square.SIZE; i++)
                    {
                        for (int j = 0; j < Square.SIZE; j++)
                        {
                            Square temp = box.Duplicate();
                            if (temp.Move(i, j, val))
                            {
                                if (temp.Possible())
                                {
                                    stack.Push(temp);
                                }
                            }
                        }
                    }
                }
            }
            return null;
        }
        /// <summary>
        /// Inserts the solution Square for the puzzle into the the current textboxes
        /// </summary>
        private void PlacePuzzle()
        {
            for (int i = 0; i < Square.SIZE; i++)
            {
                for (int j = 0; j < Square.SIZE; j++)
                {
                    _textBoxGrid[i, j].Enabled = false;
                    _textBoxGrid[i, j].Text = _solution[i, j].ToString();
                    _currentPuzzle[i,j] = _solution[i,j];
                }
            }
        }
        /// <summary>
        /// Diables all of the textboxes
        /// </summary>
        private void DisableTextBoxes()
        {
            for (int i = 0; i < Square.SIZE; i++)
            {
                for (int j = 0; j < Square.SIZE; j++)
                {
                    _textBoxGrid[i, j].Enabled = false;
                }
            }
        }
    }
}