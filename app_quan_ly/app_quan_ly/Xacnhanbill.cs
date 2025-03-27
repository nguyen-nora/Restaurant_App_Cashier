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

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            if (billController != null)
            {
                billController.Dispose();
            }
        }
    }
}
