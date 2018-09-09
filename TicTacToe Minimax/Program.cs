using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe_Minimax
{
    class Program
    {
        static List<HashSet<Node>> Layers;

        static void Main()
        {
            // Initialize hashsets
            Layers = new List<HashSet<Node>>();
            for (int i = 0; i < 10; i++) {
                Layers.Add(new HashSet<Node>());
            }

            // Trains a search tree with minimax values
            Minimax(StartBoard());

            // UI (sort of)
            Board board = StartBoard();
            Console.WriteLine("Select the square you want to play by entering the coördinates, \neg: '2 2' for the middle square");
            Console.WriteLine("Would you like to start?");
            Console.WriteLine("y: Yes");
            Console.WriteLine("n: No");
            if (Console.ReadLine() == "n") {
                board[1, 1] = 1;
            }
            board.WriteBoard();

            // Play game
            while (true) {
                int[] input;
                
                try {
                    input = Array.ConvertAll(Console.ReadLine().Split(), c => Convert.ToInt32(c));

                    // If the square is already filled
                    if (board[input[1] - 1, input[0] - 1] != 0) {
                        Console.WriteLine("This square is already filled, try again");
                        continue;
                    }
                    // Depending on who's turn it is a cross or circle, resp 1 or -1 gets played
                    if (!board.Turn) {
                        board[input[1] - 1, input[0] - 1] = 1;
                    }
                    else {
                        board[input[1] -1, input[0] -1] = -1;
                    }
                    // Player can see what he played
                    board.WriteBoard();
                    Console.WriteLine();
                }
                catch {
                    Console.WriteLine("Invalid Input, try Again");
                    continue;
                }
                board = ComputerTurn(board);
                // If board is full, determine winner, else computer playes a move
                if (board.BoardFull() || board.GameWonBy() != 0) {
                    if (board.GameWonBy() == 1) {
                        Console.WriteLine("X won!");
                        break;
                    }
                    else if (board.GameWonBy() == -1) {
                        Console.WriteLine("O won!");
                        break;
                    }
                    else {
                        Console.WriteLine("Draw!");
                        break;
                    }
                }                    
            }
            Console.ReadLine();
        }

        static Board ComputerTurn(Board board)
        {
            // Find the layer corresponding to amount of moves made, and find the board that is now currenty playing
            // Than based on who's turn it is -1 or 1, decide whether you want the child with the minimum score or maximum (minimax algorithm)
            foreach (Node node in Layers[board.MovesMade]) {
                if (node.Board.Equals(board)) {
                    if (!node.Board.Turn) {
                        int x = node.Children.Max(c => c.MinimaxScore);
                        IEnumerable<Node> n = node.Children.Where(c => c.MinimaxScore == x);
                        n.First().Board.WriteBoard();
                        Console.WriteLine();
                        return n.First().Board;
                    }
                    else {
                        int x = node.Children.Min(c => c.MinimaxScore);
                        IEnumerable<Node> n = node.Children.Where(c => c.MinimaxScore == x);
                        n.First().Board.WriteBoard();
                        Console.WriteLine();
                        return n.First().Board;
                    }
                }
            }
            return board; 
        }

        // Readboard of the form 
        // "0 1 0
        //  0 0 0
        //  -1 0 0"
        static Board ReadBoard()
        {
            int[,] x = new int[3, 3];

            for (int i = 0; i < 3; i++) {
                int[] s = Array.ConvertAll(Console.ReadLine().Trim().Split(), c => Convert.ToInt32(c));
                for (int j = 0; j < 3; j++) {
                    x[i, j] = s[j];
                }
            }
            return new Board(x);
        }

        // Initialize a empty board
        static private Board StartBoard()
        {
            int[,] x = new int[3, 3];
            for (int i = 0; i < 3; i++) {
                for (int j = 0; j < 3; j++) {
                    x[i, j] = 0;
                }
            }

            Board board = new Board(x);
            return board;
        }

        // Minimax algorithm
        static public Tuple<int, int> Minimax(Board board)
        {
            Queue<Node> queue = new Queue<Node>();

            // Start node
            Node startNode = new Node() {
                Board = board,
                MinimaxScore = 0,
                Parent = null,
                Children = new List<Node>()
            };

            Layers[startNode.Board.MovesMade].Add(startNode);

            queue.Enqueue(startNode);

            // Go through the entire tree (this is not optimal, but because tic tac toe is such a small problem there is no problem with this approach)
            while(queue.Count != 0) {
                Operations(queue, queue.Dequeue());
            }

            // Giving minimax scores to every node         
            for (int i = 8 - startNode.Board.MovesMade; i >= 0; i--) {
                foreach(Node node in Layers[i]) {
                    if (!node.Board.Turn) {
                        node.MinimaxScore = node.Children.Max(c => c.MinimaxScore);
                    }
                    else {
                        node.MinimaxScore = node.Children.Min(c => c.MinimaxScore);
                    }               
                }
            }

            return null;
        }

        // Every empty square in the tictactoe grid gets expanded
        static public void Operations(Queue<Node> queue, Node node)
        {
            foreach (Tuple<int, int> tuple in node.Board.GetEmptyBoxes()) {
                Node newNode = new Node() {
                    Board = node.Board.NewBoard(tuple),
                    MinimaxScore = node.MinimaxScore,
                    Parent = node,
                    Children = new List<Node>()
                };

                node.Children.Add(newNode);

                // If the game is won by a player or a draw, assign minimax score, else add the node to the queue and the correesponding layer
                if (newNode.Board.GameWonBy() != 0 || newNode.Board.BoardFull()) {
                    newNode.MinimaxScore = newNode.Board.GameWonBy();
                }
                else {
                    Layers[newNode.Board.MovesMade].Add(newNode);
                    queue.Enqueue(newNode);
                }
                                   
            }
        }
    }
}
