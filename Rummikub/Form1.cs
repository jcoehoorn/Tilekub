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
        private TileSet[] Players;
        private int currentPlayer;

        private void ResetGame()
        {
            DrawPile.Clear();
            DeckBox.Controls.Clear();
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
                Players[i] = new TileSet(13, 39);
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

        private void NextPlayer()
        {

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