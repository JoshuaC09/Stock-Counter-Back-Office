using StockCounterBackOffice.Interfaces;
using StockCounterBackOffice.Security;
using StockCounterBackOffice.Models;
using StockCounterBackOffice.Helpers;
using System.Text;
using StockCounterBackOffice.Services;

namespace StockCounterBackOffice
{
    public partial class Form1 : Form
    {
        private readonly StockHelper _stockHelper;

        public Form1()
        {
            InitializeComponent();
            DisableOtherButtons();
            string configFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.bgc");

            ISecurity securityService = new SecurityService(SecurityKeys.EncryptionKey, Encoding.UTF8.GetBytes(SecurityKeys.EncryptionSalt));

        
            var httpClient = new HttpClient();
            var tokenService = new TokenService(httpClient);
            ApiService apiService = new ApiService(httpClient, tokenService);

            _stockHelper = new StockHelper(apiService, securityService, configFilePath);
        }

        private async void LoadConfigFileButton_Click(object sender, EventArgs e)
        {
            bool isConnected = await _stockHelper.LoadConfigFileAsync();
            LoadConfigFileButton.Visible = !isConnected;
            if (isConnected)
            {
                EnableOtherButtons();
            }
        }

        private async void InitializeButton_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Are you sure you want to initialize the inventory?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
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

        private void ExportButton_Click(object sender, EventArgs e)
        {

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
