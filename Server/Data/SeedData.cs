using Server.Models.Account;
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
                            Business = GetRandomSpecialization(),
                            DateOfBirth = GenerateRandomDateOfBirth(),
                            AccountId = accountId
                        }
                    };

                    accounts.Add(account);
                }
            }

            return accounts;
        }

        private static AccountModel CreateAccount(string email, string role)
        {
            var accountId = Guid.NewGuid();
            var userId = Guid.NewGuid();

            return new AccountModel
            {
                Id = accountId,
                Email = email,
                Password = _bcryptService.HashPassword("123456"), // Hashed password
                Role = role,
                IsActivated = true, // Mặc định tất cả tài khoản được kích hoạt
                User = new UserModel
                {
                    Id = userId,
                    FirstName = GetRandomFirstName(),
                    LastName = GetRandomLastName(),
                    Business = GetRandomSpecialization(),
                    DateOfBirth = GenerateRandomDateOfBirth(),
                    AccountId = accountId
                }
            };
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

        private static string GetRandomSpecialization()
        {
            string[] specializations = {
                "Huấn luyện Cardio", "Yoga", "Gym Fitness",
                "Pilates", "Zumba", "Strength Training",
                "Spinning", "Personal Training", "No Specialization"
            };
            return specializations[new Random().Next(specializations.Length)];
        }

        private static DateTime GenerateRandomDateOfBirth()
        {
            Random random = new Random();
            DateTime start = new DateTime(1970, 1, 1);
            int range = (DateTime.Today.AddYears(-18) - start).Days;
            return start.AddDays(random.Next(range));
        }
    }
}
