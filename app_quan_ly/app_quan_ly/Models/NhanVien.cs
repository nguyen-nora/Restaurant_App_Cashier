namespace app_quan_ly.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("NhanVien")]
    public partial class NhanVien
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public NhanVien()
        {
            QRs = new HashSet<QR>();
        }

        [Key]
        [StringLength(3)]
        public string ma_nv { get; set; }

        [StringLength(100)]
        public string ten_nv { get; set; }

        [StringLength(20)]
        public string pw_nv { get; set; }

        [StringLength(50)]
        public string role_nv { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<QR> QRs { get; set; }
    }
}
