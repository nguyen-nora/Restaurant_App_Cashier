namespace app_quan_ly.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DetailThanhToan")]
    public partial class DetailThanhToan
    {
        [Key]
        public int id_detail { get; set; }

        [StringLength(15)]
        public string id_bill { get; set; }

        public int? so_luong { get; set; }

        public decimal? so_tien { get; set; }

        public int? id_cong_thuc { get; set; }

        public string ghi_chu { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? thoi_gian { get; set; }

        [Required]
        [StringLength(50)]
        public string trang_thai { get; set; }

        public virtual CongThuc CongThuc { get; set; }

        public virtual ThanhToan ThanhToan { get; set; }
    }
}
