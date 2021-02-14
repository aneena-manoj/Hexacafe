namespace Hexacafe.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Order
    {
        public int orderid { get; set; }

        public int OrderByUser { get; set; }

        public int RestaurentID { get; set; }

        public int MenuItemID { get; set; }

        public int? price { get; set; }

        [StringLength(50)]
        public string orderdate { get; set; }
        public string session { get; set; }
        public bool paymentdone { get; set; }

        public bool? finalorder { get; set; }


        public virtual MenuItem MenuItem { get; set; }


        public virtual RestaurentRegistration RestaurentRegistration { get; set; }

        public virtual User User { get; set; }
    }
}
