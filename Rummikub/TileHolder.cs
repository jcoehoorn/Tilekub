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

                this.Contents = source;
            }
        }

        protected override void OnDragLeave(EventArgs e)
        {
            base.OnDragLeave(e);
        }
        #endregion

    }
}
