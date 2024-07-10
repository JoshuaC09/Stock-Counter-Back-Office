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
            LoadConfigFileButton = new Button();
            label1 = new Label();
            SuspendLayout();
            // 
            // InitializeButton
            // 
            InitializeButton.BackColor = Color.Red;
            InitializeButton.BackgroundImageLayout = ImageLayout.None;
            InitializeButton.Cursor = Cursors.Hand;
            InitializeButton.Font = new Font("Tahoma", 10.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            InitializeButton.ForeColor = Color.White;
            InitializeButton.Location = new Point(414, 115);
            InitializeButton.Margin = new Padding(4, 4, 4, 4);
            InitializeButton.Name = "InitializeButton";
            InitializeButton.Size = new Size(165, 52);
            InitializeButton.TabIndex = 0;
            InitializeButton.Text = "Delete";
            InitializeButton.UseVisualStyleBackColor = false;
            InitializeButton.Click += InitializeButton_Click;
            // 
            // NewButton
            // 
            NewButton.BackColor = SystemColors.HotTrack;
            NewButton.Cursor = Cursors.Hand;
            NewButton.Font = new Font("Tahoma", 10.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            NewButton.ForeColor = Color.White;
            NewButton.Location = new Point(25, 115);
            NewButton.Margin = new Padding(4, 4, 4, 4);
            NewButton.Name = "NewButton";
            NewButton.Size = new Size(165, 52);
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
            ExportButton.Location = new Point(222, 115);
            ExportButton.Margin = new Padding(4, 4, 4, 4);
            ExportButton.Name = "ExportButton";
            ExportButton.Size = new Size(165, 52);
            ExportButton.TabIndex = 2;
            ExportButton.Text = "Export";
            ExportButton.UseVisualStyleBackColor = false;
            ExportButton.Click += ExportButton_Click;
            // 
            // LoadConfigFileButton
            // 
            LoadConfigFileButton.Cursor = Cursors.Hand;
            LoadConfigFileButton.FlatAppearance.BorderColor = Color.Gray;
            LoadConfigFileButton.Location = new Point(156, 216);
            LoadConfigFileButton.Margin = new Padding(4, 4, 4, 4);
            LoadConfigFileButton.Name = "LoadConfigFileButton";
            LoadConfigFileButton.Size = new Size(273, 55);
            LoadConfigFileButton.TabIndex = 3;
            LoadConfigFileButton.Text = "Connect to Server";
            LoadConfigFileButton.UseVisualStyleBackColor = true;
            LoadConfigFileButton.Click += LoadConfigFileButton_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Tahoma", 19.8000011F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label1.Location = new Point(121, 22);
            label1.Margin = new Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new Size(343, 41);
            label1.TabIndex = 4;
            label1.Text = "Inventory Back Office";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(11F, 21F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.GhostWhite;
            ClientSize = new Size(612, 292);
            Controls.Add(label1);
            Controls.Add(LoadConfigFileButton);
            Controls.Add(ExportButton);
            Controls.Add(NewButton);
            Controls.Add(InitializeButton);
            Font = new Font("Tahoma", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            Margin = new Padding(4, 4, 4, 4);
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Inventory Back Office";
            WindowState = FormWindowState.Minimized;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button InitializeButton;
        private Button NewButton;
        private Button ExportButton;
        private Button LoadConfigFileButton;
        private Label label1;
    }
}
