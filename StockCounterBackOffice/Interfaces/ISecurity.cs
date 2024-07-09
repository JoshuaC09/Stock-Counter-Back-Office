namespace StockCounterBackOffice.Interfaces
{
    public interface ISecurity
    {
        byte[] DeriveKeyFromPassword(string ConnString, byte[] salt);
        Task<string> DecryptAsync(string cipherText);
    }
}
