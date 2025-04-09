using Server.Models.Product;
using Server.Models.User;

namespace Server.Models.Review
{
    public class ReviewModel
    {
        public Guid Id { get; set; }
        public int Rating { get; set; }
        public string? Comment { get; set; }
        public DateTime CreatedAt { get; set; }

        //Foreign key to properties
        public Guid UserId { get; set; }
        public UserModel? User { get; set; }

        public Guid ProductId { get; set; }
        public ProductModel? Product { get; set; }
    }
}
