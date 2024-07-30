using Microsoft.EntityFrameworkCore;
using MyFriends3._5.Models;

namespace MyFriends3._5.Data
{
    public class MyFriends3_5Context : DbContext
    {
        public MyFriends3_5Context(DbContextOptions<MyFriends3_5Context> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure the one-to-many relationship between User and Picture
            modelBuilder.Entity<User>()
                .HasMany(u => u.UserPictures)
                .WithOne(p => p.User)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent cascade delete

            // Configure the one-to-one relationship between User and ProfilePicture
            modelBuilder.Entity<User>()
                .HasOne(u => u.ProfilePicture)
                .WithOne(pp => pp.User)
                .HasForeignKey<ProfilePicture>(pp => pp.UserId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent cascade delete

            // Configure the one-to-one relationship between ProfilePicture and Picture
            modelBuilder.Entity<ProfilePicture>()
                .HasOne(pp => pp.Picture)
                .WithOne()
                .HasForeignKey<ProfilePicture>(pp => pp.PictureId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent cascade delete
        }
        public DbSet<User> User { get; set; } = default!;
        public DbSet<Picture> Pictures { get; set; }
        public DbSet<ProfilePicture> ProfilePictures { get; set; }
    }
}