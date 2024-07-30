using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyFriends3._5.Models
{
    [Table("ProfilePictures"), Index(nameof(UserId), IsUnique = true)]
    public class ProfilePicture
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public int PictureId { get; set; }
        public Picture Picture { get; set; }
    }
}