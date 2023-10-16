using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Collections;
using System.Text;
using System.Data;
using ODLMWebAPI.DAL;
using ODLMWebAPI.Models;
using ODLMWebAPI.StaticStuff;
using ODLMWebAPI.BL.Interfaces;
using ODLMWebAPI.DAL.Interfaces;

namespace ODLMWebAPI.BL
{
    public class TblOrgBankDetailsBL : ITblOrgBankDetailsBL
    {
        private readonly ITblOrgBankDetailsDAO _iTblOrgBankDetailsDAO;
        private readonly IConnectionString _iConnectionString;
        public TblOrgBankDetailsBL(ITblOrgBankDetailsDAO iTblOrgBankDetailsDAO, IConnectionString iConnectionString)
        {
            _iTblOrgBankDetailsDAO = iTblOrgBankDetailsDAO;
            _iConnectionString = iConnectionString;
        }


        #region selection

        public List<TblOrgBankDetailsTO> SelectOrgBankDetailsList(Int32 orgId)
        {
            return _iTblOrgBankDetailsDAO.SelectOrgBankDetailsList(orgId);
        }

        public Boolean isDuplicateMobileNumber(String mobileNo, Int32 type,int ordId=0)
        {
            return _iTblOrgBankDetailsDAO.isDuplicateMobileNo(mobileNo, type, ordId);
        }

        public List<DropDownTO> SelectAccountTypeListForDropDown()
        {
            return _iTblOrgBankDetailsDAO.SelectAccountTypeList();
        }

        #endregion











    }
}

