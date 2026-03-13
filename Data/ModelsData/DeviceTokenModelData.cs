using System.ComponentModel.DataAnnotations;

namespace DP_BurLida.Data.ModelsData
{
    public class DeviceTokenModelData
    {
        public int Id { get; set; }

        [MaxLength(255)]
        public string UserEmail { get; set; } = string.Empty;

        [MaxLength(2048)]
        public string Token { get; set; } = string.Empty;

        [MaxLength(50)]
        public string Platform { get; set; } = "android";

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}

