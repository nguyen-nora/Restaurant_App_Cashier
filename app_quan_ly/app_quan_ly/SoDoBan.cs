using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using app_quan_ly.Models;
using app_quan_ly.Controllers;
using QRCoder;

namespace app_quan_ly
{
    public partial class SoDoBan : Form
    {
        private AppModel db;
        private Dictionary<string, Color> statusColors;
        private List<Button> tableButtons;
        private string maNhanVien;
        private app_quan_ly.Controllers.PrintController printController;

        public SoDoBan(string maNhanVien)
        {
            InitializeComponent();
            this.maNhanVien = maNhanVien;
            db = new AppModel();
            printController = new app_quan_ly.Controllers.PrintController();
            InitializeStatusColors();
            tableButtons = new List<Button>();
            //LoadTables(null); // Load all tables initially
        }

        private void InitializeStatusColors()
        {
            statusColors = new Dictionary<string, Color>();
            statusColors.Add("Pending", Color.Gray);
            statusColors.Add("Processing", Color.RoyalBlue);
            statusColors.Add("Completed", Color.Red);
        }

        private void ClearTableButtons()
        {
            foreach (var btn in tableButtons)
            {
                this.Controls.Remove(btn);
                btn.Dispose();
            }
            tableButtons.Clear();
        }

        private void LoadTables(string khuVuc)
        {
            try
            {
                ClearTableButtons();

                // Get all tables with their information
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

                // Filter by khu vuc if specified
                if (!string.IsNullOrEmpty(khuVuc))
                {
                    query = query.Where(t => t.KhuVuc == khuVuc);
                }

                // Debug logging for raw status values
                var tables = query.ToList();
                foreach (var table in tables)
                {
                    System.Diagnostics.Debug.WriteLine($"Raw status for table {table.ma_ban}: {table.TrangThai}");
                }

                foreach (var table in tables)
                {
                    CreateTableButton(table.id_ban, table.ma_ban ?? "", table.KhuVucId?.ToString() ?? "", table.TrangThai ?? "Trống");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading tables: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CreateTableButton(int id, string maTable, string khuVucId, string trangThai)
        {
            Button btn = new Button();
            btn.Name = $"Table_{id}";
            btn.Text = maTable;
            btn.Size = new Size(132, 63);
            btn.Font = new Font("Microsoft Sans Serif", 10.8f, FontStyle.Regular);
            btn.ForeColor = Color.White;        
            btn.BackColor = statusColors.ContainsKey(trangThai) ? statusColors[trangThai] : Color.Gray;

            // Set button location based on khu vuc
            int baseX = 61;
            int baseY = 231;
            int spacing = 20;
            int buttonsPerRow = 3;

            int row = (tableButtons.Count / buttonsPerRow);
            int col = (tableButtons.Count % buttonsPerRow);

            btn.Location = new Point(baseX + (btn.Width + spacing) * col, 
                                   baseY + (btn.Height + spacing) * row);

            btn.Click += TableButton_Click;
            this.Controls.Add(btn);
            tableButtons.Add(btn);
        }

        private void TableButton_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            if (btn != null)
            {
                int tableId = int.Parse(btn.Name.Split('_')[1]);
                string maBan = btn.Text;
                
                DialogResult result = MessageBox.Show("Create New Table?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                
                if (result == DialogResult.Yes)
                {
                    try
                    {
                        // Get current time
                        DateTime thoiGianVao = DateTime.Now;

                        // Generate QR code
                        Bitmap qrCodeImage = QRController.GenerateTableQRCode(maBan, maNhanVien);
                        
                        // Print QR code and table info
                        PrintTableInfo(qrCodeImage, maBan, thoiGianVao);

                        // Save QR to database
                        QRController.SaveQRToDatabase(maBan, maNhanVien, thoiGianVao);

                        // Update table status to Processing
                        var table = db.Bans.FirstOrDefault(b => b.ma_ban == maBan);
                        if (table != null)
                        {
                            table.trang_thai = "Processing";
                            db.SaveChanges();
                        }

                        // Reload tables to show updated status
                        LoadTables(null);

                        MessageBox.Show("QR code printed and saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error creating table: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
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

                // Create a new PrintDocument
                PrintDocument pd = new PrintDocument();
                pd.PrinterSettings.PrinterName = cashierPrinter.ten_may_in;

                // Create a bitmap to hold the entire printout
                Bitmap printBitmap = new Bitmap(400, 600);
                using (Graphics g = Graphics.FromImage(printBitmap))
                {
                    g.Clear(Color.White);

                    // Draw header
                    using (Font headerFont = new Font("Arial", 16, FontStyle.Bold))
                    {
                        g.DrawString("RESTAURANT QR CODE", headerFont, Brushes.Black, 30, 20);
                    }

                    // Draw table info
                    using (Font infoFont = new Font("Arial", 12))
                    {
                        g.DrawString($"Table: {maBan}", infoFont, Brushes.Black, 20, 60);
                        g.DrawString($"Time: {thoiGianVao:dd/MM/yyyy HH:mm}", infoFont, Brushes.Black, 20, 80);
                    }

                    // Draw QR code
                    g.DrawImage(qrCode, 70, 120, 200, 200);

                    // Draw footer
                    using (Font footerFont = new Font("Arial", 10))
                    {
                        g.DrawString("Scan this QR code to view menu", footerFont, Brushes.Black, 50, 340);
                    }
                }

                // Print the bitmap
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

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            if (db != null)
            {
                db.Dispose();
            }
        }

        private void RowA_Click(object sender, EventArgs e)
        {
            LoadTables("A");
        }

        private void RowB_Click(object sender, EventArgs e)
        {
            LoadTables("B");
        }

        private void RowC_Click(object sender, EventArgs e)
        {
            LoadTables("C");
        }

        private void btnQLM_Click(object sender, EventArgs e)
        {
            QuanLyMon frm = new QuanLyMon();
            frm.Show();
            this.Hide();
        }

        private void btnSetup_Click(object sender, EventArgs e)
        {
            Cai_Dat frm = new Cai_Dat();
            frm.Show();
        }

        private void btnChange_Click(object sender, EventArgs e)
        {
            ChuyenBan frm = new ChuyenBan();
            frm.Show();
        }
    }
}
