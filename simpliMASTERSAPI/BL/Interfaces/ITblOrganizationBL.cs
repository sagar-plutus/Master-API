using ODLMWebAPI.Models;
using ODLMWebAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.BL.Interfaces
{ 
    public interface ITblOrganizationBL
    {
        List<TblOrganizationTO> SelectAllTblOrganizationList();
        //List<TblOrganizationTO> SelectSalesAgentListWithBrandAndRate();
        TblOrganizationTO SelectTblOrganizationTO(Int32 idOrganization);
        List<TblOrganizationTO> SelectExistingAllTblOrganizationByRefIds(Int32 orgId, String overdueRefId, String enqRefId);
        TblOrganizationTO SelectTblOrganizationTO(Int32 idOrganization, SqlConnection conn, SqlTransaction tran);
        //dealer change by binal
        List<TblOrganizationTO> SelectAllChildOrganizationList(int orgTypeId, int parentId,int dealerlistType, int PageNumber, int RowsPerPage, string strsearchtxt, string dealerId, string villageName, string districtId);
        //List<TblOrganizationTO> SelectAllChildOrganizationList(int orgTypeId, int parentId,int dealerlistType);
        List<TblOrganizationTO> SelectAllTblOrganizationList(StaticStuff.Constants.OrgTypeE orgTypeE,int parentId);

        //distributer changed by binal 
        List<TblOrganizationTO> SelectAllTblOrganizationList(StaticStuff.Constants.OrgTypeE orgTypeE,int parentId, int PageNumber, int RowsPerPage, string strsearchtxt);
        List<TblOrganizationTO> SelectAllTblOrganizationList(StaticStuff.Constants.OrgTypeE orgTypeE, SqlConnection conn, SqlTransaction tran);
        List<DropDownTO> SelectAllOrganizationListForDropDown(StaticStuff.Constants.OrgTypeE orgTypeE, List<TblUserRoleTO> userRoleTOList, String DivisionIdsStr = "",int orgGrpType=0, Int32 IsAllowOrderBy = 0, string searchText = null, bool isFilter = false);
        List<DropDownTO> SelectAllOrganizationListForDropDownForRM(StaticStuff.Constants.OrgTypeE orgTypeE, Int32 RMId, List<TblUserRoleTO> userRoleTOList);
        List<DropDownTO> SelectAllSpecialCnfListForDropDown(List<TblUserRoleTO> tblUserRoleTOList);
        List<DropDownTO> SelectDealerListForDropDown(Int32 cnfId, List<TblUserRoleTO> tblUserRoleTOList);
        List<DropDownTO> GetDealerForLoadingDropDownList(Int32 cnfId);
        Dictionary<int, string> SelectRegisteredMobileNoDCT(String orgIds, SqlConnection conn, SqlTransaction tran);
        Dictionary<int, string> SelectRegisteredMobileNoDCTByOrgType(String orgTypeIds, SqlConnection conn, SqlTransaction tran);
        List<OrgExportRptTO> SelectAllOrgListToExport(Int32 orgTypeId, Int32 parentId); 

        ResultMessage GetAllOrgDetailsToExport(Int32 orgTypeId, Int32 parentId);
        List<TblOrganizationTO> SelectOrganizationListByRegion(Int32 orgTypeId, Int32 districtId);
        TblOrganizationTO SelectTblOrganizationTOByEnqRefId(String enq_ref_id);
        ResultMessage CheckOrgNameOrPhoneNoIsExist(String OrgName, String PhoneNo);
        List<DropDownTO> SelectDealerListForDropDownForCRM(Int32 cnfId, TblUserRoleTO tblUserRoleTO,int orgTypeId=0);
        List<DropDownTO> SelectSupplierListForDropDown(StaticStuff.Constants.OrgTypeE orgTypeE);
        int InsertTblOrganization(TblOrganizationTO tblOrganizationTO);
        int InsertTblOrganization(TblOrganizationTO tblOrganizationTO, SqlConnection conn, SqlTransaction tran);
        ResultMessage SaveNewOrganization(TblOrganizationTO tblOrganizationTO);

        //Priyanka [16-04-2019] Added
        ResultMessage SaveOrganizationRefIds(TblOrganizationTO tblOrganizationTO, String loginUserId);

        ResultMessage PostUpdateOverdueExistOrNot(TblOrganizationTO organizationTo, Int32 loginUserId);
        int UpdateTblOrganization(TblOrganizationTO tblOrganizationTO);
        int UpdateTblOrganizationRefIds(TblOrganizationTO tblOrganizationTO);
        int UpdateTblOrganization(TblOrganizationTO tblOrganizationTO, SqlConnection conn, SqlTransaction tran);
        ResultMessage UpdateOrganization(TblOrganizationTO tblOrganizationTO);
        ResultMessage DeactivateOrganization(TblOrganizationTO tblOrganizationTO);
        int DeleteTblOrganization(Int32 idOrganization);
        int DeleteTblOrganization(Int32 idOrganization, SqlConnection conn, SqlTransaction tran);
        List<DropDownTO> SelectSalesEngineerListForDropDown(Int32 orgId);
        //Priyanka [19-08-2019]
        ResultMessage UpdateAndCreateLoginToSupplliers(ref string invalidIdStr);
        //Priyanka [22-08-2019]
        ResultMessage SaveNewOrganizationList(List<TblOrganizationTO> tblOrganizationTOList);
        ResultMessage SaveOrganization(TblOrganizationTO tblOrganizationTO, SqlConnection conn, SqlTransaction tran);
        ResultMessage PrintOrganisationReport(TblOrganizationTO organisationObj); 
        ResultMessage PrintSuperWisorReport(TblSupervisorTO supervisorTOObj);
        ResultMessage DeactiveDuplicationSupplier(bool isForSupplier);
        ResultMessage MigrateOrganizationData();
        ResultMessage MigrateUserDetails();
        ResultMessage DeactivateInvalidAddressesAttachedToOrg(int orgTypeId, Int32 index = 0);
        ResultMessage SetAddressTypeAttachedToOrg(int orgTypeId, int index);
        ResultMessage PostSendAPKLink(List<DropDownTO> aPKLinkToOrgList,int loginUserId);
        ResultMessage CheckIfOrgIsAvailableInSAP(int orgId);

        ResultMessage UpdateOrgLicenseDtls();
    }
}