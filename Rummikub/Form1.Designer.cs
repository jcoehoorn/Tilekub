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
            this.SetBox = new System.Windows.Forms.GroupBox();
            this.SetView1 = new Rummikub.TileSet();
            this.SetView2 = new Rummikub.TileSet();
            this.RunBox = new System.Windows.Forms.GroupBox();
            this.RunView = new Rummikub.TileSet();
            this.DeckBox = new System.Windows.Forms.GroupBox();
            this.btnDraw = new System.Windows.Forms.Button();
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
            // SetView1
            // 
            this.SetView1.Columns = 4;
            this.SetView1.Count = 52;
            this.SetView1.Location = new System.Drawing.Point(7, 20);
            this.SetView1.Margin = new System.Windows.Forms.Padding(0);
            this.SetView1.Name = "SetView1";
            this.SetView1.Size = new System.Drawing.Size(136, 572);
            this.SetView1.TabIndex = 0;
            // 
            // SetView2
            // 
            this.SetView2.Columns = 4;
            this.SetView2.Count = 52;
            this.SetView2.Location = new System.Drawing.Point(157, 20);
            this.SetView2.Margin = new System.Windows.Forms.Padding(0);
            this.SetView2.Name = "SetView2";
            this.SetView2.Size = new System.Drawing.Size(136, 572);
            this.SetView2.TabIndex = 52;
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
            // RunView
            // 
            this.RunView.Count = 104;
            this.RunView.Location = new System.Drawing.Point(4, 13);
            this.RunView.Margin = new System.Windows.Forms.Padding(0);
            this.RunView.Name = "RunView";
            this.RunView.Size = new System.Drawing.Size(442, 352);
            this.RunView.TabIndex = 0;
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
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(776, 608);
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
    }
}

