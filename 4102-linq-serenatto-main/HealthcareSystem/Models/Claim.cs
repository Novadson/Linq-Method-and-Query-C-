using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HealthcareSystem.Models
{

    public class Claim
    {
        #region PROPERTIES
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Auto-increment
        public int ClaimID { get; set; }
      
        public DateTime DateOfClaim { get; set; }
        public decimal AmountClaimed { get; set; }
        public string? ClaimStatus { get; set; }
        #endregion 


        #region FOREIGNKEY

        [ForeignKey("patient_id")]
        public int PatientID { get; set; }
        [NotMapped]
        public virtual Patient? Patient { get; set; }
        [ForeignKey("policy_id")]
        public int PolicyID { get; set; }
        #endregion 
        [NotMapped]
        public InsurancePolicy? InsurancePolicy { get; set; }


       
        public virtual ICollection<DoctorClaim>? DoctorClaims { get; set; }
    }

}
