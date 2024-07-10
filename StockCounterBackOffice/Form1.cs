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

        public Form1()
        {
            InitializeComponent();
            DisableOtherButtons();

            // Initialize the loadingLabel
            loadingLabel = new Label()
            {
                Text = "Loading, please wait...",
                Visible = false,
                AutoSize = true
            };
            this.Controls.Add(loadingLabel);

            // Set the loadingLabel position
            UpdateLoadingLabelPosition();

            string configFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.bgc");

            ISecurity securityService = new SecurityService(SecurityKeys.EncryptionKey, Encoding.UTF8.GetBytes(SecurityKeys.EncryptionSalt));

            var httpClient = new HttpClient();
            var tokenService = new TokenService(httpClient);
            ApiService apiService = new ApiService(httpClient, tokenService);

            _stockHelper = new StockHelper(apiService, securityService, configFilePath);
        }

        private void UpdateLoadingLabelPosition()
        {
            int buttonCenterX = LoadConfigFileButton.Location.X + (LoadConfigFileButton.Width / 2);
            int labelX = buttonCenterX - (loadingLabel.Width / 2);
            int labelY = LoadConfigFileButton.Location.Y + LoadConfigFileButton.Height + 10;

            loadingLabel.Location = new Point(labelX, labelY);
        }

        private async void LoadConfigFileButton_Click(object sender, EventArgs e)
        {
            LoadConfigFileButton.Enabled = false;

            loadingLabel.Visible = true;
            UpdateLoadingLabelPosition(); 

            bool isConnected = await _stockHelper.LoadConfigFileAsync();

            loadingLabel.Visible = false;

            if (isConnected)
            {
                EnableOtherButtons();
                LoadConfigFileButton.Visible = false;
            }
            else
            {
       
                LoadConfigFileButton.Enabled = true;
            }
        }

        private async void InitializeButton_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Are you sure you want to initialize the inventory?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                try
                {
                    await _stockHelper.InitializeInventoryAsync();
                    MessageBox.Show("Inventory initialized successfully.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred while initializing the inventory:\n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                            worksheet.Cells["A1"].LoadFromCollection(exportedItems, true);

                         
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


        private void DisableOtherButtons()
        {
            InitializeButton.Enabled = false;
            NewButton.Enabled = false;
            ExportButton.Enabled = false;
        }

        private void EnableOtherButtons()
        {
            InitializeButton.Enabled = true;
            NewButton.Enabled = true;
            ExportButton.Enabled = true;
        }
    }
}
