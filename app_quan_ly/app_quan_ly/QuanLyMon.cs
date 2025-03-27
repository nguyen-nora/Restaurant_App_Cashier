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
    public partial class QuanLyMon : Form
    {
        private FoodController foodController;
        private List<CongThuc> allFoods; // Store all foods for filtering

        public QuanLyMon()
        {
            InitializeComponent();
            foodController = new FoodController();
            SetupListView();
            SetupComboBox();
            txtSearch.TextChanged += txtSearch_TextChanged;
        }

        private void SetupListView()
        {
            lvFood.FullRowSelect = true;
            lvFood.GridLines = true;
            lvFood.View = View.Details;
            lvFood.MultiSelect = false;
            lvFood.SelectedIndexChanged += lvFood_SelectedIndexChanged;
        }

        private void SetupComboBox()
        {
            cbloaiMon.Items.Clear();
            cbloaiMon.Items.Add("Tất cả"); // Add "All" option
            var categories = foodController.GetAllCategories();
            foreach (var category in categories)
            {
                cbloaiMon.Items.Add(category.ten_loai);
            }
            cbloaiMon.SelectedIndex = 0; // Select "All" by default
            cbloaiMon.SelectedIndexChanged += cbloaiMon_SelectedIndexChanged;
        }

        private void cbloaiMon_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbloaiMon.SelectedIndex == 0) // "All" selected
            {
                DisplayFoods(allFoods);
            }
            else
            {
                var categories = foodController.GetAllCategories();
                var selectedCategory = categories[cbloaiMon.SelectedIndex - 1]; // -1 because of "All" option
                var filteredFoods = allFoods.Where(f => f.id_loai_mon == selectedCategory.id_loai_mon).ToList();
                DisplayFoods(filteredFoods);
            }
        }

        private void btnHoaDon_Click(object sender, EventArgs e)
        {
            HoaDon frm = new HoaDon();
            frm.Show();
            this.Hide();
        }

        private void QuanLyMon_Load(object sender, EventArgs e)
        {
            LoadFoodData();
        }

        private void LoadFoodData()
        {
            lvFood.Items.Clear();
            allFoods = foodController.GetAllFoods(); // Store all foods
            DisplayFoods(allFoods); // Display all foods initially
        }

        private void DisplayFoods(List<CongThuc> foods)
        {
            lvFood.Items.Clear();
            foreach (var food in foods)
            {
                ListViewItem item = new ListViewItem(food.trang_thai);
                item.SubItems.Add(food.ten_mon);
                item.SubItems.Add(food.gia_tien.ToString());
                item.Tag = food.id_cong_thuc;
                
                if (food.trang_thai == "off")
                {
                    item.BackColor = Color.Red;
                    item.ForeColor = Color.White;
                }
                else
                {
                    item.BackColor = Color.White;
                    item.ForeColor = Color.Black;
                }
                
                lvFood.Items.Add(item);
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            string searchText = txtSearch.Text.ToLower();
            var filteredFoods = allFoods.Where(f => f.ten_mon.ToLower().Contains(searchText)).ToList();
            DisplayFoods(filteredFoods);
        }

        private void butTT_Click(object sender, EventArgs e)
        {
            if (lvFood.SelectedItems.Count > 0)
            {
                var selectedItem = lvFood.SelectedItems[0];
                int foodId = (int)selectedItem.Tag;

                if (foodController.ToggleFoodStatus(foodId))
                {
                    LoadFoodData();
                }
                else
                {
                    MessageBox.Show("Không thể thay đổi trạng thái món ăn!");
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn món ăn cần thay đổi trạng thái!");
            }
        }

        private void lvFood_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvFood.SelectedItems.Count > 0)
            {
                var selectedItem = lvFood.SelectedItems[0];
                selectedItem.BackColor = Color.LightBlue;
                selectedItem.ForeColor = Color.Black;

                txtName.Text = selectedItem.SubItems[1].Text;
                txtGia.Text = selectedItem.SubItems[2].Text;
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {

        }

    }
}
    