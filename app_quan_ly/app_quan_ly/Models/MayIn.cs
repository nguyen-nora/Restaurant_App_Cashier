namespace app_quan_ly.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("MayIn")]
    public partial class MayIn
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int id_may_in { get; set; }

        [StringLength(300)]
        public string ten_may_in { get; set; }

        [StringLength(300)]
        public string role_bo_phan { get; set; }
    }
}
