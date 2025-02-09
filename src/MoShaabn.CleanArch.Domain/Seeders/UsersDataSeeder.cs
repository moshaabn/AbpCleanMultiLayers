using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MoShaabn.CleanArch.Entities.Users;
using MoShaabn.CleanArch.Enums;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Identity;

namespace MoShaabn.CleanArch.Seeders
{
    public class UsersDataSeeder(IdentityUserManager identityUserManager) : ITransientDependency
    {

        public async Task SeedAsync(DataSeedContext context)
        {
            var existingUser = await identityUserManager.FindByEmailAsync("admin@senwan.com");
            if (existingUser == null)
            {
                var user = new User("admin", "000000", "admin@rassi.com", "admin@rassi.com");
                await identityUserManager.CreateAsync(user, "secret");
                // Create the user with the specified password
                var createResult = await identityUserManager.CreateAsync(user, "Secret@1234");
                if (createResult.Succeeded)
                {
                    await identityUserManager.AddToRoleAsync(user, RoleEnum.ADMIN);
                }
            }
            else
            {
                if (!await identityUserManager.IsInRoleAsync(existingUser, RoleEnum.ADMIN))
                {
                    await identityUserManager.AddToRoleAsync(existingUser, RoleEnum.ADMIN);
                }
            }
        }
    }
}