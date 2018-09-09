using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe_Minimax
{
    class Node
    {
        public Board Board { get; set; }
        public int MinimaxScore { get; set; }
        public Node Parent { get; set; }
        public List<Node> Children { get; set; }
    }
}
