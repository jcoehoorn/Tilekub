﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace Rummikub
{
    public partial class btnDone : Form
    {
        public btnDone()
        {
            InitializeComponent();

            Debug.WriteLine("Ready");
        }
        bool gameReady = false;

        private Deck DrawPile = new Deck();
        private TileSet[] Players;
        private int currentPlayer;

        private void ResetGame()
        {
            DrawPile.Clear();
            DeckBox.Controls.Clear();
            SetView1.Clear();
            SetView2.Clear();
            RunView.Clear();

            for (int i = 1; i <= 13; i++)
            {
                for (int j = 0; j <= (int)Color.Black; j++)
                {
                    DrawPile.Add(new Tile(i, (Color)j));
                    DrawPile.Add(new Tile(i, (Color)j));
                }
            }
            DrawPile.Add(new Tile(50, Color.Red));
            DrawPile.Add(new Tile(50, Color.Black));
            DrawPile.Shuffle();

            var playerSelect = new PlayerSelect();
            playerSelect.ShowDialog();

            int playerCount = playerSelect.SelectedPlayerCount;
            Players = new TileSet[playerCount];
            for (int i = 0; i < playerCount; i++)
            {
                Players[i] = new TileSet(13, 3);
                Players[i].Left = 4;
                Players[i].Top = 16;
            }

            for (int i = 0; i < playerCount; i++)
            {
                for (int j=0; j < 14; j++)
                {
                    Draw(Players[i]);
                }
            }

            currentPlayer = 0;
            DeckBox.Controls.Add(Players[currentPlayer]);
        }

        private Tile Draw(TileSet player)
        {
            Tile result = DrawPile[DrawPile.Count - 1];
            DrawPile.Remove(result);
            player.Add(result);
            return result;
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            if (!gameReady)
            {
                ResetGame();
                gameReady = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            EndTurn();
        }

        private void EndTurn()
        {
            if (!PlayAreaIsValid())
            {
                MessageBox.Show("Invalid tile placement.");
                return; //cancel
            }

            if (CheckWin())
            {
                MessageBox.Show("You win!");
                ResetGame();
            }

            currentPlayer++;
            if (currentPlayer >= Players.Length)
                currentPlayer = 0;

            SuspendLayout();
            DeckBox.Controls.Clear();
            DeckBox.Controls.Add(Players[currentPlayer]);
            ResumeLayout();
        }

        private bool CheckWin()
        {
            return Players[currentPlayer].Count == 0;
        }
        private bool PlayAreaIsValid()
        {
            //NotImplemented!
            return true;
        }

        private void btnDraw_Click(object sender, EventArgs e)
        {
            Draw(Players[currentPlayer]);
            EndTurn();
        }

        private void RunView_TileDropped(object sender, TileDropEventArgs args)
        {
            if (args.Tile.IsJoker) return; // never handle a joker

            var me = sender as TileSet;
            if (me == null) return;
            var target = me.CheckPosition(args.X,args.Y);

            bool handle = false;
            if ((int)args.Tile.Value != args.X + 1)
            {   //is the X position right?
                handle = true;
                args.X = args.Tile.Value-1;
                target = me.CheckPosition(args.X, args.Y);
            }

            if ((int)args.Tile.TileColor != args.Y / 2) //if they dropped in the space for the correct color, they may have had a reason. Don't "improve" the placement
            {
                //is the Y position right?

                int lowerCoord = ((int)args.Tile.TileColor) * 2;
                int upperCoord = lowerCoord + 1;
                Tile lowerTarget = me.CheckPosition(args.X, lowerCoord);
                Tile upperTarget = me.CheckPosition(args.X, upperCoord);
                //int will be easier than bool, because some of these I'll want to count (sum) occupied tiles
                int[] lower = me.Controls.OfType<TileHolder>().Skip(me.GridToIndex(0, lowerCoord)).Take(13).Select(c => c.Contents == null ? 0 : 1).ToArray();
                int[] upper = me.Controls.OfType<TileHolder>().Skip(me.GridToIndex(0, upperCoord)).Take(13).Select(c => c.Contents == null ? 0 : 1).ToArray();

                args.Y = (((int)args.Tile.TileColor) * 2) + TileLayoutHelper.PlaceRun(upper, lower, args.X, upperTarget != null && upperTarget.IsJoker, lowerTarget != null && lowerTarget.IsJoker);

                handle = true;
                target = me.CheckPosition(args.X, args.Y);
            }
            if (target != null)
            {
                //is the spot vacant?

                if (target.IsJoker)
                {
                    //keep this spot, but move the Joker to the next available position
                    Point p = me.IndexToGrid(me.NextVacantPosition(me.GridToIndex(args.X, args.Y)+1));
                    me.MoveTile(target, p.X, p.Y);
                }
                else
                {
                    //use the other Y spot
                    if (args.Y % 2 == 0)
                        args.Y++;
                    else
                        args.Y--;
                }

                handle = true;
            }
            
            if (handle)
            {
                //new x,y coords chosen
                // any adjustments were already made
                me.Add(args.Tile, args.X, args.Y);
                args.Handled = true;
            }
        }
    }
}