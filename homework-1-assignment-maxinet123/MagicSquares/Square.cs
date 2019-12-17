/* 
 * Square.cs
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
public class Square
{
    public const int SIZE = 4;
    public const int MAGIC = 34;
    public const int EMPTY = 0;

    private int[,] _grid = new int[SIZE, SIZE];
    bool[] _used = new bool[SIZE*SIZE+1];
    
    /// <summary>
    /// This is the indexer for the program. It allows the program to get and set values for the grid field.
    /// </summary>
    /// <param name="i"></param>
    /// <param name="j"></param>
    /// <returns></returns>
    public int this[int i, int j]
    {
        get => _grid[i,j];
        set
        {
            _grid[i, j] = value;
            _used[value] = true;
            if (value == 0)
            {
                _used = new bool[SIZE * SIZE + 1];
                for (int row = 0; row < SIZE; row++)
                {
                    for (int col = 0; col < SIZE; col++)
                    {
                        _used[_grid[row, col]] = true; 
                    }
                }
            }
        }
    }
    /// <summary>
    /// Creates a new square object that goes though each place in the grid and used arrays and transferrs it into the NewSquare.
    /// </summary>
    /// <returns>The new Square object containing all the same values as the original but with a different memory location for the arrays</returns>
	public Square Duplicate()
	{
        Square newSquare = new Square();
        for (int i = 0; i < SIZE; i++)
        {
            for (int j = 0; j < SIZE; j++)
            {
                newSquare._grid[i, j] = _grid[i, j];
            }
        }
        for (int i = 0; i < SIZE * SIZE + 1; i++)
        {
            newSquare._used[i] = _used[i];
        }
        return newSquare;
	}
    /// <summary>
    /// Calls the several methods to make sure they all return true and unture statments will return false right away. Also makes sure nothing int eh grid is empty.
    /// </summary>
    /// <returns>True if the Magic square is a solution</returns>
    public bool Complete()
    {
        if (!CheckDiagonal(true))
        {
            return false;
        }
        for (int i = 0; i < SIZE; i++)
        {
            if (!CheckRow(i, true) || !CheckColumn(i, true))
            {
                return false;
            }
        }
        for (int i = 0; i < SIZE; i++)
        {
            for (int j = 0; j < SIZE; j++)
            {
                if (_grid[i,j] == EMPTY)
                {
                    return false;
                }
            }
        }
        return true;
    }
    /// <summary>
    /// Runs through the loops to make sure the number is not in the used array. If it is not in the arry then that number will be returned.
    /// </summary>
    /// <returns>-1 if no answer is found for the loop</returns>
    public int NextAvailable()
    {
        for (int i = 0; i < SIZE * SIZE + 1; i++){
            if (!_used[i])
            {
                return i;
            }
        }
        return -1;
    }
    /// <summary>
    /// Checks if the textbox is empty or if the value as already been used. If yes to either condition then it will return false immediately.
    /// Otherwise, val will be placed at the location of r and c in the grid array and make the used array at the value true.
    /// </summary>
    /// <param name="r">number of the row</param>
    /// <param name="c">number of the column</param>
    /// <param name="val">number in the textbox</param>
    /// <returns>returns true if it makes it to the end of the method</returns>
    public bool Move(int r, int c, int val)
    {
        if (_grid[r,c] != EMPTY || _used[val])
        {
            return false;
        }
        _grid[r, c] = val;
        _used[val] = true;
        return true;
    }
    /// <summary>
    /// Checks to see if the grid is full or empty. If it is full or the parameter is true then it checks to see if the sum doesnt equal the MAGIC constant
    /// If neither are true then it will check if the sum is greater than MAGIC
    /// </summary>
    /// <param name="b"> true or false is passed into the method</param>
    /// <returns>true if all tests are passed</returns>
    private bool CheckDiagonal(bool b)
    {
        int sum = 0;
        bool full = true;
        for (int i = 0; i < SIZE; i++)
        {
            sum = SumTopBottom();
            if (_grid[i, i] == EMPTY)
            {
                full = false;
            }
        }
        if (full || b)
        {
            if (sum != MAGIC)
            {
                return false;
            }
        }
        else
        {
            if (sum > MAGIC)
            {
                return false;
            }
        }
        sum = 0;
        for (int i = 0; i < SIZE; i++)
        {
            sum = SumBottomTop();
            if (_grid[i, SIZE - 1 - i] == EMPTY)

            {
                full = false;
            }
        }
        if (full || b)
        {
            if (sum != MAGIC)
            {
                return false;
            }
        }
        else
        {
            if (sum > MAGIC)
            {
                return false;
            }
        }
        return true;
    }
    /// <summary>
    /// Checks to see if the grid is full or empty. If it is full or the parameter is true then it checks to see if the sum doesnt equal the MAGIC constant
    /// If neither are true then it will check if the sum is greater than MAGIC
    /// </summary>
    /// <param name="r">integer for the row</param>
    /// <param name="b"> true or false is passed into the method</param>
    /// <returns>true if all tests are passed</returns>
    private bool CheckRow(int r, bool b)
    {
        int sum = 0;
        bool full = true;
        for (int col = 0; col < SIZE; col++)
        {
            sum = SumRow(r);
            if (_grid[r, col] == EMPTY)
            {
                full = false;
            }
        }
        if (full || b)
        {
            if (sum != MAGIC)
            {
                return false;
            }
        }
        else
        {
            if (sum > MAGIC)
            {
                return false;
            }
        }
        return true;
    }
    /// <summary>
    /// Checks to see if the grid is full or empty. If it is full or the parameter is true then it checks to see if the sum doesnt equal the MAGIC constant
    /// If neither are true then it will check if the sum is greater than MAGIC
    /// </summary>
    /// <param name="c">integer for the column</param>
    /// <param name="b"> true or false is passed into the method</param>
    /// <returns>true if all tests are passed</returns>
    private bool CheckColumn(int c, bool b)
    {
        int sum = 0;
        bool full = true;
        for (int row = 0; row < SIZE; row++)
        {
            sum = SumColumn(c);
            if (_grid[row, c] == EMPTY)
            {
                full = false;
            }
        }
        if (full || b)
        {
            if (sum != MAGIC)
            {
                return false;
            }
        }
        else
        {
            if (sum > MAGIC)
            {
                return false;
            }
        }
        return true;
    }
    /// <summary>
    /// Check the row where it is referenced at to see if any textboxes are empty. If exactly one is empty then it will return true.
    /// </summary>
    /// <param name="r"></param>
    /// <returns>false</returns>
    private bool RowOneLeft(int r)
    {
        int counter = 0;
        for (int col = 0; col < SIZE; col++)
        {
            if (_grid[r,col] == EMPTY)
            {
                counter++;
            }
        }
        if (counter == 1)
        {
            return true;
        }
        return false;
    }
    /// <summary>
    /// Check the column where it is referenced at to see if any textboxes are empty. If exactly one is empty then it will return true.
    /// </summary>
    /// <param name="c"></param>
    /// <returns>false</returns>
    private bool ColumnOneLeft(int c)
    {
        int counter = 0;
        for (int row = 0; row < SIZE; row++)
        {
            if (_grid[row, c] == EMPTY)
            {
                counter++;
            }
        }
        if (counter == 1)
        {
            return true;
        }
        return false;
    }
    /// <summary>
    /// Check the top to bottom diagonal where it is referenced at to see if any textboxes are empty. If exactly one is empty then it will return true.
    /// </summary>
    /// <returns>false</returns>
    private bool TopBottomDiagonalOneLeft()
    {
        int counter = 0;
        for (int i = 0; i < SIZE; i++)
        {
            if (_grid[i, i] == EMPTY)
            {
                counter++;
            }
        }
        if (counter == 1)
        {
            return true;
        }
        return false;
    }
    /// <summary>
    /// Check the bottom to top diagonal where it is referenced at to see if any textboxes are empty. If exactly one is empty then it will return true.
    /// </summary>
    /// <returns>false</returns>
    private bool BottomTopDiagonalOneLeft()
    {
        int counter = 0;
        for (int i = 0; i < SIZE; i++)
        {
            if (_grid[i, SIZE - 1 - i] == EMPTY)
            {
                counter++;
            }
        }
        if (counter == 1)
        {
            return true;
        }
        return false;
    }
    /// <summary>
    /// Finds the next biggest integer not used yet. Will return that value if it hasnt been used.
    /// </summary>
    /// <returns>-1 if there is no other values left/returns>
    private int NextBig()
    {
        for (int i = _used.Length - 1; i >= 1; i--)
        {
            if (!_used[i])
            {
                return i;
            }
        }
        return -1;
    }
    /// <summary>
    /// Uses the One left methods to see if the return is true and to seem if the sum plus the next biggest number is still less than magic.
    /// If both cases are true then it returns false. Since if the next biggest number is plugged into the last square of that section and it is still less than MAGIC then it will return false;
    /// otherwise if it passes all the cases then it will be true
    /// </summary>
    /// <returns>true</returns>
    private bool CheckOneLeft()
    {
        int biggest = NextBig();
        if (TopBottomDiagonalOneLeft() && (SumTopBottom() + biggest < MAGIC))
        {
            return false;
        }
        if (BottomTopDiagonalOneLeft() && (SumBottomTop() + biggest < MAGIC))
        {
            return false;
        }
        for (int i = 0; i < SIZE; i++)
        {
            if (RowOneLeft(i) && (SumRow(i) + biggest < MAGIC))
            {
                return false;
            }
            if (ColumnOneLeft(i) && (SumColumn(i) + biggest < MAGIC))
            {
                return false;
            }
        }
        return true;
    }
    /// <summary>
    /// Checks rows, columns and diagonals methods to make sure it will all return true. If any return false then the solution is not possible.
    /// </summary>
    /// <returns>trueif the CheckOneLeft method also returns true</returns>
    public bool Possible()
    {
        for (int i = 0; i < SIZE; i++)
        {
            if (!CheckRow(i, false))
            {
                return false;
            }
        }
        for (int j = 0; j < SIZE; j++)
        {
            if (!CheckColumn(j, false))
            {
                return false;
            }
        }
        if (!CheckDiagonal(false))
        {
            return false;
        }
        return CheckOneLeft();
    }
    /// <summary>
    /// Checks to see if there are any duplicate values in the grid
    /// </summary>
    /// <returns>false for duplicate values</returns>
    public bool Conflict()
    {
        int[] counter = new int[SIZE*SIZE+1];
        for (int i = 0; i < SIZE; i++)
        {
            for (int j = 0; j <SIZE; j++)
            {
                counter[_grid[i, j]]++;
                if (counter[_grid[i,j]] > 1 && _grid[i,j] > 0)
                {
                    return true;
                }
            }
        }
        return false;
    }
    /// <summary>
    /// Sums all the textboxes entries of row i
    /// </summary>
    /// <param name="i">row that is being checked</param>
    /// <returns>sum off all textbox entries</returns>
    public int SumRow(int i)
    {
        int sum = 0;
        for (int row = 0; row < SIZE; row++)
        {
            sum += _grid[i, row];
        }
        return sum;
    }
    /// <summary>
    /// Sums all the textboxes entries of column j
    /// </summary>
    /// <param name="j">column that is being checked</param>
    /// <returns>sum of all textbox entries</returns>
    public int SumColumn(int j)
    {
        int sum = 0;
        for (int col = 0; col < SIZE; col++)
        {
            sum += _grid[col, j];
        }
        return sum;
    }
    /// <summary>
    /// finds the sum of the text boxes that are diagonally top to bottom to top from  left to right
    /// </summary>
    /// <returns>sum of numbers in textboxes</returns>
    public int SumTopBottom()
    {
            int sum = 0;
            for (int i = 0; i < SIZE; i++)
            {
                sum += _grid[i, i];
            }
            return sum;
    }
    /// <summary>
    /// finds the sum of the text boxes that are diagonally bottom to top from  left to right
    /// </summary>
    /// <returns>sum of numbers in textboxes</returns>
    public int SumBottomTop()
    {
        int sum = 0;
        for (int i = 0; i < SIZE; i++)
        {
            sum += _grid[SIZE - 1 - i, i];
        }
        return sum;
    }
}
