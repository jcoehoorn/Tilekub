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
            _count = Count;
            SetSize();
        }

        public void Add(Tile tile, int x, int y)
        {
            var holder = (TileHolder)Controls[GridToIndex(x,y)];
            holder.Contents = tile;
        }
        public void Add(Tile tile)
        {
            var holder = Controls.OfType<TileHolder>().FirstOrDefault(h => h.Contents == null);
            holder.Contents = tile;
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

        private int GridToIndex(int x, int y)
        {
            return x + (y * Columns);
        }

        private Point IndexToGrid(int index)
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
        [DefaultValue(39)]
        public int Count
        {
            get { return _count; }
            set {  _count = value; SetSize(); }
        
        }
        private int _count = 39;

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

        protected void SetSize()
        {
            Width = Columns * (Tile.SpacingX + Tile.TileWidth);
            int rows = Count / Columns;
            if (Count % Columns != 0) rows++;
            Height = rows * (Tile.SpacingY + Tile.TileHeight);

            Controls.Clear();
            for (int i = 0; i < Count; i++)
            {
                Controls.Add(new TileHolder());
            }
        }
    }
}
