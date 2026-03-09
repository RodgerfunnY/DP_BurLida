using System.ComponentModel.DataAnnotations;

namespace DP_BurLida.Data.ModelsData
{
    public class OrderCommentModelData
    {
        public int Id { get; set; }

        public int OrderId { get; set; }

        [MaxLength(2000)]
        public string Text { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [MaxLength(255)]
        public string? CreatedBy { get; set; }

        public bool IsDone { get; set; }

        public DateTime? DoneAt { get; set; }
    }
}

