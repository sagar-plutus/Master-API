
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using ODLMWebAPI.Models;
using System.Data;
namespace ODLMWebAPI.DAL.Interfaces
{
    public interface IDimUserTypeDAO
    {
List<DropDownTO> SelectAllDimUserType();
DropDownTO SelectDimUserType(int idUserType, SqlConnection con, SqlTransaction tran);
List<DropDownTO> ConvertDTToList(SqlDataReader dimUserTypeTODT);
    }
    }