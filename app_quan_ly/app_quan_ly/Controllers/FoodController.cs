using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using app_quan_ly.Models;

namespace app_quan_ly.Controllers
{
    public class FoodController
    {
        private AppModel db = new AppModel();

        public List<CongThuc> GetAllFoods()
        {
            return db.CongThucs.ToList();
        }

        public List<DanhMuc> GetAllCategories()
        {
            return db.DanhMucs.ToList();
        }

        public CongThuc GetFoodById(int id)
        {
            return db.CongThucs.Find(id);
        }

        public bool AddFood(CongThuc food)
        {
            try
            {
                db.CongThucs.Add(food);
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool UpdateFood(CongThuc food)
        {
            try
            {
                var existingFood = db.CongThucs.Find(food.id_cong_thuc);
                if (existingFood != null)
                {
                    existingFood.ten_mon = food.ten_mon;
                    existingFood.id_loai_mon = food.id_loai_mon;
                    existingFood.hinh_anh = food.hinh_anh;
                    existingFood.mo_ta = food.mo_ta;
                    existingFood.gia_tien = food.gia_tien;
                    existingFood.trang_thai = food.trang_thai;
                    db.SaveChanges();
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        public bool DeleteFood(int id)
        {
            try
            {
                var food = db.CongThucs.Find(id);
                if (food != null)
                {
                    db.CongThucs.Remove(food);
                    db.SaveChanges();
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        public List<CongThuc> GetFoodsByStatus(string status)
        {
            return db.CongThucs.Where(f => f.trang_thai == status).ToList();
        }

        public List<CongThuc> GetFoodsByCategory(int categoryId)
        {
            return db.CongThucs.Where(f => f.id_loai_mon == categoryId).ToList();
        }

        public bool ToggleFoodStatus(int id)
        {
            try
            {
                var food = db.CongThucs.Find(id);
                if (food != null)
                {
                    food.trang_thai = food.trang_thai == "on" ? "off" : "on";
                    db.SaveChanges();
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }
    }
}
