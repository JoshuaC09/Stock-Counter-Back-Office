using System.Text;
using StockCounterBackOffice.Helpers;
using StockCounterBackOffice.Interfaces;
using StockCounterBackOffice.Models;
using StockCounterBackOffice.Security;
using StockCounterBackOffice.Services;

namespace StockCounterBackOffice
{
    public partial class Form1 : Form
    {
        private readonly StockHelper _stockHelper;
        private Label loadingLabel;
        private Label statusLabel;
        private Button initializeButton;
        private Button newButton;
        private Button exportButton;

        public Form1()
        {
            InitializeComponent();

            loadingLabel = new Label()
            {
                Text = "Connecting, please wait...",
                Visible = true,
                AutoSize = true
            };
            this.Controls.Add(loadingLabel);

            statusLabel = new Label()
            {
                Text = "",
                Visible = true,
                AutoSize = true
            };
            this.Controls.Add(statusLabel);

            initializeButton = this.Controls.Find("InitializeButton", true).FirstOrDefault() as Button;
            newButton = this.Controls.Find("NewButton", true).FirstOrDefault() as Button;
            exportButton = this.Controls.Find("ExportButton", true).FirstOrDefault() as Button;

         
            SetButtonsEnabled(false);

            UpdateLabelPositions();

            string configFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.bgc");

            ISecurity securityService = new SecurityService(SecurityKeys.EncryptionKey, Encoding.UTF8.GetBytes(SecurityKeys.EncryptionSalt));

            var httpClient = new HttpClient();
            var tokenService = new TokenService(httpClient);
            ApiService apiService = new ApiService(httpClient, tokenService);

            _stockHelper = new StockHelper(apiService, securityService, configFilePath);

            LoadConfigFileAsync();
        }

        private void SetButtonsEnabled(bool enabled)
        {
            if (initializeButton != null) initializeButton.Enabled = enabled;
            if (newButton != null) newButton.Enabled = enabled;
            if (exportButton != null) exportButton.Enabled = enabled;
        }

        private void UpdateLabelPositions()
        {
            int centerX = this.ClientSize.Width / 2;

            int loadingLabelX = centerX - (loadingLabel.Width / 2);
            int loadingLabelY = 10; 
            loadingLabel.Location = new Point(loadingLabelX, loadingLabelY);

            int statusLabelX = centerX - (statusLabel.Width / 2);
            int statusLabelY = loadingLabelY + loadingLabel.Height + 10; 
            statusLabel.Location = new Point(statusLabelX, statusLabelY);
        }

        private async void LoadConfigFileAsync()
        {
            SetButtonsEnabled(false); 
            NewButton.BackColor = System.Drawing.ColorTranslator.FromHtml("#3084d6");
            ExportButton.BackColor = System.Drawing.ColorTranslator.FromHtml("#60c690");
            bool isConnected = await _stockHelper.LoadConfigFileAsync();
            loadingLabel.Visible = false;

            if (!isConnected)
            {
                statusLabel.Text = "Not connected to any server.";
                statusLabel.ForeColor = System.Drawing.Color.Red;
                SetButtonsEnabled(false); 
            }
            else
            {
                statusLabel.Text = "Connected to the server.";
                statusLabel.ForeColor = System.Drawing.Color.Green;
                NewButton.BackColor = System.Drawing.ColorTranslator.FromHtml("#0066cc");
                ExportButton.BackColor = System.Drawing.ColorTranslator.FromHtml("#00a24d");
                SetButtonsEnabled(true);
            }

            UpdateLabelPositions();
        }

        private async void InitializeButton_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Are you sure you want to delete all inventory data?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                try
                {
                    await _stockHelper.InitializeInventoryAsync();
                    MessageBox.Show("Inventory data deleted successfully.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred while deleting the inventory:\n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private async void NewButton_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Are you sure you want to post new inventory data?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                try
                {
                    await _stockHelper.PostInventoryAsync();
                    MessageBox.Show("Inventory posted successfully.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred while posting the inventory:\n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private async void ExportButton_Click(object sender, EventArgs e)
        {
            try
            {
                var exportedItems = await _stockHelper.ExportInventoryAsync();
                if (exportedItems == null || !exportedItems.Any())
                {
                    MessageBox.Show("No data to export.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                string currentDate = DateTime.Now.ToString("yyyy-MM-dd");

                using (var sfd = new SaveFileDialog()
                {
                    Filter = "Excel Workbook|*.xlsx",
                    FileName = $"Stock_Count_{currentDate}.xlsx" 
                })
                {
                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        ExportHelper.ExportToExcel(exportedItems, sfd.FileName);
                        MessageBox.Show("Inventory exported successfully.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while exporting the inventory:\n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
