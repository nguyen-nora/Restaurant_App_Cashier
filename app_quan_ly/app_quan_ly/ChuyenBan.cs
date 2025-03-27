using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using app_quan_ly.Models;
using System.Drawing.Printing;

namespace app_quan_ly
{
    public partial class ChuyenBan : Form
    {
        private AppModel db = new AppModel();
        private PrintDocument printDocument = new PrintDocument();

        public ChuyenBan()
        {
            InitializeComponent();
            LoadTables();
            printDocument.PrintPage += new PrintPageEventHandler(printDocument_PrintPage);
        }

        private void LoadTables()
        {
            // Load Processing tables into cbChuyenA
            var processingTables = db.Bans.Where(b => b.trang_thai == "Processing").ToList();
            cbChuyenA.DataSource = processingTables;
            cbChuyenA.DisplayMember = "ma_ban";
            cbChuyenA.ValueMember = "id_ban";

            // Load Pending tables into cbChuyenB
            var pendingTables = db.Bans.Where(b => b.trang_thai == "Pending").ToList();
            cbChuyenB.DataSource = pendingTables;
            cbChuyenB.DisplayMember = "ma_ban";
            cbChuyenB.ValueMember = "id_ban";
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (cbChuyenA.SelectedItem == null || cbChuyenB.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng chọn bàn nguồn và bàn đích!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var sourceTable = (Ban)cbChuyenA.SelectedItem;
                var targetTable = (Ban)cbChuyenB.SelectedItem;

                // Update table statuses
                sourceTable.trang_thai = "Pending";
                targetTable.trang_thai = "Processing";

                // Find and update the QR record for the source table
                var qr = db.QRs.Where(q => q.id_ban == sourceTable.id_ban)
                              .OrderByDescending(q => q.thoi_gian_vao)
                              .FirstOrDefault();

                if (qr != null)
                {
                    qr.id_ban = targetTable.id_ban;
                }

                db.SaveChanges();

                // Print notification
                PrintNotification(sourceTable, targetTable);

                MessageBox.Show("Chuyển bàn thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi chuyển bàn: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void PrintNotification(Ban sourceTable, Ban targetTable)
        {
            try
            {
                // Get the default printer
                var defaultPrinter = new PrinterSettings();
                printDocument.PrinterSettings.PrinterName = defaultPrinter.PrinterName;

                // Set up the print document
                printDocument.DefaultPageSettings.PaperSize = new PaperSize("Custom", 300, 200);
                printDocument.DefaultPageSettings.Margins = new Margins(10, 10, 10, 10);

                // Print the document
                printDocument.Print();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi in thông báo: {ex.Message}", "Lỗi In", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void printDocument_PrintPage(object sender, PrintPageEventArgs e)
        {
            var sourceTable = (Ban)cbChuyenA.SelectedItem;
            var targetTable = (Ban)cbChuyenB.SelectedItem;

            // Create fonts
            Font titleFont = new Font("Arial", 12, FontStyle.Bold);
            Font contentFont = new Font("Arial", 10);

            // Draw content
            e.Graphics.DrawString("THÔNG BÁO CHUYỂN BÀN", titleFont, Brushes.Black, 50, 20);
            e.Graphics.DrawString($"Từ bàn: {sourceTable.ma_ban}", contentFont, Brushes.Black, 50, 50);
            e.Graphics.DrawString($"Sang bàn: {targetTable.ma_ban}", contentFont, Brushes.Black, 50, 70);
            e.Graphics.DrawString($"Thời gian: {DateTime.Now:dd/MM/yyyy HH:mm:ss}", contentFont, Brushes.Black, 50, 90);
            e.Graphics.DrawString("--------------------------------", contentFont, Brushes.Black, 50, 110);
            e.Graphics.DrawString("Vui lòng chuyển đến bàn mới", contentFont, Brushes.Black, 50, 130);
        }
    }
}
