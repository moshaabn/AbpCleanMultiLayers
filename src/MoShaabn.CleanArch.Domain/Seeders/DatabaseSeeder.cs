using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;

namespace MoShaabn.CleanArch.Seeders
{
    public class DatabaseSeeder(
    PagesDataSeeder pagesDataSeeder,
    UserRolesDataSeeder userRolesSeeder,
    UsersDataSeeder usersDataSeeder
) : IDataSeedContributor, ITransientDependency
    {
        public async Task SeedAsync(DataSeedContext context)
        {
            await pagesDataSeeder.SeedAsync(context);
            await userRolesSeeder.SeedAsync(context);
            await usersDataSeeder.SeedAsync(context);
        }
    }
}