using Volo.Abp.Application.Dtos;

namespace MoShaabn.CleanArch.Extensions
{
    public class FilterPagedRequest : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
    }
}
