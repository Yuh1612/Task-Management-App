using Microsoft.EntityFrameworkCore.ChangeTracking;
using Newtonsoft.Json;

namespace Domain.Histories
{
    public class HistoryEntry
    {
        public HistoryEntry(EntityEntry entry)
        {
            Entry = entry;
        }

        public EntityEntry Entry { get; }
        public Guid? UserId { get; set; }
        public string Ref { get; set; }
        public Dictionary<string, object?> RefId { get; } = new Dictionary<string, object?>();
        public Dictionary<string, object?> OldValues { get; } = new Dictionary<string, object?>();
        public Dictionary<string, object?> NewValues { get; } = new Dictionary<string, object?>();
        public ActionType ActionType { get; set; }
        public List<string> ChangedColumns { get; } = new List<string>();

        public History ToHistory()
        {
            var history = new History();
            history.UserId = UserId;
            history.Action = ActionType.ToString();
            history.Ref = Ref;
            history.CreateAt = DateTime.UtcNow;
            history.RefId = JsonConvert.SerializeObject(RefId);
            history.OldValues = OldValues.Count == 0 ? null : JsonConvert.SerializeObject(OldValues);
            history.NewValues = NewValues.Count == 0 ? null : JsonConvert.SerializeObject(NewValues);
            history.AffectedColumns = ChangedColumns.Count == 0 ? null : JsonConvert.SerializeObject(ChangedColumns);
            return history;
        }
    }
}