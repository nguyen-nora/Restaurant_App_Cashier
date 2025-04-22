using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using app_quan_ly.Models;

namespace app_quan_ly.Controllers
{
    public class KitchenPrintMonitor : IDisposable
    {
        private readonly AppModel db;
        private readonly PrintController printController;
        private readonly System.Windows.Forms.Timer pollTimer;
        private readonly HashSet<int> processedDetailIds;
        private bool isDisposed;

        public KitchenPrintMonitor()
        {
            db = new AppModel();
            printController = new PrintController();
            processedDetailIds = new HashSet<int>();
            
            // Initialize timer to poll every 5 seconds
            pollTimer = new System.Windows.Forms.Timer();
            pollTimer.Interval = 5000; // 5 seconds
            pollTimer.Tick += PollTimer_Tick;
            pollTimer.Start();
        }

        private void PollTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                var kitchenPrinter = printController.GetKitchenPrinter();
                if (kitchenPrinter == null)
                {
                    return; // No kitchen printer configured
                }

                // Get new DetailThanhToan records that haven't been processed and have trang_thai = "No"
                var newDetails = db.DetailThanhToans
                    .Where(d => !processedDetailIds.Contains(d.id_detail) && d.trang_thai == "No")
                    .ToList();

                // Group details by bill ID and time
                var groupedDetails = newDetails
                    .GroupBy(d => new { d.id_bill, d.thoi_gian })
                    .OrderBy(g => g.Key.thoi_gian);

                foreach (var group in groupedDetails)
                {
                    PrintKitchenOrder(group.ToList(), kitchenPrinter);
                    
                    // Update trang_thai to "Yes" for all printed details
                    foreach (var detail in group)
                    {
                        detail.trang_thai = "Yes";
                        processedDetailIds.Add(detail.id_detail);
                    }
                    
                    // Save changes to database
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                // Log error but don't throw to keep timer running
                MessageBox.Show($"Error in kitchen print monitor: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void PrintKitchenOrder(List<DetailThanhToan> details, MayIn printer)
        {
            if (!details.Any()) return;

            var firstDetail = details.First();
            var bill = db.ThanhToans.FirstOrDefault(t => t.id_bill == firstDetail.id_bill);
            if (bill == null) return;

            PrintDocument pd = new PrintDocument();
            pd.PrinterSettings.PrinterName = printer.ten_may_in;

            pd.PrintPage += (sender, e) =>
            {
                // Create fonts
                Font titleFont = new Font("Arial", 16, FontStyle.Bold);
                Font headerFont = new Font("Arial", 12, FontStyle.Bold);
                Font contentFont = new Font("Arial", 10);

                int yPos = 20;
                int leftMargin = 50;
                int lineHeight = 25;

                // Draw header
                e.Graphics.DrawString("KITCHEN ORDER", titleFont, Brushes.Black, leftMargin, yPos);
                yPos += lineHeight + 10;

                e.Graphics.DrawString($"Bill ID: {firstDetail.id_bill}", contentFont, Brushes.Black, leftMargin, yPos);
                yPos += lineHeight;

                e.Graphics.DrawString($"Time: {firstDetail.thoi_gian:dd/MM/yyyy HH:mm:ss}", contentFont, Brushes.Black, leftMargin, yPos);
                yPos += lineHeight;

                e.Graphics.DrawString("--------------------------------", contentFont, Brushes.Black, leftMargin, yPos);
                yPos += lineHeight;

                // Draw order details
                e.Graphics.DrawString("ORDER DETAILS:", headerFont, Brushes.Black, leftMargin, yPos);
                yPos += lineHeight + 5;

                foreach (var detail in details)
                {
                    var recipe = db.CongThucs.FirstOrDefault(c => c.id_cong_thuc == detail.id_cong_thuc);
                    if (recipe == null) continue;

                    // Item name and quantity
                    e.Graphics.DrawString($"{recipe.ten_mon} x{detail.so_luong}", contentFont, Brushes.Black, leftMargin + 20, yPos);
                    yPos += lineHeight;

                    // Special notes if any
                    if (!string.IsNullOrEmpty(detail.ghi_chu))
                    {
                        e.Graphics.DrawString($"Note: {detail.ghi_chu}", contentFont, Brushes.Black, leftMargin + 40, yPos);
                        yPos += lineHeight;
                    }
                }

                // Draw footer
                yPos += 10;
                e.Graphics.DrawString("--------------------------------", contentFont, Brushes.Black, leftMargin, yPos);
                yPos += lineHeight;
                e.Graphics.DrawString("Please prepare this order", contentFont, Brushes.Black, leftMargin + 20, yPos);
            };

            pd.Print();
        }

        public void Dispose()
        {
            if (!isDisposed)
            {
                pollTimer?.Stop();
                pollTimer?.Dispose();
                db?.Dispose();
                isDisposed = true;
            }
        }
    }
} 