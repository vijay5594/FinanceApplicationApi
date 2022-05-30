using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinanceApp.Model
{
    public class ProductCustomerModel
    {
        [Key]
        public int ProductCustomerId { get; set; }
        [ForeignKey("ProductId")]
        public int ProductId { get; set; }
        [ForeignKey("CustomerId")]
        public int CustomerId { get; set; }
        public int SlotNo { get; set; }
        public string CreatedBy { get; set; }
        public DateTime DateOfCreated { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime DateOfModified { get; set; }
        public bool IsActive { get; set; }
       
    }
}
