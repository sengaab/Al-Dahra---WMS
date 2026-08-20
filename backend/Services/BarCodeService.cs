using System.Drawing;
using System.Drawing.Imaging;
using ZXing;
using ZXing.Common;
using ZXing.Rendering;
using ZXing.Windows.Compatibility;

namespace whm.Services
{
    public class BarcodeService : IBarcodeService
    {
        // Generate a unique barcode value
        public string GenerateBarcodeValue()
        {
            return $"890{Random.Shared.Next(100000000, 999999999)}";
        }


        // Generate barcode image
        public byte[] GenerateBarcode(string barcode)
        {
            if (string.IsNullOrWhiteSpace(barcode))
            {
                throw new ArgumentException(
                    "Barcode cannot be empty."
                );
            }

            var writer = new BarcodeWriter<Bitmap>
            {
                Format = BarcodeFormat.CODE_128,

                Options = new EncodingOptions
                {
                    Width = 600,
                    Height = 200,
                    Margin = 20,
                    PureBarcode = false
                },

                Renderer = new BitmapRenderer()
            };

            using var bitmap = writer.Write(barcode);

            using var stream = new MemoryStream();

            bitmap.Save(
                stream,
                ImageFormat.Png
            );

            return stream.ToArray();
        }
    }
}