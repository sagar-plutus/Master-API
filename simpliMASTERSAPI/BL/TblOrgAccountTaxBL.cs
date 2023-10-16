using ODLMWebAPI.BL.Interfaces;
using simpliMASTERSAPI.Models;
using ODLMWebAPI.StaticStuff;
using simpliMASTERSAPI.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using simpliMASTERSAPI.BL.Interfaces;

namespace simpliMASTERSAPI.BL
{
    public class TblOrgAccountTaxBL : ITblOrgAccountTaxBL
    {
        private readonly ITblOrgAccountTaxDAO _iTblOrgAccountTaxDAO;
        private readonly ITblOrgAccountTaxDtlsDAO _iTblOrgAccountTaxDtlsDAO;

        public TblOrgAccountTaxBL(ITblOrgAccountTaxDAO iTblOrgAccountTaxDAO, ITblOrgAccountTaxDtlsDAO iTblOrgAccountTaxDtlsDAO)
        {
            _iTblOrgAccountTaxDAO = iTblOrgAccountTaxDAO;
            _iTblOrgAccountTaxDtlsDAO = iTblOrgAccountTaxDtlsDAO;
        }

        public TblOrgAccountTaxTO SelectOrgAccountTaxsList(Int32 orgId)
        {
            TblOrgAccountTaxTO tblOrgAccountTaxTO = _iTblOrgAccountTaxDAO.SelectOrgAccountTaxList(orgId);
            if(tblOrgAccountTaxTO == null) { return null; }
            tblOrgAccountTaxTO.TblOrgAccountTaxDtlsList = _iTblOrgAccountTaxDtlsDAO.SelectOrgAccountTaxDtlsList(tblOrgAccountTaxTO.IdOrgAccountTax);
            return tblOrgAccountTaxTO;
        }

    }
}
