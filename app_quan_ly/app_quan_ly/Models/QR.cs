namespace app_quan_ly.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("QR")]
    public partial class QR
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public QR()
        {
            ThanhToans = new HashSet<ThanhToan>();
        }

        [Key]
        public int id_qr { get; set; }

        [StringLength(17)]
        public string ma_qr { get; set; }

        [StringLength(3)]
        public string ma_nv { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? thoi_gian_vao { get; set; }

        public int? id_ban { get; set; }

        public virtual Ban Ban { get; set; }

        public virtual NhanVien NhanVien { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ThanhToan> ThanhToans { get; set; }
    }
}
