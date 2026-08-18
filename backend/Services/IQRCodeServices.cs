namespace whm.Services
{
    public interface IQRCodeService
    {
        byte[] GenerateQRCode(string value);
    }
}