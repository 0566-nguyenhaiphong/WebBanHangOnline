using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebBanHangOnline.Models.EF
{
    [Table("tb_Voucher")]
    public class Voucher : CommonAbstract
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required(ErrorMessage = "Tên voucher không được để trông")]
        [StringLength(150)]
        public string Title { get; set; }
        [StringLength(500)]
        public string Description { get; set; }
        [AllowHtml]
        public string Detail { get; set; }
        public decimal? ReducePrice { get; set; }
        public int? PercentReduce { get; set; }
        public string UserName { get; set; }
        public string VoucherCode { get; set; }
        public bool isUse { get; set; }
        public bool isSave { get; set; }
        public bool isApply { get; set; }

    }
}