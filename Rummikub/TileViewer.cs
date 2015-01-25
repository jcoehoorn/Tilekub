using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Rummikub
{
    public partial class TileSetViewer : Panel
    {
        public TileSetViewer()
        {
            this.AllowDrop = true;
            InitializeComponent();
        }

        private int _rows;
        public int Rows
        {
            get { return _rows; }
            set
            {
                this.Height = (value * (Tile.TileHeight + Tile.SpacingY)) + Tile.SpacingY;
                _rows = value;
            }
        }

        private int _cols;
        public int Columns
        {
            get { return _cols; }
            set
            {
                this.Width = (value * (Tile.TileWidth + Tile.SpacingX)) + Tile.SpacingX;
                _cols = value;
            }
        }

        private Deck _tiles = new Deck();

        [Browsable(false)]
        public Deck Tiles
        {
            get { return _tiles; }
            set
            {
                _tiles = value;
                tilePositions.Clear();
                DrawScreen();
            }
        }

        #region DragDrop

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
               
                //get x,y position coordinates
                var p = PointToClient(new Point(e.X, e.Y));
                int x = (p.X  - Tile.SpacingX) / (Tile.TileWidth + Tile.SpacingX);           
                int y = (p.Y- Tile.SpacingY) / (Tile.TileHeight + Tile.SpacingY); //not sure why the -1 is needed

                Tile source = (Tile)e.Data.GetData(Tile.DragDropFormatName);
                if (source.Parent == this)
                {
                    //moved withing same view area
                    SetPostion(source, x, y);
                    SuspendLayout();
                    source.Location = GridToPoint(x, y);
                    ResumeLayout();
                    
                }
                else
                {
                    //moved from different view area
                    var original = (TileSetViewer)source.Parent;
                    original.RemoveTile(source);//remove from previous area.
                    AddTile(source, x, y); //Add here
                }
            }
        }

        protected override void OnDragLeave(EventArgs e)
        {
            base.OnDragLeave(e);
        }
        #endregion

        private List<Tuple<int, int, Tile>> tilePositions= new List<Tuple<int, int, Tile>>();

        public void SetPostion(Tile tile, int x, int y)
        {
            var destination = CheckPosition(x, y);
            if (destination != null) return; //if the position is occupied, do nothing, even if it's the same item

            for (int i = 0; i < tilePositions.Count; i++)
            {
                if (tilePositions[i].Item3 == tile)
                {
                    tilePositions[i] = Tuple.Create(x, y, tile);
                    i = tilePositions.Count;
                }
            }     
        }

        public Tuple<int, int> FindTile(Tile tile)
        {
            foreach (var entry in tilePositions)
            {
                if (entry.Item3 == tile)
                    return Tuple.Create(entry.Item1, entry.Item2);
            }
            return default(Tuple<int, int>);
        }

        /// <summary>
        /// Find the index of the tile in the tilePositions list
        /// </summary>
        /// <returns>The index of the tile if it is found, or -1 if the tile is not found.</returns>
        private int FindTilePositionIndex(Tile tile)
        {
            for (int i = 0; i < tilePositions.Count; i++)
            {
                if (tilePositions[i].Item3 == tile) return i;
            }
            return -1;
        }

        public bool AddTile(Tile tile)
        {
            //this is an awful way to do this :(
            var spots = new bool[Columns, Rows];
            foreach (var entry in tilePositions)
            {
                spots[entry.Item1, entry.Item2] = true;
            }
            for(int y = 0;y<Rows;y++)
                for (int x = 0; x < Columns; x++)
                    if (!spots[x, y])
                    {
                        return AddTile(tile, x, y);                      
                    }
            return false;
        }

        public bool AddTile(Tile tile, int x, int y)
        {
            if (CheckPosition(x, y) != null) return false;

            Tiles.Add(tile);
            tilePositions.Add(Tuple.Create(x,y,tile));
            tile.Location = GridToPoint(x, y);
            Controls.Add(tile);
            return true;
        }

        private Point GridToPoint(int x, int y)
        {
            return new Point(Tile.SpacingX + (x * (Tile.TileWidth + Tile.SpacingX)), Tile.SpacingY + (y * (Tile.TileHeight + Tile.SpacingY)));
        }

        public void SwapTiles(Tile t1, Tile t2)
        {
            int a = FindTilePositionIndex(t1);
            int b = FindTilePositionIndex(t2);
            if (a >= 0 && b >= 0)
            {
                int x1 = tilePositions[a].Item1;
                int y1 = tilePositions[a].Item2;
                int x2 = tilePositions[b].Item1;
                int y2 = tilePositions[b].Item2;
                tilePositions[a] = Tuple.Create(x2, y2, t1);
                tilePositions[b] = Tuple.Create(x1, y1, t2);
                SuspendLayout();
                x1 = t1.Left;
                y1 = t1.Top;
                t1.Left = t2.Left;
                t1.Top = t2.Top;
                t2.Left = x1;
                t2.Top = y1;
                ResumeLayout();
            }
        }

        public void RemoveTile(Tile tile)
        {
            Tiles.Remove(tile);

            int i = FindTilePositionIndex(tile);
            if (i >= 0) tilePositions.RemoveAt(i);
            Controls.Remove(tile);
        }

        public Tile CheckPosition(int x, int y)
        {
            return tilePositions.Where(t => t.Item1 == x && t.Item2 == y).Select(t=> t.Item3).FirstOrDefault();
        }

        public void DrawScreen()
        {
            this.SuspendLayout();
            Controls.Clear();

            if (Tiles != null && Tiles.Count > 0)
            {
                SetTilePositions();
                
                foreach (var tile in tilePositions)
                {
                    tile.Item3.Location = GridToPoint(tile.Item1, tile.Item2);
                    Controls.Add(tile.Item3);
                }
            }
            this.ResumeLayout();
        }

        private void SetTilePositions()
        {
            if (tilePositions.Count == 0)
            {
                tilePositions = new List<Tuple<int, int, Tile>>();
                int x = 0; int y = 0;
                foreach (var tile in Tiles)
                {
                    tilePositions.Add(Tuple.Create(x, y, tile));
                    x++;
                    if (x >= Columns)
                    {
                        x = 0;
                        y++;
                    }
                }
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            var g = CreateGraphics();
            var p = SystemPens.ControlDark;
            for (int i = 1; i<Columns;i++)
            {
                int x = (Tile.SpacingX / 2) + ((Tile.SpacingX + Tile.TileWidth)* i);
                g.DrawLine(p, x, 0, x, Height);
            }

            for (int i = 1; i < Rows; i++)
            {
                int y = (Tile.SpacingY / 2) + ((Tile.SpacingY + Tile.TileHeight) * i);
                g.DrawLine(p, 0, y, Width, y);
            }
        }
    }
}
