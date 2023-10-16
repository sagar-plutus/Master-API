using ODLMWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.BL.Interfaces
{
    public interface ITblOrgBankDetailsBL
    {
        List<TblOrgBankDetailsTO> SelectOrgBankDetailsList(Int32 orgId);
        Boolean isDuplicateMobileNumber(String mobileNo, Int32 type, int orgId = 0);

        List<DropDownTO> SelectAccountTypeListForDropDown();
    }
}
