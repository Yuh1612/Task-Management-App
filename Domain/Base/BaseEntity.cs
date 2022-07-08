using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Base
{
    public interface IBaseEntity<Tkey>
    {
        public Tkey Id { get; set; }
    }

    public interface IAuditEntity
    {
        public DateTime CreateDate { get; set; }
        public int CreatedById { get; set; }
        public DateTime UpdateDate { get; set; }
        public int UpdatedById { get; set; }
    }

    public interface IDeleteEntity
    {
        public bool IsDelete { get; set; }
    }

    public abstract class AuditEntity : IAuditEntity
    {
        public virtual DateTime CreateDate { get; set; }
        public virtual int CreatedById { get; set; }
        public virtual DateTime UpdateDate { get; set; }
        public virtual int UpdatedById { get; set; }
    }

    public abstract class DeleteEntity : AuditEntity, IDeleteEntity
    {
        public virtual bool IsDelete { get; set; }
    }

    public abstract class BaseEntity : DeleteEntity
    {
        [NotMapped]
        private List<BaseDomainEvent> _events;

        [NotMapped]
        public IReadOnlyList<BaseDomainEvent> Events => _events?.AsReadOnly();

        protected void AddEvent(BaseDomainEvent @event)
        {
            _events = _events ?? new List<BaseDomainEvent>();
            _events.Add(@event);
        }

        protected void RemoveEvent(BaseDomainEvent @event)
        {
            _events.Remove(@event);
        }

        public void ClearEvents()
        {
            _events?.Clear();
        }
    }

    public abstract class BaseEntity<TKey> : BaseEntity, IBaseEntity<TKey>
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual TKey Id { get; set; }
    }
}