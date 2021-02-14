namespace Hexacafe.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class MenuItem
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public MenuItem()
        {
            Orders = new HashSet<Order>();
        }

        public int menuitemid { get; set; }

        public string itemname { get; set; }

        public string description { get; set; }

        public int? price { get; set; }

        public int restaurentid { get; set; }

        public int FoodCategoryID { get; set; }

        public int MainFoodCategoryID { get; set; }

        public string itempic { get; set; }

        public virtual MainFoodType MainFoodType { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Order> Orders { get; set; }
    }
}
