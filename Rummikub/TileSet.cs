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
    public partial class TileSet : FlowLayoutPanel
    {
        public TileSet()
        {
            InitializeComponent();

            Margin = new Padding(0, 0, 0, 0);
            SetSize();
        }

        public TileSet(int Columns, int Count) : this()
        {
            _columns = Columns;
            _rows = Count;
            SetSize();
        }

        public bool Add(Tile tile, int x, int y)
        {
            var holder = (TileHolder)Controls[GridToIndex(x,y)];
            if (holder == null || holder.Contents != null) return false;
 
            holder.Contents = tile;
            return true;
        }
        public bool Add(Tile tile)
        {
            int idx = NextVacantPosition(0);
            if (idx >= 0)
            {
                var holder = Controls[idx] as TileHolder;
                if (holder != null)
                {
                    holder.Contents = tile;
                    return true;
                }
            }
            return false;
        }
        public int NextVacantPosition(int minPosition)
        {
            int result;
            for (result = minPosition; result < Controls.Count;result++)
            {
                var holder = Controls[result] as TileHolder;
                if (holder != null && holder.Contents == null)
                {
                    return result;
                }
            }
            //didn't find an empty spot yet... continue at beginning
            for (result = 0; result < minPosition;result++ )
            {
                var holder = Controls[result] as TileHolder;
                if (holder != null && holder.Contents == null)
                {
                    return result;
                }
            }
            // no empty spots
            return -1;
        }
        public void MoveTile(Tile tile, int x, int y)
        {
            var holder = Controls.OfType<TileHolder>().FirstOrDefault(h => h.Contents == tile);
            if (Add(tile, x, y) && holder != null)
                holder.Contents = null;
        }

        public void Remove(Tile tile)
        {
            foreach (TileHolder c in Controls.OfType<TileHolder>())
            {
                if (c.Contents == tile)
                {
                    c.Contents = null;
                    break;
                }
            }
        }

        public int GridToIndex(int x, int y)
        {
            return x + (y * Columns);
        }

        public Point IndexToGrid(int index)
        {
            return new Point(index % Columns, index / Columns);
        }

        public Tile this[int index]
        {
            get
            {
                return ((TileHolder)Controls[index]).Contents;
            }
            set
            {
                ((TileHolder)Controls[index]).Contents = value;
            }
        }

        public Tile this[int x, int y]
        {
            get
            {
                return ((TileHolder)Controls[GridToIndex(x,y)]).Contents;
            }
            set
            {
                ((TileHolder)Controls[GridToIndex(x,y)]).Contents = value;
            }
        }

        public Tile CheckPosition(int x, int y)
        {
            return ((TileHolder)Controls[GridToIndex(x, y)]).Contents;
        }

        [Browsable(true)]
        [DefaultValue(3)]
        public int Rows
        {
            get { return _rows; }
            set {  _rows = value; SetSize(); }
        
        }
        private int _rows = 39;

        [DefaultValue(13)]
        public int Columns 
        { 
            get { return _columns; } 
            set { _columns = value; SetSize(); } 
        }
        private int _columns = 13;

        [Browsable(false)]
        public new int Width { get { return base.Width; } private set { base.Width = value; } }

        [Browsable(false)]
        public new int Height { get { return base.Height; } private set { base.Height = value; } }

        public int Count { get { return Controls.OfType<TileHolder>().Count(c => c.Contents != null); } }

        public void Clear()
        {
            foreach (var c in Controls.OfType<TileHolder>())
            {
                c.Contents = null;
            }
        }

        protected void SetSize()
        {
            Width = Columns * (Tile.SpacingX + Tile.TileWidth);
            Height = Rows * (Tile.SpacingY + Tile.TileHeight);

            Controls.Clear();
            for (int i = 0; i < (Columns * Rows); i++)
            {
                Controls.Add(new TileHolder());
            }
        }

        #region "TileDropped Event"
        public delegate void TileDropEventHandler(object sender, TileDropEventArgs args);

        public event TileDropEventHandler TileDropped;

        public bool RaiseTileDroppedEvent(Tile tile, int x, int y)
        {
            var dropped = this.TileDropped;
            if (dropped != null)
            {
                var args = new TileDropEventArgs(tile, x, y);
                dropped(this, args);
                return args.Handled;
            }
            return false;
        }
        #endregion

    }

    public class TileDropEventArgs : EventArgs 
    {
        public int X { get; set; }
        public int Y { get; set; }
        public Tile Tile;
        public bool Handled { get; set; }

        public TileDropEventArgs(Tile tile, int x, int y)
        {
            this.Tile = tile;
            this.X = x;
            this.Y = y;
            this.Handled = false;
        }
    }
}
