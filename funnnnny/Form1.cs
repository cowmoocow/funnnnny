using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace funnnnny
{
    public partial class Form1 : Form
    {
        private Game game = new Game();
        private int click = 0;
        private int[] movesToSend = new int[4];
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            DrawBoard();
        }

        private void DrawBoard()
        {
            Controls.Clear();
            Button calculations = new Button();
            calculations.Location = new Point(400, 0);
            calculations.Size = new Size(200, 30);
            calculations.Text = game.getCalculations().ToString() + " " + game.computer.time;
            calculations.Tag = new int[] { -1, -1 };
            Controls.Add(calculations);
            for (int y = 0; y < 8; y++)
            {
                for (int x = 0; x < 8; x++)
                {
                    Button button = new Button();
                    button.Size = new Size(50, 50);
                    button.Location = new Point(350 - (x * 50),350 - (y * 50));
                    button.Tag = new int[] { y, x };
                    button.Click += new EventHandler(Button_MouseDown);
                    if (game.board.getSquare(y, x) == 1)
                    {
                        button.BackColor = Color.White;
                    }
                    else if (game.board.getSquare(y, x) == 2)
                    {
                        button.BackColor = Color.Black;
                    }
                    else if (game.board.getSquare(y, x) == 3)
                    {
                        button.BackColor = Color.LightGray;
                    }
                    else if (game.board.getSquare(y, x) == 4)
                    {
                        button.BackColor = Color.DarkGray;
                    }
                    else
                    {
                        button.BackColor = Color.Gray;
                    }
                    button.Visible = true;
                    Controls.Add(button);
                }
            }
        }
        private void Button_MouseDown(object sender, EventArgs e)
        {
            if (click == 0)
            {
                for (int i = 0; i < 2; i++)
                {
                    movesToSend[i] = ((Button)sender).GetTag<int[]>()[i];
                }
            }
            if (game.board.getSquare(movesToSend[0], movesToSend[1]) % 2 == 1 || click == 1)
            {
                if (((MouseEventArgs)e).Button == MouseButtons.Left)
                {
                    if (click == 0)
                    {
                        var moves = game.board.GetMoves(1, movesToSend[0], movesToSend[1]);
                        if (moves.Count != 0)
                        {
                            ChangeAllButtons(false);
                            for (int i = 0; i < moves.Count; i++)
                            {
                                foreach (Button button in Controls)
                                {
                                    if (button.GetTag<int[]>()[0] == moves[i].Item3 && moves[i].Item4 == button.GetTag<int[]>()[1])
                                    {
                                        button.Enabled = true;
                                        button.BackColor = Color.Firebrick;
                                    }
                                }
                            }

                            click = 1;
                        }
                    }
                    else
                    {
                        for (int i = 0; i < 2; i++)
                        {
                            movesToSend[i + 2] = ((Button)sender).GetTag<int[]>()[i];
                        }
                        game.board.MakeMove(1, movesToSend[0], movesToSend[1], movesToSend[2], movesToSend[3]);
                        game.board.setBoard(game.ComputerTurn());
                        DrawBoard();
                        click = 0;
                    }
                }
                else
                {
                    ChangeAllButtons(true);
                }
            }           
        }

        private void ChangeAllButtons(bool state)
        {
            foreach (Button button in Controls)
            {
                button.Enabled = state;
            }
        }

       
    }
}

