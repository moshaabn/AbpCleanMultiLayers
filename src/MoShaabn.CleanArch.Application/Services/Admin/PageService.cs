// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Threading.Tasks;

// namespace MoShaabn.CleanArch.Services.Admin
// {
//     public class PageService(
//     IRepository<Page, Guid> pageRepo,
//     IStringLocalizer<LocalizationResource> localizer
// ) : BaseAppService, IPageService
//     {
//         public async Task<PageResult> GetBySlugAsync(string slug)
//         {
//             var page = await (await pageRepo.GetQueryableAsync())
//                 .FirstOrDefaultAsync(x => x.Slug == slug);

//             if (page == null)
//             {
//                 throw new UserFriendlyException(localizer["page-not-found"]);
//             }

//             var result = ObjectMapper.Map<Page, PageResult>(page);

//             return result;
//         }
//         public async Task<PageResult> UpdateAsync(string slug, PageCommand command)
//         {
//             var page = await (await pageRepo.GetQueryableAsync())
//                 .FirstOrDefaultAsync(x => x.Slug == slug);

//             if (page == null)
//             {
//                 throw new UserFriendlyException(localizer["page-not-found"]);
//             }


//             page.Description = command.Description;

//             page = await pageRepo.UpdateAsync(page, true);

//             var result = ObjectMapper.Map<Page, PageResult>(page);

//             return result;
//         }
//     }
// }