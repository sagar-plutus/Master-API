using ODLMWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.DAL.Interfaces
{
    public interface ITblCRMLabelDAO
    {
        #region Methods
        String SqlSelectQuery();

        #endregion


        List<TblCRMLabelTO> SelectAllTblCRMLabelList(int pageId, int langId);


        List<TblCRMLabelTO> SelectAllTblCRMLabel();
        List<TblCRMLabelTO> SelectAllTblCRMLabelForPage(int pageId);
        List<TblCRMLabelTO> SelectAllTblCRMLabelForAttribute(int attrId);


        List<TblCRMLabelTO> ConvertDTToList(SqlDataReader tblCRMLabelTODT);

        int InsertTblCRMLabel(TblCRMLabelTO tblCRMLabelTO, SqlConnection conn, SqlTransaction tran);
        int UpdateTblCRMLabelValue(TblCRMLabelTO tblCRMLabelTO, SqlConnection conn, SqlTransaction tran);

    }
}