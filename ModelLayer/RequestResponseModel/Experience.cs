using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer
{
    public class ExperienceRequest
    {
        public string CompanyName { get; set; }

        public DateTime From { get; set; } 

        public DateTime To { get; set; }

        public int Salary { get; set; }

        public Guid EmpId { get; set; }

    }

    public class ExperienceResponse :ExperienceRequest
    {
        public Guid Id { get; set; }

        public string TotalExperience { get; set; }
    }


    public class ExperienceUpdateRequest
    {
        public Guid Id { get; set; }

        public string CompanyName { get; set; }

        public DateTime From { get; set; }

        public DateTime To { get; set; }

        public int Salary { get; set; }

        public Guid EmpId { get; set; }


    }

    public class ExperienceUpdateResponse :ExperienceUpdateRequest
    {

    }
}
