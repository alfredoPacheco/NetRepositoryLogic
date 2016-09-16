using RepositoryLogic.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace RepositoryLogic.BaseEntity
{
    public abstract class Document : Entity, ITrackable
    {
        [NotMapped]
        public Track InfoTrack { get; set; }

        public virtual bool sys_active { get; set; }
    }
}
