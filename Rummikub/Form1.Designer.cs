namespace Rummikub
{
    partial class Form1
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
            Rummikub.Deck deck1 = new Rummikub.Deck();
            Rummikub.Deck deck2 = new Rummikub.Deck();
            Rummikub.Deck deck3 = new Rummikub.Deck();
            this.SetBox = new System.Windows.Forms.GroupBox();
            this.SetViewer = new Rummikub.TileSetViewer();
            this.RunBox = new System.Windows.Forms.GroupBox();
            this.RunViewer = new Rummikub.TileSetViewer();
            this.DeckBox = new System.Windows.Forms.GroupBox();
            this.PlayerDeck = new Rummikub.TileSetViewer();
            this.btnDraw = new System.Windows.Forms.Button();
            this.SetBox.SuspendLayout();
            this.RunBox.SuspendLayout();
            this.DeckBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // SetBox
            // 
            this.SetBox.Controls.Add(this.SetViewer);
            this.SetBox.Location = new System.Drawing.Point(8, 0);
            this.SetBox.Name = "SetBox";
            this.SetBox.Size = new System.Drawing.Size(275, 585);
            this.SetBox.TabIndex = 0;
            this.SetBox.TabStop = false;
            this.SetBox.Text = "Sets";
            // 
            // SetViewer
            // 
            this.SetViewer.AllowDrop = true;
            this.SetViewer.Columns = 0;
            this.SetViewer.Location = new System.Drawing.Point(7, 20);
            this.SetViewer.Name = "SetViewer";
            this.SetViewer.Rows = 0;
            this.SetViewer.Size = new System.Drawing.Size(200, 100);
            this.SetViewer.TabIndex = 0;
            this.SetViewer.Tiles = deck1;
            // 
            // RunBox
            // 
            this.RunBox.Controls.Add(this.RunViewer);
            this.RunBox.Location = new System.Drawing.Point(292, 0);
            this.RunBox.Name = "RunBox";
            this.RunBox.Size = new System.Drawing.Size(445, 375);
            this.RunBox.TabIndex = 1;
            this.RunBox.TabStop = false;
            this.RunBox.Text = "Runs";
            // 
            // RunViewer
            // 
            this.RunViewer.AllowDrop = true;
            this.RunViewer.Columns = 0;
            this.RunViewer.Location = new System.Drawing.Point(7, 20);
            this.RunViewer.Name = "RunViewer";
            this.RunViewer.Rows = 0;
            this.RunViewer.Size = new System.Drawing.Size(200, 100);
            this.RunViewer.TabIndex = 0;
            this.RunViewer.Tiles = deck2;
            // 
            // DeckBox
            // 
            this.DeckBox.Controls.Add(this.PlayerDeck);
            this.DeckBox.Location = new System.Drawing.Point(292, 380);
            this.DeckBox.Name = "DeckBox";
            this.DeckBox.Size = new System.Drawing.Size(445, 146);
            this.DeckBox.TabIndex = 2;
            this.DeckBox.TabStop = false;
            this.DeckBox.Text = "Deck";
            // 
            // PlayerDeck
            // 
            this.PlayerDeck.AllowDrop = true;
            this.PlayerDeck.Columns = 0;
            this.PlayerDeck.Location = new System.Drawing.Point(3, 11);
            this.PlayerDeck.Name = "PlayerDeck";
            this.PlayerDeck.Rows = 0;
            this.PlayerDeck.Size = new System.Drawing.Size(200, 100);
            this.PlayerDeck.TabIndex = 0;
            this.PlayerDeck.Tiles = deck3;
            // 
            // btnDraw
            // 
            this.btnDraw.Location = new System.Drawing.Point(441, 535);
            this.btnDraw.Name = "btnDraw";
            this.btnDraw.Size = new System.Drawing.Size(54, 21);
            this.btnDraw.TabIndex = 3;
            this.btnDraw.Text = "Draw";
            this.btnDraw.UseVisualStyleBackColor = true;
            this.btnDraw.Click += new System.EventHandler(this.btnDraw_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(746, 624);
            this.Controls.Add(this.btnDraw);
            this.Controls.Add(this.DeckBox);
            this.Controls.Add(this.RunBox);
            this.Controls.Add(this.SetBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Rummikub";
            this.Shown += new System.EventHandler(this.Form1_Shown);
            this.SetBox.ResumeLayout(false);
            this.RunBox.ResumeLayout(false);
            this.DeckBox.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox SetBox;
        private System.Windows.Forms.GroupBox RunBox;
        private System.Windows.Forms.GroupBox DeckBox;
        private System.Windows.Forms.Button btnDraw;
        private TileSetViewer PlayerDeck;
        private TileSetViewer SetViewer;
        private TileSetViewer RunViewer;
    }
}

