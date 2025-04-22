using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace app_quan_ly.Models
{
    public partial class AppModel : DbContext
    {
        public AppModel()
            : base("name=AppModel9")
        {
        }

        public virtual DbSet<Ban> Bans { get; set; }
        public virtual DbSet<CongThuc> CongThucs { get; set; }
        public virtual DbSet<DanhMuc> DanhMucs { get; set; }
        public virtual DbSet<DetailThanhToan> DetailThanhToans { get; set; }
        public virtual DbSet<Kho> Khoes { get; set; }
        public virtual DbSet<KhuVuc> KhuVucs { get; set; }
        public virtual DbSet<MayIn> MayIns { get; set; }
        public virtual DbSet<NhanVien> NhanViens { get; set; }
        public virtual DbSet<QR> QRs { get; set; }
        public virtual DbSet<ThanhToan> ThanhToans { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Ban>()
                .Property(e => e.ma_ban)
                .IsUnicode(false);

            modelBuilder.Entity<Ban>()
                .Property(e => e.trang_thai)
                .IsUnicode(false);

            modelBuilder.Entity<CongThuc>()
                .Property(e => e.gia_tien)
                .HasPrecision(18, 0);

            modelBuilder.Entity<CongThuc>()
                .Property(e => e.trang_thai)
                .IsUnicode(false);

            modelBuilder.Entity<DetailThanhToan>()
                .Property(e => e.id_bill)
                .IsUnicode(false);

            modelBuilder.Entity<DetailThanhToan>()
                .Property(e => e.so_tien)
                .HasPrecision(18, 0);

            modelBuilder.Entity<DetailThanhToan>()
                .Property(e => e.thoi_gian)
                .HasPrecision(0);

            modelBuilder.Entity<DetailThanhToan>()
                .Property(e => e.trang_thai)
                .IsUnicode(false);

            modelBuilder.Entity<NhanVien>()
                .Property(e => e.ma_nv)
                .IsUnicode(false);

            modelBuilder.Entity<NhanVien>()
                .Property(e => e.pw_nv)
                .IsUnicode(false);

            modelBuilder.Entity<QR>()
                .Property(e => e.ma_qr)
                .IsUnicode(false);

            modelBuilder.Entity<QR>()
                .Property(e => e.ma_nv)
                .IsUnicode(false);

            modelBuilder.Entity<QR>()
                .Property(e => e.thoi_gian_vao)
                .HasPrecision(0);

            modelBuilder.Entity<ThanhToan>()
                .Property(e => e.id_bill)
                .IsUnicode(false);

            modelBuilder.Entity<ThanhToan>()
                .Property(e => e.tong_tien)
                .HasPrecision(18, 0);

            modelBuilder.Entity<ThanhToan>()
                .Property(e => e.thoi_gian_bill)
                .HasPrecision(0);

            modelBuilder.Entity<ThanhToan>()
                .Property(e => e.trang_thai)
                .IsUnicode(false);
        }
    }
}
