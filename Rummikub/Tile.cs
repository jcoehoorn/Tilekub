using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;
using System.Diagnostics;

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
