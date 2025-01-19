using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HealthcareSystem.Models
{
    public class DoctorClaim
    {
        #region PROPERTIES

        [Key]
        [Column("doctorclaim_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Auto-increment
        public int DoctorClaimID { get; set; }

        #endregion

        #region ForeignKey
        [ForeignKey("claimid")]
        public int ClaimID { get; set; }
        public Claim? Claim { get; set; }

        [NotMapped]
        public  string? FirstName { get; set; }
        [NotMapped]
        public  string? Specialty { get; set; }


        [ForeignKey("doctorid")]
        public int DoctorID { get; set; }
        public Doctors? Doctor { get; set; }
        #endregion ForeignKey

    }
}
