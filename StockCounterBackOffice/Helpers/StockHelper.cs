using StockCounterBackOffice.Interfaces;
using StockCounterBackOffice.Models;
using StockCounterBackOffice.Services;

namespace StockCounterBackOffice.Helpers
{
    internal class StockHelper
    {
        private readonly ApiService _apiService;
        private readonly ISecurity _securityService;
        private readonly string _configFilePath;

        public StockHelper(ApiService apiService, ISecurity securityService, string configFilePath)
        {
            _apiService = apiService ?? throw new ArgumentNullException(nameof(apiService));
            _securityService = securityService ?? throw new ArgumentNullException(nameof(securityService));
            _configFilePath = configFilePath ?? throw new ArgumentNullException(nameof(configFilePath));
        }

        public async Task<bool> LoadConfigFileAsync()
        {
            try
            {
                if (!File.Exists(_configFilePath))
                {
                    MessageBox.Show("Config file not found. Please make sure the file is in the correct location.", "File Not Found", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }

                string encryptedContent;
                try
                {
                    encryptedContent = await File.ReadAllTextAsync(_configFilePath);
                }
                catch (IOException)
                {
                    MessageBox.Show("Unable to read the config file. Please check if the file is accessible.", "File Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }

                string decryptedContent;
                try
                {
                    decryptedContent = await _securityService.DecryptAsync(encryptedContent);
                }
                catch (Exception)
                {
                    MessageBox.Show("Failed to process the config file. Please ensure the file is correct.", "File Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }

                string serverValue = ConnectionStringHelper.GetServerValue(decryptedContent);
                string portNumber = ConnectionStringHelper.GetPortNumber(decryptedContent);

                if (string.IsNullOrEmpty(serverValue) || string.IsNullOrEmpty(decryptedContent))
                {
                    MessageBox.Show("Invalid config file content. Please check the file.", "File Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }


                GlobalVariable.BaseAddress = GetBaseAddress(serverValue,portNumber);

                bool isConnected;
                try
                {
                    isConnected = await _apiService.SetConnectionStringAsync(decryptedContent);
                }
                catch (HttpRequestException)
                {
                    MessageBox.Show("Cannot connect to the server. Please check your network connection.", "Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
                catch (Exception)
                {
                    MessageBox.Show("Unable to connect to the server. Please try again.", "Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }

                if (!isConnected)
                {
                    MessageBox.Show("Unable to connect to the server. Please verify the config file.", "Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                return isConnected;
            }
            catch (Exception)
            {
                MessageBox.Show("An unexpected error occurred. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        public static Uri GetBaseAddress(string serverName, string portNumber)
        {
            try
            {
                string serverTemp = $"http://{serverName}:{portNumber}/";
                return new Uri(serverTemp);
            }
            catch (UriFormatException)
            {
                MessageBox.Show("Invalid server address. Please check the config file.", "Configuration Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return null;
            }
        }

        public async Task InitializeInventoryAsync()
        {
            try
            {
                await _apiService.InitInventoryAsync();
            }
            catch (Exception)
            {
                MessageBox.Show("Unable to initialize inventory. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        public async Task PostInventoryAsync()
        {
            try
            {
                await _apiService.PostInventoryAsync();
            }
            catch (Exception)
            {
                MessageBox.Show("Unable to update inventory. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
