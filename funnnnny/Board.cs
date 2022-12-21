using System;
using System.Collections.Generic;
using System.Text;

namespace funnnnny
{
    public class Board
    {
        private int[,] board;
        private int BOARD_SIZE;
        private int PLAYER1;
        private int PLAYER2;
        public Board(int boardSize, int playerOne, int playerTwo)
        {
            board = new int[8, 8]
        {
            {0, 1, 0, 1, 0, 1, 0, 1},
            {1, 0, 1, 0, 1, 0, 1, 0},
            {0, 1, 0, 1, 0, 1, 0, 1},
            {0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0},
            {2, 0, 2, 0, 2, 0, 2, 0},
            {0, 2, 0, 2, 0, 2, 0, 2},
            {2, 0, 2, 0, 2, 0, 2, 0},
        };
            BOARD_SIZE = boardSize;
            PLAYER1 = playerOne;
            PLAYER2 = playerTwo;
        }

        public int[,] getBoard()
        {
            return board;
        }
        public void setBoard(int[,] board)
        {
            this.board = (int[,])board.Clone();
        }
        public int getSquare(int x, int y)
        {
            return this.board[x, y];
        }
        public void setSquare(int x, int y, int piece)
        {
            this.board[x, y] = piece;
        }
        public List<(int, int, int, int)> GetMoves(int player, int x, int y)
        {
            var moves = GetValidMoves(player);
            var movesNew = moves;
            foreach (var move in GetValidMoves(player))
            {
                if (move.Item1 != x || move.Item2 != y)
                {
                    movesNew.Remove(move);
                }
            }
            return movesNew;
        }
        public void MakeMove(int player, int x1, int y1, int x2, int y2)
        {
            // check if the move is valid
            if (!IsValidMove(x1, y1, x2, y2))
                return;

            // move the piece to the new position
            board[x2, y2] = board[x1, y1];
            board[x1, y1] = 0;

            // check if the piece should be promoted to a king
            if (player == PLAYER1 && x2 == BOARD_SIZE - 1)
                board[x2, y2] = PLAYER1 + 2;
            if (player == PLAYER2 && x2 == 0)
                board[x2, y2] = PLAYER2 + 2;

            // if the move is a capture, remove the captured piece
            int dx = (x2 - x1) / Math.Abs(x2 - x1);
            int dy = (y2 - y1) / Math.Abs(y2 - y1);
            if (Math.Abs(x2 - x1) > 1)
            {
                int x3 = x1 + dx;
                int y3 = y1 + dy;
                board[x3, y3] = 0;
            }
        }
        public List<(int, int, int, int)> GetValidMoves(int player)
        {
            var moves = new List<(int, int, int, int)>();

            // check all possible capture moves
            moves.AddRange(GetCaptureMoves(player));

            // if there are no capture moves, add all valid non-capture moves to the list
            if (moves.Count == 0)
            {
                moves.AddRange(GetNonCaptureMoves(player));
            }

            return moves;
        }
        private bool IsCaptureMove(int player, int x1, int y1, int x2, int y2)
        {
            // check if the move is a capture move
            int dx = x2 - x1;
            int dy = y2 - y1;
            int absDx = Math.Abs(dx);
            if (absDx > 1)
            {
                int x3 = (x1 + x2) / 2;
                int y3 = (y1 + y2) / 2;
                if (board[x3, y3] != player && board[x3, y3] != player + 2)
                {
                    return true;
                }
            }

            return false;
        }
        // check if a move is valid
        public bool IsValidMove(int x1, int y1, int x2, int y2)
        {
            int player = board[x1, y1];
            // check if the start and end positions are on the board
            if (x1 < 0 || x1 >= BOARD_SIZE || y1 < 0 || y1 >= BOARD_SIZE ||
                x2 < 0 || x2 >= BOARD_SIZE || y2 < 0 || y2 >= BOARD_SIZE)
                return false;

            // check if the start position has the player's piece
            //if (board[x1, y1] != player && board[x1, y1] != player + 2)
            //    return false;

            // check if the end position is empty
            if (board[x2, y2] != 0)
                return false;

            // check if the move is along a diagonal
            if (Math.Abs(x2 - x1) != Math.Abs(y2 - y1) || Math.Abs(y2 - y1) > 2)
                return false;

            // if player is not a king, check if the move is in the correct direction
            if (player == PLAYER1 && x2 < x1)
                return false;
            if (player == PLAYER2 && x2 > x1)
                return false;

            // check if there is a piece to capture
            int dx = (x2 - x1) / Math.Abs(x2 - x1);
            int dy = (y2 - y1) / Math.Abs(y2 - y1);
            if (Math.Abs(x2 - x1) > 1)
            {
                int x3 = x1 + dx;
                int y3 = y1 + dy;
                if (board[x3, y3] == 0 || board[x3, y3] == player || board[x3, y3] == player + 2)
                    return false;
            }

            return true;
        }
        private List<(int, int, int, int)> GetNonCaptureMoves(int player)
        {
            var nonCaptureMoves = new List<(int, int, int, int)>();
            for (int x1 = 0; x1 < BOARD_SIZE; x1++)
            {
                for (int y1 = 0; y1 < BOARD_SIZE; y1++)
                {
                    if (board[x1, y1] == player || board[x1, y1] == player + 2)
                    {
                        // check moves in all directions for kings
                        if (board[x1, y1] == player + 2)
                        {
                            for (int x2 = x1 - 1; x2 <= x1 + 1; x2++)
                            {
                                for (int y2 = y1 - 1; y2 <= y1 + 1; y2++)
                                {
                                    if (IsValidMove(x1, y1, x2, y2) && !IsCaptureMove(player, x1, y1, x2, y2))
                                    {
                                        nonCaptureMoves.Add((x1, y1, x2, y2));
                                    }
                                }
                            }
                        }
                        // check moves in diagonal directions for regular pieces
                        else
                        {
                            for (int x2 = x1 - 1; x2 <= x1 + 1; x2++)
                            {
                                for (int y2 = y1 - 1; y2 <= y1 + 1; y2++)
                                {
                                    if (x2 != x1 && y2 != y1)
                                    {
                                        if (IsValidMove(x1, y1, x2, y2) && !IsCaptureMove(player, x1, y1, x2, y2))
                                        {
                                            nonCaptureMoves.Add((x1, y1, x2, y2));
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return nonCaptureMoves;
        }
        private List<(int, int, int, int)> GetCaptureMoves(int player)
        {
            var captureMoves = new List<(int, int, int, int)>();
            for (int x1 = 0; x1 < BOARD_SIZE; x1++)
            {
                for (int y1 = 0; y1 < BOARD_SIZE; y1++)
                {
                    if (board[x1, y1] == player || board[x1, y1] == player + 2)
                    {
                        // check moves in all directions for kings
                        if (board[x1, y1] == player + 2)
                        {
                            for (int x2 = x1 - 2; x2 <= x1 + 2; x2++)
                            {
                                for (int y2 = y1 - 2; y2 <= y1 + 2; y2++)
                                {
                                    if (IsValidMove(x1, y1, x2, y2) && IsCaptureMove(player, x1, y1, x2, y2))
                                    {
                                        captureMoves.Add((x1, y1, x2, y2));
                                    }
                                }
                            }
                        }
                        // check moves in diagonal directions for regular pieces
                        else
                        {
                            for (int x2 = x1 - 2; x2 <= x1 + 2; x2 += 4)
                            {
                                for (int y2 = y1 - 2; y2 <= y1 + 2; y2 += 4)
                                {
                                    if (IsValidMove(x1, y1, x2, y2) && IsCaptureMove(player, x1, y1, x2, y2))
                                    {
                                        captureMoves.Add((x1, y1, x2, y2));
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return captureMoves;
        }

    }
}
