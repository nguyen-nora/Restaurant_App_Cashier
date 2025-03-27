using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using app_quan_ly.Controllers;

namespace app_quan_ly
{
    public partial class Cai_Dat : Form
    {
        private PrintController printController;

        public Cai_Dat()
        {
            InitializeComponent();
            printController = new PrintController();
            LoadPrinters();
            LoadSavedPrinters();
        }

        private void LoadPrinters()
        {
            // Get all installed printers
            var printers = printController.GetAllPrinters();
            
            // Add "Tất cả" option to both comboboxes
            cbDevice1.Items.Clear();
            cbDevice2.Items.Clear();
            
            cbDevice1.Items.Add("Tất cả");
            cbDevice2.Items.Add("Tất cả");
            
            foreach (var printer in printers)
            {
                cbDevice1.Items.Add(printer);
                cbDevice2.Items.Add(printer);
            }
        }

        private void LoadSavedPrinters()
        {
            var cashierPrinter = printController.GetCashierPrinter();
            var kitchenPrinter = printController.GetKitchenPrinter();

            if (cashierPrinter != null)
            {
                cbDevice1.Text = cashierPrinter.ten_may_in;
            }
            else
            {
                cbDevice1.Text = "Tất cả";
            }

            if (kitchenPrinter != null)
            {
                cbDevice2.Text = kitchenPrinter.ten_may_in;
            }
            else
            {
                cbDevice2.Text = "Tất cả";
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                printController.SavePrinterSettings(cbDevice1.Text, cbDevice2.Text);
                MessageBox.Show("Lưu cài đặt máy in thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Có lỗi xảy ra khi lưu cài đặt: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
