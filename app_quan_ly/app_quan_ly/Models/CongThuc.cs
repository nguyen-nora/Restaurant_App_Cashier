namespace app_quan_ly.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CongThuc")]
    public partial class CongThuc
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CongThuc()
        {
            DetailThanhToans = new HashSet<DetailThanhToan>();
        }

        [Key]
        public int id_cong_thuc { get; set; }

        [StringLength(500)]
        public string ten_mon { get; set; }

        public int? id_loai_mon { get; set; }

        public string hinh_anh { get; set; }

        public string mo_ta { get; set; }

        public decimal? gia_tien { get; set; }

        [Required]
        [StringLength(10)]
        public string trang_thai { get; set; }

        public string ghi_chu { get; set; }

        public virtual DanhMuc DanhMuc { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DetailThanhToan> DetailThanhToans { get; set; }
    }
}
