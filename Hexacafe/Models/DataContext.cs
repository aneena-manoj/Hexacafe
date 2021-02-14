namespace Hexacafe.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class DataContext : DbContext
    {
        public DataContext()
            : base("name=DataContext")
        {
            this.Configuration.LazyLoadingEnabled = false;

        }

        public virtual DbSet<FoodCategory> FoodCategories { get; set; }
        public virtual DbSet<MainFoodType> MainFoodTypes { get; set; }
        public virtual DbSet<MenuItem> MenuItems { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<RestaurentRegistration> RestaurentRegistrations { get; set; }
        public virtual DbSet<sysdiagram> sysdiagrams { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FoodCategory>()
                .Property(e => e.CategoryName)
                .IsUnicode(false);

            modelBuilder.Entity<MainFoodType>()
                .Property(e => e.maincategory)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<MainFoodType>()
                .HasMany(e => e.FoodCategories)
                .WithRequired(e => e.MainFoodType)
                .HasForeignKey(e => e.FoodMainID);

            modelBuilder.Entity<MainFoodType>()
                .HasMany(e => e.MenuItems)
                .WithRequired(e => e.MainFoodType)
                .HasForeignKey(e => e.MainFoodCategoryID);

            modelBuilder.Entity<MainFoodType>()
                .HasMany(e => e.RestaurentRegistrations)
                .WithRequired(e => e.MainFoodType)
                .HasForeignKey(e => e.RestaurentCategory);

            modelBuilder.Entity<MainFoodType>()
                .HasMany(e => e.Users)
                .WithRequired(e => e.MainFoodType)
                .HasForeignKey(e => e.userfoodpreference);

            modelBuilder.Entity<Order>()
                .Property(e => e.orderdate)
                .IsUnicode(false);

            modelBuilder.Entity<RestaurentRegistration>()
                .Property(e => e.Regdate)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.regdate)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Orders)
                .WithRequired(e => e.User)
                .HasForeignKey(e => e.OrderByUser);
        }
    }
}
