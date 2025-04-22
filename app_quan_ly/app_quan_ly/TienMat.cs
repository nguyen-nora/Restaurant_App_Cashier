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
using System.Drawing.Printing;

namespace app_quan_ly
{
    public partial class TienMat : Form
    {
        private string billId;
        private BillController billController;
        private PayController payController;

        public TienMat(string billId)
        {
            InitializeComponent();
            this.billId = billId;
            this.billController = new BillController();
            this.payController = new PayController();
            LoadBillDetails();
        }

        private void LoadBillDetails()
        {
            billController.LoadBillDetails(billId, lvBill, txtSum, txtDiscount, txtTotal);
            txtTotal.Text = txtSum.Text;
        }

        private void TienMat_Load(object sender, EventArgs e)
        {
            // Form load logic here
        }

        private void btnCK_Click(object sender, EventArgs e)
        {
            ChuyenKhoan frm = new ChuyenKhoan();
            frm.Show();
        }

        private void btnThe_Click(object sender, EventArgs e)
        {
            The frm = new The();
            frm.Show();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            if (billController != null)
            {
                billController.Dispose();
            }
            if (payController != null)
            {
                payController.Dispose();
            }
        }

        private void txtMoneyCus_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMoneyCus.Text))
            {
                txtMoneyCus.Text = "0";
                txtMoneyGCus.Text = "0";
                return;
            }

            try
            {
                if (decimal.TryParse(txtMoneyCus.Text, out decimal amountPaid) && 
                    decimal.TryParse(txtSum.Text, out decimal totalAmount))
                {
                    decimal change = amountPaid - totalAmount;
                    txtMoneyGCus.Text = change > 0 ? change.ToString() : "0";
                }
                else
                {
                    txtMoneyCus.Text = "0";
                    txtMoneyGCus.Text = "0";
                }
            }
            catch
            {
                txtMoneyCus.Text = "0";
                txtMoneyGCus.Text = "0";
            }
        }

        private void btnSuccess_Click(object sender, EventArgs e)
        {
            try
            {
                // Validate input values
                if (!decimal.TryParse(txtMoneyCus.Text, out decimal amountPaid))
                {
                    MessageBox.Show("Vui lòng nhập số tiền khách đưa hợp lệ", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }       

                if (!decimal.TryParse(txtTotal.Text, out decimal totalAmount))
                {
                    MessageBox.Show("Số tiền thanh toán không hợp lệ", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Validate payment amount
                if (amountPaid < totalAmount)
                {
                    MessageBox.Show("Số tiền khách đưa không đủ để thanh toán", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Process the payment
                payController.ProcessPayment(billId, amountPaid, totalAmount, "Tiền mặt");

                // Update table status in database
                using (var db = new AppModel())
                {
                    // Get the QR record for this bill
                    var qr = db.QRs.FirstOrDefault(q => q.ThanhToans.Any(t => t.id_bill == billId));
                    if (qr != null)
                    {
                        // Update the table status to Pending
                        var table = db.Bans.FirstOrDefault(b => b.id_ban == qr.id_ban);
                        if (table != null)
                        {
                            table.trang_thai = "Pending";
                        }

                        // Update the payment status to Completed
                        var payment = db.ThanhToans.FirstOrDefault(p => p.id_bill == billId);
                        if (payment != null)
                        {
                            payment.trang_thai = "Completed";
                        }

                        db.SaveChanges();
                    }
                }

                // Print the receipt
                payController.PrintPaymentReceipt(billId, lvBill, txtSum, txtDiscount, txtTotal, txtMoneyCus, txtMoneyGCus, "Tiền mặt");

                // Show success message
                MessageBox.Show("Thanh toán thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Close the form
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi xử lý thanh toán: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btn1_Click(object sender, EventArgs e)
        {
            txtMoneyCus.Text += "1";
        }

        private void btn2_Click(object sender, EventArgs e)
        {
            txtMoneyCus.Text += "2";
        }

        private void btn3_Click(object sender, EventArgs e)
        {
            txtMoneyCus.Text += "3";
        }

        private void btn4_Click(object sender, EventArgs e)
        {
            txtMoneyCus.Text += "4";
        }

        private void btn5_Click(object sender, EventArgs e)
        {
            txtMoneyCus.Text += "5";
        }

        private void btn6_Click(object sender, EventArgs e)
        {
            txtMoneyCus.Text += "6";
        }

        private void btn7_Click(object sender, EventArgs e)
        {
            txtMoneyCus.Text += "7";
        }

        private void btn8_Click(object sender, EventArgs e)
        {
            txtMoneyCus.Text += "8";
        }

        private void btn9_Click(object sender, EventArgs e)
        {
            txtMoneyCus.Text += "9";
        }

        private void btn10_Click(object sender, EventArgs e)
        {
            txtMoneyCus.Text += "0";
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (txtMoneyCus.Text.Length > 0)
            {
                txtMoneyCus.Text = txtMoneyCus.Text.Substring(0, txtMoneyCus.Text.Length - 1);
            }
        }

        private void btn000_Click(object sender, EventArgs e)
        {
            txtMoneyCus.Text += "000";
        }
    }
}
