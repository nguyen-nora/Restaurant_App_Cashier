using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using app_quan_ly.Models;

namespace app_quan_ly.Controllers
{
    public class BillController
    {
        private AppModel db;

        public BillController()
        {
            db = new AppModel();
        }

        public void LoadBillDetails(string billId, ListView lvBill, TextBox txtSum, TextBox txtDiscount, TextBox txtTotal)
        {
            try
            {
                lvBill.Items.Clear();

                var billDetails = from dt in db.DetailThanhToans
                                join ct in db.CongThucs on dt.id_cong_thuc equals ct.id_cong_thuc
                                where dt.id_bill == billId
                                select new
                                {
                                    TenMon = ct.ten_mon,
                                    SoLuong = dt.so_luong,
                                    Gia = ct.gia_tien,
                                    TongGia = ct.gia_tien * dt.so_luong
                                };

                decimal totalSum = 0;
                foreach (var detail in billDetails)
                {
                    ListViewItem item = new ListViewItem(detail.SoLuong.ToString());
                    item.SubItems.Add(detail.TenMon);
                    item.SubItems.Add(detail.TongGia.ToString());
                    lvBill.Items.Add(item);
                    totalSum += detail.TongGia ?? 0;
                }

                txtSum.Text = totalSum.ToString();
                UpdateTotal(txtSum, txtDiscount, txtTotal);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading bill details: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void LoadAllMenuItems(ListView lvAll, ComboBox cbloaiMon)
        {
            try
            {
                // Load categories
                var categories = db.DanhMucs.Select(dm => dm.ten_loai).ToList();
                cbloaiMon.Items.Clear();
                cbloaiMon.Items.Add("Tất cả");
                cbloaiMon.Items.AddRange(categories.ToArray());

                // Load all menu items
                var menuItems = from ct in db.CongThucs
                               where ct.trang_thai == "Active"
                               select ct.ten_mon;

                lvAll.Items.Clear();
                foreach (var item in menuItems)
                {
                    ListViewItem lvItem = new ListViewItem(item);
                    lvAll.Items.Add(lvItem);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading menu items: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void SearchMenuItems(string searchText, string category, ListView lvAll)
        {
            try
            {
                var query = from ct in db.CongThucs
                           join dm in db.DanhMucs on ct.id_loai_mon equals dm.id_loai_mon
                           where ct.trang_thai == "Active"
                           select new { ct.ten_mon, dm.ten_loai };

                if (!string.IsNullOrEmpty(searchText))
                {
                    query = query.Where(x => x.ten_mon.Contains(searchText));
                }

                if (!string.IsNullOrEmpty(category) && category != "Tất cả")
                {
                    query = query.Where(x => x.ten_loai == category);
                }

                lvAll.Items.Clear();
                foreach (var item in query)
                {
                    ListViewItem lvItem = new ListViewItem(item.ten_mon);
                    lvAll.Items.Add(lvItem);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error searching menu items: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void UpdateTotal(TextBox txtSum, TextBox txtDiscount, TextBox txtTotal)
        {
            try
            {
                if (decimal.TryParse(txtSum.Text, out decimal sum) && 
                    decimal.TryParse(txtDiscount.Text, out decimal discount))
                {
                    decimal total = sum - discount;
                    txtTotal.Text = total.ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating total: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void Dispose()
        {
            if (db != null)
            {
                db.Dispose();
            }
        }
    }
}
