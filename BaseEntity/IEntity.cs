using System;

namespace RepositoryLogic.BaseEntity
{
    public interface IEntity : ICloneable
    {
        int id { get; }

        string AAA_EntityName { get; }
    }
}
