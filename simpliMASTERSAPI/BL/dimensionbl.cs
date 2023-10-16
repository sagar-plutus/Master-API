using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Collections;
using System.Text;
using System.Data;
using ODLMWebAPI.DAL;
using ODLMWebAPI.Models;
using ODLMWebAPI.StaticStuff;
using ODLMWebAPI.BL.Interfaces;
using ODLMWebAPI.DAL.Interfaces;
using simpliMASTERSAPI;
using simpliMASTERSAPI.Models;
using Newtonsoft.Json;

namespace ODLMWebAPI.BL
{     
    public class DimensionBL : IDimensionBL
    {
        private readonly IDimensionDAO _iDimensionDAO;
        private readonly ITblConfigParamsDAO _iTblConfigParamsDAO;
        private readonly ITblLocationDAO _iTblLocationDAO;
        private readonly ITblEntityRangeDAO _iTblEntityRangeDAO;
        private readonly ITblRoleOrgSettingDAO _iTblRoleOrgSettingDAO;
        private readonly IConnectionString _iConnectionString;
        public DimensionBL(ITblRoleOrgSettingDAO iTblRoleOrgSettingDAO, ITblEntityRangeDAO iTblEntityRangeDAO, ITblLocationDAO iTblLocationDAO, ITblConfigParamsDAO iTblConfigParamsDAO, IConnectionString iConnectionString, IDimensionDAO iDimensionDAO)
        {
            _iDimensionDAO = iDimensionDAO;
            _iTblConfigParamsDAO = iTblConfigParamsDAO;
            _iTblLocationDAO = iTblLocationDAO;
            _iTblEntityRangeDAO = iTblEntityRangeDAO;
            _iTblRoleOrgSettingDAO = iTblRoleOrgSettingDAO;
            _iConnectionString = iConnectionString;
        }
        #region Selection

        //Sudhir[24-APR-2018] Added for Get All Organization Type.
        public List<DropDownTO> GetAllOrganizationType()
        {
            return _iDimensionDAO.SelectAllOrganizationType();
        }

        public List<DropDownTO> SelectDeliPeriodForDropDown()
        {
            return _iDimensionDAO.SelectDeliPeriodForDropDown();
        }
        //Priyanka [13-09-2019]
        public List<DropDownTO> GetSupplierDivisionGroupDDL()
        {
            return _iDimensionDAO.GetSupplierDivisionGroupDDL();
        }
        
        //Priyanka [23-04-2019]
        public List<DropDownTO> GetSAPMasterDropDown(Int32 dimensionId)
        {
            return _iDimensionDAO.GetSAPMasterDropDown(dimensionId);
        }

        public DropDownTO GetSAPMasterByIdGenericMaster(Int32 idGenericMaster)
        {
            return _iDimensionDAO.GetSAPMasterByIdGenericMaster(idGenericMaster);
        }

        /// <summary>
        /// Hrishikesh[27-03-2018]Added to get district by state
        /// </summary>
        /// <param name="stateId"></param>
        /// <returns></returns>
        public List<StateMasterTO> GetDistrictsForStateMaster(int stateId)
        {
            return _iDimensionDAO.SelectDistrictForStateMaster(stateId);
        }

        public List<DropDownTO> SelectCDStructureForDropDown(int moduleId,int orgTypeId)
        {
            //Vijaymala added[22-06-2018]
            Int32 isRsOrPerncent = 0;
            Int32 isRs = 0, isPercent = 0;

            TblConfigParamsTO tblConfigParamsTORs = _iTblConfigParamsDAO.SelectTblConfigParams(Constants.CP_CD_STRUCTURE_IN_RS);
            if (tblConfigParamsTORs != null)
            {
                if (Convert.ToInt32(tblConfigParamsTORs.ConfigParamVal) == 1)
                {
                    isRs = 1;
                }
            }
            TblConfigParamsTO tblConfigParamsTOPer = _iTblConfigParamsDAO.SelectTblConfigParams(Constants.CP_CD_STRUCTURE_IN_PERCENTAGE);
            if (tblConfigParamsTOPer != null)
            {
                if (Convert.ToInt32(tblConfigParamsTOPer.ConfigParamVal) == 1)
                {
                    isPercent = 1;
                }
            }
            if ((isRs == 0 && isPercent == 0) || (isRs == 1 && isPercent == 1))
            {
                isRsOrPerncent = 0;
            }
            else if (isRs == 1 && isPercent == 0)
            {
                isRsOrPerncent = 1;
            }
            else if (isRs == 0 && isPercent == 1)
            {
                isRsOrPerncent = 2;
            }

            return _iDimensionDAO.SelectCDStructureForDropDown(isRsOrPerncent,moduleId, orgTypeId);
        }

        //Vijaymala added[22-06-2018]
        public DropDownTO SelectCDDropDown(Int32 cdStructureId)
        {
            return _iDimensionDAO.SelectCDDropDown(cdStructureId);
        }

        public List<Dictionary<string, string>> GetColumnName(string tableName,Int32 tableValue)
        {
            return _iDimensionDAO.GetColumnName(tableName, tableValue);
        }

        public List<DropDownTO> SelectCountriesForDropDown()
        {
            return _iDimensionDAO.SelectCountriesForDropDown();
        }

        public List<DropDownTO> SelectStatesForDropDown(int countryId)
        {
            return _iDimensionDAO.SelectStatesForDropDown(countryId);
        }
        public List<DropDownTO> SelectDistrictForDropDown(int stateId)
        {
            return _iDimensionDAO.SelectDistrictForDropDown(stateId);
        }

        public List<DropDownTO> SelectTalukaForDropDown(int districtId)
        {
            return _iDimensionDAO.SelectTalukaForDropDown(districtId);
        }

        /// <summary>
        ///Hrishikesh[27 - 03 - 2018] Added to get taluka by district
        /// </summary>
        /// <param name="districtId"></param>
        /// <returns></returns>
        public List<StateMasterTO> GetTalukasForStateMaster(int districtId)
        {
            return _iDimensionDAO.SelectTalukaForStateMaster(districtId);
        }

        public List<DropDownTO> SelectOrgLicensesForDropDown()
        {
            return _iDimensionDAO.SelectOrgLicensesForDropDown();
        }

        public List<DropDownTO> SelectSalutationsForDropDown()
        {
            return _iDimensionDAO.SelectSalutationsForDropDown();
        }

        public List<DropDownTO> SelectRoleListWrtAreaAllocationForDropDown()
        {
            return _iDimensionDAO.SelectRoleListWrtAreaAllocationForDropDown();

        }

        public List<DropDownTO> SelectAllSystemRoleListForDropDown()
        {
            return _iDimensionDAO.SelectAllSystemRoleListForDropDown();

        }

        public List<DropDownTO> SelectCnfDistrictForDropDown(int cnfOrgId)
        {
            return _iDimensionDAO.SelectCnfDistrictForDropDown(cnfOrgId);

        }

        public List<DropDownTO> SelectAllTransportModeForDropDown()
        {
            return _iDimensionDAO.SelectAllTransportModeForDropDown();

        }

        public List<DropDownTO> SelectInvoiceTypeForDropDown()
        {
            return _iDimensionDAO.SelectInvoiceTypeForDropDown();

        }

        public List<DropDownTO> SelectInvoiceModeForDropDown()
        {
            return _iDimensionDAO.SelectInvoiceModeForDropDown();

        }
        public List<DropDownTO> SelectCurrencyForDropDown()
        {
            return _iDimensionDAO.SelectCurrencyForDropDown();

        }
        public TenantTO SelectCurrentTenant()
        {
            try
            {
                //TenantTO tenantTO = new TenantTO();
                //tenantTO.TenantId = _iConnectionString.GetConnectionString(Constants.TENANT_ID);
                //if (String.IsNullOrEmpty(tenantTO.TenantId))
                //    return null;
                //tenantTO.AuthKey = Constants.TenantDict[tenantTO.TenantId].AuthKey;
                //tenantTO.Url = Constants.TenantDict[tenantTO.TenantId].Url;
                //return tenantTO;
                TblConfigParamsTO tblConfigParamsTO = _iTblConfigParamsDAO.SelectTblConfigParamsValByName(Constants.RABBIT_MQ_CONFIGURATION_DETAILS);
                TenantTO tenantTO = new TenantTO();
                if (tblConfigParamsTO != null)
                {
                    tenantTO = JsonConvert.DeserializeObject<TenantTO>(tblConfigParamsTO.ConfigParamVal.ToString());
                }
                return tenantTO;
            }
            catch (Exception e)
            {
                return null;
            }

        }

        public List<DropDownTO> GetInvoiceStatusForDropDown()
        {
            return _iDimensionDAO.GetInvoiceStatusForDropDown();

        }
        //Kiran[15-MAR-2018] Added for Select All Dimension Tables from tblMasterDimension
        public List<DimensionTO> SelectAllMasterDimensionList()
        {
            return _iDimensionDAO.SelectAllMasterDimensionList();

        }

        public List<DropDownTO> SelectDefaultRoleListForDropDown()
        {
            return _iDimensionDAO.SelectDefaultRoleListForDropDown();
        }

        /// <summary>
        /// Sanjay [14-June-2019] To Get the List of Active Item Make Drop Down List
        /// </summary>
        /// <returns></returns>
        public List<DropDownTO> SelectItemMakeDropdownList()
        {
            return _iDimensionDAO.SelectItemMakeDropdownList();
        }

        /// <summary>
        ///Sanjay [14-June-2019] To Get the List of Active Item Brand Drop Down List
        /// If itemMakeId=0 Then it will return all brands else specific to given make
        /// </summary>
        /// <param name="itemMakeId"></param>
        /// <returns></returns>
        public List<DropDownTO> SelectItemBrandDropdownList(int itemMakeId = 0)
        {
            return _iDimensionDAO.SelectItemBrandDropdownList(itemMakeId);
        }

        public List<DropDownTO> GetFinVoucherTypeList(String VoucherType = "")
        {
            return _iDimensionDAO.GetFinVoucherTypeList(VoucherType);
        }

        public List<DropDownTO> GetVoucherNoteReasonList(String IdVoucherNoteReasonStr = "")
        {
            return _iDimensionDAO.GetVoucherNoteReasonList(IdVoucherNoteReasonStr);
        }

        public List<DropDownTO> GetAllGstCodeTaxPercentageList()
        {
            return _iDimensionDAO.GetAllGstCodeTaxPercentageList();
        }
        public List<DropDownTO> RemoveSupplierFromSAP()
        {
            List<DropDownTO> RemoveSupplierList = new List<DropDownTO>();
            RemoveSupplierList = _iDimensionDAO.RemoveSupplierFromSAP();
            if(RemoveSupplierList != null && RemoveSupplierList.Count > 0)
            {
                for (int i = 0; i < RemoveSupplierList.Count; i++)
                {
                    SAPbobsCOM.BusinessPartners oBusinesspartner;
                    oBusinesspartner = Startup.CompanyObject.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oBusinessPartners);

                    bool getResult = oBusinesspartner.GetByKey(RemoveSupplierList[i].Text);
                    int result = oBusinesspartner.Remove();
                }
            }
            return RemoveSupplierList;
        }

        
        //Kiran[15-MAR-2018] Added for New Dimension in  Selected Dim Tables 
        public Int32 saveNewDimensional(Dictionary<string, string> tableData, string tableName)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            conn.Open();
            SqlTransaction tran = conn.BeginTransaction();
            var result = 0;
            int cnt = 0;
            string masterValue = "";
            string locationDesc = "";
            string primaryKey = "";
            Int32 dimensionId = 0;
            Int32 parentLocId = 0;
            Boolean IsSAPTransaction = false;
            Int32 result1 = 0;
            Int32 result2 = 0;
            string query = "INSERT INTO " + tableName + "( ";
            string values = "VALUES(";
            foreach (KeyValuePair<string, string> item in tableData)
            {
                if (cnt == 0)
                {
                    if (item.Key == "idGenericMaster")
                    {
                        cnt++;
                        continue;
                    }
                    if (item.Key == "idGenericMaster" || item.Key == "idLocation" || item.Key == "itemState")
                    {
                        primaryKey = item.Key;
                        IsSAPTransaction = true;
                    }
                        
                    result = getIdentityOfTable(item.Key, tableName);
                    result2 = result;
                    if (result > 0)
                    {
                        query += item.Key + ",";
                        values += "'" + result + "',";
                    }
                }
                else
                {
                    if(IsSAPTransaction == true && item.Key == "value")
                        masterValue = item.Value;
                    if (IsSAPTransaction == true && item.Key == "dimensionId")
                        dimensionId = Convert.ToInt32(item.Value);
                    if (IsSAPTransaction == true && item.Key == "locationDesc")
                        locationDesc = item.Value;
                    if (IsSAPTransaction == true && item.Key == "parentLocId")
                        parentLocId = Convert.ToInt32(item.Value);
                    query += item.Key + ",";
                    values += "'" + item.Value + "',";
                }
                cnt++;
            }
            string executeQuery = query.TrimEnd(',') + ")" + values.TrimEnd(',') + ")";
            result1 = _iDimensionDAO.InsertdimentionalData(executeQuery, true, conn, tran);
            //if (result1 > 0)
            //{
            //    result1 = result2;
            //}
                if (result1 > 0 && IsSAPTransaction == true)
            {
                result1 = result2;
                Int32 IsSAPServiceEnable = 0;
                TblConfigParamsTO tblConfigParamsTOSAPService = _iTblConfigParamsDAO.SelectTblConfigParams(Constants.SAPB1_SERVICES_ENABLE);
                if (tblConfigParamsTOSAPService != null)
                {
                    if (Convert.ToInt32(tblConfigParamsTOSAPService.ConfigParamVal) == 1)
                    {
                        IsSAPServiceEnable = 1;
                    }
                }
                if(IsSAPServiceEnable == 1)
                {
                    string mappedTxnId = "";
                    if (masterValue != "" && dimensionId != 0)
                    {
                        mappedTxnId = AddSAPMasters(masterValue, dimensionId, parentLocId, result1);//here result1 use as a last insert Id
                    }
                    else if (locationDesc != "")
                    {
                        dimensionId = (Int32)Constants.SAPMasters.Location_Compartment;
                        mappedTxnId = AddSAPMasters(locationDesc, dimensionId, parentLocId, result1);//here result1 use as a last insert Id
                    }
                    if (!String.IsNullOrEmpty(mappedTxnId))
                    {
                        string queryString = "UPDATE " + tableName + " SET mappedTxnId = '"+ mappedTxnId + "' where " + primaryKey + " = " + result1;
                        result1 = _iDimensionDAO.InsertdimentionalData(queryString, false, conn, tran);
                    }
                    else
                    {
                        result1 = 0;
                    }
                }
            }
            if (result1 > 0)
                tran.Commit();
            else
                tran.Rollback();
            return result1;
        }
        public String AddSAPMasters(string masterValue, Int32 dimensionId, Int32 parentLocId, Int32 LastInsertId)
        {
            Int32 outputResult = 0;
            int result = 0;
            switch (dimensionId)
            {
                case (Int32)Constants.SAPMasters.UOM_Group:
                    
                    break;
                case (Int32)Constants.SAPMasters.Price_List:
                    SAPbobsCOM.PriceLists oPriceLists;
                    oPriceLists = Startup.CompanyObject.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oPriceLists);
                    oPriceLists.PriceListName = masterValue;
                    //oPriceLists.PriceListNo = LastInsertId;
                    result = oPriceLists.Add();
                    break;
                case (Int32)Constants.SAPMasters.Manufacturer:
                    SAPbobsCOM.Manufacturers oManuF;
                    oManuF = Startup.CompanyObject.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oManufacturers);
                    oManuF.ManufacturerName = masterValue;
                    //oManuF.Code = LastInsertId;
                    result = oManuF.Add();
                    break;
                case (Int32)Constants.SAPMasters.Shipping_Type:
                    SAPbobsCOM.ShippingTypes oShipping;
                    oShipping = Startup.CompanyObject.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oShippingTypes);
                    oShipping.Name = masterValue;
                    //oShipping.Code = LastInsertId;
                    result = oShipping.Add();
                    break;
                case (Int32)Constants.SAPMasters.Warranty:
                    
                    break;
                case (Int32)Constants.SAPMasters.Location_Compartment:
                    if(parentLocId != null && parentLocId != 0)
                    {
                        //Add Warehouse
                        SAPbobsCOM.Warehouses owarehouse;
                        owarehouse = Startup.CompanyObject.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oWarehouses);
                        owarehouse.WarehouseName = masterValue;
                        owarehouse.WarehouseCode = LastInsertId.ToString();
                        owarehouse.Location = parentLocId;
                        result = owarehouse.Add();
                    }
                    else
                    {
                        //Add Location
                        SAPbobsCOM.WarehouseLocations oLocation;
                        oLocation = Startup.CompanyObject.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oWarehouseLocations);
                        oLocation.Name = masterValue;
                        //oLocation.Code = LastInsertId;
                        result = oLocation.Add();
                    }
                    break;
                case (Int32)Constants.SAPMasters.State:
                  
                    break;
            }
            string TxnId = "";
            if (result != 0)
            {
                string errorMsg = Startup.CompanyObject.GetLastErrorDescription();
            }
            else
            {
                TxnId = Startup.CompanyObject.GetNewObjectKey();
            }
            return TxnId;
        }
        //Kiran[15-MAR-2018] Added for Update Selected Dim Tables 
        public Int32 UpdateDimensionalData(Dictionary<string, string> tableData, string tableName)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            conn.Open();
            SqlTransaction tran = conn.BeginTransaction();
            Int32 result = 0;
            string masterValue = "";
            string locationDesc = "";
            Int32 dimensionId = 0;
            Int32 parentLocId = 0;
            Int32 idTransaction = 0;
            string query = "UPDATE " + tableName + " SET ";
            string value = "";
            Int32 cnt = 0;
            foreach (KeyValuePair<string, string> item in tableData)
            {
                if (cnt == 0)
                {
                    if (item.Key == "idGenericMaster" || item.Key == "idLocation" || item.Key == "itemState")
                        idTransaction = Convert.ToInt32(item.Value);
                    value += item.Key + " = " + item.Value;
                }
                else
                {
                    string stree = string.Empty;
                    if (item.Value != null)
                        if (item.Value.Contains("PM") || item.Value.Contains("AM"))
                        {
                            DateTime dt = Convert.ToDateTime(item.Value);
                            stree = dt.ToString("yyyy-MM-dd HH:MM");
                        }
                    if (stree == string.Empty)
                        query += item.Key + " = '" + item.Value + "',";
                    else
                    {
                        query += item.Key + " = '" + stree + "',";
                        stree = string.Empty;
                    }
                    if (item.Value != null)
                    {
                        if (idTransaction != 0 && item.Key == "value")
                            masterValue = item.Value;
                        if (idTransaction != 0 && item.Key == "dimensionId")
                            dimensionId = Convert.ToInt32(item.Value);
                        if (idTransaction != 0 == true && item.Key == "locationDesc")
                            locationDesc = item.Value;
                        if (idTransaction != 0 == true && item.Key == "parentLocId")
                            parentLocId = Convert.ToInt32(item.Value);
                    }
                }
                cnt++;
            }
            string executeQuery = query.TrimEnd(',') + " WHERE " + value;
           // return _iDimensionDAO.InsertdimentionalData(executeQuery);

            Int32 finalResult = 0;
            finalResult = _iDimensionDAO.InsertdimentionalData(executeQuery, false, conn, tran);
            if (finalResult == 1 && idTransaction != 0)
            {
                Int32 IsSAPServiceEnable = 0;
                TblConfigParamsTO tblConfigParamsTOSAPService = _iTblConfigParamsDAO.SelectTblConfigParams(Constants.SAPB1_SERVICES_ENABLE);
                if (tblConfigParamsTOSAPService != null)
                {
                    if (Convert.ToInt32(tblConfigParamsTOSAPService.ConfigParamVal) == 1)
                    {
                        IsSAPServiceEnable = 1;
                    }
                }
                if (IsSAPServiceEnable == 1)
                {
                    if (masterValue != "" && dimensionId != 0)
                    {
                        finalResult = UpdateSAPMasters(idTransaction, masterValue, dimensionId, parentLocId);
                    }
                    else if (locationDesc != "")
                    {
                        dimensionId = (Int32)Constants.SAPMasters.Location_Compartment;
                        finalResult = UpdateSAPMasters(idTransaction, locationDesc, dimensionId, parentLocId);
                    }
                }
            }
            if (finalResult == 1)
                tran.Commit();
            else
                tran.Rollback();
            return finalResult;
        }
        public Int32 UpdateSAPMasters(Int32 idTransaction, string masterValue, Int32 dimensionId, Int32 parentLocId)
        {
            Int32 outputResult = 0;
            int result = 0;
            switch (dimensionId)
            {
                case (Int32)Constants.SAPMasters.UOM_Group:

                    break;
                case (Int32)Constants.SAPMasters.Price_List:
                    SAPbobsCOM.PriceLists oPriceLists;
                    oPriceLists = Startup.CompanyObject.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oPriceLists);
                    oPriceLists.GetByKey(idTransaction.ToString());
                    oPriceLists.PriceListName = masterValue;
                    result = oPriceLists.Update();
                    break;
                case (Int32)Constants.SAPMasters.Manufacturer:
                    SAPbobsCOM.Manufacturers oManuF;
                    oManuF = Startup.CompanyObject.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oManufacturers);
                    oManuF.GetByKey(idTransaction);
                    oManuF.ManufacturerName = masterValue;
                    result = oManuF.Update();
                    break;
                case (Int32)Constants.SAPMasters.Shipping_Type:
                    SAPbobsCOM.ShippingTypes oShipping;
                    oShipping = Startup.CompanyObject.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oShippingTypes);
                    oShipping.GetByKey(idTransaction);
                    oShipping.Name = masterValue;
                    result = oShipping.Update();
                    break;
                case (Int32)Constants.SAPMasters.Warranty:

                    break;
                case (Int32)Constants.SAPMasters.Location_Compartment:
                    if (parentLocId != null && parentLocId != 0)
                    {
                        //Update Warehouse
                        SAPbobsCOM.Warehouses owarehouse;
                        owarehouse = Startup.CompanyObject.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oWarehouses);
                        owarehouse.GetByKey(idTransaction.ToString());
                        owarehouse.WarehouseName = masterValue;
                        owarehouse.Location = parentLocId;
                        result = owarehouse.Update();
                    }
                    else
                    {
                        //Update Location
                        SAPbobsCOM.WarehouseLocations oLocation;
                        oLocation = Startup.CompanyObject.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oWarehouseLocations);
                        oLocation.GetByKey(idTransaction);
                        oLocation.Name = masterValue;
                        result = oLocation.Update();
                    }
                    break;
                case (Int32)Constants.SAPMasters.State:
                    //SAPbobsCOM.State state = Startup.CompanyObject.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oServiceCalls);
                    //state.Name = "Test";
                    //state.Code = "Abc";
                    //SAPbobsCOM.StatesService oState;
                    //oState = Startup.CompanyObject.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oServiceCalls);
                    //oState.AddState(state);
                    break;
            }
            if (result != 0)
            {
                string errorMsg = Startup.CompanyObject.GetLastErrorDescription();
                outputResult = 0;
            }
            else
            {
                outputResult = 1;
            }
            return outputResult;
        }
        //Kiran[15-MAR-2018] Added for Validate Identity of Selected Tables Coloumn.
        public Int32 getIdentityOfTable(string columnName, string tableName)
        {

            var query = "SELECT name FROM sys.identity_columns WHERE OBJECT_NAME(object_id) = " + "'" + tableName + "'";
            Int32 result = _iDimensionDAO.getidentityOfTable(query);
            //if (result == 1)
            {
                return _iDimensionDAO.getMaxCountOfTable(columnName, tableName) + 1;
            }
            //return 0;
        }
       
        public DimFinYearTO GetCurrentFinancialYear(DateTime curDate, SqlConnection conn, SqlTransaction tran)
        {
            List<DimFinYearTO> mstFinYearTOList = _iDimensionDAO.SelectAllMstFinYearList(conn, tran);
            for (int i = 0; i < mstFinYearTOList.Count; i++)
            {
                DimFinYearTO mstFinYearTO = mstFinYearTOList[i];
                if (curDate >= mstFinYearTO.FinYearStartDate &&
                    curDate <= mstFinYearTO.FinYearEndDate)
                    return mstFinYearTO;
            }

            //Means Current Financial year not found so insert it
            DateTime startDate = Constants.GetStartDateTimeOfYear(curDate);
            DateTime endDate = Constants.GetEndDateTimeOfYear(curDate);
            int finYear = startDate.Year;
            DimFinYearTO newMstFinYearTO = new DimFinYearTO();
            newMstFinYearTO.FinYearDisplayName = finYear + "-" + (finYear + 1);
            newMstFinYearTO.FinYearEndDate = endDate;
            newMstFinYearTO.IdFinYear = finYear;
            newMstFinYearTO.FinYearStartDate = startDate;
            int result = _iDimensionDAO.InsertMstFinYear(newMstFinYearTO, conn, tran);
            if (result == 1)
            {
                return newMstFinYearTO;
            }

            return null;
        }



        // Vaibhav [27-Sep-2017] added to select all reporting type list
        public List<DropDownTO> GetReportingType()
        {
            List<DropDownTO> reportingTypeList = _iDimensionDAO.SelectReportingType();
            if (reportingTypeList != null)
                return reportingTypeList;
            else
                return null;
        }

        // Vaibhav [3-Oct-2017] added to select visit issue reason list
        public List<DimVisitIssueReasonsTO> GetVisitIssueReasonsList()
        {
            List<DimVisitIssueReasonsTO> visitIssueReasonList = _iDimensionDAO.SelectVisitIssueReasonsList();
            if (visitIssueReasonList != null)
                return visitIssueReasonList;
            else
                return null;
        }

        /// <summary>
        /// [2017-11-20]Vijaymala:Added to get brand list to changes in parity details 
        /// </summary>
        /// <returns></returns>
        public List<DropDownTO> SelectBrandList()
        {
            return _iDimensionDAO.SelectBrandList();
        }


        /// <summary>
        /// [2018-01-02]Vijaymala:Added to get loading layer list  
        /// </summary>
        /// <returns></returns>
        public List<DropDownTO> SelectLoadingLayerList()
        {
            return _iDimensionDAO.SelectLoadingLayerList();
        }

        // Vijaymala [09-11-2017] added to get state Code
        public DropDownTO SelectStateCode(Int32 stateId)
        {
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                DropDownTO dropDownTO = _iDimensionDAO.SelectStateCode(stateId);
                if (dropDownTO != null)
                    return dropDownTO;
                else return null;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "SelectStateCode");
                return null;
            }
        }

        public List<DropDownTO> GetItemProductCategoryListForDropDown()
        {
            return _iDimensionDAO.GetItemProductCategoryListForDropDown();
        }

        //Sudhir[22-01-2018] Added  for GetInvoiceStatusList.
        public List<DropDownTO> GetInvoiceStatusDropDown()
        {
            return _iDimensionDAO.GetInvoiceStatusDropDown();
        }

        //Sudhir[07-MAR-2018] Added for Get All Firm List.
        public List<DropDownTO> GetAllFirmTypesForDropDown()
        {
            return _iDimensionDAO.SelectAllFirmTypesForDropDown();
        }

        //Sudhir[07-MAR-2018] Added for Get All Firm List.
        public List<DropDownTO> GetAllInfluencerTypesForDropDown()
        {
            return _iDimensionDAO.SelectAllInfluencerTypesForDropDown();
        }

        //Sudhir[15-MAR-2018] Added for Select All Enquiry Channels  dimEnqChannel
        public List<DropDownTO> SelectAllEnquiryChannels()
        {
            return _iDimensionDAO.SelectAllEnquiryChannels();
        }

        //Sudhir[15-MAR-2018] Added for Select All Industry Sector.
        public List<DropDownTO> SelectAllIndustrySector()
        {
            return _iDimensionDAO.SelectAllIndustrySector();
        }

        /// <summary>
        /// Sanjay [2018-03-21] For Call By Self Drop Down in Tasktracker CRM Ext
        /// </summary>
        /// <returns></returns>
        public List<DropDownTO> GetCallBySelfForDropDown()
        {
            return _iDimensionDAO.GetCallBySelfForDropDown();
        }

        /// <summary>
        /// Sanjay [2018-03-21] For Call By Self Drop Down in Tasktracker CRM Ext
        /// </summary>
        /// <returns></returns>
        public List<DropDownTO> GetArrangeForDropDown()
        {
            return _iDimensionDAO.GetArrangeForDropDown();
        }


        /// <summary>
        /// Sanjay [2018-03-21] For Call By Self Drop Down in Tasktracker CRM Ext
        /// </summary>
        /// <returns></returns>
        public List<DropDownTO> GetArrangeVisitToDropDown()
        {
            return _iDimensionDAO.GetArrangeVisitToDropDown();
        }

        public List<DropDownTO> GetLocationWiseCompartmentList()
        {
            try
            {
                List<TblLocationTO> tblLocationTOList = _iTblLocationDAO.SelectAllTblLocation().FindAll(ele => ele.ParentLocId > 0);
                if (tblLocationTOList != null && tblLocationTOList.Count > 0)
                {
                    List<DropDownTO> compartmentList = new List<Models.DropDownTO>();
                    for (int i = 0; i < tblLocationTOList.Count; i++)
                    {
                        DropDownTO dropDownTO = new DropDownTO();
                        dropDownTO.Text = tblLocationTOList[i].LocationDesc;
                        dropDownTO.Value = tblLocationTOList[i].IdLocation;
                        dropDownTO.Tag = tblLocationTOList[i].ParentLocationDesc;
                        compartmentList.Add(dropDownTO);
                    }
                    return compartmentList;
                }
                else return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        //Vijaymala[28-03-2018] Added to get Address Type
        public List<DropDownTO> SelectAddressTypeListForDropDown()
        {
            return _iDimensionDAO.SelectAddressTypeListForDropDown();

        }

        //Dipali[26-07-2018] For RoleOrgTo List Mapping With Role
        public List<RoleOrgTO> SelectAllSystemRoleListForTbl(int visitTypeId, int personTypeId)
        {
            List<RoleOrgTO> roleOrgTOList = new List<RoleOrgTO>();

            List<DropDownTO> list = new List<DropDownTO>();
            List<DropDownTO> listSaved = new List<DropDownTO>();
            listSaved = _iTblRoleOrgSettingDAO.SelectSavedRoles(visitTypeId, personTypeId);
            list = _iDimensionDAO.SelectAllSystemRoleListForDropDown();


            for (int i = 0; i < list.Count; i++)
            {
                RoleOrgTO roleorgTO = new RoleOrgTO();
                roleorgTO.Role = list[i].Text;
                roleorgTO.RoleId = list[i].Value;
                if (listSaved != null)
                {
                    for (int j = 0; j < listSaved.Count; j++)
                    {
                        if (listSaved[j].Value == list[i].Value)
                        {
                            if (listSaved[j].Tag.ToString() == "1")
                                roleorgTO.Status = true;

                            else
                                roleorgTO.Status = false;
                        }
                    }
                }
                roleOrgTOList.Add(roleorgTO);
            }

            return roleOrgTOList;
        }
        public List<RoleOrgTO> SelectAllSystemOrgListForTbl(int visitTypeId, int personTypeId)
        {
            List<RoleOrgTO> roleOrgTOList = new List<RoleOrgTO>();

            List<DropDownTO> list = new List<DropDownTO>();
            list = _iDimensionDAO.SelectAllOrganizationType();
            List<DropDownTO> listSaved = new List<DropDownTO>();
            listSaved = _iTblRoleOrgSettingDAO.SelectSavedOrg(visitTypeId, personTypeId);



            for (int i = 0; i < list.Count; i++)
            {
                RoleOrgTO roleorgTO = new RoleOrgTO();
                roleorgTO.Org = list[i].Text;
                roleorgTO.OrgId = list[i].Value;
                if (listSaved != null)
                {
                    for (int j = 0; j < listSaved.Count; j++)
                    {
                        if (listSaved[j].Value == list[i].Value)
                        {
                            if (listSaved[j].Tag.ToString() == "1")
                                roleorgTO.Status = true;

                            else
                                roleorgTO.Status = false;
                        }
                    }
                }

                roleOrgTOList.Add(roleorgTO);
            }
            return roleOrgTOList;
        }
        public List<DropDownTO> SelectAllVisitTypeListForDropDown()
        {
            return _iDimensionDAO.SelectAllVisitTypeListForDropDown();
        }

        public List<DropDownTO> GetFixedDropDownValues()
        {
            return _iDimensionDAO.GetFixedDropDownList();
        }

        public List<DropDownTO> SelectMasterSiteTypes(int parentSiteTypeId)
        {
            return _iDimensionDAO.SelectMasterSiteTypes(parentSiteTypeId);
        }
        //Aniket
        public List<DropDownTO> SelectAllSystemRoleListForDropDownByUserId(Int32 userId)
        {
            return _iDimensionDAO.SelectAllSystemRoleListForDropDownByUserId(userId);

        }


        /// <summary>
        /// Harshala [24 - 09 - 2019] Added to get Country for State Master
        /// </summary>
        public List<DimCountryTO> GetAllCountryForStateMaster()
        {
            return _iDimensionDAO.SelectCountryForStateMaster();
        }
        #endregion

        #region Insertion

        public int InsertTaluka(CommonDimensionsTO commonDimensionsTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iDimensionDAO.InsertTaluka(commonDimensionsTO, conn, tran);
        }


        public int InsertDistrict(CommonDimensionsTO commonDimensionsTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iDimensionDAO.InsertDistrict(commonDimensionsTO, conn, tran);
        }

        //Harshala
        //Harshala[25-09-2019] Added for SaveNewState.
        public ResultMessage SaveNewCountry(DimCountryTO countryTO)
        {
            ResultMessage resultMessage = new ResultMessage();
            int result = 0;
            try
            {
                result = InsertDimCountry(countryTO);
                if (result != 1)
                {
                    resultMessage.DefaultBehaviour("SaveNewCountry");
                    return resultMessage;
                }
                resultMessage.DefaultSuccessBehaviour();
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "SaveNewCountry");
                return resultMessage;
            }
            finally
            {

            }
        }

        public int InsertDimCountry(DimCountryTO countryTO)
        {
            return _iDimensionDAO.InsertDimCountry(countryTO);
        }
        #endregion

        public ResultMessage InsertManufacturer(DimGenericMasterTO dimGenericMasterTO)
        {
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();

                dimGenericMasterTO.Value = dimGenericMasterTO.Value.Trim();

                #region Chech Manufacturer Already exist or not
                List<DropDownTO> manufacturerList = _iDimensionDAO.CheckManuifacturerExistsOrNot(dimGenericMasterTO.Value, dimGenericMasterTO.DimensionId ,conn, tran);
                if (manufacturerList != null && manufacturerList.Count > 0)
                {
                    tran.Rollback();
                    resultMessage.DefaultBehaviour("Manufacturer already added.");
                    // resultMessage.DisplayMessage = "Make Item already added.";
                    // resultMessage.Result = 2;
                    return resultMessage;
                }
                #endregion

                Int32 result = 0;
                dimGenericMasterTO.IsActive = 1;
                #region Add Manufacturer
                result = _iDimensionDAO.InsertManufacturer(dimGenericMasterTO, conn, tran);
                if (result == 0)
                {
                    tran.Rollback();
                    resultMessage.DefaultBehaviour("Add Manufacturer Failed - InsertManufacturer");
                    return resultMessage;
                }
                #endregion
                #region Add Manufacture In SAP
                TblConfigParamsTO tblConfigParamsTOSAPService = _iTblConfigParamsDAO.SelectTblConfigParams(Constants.SAPB1_SERVICES_ENABLE);
                if (tblConfigParamsTOSAPService != null)
                {
                    if (Convert.ToInt32(tblConfigParamsTOSAPService.ConfigParamVal) == 1)
                    {
                        SAPbobsCOM.Manufacturers oManuF;
                        oManuF = Startup.CompanyObject.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oManufacturers);
                        oManuF.ManufacturerName = dimGenericMasterTO.Value;
                        result = oManuF.Add();
                        if (result != 0)
                        {
                            tran.Rollback();
                            string errorMsg = Startup.CompanyObject.GetLastErrorDescription();
                            resultMessage.DefaultBehaviour(errorMsg);
                            return resultMessage;
                        }
                        dimGenericMasterTO.MappedTxnId = Startup.CompanyObject.GetNewObjectKey();
                        result = _iDimensionDAO.UpdateManufacturer(dimGenericMasterTO, conn, tran);
                        if (result == 0)
                        {
                            tran.Rollback();
                            resultMessage.DefaultBehaviour("Update Manufacturer Failed - UpdateManufacturer");
                            return resultMessage;
                        }
                    }
                }
                #endregion
                tran.Commit();
                resultMessage.DefaultSuccessBehaviour();
                resultMessage.Tag = dimGenericMasterTO.IdGenericMaster;  //Saket [2019-10-09] For auto selection of manufacture while adding
                return resultMessage;
            }
            catch (Exception ex)
            {
                tran.Rollback();
                resultMessage.DefaultExceptionBehaviour(ex, "");
                return resultMessage;
            }
            finally{
                conn.Close();
            }
        }


        #region Execute Command

        public int ExecuteGivenCommand(String cmdStr, SqlConnection conn, SqlTransaction tran)
        {
            return _iDimensionDAO.ExecuteGivenCommand(cmdStr, conn, tran);
        }


        #endregion

        public TblEntityRangeTO SelectEntityRangeTOFromVisitType(string entityName, DateTime createdOn)
        {

            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            int result = 0;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                DimFinYearTO curFinYearTO = GetCurrentFinancialYear(createdOn, conn, tran);
                if (curFinYearTO == null)
                {
                    tran.Rollback();
                    return null;
                }
                TblEntityRangeTO EntityRangeTO = _iTblEntityRangeDAO.SelectEntityRangeFromInvoiceType(entityName, curFinYearTO.IdFinYear, conn, tran);
                EntityRangeTO.EntityPrevValue = EntityRangeTO.EntityPrevValue + EntityRangeTO.IncrementBy;
                result = _iTblEntityRangeDAO.UpdateTblEntityRange(EntityRangeTO);
                if (result == 0)
                {
                    tran.Rollback();
                    return null;
                }
                tran.Commit();
                return EntityRangeTO;
            }
            catch (Exception ex)
            {
                tran.Rollback();
                return null;
            }
            finally
            {
                conn.Close();
            }

        }

        /// <summary>
        /// Deepali[19-10-2018]added :to get Department wise Users
        ///
        public List<DropDownTO> GetUserListDepartmentWise(string deptId,int roleTypeId=0)
        {
            return _iDimensionDAO.GetUserListDepartmentWise(deptId, roleTypeId);

        }

        /// <summary>
        /// Vijaymala[08-09-2018]added :to get state from booking
        /// </summary>
        /// <param name="countryId"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>

        public List<DropDownTO> SelectStatesForDropDownAccToBooking(int countryId, DateTime fromDate, DateTime toDate)
        {
            return _iDimensionDAO.SelectStatesForDropDownAccToBooking(countryId, fromDate, toDate);
        }

        /// <summary>
        /// Vijaymala[08-09-2018]added : to get district from booking
        /// </summary>
        /// <param name="countryId"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        public List<DropDownTO> SelectDistrictForDropDownAccToBooking(int countryId, DateTime fromDate, DateTime toDate)
        {
            return _iDimensionDAO.SelectDistrictForDropDownAccToBooking(countryId, fromDate, toDate);
        }

        //Aniket [01-02-2019] to fetch multiple copy details for invoice
        public  List<DropDownTO> SelectInvoiceCopyList()
        {
            return _iDimensionDAO.SelectAllInvoiceCopyList();
        }


        #region Updation
        //Harshala
        public int UpdateDimCountry(DimCountryTO dimCountryTo)
        {
            return _iDimensionDAO.UpdateDimCountry(dimCountryTo);

        }

        public int UpdateDimItemProdCateg(DimItemProdCategTO dimItemProdCategTO)
        {
            return _iDimensionDAO.UpdateDimItemProdCateg(dimItemProdCategTO);

        }

        public int InsertDimItemProdCateg(DimItemProdCategTO dimItemProdCategTO)
        {
            return _iDimensionDAO.InsertDimItemProdCateg(dimItemProdCategTO);

        }
        public DimItemProdCategTO getDimItemProdCategTO(int idProdCat)
        {
           return _iDimensionDAO.SelectDimItemProdCateg(idProdCat);
        }
        public List<DimItemProdCategTO> SelectDimItemProdCateg(DimItemProdCategTO dimItemProdCategTO)
        {
            return _iDimensionDAO.SelectDimItemProdCateg(dimItemProdCategTO);
        }
        #endregion


        public List<DropDownTO> SelectDimMasterValues(Int32 masterId)
        {
            return _iDimensionDAO.SelectDimMasterValues(masterId);
        }

        public List<DropDownTO> SelectDimMasterValuesByParentMasterValueId(Int32 parentMasterValueId)
        {
            return _iDimensionDAO.SelectDimMasterValuesByParentMasterValueId(parentMasterValueId);
        }
        public List<DropDownTO> SelectAllTransTypeValues()
        {
            return _iDimensionDAO.SelectAllTransTypeValues();
        }

        //Harshala[17/09/2020] added to get GstTaxCategoryType for Item Master
        public List<DimGstTaxCategoryTypeTO> SelectDimGstTaxCategoryType()
        {
            return _iDimensionDAO.SelectDimGstTaxCategoryType();
        }

        //Harshala[17/09/2020] added to get GstTaxCategoryType for Item Master
        public List<DropDownTO> SelectDimOrgGroupType(Int32 orgTypeId)
        {
            return _iDimensionDAO.SelectDimOrgGroupType(orgTypeId);
        }


        //Harshala[22/09/2020] added to get GstType 
        public List<DropDownTO> SelectDimGstType()
        {
            return _iDimensionDAO.SelectDimGstType();
        }
        
         public List<DropDownTO> GetBomTypeList()
        {
            return _iDimensionDAO.GetBomTypeList();
        }


        //Harshala[28/09/2020] added to get AssesseeType 
        public List<DropDownTO> SelectDimAssessetype()
        {
            return _iDimensionDAO.SelectDimAssessetype();
        }

        //Harshala[28/09/2020] added to get TblWithHoldingTax 
        public List<TblWithHoldingTaxTO> SelectTblWithHoldingTax(int assesseTypeId=0)
        {
            return _iDimensionDAO.SelectTblWithHoldingTax(assesseTypeId);
        }

        public List<TblAssetClassTO> SelectTblAssetClassTOList()
        {
            return _iDimensionDAO.SelectTblAssetClassTOList();
        }
        public List<DropDownTO> SelectNCOrC()
        {
            return _iDimensionDAO.SelectNCOrC();
        }

        public List<DropDownTO> SelectSpotEntryOrSauda()
        {
            return _iDimensionDAO.SelectSpotEntryOrSauda();
        }

        //ReshmaP[24-01-22] For Internal Transfer
        public ResultMessage UpdateGenericMasterData(DimGenericMasterTO DimGenericMasterTO)
        {
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                Boolean isAlreadyExist = false;
                int result = 0;
                isAlreadyExist = _iDimensionDAO.CheckExistingData(DimGenericMasterTO);

                if (!isAlreadyExist)
                    result = _iDimensionDAO.UpdateDimGenericMasterData(DimGenericMasterTO);
                else
                {
                    resultMessage.DefaultBehaviour("Name Already Present");
                    return resultMessage;
                }

                if (result != 1)
                {
                    resultMessage.DefaultBehaviour("Error In Method UpdateGenericMasterData");
                    return resultMessage;
                }
                resultMessage.DefaultSuccessBehaviour("Record Update Successfully");
                return resultMessage;

            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, " UpdateGenericMasterData");
                return resultMessage;
            }
        }

        public List<DimGenericMasterTO> GetGenericMasterData(int IdDimension, Int32 SkipIsActiveFilter = 0, Int32 ParentIdGenericMaster = 0)
        {
            return _iDimensionDAO.GetGenericMasterData(IdDimension, SkipIsActiveFilter, ParentIdGenericMaster);

        }
        public List<DimGenericMasterTO> GetGenericMasterDataForDept(int IdDimension, Int32 SkipIsActiveFilter = 0, Int32 ParentIdGenericMaster = 0)
        {
            return _iDimensionDAO.GetGenericMasterDataForDept(IdDimension, SkipIsActiveFilter, ParentIdGenericMaster);

        }
        public ResultMessage PostGenericMasterData(DimGenericMasterTO DimGenericMasterTO)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            try
            {
                Boolean isAlreadyExist = false;
                int result = 0;
                isAlreadyExist = _iDimensionDAO.CheckAleradyAvailableData(DimGenericMasterTO);
                if (!isAlreadyExist)
                    result = _iDimensionDAO.SaveDimGenericMasterData(DimGenericMasterTO);
                else
                {
                    resultMessage.DefaultBehaviour("Record Already Exists");
                    return resultMessage;
                }

                if (result != 1)
                {
                    resultMessage.DefaultBehaviour("Error In Method Save New PostGenericMasterData");
                    return resultMessage;
                }
                resultMessage.DefaultSuccessBehaviour("Recored Save Successfully");
                resultMessage.data = DimGenericMasterTO;
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, " PostGenericMasterData");
                return resultMessage;
            }

        }


    }
}
