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
    public partial class TileHolder : UserControl
    {
        public TileHolder()
        {
            InitializeComponent();

            AllowDrop = true;
            var margin = new Padding(0, 0, 0, 0);
            Margin = margin;
            BorderStyle = BorderStyle.FixedSingle;
            Width = Tile.TileWidth + Tile.SpacingX;
            Height = Tile.TileHeight + Tile.SpacingY;
        }

        private Tile _tile;
        public Tile Contents
        {
            get{return _tile;}
            set
            {
                if (_tile != null) Controls.Remove(_tile);
                if (value != null)
                {
                    value.Left = (Tile.SpacingX / 2)-1;
                    value.Top = (Tile.SpacingY / 2) -1;
                    Controls.Add(value);
                }
                _tile = value;        
            } 
       }

        public TileSet ViewPort { get { return Parent as TileSet; } }

        public int ParentIndex()
        {
            if (ViewPort != null)
            {
                for(int i =0;i<ViewPort.Controls.Count;i++)
                {
                    if (ViewPort.Controls[i] == this) return i;
                }
            }
            return -1;
        }

        private bool _badTile = false;
        public bool BadTile
        {
            get { return _badTile; }
            set
            {
                if (value)
                {//too flat. Need something brighter (but not Color.Red!). Maybe create some kind of gradient for a glow effect. For now, this gets the job done, though
                    BackColor = System.Drawing.Color.Salmon; 
                }
                else
                {
                    BackColor = System.Drawing.SystemColors.Control;
                }
                _badTile = value;
            }
        }

        public TileHolder LeftNeighbor 
        {
            get
            {
                if (ViewPort == null) return null;
                int idx = ParentIndex(); if (idx <= 0 || idx % ViewPort.Columns == 0) return null; return ViewPort.Controls[idx + 1] as TileHolder; 
            }
        }

        public TileHolder RightNeighbor 
        {
            get 
            {
                if (ViewPort == null) return null;
                int idx = ParentIndex(); if (idx < 0 || idx % ViewPort.Columns == ViewPort.Columns - 1) return null; return ViewPort.Controls[idx - 1] as TileHolder;
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
                Tile source = (Tile)e.Data.GetData(Tile.DragDropFormatName);
                if (source.Parent == this) return;
      
                var original = (TileHolder)source.Parent;
                original.Contents = null;

                //raise parent's OnTileDropped event
                bool DropHandled = false;
                int idx = ParentIndex();
                if (idx >= 0)
                {
                    Point p = ViewPort.IndexToGrid(idx);             
                    DropHandled = ViewPort.RaiseTileDroppedEvent(source, p.X, p.Y);
                }
                //fallback, shouldn't really need this
                if (!DropHandled) this.Contents = source;
            }
        }

        protected override void OnDragLeave(EventArgs e)
        {
            base.OnDragLeave(e);
        }
        #endregion

    }
}
