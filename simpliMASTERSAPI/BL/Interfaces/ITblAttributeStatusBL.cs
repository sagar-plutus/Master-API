using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using ODLMWebAPI.Models;
using System.Data;
using ODLMWebAPI.StaticStuff;

namespace ODLMWebAPI.DAL.Interfaces
{
    public interface ITblAttributeStatusBL
    {

        List<AttributeStatusTO> GetAllAttributeStatusList(Int32 pageId,int orgTypeId,int userId);
        List<AttributeStatusTO> AllAttributeListForUI(Int32 pageId, int orgTypeId);
        ResultMessage InsertAttributeList(AttributeStatusTO attributeTO);
        List<AttributePageTO> AllAttributePages();
        List<DropDownTO> GetAttributeSrcList(AttributePageTO attrSrcTO);

        ResultMessage InsertAttributeMultiList(List<AttributeStatusTO> attributeTO);

        int PostEditAttributeName(AttributeStatusTO attributeTO);
        List<TblCRMLabelTO> GetAttributeLanguageDetails(Int32 attrId);
        int PostEditAttributeNameByLanguage(TblCRMLabelTO tblCRMLabelTO);

        TblAttributesTO SelectAttributesByName(string attrName);
    }
}
