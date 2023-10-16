using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace simpliMASTERSAPI.Models
{
    public class DimGstTaxCategoryTypeTO
    {
        public Int32 SequenceNo { get; set; }
        public String GSTTaxCategoryTypeName { get; set; }
        public Int32 GSTTaxCategoryTypeId { get; set; }

        public Boolean IsService { get; set; }

        public Boolean IsActive { get; set; }

    }


}
