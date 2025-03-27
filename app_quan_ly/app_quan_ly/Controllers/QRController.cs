using System;
using System.Drawing;
using QRCoder;
using app_quan_ly.Models;
using System.Linq;

namespace app_quan_ly.Controllers
{
    public class QRController
    {
        private static AppModel db = new AppModel();

        public static Bitmap GenerateQRCode(string data)
        {
            using (QRCodeGenerator qrGenerator = new QRCodeGenerator())
            using (QRCodeData qrCodeData = qrGenerator.CreateQrCode(data, QRCodeGenerator.ECCLevel.Q))
            using (QRCode qrCode = new QRCode(qrCodeData))
            {
                return qrCode.GetGraphic(20);
            }
        }

        public static string GenerateQRData(string maBan, string maNhanVien)
        {
            string currentTime = DateTime.Now.ToString("ddMMyyHHmm");
            return $"{currentTime}{maBan}{maNhanVien}";
        }

        public static Bitmap GenerateTableQRCode(string maBan, string maNhanVien)
        {
            string qrData = GenerateQRData(maBan, maNhanVien);
            return GenerateQRCode(qrData);
        }

        public static Bitmap ScaleQRCode(Bitmap qrCode, int width, int height)
        {
            Bitmap scaledQR = new Bitmap(width, height);
            using (Graphics g = Graphics.FromImage(scaledQR))
            {
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.DrawImage(qrCode, 0, 0, width, height);
            }
            return scaledQR;
        }

        public static void SaveQRToDatabase(string maBan, string maNhanVien, DateTime thoiGianVao)
        {
            try
            {
                // Get the table ID from ma_ban
                var ban = db.Bans.FirstOrDefault(b => b.ma_ban == maBan);
                if (ban == null)
                {
                    throw new Exception($"Table with code {maBan} not found");
                }

                // Generate QR code string
                string maQR = GenerateQRData(maBan, maNhanVien);

                // Create new QR record
                var qr = new QR
                {
                    ma_qr = maQR,
                    ma_nv = maNhanVien,
                    thoi_gian_vao = thoiGianVao,
                    id_ban = ban.id_ban
                };

                // Get next available ID
                int nextId = 1;
                if (db.QRs.Any())
                {
                    nextId = db.QRs.Max(q => q.id_qr) + 1;
                }
                qr.id_qr = nextId;

                // Add and save to database
                db.QRs.Add(qr);
                db.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
