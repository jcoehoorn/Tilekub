using System;
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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            Debug.WriteLine("Ready");
        }
        bool gameReady = false;

        private Deck DrawPile = new Deck();
        private Deck[] Players;
        private Deck inPlay = new Deck();

        private void ResetGame()
        {
            DrawPile.Clear();
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
            Players = new Deck[playerCount];
            for (int i = 0; i < playerCount; i++)
                Players[i] = new Deck();

            for (int i = 0; i < playerCount; i++)
            {
                for (int j=0; j < 14; j++)
                {
                    Draw(i);
                }
            }
 
            foreach (var tile in Players[1])
            {
                PlayerView.Add(tile);
            }
        }

        private Tile Draw(int playerIndex)
        {
            Tile result = DrawPile[DrawPile.Count - 1];
            DrawPile.Remove(result);
            Players[playerIndex].Add(result);
            return result;
        }

        private void btnDraw_Click(object sender, EventArgs e)
        {
            //PlayerDeck.AddTile(Draw(0));
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            if (!gameReady)
            {
                ResetGame();
                gameReady = true;
            }
        }
    }
}