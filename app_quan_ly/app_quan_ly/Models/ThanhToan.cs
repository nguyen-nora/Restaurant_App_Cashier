namespace app_quan_ly.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ThanhToan")]
    public partial class ThanhToan
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ThanhToan()
        {
            DetailThanhToans = new HashSet<DetailThanhToan>();
        }

        [Key]
        [StringLength(15)]
        public string id_bill { get; set; }

        public decimal? tong_tien { get; set; }

        [StringLength(100)]
        public string loai_thanh_toan { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? thoi_gian_bill { get; set; }

        public int? id_qr { get; set; }

        public string ghi_chu { get; set; }

        [Required]
        [StringLength(50)]
        public string trang_thai { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DetailThanhToan> DetailThanhToans { get; set; }

        public virtual QR QR { get; set; }
    }
}
