using System.Text;
using OfficeOpenXml;
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

            // Disable buttons initially
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
            int loadingLabelY = 10; // Adjust the Y position as needed for the upper center
            loadingLabel.Location = new Point(loadingLabelX, loadingLabelY);

            int statusLabelX = centerX - (statusLabel.Width / 2);
            int statusLabelY = loadingLabelY + loadingLabel.Height + 10; // Adjust the Y position as needed
            statusLabel.Location = new Point(statusLabelX, statusLabelY);
        }

        private async void LoadConfigFileAsync()
        {
            SetButtonsEnabled(false); // Disable buttons while loading
            bool isConnected = await _stockHelper.LoadConfigFileAsync();
            loadingLabel.Visible = false;

            if (!isConnected)
            {
                statusLabel.Text = "Not connected to any server.";
                statusLabel.ForeColor = System.Drawing.Color.Red;
                SetButtonsEnabled(false); // Ensure buttons are disabled if not connected
            }
            else
            {
                statusLabel.Text = "Connected to the server.";
                statusLabel.ForeColor = System.Drawing.Color.Green;
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
                if (exportedItems == null)
                {
                    MessageBox.Show("No data to export.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                using (var sfd = new SaveFileDialog() { Filter = "Excel Workbook|*.xlsx" })
                {
                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                        using (var package = new OfficeOpenXml.ExcelPackage())
                        {
                            var worksheet = package.Workbook.Worksheets.Add("Inventory");

                            var headers = new string[]
                            {
                                "Item No", "Item User Define", "Barcode", "Description",
                                "BUOM", "Stocks(Pcs)", "Lot #", "Expiration",
                                "Variance", "Rack", "CFactor", "Cntr"
                            };

                            for (int i = 0; i < headers.Length; i++)
                            {
                                worksheet.Cells[1, i + 1].Value = headers[i];
                            }

                            for (int i = 0; i < exportedItems.Count; i++)
                            {
                                worksheet.Cells[i + 2, 1].Value = exportedItems[i].ItemNo;
                                worksheet.Cells[i + 2, 2].Value = exportedItems[i].ItemUserDefine;
                                worksheet.Cells[i + 2, 3].Value = exportedItems[i].Barcode;
                                worksheet.Cells[i + 2, 4].Value = exportedItems[i].Description;
                                worksheet.Cells[i + 2, 5].Value = exportedItems[i].BUOM;
                                worksheet.Cells[i + 2, 6].Value = exportedItems[i].Stocks;
                                worksheet.Cells[i + 2, 7].Value = exportedItems[i].Lot;
                                worksheet.Cells[i + 2, 8].Value = exportedItems[i].Expiration;
                                worksheet.Cells[i + 2, 9].Value = exportedItems[i].Variance;
                                worksheet.Cells[i + 2, 10].Value = exportedItems[i].Rack;
                                worksheet.Cells[i + 2, 11].Value = exportedItems[i].CFactor;
                                worksheet.Cells[i + 2, 12].Value = exportedItems[i].Counter;
                            }

                            using (var range = worksheet.Cells["A1:L1"])
                            {
                                range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Yellow);
                            }

                            package.SaveAs(new FileInfo(sfd.FileName));
                        }
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