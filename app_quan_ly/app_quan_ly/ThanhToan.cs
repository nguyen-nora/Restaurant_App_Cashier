using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace app_quan_ly
{
    public partial class ThanhToan : Form
    {
        public ThanhToan()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button19_Click(object sender, EventArgs e)
        {
            TienMat frm = new TienMat();
            frm.Show();
        }

        private void button18_Click(object sender, EventArgs e)
        {
            ChuyenKhoan frm = new ChuyenKhoan();
            frm.Show();
        }

        private void btnSoDoBan_Click(object sender, EventArgs e)
        {
            The frm = new The();
            frm.Show();
        }
    }
}
