using ODLMWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.DAL.Interfaces
{
    public interface ITblCRMLanguageDAO
    {
        #region Methods

        String SqlSelectQuery();

        #endregion

        List<tblCRMLanguageTO> SelectAllTblCRMLanguage();
    }
}
