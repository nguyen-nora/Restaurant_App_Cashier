namespace app_quan_ly.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Kho")]
    public partial class Kho
    {
        [Key]
        public int id_kho { get; set; }

        public int? id_cong_thuc { get; set; }

        public int? so_luong { get; set; }
    }
}
