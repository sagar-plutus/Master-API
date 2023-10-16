using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace simpliMASTERSAPI.Models
{
    public class TblWithHoldingTaxTO
    {
        public int IdWithHoldTax { get; set; }
        public string TaxName { get; set; }

        public int AssesseeTypeId { get; set; }

        public int TdsTypeId { get; set; }

        public int LocationId { get; set; }

        public Double ThreasholdAmt { get; set; }

        public Double SurchargeAmt { get; set; }



    }
}
