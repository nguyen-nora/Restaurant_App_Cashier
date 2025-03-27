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
        private TableController tableController;
        private string maNhanVien;

        public SoDoBan(string maNhanVien)
        {
            InitializeComponent();
            this.maNhanVien = maNhanVien;
            tableController = new TableController(maNhanVien);
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            if (tableController != null)
            {
                tableController.Dispose();
            }
        }

        private void RowA_Click(object sender, EventArgs e)
        {
            tableController.LoadTables(this, "A");
        }

        private void RowB_Click(object sender, EventArgs e)
        {
            tableController.LoadTables(this, "B");
        }

        private void RowC_Click(object sender, EventArgs e)
        {
            tableController.LoadTables(this, "C");
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
