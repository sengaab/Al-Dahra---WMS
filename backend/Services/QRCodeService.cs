using QRCoder;

namespace whm.Services
{
    public class QRCodeService : IQRCodeService
    {
        public byte[] GenerateQRCode(string value)
        {
            using var qrGenerator = new QRCodeGenerator();

            using var qrCodeData =
                qrGenerator.CreateQrCode(
                    value,
                    QRCodeGenerator.ECCLevel.Q
                );

            var qrCode =
                new PngByteQRCode(qrCodeData);

            return qrCode.GetGraphic(20);
        }
    }
}