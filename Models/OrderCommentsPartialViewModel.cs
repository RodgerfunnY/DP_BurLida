using DP_BurLida.Data.ModelsData;

namespace DP_BurLida.Models
{
    public class OrderCommentsPartialViewModel
    {
        public int OrderId { get; set; }
        public string? ReturnUrl { get; set; }
        public List<OrderCommentModelData> Comments { get; set; } = new();
    }
}

