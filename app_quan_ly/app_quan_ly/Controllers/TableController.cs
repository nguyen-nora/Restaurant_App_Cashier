using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Printing;
using app_quan_ly.Models;
using QRCoder;

namespace app_quan_ly.Controllers
{
    public class TableController
    {
        private AppModel db;
        private Dictionary<string, Color> statusColors;
        private List<Button> tableButtons;
        private string maNhanVien;
        private PrintController printController;

        public TableController(string maNhanVien)
        {
            this.maNhanVien = maNhanVien;
            db = new AppModel();
            printController = new PrintController();
            InitializeStatusColors();
            tableButtons = new List<Button>();
        }

        private void InitializeStatusColors()
        {
            statusColors = new Dictionary<string, Color>();
            statusColors.Add("Pending", Color.Gray);
            statusColors.Add("Processing", Color.RoyalBlue);
            //statusColors.Add("Completed", Color.Red);
        }

        public void ClearTableButtons(Form form)
        {
            foreach (var btn in tableButtons)
            {
                form.Controls.Remove(btn);
                btn.Dispose();
            }
            tableButtons.Clear();
        }

        public void LoadTables(Form form, string khuVuc)
        {
            try
            {
                ClearTableButtons(form);

                var query = from b in db.Bans
                           join kv in db.KhuVucs on b.id_khu_vuc equals kv.id_khu_vuc into kvInfo
                           from kvi in kvInfo.DefaultIfEmpty()
                           select new
                           {
                               b.id_ban,
                               b.ma_ban,
                               KhuVucId = b.id_khu_vuc,
                               TrangThai = b.trang_thai,
                               KhuVuc = kvi != null ? kvi.khu_vuc : ""
                           };

                if (!string.IsNullOrEmpty(khuVuc))
                {
                    query = query.Where(t => t.KhuVuc == khuVuc);
                }

                var tables = query.ToList();
                foreach (var table in tables)
                {
                    CreateTableButton(form, table.id_ban, table.ma_ban ?? "", table.KhuVucId?.ToString() ?? "", table.TrangThai ?? "Trống");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading tables: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CreateTableButton(Form form, int id, string maTable, string khuVucId, string trangThai)
        {
            Button btn = new Button();
            btn.Name = $"Table_{id}";
            btn.Text = maTable;
            btn.Size = new Size(132, 63);
            btn.Font = new Font("Microsoft Sans Serif", 10.8f, FontStyle.Regular);
            btn.ForeColor = Color.White;        
            btn.BackColor = statusColors.ContainsKey(trangThai) ? statusColors[trangThai] : Color.Gray;

            int baseX = 61;
            int baseY = 231;
            int spacing = 20;
            int buttonsPerRow = 3;

            int row = (tableButtons.Count / buttonsPerRow);
            int col = (tableButtons.Count % buttonsPerRow);

            btn.Location = new Point(baseX + (btn.Width + spacing) * col, 
                                   baseY + (btn.Height + spacing) * row);

            btn.Click += TableButton_Click;
            form.Controls.Add(btn);
            tableButtons.Add(btn);
        }

        private void TableButton_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            if (btn != null)
            {
                int tableId = int.Parse(btn.Name.Split('_')[1]);
                string maBan = btn.Text;
                
                var table = db.Bans.FirstOrDefault(b => b.ma_ban == maBan);
                if (table != null)
                {
                    if (table.trang_thai == "Processing")
                    {
                        DialogResult result = MessageBox.Show("Pay this table?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (result == DialogResult.Yes)
                        {
                            var latestQR = GetLatestQRForTable(tableId);
                            if (latestQR != null)
                            {
                                var bill = latestQR.ThanhToans.FirstOrDefault();
                                if (bill != null)
                                {
                                    //MessageBox.Show($"Opening bill: {bill.id_bill}", "Debug", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    Xacnhanbill xacNhanBill = new Xacnhanbill(bill.id_bill);
                                    xacNhanBill.Show();
                                }
                                else
                                {
                                    MessageBox.Show("No bill found for this table", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }
                            else
                            {
                                MessageBox.Show("No QR found for this table", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                    else
                    {
                        DialogResult result = MessageBox.Show("Create New Table?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (result == DialogResult.Yes)
                        {
                            try
                            {
                                DateTime thoiGianVao = DateTime.Now;
                                Bitmap qrCodeImage = QRController.GenerateTableQRCode(maBan, maNhanVien);
                                PrintTableInfo(qrCodeImage, maBan, thoiGianVao);
                                QRController.SaveQRToDatabase(maBan, maNhanVien, thoiGianVao);

                                table.trang_thai = "Processing";
                                db.SaveChanges();

                                MessageBox.Show("QR code printed and saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show($"Error creating table: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                }
            }
        }

        private QR GetLatestQRForTable(int tableId)
        {
            return db.QRs
                .Where(q => q.id_ban == tableId)
                .OrderByDescending(q => q.thoi_gian_vao)
                .FirstOrDefault();
        }

        private void PrintTableInfo(Bitmap qrCode, string maBan, DateTime thoiGianVao)
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

                Bitmap printBitmap = new Bitmap(400, 600);
                using (Graphics g = Graphics.FromImage(printBitmap))
                {
                    g.Clear(Color.White);

                    using (Font headerFont = new Font("Arial", 16, FontStyle.Bold))
                    {
                        g.DrawString("RESTAURANT QR CODE", headerFont, Brushes.Black, 30, 20);
                    }

                    using (Font infoFont = new Font("Arial", 12))
                    {
                        g.DrawString($"Table: {maBan}", infoFont, Brushes.Black, 20, 60);
                        g.DrawString($"Time: {thoiGianVao:dd/MM/yyyy HH:mm}", infoFont, Brushes.Black, 20, 80);
                    }

                    g.DrawImage(qrCode, 70, 120, 200, 200);

                    using (Font footerFont = new Font("Arial", 10))
                    {
                        g.DrawString("Scan this QR code to view menu", footerFont, Brushes.Black, 50, 340);
                    }
                }

                pd.PrintPage += (sender, e) =>
                {
                    e.Graphics.DrawImage(printBitmap, 0, 0, 400, 600);
                };

                pd.Print();
                printBitmap.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error printing: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
