namespace Pawgress.Dtos
{
    public class ProgressDto
    {
        public string ModuleName { get; set; }
        public int CompletedItems { get; set; }
        public int TotalItems { get; set; }
        public string Status { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? CompletionDate { get; set; }
        public List<ItemProgressDto> ItemProgress { get; set; } = new List<ItemProgressDto>();
        public double PercentageComplete { get; set; }
    }

    public class ItemProgressDto
    {
        public Guid ItemId { get; set; }
        public string ItemType { get; set; }
        public string Name { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime? CompletedDate { get; set; }
        public int? Score { get; set; }
    }
} 