using simpliMASTERSAPI.BL.Interfaces;
using simpliMASTERSAPI.Models;
using ODLMWebAPI.StaticStuff;
using simpliMASTERSAPI.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace simpliMASTERSAPI.BL
{
    public class TblOrgAccountTaxDtlsBL : ITblOrgAccountTaxDtlsBL
    {
        private readonly ITblOrgAccountTaxDtlsDAO _iTblOrgAccountTaxDtlsDAO;
        public TblOrgAccountTaxDtlsBL(ITblOrgAccountTaxDtlsDAO iTblOrgAccountTaxDtlsDAO)
        {
            _iTblOrgAccountTaxDtlsDAO = iTblOrgAccountTaxDtlsDAO;
        }
  
	}
}
