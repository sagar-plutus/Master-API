using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using ODLMWebAPI.Models;
using System.Data;
namespace ODLMWebAPI.DAL.Interfaces
{
    public interface ITblOrgBankDetailsDAO
    {
        int InsertTblOrgBankDetails(TblOrgBankDetailsTO tblOrgBankDetailsTO, SqlConnection conn, SqlTransaction tran);
        int InsertTblOrgBankDetails(TblOrgBankDetailsTO tblOrgBankDetailsTO);

        int UpdateTblOrgBankDetails(TblOrgBankDetailsTO tblOrgBankDetailsTO);
        int UpdateTblOrgBankDetails(TblOrgBankDetailsTO tblOrgBankDetailsTO, SqlConnection conn, SqlTransaction tran);
        List<TblOrgBankDetailsTO> SelectOrgBankDetailsList(Int32 orgId);

        List<DropDownTO> SelectAccountTypeList();

        /// <summary>
        /// Harshala [14/10/2019] - 
        /// this method is created to check mobile no 
        /// Duplication on organization,user,person 
        /// </summary>
        /// <param name="mobileNo"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        Boolean isDuplicateMobileNo(String mobileNo, Int32 type, int orgId = 0);
    }
}