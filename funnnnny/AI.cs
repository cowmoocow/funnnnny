using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace funnnnny
{
    public class AI
    {
        // board layout
        private Board board;
        private Board boardCopy;
        private int BOARD_SIZE;
        private int DEFAULT_DEPTH;
        private int calculations { get; set; }
        private Stopwatch timer = new Stopwatch();
        public long time { get; set; }
        int moveCount = 0;
        public AI(Board board, int boardSize, int depth)
        {
            this.board = new Board(boardSize, 1, 2);
            this.board.setBoard(board.getBoard());
            this.boardCopy = new Board(boardSize, 1, 2);
            this.boardCopy.setBoard(board.getBoard());
            BOARD_SIZE = boardSize;
            DEFAULT_DEPTH = depth;
        }

        public int getCalculations()
        {
            return calculations;
        }

        public long getTime()
        {
            return time;
        }
        public void setBoard(int[,] board)
        {
            this.board.setBoard(board);
        }
        public void setSquare(int piece, int x, int y)
        {
            this.board.setSquare(x, y, piece);
        }

        // get the value of a board position for a player
        public int EvaluateBoard(int[,] board)
        {
            int player = 2;
            int value = 0;

            // add or subtract the number of pieces for the player
            for (int x = 0; x < BOARD_SIZE; x++)
            {
                for (int y = 0; y < BOARD_SIZE; y++)
                {
                    if (board[x, y] == player)
                        value++;
                    if (board[x, y] == player + 2)
                        value += 3;
                    if (board[x, y] == 3 - player)
                        value--;
                    if (board[x, y] == 3 - player + 2)
                        value -= 3;
                }
            }
            calculations++;
            return value;
        }


        //public int Minimax(int player, int depth, int alpha, int beta, bool maximizingPlayer)
        //{
        //    // check if the maximum depth has been reached or if the game is over
        //    if (depth == 0 || !boardCopy.GetValidMoves(player).Any())
        //    {
        //        // evaluate the board from the perspective of the current player
        //        int value = EvaluateBoard();
        //        boardCopy.setBoard(board.getBoard());
        //        return value;
        //    }

        //    if (maximizingPlayer)
        //    {
        //        // try all possible moves for the current player
        //        foreach (var move in boardCopy.GetValidMoves(player))
        //        {
        //            // make the move on a copy of the board

        //            boardCopy.MakeMove(player, move.Item1, move.Item2, move.Item3, move.Item4);

        //            // get the minimax value of the next player
        //            int value = Minimax(3 - player, depth - 1, alpha, beta, false);

        //            // update the alpha value
        //            alpha = Math.Max(alpha, value);

        //            // check if beta <= alpha
        //            if (beta <= alpha)
        //                break;
        //        }

        //        return alpha;
        //    }
        //    else
        //    {
        //        // try all possible moves for the current player
        //        foreach (var move in boardCopy.GetValidMoves(player))
        //        {
        //            // make the move on a copy of the board

        //            boardCopy.MakeMove(player, move.Item1, move.Item2, move.Item3, move.Item4);

        //            // get the minimax value of the next player
        //            int value = Minimax(3 - player, depth - 1, alpha, beta, true);

        //            // update the beta value
        //            beta = Math.Min(beta, value);

        //            // check if beta <= alpha
        //            if (beta <= alpha)
        //                break;
        //        }

        //        return beta;
        //    }
        //}

        public int MinimaxMax(int player, int depth, int alpha, int beta)
        {
            // check if the maximum depth has been reached or if the game is over
            if (depth == 0 || !boardCopy.GetValidMoves(player).Any())
            {
                // evaluate the board from the perspective of the current player
                return EvaluateBoard(boardCopy.getBoard());
            }

            // try all possible moves for the current player
            foreach (var move in boardCopy.GetValidMoves(player))
            {
                moveCount++;
                // make the move on a copy of the board
                boardCopy.MakeMove(player, move.Item1, move.Item2, move.Item3, move.Item4);

                // get the minimax value of the next player
                int value = MinimaxMin(3 - player, depth - 1, alpha, beta);
                boardCopy.setSquare(move.Item3, move.Item4, boardCopy.getSquare(move.Item1, move.Item2));
                boardCopy.setSquare(move.Item3, move.Item4, 0);

                // update the alpha value
                if (value > alpha)
                {
                    alpha = value;
                }

                // check if beta <= alpha
                if (beta <= alpha)
                    break;
            }

            return alpha;
        }

        public int MinimaxMin(int player, int depth, int alpha, int beta)
        {
            // check if the maximum depth has been reached or if the game is over
            if (depth == 0 || !boardCopy.GetValidMoves(player).Any())
            {
                // evaluate the board from the perspective of the current player
                int value = EvaluateBoard(boardCopy.getBoard());
                return value;
            }

            // try all possible moves for the current player
            foreach (var move in boardCopy.GetValidMoves(player))
            {
                moveCount++;
                // make the move on a copy of the board
                boardCopy.MakeMove(player, move.Item1, move.Item2, move.Item3, move.Item4);

                // get the minimax value of the next player
                int value = MinimaxMax(3 - player, depth - 1, alpha, beta);

                boardCopy.setSquare(move.Item3, move.Item4, boardCopy.getSquare(move.Item1, move.Item2));
                boardCopy.setSquare(move.Item3, move.Item4, 0);

                // update the beta value
                if (value < beta)
                {
                    beta = value;
                }

                // check if beta <= alpha
                if (beta <= alpha)
                    break;
            }

            return beta;
        }




        public Board AfterPlayerMoves()
        {
            if (!board.GetValidMoves(2).Any())
            {
                //End the game
            }
            calculations = 0;
            // get the best move for the AI
            timer.Start();
        //    int[,] testBoard = new int[8, 8]
        //{
        //    {0, 0, 0, 0, 0, 0, 0, 0},
        //    {0, 0, 0, 0, 0, 0, 0, 0},
        //    {0, 0, 0, 1, 0, 0, 0, 0},
        //    {0, 0, 0, 0, 0, 0, 0, 0},
        //    {0, 0, 0, 0, 0, 0, 0, 0},
        //    {0, 0, 2, 0, 0, 0, 0, 0},
        //    {0, 0, 0, 0, 0, 0, 2, 0},
        //    {0, 0, 2, 0, 0, 2, 0, 0},
        //};
        //    board.setBoard(testBoard);
            boardCopy.setBoard(board.getBoard());
            moveCount = 0;
            var move = GetBestMove(2, DEFAULT_DEPTH);
            time = timer.ElapsedMilliseconds;
            timer.Stop();
            timer.Reset();
            // make the move on the board
            board.MakeMove(2, move.Item1, move.Item2, move.Item3, move.Item4);
            return board;
        }
        // get the best move for a player using minimax with alpha-beta pruning
        public (int, int, int, int) GetBestMove(int player, int depth)
        {
            int bestValue = int.MinValue;
            (int, int, int, int) bestMove = (-1, -1, -1, -1);

            // try all possible moves for the player
            foreach (var move in board.GetValidMoves(player))
            {
                // make the move on a copy of the board
                boardCopy.setBoard(board.getBoard());

                boardCopy.MakeMove(player, move.Item1, move.Item2, move.Item3, move.Item4);

                // get the minimax value of the next player
                int value = MinimaxMax(3 - player, depth - 1, int.MinValue, int.MaxValue);

                // update the best move
                if (value > bestValue)
                {
                    bestValue = value;
                    bestMove = move;
                }
            }

            
            return bestMove;
        }
    }
}
