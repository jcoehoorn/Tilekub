namespace Rummikub
{
    partial class btnDone
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.SetBox = new System.Windows.Forms.GroupBox();
            this.RunBox = new System.Windows.Forms.GroupBox();
            this.DeckBox = new System.Windows.Forms.GroupBox();
            this.btnDraw = new System.Windows.Forms.Button();
            this.RunView = new Rummikub.TileSet();
            this.SetView1 = new Rummikub.TileSet();
            this.SetView2 = new Rummikub.TileSet();
            this.button1 = new System.Windows.Forms.Button();
            this.SetBox.SuspendLayout();
            this.RunBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // SetBox
            // 
            this.SetBox.Controls.Add(this.SetView1);
            this.SetBox.Controls.Add(this.SetView2);
            this.SetBox.Location = new System.Drawing.Point(8, 0);
            this.SetBox.Name = "SetBox";
            this.SetBox.Size = new System.Drawing.Size(301, 601);
            this.SetBox.TabIndex = 0;
            this.SetBox.TabStop = false;
            this.SetBox.Text = "Sets";
            // 
            // RunBox
            // 
            this.RunBox.Controls.Add(this.RunView);
            this.RunBox.Location = new System.Drawing.Point(317, 0);
            this.RunBox.Name = "RunBox";
            this.RunBox.Size = new System.Drawing.Size(452, 375);
            this.RunBox.TabIndex = 1;
            this.RunBox.TabStop = false;
            this.RunBox.Text = "Runs";
            // 
            // DeckBox
            // 
            this.DeckBox.Location = new System.Drawing.Point(317, 380);
            this.DeckBox.Name = "DeckBox";
            this.DeckBox.Size = new System.Drawing.Size(452, 153);
            this.DeckBox.TabIndex = 2;
            this.DeckBox.TabStop = false;
            this.DeckBox.Text = "Deck";
            // 
            // btnDraw
            // 
            this.btnDraw.Location = new System.Drawing.Point(506, 539);
            this.btnDraw.Name = "btnDraw";
            this.btnDraw.Size = new System.Drawing.Size(54, 21);
            this.btnDraw.TabIndex = 3;
            this.btnDraw.Text = "Draw";
            this.btnDraw.UseVisualStyleBackColor = true;
            this.btnDraw.Click += new System.EventHandler(this.btnDraw_Click);
            // 
            // RunView
            // 
            this.RunView.Rows = 8;
            this.RunView.Location = new System.Drawing.Point(4, 13);
            this.RunView.Margin = new System.Windows.Forms.Padding(0);
            this.RunView.Name = "RunView";
            this.RunView.Size = new System.Drawing.Size(442, 352);
            this.RunView.TabIndex = 0;
            // 
            // SetView1
            // 
            this.SetView1.Columns = 4;
            this.SetView1.Rows = 13;
            this.SetView1.Location = new System.Drawing.Point(7, 20);
            this.SetView1.Margin = new System.Windows.Forms.Padding(0);
            this.SetView1.Name = "SetView1";
            this.SetView1.Size = new System.Drawing.Size(136, 572);
            this.SetView1.TabIndex = 0;
            // 
            // SetView2
            // 
            this.SetView2.Columns = 4;
            this.SetView2.Rows = 13;
            this.SetView2.Location = new System.Drawing.Point(157, 20);
            this.SetView2.Margin = new System.Windows.Forms.Padding(0);
            this.SetView2.Name = "SetView2";
            this.SetView2.Size = new System.Drawing.Size(136, 572);
            this.SetView2.TabIndex = 52;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(446, 539);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(54, 21);
            this.button1.TabIndex = 4;
            this.button1.Text = "Done";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnDone
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(776, 608);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btnDraw);
            this.Controls.Add(this.DeckBox);
            this.Controls.Add(this.RunBox);
            this.Controls.Add(this.SetBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "btnDone";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Rummikub";
            this.Shown += new System.EventHandler(this.Form1_Shown);
            this.SetBox.ResumeLayout(false);
            this.RunBox.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox SetBox;
        private System.Windows.Forms.GroupBox RunBox;
        private System.Windows.Forms.GroupBox DeckBox;
        private System.Windows.Forms.Button btnDraw;
        private TileSet RunView;
        private TileSet SetView1;
        private TileSet SetView2;
        private System.Windows.Forms.Button button1;
    }
}

