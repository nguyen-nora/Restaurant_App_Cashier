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
    public partial class Xacnhanbill : Form
    {
        private string billId;
        private BillController billController;
        private FoodController foodController;
        private List<CongThuc> allFoods;

        public Xacnhanbill(string billId)
        {
            try
            {
                InitializeComponent();
                this.billId = billId;
                this.billController = new BillController();
                this.foodController = new FoodController();
                LoadBillDetails();
                SetupFoodControls();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error initializing Xacnhanbill: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SetupFoodControls()
        {
            // Setup ComboBox
            cbloaiMon.Items.Clear();
            cbloaiMon.Items.Add("Tất cả");
            var categories = foodController.GetAllCategories();
            foreach (var category in categories)
            {
                cbloaiMon.Items.Add(category.ten_loai);
            }
            cbloaiMon.SelectedIndex = 0;
            cbloaiMon.SelectedIndexChanged += CbloaiMon_SelectedIndexChanged;

            // Setup TextBox
            txtSearch.TextChanged += TxtSearch_TextChanged;

            // Load initial data
            LoadFoodData();
        }

        private void LoadFoodData()
        {
            lvFood.Items.Clear();
            allFoods = foodController.GetAllFoods();
            DisplayFoods(allFoods);
        }

        private void DisplayFoods(List<CongThuc> foods)
        {
            lvFood.Items.Clear();
            foreach (var food in foods)
            {
                ListViewItem item = new ListViewItem(food.ten_mon);
                item.Tag = food.id_cong_thuc;
                lvFood.Items.Add(item);
            }
        }

        private void CbloaiMon_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbloaiMon.SelectedIndex == 0) // "Tất cả" selected
            {
                DisplayFoods(allFoods);
            }
            else
            {
                var categories = foodController.GetAllCategories();
                var selectedCategory = categories[cbloaiMon.SelectedIndex - 1]; // -1 because of "Tất cả" option
                var filteredFoods = foodController.GetFoodsByCategory(selectedCategory.id_loai_mon);
                DisplayFoods(filteredFoods);
            }
        }

        private void TxtSearch_TextChanged(object sender, EventArgs e)
        {
            string searchText = txtSearch.Text.ToLower();
            var filteredFoods = allFoods.Where(f => f.ten_mon.ToLower().Contains(searchText)).ToList();
            DisplayFoods(filteredFoods);
        }

        private void LoadBillDetails()
        {
            billController.LoadBillDetails(billId, lvBill, txtSum, txtDiscount, txtTotal);
        }

        private void txtDiscount_TextChanged(object sender, EventArgs e)
        {
            billController.UpdateTotal(txtSum, txtDiscount, txtTotal);
        }

        private void btnTam_Click(object sender, EventArgs e)
        {
            try
            {
                billController.PrintTemporaryBill(billId, lvBill, txtSum, txtDiscount, txtTotal);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error printing temporary bill: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            if (billController != null)
            {
                billController.Dispose();
            }
        }

        private void btnThanhToan_Click(object sender, EventArgs e)
        {
            try
            {
                TienMat tienMatForm = new TienMat(billId);
                tienMatForm.ShowDialog();
                this.Hide();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error opening payment form: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btn1_Click(object sender, EventArgs e)
        {
            txtTotal.Text += "1";
        }

        private void btn2_Click(object sender, EventArgs e)
        {
            txtTotal.Text += "2";
        }

        private void btn3_Click(object sender, EventArgs e)
        {
            txtTotal.Text += "3";
        }

        private void btn4_Click(object sender, EventArgs e)
        {
            txtTotal.Text += "4";
        }

        private void btn5_Click(object sender, EventArgs e)
        {
            txtTotal.Text += "5";
        }

        private void btn6_Click(object sender, EventArgs e)
        {
            txtTotal.Text += "6";
        }

        private void btn7_Click(object sender, EventArgs e)
        {
            txtTotal.Text += "7";
        }

        private void btn8_Click(object sender, EventArgs e)
        {
            txtTotal.Text += "8";
        }

        private void btn9_Click(object sender, EventArgs e)
        {
            txtTotal.Text += "9";
        }

        private void btn0_Click(object sender, EventArgs e)
        {
            txtTotal.Text += "0";
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (txtTotal.Text.Length > 0)
            {
                txtTotal.Text = txtTotal.Text.Substring(0, txtTotal.Text.Length - 1);
            }
        }
    }
}
