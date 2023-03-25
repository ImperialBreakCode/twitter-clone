using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TwitterUni.Data.BaseEntities
{
    public abstract class BaseEntity : IEntityId, IEntityCreationInfo
    {
        public BaseEntity()
        {
            CreatedAt = DateTime.UtcNow;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
