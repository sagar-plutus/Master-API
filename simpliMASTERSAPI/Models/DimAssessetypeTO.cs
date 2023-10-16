using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace simpliMASTERSAPI.Models
{
    public class DimAssessetypeTO
    {
        public int IdAssesseeType { get; set; }

        public string AssesseeName { get; set; }

        public int SapMapAssesseeTypeId { get; set; }

        public Boolean IsForVendor { get; set; }

        public Boolean IsForTax { get; set; }

        public Boolean IsActive { get; set; }

        public int ParentAssesseTypeId { get; set; }


    }
}
