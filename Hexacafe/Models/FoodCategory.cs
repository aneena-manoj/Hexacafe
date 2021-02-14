namespace Hexacafe.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("FoodCategory")]
    public partial class FoodCategory
    {
        public int FoodCategoryid { get; set; }

        [StringLength(50)]
        public string CategoryName { get; set; }

        public int? FoodMainID { get; set; }

        public int RestaurentID { get; set; }

        public virtual MainFoodType MainFoodType { get; set; }
        [ForeignKey("RestaurentID")]
        public virtual RestaurentRegistration RestaurentRegistration { get; set; }
    }
}
