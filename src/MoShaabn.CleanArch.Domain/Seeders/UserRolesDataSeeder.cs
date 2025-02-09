using MoShaabn.CleanArch.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Identity;

namespace MoShaabn.CleanArch.Seeders
{

    public class UserRolesDataSeeder : ITransientDependency
    {
        private readonly IdentityRoleManager roleManager;

        public UserRolesDataSeeder(IdentityRoleManager roleManager)
        {
            this.roleManager = roleManager;
        }

        public async Task SeedAsync(DataSeedContext context)
        {

            // Define the roles to be seeded
            var roles = new List<IdentityRole>
        {
            new IdentityRole(Guid.NewGuid(), RoleEnum.ADMIN),
            new IdentityRole(Guid.NewGuid(), RoleEnum.USER)
        };

            // Iterate through each role and create it
            foreach (var role in roles)
            {
                var existingRole = await roleManager.FindByNameAsync(role.Name);
                if (existingRole == null)
                {
                    await roleManager.CreateAsync(role);
                }
            }
        }
    }

}
