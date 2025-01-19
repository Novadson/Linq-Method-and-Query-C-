using Arch.EntityFrameworkCore;
using HealthcareSystem.DataServices;
using HealthcareSystem.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static Azure.Core.HttpHeader;




namespace HealthcareSystem.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HealthCareController : ControllerBase
    {

        private readonly ApplicationDbContext _context;
        public HealthCareController(ApplicationDbContext application)
        {
            _context = application;
        }

        //https://www.youtube.com/watch?v=SqcY0GlETPk React Course


        // 1- Find all patients who have made claims.

        [HttpGet("getallpatientswithclaimsasync")]

        public async Task<List<Patient>> GetAllPatientsWithClaimsAsync()
        {

            List<Patient> patiensList = _context.Patients.ToList();
            List<Claim> claimsList = _context.Claims.ToList();
            // Ensure we query the database efficiently
            List<Patient> patientsWithClaims = await (from patient in _context.Patients
                                                      join claim in _context.Claims
                                                      on patient.PatientID equals claim.PatientID
                                                      select patient).ToListAsync();

            // Throw an exception if no data is found
            if (!patientsWithClaims.Any())
                throw new InvalidOperationException("No patients with claims found. Please ensure data exists.");

            return patientsWithClaims;
        }


        //List all insurance policies with a premium greater than $200.
        [HttpGet("getinsurancepoliciesasync")]
        public async Task<List<InsurancePolicy>> GetInsurancePoliciesAsync()
        {


            List<InsurancePolicy>? insurancePolicyMethod = await _context.InsurancePolicies
                                                                         .Where(ins => ins.PremiumAmount > 200)
                                                                         .ToListAsync();// linq method


            List<InsurancePolicy>? insurancePolicyQuery = await (from ins in _context.InsurancePolicies // linq query using collection expression
                                                                 where ins.PremiumAmount > 200
                                                                 select ins).ToListAsync();

            if (insurancePolicyMethod.Count() > 0)
                throw new InvalidOperationException("There is no  insurance policies with a premium greater than $200");


            return insurancePolicyMethod;
        }



        // 3- Get the names of patients who have made a claim, ordered by their last name.

        [HttpGet("getpatientsasync")]
        public async Task<object[]> GetPatientsAsync()
        {
            var patients = await (from patient in _context.Patients
                                  join claim in _context.Claims
                                  on patient.PatientID equals claim.PatientID
                                  where claim.PatientID != 0
                                  orderby patient.LastName
                                  select new
                                  {
                                      patient.FirstName,
                                      patient.LastName
                                  }).ToArrayAsync();

            if (!patients.Any())
                throw new InvalidOperationException("There no patients who have made a claim");


            return patients;
        }

        // 4- Count the total number of claims made for each insurance policy.

        [HttpGet("gettotaclaimsforeachpolicyasync")]
        public async Task<IActionResult> GetTotaClaimsForEachPolicyAsync()
        {
            var totalClaim = await (from claim in _context.Claims
                                    group claim by claim.ClaimID into groupedClaims
                                    select new
                                    {
                                        PolicyId = groupedClaims.Key,
                                        ClaimCount = groupedClaims.Count()
                                    }).ToListAsync();

            return Ok(totalClaim);

        }

        // 5- Calculate the total amount claimed across all claims.

        [HttpGet("calculatetotalamountasync")]
        public async Task<decimal> CalculateTotalAmountAsync()
        {
            decimal totalAmountClaimedQuery = await Task.FromResult((from c in _context.Claims // query linq
                                                                     select c.AmountClaimed).Sum());

            decimal totalAmountClaimedMethod = _context.Claims.Sum(c => c.AmountClaimed); // linq method


            return totalAmountClaimedQuery;
        }

        // Find the claim with the highest amount claimed
        [HttpGet("gethighestamoutclaimedasync")]
        public async Task<decimal> GetHighestAmoutClaimedAsync() =>
                                   await Task.FromResult(_context.Claims
                                                                 .Max(c => c.AmountClaimed));

        //List all doctors who have attended to claims related to 'Cardiology.'
        [HttpGet("getdotorsattendedclaimscardiologyasync")]
        public async Task<List<Doctors>> GetDotorsAttendedClaimsCardiologyAsync()
        {
            Task<List<Doctors>>? doctors = await Task.FromResult(_context.Doctors?
                                                     .Where(d => d.Specialty
                                                     .Equals("Cardiology"))
                                                     .ToListAsync());
            if (!doctors.Result.Any())
                throw new InvalidOperationException("There is no Doctor who attended Cardiology");


            return doctors.Result;
        }
        // 8- Find all policies that start before '2024-01-01'
        [HttpGet("getinsurancepoliciesbydateasync")]
        public async Task<List<InsurancePolicy>> GetInsurancePoliciesByDateAsync()
        {
            var insurancePolicies = await Task.FromResult(_context.InsurancePolicies?
                                    .Where(i => i.StartDate < Convert.ToDateTime("2024-01- 01")).ToListAsync());

            if (insurancePolicies.Result.Count < 0)
                throw new InvalidOperationException("There is no date started before 2024-01-01");

            return insurancePolicies.Result;
        }


        // 8- Show all patients who do not have a phone number listed.
        [HttpGet("getpatientswithphonenumberasync")]
        public async Task<List<Patient>> GetPatientsWithPhoneNumberAsync()
        {
            var patientsWithOutPhoneNumber = await _context.Patients
                                               .Where(p => p.PhoneNumber == null || p.PhoneNumber
                                               .Equals(""))
                                               .ToListAsync();
            if (!patientsWithOutPhoneNumber.Any())
                throw new ArgumentException("All patiens have phone number listed");

            return patientsWithOutPhoneNumber;
        }

        // Select the top 5 most recent claims by date.
        [HttpGet("getfivesmostrecentclaimsasync")]
        public async Task<List<Claim>> GetFivesMostRecentClaimsAsync()
        {
            var FiveMostRecentClaims = await Task.FromResult((_context.Claims
                                                 .OrderByDescending(c => c.DateOfClaim))
                                                 .Take(5).ToList());

            return FiveMostRecentClaims;
        }
        //Update the status of the claim for patient 'Jane Smith' to 'Approved.'

        [HttpGet("updatepatient")]
        public ActionResult UpdatePatient()
        {
            var patientClaims = (_context.Patients.Where(p => p.FirstName == "Jane" && p.LastName == "Smith")
                                                  .Include(c => c.Claims))
                                                  .SelectMany(c => c.Claims);

            if (patientClaims.Any())
            {
                foreach (var claim in patientClaims)
                {
                    claim.ClaimStatus = "Approved";
                    _context.SaveChanges();
                }
            }

            return Ok(patientClaims);
        }


        //Delete any claim with a status of 'Pending.'
        [HttpDelete("DeleteClaimStatusPedingAsync")]
        public async Task<ActionResult> DeleteClaimStatusPedingAsync(string statu)
        {
            var claimsPeding = await _context.Claims.Where(c => c.ClaimStatus == statu).ToListAsync();

            if (claimsPeding.Any())
                foreach (var clain in claimsPeding)
                {
                    _context.Remove(clain);
                    _context.SaveChanges();
                }

            return Ok(claimsPeding);

        }

        // 13- Calculate the average claim amount for each policy.

        [HttpGet("getaverageamoutforeachclaim")]
        public async Task<ActionResult> GetAverageAmoutForEachClaim()
        {

            var averageAmount = await (from policy in _context.InsurancePolicies
                                       join claim in _context.Claims
                                       on policy.PolicyID equals claim.PolicyID
                                       group new { policy, claim } by claim.PolicyID into collectionList
                                       select new
                                       {
                                           PolicyDetails = collectionList.Select(p => p.policy).FirstOrDefault(),
                                           AverageAmount = collectionList.Average(c => c.claim.AmountClaimed)
                                       }).ToListAsync();


            if (!averageAmount.Any())
                throw new InvalidOperationException("We do not have claim amount available now.");

            return Ok(averageAmount);
        }

        //14- Find all patients whose first name starts with 'J'.
        [HttpGet("getpatientsasyncstartwithj")]
        public async Task<ActionResult> GetPatientsAsyncStartWithJ()
        {
            List<Patient>? patients = await _context.Patients.Where(p => p.FirstName
                                            .StartsWith("J"))
                                            .ToListAsync();
            if (!patients.Any())
                throw new InvalidOperationException("There no patient wirh First Name started with J.");

            return Ok(patients);
        }

        //15- Get all claims where the amount claimed is between two amounts
        [HttpGet("getamountbetweenamount")]
        public async Task<ActionResult> GetAmountBetweenAmount(decimal amountOne, decimal amountTwo)
        {

            List<Claim>? claims = await (_context.Claims.Where(c => c.AmountClaimed >= amountOne &&
                                                 c.AmountClaimed <= amountTwo).ToListAsync());

            if (!claims.Any())
                throw new InvalidOperationException($"There is no Amount Claimed " +
                                                    $"between those {amountOne}|{amountTwo}  amount");

            return Ok(claims);
        }

        // 16-List all policies that contain the word 'Health' in the coverage type.
        [HttpGet("getinsurancepolicyhealthasync")]
        public ActionResult GetInsurancePolicyHealthAsync(string coverageType)
        {
            var insurancePolicy = _context.InsurancePolicies
                                                .Where(i => i.CoverageType
                                                .Contains(coverageType))
                                                .ToList();
            if (!insurancePolicy.Any())
                throw new InvalidOperationException("There is no Policies with coverage type Health");

            return Ok(insurancePolicy);
        }

        // 17- Join the Patients table with the Claims table to display patient names and claim details.

        [HttpGet("getpatientsandclaimsdetials")]
        public IActionResult GetPatientsAndClaimsDetials()
        {

            var patientsAndClaimsDetials = from patient in _context.Patients
                                           join claim in _context.Claims
                                           on patient.PatientID equals claim.PatientID
                                           group new { patient, claim } by patient.PatientID into collectionList
                                           select new
                                           {
                                               patient = collectionList.Select(p => p.patient).FirstOrDefault(),
                                               claimDetails = (collectionList.Select(c => c.claim).ToList()),
                                               Qttclaims = collectionList.Select(c => c.claim).Count()
                                           };

            return Ok(patientsAndClaimsDetials);
        }
        // 17- Join the Patients table with the Claims table to display patient names and claim details.
        [HttpGet("getpatientwithnoclaim")]
        public IActionResult GetPatientWithNoClaim()
        {

            List<int>? resExpectionIds = (from patient in _context.Patients
                                          select patient.PatientID)
                                  .Except(_context.Claims
                                  .Select(c => c.PatientID))
                                  .ToList();

            List<Patient>? patients = _context.Patients.Where(patient => resExpectionIds
                                                       .Contains(patient.PatientID))
                                                       .ToList();

            return Ok(patients);
        }

        // 21 Perform a SELF JOIN to listNumber patients who share the same last name.
        [HttpGet("getpatientswithsamelastname")]
        public IActionResult GetPatientsWithSameLastName()
        {
            //using Linq query
            var patientsSameLastName = _context.Patients
                              .Where(p => _context.Patients
                              .Any(p2 => p2.LastName == p.LastName && p.PatientID != p2.PatientID))
                              .ToList();

            var patients = from patient in _context.Patients
                           group patient by patient.LastName into collectionList
                           where collectionList.Count() > 1
                           select new
                           {
                               LastName = collectionList.Key,
                               PatientFullName = collectionList
                               .Select(p => $"{p.FirstName} {collectionList.Key}")
                               .ToList()
                           };

            return Ok(patients);
        }

        //Combine the data of doctors who specialize in 'Cardiology' and 'Dentistry' using UNION.
        [HttpGet("getcombineinformation")]
        public IActionResult GetCombineInformation()
        {
            var doctorsCardiologyAndDentistry = (_context.Doctors
                                                 .Where(d => d.Specialty
                                                 .Equals("Cardiology"))
                                                 .Union(_context.Doctors
                                                 .Where(d => d.Specialty
                                                 .Equals("Dentistry")))).ToList();

            // best solution
            var bestSolution = _context.Doctors.Where(d => d.Specialty
                                               .Equals("Cardiology") || d.Specialty
                                               .Equals("Dentistry"));

            return Ok(doctorsCardiologyAndDentistry);
        }

        //21 Use GROUP BY to get the number of claims per patient.

        [HttpGet("numberclaimperpatient")]
        public IActionResult GNumberClaimPerPatient()
        {

            var numberClaimPerPatient = from patient in _context.Patients
                                        join claim in _context.Claims
                                        on patient.PatientID equals claim.PatientID
                                        group new { patient, claim }
                                        by claim.PatientID into collectionInfo
                                        select new
                                        {
                                            PatientFullName = collectionInfo
                                            .Select(p => $"{p.patient.FirstName} {p.patient.LastName}"),
                                            claimNumber = collectionInfo.Count()
                                        };

            return Ok(numberClaimPerPatient);
        }

        //21 List patients who have made more than 1 claim.
        [HttpGet("getlistpatienthasonemoreclaim")]
        public IActionResult GetListPatientHasOneMoreClaim()
        {



            var listPatients = from patient in _context.Patients
                               join claim in _context.Claims
                               on patient.PatientID equals claim.PatientID
                               group new { patient, claim }
                               by claim.PatientID into claimclaim
                               where claimclaim.Count() > 1
                               select new
                               {
                                   PatientFullName = claimclaim
                                   .Select(p => $"{p.patient.FirstName} {p.patient.LastName}"),
                                   QttClaims = claimclaim.Count()
                               };


            return Ok(listPatients);
        }

        //---------------------PART TWO PRACTIVE LINQ QUERY / METHOD------------------------------
        //22
        [HttpGet("getclaimstatusapproved")]
        public List<Claim> GetClaimStatusApproved(string status)
        {
            var claimStatusApproved = (from claim in _context.Claims
                                       where claim.ClaimStatus == status
                                       select claim).ToList();

            if (!claimStatusApproved.Any())
                throw new InvalidOperationException("There no claim with status approved.");

            return claimStatusApproved;
        }

        //23 Group claims by PatientID and count how many claims each patient has.
        [HttpGet("groupclaimsbypatientid")]
        public IActionResult GroupClaimsByPatientID()
        {
            var claimResult = from claim in _context.Claims
                              join patient in _context.Patients
                              on claim.PatientID equals patient.PatientID
                              group new { claim, patient } by claim.PatientID into claimList
                              select new
                              {
                                  patient = claimList.Select(p =>
                                  $"{p.patient.FirstName} {p.patient.LastName}"),

                                  numberOfClaim = claimList.Count()
                              };

            if (!claimResult.Any())
                throw new InvalidOperationException("");

            return Ok(claimResult);
        }


        //24 Group claims by PolicyID and calculate the total AmountClaimed.
        [HttpGet("getamountclaimedperpolicy")]
        public object GetAmountClaimedPerPolicy()
        {

            var claimedPerPolicy = (from claim in _context.Claims
                                    group new { claim } by claim.PolicyID into claimList
                                    select new
                                    {
                                        PolicyId = claimList.Key,
                                        AmountTotalPerPolicy = claimList.Sum(p => p.claim.AmountClaimed)

                                    }).ToList();


            if (!claimedPerPolicy.Any())
                throw new InvalidOperationException("");

            return claimedPerPolicy;
        }


        //25 Create a list of anonymous objects that combine ClaimID, DateOfClaim, PatientID, and Patient.c.
        [HttpGet("patientsclaims")]
        public object PatientsClaims()
        {
            var listOfAnonymousObject = (from patient in _context.Patients
                                         join claim in _context.Claims
                                         on patient.PatientID equals claim.PatientID
                                         select new
                                         {
                                             claim.ClaimID,
                                             claim.DateOfClaim,
                                             patient.PatientID,
                                             patient.FirstName
                                         }).ToList();

            if (!listOfAnonymousObject.Any())
                throw new InvalidOperationException("dadasda");

            return listOfAnonymousObject;
        }


        //26 Flatten the data to create a list where each entry includes ClaimID, DoctorID, FirstName, and Specialty.

        [HttpGet("getdoctorclaims")]
        public List<DoctorClaim> GetDoctorClaims()
        {

            var doctorClaim = (from docclaim in _context.DoctorClaims
                               join claim in _context.Claims
                               on docclaim.ClaimID equals claim.ClaimID
                               join doc in _context.Doctors
                               on docclaim.DoctorID equals doc.DoctorID
                               select new DoctorClaim
                               {
                                   ClaimID = claim.ClaimID,
                                   DoctorID = docclaim.DoctorID,
                                   FirstName = doc.FirstName,
                                   Specialty = doc.Specialty
                               }).ToList();

            if (!doctorClaim.Any())
                throw new InvalidOperationException("dadasda");

            return doctorClaim;
        }

        //27 Create a list of unique doctor names (FirstName + LastName) and their specialties.

        [HttpGet("getdoctorsandspecialties")]
        public object GetDoctorsAndSpecialties()
        {

            var doctorNamesAnSpecialties = (from doc in _context.Doctors
                                            select new
                                            {
                                                DocFullName = $"{doc.FirstName} {doc.LastName}",
                                                doc.Specialty
                                            }).ToList();

            if (!doctorNamesAnSpecialties.Any())
                throw new InvalidOperationException("No data found");

            return doctorNamesAnSpecialties;
        }

        //28 Retrieve the top 5 claims sorted by DateOfClaim in descending order.
        [HttpGet("getfivetopclaim")]
        public List<Claim> GetFiveTopClaim()
        {
            var fiveTopClaim = _context.Claims
                                .Select(c => c)
                                .OrderByDescending(c => c.DateOfClaim)
                                .Take(5).ToList();

            if (!fiveTopClaim.Any())
                throw new InvalidOperationException("");

            return fiveTopClaim;
        }



        //29 Retrieve claims associated with policies where the EndDate has passed.

        [HttpGet("getexpiredclaimspolicies")]
        public object GetExpiredClaimsPolicies()
        {
            var expiredClaimsPolicie = from claim in _context.Claims
                                       join insp in _context.InsurancePolicies
                                       on claim.PolicyID equals insp.PolicyID
                                       where insp.EndDate.Date > DateTime.Now.Date
                                       select new { claim, insp };

            if (!expiredClaimsPolicie.Any())
                throw new InvalidOperationException("No data found");

            return expiredClaimsPolicie;
        }


        //30 Retrieve claims with patient details, policy details, and a list of doctors for each claim.
        [HttpGet("getclaimspatientsanddoctors")]
        public object GetClaimsPatientsAndDoctors()
        {

            var claimsPatientsAndDoctors = (from patient in _context.Patients
                                            join claim in _context.Claims
                                            on patient.PatientID equals claim.PatientID
                                            join dclaim in _context?.DoctorClaims
                                            on claim.ClaimID equals dclaim.ClaimID
                                            join doc in _context?.Doctors
                                            on dclaim.DoctorID equals doc.DoctorID
                                            select new
                                            {
                                                patient,
                                                claim,
                                                dclaim,
                                                doc
                                            }).ToList();

            if (!claimsPatientsAndDoctors.Any())
                throw new InvalidOperationException("No data found!");


            return claimsPatientsAndDoctors;
        }

        //Retrieve all claims where AmountClaimed exceeds the policy's PremiumAmount.
        [HttpGet("getamountclaimedexceedspremiumamount")]
        public List<Claim> GetAmountClaimedExceedsPremiumAmount()
        {

            var amountClaimedExceedsPremiumAmount = (from claim in _context.Claims
                                                     join insp in _context?.InsurancePolicies
                                                     on claim.PolicyID equals insp.PolicyID
                                                     where claim.AmountClaimed > insp.PremiumAmount
                                                     select claim).ToList();

            if (!amountClaimedExceedsPremiumAmount.Any())
                throw new InvalidOperationException("No data found");

            return amountClaimedExceedsPremiumAmount;
        }


        //Calculate the age of patients based on their DateOfBirth and include it in the result set with their claims.

        [HttpGet("getpatientsagewithclaims")]
        public object GetPatientsAgeWithClaims()
        {

            var patientsAgeWithClaims = (from patient in _context.Patients
                                         join claim in _context.Claims
                                         on patient.PatientID equals claim.PatientID
                                         select new
                                         {
                                             patient = new Patient
                                             {
                                                 PatientID = patient.PatientID,
                                                 PhoneNumber = patient.PhoneNumber,
                                                 LastName = patient.LastName,
                                                 FirstName = patient.FirstName,
                                                 Address = patient.Address,
                                                 DateOfBirth = patient.DateOfBirth.Date,
                                                 PatientAge = (DateTime.Now.Date - patient.DateOfBirth.Date).Days / 365,// convert days age to years
                                             },
                                             claim
                                         }).ToList();


            if (!patientsAgeWithClaims.Any())
                throw new InvalidOperationException("No data found.");


            return patientsAgeWithClaims;
        }














        /*  [HttpGet("duplicatenumber")]
          public IActionResult DuplicateNumber()
          {
              List<int> number = [1, 2, 2, 3, 5, 6, 6, 7, 8, 8, 9, 9];



              var result = number.Where(n => number.Count(n2 => n2 == n) > 1);


              var duplicateNumber = from n in number
                                    group n by n into numbers
                                    where numbers.Count() > 1
                                    select new
                                    {
                                        Duplicates = numbers.FirstOrDefault()
                                    };


              if (true)
              {

              }
              return Ok(result);
          }

          // teste 
          string trs = "()()()()()()(((())))";

          //see if parantheses are closed or not.

          [HttpPost("isclosedornot")]
          public bool IsClosedOrNot(string value)
          {

              var res = (from par in value.ToCharArray()
                         group par by par into parList
                         select new
                         {
                             parentheses = parList.Count(),
                         }).ToList();

              if (res[0].parentheses == res[1].parentheses)
                  return true;

              return false;
          }

          //int[] nums = [2, 7, 11, 15];
          //int target = 9;
          [HttpGet("getnums")]
          public string GetNums()
          {

              int[] nums = [2, 7, 11, 15];
              int target = 9;
              for (int i = 0; i < nums.Length; i++)
              {
                  for (int j = 0; j < nums.Length; j++)
                  {
                      if (nums[i] + nums[j] == target)
                          return $"{nums[i] + nums[j]}";
                  }
              }
              return "";
          }

          [HttpGet("getnumstwo")]
          public string GetNumsTwo() //target
          {

              int[] nums = [2, 7, 11, 15];
              int target = 9;


              var result = from n in nums
                           .Where(n => nums.Any(n2 => n2 + n == target))
                           group n by n into numsList
                           select new
                           {
                               num = numsList.Sum(),
                           };

              int targetRes = result.Select(n => n.num).Sum();

              return "";
          }
        */



    }
}
