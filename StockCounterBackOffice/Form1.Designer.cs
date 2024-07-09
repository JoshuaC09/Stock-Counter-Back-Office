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
            SuspendLayout();
            // 
            // InitializeButton
            // 
            InitializeButton.Location = new Point(110, 118);
            InitializeButton.Name = "InitializeButton";
            InitializeButton.Size = new Size(94, 29);
            InitializeButton.TabIndex = 0;
            InitializeButton.Text = "Initialize";
            InitializeButton.UseVisualStyleBackColor = true;
            InitializeButton.Click += InitializeButton_Click;
            // 
            // NewButton
            // 
            NewButton.Location = new Point(257, 118);
            NewButton.Name = "NewButton";
            NewButton.Size = new Size(94, 29);
            NewButton.TabIndex = 1;
            NewButton.Text = "New";
            NewButton.UseVisualStyleBackColor = true;
            NewButton.Click += NewButton_Click;
            // 
            // ExportButton
            // 
            ExportButton.Location = new Point(388, 118);
            ExportButton.Name = "ExportButton";
            ExportButton.Size = new Size(94, 29);
            ExportButton.TabIndex = 2;
            ExportButton.Text = "Export";
            ExportButton.UseVisualStyleBackColor = true;
            ExportButton.Click += ExportButton_Click;
            // 
            // LoadConfigFileButton
            // 
            LoadConfigFileButton.Location = new Point(202, 191);
            LoadConfigFileButton.Name = "LoadConfigFileButton";
            LoadConfigFileButton.Size = new Size(198, 52);
            LoadConfigFileButton.TabIndex = 3;
            LoadConfigFileButton.Text = "Connect to Server";
            LoadConfigFileButton.UseVisualStyleBackColor = true;
            LoadConfigFileButton.Click += LoadConfigFileButton_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(602, 330);
            Controls.Add(LoadConfigFileButton);
            Controls.Add(ExportButton);
            Controls.Add(NewButton);
            Controls.Add(InitializeButton);
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            ResumeLayout(false);
        }

        #endregion

        private Button InitializeButton;
        private Button NewButton;
        private Button ExportButton;
        private Button LoadConfigFileButton;
    }
}
