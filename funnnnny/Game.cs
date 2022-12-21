using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace funnnnny
{
    public class Game
    {
        // constants
        private const int BOARD_SIZE = 8;
        private const int PLAYER1 = 1;
        private const int PLAYER2 = 2;
        private const int DEFAULT_DEPTH = 15;

        public Board board;
        public AI computer;

        public Game()
        {
            board = new Board(BOARD_SIZE, PLAYER1, PLAYER2);
            computer = new AI(board, BOARD_SIZE, DEFAULT_DEPTH);
        }

        public int[,] ComputerTurn()
        {
            computer.setBoard(board.getBoard());
            return computer.AfterPlayerMoves().getBoard();
        }

        public int getCalculations()
        {
            return computer.getCalculations();
        }
        public long getTime()
        {
            return computer.getTime();
        }
    }
}
