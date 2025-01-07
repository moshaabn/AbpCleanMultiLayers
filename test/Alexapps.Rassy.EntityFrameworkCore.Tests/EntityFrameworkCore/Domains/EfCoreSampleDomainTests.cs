using MoShaabn.CleanArch.Samples;
using Xunit;

namespace MoShaabn.CleanArch.EntityFrameworkCore.Domains;

[Collection(CleanArchTestConsts.CollectionDefinitionName)]
public class EfCoreSampleDomainTests : SampleDomainTests<CleanArchEntityFrameworkCoreTestModule>
{

}
