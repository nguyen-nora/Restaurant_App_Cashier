using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using app_quan_ly.Models;

namespace app_quan_ly.Controllers
{
    public class PrintController
    {
        private AppModel db;

        public PrintController()
        {
            db = new AppModel();
        }

        public List<string> GetAllPrinters()
        {
            // Get all installed printers from Windows
            return System.Drawing.Printing.PrinterSettings.InstalledPrinters.Cast<string>().ToList();
        }

        public void SavePrinterSettings(string cashierPrinter, string kitchenPrinter)
        {
            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    // Remove existing printer settings
                    var existingPrinters = db.MayIns.ToList();
                    db.MayIns.RemoveRange(existingPrinters);
                    db.SaveChanges();

                    // Get the next available ID
                    int nextId = 1;
                    if (db.MayIns.Any())
                    {
                        nextId = db.MayIns.Max(p => p.id_may_in) + 1;
                    }

                    // Save Cashier printer
                    if (!string.IsNullOrEmpty(cashierPrinter) && cashierPrinter != "Tất cả")
                    {
                        var cashierDevice = new MayIn
                        {
                            id_may_in = nextId++,
                            ten_may_in = cashierPrinter,
                            role_bo_phan = "Cashier"
                        };
                        db.MayIns.Add(cashierDevice);
                    }

                    // Save Kitchen printer
                    if (!string.IsNullOrEmpty(kitchenPrinter) && kitchenPrinter != "Tất cả")
                    {
                        var kitchenDevice = new MayIn
                        {
                            id_may_in = nextId,
                            ten_may_in = kitchenPrinter,
                            role_bo_phan = "Bep"
                        };
                        db.MayIns.Add(kitchenDevice);
                    }

                    db.SaveChanges();
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public MayIn GetCashierPrinter()
        {
            return db.MayIns.FirstOrDefault(p => p.role_bo_phan == "Cashier");
        }

        public MayIn GetKitchenPrinter()
        {
            return db.MayIns.FirstOrDefault(p => p.role_bo_phan == "Bep");
        }
    }
}
