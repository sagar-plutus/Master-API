
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using ODLMWebAPI.Models;
using System.Data;
namespace ODLMWebAPI.DAL.Interfaces
{
    public interface ITblAttributeStatusDAO
    {

        List<AttributeStatusTO> SelectAllAttributeStatusList(Int32 pageId,int orgTypeId);
        List<AttributeStatusTO> AllAttributeListForUI(Int32 pageId, int orgTypeId);
        List<AttributePageTO> SelectAllAttributePages();
        List<AttributeStatusTO> CheckDataExistsOrNot(AttributeStatusTO attributeTO);
        int InsertTblAttributeStatus(AttributeStatusTO tblAttributeStatusTO, SqlConnection conn, SqlTransaction tran);
        int UpdateTblAttributeStatus(AttributeStatusTO tblAttributeStatusTO, SqlConnection conn, SqlTransaction tran);


        List<DropDownTO> GetAttributeSrcList(AttributePageTO attrSrcTO);

        int UpdateEditedAttributeName(AttributeStatusTO attributeTO, SqlConnection conn, SqlTransaction tran);
        int UpdateEditedAttributeNameForAttrStatus(AttributeStatusTO attributeTO, SqlConnection conn, SqlTransaction tran);
        List<AttributeStatusTO> SelectAllAttributeStatusLabelList(Int32 pageId, int orgTypeId); 
    }
}