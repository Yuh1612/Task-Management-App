namespace API.DTOs.Histories
{
    public class HistoryDTO
    {
        public string Ref { get; set; }

        public int? RefId { get; set; }

        public string Action { get; set; }

        public string? Message { get; set; }

        public string? contentJson { get; set; }

        public DateTime CreateAt { get; set; }

        public int CreateById { get; set; }
    }
}