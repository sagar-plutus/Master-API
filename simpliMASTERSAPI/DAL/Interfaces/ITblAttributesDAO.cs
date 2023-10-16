using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using ODLMWebAPI.Models;
using System.Data;
namespace ODLMWebAPI.DAL.Interfaces
{
    public interface ITblAttributesDAO
    {
        List<TblAttributesTO> SelectAllAttributes();
        List<TblAttributesTO> SelectAllAttributesForPage(int pageId);
        TblAttributesTO SelectAttributesById(int attrId);

        TblAttributesTO SelectAttributesByName(string attrName);
    }
}
