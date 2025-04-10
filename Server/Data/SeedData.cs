using Server.Models.Account;
using Server.Models.Order;
using Server.Models.Product;
using Server.Models.User;
using Server.Services;

namespace Server.Data
{
    public class SeedData
    {
        private static readonly BcryptService _bcryptService = new();
        public static List<AccountModel> GetAccountSeedData()
        {
            var accounts = new List<AccountModel>();

            // Chỉ định số lượng tài khoản cho từng vai trò
            var predefinedRoles = new List<(string Role, int Count)>
                                {
                                    ("Admin", 1),      // 1 Admin
                                    ("Manager", 2),    // 2 Managers
                                    ("Trainer", 13),   // 13 Trainers
                                    ("Member", 14)     // 14 Members
                                };

            int userIndex = 1;

            foreach (var (role, count) in predefinedRoles)
            {
                for (int i = 0; i < count; i++, userIndex++)
                {
                    var accountId = Guid.NewGuid();
                    var userId = Guid.NewGuid();

                    var account = new AccountModel
                    {
                        Id = accountId,
                        Email = $"{role.ToLower()}{i + 1}@gymcenter.com",
                        Password = _bcryptService.HashPassword("123456"), // Hashed password
                        Role = role,
                        IsActivated = role is "Admin" or "Manager" || userIndex % 5 != 0, // Admin & Manager luôn kích hoạt
                        User = new UserModel
                        {
                            Id = userId,
                            FirstName = GetRandomFirstName(),
                            LastName = GetRandomLastName(),
                            DateOfBirth = GenerateRandomDateOfBirth(),
                            AccountId = accountId
                        }
                    };

                    accounts.Add(account);
                }
            }

            return accounts;
        }

        private static string GetRandomFirstName()
        {
            string[] firstNames = {
                "Minh", "Anh", "Hùng", "Lan", "Hoa",
                "Tuấn", "Linh", "Đức", "Mai", "Trang",
                "Quân", "Nga", "Khoa", "Hương", "Trung"
            };
            return firstNames[new Random().Next(firstNames.Length)];
        }

        private static string GetRandomLastName()
        {
            string[] lastNames = {
                "Nguyễn", "Trần", "Lê", "Phạm", "Hoàng",
                "Huỳnh", "Phan", "Võ", "Đặng", "Bùi"
            };
            return lastNames[new Random().Next(lastNames.Length)];
        }

        private static DateTime GenerateRandomDateOfBirth()
        {
            Random random = new Random();
            DateTime start = new DateTime(1970, 1, 1);
            int range = (DateTime.Today.AddYears(-18) - start).Days;
            return start.AddDays(random.Next(range));
        }

        public static List<ProductModel> GetProductSeedData()
        {
            string[] productNames = {
            "Luxury Bag", "Koenigsegg Jesko",
            "Rolex Watch", "Gucci Shoes", "iPhone 13 Pro",
            "Macbook Pro", "Samsung Galaxy S21", "Sony PS5"
        };

            string[] brands = {
            "Louis Vuitton", "Koenigsegg", "Rolex", "Gucci",
            "Apple", "Apple", "Samsung", "Sony"
        };

            string[] categories = {
            "Fashion", "Vehicles", "Accessories", "Fashion",
            "Smartphones", "Laptops", "Smartphones", "Gaming"
        };

            decimal[] prices = {
            1500m, 3000000m, 12000m, 800m, 999m, 2000m, 799m, 499m
        };

            List<ProductModel> products = new List<ProductModel>();

            for (int i = 0; i < productNames.Length; i++)
            {
                products.Add(new ProductModel
                {
                    Id = Guid.NewGuid(),
                    Name = productNames[i],
                    Brand = brands[i],
                    Description = $"{productNames[i]} from {brands[i]}",
                    Price = prices[i],
                    Category = categories[i],
                    Image = $"{productNames[i].ToLower().Replace(" ", "_")}.jpg",
                    Stock = new Random().Next(5, 50)
                });
            }

            return products;
        }
        public static List<OrderModel> GetOrderSeedData(List<ProductModel> products, List<AccountModel> accounts)
        {
            var orders = new List<OrderModel>();
            var random = new Random();

            // Get some users (Members) to create orders
            var memberUsers = accounts
                .Where(a => a.Role == "Member")
                .Select(a => a.User)
                .ToList();

            for (int i = 0; i < 10; i++) // Seed 10 orders
            {
                var user = memberUsers[random.Next(memberUsers.Count)];
                var orderId = Guid.NewGuid();
                int numberOfItems = random.Next(1, 4); // Each order has 1–3 items

                var orderItems = new List<OrderItemModel>();
                decimal itemPrice = 0;
                decimal itemShippingFee = 0;

                var selectedProducts = products.OrderBy(x => random.Next()).Take(numberOfItems).ToList();

                foreach (var product in selectedProducts)
                {
                    var quantity = random.Next(1, 3);
                    var price = product.Price;
                    var shippingFee = 9.99m;

                    orderItems.Add(new OrderItemModel
                    {
                        Id = Guid.NewGuid(),
                        Quantity = quantity,
                        ItemShippingFee = shippingFee,
                        ProductId = product.Id,
                        Product = product
                    });

                    itemPrice += price * quantity;
                    itemShippingFee += shippingFee;
                }

                var discount = random.Next(0, 2) == 0 ? 0 : 10m;
                var tax = itemPrice * 0.1m;
                var totalPrice = itemPrice - discount + tax + itemShippingFee;

                orders.Add(new OrderModel
                {
                    Id = orderId,
                    UserId = user.Id,
                    ShippingInfo = "123 Test Street, Hanoi",
                    ShippingStatus = "Pending",
                    PaymentMethod = "CreditCard",
                    PaymentStatus = "Pending",
                    CreatedAt = DateTime.UtcNow.AddDays(-random.Next(0, 30)),
                    Tax = tax,
                    Discount = discount,
                    ShippingFee = itemShippingFee,
                    TotalPrice = totalPrice,
                    OrderItems = orderItems
                });
            }

            return orders;
        }

    }
}
