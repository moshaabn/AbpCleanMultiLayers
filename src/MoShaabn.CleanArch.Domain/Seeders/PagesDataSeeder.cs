// using MoShaabn.CleanArch.Entities;
// using System;
// using System.Collections.Generic;
// using System.Threading.Tasks;
// using Volo.Abp.Data;
// using Volo.Abp.DependencyInjection;
// using Volo.Abp.Domain.Repositories;

// namespace MoShaabn.CleanArch.Seeders;

// public class PagesDataSeeder(IRepository<Page, Guid> pageRepo) : IDataSeedContributor, ITransientDependency
// {
//     public async Task SeedAsync(DataSeedContext context)
//     {
//         if (await pageRepo.AnyAsync())
//         {
//             return;
//         }

//         var pages = new List<Page>
//         {
//             new Page("privacy_policy", "Privacy Policy", "Privacy Policy Page"),
//             new Page("about_us", "About Us", "About Us Page"),
//             new Page("terms_and_conditions", "Terms And Conditions", "Terms And Conditions Page")
//         };

//         await pageRepo.InsertManyAsync(pages, true);
//     }
// }