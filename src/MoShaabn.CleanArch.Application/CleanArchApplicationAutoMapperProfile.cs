using AutoMapper;
using Volo.Abp.Application.Dtos;

namespace MoShaabn.CleanArch;

public class CleanArchApplicationAutoMapperProfile : Profile
{
    public CleanArchApplicationAutoMapperProfile()
    {
        /* You can configure your AutoMapper mapping configuration here.
         * Alternatively, you can split your mapping configurations
         * into multiple profile classes for a better organization. */
        CreateMap(typeof(PagedResultDto<>), typeof(PagedResultDto<>));

    }
}
