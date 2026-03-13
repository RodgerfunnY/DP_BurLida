using System.ComponentModel.DataAnnotations;

namespace DP_BurLida.Api.Dtos
{
    public record OrderListItemResponse(
        int Id,
        string NameClient,
        string Phone,
        string? City,
        string Status,
        string? CreatedBy,
        DateTime CreationTimeData,
        DateTime? WorkDate,
        DateTime? ArrangementDate
    );

    public record OrderDetailsResponse(
        int Id,
        string NameClient,
        string? SurnameClient,
        string Phone,
        string? Area,
        string? District,
        string? City,
        int Diameter,
        int PricePerMeter,
        int Pump,
        int? MetersCount,
        string? Depth,
        string? StaticLevel,
        string? DynamicLevel,
        string? Filter,
        string? PumpModel,
        string? Arrangement,
        string? PumpInstalled,
        string? ArrangementDone,
        bool IsDrillingInstallment,
        int? DrillingFirstContribution,
        int? DrillingFirstPayment,
        DateTime? DrillingFirstPaymentDueDate,
        int? DrillingSecondPayment,
        DateTime? DrillingSecondPaymentDueDate,
        int? DrillingThirdPayment,
        DateTime? DrillingThirdPaymentDueDate,
        int? DrillingFourthPayment,
        DateTime? DrillingFourthPaymentDueDate,
        bool IsArrangementInstallment,
        int? ArrangementFirstContribution,
        int? ArrangementFirstPayment,
        DateTime? ArrangementFirstPaymentDueDate,
        int? ArrangementSecondPayment,
        DateTime? ArrangementSecondPaymentDueDate,
        int? ArrangementThirdPayment,
        DateTime? ArrangementThirdPaymentDueDate,
        int? ArrangementFourthPayment,
        DateTime? ArrangementFourthPaymentDueDate,
        string? TotalDrillingAmount,
        string? TotalArrangementAmount,
        string? InstallmentEripNumber,
        string Info,
        string Status,
        string? BrigadeStatus,
        string? CreatedBy,
        DateTime CreationTimeData,
        DateTime? WorkDate,
        DateTime? ArrangementDate,
        string? Contractor,
        string? Coordinates,
        string? Sewer,
        int? DrillingBrigadeId,
        int? ArrangementBrigadeId
    );

    public class OrderUpsertRequest
    {
        [Required]
        public string NameClient { get; set; } = string.Empty;

        public string? SurnameClient { get; set; }

        [Required]
        public string Phone { get; set; } = string.Empty;

        public string? Area { get; set; }
        public string? District { get; set; }
        public string? City { get; set; }

        public int Diameter { get; set; }
        public int PricePerMeter { get; set; }
        public int Pump { get; set; }

        public int? MetersCount { get; set; }
        public string? Depth { get; set; }
        public string? StaticLevel { get; set; }
        public string? DynamicLevel { get; set; }
        public string? Filter { get; set; }
        public string? PumpModel { get; set; }
        public string? Arrangement { get; set; }
        public string? PumpInstalled { get; set; }
        public string? ArrangementDone { get; set; }

        public bool IsDrillingInstallment { get; set; }
        public int? DrillingFirstContribution { get; set; }
        public int? DrillingFirstPayment { get; set; }
        public DateTime? DrillingFirstPaymentDueDate { get; set; }
        public int? DrillingSecondPayment { get; set; }
        public DateTime? DrillingSecondPaymentDueDate { get; set; }
        public int? DrillingThirdPayment { get; set; }
        public DateTime? DrillingThirdPaymentDueDate { get; set; }
        public int? DrillingFourthPayment { get; set; }
        public DateTime? DrillingFourthPaymentDueDate { get; set; }

        public bool IsArrangementInstallment { get; set; }
        public int? ArrangementFirstContribution { get; set; }
        public int? ArrangementFirstPayment { get; set; }
        public DateTime? ArrangementFirstPaymentDueDate { get; set; }
        public int? ArrangementSecondPayment { get; set; }
        public DateTime? ArrangementSecondPaymentDueDate { get; set; }
        public int? ArrangementThirdPayment { get; set; }
        public DateTime? ArrangementThirdPaymentDueDate { get; set; }
        public int? ArrangementFourthPayment { get; set; }
        public DateTime? ArrangementFourthPaymentDueDate { get; set; }

        public string? TotalDrillingAmount { get; set; }
        public string? TotalArrangementAmount { get; set; }
        public string? InstallmentEripNumber { get; set; }

        [Required]
        public string Info { get; set; } = string.Empty;

        [Required]
        public string Status { get; set; } = string.Empty;

        public string? BrigadeStatus { get; set; }
        public DateTime? WorkDate { get; set; }
        public DateTime? ArrangementDate { get; set; }
        public string? Contractor { get; set; }
        public string? Coordinates { get; set; }
        public string? Sewer { get; set; }

        public int? DrillingBrigadeId { get; set; }
        public int? ArrangementBrigadeId { get; set; }
    }
}

