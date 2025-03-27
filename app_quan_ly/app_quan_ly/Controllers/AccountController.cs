using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using app_quan_ly.Models;  // Add this to use NhanVien model

namespace app_quan_ly.Controllers
{
    public class AccountController  // Changed to public
    {
        private readonly AppModel _context;  // Add database context

        public AccountController(AppModel context)
        {
            _context = context;
        }

        public NhanVien Login(string maNV, string password)
        {
            // Find employee by ID and password
            var nhanVien = _context.NhanViens
                .FirstOrDefault(nv => nv.ma_nv == maNV && nv.pw_nv == password);

            return nhanVien;  // Returns null if not found
        }
    }
}
