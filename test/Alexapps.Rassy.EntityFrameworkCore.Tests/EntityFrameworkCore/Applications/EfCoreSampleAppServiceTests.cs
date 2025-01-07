using MoShaabn.CleanArch.Samples;
using Xunit;

namespace MoShaabn.CleanArch.EntityFrameworkCore.Applications;

[Collection(CleanArchTestConsts.CollectionDefinitionName)]
public class EfCoreSampleAppServiceTests : SampleAppServiceTests<CleanArchEntityFrameworkCoreTestModule>
{

}
