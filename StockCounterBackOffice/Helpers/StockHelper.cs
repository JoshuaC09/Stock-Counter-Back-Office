﻿using StockCounterBackOffice.Interfaces;
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
            string errorMessage = string.Empty;
            try
            {
                if (!File.Exists(_configFilePath))
                {
                    errorMessage = "Config file not found.";
                    throw new FileNotFoundException(errorMessage);
                }

                string encryptedContent = await File.ReadAllTextAsync(_configFilePath);
                string decryptedContent = await _securityService.DecryptAsync(encryptedContent);

                string serverValue = ConnectionStringHelper.GetConnectionStringParameter(decryptedContent, "Server");
                string portNumber = ConnectionStringHelper.GetConnectionStringParameter(decryptedContent, "PortNumber");

                if (string.IsNullOrEmpty(serverValue) || string.IsNullOrEmpty(decryptedContent))
                {
                    errorMessage = "Invalid config file content.";
                    throw new InvalidOperationException(errorMessage);
                }

                GlobalVariable.BaseAddress = ConnectionStringHelper.GetBaseAddress(serverValue, portNumber);

                bool isConnected = await _apiService.SetConnectionStringAsync(decryptedContent);

                if (!isConnected)
                {
                    errorMessage = "Unable to connect to the server.";
                    throw new HttpRequestException(errorMessage);
                }

                return isConnected;
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return false;
            }
            finally
            {
                if (!string.IsNullOrEmpty(errorMessage))
                {
                    var result = MessageBox.Show($"{errorMessage}\nDo you want to continue without connection?", "Error", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
                    if (result == DialogResult.No)
                    {
                        Application.Exit();
                    }
                }
            }
        }

        public async Task InitializeInventoryAsync()
        {
            await _apiService.InitInventoryAsync();
        }

        public async Task PostInventoryAsync()
        {
            await _apiService.PostInventoryAsync();
        }

        public async Task<List<ExportedItem>> ExportInventoryAsync()
        {
            return await _apiService.ExportInventoryAsync();
        }
    }
}
