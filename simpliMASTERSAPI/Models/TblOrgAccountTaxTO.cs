using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace simpliMASTERSAPI.Models
{
    public class TblOrgAccountTaxTO
    {
        public int IdOrgAccountTax { get; set; }

        public int OrganizationId { get; set; }

        public string MappedTxnId { get;set; }
        public int AssesseeTypeId { get; set; }

        public String AssesseeTypeName { get; set; }

        public Boolean SubjectToHoldingTax { get; set; }

        public Boolean Accrual { get; set; }

        public Boolean Cash { get; set; }

        public Boolean ThreasholdOverlook { get; set; }

        public Boolean SurchargeOverlook { get; set; }

        public String CerificateNo { get; set; }

        public DateTime CertificateExpiryDate { get; set; }

        public String NINumber { get; set; }

        public String WtTaxCategory { get; set; }

        public int AccountPayable { get; set; }
        public String AccountPayableName { get; set; }
        public string AccountPayableCode { get; set; }

        public int ClearingAccount { get; set; }
        public String ClearingAccountName { get; set; }

        public String ClearingAccountCode { get; set; }

        public int InterimAccount { get; set; }
        public String InterimAccountName { get; set; }

        public string InterimAccountCode { get; set; }
        public Int32 Remark1 { get; set; }

        public String CertificateFor26Q { get; set; }

        public List<TblOrgAccountTaxDtlsTO> TblOrgAccountTaxDtlsList { get; set; }

        public Boolean IsInsertTblOrgAccTaxDtls { get; set; }

    }
}
