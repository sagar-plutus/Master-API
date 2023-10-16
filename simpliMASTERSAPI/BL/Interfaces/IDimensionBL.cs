using ODLMWebAPI.Models;
using ODLMWebAPI.StaticStuff;
using simpliMASTERSAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.BL.Interfaces
{ 
    public interface IDimensionBL
    { 
        List<DropDownTO> GetAllOrganizationType();
        List<DropDownTO> SelectDeliPeriodForDropDown();
        List<StateMasterTO> GetDistrictsForStateMaster(int stateId);
        List<DropDownTO> SelectCDStructureForDropDown(int moduleId,int orgTypeId = 0);
        DropDownTO SelectCDDropDown(Int32 cdStructureId);
        List<Dictionary<string, string>> GetColumnName(string tableName, Int32 tableValue);
        List<DropDownTO> SelectCountriesForDropDown();
        List<DropDownTO> SelectStatesForDropDown(int countryId);
        List<DropDownTO> SelectDistrictForDropDown(int stateId);
        List<DropDownTO> SelectTalukaForDropDown(int districtId);
        List<StateMasterTO> GetTalukasForStateMaster(int districtId);
        List<DropDownTO> SelectOrgLicensesForDropDown();
        List<DropDownTO> SelectSalutationsForDropDown();
        //Priyanka [23-04-2019]
        List<DropDownTO> GetSAPMasterDropDown(int dimensionId);
        DropDownTO GetSAPMasterByIdGenericMaster(int idGeneriMaster);


        List<DropDownTO> SelectRoleListWrtAreaAllocationForDropDown();
        List<DropDownTO> SelectAllSystemRoleListForDropDown();
        List<DropDownTO> SelectCnfDistrictForDropDown(int cnfOrgId);
        List<DropDownTO> SelectAllTransportModeForDropDown();
        List<DropDownTO> SelectInvoiceTypeForDropDown();
        List<DropDownTO> SelectInvoiceModeForDropDown();
        List<DropDownTO> SelectCurrencyForDropDown();
        List<DropDownTO> GetInvoiceStatusForDropDown();
        List<DimensionTO> SelectAllMasterDimensionList();
        List<DropDownTO> SelectDefaultRoleListForDropDown();
        Int32 saveNewDimensional(Dictionary<string, string> tableData, string tableName);
        Int32 UpdateDimensionalData(Dictionary<string, string> tableData, string tableName);
        Int32 getIdentityOfTable(string columnName, string tableName);
        DimFinYearTO GetCurrentFinancialYear(DateTime curDate, SqlConnection conn, SqlTransaction tran);
        List<DropDownTO> GetReportingType();
        List<DimVisitIssueReasonsTO> GetVisitIssueReasonsList();
        List<DropDownTO> SelectBrandList();
        List<DropDownTO> SelectLoadingLayerList();
        DropDownTO SelectStateCode(Int32 stateId);
        List<DropDownTO> GetItemProductCategoryListForDropDown();
        List<DropDownTO> GetInvoiceStatusDropDown();
        List<DropDownTO> GetAllFirmTypesForDropDown();
        List<DropDownTO> GetAllInfluencerTypesForDropDown();
        List<DropDownTO> SelectAllEnquiryChannels();
        List<DropDownTO> SelectAllIndustrySector();
        List<DropDownTO> GetCallBySelfForDropDown();
        List<DropDownTO> GetArrangeForDropDown();
        List<DropDownTO> GetArrangeVisitToDropDown();
        List<DropDownTO> GetLocationWiseCompartmentList();
        List<DropDownTO> SelectAddressTypeListForDropDown();
        List<RoleOrgTO> SelectAllSystemRoleListForTbl(int visitTypeId, int personTypeId);
        List<RoleOrgTO> SelectAllSystemOrgListForTbl(int visitTypeId, int personTypeId);
        List<DropDownTO> SelectAllVisitTypeListForDropDown();
        List<DropDownTO> GetFixedDropDownValues();
        List<DropDownTO> SelectMasterSiteTypes(int parentSiteTypeId);
        int InsertTaluka(CommonDimensionsTO commonDimensionsTO, SqlConnection conn, SqlTransaction tran);
        int InsertDistrict(CommonDimensionsTO commonDimensionsTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteGivenCommand(String cmdStr, SqlConnection conn, SqlTransaction tran);
        TblEntityRangeTO SelectEntityRangeTOFromVisitType(string entityName, DateTime createdOn);
        List<DropDownTO> SelectStatesForDropDownAccToBooking(int countryId, DateTime fromDate, DateTime toDate);
        List<DropDownTO> SelectDistrictForDropDownAccToBooking(int countryId, DateTime fromDate, DateTime toDate);
        List<DropDownTO> GetUserListDepartmentWise(string deptId,int roleTypeId=0);
        List<DropDownTO> SelectInvoiceCopyList();
        List<DropDownTO> SelectItemMakeDropdownList();
        List<DropDownTO> SelectItemBrandDropdownList(int itemMakeId = 0);
        ResultMessage InsertManufacturer(DimGenericMasterTO dimGenericMasterTO);
        //Aniket [1-7-2019]
        List<DropDownTO> SelectAllSystemRoleListForDropDownByUserId(Int32 userId);
        //Priyanka [13-09-2019]
        List<DropDownTO> GetSupplierDivisionGroupDDL();

        //Harshala
        List<DimCountryTO> GetAllCountryForStateMaster();
    
        int UpdateDimCountry(DimCountryTO dimCountryTo);

        ResultMessage SaveNewCountry(DimCountryTO countryTO);


        List<DropDownTO> SelectDimMasterValues(Int32 masterId);
        List<DropDownTO> SelectDimMasterValuesByParentMasterValueId(Int32 parentMasterValueId);

        List<DropDownTO> SelectAllTransTypeValues();
        TenantTO SelectCurrentTenant();
        List<DropDownTO> GetFinVoucherTypeList(String VoucherType = "");
        List<DropDownTO> GetAllGstCodeTaxPercentageList();
        List<DropDownTO> GetVoucherNoteReasonList(String IdVoucherNoteReasonStr = "");
        List<DropDownTO> RemoveSupplierFromSAP();
        List<DimGstTaxCategoryTypeTO> SelectDimGstTaxCategoryType();

        List<DropDownTO> SelectDimOrgGroupType(Int32 orgTypeId);

        List<DropDownTO> SelectDimGstType();
        List<DropDownTO> GetBomTypeList();

        List<DropDownTO> SelectDimAssessetype();

        List<TblWithHoldingTaxTO> SelectTblWithHoldingTax(int assesseTypeId=0);
        List<TblAssetClassTO> SelectTblAssetClassTOList();

        List<DropDownTO> SelectNCOrC();

        List<DropDownTO> SelectSpotEntryOrSauda();
        int InsertDimItemProdCateg(DimItemProdCategTO dimItemProdCategTO);
        int UpdateDimItemProdCateg(DimItemProdCategTO dimItemProdCategTO);
        DimItemProdCategTO getDimItemProdCategTO(int idProdCat);
        List<DimItemProdCategTO> SelectDimItemProdCateg(DimItemProdCategTO dimItemProdCategTO);
        ResultMessage UpdateGenericMasterData(DimGenericMasterTO DimGenericMasterTO);
        List<DimGenericMasterTO> GetGenericMasterData(int IdDimension, Int32 SkipIsActiveFilter = 0, Int32 ParentIdGenericMaster = 0);
        ResultMessage PostGenericMasterData(DimGenericMasterTO DimGenericMasterTO);
        List<DimGenericMasterTO> GetGenericMasterDataForDept(int IdDimension, Int32 SkipIsActiveFilter = 0, Int32 ParentIdGenericMaster = 0);

    }
}
