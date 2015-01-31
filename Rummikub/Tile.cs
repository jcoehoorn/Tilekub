using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;
using System.Diagnostics;

//MISSING FEATURES/ROADMAP:
// TaDone 0.1 Mark incomplete groups after each tile placement
// TaDone 0.2 Force no incomplete groups before accepting the "Done" button
// TaDone 0.21 Prevent moving tiles to PlayerSpace that did not start the turn in the playerspace
// TODO 0.3 Automatically Undo any tile placements when clicking Draw Tile
// TODO 0.4 Enforce meeting meld for each player before the first time the lay down
// -- all the important game rules are enfoced at this point
// TODO 0.5 Undo back to beginning of turn
// TODO 0.6 Undo one tile placement (note: make sure incomplete group marks are drawn correctly)
// TODO 0.7 Enforce 2min time limit to expire turns (rummikub rules)
// TODO 0.8 Enforce 15s time limit to expire turns if player moves no tiles, increase to normal limit after first tile move
// TODO 0.9 Add some audio, maybe some animations, cosmetics and bug fixes ahead of full release
// TODO 1.0 Bug fixes

// -------------------------------
// Version 2: Network/Web play
// TODO 1.1 Inital release for web service to support web play -- separate project in the solution
// TODO 1.2 Intial release to support client connecting to service
// TODO 1.3 Intial release to support client automatically updating from service
// TODO 1.4 Support chat
// TODO 1.5 Add spectators to Chat
// TODO 1.6 Waiting room: losers are demoted to make room for spectators
// TODO 1.7 Host web service on Azure
// TODO 1.8 Support reporting chat offenders
// TODO 1.9 Bug Fixes /RC1
// TODO 2.0 Bug Fixes

//-------------------------------
// Version 3: Web client
// Straight up port, will jump straight to 3.0 for initial web release
// TODO 3.0 Port of client code to web site
// TODO 3.1 Require account (OpenID, Facebook, etc)
// TODO 3.2 Start tracking stats when signed in
// TODO 3.3 Unobtrusive/non-interfering ads on the web site

//---------------------------------
// Version 4.0: Graphics
// If I get enough ad revenue, I'll hire graphics help
// This would come out all at once in a 4.0 release with no new features,
// but it would likely include a 4.0.x series to correct bugs in the release

//---------------------------------
// Version 5.0: Mobile
// Clients for iOS, Android, and Windows
// Probably need to re-work a lot of the user interface

namespace Rummikub
{
    public enum Color
    {
        Red =0,
        Yellow,
        Blue,
        Black
    }

    public class Tile : Panel, IComparable<Tile>
    {
        public int Value { get; private set; }
        public Color TileColor { get; private set; }

        public bool IsJoker { get {return Value == 50;}}

        public static int TileWidth { get { return 30; } }
        public static int TileHeight { get { return 40; } }
        public static int SpacingX { get { return 4; } }
        public static int SpacingY { get { return 4; } }

        public Tile(int Value, Color TileColor)
        {
            if (Value < 1 || Value > 13) Value = 50;
            this.Value = Value;

            this.TileColor = TileColor;
            switch (this.TileColor)
            {
                case Color.Red:
                    this.ForeColor = System.Drawing.Color.Crimson;
                    break;
                case Color.Yellow:
                    this.ForeColor = System.Drawing.Color.Gold;
                    break;
                case Color.Blue:
                    this.ForeColor = System.Drawing.Color.DodgerBlue;
                    break;
                case Color.Black:
                    this.ForeColor = System.Drawing.Color.Black;
                    break;
            }

            this.Width = Tile.TileWidth;
            this.Height = Tile.TileHeight;
            FontStyle style = FontStyle.Bold;
            this.Font = new Font(this.Font.FontFamily, 13.5f, style);
            AllowDrop = true;
            DrawTile();
        }

        public TileSet ViewPort 
        {
            get
            {
                var parent = this.Parent as TileHolder;
                if (parent == null) return null;
                return parent.Parent as TileSet;
            }
        }

        public bool CanMoveToPlayerSpace { get; set; }

        private void DrawTile()
        {
            Image background = new Bitmap(this.Width, this.Height);
            Graphics g = Graphics.FromImage(background);

            g.FillRectangle(new SolidBrush(System.Drawing.Color.White),0, 0, this.Width, this.Height);
            g.DrawRectangle(new Pen(System.Drawing.Color.Black), 0, 0, this.Width-1, this.Height-1);
 
            string s = (this.IsJoker) ? "@" : this.Value.ToString();
            SolidBrush b = new SolidBrush(this.ForeColor);
            var size = g.MeasureString(s, this.Font);
           
            g.DrawString(s, this.Font, b, (this.Width - size.Width)/2, 0);
            
            this.BackgroundImage = background;
        }

        public int CompareTo(Tile other)
        {
            if (other == null) return 1;

            if (other.IsJoker && !this.IsJoker) return -1;
            if (other.IsJoker && this.TileColor == Color.Black) return 1;
            if (other.IsJoker) return -1;

            int result = this.Value.CompareTo(other.Value);
            if (result == 0) return this.TileColor.CompareTo(other.TileColor);
            return result;           
        }

        #region DragDrop
        public static string DragDropFormatName = "RummikubTile";

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (Parent is TileHolder) //this is needed or the Click event won't fire in the PlayerSelect form
            {
                DataObject data = new DataObject(Tile.DragDropFormatName, this);
                DoDragDrop(data, DragDropEffects.Move);
            }
        }
        protected override void OnDragEnter(DragEventArgs e)
        {
            base.OnDragEnter(e);

            if (e.Data.GetDataPresent(Tile.DragDropFormatName))
            {
                e.Effect = DragDropEffects.Move;
            }
        }

        protected override void OnDragOver(DragEventArgs e)
        {
            base.OnDragOver(e);

            if (e.Data.GetDataPresent(Tile.DragDropFormatName))
            {
                e.Effect = DragDropEffects.Move;
            }
        }

        protected override void OnDragDrop(DragEventArgs e)
        {
            base.OnDragDrop(e);

            if (e.Data.GetDataPresent(Tile.DragDropFormatName))
            {
                Tile source = (Tile)e.Data.GetData(Tile.DragDropFormatName);
 
                if (this == source) return; // nothing to do
                var myHolder = this.Parent as TileHolder;
                if (myHolder == null) return;

                //raise parent's OnTileDropped event
                bool DropHandled = false;
                int idx = myHolder.ParentIndex();
                if (idx >= 0)
                {
                    Point p = ViewPort.IndexToGrid(idx);
                    DropHandled = ViewPort.RaiseTileDroppedEvent(source, p.X, p.Y);
                }

                if (!DropHandled) 
                {
                    if (this.ViewPort != source.ViewPort) return; //don't swap with tile in different area

                    var sourceHolder = source.Parent as TileHolder;             
                    if (source == null) return; 

                    myHolder.Contents = source;
                    sourceHolder.Contents = this;
                }
            }
        }
        #endregion
    }
}
