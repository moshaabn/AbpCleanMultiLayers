using MoShaabn.CleanArch.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;

namespace MoShaabn.CleanArch.Seeders;

public class PagesDataSeeder(IRepository<Page, Guid> pageRepo) : ITransientDependency
{
    public async Task SeedAsync(DataSeedContext context)
    {
        if (await pageRepo.AnyAsync())
        {
            return;
        }

        var pages = new List<Page>
        {
            new Page("privacy_policy", "سياسة الخصوصية", "Privacy Policy", "صفحة سياسة الخصوصية", "Privacy Policy Page"),
            new Page("about_us", "من نحن", "About Us", "صفحة من نحن", "About Us Page"),
            new Page("terms_and_conditions", "الشروط والأحكام", "Terms And Conditions", "صفحة الشروط والأحكام", "Terms And Conditions Page")
        };

        await pageRepo.InsertManyAsync(pages, true);
    }
}