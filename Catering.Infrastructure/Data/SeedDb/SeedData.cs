using Microsoft.AspNetCore.Identity;

namespace Catering.Infrastructure.Data.SeedDb
{
    internal class SeedData
    {
        public IdentityRole Admin { get; set; } = null!;
        public IdentityRole Moderator { get; set; } = null!;
        public IdentityRole RestaurantOwner { get; set; } = null!;
        public IdentityRole User { get; set; } = null!;

        public SeedData()
        {
            SeedRoles();
        }

        private void SeedRoles()
        {
            Admin = new IdentityRole
            {
                Id = "6d5800ce-d726-4fc8-83d9-d6b3ac1f591e",
                Name = "Admin",
                NormalizedName = "ADMIN",
                ConcurrencyStamp = "cd76a1be-26ad-4f32-b29d-f0c0d5f7cd37"
            };

            Moderator = new IdentityRole
            {
                Id = "9054b99c-81ba-465d-bb62-606df48b58b9",
                Name = "Moderator",
                NormalizedName = "MODERATOR",
                ConcurrencyStamp = "76a5f411-9a1a-4318-b2d0-c826fc86b32d"
            };

            RestaurantOwner = new IdentityRole
            {
                Id = "32dbb61d-0b46-4c77-8449-f3a633b6a72b",
                Name = "RestaurantOwner",
                NormalizedName = "RESTAURANTOWNER",
                ConcurrencyStamp = "459d0654-cabb-4c9d-bc6d-b81d8d72cb51"
            };

            User = new IdentityRole
            {
                Id = "b9711b31-d6cf-4c89-b7c0-9634db87154d",
                Name = "User",
                NormalizedName = "USER",
                ConcurrencyStamp = "8d103fbd-2ede-4e27-ade3-3d0c959935b1"
            };
        }
    }
}
