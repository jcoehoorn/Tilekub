using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Rummikub
{
    public partial class PlayerSelect : Form
    {
        public PlayerSelect()
        {
            InitializeComponent();
        }

        public int SelectedPlayerCount {get;private set;}

        private void PlayerSelect_Load(object sender, EventArgs e)
        {
            Tile[] t = new Tile[3];
            t[0] = new Tile(2, Color.Blue);
            t[1] = new Tile(3, Color.Red);
            t[2] = new Tile(4, Color.Yellow);

            int XSpacing = 10;
            int XOffset = (ClientSize.Width - ((Tile.TileWidth * 3) + (XSpacing * 2))) / 2;
            for(int i = 0;i<3;i++)
            {
                t[i].Left = XOffset + (i*(Tile.TileWidth + XSpacing));
                t[i].Top = 50;
                t[i].AllowDrop = false;
                t[i].MouseClick += (s, a) => { SelectedPlayerCount = ((Tile)s).Value; DialogResult = DialogResult.OK; };
            }
            Controls.AddRange(t);
        }
    }
}
