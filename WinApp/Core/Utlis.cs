using QRCoder;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinApp.Core
{
    public static class Utlis
    {
        public static Bitmap GenerateQRCode(string message)
        {
            var gen = new QRCodeGenerator();
            var data = gen.CreateQrCode(message, QRCodeGenerator.ECCLevel.Q);
            var qrCode = new QRCode(data);
            return qrCode.GetGraphic(8);
        }
    }
}
