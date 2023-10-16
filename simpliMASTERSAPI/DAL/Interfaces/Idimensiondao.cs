using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using ODLMWebAPI.Models;
using System.Data;
using simpliMASTERSAPI.Models;

namespace ODLMWebAPI.DAL.Interfaces
{ 
    public interface IDimensionDAO
    {
        DropDownTO SelectSalutationOnId(int idSalutation);
        List<DropDownTO> SelectDeliPeriodForDropDown();
        List<Dictionary<string, string>> GetColumnName(string tablename, Int32 tableValue);
        Int32 InsertdimentionalData(string tableQuery, Boolean IsInsertion, SqlConnection conn = null, SqlTransaction tran = null);
        List<DimensionTO> SelectAllMasterDimensionList();
        Int32 getidentityOfTable(string Query);
        Int32 getMaxCountOfTable(string CoulumName, string tableName);
        List<DropDownTO> SelectCDStructureForDropDown(Int32 isRsOrPerncent, int moduleId,int orgTypeId);
        List<DropDownTO> SelectCountriesForDropDown();
        List<DropDownTO> SelectOrgLicensesForDropDown();
        List<DropDownTO> SelectSalutationsForDropDown();
        List<StateMasterTO> SelectDistrictForStateMaster(int stateId);
        //Priyanka [23-04-2019]
        List<DropDownTO> GetSAPMasterDropDown(int dimensionId);
        DropDownTO GetSAPMasterByIdGenericMaster(int idGenericMaster);
        List<DropDownTO> SelectDistrictForDropDown(int stateId);
        List<DropDownTO> SelectStatesForDropDown(int countryId);
        List<DropDownTO> SelectTalukaForDropDown(int districtId);
        List<StateMasterTO> SelectTalukaForStateMaster(int districtId);
        List<DropDownTO> SelectRoleListWrtAreaAllocationForDropDown();
        List<DropDownTO> SelectAllSystemRoleListForDropDown();
        List<DropDownTO> SelectCnfDistrictForDropDown(int cnfOrgId);
        List<DropDownTO> SelectAllTransportModeForDropDown();
        List<DropDownTO> SelectInvoiceTypeForDropDown();
        List<DropDownTO> SelectInvoiceModeForDropDown();
        List<DropDownTO> SelectCurrencyForDropDown();
        List<DropDownTO> GetInvoiceStatusForDropDown();
        List<DimFinYearTO> SelectAllMstFinYearList(SqlConnection conn, SqlTransaction tran);
        List<DropDownTO> SelectReportingType();
        List<DimVisitIssueReasonsTO> SelectVisitIssueReasonsList();
        List<DropDownTO> SelectBrandList();
        List<DropDownTO> SelectLoadingLayerList();
        DropDownTO SelectStateCode(Int32 stateId);
        List<DropDownTO> GetItemProductCategoryListForDropDown();
        List<DropDownTO> GetInvoiceStatusDropDown();
        List<DropDownTO> SelectAllFirmTypesForDropDown();
        List<DropDownTO> SelectAllInfluencerTypesForDropDown();
        List<DropDownTO> SelectAllEnquiryChannels();
        List<DropDownTO> SelectAllIndustrySector();
        List<DropDownTO> GetCallBySelfForDropDown();
        List<DropDownTO> GetArrangeForDropDown();
        List<DropDownTO> GetArrangeVisitToDropDown();
        List<DropDownTO> SelectAllOrganizationType();
        List<DropDownTO> SelectAddressTypeListForDropDown();
        DropDownTO SelectCDDropDown(Int32 cdStructureId);
        List<DropDownTO> SelectAllVisitTypeListForDropDown();
        List<DropDownTO> SelectDefaultRoleListForDropDown();
        List<DropDownTO> SelectStatesForDropDownAccToBooking(int countryId, DateTime fromDate, DateTime toDate);
        List<DropDownTO> SelectDistrictForDropDownAccToBooking(int stateId, DateTime fromDate, DateTime toDate);
        List<DropDownTO> GetFixedDropDownList();
        List<DropDownTO> SelectMasterSiteTypes(int parentSiteTypeId);
        int InsertTaluka(CommonDimensionsTO commonDimensionsTO, SqlConnection conn, SqlTransaction tran);
        int InsertDistrict(CommonDimensionsTO commonDimensionsTO, SqlConnection conn, SqlTransaction tran);
        int ExecuteGivenCommand(String cmdStr, SqlConnection conn, SqlTransaction tran);
        int InsertMstFinYear(DimFinYearTO newMstFinYearTO, SqlConnection conn, SqlTransaction tran);
        List<DropDownTO> GetUserListDepartmentWise(string deptId,int roleTypeId=0);
        List<DropDownTO> SelectAllInvoiceCopyList();

        List<DropDownTO> SelectItemMakeDropdownList();
        List<DropDownTO> SelectItemBrandDropdownList(int itemMakeId = 0);
        //List<DropDownTO> getUomGropFromSAP(Int32 baseUom);
        List<DropDownTO> getUomGropConversionFromSAP(Int32 baseUom, Int32 conversionUOM, Double altQty, SqlConnection conn, SqlTransaction tran);
        Int32 getMaxCountOfSAPTable(string CoulumName, string tableName, SqlConnection conn, SqlTransaction tran);
        List<DropDownTO> getUomGropConversion(Int32 baseUom, Int32 conversionUOM, Double altQty, SqlConnection conn, SqlTransaction tran);
        Int32 GetSAPUOMGroupUGPEntry(string ugpCode, SqlConnection conn, SqlTransaction tran);
        int InsertManufacturer(DimGenericMasterTO dimGenericMasterTO, SqlConnection conn, SqlTransaction tran);
        int UpdateManufacturer(DimGenericMasterTO dimGenericMasterTO, SqlConnection conn, SqlTransaction tran);
        List<DropDownTO> CheckManuifacturerExistsOrNot(String value,Int32 dimensionId, SqlConnection conn, SqlTransaction tran);
        Int32 GetMaxCountOfSAPTable(string CoulumName, string tableName);
        //Aniket [1-7-2019]
        List<DropDownTO> SelectAllSystemRoleListForDropDownByUserId(Int32 userId);
        //Priyanka [13-09-2019]
        List<DropDownTO> GetSupplierDivisionGroupDDL();
        //Priyanka [16-09-2019]
        List<DropDownTO> SelectDistrictByName(string district, Int32 stateId, SqlConnection conn, SqlTransaction tran);
        List<DropDownTO> SelectTalukaByName(string taluka, Int32 districtId, SqlConnection conn, SqlTransaction tran);
        //Harshala
        List<DimCountryTO> SelectCountryForStateMaster();
        int UpdateDimCountry(DimCountryTO dimCountryTo);
        int InsertDimCountry(DimCountryTO countryTO);
        Int32 UpdateSAPUOMGroupUGPEntry(Int32 UgpEntry, Double altQty,Int32 mappedTxnId, SqlConnection conn, SqlTransaction tran);
        List<DropDownTO> SelectDimMasterValues(Int32 masterId);
        List<DropDownTO> SelectDimMasterValuesByParentMasterValueId(Int32 parentMasterValueId);

        DropDownTO SelectCurrencyById(Int32 currencyId);

        List<DropDownTO> SelectAllTransTypeValues();
        List<DropDownTO> GetFinVoucherTypeList(String VoucherType = "");
        List<DropDownTO> GetAllGstCodeTaxPercentageList();
        List<DropDownTO> GetVoucherNoteReasonList(String IdVoucherNoteReasonStr = "");
        List<DropDownTO> RemoveSupplierFromSAP();
        List<DimGstTaxCategoryTypeTO> SelectDimGstTaxCategoryType();
        List<DropDownTO> SelectDimOrgGroupType(Int32 orgTypeId);

        List<DropDownTO> SelectDimGstType();
        List<DropDownTO> GetBomTypeList();

        List<DropDownTO> SelectDimAssessetype();

        List<TblWithHoldingTaxTO> SelectTblWithHoldingTax(int assesseeTypeId);
        List<TblAssetClassTO> SelectTblAssetClassTOList();

        List<DropDownTO> SelectNCOrC();

        List<DropDownTO> SelectSpotEntryOrSauda();
        int UpdateDimItemProdCateg(DimItemProdCategTO dimItemProdCategTO);
        int InsertDimItemProdCateg(DimItemProdCategTO dimItemProdCategTO);
        DimItemProdCategTO SelectDimItemProdCateg(int idProdCat);

        List<DimItemProdCategTO> SelectDimItemProdCateg(DimItemProdCategTO dimItemProdCategTO);

        int SaveDimGenericMasterData(DimGenericMasterTO dropDownTO);
        int UpdateDimGenericMasterData(DimGenericMasterTO dropDownTO);
        List<DimGenericMasterTO> GetGenericMasterData(int IdDimension, Int32 SkipIsActiveFilter = 0, Int32 ParentIdGenericMaster = 0);

        List<DimGenericMasterTO> GetGenericMasterDataForDept(int IdDimension, Int32 SkipIsActiveFilter = 0, Int32 ParentIdGenericMaster = 0);
        Boolean CheckExistingData(DimGenericMasterTO DimGenericMasterTO);
        Boolean CheckAleradyAvailableData(DimGenericMasterTO DimGenericMasterTO);

    }
}