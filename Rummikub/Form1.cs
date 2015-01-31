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
                Players[i].TileDropped += PlayerView_TileDropped;
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

        protected class TileLocation
        {
            public TileLocation(TileSet view, Point coordinates) { View = view; X = coordinates.X; Y = coordinates.Y; }
            public TileLocation(TileSet view, int x, int y) { View = view; X = x; Y = y; }
            public TileSet View { get;private set; }
            public int X { get; private set; }
            public int Y { get; private set; }
        }

        private bool PlaySpaceIsValid()
        {
            bool result = true;
            SuspendLayout();

            for (int a = 0; a < RunView.Controls.Count; a++)
            {
                ((TileHolder)RunView.Controls[a]).BadTile = false;
            }
            for (int a = 0; a < SetView1.Controls.Count; a++)
            {
                ((TileHolder)SetView1.Controls[a]).BadTile = false;
                ((TileHolder)SetView2.Controls[a]).BadTile = false;
            }


            bool inRun = false;
            int runStartPos=0;
            int i = 0;
            while (i < RunView.Controls.Count)
            {
                var holder = RunView.Controls[i] as TileHolder;
                if (holder == null) throw new InvalidOperationException("Somehow a View has a control other than a TileHolder");

                if (i %13==0) //starting new row
                {
                    if (inRun) //finish previous run
                    {
                        if (i - runStartPos < 3)
                        {
                            for (int j = runStartPos; j<i;j++) ((TileHolder)RunView.Controls[j]).BadTile = true;
                            result = false;
                        }
                        inRun = false;
                    }
                }

                if (holder.Contents != null && !inRun)
                {
                    runStartPos = i;
                    inRun = true;
                }
                else if (holder.Contents == null && inRun)
                {
                    if (i - runStartPos < 3)
                    {
                        //too short
                        for (int j = runStartPos; j < i; j++) ((TileHolder)RunView.Controls[j]).BadTile = true;
                        result = false;
                    }
                    inRun = false;
                }
                i++;
            }


            //Sets
            var sets = new TileSet[2] {SetView1, SetView2};
            for (i = 0; i < 13; i++)
            {
                for (int j=0;j<sets.Length; j++)
                {
                    int idx = sets[j].GridToIndex(0, i * 4);
                    var Tiles = sets[j].Controls.OfType<TileHolder>().Skip(idx).Take(4);
                    int sum = Tiles.Select(c => c.Contents == null ? 0 : 1).Sum();
                    if (sum > 0 && sum < 3)
                    {
                        //highlight the whole row
                        foreach (TileHolder t in Tiles) { t.BadTile = true; }
                        result = false;
                    }
                }
            }

            ResumeLayout();
            return result;
        }

        private void EndTurn()
        {
            if (!PlaySpaceIsValid())
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
 
        private void btnDraw_Click(object sender, EventArgs e)
        {
            if (!PlaySpaceIsValid()) //change later to revert to start of turn state
            {
                MessageBox.Show("Invalid tile placement.");
                return; //cancel
            }
            Draw(Players[currentPlayer]);
            EndTurn();
        }

        private void RunView_TileDropped(object sender, TileDropEventArgs args)
        {
            if (args.Tile.IsJoker) return; // never handle a joker

            var me = sender as TileSet;
            if (me == null) return;
            var target = me.CheckPosition(args.X,args.Y);

            if ((int)args.Tile.Value != args.X + 1)
            {   //is the X position right?
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

                int choice = TileLayoutHelper.PlaceRun(upper, lower, args.X, upperTarget != null && upperTarget.IsJoker, lowerTarget != null && lowerTarget.IsJoker);
                if (choice == -1)  choice = 0; // no clear winner... use 0

                args.Y = (((int)args.Tile.TileColor) * 2) + choice;
                target = me.CheckPosition(args.X, args.Y);
            }
            if (target != null)//is the spot vacant?
            {
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
            }

            //new x,y coords chosen
            // any adjustments were already made
            me.Add(args.Tile, args.X, args.Y);
            args.Handled = true;
            PlaySpaceIsValid();
        }

        private void SetView_TileDropped(object sender, TileDropEventArgs args)
        {
            if (args.Tile.IsJoker) return; // never handle a joker

            var me = sender as TileSet;
            var other = (me == SetView1 ? SetView2 : SetView1);
            if (me == null) return;
            var target = me.CheckPosition(args.X, args.Y);

            if ((int)args.Tile.Value != args.Y + 1)
            {   //is the Y position right?
                args.Y = (int)args.Tile.Value-1;
                target = me.CheckPosition(args.X, args.Y);
            }
            if ((int)args.Tile.TileColor != args.X) //is the X position right?
            {
                args.X = (int)args.Tile.TileColor;

                int[] left = SetView1.Controls.OfType<TileHolder>().Skip(SetView1.GridToIndex(0,args.Y)).Take(4).Select(c => c.Contents == null ? 0 : 1).ToArray();
                int[] right = SetView2.Controls.OfType<TileHolder>().Skip(SetView2.GridToIndex(0,args.Y)).Take(4).Select(c => c.Contents == null ? 0 : 1).ToArray();
                Tile lTile = SetView1.CheckPosition(args.X, args.Y);
                Tile rTile = SetView2.CheckPosition(args.X, args.Y);
                int choice = TileLayoutHelper.PlaceSet(left, right, args.X, lTile != null && lTile.IsJoker, rTile != null && rTile.IsJoker);
                if (choice >= 0)
                {
                    var sets = new TileSet[2] { SetView1, SetView2 };
                    me = sets[choice];
                }
                target = me.CheckPosition(args.X, args.Y);
            }
            
            if (target != null)  //is the spot vacant?
            {
                if (target.IsJoker)
                {
                    //keep this spot, but move the Joker to the next available position
                    Point p = me.IndexToGrid(me.NextVacantPosition(me.GridToIndex(args.X, args.Y) + 1));
                    me.MoveTile(target, p.X, p.Y);
                }
                else
                {
                    //use the other TileSet
                    var otherTile = other.CheckPosition(args.X, args.Y);
                    if (otherTile != null) //either the other tile is a joker, or the new tile is a joker
                    {
                        Point p = other.IndexToGrid(other.NextVacantPosition(other.GridToIndex(args.X, args.Y)));

                        if(otherTile.IsJoker)
                        {
                            other.MoveTile(otherTile, p.X, p.Y);
                        }
                        else
                        {   //I'm the joker
                            me = other;
                            args.X = p.X;
                            args.Y= p.Y;
                        }
                    }
                    
                }
            }

            me.Add(args.Tile, args.X, args.Y);
            args.Handled = true;
            PlaySpaceIsValid();
        }

        private void PlayerView_TileDropped(object sender, TileDropEventArgs args)
        {
            if (args.Tile.ViewPort != sender)
              PlaySpaceIsValid();
        }
    }
}