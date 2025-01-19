using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HealthcareSystem.Models
{
    public class InsurancePolicy
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Auto-increment
        [Column("insurancepolicy_id")]
        public int InsurancePolicyID { get; set; }
        public string? PolicyNumber { get; set; }
        public string? CoverageType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal PremiumAmount { get; set; }

        [ForeignKey("policy_id")]
        public int? PolicyID { get; set; }
        [NotMapped]
        public ICollection<Claim>? Claims { get; set; }
    }
}

