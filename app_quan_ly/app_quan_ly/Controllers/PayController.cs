using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Printing;
using app_quan_ly.Models;

namespace app_quan_ly.Controllers
{
    public class PayController
    {
        private AppModel db;
        private PrintController printController;

        public PayController()
        {
            db = new AppModel();
            printController = new PrintController();
        }

        public void ProcessPayment(string billId, decimal amountPaid, decimal totalAmount, string paymentMethod)
        {
            try
            {
                using (var transaction = db.Database.BeginTransaction())
                {
                    // Update the payment record
                    var payment = db.ThanhToans.FirstOrDefault(p => p.id_bill == billId);
                    if (payment != null)
                    {
                        payment.loai_thanh_toan = paymentMethod;
                        payment.thoi_gian_bill = DateTime.Now;
                        payment.tong_tien = totalAmount;
                    }

                    db.SaveChanges();
                    transaction.Commit();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error processing payment: {ex.Message}");
            }
        }

        public void PrintPaymentReceipt(string billId, ListView lvBill, TextBox txtSum, TextBox txtDiscount, 
            TextBox txtTotal, TextBox txtMoneyCus, TextBox txtMoneyGCus, string paymentMethod)
        {
            try
            {
                var cashierPrinter = printController.GetCashierPrinter();
                if (cashierPrinter == null)
                {
                    MessageBox.Show("No printer configured for cashier!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                PrintDocument pd = new PrintDocument();
                pd.PrinterSettings.PrinterName = cashierPrinter.ten_may_in;

                pd.PrintPage += (sender, e) =>
                {
                    // Create fonts
                    Font titleFont = new Font("Arial", 16, FontStyle.Bold);
                    Font headerFont = new Font("Arial", 10, FontStyle.Bold);
                    Font contentFont = new Font("Arial", 9);
                    Font totalFont = new Font("Arial", 10, FontStyle.Bold);

                    // Draw header
                    e.Graphics.DrawString("BILL THANH TOÁN", titleFont, Brushes.Black, 50, 20);
                    e.Graphics.DrawString($"Bill ID: {billId}", contentFont, Brushes.Black, 50, 50);
                    e.Graphics.DrawString($"Thời gian: {DateTime.Now:dd/MM/yyyy HH:mm:ss}", contentFont, Brushes.Black, 50, 70);
                    e.Graphics.DrawString($"Phương thức thanh toán: {paymentMethod}", contentFont, Brushes.Black, 50, 90);
                    e.Graphics.DrawString("--------------------------------", contentFont, Brushes.Black, 50, 110);

                    // Draw column headers
                    e.Graphics.DrawString("SL", headerFont, Brushes.Black, 10, 130);
                    e.Graphics.DrawString("Tên món", headerFont, Brushes.Black, 40, 130);
                    e.Graphics.DrawString("Thành tiền", headerFont, Brushes.Black, 200, 130);

                    // Draw items
                    int yPos = 160;
                    foreach (ListViewItem item in lvBill.Items)
                    {
                        e.Graphics.DrawString(item.SubItems[0].Text, contentFont, Brushes.Black, 10, yPos);
                        e.Graphics.DrawString(item.SubItems[1].Text, contentFont, Brushes.Black, 40, yPos);
                        e.Graphics.DrawString(item.SubItems[2].Text, contentFont, Brushes.Black, 200, yPos);
                        yPos += 25;
                    }

                    // Draw total section
                    yPos += 10;
                    e.Graphics.DrawString("--------------------------------", contentFont, Brushes.Black, 50, yPos);
                    yPos += 20;
                    e.Graphics.DrawString("Tổng cộng:", totalFont, Brushes.Black, 50, yPos);
                    e.Graphics.DrawString(txtSum.Text + " VNĐ", totalFont, Brushes.Black, 150, yPos);
                    
                    if (!string.IsNullOrEmpty(txtDiscount.Text) && decimal.Parse(txtDiscount.Text) > 0)
                    {
                        yPos += 25;
                        e.Graphics.DrawString("Giảm giá:", contentFont, Brushes.Black, 50, yPos);
                        e.Graphics.DrawString(txtDiscount.Text + " VNĐ", contentFont, Brushes.Black, 150, yPos);
                    }

                    yPos += 25;
                    e.Graphics.DrawString("Thành tiền:", totalFont, Brushes.Black, 50, yPos);
                    e.Graphics.DrawString(txtTotal.Text + " VNĐ", totalFont, Brushes.Black, 150, yPos);

                    // Draw payment information
                    yPos += 40;
                    e.Graphics.DrawString("--------------------------------", contentFont, Brushes.Black, 50, yPos);
                    yPos += 20;
                    e.Graphics.DrawString("Tiền khách đưa:", contentFont, Brushes.Black, 50, yPos);
                    e.Graphics.DrawString(txtMoneyCus.Text + " VNĐ", contentFont, Brushes.Black, 150, yPos);
                    
                    yPos += 25;
                    e.Graphics.DrawString("Tiền trả khách:", contentFont, Brushes.Black, 50, yPos);
                    e.Graphics.DrawString(txtMoneyGCus.Text + " VNĐ", contentFont, Brushes.Black, 150, yPos);

                    // Draw footer
                    yPos += 40;
                    e.Graphics.DrawString("--------------------------------", contentFont, Brushes.Black, 50, yPos);
                    yPos += 20;
                    e.Graphics.DrawString("Cảm ơn quý khách!", contentFont, Brushes.Black, 70, yPos);
                };

                pd.Print();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error printing receipt: {ex.Message}");
            }
        }

        public void Dispose()
        {
            if (db != null)
            {
                db.Dispose();
            }
        }
    }
}
