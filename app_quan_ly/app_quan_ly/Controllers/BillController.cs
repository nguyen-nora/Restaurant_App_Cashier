using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Printing;
using app_quan_ly.Models;

namespace app_quan_ly.Controllers
{
    public class BillController
    {
        private AppModel db;
        private PrintController printController;

        public BillController()
        {
            db = new AppModel();
            printController = new PrintController();
        }

        public void LoadBillDetails(string billId, ListView lvBill, TextBox txtSum, TextBox txtDiscount, TextBox txtTotal)
        {
            try
            {
                lvBill.Items.Clear();

                var billDetails = from dt in db.DetailThanhToans
                                join ct in db.CongThucs on dt.id_cong_thuc equals ct.id_cong_thuc
                                where dt.id_bill == billId
                                select new
                                {
                                    TenMon = ct.ten_mon,
                                    SoLuong = dt.so_luong,
                                    Gia = ct.gia_tien,
                                    TongGia = ct.gia_tien * dt.so_luong
                                };

                decimal totalSum = 0;
                foreach (var detail in billDetails)
                {
                    ListViewItem item = new ListViewItem(detail.SoLuong.ToString());
                    item.SubItems.Add(detail.TenMon);
                    item.SubItems.Add(detail.TongGia.ToString());
                    lvBill.Items.Add(item);
                    totalSum += detail.TongGia ?? 0;
                }

                txtSum.Text = totalSum.ToString();
                UpdateTotal(txtSum, txtDiscount, txtTotal);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading bill details: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void LoadAllMenuItems(ListView lvAll, ComboBox cbloaiMon)
        {
            try
            {
                // Load categories
                var categories = db.DanhMucs.Select(dm => dm.ten_loai).ToList();
                cbloaiMon.Items.Clear();
                cbloaiMon.Items.Add("Tất cả");
                cbloaiMon.Items.AddRange(categories.ToArray());

                // Load all menu items
                var menuItems = from ct in db.CongThucs
                               where ct.trang_thai == "Active"
                               select ct.ten_mon;

                lvAll.Items.Clear();
                foreach (var item in menuItems)
                {
                    ListViewItem lvItem = new ListViewItem(item);
                    lvAll.Items.Add(lvItem);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading menu items: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void SearchMenuItems(string searchText, string category, ListView lvAll)
        {
            try
            {
                var query = from ct in db.CongThucs
                           join dm in db.DanhMucs on ct.id_loai_mon equals dm.id_loai_mon
                           where ct.trang_thai == "Active"
                           select new { ct.ten_mon, dm.ten_loai };

                if (!string.IsNullOrEmpty(searchText))
                {
                    query = query.Where(x => x.ten_mon.Contains(searchText));
                }

                if (!string.IsNullOrEmpty(category) && category != "Tất cả")
                {
                    query = query.Where(x => x.ten_loai == category);
                }

                lvAll.Items.Clear();
                foreach (var item in query)
                {
                    ListViewItem lvItem = new ListViewItem(item.ten_mon);
                    lvAll.Items.Add(lvItem);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error searching menu items: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void UpdateTotal(TextBox txtSum, TextBox txtDiscount, TextBox txtTotal)
        {
            try
            {
                if (decimal.TryParse(txtSum.Text, out decimal sum) && 
                    decimal.TryParse(txtDiscount.Text, out decimal discount))
                {
                    decimal total = sum - discount;
                    txtTotal.Text = total.ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating total: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void PrintTemporaryBill(string billId, ListView lvBill, TextBox txtSum, TextBox txtDiscount, TextBox txtTotal)
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
                    e.Graphics.DrawString("BILL TẠM TÍNH", titleFont, Brushes.Black, 50, 20);
                    e.Graphics.DrawString($"Bill ID: {billId}", contentFont, Brushes.Black, 50, 50);
                    e.Graphics.DrawString($"Thời gian: {DateTime.Now:dd/MM/yyyy HH:mm:ss}", contentFont, Brushes.Black, 50, 70);
                    e.Graphics.DrawString("--------------------------------", contentFont, Brushes.Black, 50, 90);

                    // Draw column headers
                    e.Graphics.DrawString("SL", headerFont, Brushes.Black, 10, 110);
                    e.Graphics.DrawString("Tên món", headerFont, Brushes.Black, 40, 110);
                    e.Graphics.DrawString("Thành tiền", headerFont, Brushes.Black, 200, 110);

                    // Draw items
                    int yPos = 140;
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
                    // If discount is null or 0, use total sum as final amount
                    string finalAmount = string.IsNullOrEmpty(txtDiscount.Text) || decimal.Parse(txtDiscount.Text) == 0 
                        ? txtSum.Text 
                        : txtTotal.Text;
                    e.Graphics.DrawString(finalAmount + " VNĐ", totalFont, Brushes.Black, 150, yPos);

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
                MessageBox.Show($"Error printing temporary bill: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
