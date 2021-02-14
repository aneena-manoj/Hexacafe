namespace Hexacafe.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Web.Security;

    [Table("RestaurentRegistration")]
    public partial class RestaurentRegistration
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public RestaurentRegistration()
        {
            Orders = new HashSet<Order>();
            FoodCategories = new HashSet<FoodCategory>();

        }

        [Key]
        public int RestaurentId { get; set; }

        [Display(Name = "Restaurant Name")]
        [Required(ErrorMessage = "Restaurent Name is mandatory")]
        public string RestaurentName { get; set; }
        [StringLength(50)]
        [Display(Name = "Restaurant Official Email")]
        [Required(ErrorMessage = "Restaurant Email is mandatory")]
        [EmailAddress(ErrorMessage = "Check Email Format")]
        public string RestaurentEmail { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [MembershipPassword(
    MinRequiredNonAlphanumericCharacters = 1,

    MinNonAlphanumericCharactersError = "Your password needs to contain at least one symbol (!, @, #, etc).",
    ErrorMessage = "Your password must be 8 characters long and contain at least one symbol (!, @, #, etc).",
    MinRequiredPasswordLength = 8)]
        public string password { get; set; }

        [Display(Name = "Confirm Password")]
        [Compare("password", ErrorMessage = "Password and Confirm Password must be same")]
        [DataType(DataType.Password)]
        public string confirmpassword { get; set; }
        public bool? emailvarified { get; set; }
        [Display(Name = "Type of Restaurant")]
        [Required(ErrorMessage = "Please select type of Restaurent")]
        public int RestaurentCategory { get; set; }

        [Display(Name = "Address")]
        [Required(ErrorMessage = "Add Restaurant Address")]
        public string address { get; set; }
        [Display(Name = "Mobile")]
        [Required(ErrorMessage = "Add Restaurant official Mobile Number")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Not a valid phone number")]
        public long? Mobile { get; set; }

        [StringLength(50)]
        [Display(Name = "State Name")]
        [Required(ErrorMessage = "Enter State Name")]
        public string State { get; set; }

        [StringLength(50)]
        [Display(Name = "City Name")]
        [Required(ErrorMessage = "Enter City Name")]
        public string City { get; set; }

        [StringLength(50)]
        public string Regdate { get; set; }

        public Guid? guid { get; set; }

        public virtual MainFoodType MainFoodType { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Order> Orders { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FoodCategory> FoodCategories { get; set; }
    }
}
