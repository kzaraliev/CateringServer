using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catering.Infrastructure.Data.SeedDb
{
    internal class RoleConfiguration : IEntityTypeConfiguration<IdentityRole>
    {
        public void Configure(EntityTypeBuilder<IdentityRole> builder)
        {
            var data = new SeedData();

            builder.HasData(new IdentityRole[] { data.Admin, data.Moderator, data.RestaurantOwner, data.User });
        }
    }
}
