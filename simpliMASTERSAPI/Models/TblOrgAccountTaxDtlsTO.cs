using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace simpliMASTERSAPI.Models
{
    public class TblOrgAccountTaxDtlsTO
    {
        public int IdOrgAccountTaxDtl { get; set; }

        public int OrgAccountTaxId { get; set; }

        public int WithholdTaxId { get; set; }
        public Boolean IsActive { get; set; }

    }
}
