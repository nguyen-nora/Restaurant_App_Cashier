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
using app_quan_ly.Controllers;

namespace app_quan_ly
{
    public partial class Form1 : Form
    {
        private TextBox currentTextBox = null; // Track which textbox is selected
        private readonly AccountController _accountController;

        public Form1()
        {
            InitializeComponent();
            SetupTextBoxes();
            _accountController = new AccountController(new AppModel());
        }

        private void SetupTextBoxes()
        {
            // Add event handlers for textboxes
            txtMnv.Click += TextBox_Click;
            txtPw.Click += TextBox_Click;
        }

        private void TextBox_Click(object sender, EventArgs e)
        {
            currentTextBox = (TextBox)sender;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {
            AddNumber("6");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            AddNumber("3");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AddNumber("1");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            AddNumber("2");
        }

        private void button9_Click(object sender, EventArgs e)
        {
            AddNumber("9");
        }

        private void button8_Click(object sender, EventArgs e)
        {
            AddNumber("8");
        }

        private void button5_Click(object sender, EventArgs e)
        {
            AddNumber("5");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            AddNumber("4");
        }

        private void button7_Click(object sender, EventArgs e)
        {
            AddNumber("7");
        }

        private void button10_Click(object sender, EventArgs e)
        {
            AddNumber("0");
        }

        private void buttonLogin_Click(object sender, EventArgs e)
        {
            try
            {
                var nhanVien = _accountController.Login(txtMnv.Text, txtPw.Text);
                
                if (nhanVien != null)
                {
                    MessageBox.Show("Đăng nhập thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    SoDoBan frm = new SoDoBan(txtMnv.Text);
                    frm.Show();
                    this.Hide();  // Hide the login form
                    frm.FormClosed += (s, args) => this.Close();  // Close the application when SoDoBan is closed
                }
                else
                {
                    MessageBox.Show("Sai mã nhân viên hoặc mật khẩu!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi kết nối: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonBackspace_Click(object sender, EventArgs e)
        {
            if (currentTextBox != null && currentTextBox.Text.Length > 0)
            {
                currentTextBox.Text = currentTextBox.Text.Substring(0, currentTextBox.Text.Length - 1);
            }
        }

        private void AddNumber(string number)
        {
            if (currentTextBox != null)
            {
                currentTextBox.Text += number;
            }
        }
    }
}
