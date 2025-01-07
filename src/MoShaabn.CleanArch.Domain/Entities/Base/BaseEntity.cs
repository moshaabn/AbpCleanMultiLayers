using System;
using Volo.Abp.Domain.Entities.Auditing;

namespace MoShaabn.CleanArch.Entities.Base;

public abstract class BaseEntity : FullAuditedEntity<Guid>
{
    protected BaseEntity()
    {
        HandleGuidPrimaryKeyGeneration();
    }

    private void HandleGuidPrimaryKeyGeneration()
    {
        GetType().GetProperty(nameof(Id))?.SetValue(this, Guid.NewGuid());
    }
}