// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Threading.Tasks;

// namespace MoShaabn.CleanArch.Controllers.Admin
// {
//     [Authorize(Roles = RoleEnum.ADMIN)]
//     [Tags("Admin Pages")]
//     public class PagesController(IPageService pageService) : OutletNetController
//     {
//         [HttpGet(AdminRoutes.Pages.GetBySlug)]
//         public async Task<ActionResult<PageResult>> GetBySlugAsync([FromRoute] string slug)
//         {
//             var response = await pageService.GetBySlugAsync(slug);
//             return Ok(response);
//         }

//         [HttpPut(AdminRoutes.Pages.Update)]
//         public async Task<ActionResult<PageResult>> UpdateAsync([FromRoute] string slug, [FromBody] PageCommand command)
//         {
//             var response = await pageService.UpdateAsync(slug, command);
//             return Ok(response);
//         }
//     }
// }