namespace StockCounterBackOffice
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            InitializeButton = new Button();
            NewButton = new Button();
            ExportButton = new Button();
            SuspendLayout();
            // 
            // InitializeButton
            // 
            InitializeButton.BackColor = Color.Transparent;
            InitializeButton.BackgroundImageLayout = ImageLayout.None;
            InitializeButton.Cursor = Cursors.Hand;
            InitializeButton.Font = new Font("Tahoma", 10.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            InitializeButton.ForeColor = Color.Black;
            InitializeButton.Location = new Point(485, 267);
            InitializeButton.Margin = new Padding(4);
            InitializeButton.Name = "InitializeButton";
            InitializeButton.Size = new Size(103, 34);
            InitializeButton.TabIndex = 0;
            InitializeButton.Text = "Initialize";
            InitializeButton.UseVisualStyleBackColor = false;
            InitializeButton.Click += InitializeButton_Click;
            // 
            // NewButton
            // 
            NewButton.BackColor = SystemColors.HotTrack;
            NewButton.Cursor = Cursors.Hand;
            NewButton.Font = new Font("Tahoma", 10.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            NewButton.ForeColor = Color.White;
            NewButton.Location = new Point(107, 75);
            NewButton.Margin = new Padding(4);
            NewButton.Name = "NewButton";
            NewButton.Size = new Size(179, 81);
            NewButton.TabIndex = 1;
            NewButton.Text = "New";
            NewButton.UseVisualStyleBackColor = false;
            NewButton.Click += NewButton_Click;
            // 
            // ExportButton
            // 
            ExportButton.BackColor = Color.FromArgb(0, 162, 77);
            ExportButton.Cursor = Cursors.Hand;
            ExportButton.Font = new Font("Tahoma", 10.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            ExportButton.ForeColor = Color.White;
            ExportButton.Location = new Point(320, 75);
            ExportButton.Margin = new Padding(4);
            ExportButton.Name = "ExportButton";
            ExportButton.Size = new Size(179, 81);
            ExportButton.TabIndex = 2;
            ExportButton.Text = "Export";
            ExportButton.UseVisualStyleBackColor = false;
            ExportButton.Click += ExportButton_Click;
            // 
            // Form1
            // 
            AllowDrop = true;
            AutoScaleDimensions = new SizeF(11F, 21F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.GhostWhite;
            ClientSize = new Size(612, 326);
            Controls.Add(ExportButton);
            Controls.Add(NewButton);
            Controls.Add(InitializeButton);
            Font = new Font("Tahoma", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            ImeMode = ImeMode.Off;
            Margin = new Padding(4);
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Inventory Stock Count";
            ResumeLayout(false);
        }

        #endregion

        private Button InitializeButton;
        private Button NewButton;
        private Button ExportButton;
    }
}
