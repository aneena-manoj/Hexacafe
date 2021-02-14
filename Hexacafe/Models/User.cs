namespace Hexacafe.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Web.Security;

    [Table("User")]
    public partial class User
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public User()
        {
            Orders = new HashSet<Order>();
        }

        public int userid { get; set; }
        [Required(ErrorMessage = "Customer Email is Required")]
        [EmailAddress(ErrorMessage = "Please check Email Format")]
        public string email { get; set; }

        [StringLength(50)]
        [Required(ErrorMessage = "Customer Email is Required")]
        [DataType(DataType.Password)]
        [MembershipPassword(
    MinRequiredNonAlphanumericCharacters = 1,

    MinNonAlphanumericCharactersError = "Your password needs to contain at least one symbol (!, @, #, etc).",
    ErrorMessage = "Your password must be 8 characters long and contain at least one symbol (!, @, #, etc).",
    MinRequiredPasswordLength = 8)]
        public string password { get; set; }
        [NotMapped]
        [Display(Name = "Confirm Password")]
        [Compare("password", ErrorMessage = "Password and Confirm Password must be same")]
        [DataType(DataType.Password)]
        public string confirmpassword { get; set; }
        [Required(ErrorMessage = "Customer Food Preference is Required")]
        [Display(Name = "Select Your food Preference")]
        public int userfoodpreference { get; set; }

        public bool? emailvarified { get; set; }

        [StringLength(50)]
        public string regdate { get; set; }

        public Guid? guid { get; set; }
        [Required(ErrorMessage = "Customer Mobile is Required")]
        [Display(Name = "Customer Mobile")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Not a valid phone number")]
        public long mobile { get; set; }

        public virtual MainFoodType MainFoodType { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Order> Orders { get; set; }
    }
}
