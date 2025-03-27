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
    public partial class ChuyenKhoan : Form
    {
        public ChuyenKhoan()
        {
            InitializeComponent();
        }

        private void btnSoDoBan_Click(object sender, EventArgs e)
        {
            The frm = new The();
            frm.Show();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
