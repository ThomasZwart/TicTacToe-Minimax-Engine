using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe_Minimax
{
    class Board
    {
        public int[,] grid;
        private bool turn = false;

        public Board(int [,] _grid)
        {
            grid = _grid;
        }

        public int GameWonBy()
        {
            if (WinCheck(1)) {
                return 1;
            }
            else if (WinCheck(-1)) {
                return -1;
            }
            else {
                return 0;
            }
        }

        public bool BoardFull()
        {
            for (int i = 0; i < 3; i++) {
                for (int j = 0; j < 3; j++) {
                    if (grid[i,j] == 0) {
                        return false;
                    }
                }
            }
            return true;
        }

        // Who's turn it is, False = 1 = cross, True = -1 = circle
        public bool Turn {
            get {
                if (MovesMade % 2 == 0) {
                    return false;
                }
                else {
                    return true;
                }
            }
            set {
                turn = value;
            }
        }
        private bool WinCheck(int i)
        {
            if ( // Horizontals
                (grid[0,0] == i && grid[1,0] == i && grid[2,0] == i)
                || (grid[0, 1] == i && grid[1, 1] == i && grid[2, 1] == i)
                || (grid[0, 2] == i && grid[1, 2] == i && grid[2, 2] == i)
                // Verticals
                || (grid[0, 0] == i && grid[0, 1] == i && grid[0, 2] == i)
                || (grid[1, 0] == i && grid[1, 1] == i && grid[1, 2] == i)
                || (grid[2, 0] == i && grid[2, 1] == i && grid[2, 2] == i)
                // Diagonals
                || (grid[0, 0] == i && grid[1, 1] == i && grid[2, 2] == i)
                || (grid[2, 0] == i && grid[1, 1] == i && grid[0, 2] == i)) {
                return true;
            }
            else {
                return false;
            }   
        }

        // Get the amount of moves that have been made
        public int MovesMade {
            get {
                int count = 0;
                for (int i = 0; i < 3; i++) {
                    for (int j = 0; j < 3; j++) {
                        if (grid[i,j] != 0) {
                            count++;
                        }
                    }
                }
                return count;
            }
        }

        public List<Tuple<int, int>> GetEmptyBoxes()
        {
            List<Tuple<int, int>> list = new List<Tuple<int, int>>();
            for (int i = 0; i < 3; i++) {
                for (int j = 0; j < 3; j++) {
                    if (grid[i,j] == 0) {
                        list.Add(new Tuple<int, int>(i, j));
                    }
                }
            }
            return list;
        }


        public int this[int index1, int index2] // Used to access the grid variable easily
        {
            get {
                try { return grid[index1, index2]; }
                catch {
                    Console.WriteLine("Invalid Input");
                    return 0;
                }
            }

            set { try {
                    grid[index1, index2] = value;
                }
                catch {
                    Console.WriteLine("Invalid Input");
                }
            }
        }

        // Return a new Board with that move made
        public Board NewBoard(Tuple<int, int> moveMade)
        {
            Board newBoard = new Board(grid.Clone() as int[,]);
            if (!Turn) {
                newBoard[moveMade.Item1, moveMade.Item2] = 1;
            }
            else {
                newBoard[moveMade.Item1, moveMade.Item2] = -1;
            }
            
            return newBoard;
        }

        public void WriteBoard()
        {
            for (int i = 0; i < 3;  i++) {
                for (int j = 0; j < 3; j++) {
                    if (grid[i,j] == 1) {
                        Console.Write("X ");
                    }
                    else if (grid[i,j] == -1) {
                        Console.Write("O ");
                    }
                    else {
                        Console.Write("- ");
                    }
                    
                }
                Console.Write("\n");
            }
        }

        public bool Equals(Board board)
        {
            for (int i = 0; i < 3; i++) {
                for (int j = 0; j < 3; j++) {
                    if (board.grid[i, j] != grid[i, j]) {
                        return false;
                    }
                }
            }
            return true;
        }
    }
}
