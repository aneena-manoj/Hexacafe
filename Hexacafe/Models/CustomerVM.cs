using Hexacafe.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hexacafe.Models
{
    public class CustomerVM
    {
        public User User { get; set; }
        public MenuItem MenuItem { get; set; }
        public MainFoodType MainFoodType { get; set; }
        public RestaurentRegistration RestaurentRegistration { get; set; }
        public Order Order { get; set; }
        public FoodCategory FoodCategory { get; set; }
    }
}