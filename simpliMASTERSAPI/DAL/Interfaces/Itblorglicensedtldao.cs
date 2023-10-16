using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using ODLMWebAPI.Models;
using System.Data;
namespace ODLMWebAPI.DAL.Interfaces
{
    public interface ITblOrgLicenseDtlDAO
    {
        String SqlSelectQuery();

        /// <summary>
        /// HRUSHIKESH ADDED ON [30/09/2019]
        /// this method is used to check whether license values like GST,PAN etc
        /// are already registered for other organizations or not
        /// </summary>
        /// <returns>true if duplicate is present</returns>
        Boolean isDuplicateLicenseValue(Int32 licenseType, String licValue, Int32 orgTypeId, int orgId = 0);
        Boolean isDuplicateLicenseValue(Int32 licenseType, String licValue, Int32 orgTypeId, SqlConnection conn,SqlTransaction tran, int orgId = 0);
        List<TblOrgLicenseDtlTO> SelectAllTblOrgLicenseDtl(Int32 orgId);

        List<TblOrgLicenseDtlTO> SelectAllOrgList();
        List<TblOrgLicenseDtlTO> SelectAllTblOrgLicenseDtl(Int32 orgId, SqlConnection conn, SqlTransaction tran);
        List<TblOrgLicenseDtlTO> SelectAllTblOrgLicenseDtl(Int32 orgId, Int32 licenseId, String licenseVal);
        TblOrgLicenseDtlTO SelectTblOrgLicenseDtl(Int32 idOrgLicense);
        List<TblOrgLicenseDtlTO> ConvertDTToList(SqlDataReader tblOrgLicenseDtlTODT);
        int InsertTblOrgLicenseDtl(TblOrgLicenseDtlTO tblOrgLicenseDtlTO);
        int InsertTblOrgLicenseDtl(TblOrgLicenseDtlTO tblOrgLicenseDtlTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteInsertionCommand(TblOrgLicenseDtlTO tblOrgLicenseDtlTO, SqlCommand cmdInsert);
        int UpdateTblOrgLicenseDtl(TblOrgLicenseDtlTO tblOrgLicenseDtlTO);
        int UpdateTblOrgLicenseDtl(TblOrgLicenseDtlTO tblOrgLicenseDtlTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteUpdationCommand(TblOrgLicenseDtlTO tblOrgLicenseDtlTO, SqlCommand cmdUpdate);
        int DeleteTblOrgLicenseDtl(Int32 idOrgLicense);
        int DeleteTblOrgLicenseDtl(Int32 idOrgLicense, SqlConnection conn, SqlTransaction tran);
        int ExecuteDeletionCommand(Int32 idOrgLicense, SqlCommand cmdDelete);

        int UpdateOrgLicense(TblOrgLicenseDtlTO tblOrgLicenseDtlTO);

    }
}