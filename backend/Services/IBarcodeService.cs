namespace whm.Services
{
    public interface IBarcodeService
    {
        string GenerateBarcodeValue();

        byte[] GenerateBarcode(string barcode);
    }
}