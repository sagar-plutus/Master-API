using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Collections;
using System.Text;
using System.Data;
using ODLMWebAPI.DAL;
using ODLMWebAPI.Models;
using ODLMWebAPI.StaticStuff;
using System.Linq;
using ODLMWebAPI.BL.Interfaces;
using ODLMWebAPI.DAL.Interfaces;
using simpliMASTERSAPI.DAL.Interfaces;
using simpliMASTERSAPI;
using System.IO;
using System.Drawing;
using System.Net;
using SAPbobsCOM;
using ODLMWebAPI.DAL;
using ODLMWebAPI.StaticStuff;
using MimeKit;


namespace ODLMWebAPI.BL
{
    public class TblOrganizationBL : ITblOrganizationBL
    {
        private readonly ITblOrgPersonDtlsBL _iTblOrgPersonDtlsBL;
        private readonly ITblOrganizationDAO _iTblOrganizationDAO;
        private readonly IDimensionDAO _iDimensionDAO;
        private readonly ITblGlobalRateDAO _iTblGlobalRateDAO;
        private readonly ITblQuotaDeclarationDAO _iTblQuotaDeclarationDAO;
        private readonly ITblUserRoleBL _iTblUserRoleBL;
        private readonly ITblConfigParamsBL _iTblConfigParamsBL;
        private readonly ITblCompetitorExtDAO _iTblCompetitorExtDAO;
        private readonly ITblPurchaseCompetitorExtDAO _iTblPurchaseCompetitorExtDAO;
        private readonly ITblPersonDAO _iTblPersonDAO;
        private readonly ITblAddressDAO _iTblAddressDAO;
        private readonly ITblOrgAddressDAO _iTblOrgAddressDAO;
        private readonly ITblOrgBankDetailsDAO _iTblOrgBankDetailsDAO;
        private readonly ITblOrgLicenseDtlDAO _iTblOrgLicenseDtlDAO;
        private readonly ITblKYCDetailsBL _iTblKYCDetailsBL;
        //  private readonly ITblLoadingQuotaConfigDAO _iTblLoadingQuotaConfigDAO;
        //  private readonly ITblLoadingQuotaDeclarationDAO _iTblLoadingQuotaDeclarationDAO; 
        private readonly ITblCnfDealersBL _iTblCnfDealersBL;
        private readonly IDimOrgTypeDAO _iDimOrgTypeDAO;
        private readonly ITblUserBL _iTblUserBL;
        private readonly ITblUserExtDAO _iTblUserExtDAO;
        private readonly ITblOrgOverdueHistoryDAO _iTblOrgOverdueHistoryDAO;
        private readonly IConnectionString _iConnectionString;
        private readonly ICommon _iCommon;
        private readonly ITblConfigParamsDAO _iTblConfigParamsDAO;
        private readonly IDimReportTemplateBL _iDimReportTemplateBL;
        private readonly IRunReport _iRunReport;
        private readonly IDimStatusDAO _iDimStatusDAO;
        private readonly IDimStatusBL _iDimStatusBL;
        private readonly ITblAddonsFunDtlsDAO _iTblAddonsFunDtlsDAO;
        private readonly ITblOrgAccountTaxDAO _itblOrgAccountTaxDAO;
        private readonly ITblOrgAccountTaxDtlsDAO _iTblOrgAccountTaxDtlsDAO;
        private readonly ITblSysElementsBL _iTblSysElementsBL;
        private readonly IDimOrgTypeBL _iDimOrgTypeBL;
        private readonly ITblPurchaseManagerSupplierBL _iTblPurchaseManagerSupplierBL;
        private readonly ITblOrgPersonDtlsDAO _iTblOrgPersonDtlsDAO;
        private readonly ITblAlertDefinitionBL _iTblAlertDefinitionBL;
        private readonly IVitplSMS _iVitplSMS;
        private readonly ITblSmsBL _iTblSmsBL;
        private readonly ITblVersionBL _iTblVersionBL;
        private readonly ITblPersonBL _iTblPersonBL;

        public TblOrganizationBL(ITblAddonsFunDtlsDAO iTblAddonsFunDtlsDAO, ITblOrgPersonDtlsDAO iTblOrgPersonDtlsDAO, IDimStatusBL iDimStatusBL, IDimOrgTypeBL iDimOrgTypeBL,  IDimStatusDAO iDimStatusDAO, IRunReport iRunReport, IDimReportTemplateBL iDimReportTemplateBL, ITblConfigParamsDAO iTblConfigParamsDAO
            , ITblOrgPersonDtlsBL iTblOrgPersonDtlsBL, ITblAlertDefinitionBL iTblAlertDefinitionBL
            , ITblUserBL iTblUserBL, ITblCnfDealersBL iTblCnfDealersBL, ITblKYCDetailsBL iTblKYCDetailsBL, ITblConfigParamsBL iTblConfigParamsBL, ITblUserRoleBL iTblUserRoleBL, ITblOrgOverdueHistoryDAO iTblOrgOverdueHistoryDAO, ITblUserExtDAO iTblUserExtDAO, IDimOrgTypeDAO iDimOrgTypeDAO
            //,ITblLoadingQuotaDeclarationDAO iTblLoadingQuotaDeclarationDAO, ITblLoadingQuotaConfigDAO iTblLoadingQuotaConfigDAO
            , ITblOrgLicenseDtlDAO iTblOrgLicenseDtlDAO, ITblOrgAddressDAO iTblOrgAddressDAO, ITblAddressDAO iTblAddressDAO, ITblPersonDAO iTblPersonDAO, ITblPurchaseCompetitorExtDAO iTblPurchaseCompetitorExtDAO, ITblCompetitorExtDAO iTblCompetitorExtDAO,
            ITblQuotaDeclarationDAO iTblQuotaDeclarationDAO, ICommon iCommon, IConnectionString iConnectionString, ITblOrganizationDAO iTblOrganizationDAO,
            IDimensionDAO iDimensionDAO, ITblGlobalRateDAO iTblGlobalRateDAO, ITblOrgBankDetailsDAO iTblOrgBankDetailsDAO, ITblOrgAccountTaxDAO itblOrgAccountTaxDAO, ITblOrgAccountTaxDtlsDAO iTblOrgAccountTaxDtlsDAO, ITblSysElementsBL iTblSysElementsBL, ITblPurchaseManagerSupplierBL iTblPurchaseManagerSupplierBL
            ,IVitplSMS iVitplSMS, ITblSmsBL iTblSmsBL, ITblVersionBL iTblVersionBL, ITblPersonBL iTblPersonBL)
        {  
            _iTblAlertDefinitionBL = iTblAlertDefinitionBL;
            _iTblOrgPersonDtlsBL = iTblOrgPersonDtlsBL;
            _iTblOrganizationDAO = iTblOrganizationDAO;
            _iDimensionDAO = iDimensionDAO;
            _iDimOrgTypeBL = iDimOrgTypeBL;
            _iTblGlobalRateDAO = iTblGlobalRateDAO;
            _iTblQuotaDeclarationDAO = iTblQuotaDeclarationDAO;
            _iTblUserRoleBL = iTblUserRoleBL;
            _iTblConfigParamsBL = iTblConfigParamsBL;
            _iTblCompetitorExtDAO = iTblCompetitorExtDAO;
            _iTblPurchaseCompetitorExtDAO = iTblPurchaseCompetitorExtDAO;
            _iTblPersonDAO = iTblPersonDAO;
            _iTblOrgPersonDtlsDAO = iTblOrgPersonDtlsDAO;


            _iTblAddressDAO = iTblAddressDAO;
            _iTblOrgBankDetailsDAO = iTblOrgBankDetailsDAO;
            _iTblOrgAddressDAO = iTblOrgAddressDAO;
            _iTblOrgLicenseDtlDAO = iTblOrgLicenseDtlDAO;
            _iTblKYCDetailsBL = iTblKYCDetailsBL;
            //     _iTblLoadingQuotaConfigDAO = iTblLoadingQuotaConfigDAO;
            //    _iTblLoadingQuotaDeclarationDAO = iTblLoadingQuotaDeclarationDAO;
            _iTblCnfDealersBL = iTblCnfDealersBL;
            _iDimOrgTypeDAO = iDimOrgTypeDAO;
            _iTblUserBL = iTblUserBL;
            _iTblUserExtDAO = iTblUserExtDAO;
            _iTblOrgOverdueHistoryDAO = iTblOrgOverdueHistoryDAO;
            _iConnectionString = iConnectionString;
            _iCommon = iCommon;
            _iTblConfigParamsDAO = iTblConfigParamsDAO;
            _iDimReportTemplateBL = iDimReportTemplateBL;
            _iRunReport = iRunReport;
            _iDimStatusBL = iDimStatusBL;
            _iTblAddonsFunDtlsDAO = iTblAddonsFunDtlsDAO;
            _itblOrgAccountTaxDAO = itblOrgAccountTaxDAO;
            _iTblOrgAccountTaxDtlsDAO = iTblOrgAccountTaxDtlsDAO;
            _iTblSysElementsBL = iTblSysElementsBL;
            _iTblPurchaseManagerSupplierBL = iTblPurchaseManagerSupplierBL;
            _iTblVersionBL = iTblVersionBL;
            _iTblPersonBL = iTblPersonBL;
            _iVitplSMS = iVitplSMS;
            _iTblSmsBL = iTblSmsBL;
        }
        #region Selection

        public List<TblOrganizationTO> SelectAllTblOrganizationList()
        {
            return _iTblOrganizationDAO.SelectAllTblOrganization();
        }

        // Shifted in tblbookingbl.cs : YA - 14/03/2019
        //public List<TblOrganizationTO> SelectSalesAgentListWithBrandAndRate()
        //{
        //    try
        //    {
        //        List<TblOrganizationTO> orgList = _iTblOrganizationDAO.SelectSaleAgentOrganizationList();
        //        if (orgList != null)
        //        {
        //            List<DropDownTO> brandList = _iDimensionDAO.SelectBrandList();
        //            Dictionary<Int32, Int32> brandRateDCT = _iTblGlobalRateDAO.SelectLatestBrandAndRateDCT();
        //            Dictionary<Int32, List<TblQuotaDeclarationTO>> rateAndBandDCT = new Dictionary<int, List<TblQuotaDeclarationTO>>();
        //            List<TblGlobalRateTO> tblGlobalRateTOList = new List<TblGlobalRateTO>();
        //            if (brandList == null || brandList.Count == 0)
        //                return null;

        //            foreach (var item in brandRateDCT.Keys)
        //            {
        //                Int32 rateID = brandRateDCT[item];
        //                TblGlobalRateTO rateTO = _iTblGlobalRateDAO.SelectTblGlobalRate(rateID);
        //                if (rateTO != null)
        //                    tblGlobalRateTOList.Add(rateTO);
        //                List<TblQuotaDeclarationTO> rateBandList = _iTblQuotaDeclarationDAO.SelectAllTblQuotaDeclaration(rateID);

        //                rateAndBandDCT.Add(rateID, rateBandList);
        //            }

        //            for (int i = 0; i < orgList.Count; i++)
        //            {
        //                TblOrganizationTO tblOrganizationTO = orgList[i];
        //                tblOrganizationTO.BrandRateDtlTOList = new List<Models.BrandRateDtlTO>();
        //                for (int b = 0; b < brandList.Count; b++)
        //                {
        //                    Models.BrandRateDtlTO brandRateDtlTO = new Models.BrandRateDtlTO();
        //                    brandRateDtlTO.BrandId = brandList[b].Value;
        //                    brandRateDtlTO.BrandName = brandList[b].Text;

        //                    if (brandRateDCT != null && brandRateDCT.ContainsKey(brandRateDtlTO.BrandId))
        //                    {
        //                        int rateId = brandRateDCT[brandRateDtlTO.BrandId];

        //                        if (tblGlobalRateTOList != null)
        //                        {
        //                            TblGlobalRateTO rateTO = tblGlobalRateTOList.Where(ri => ri.IdGlobalRate == rateId).FirstOrDefault();
        //                            if (rateTO != null)
        //                                brandRateDtlTO.Rate = rateTO.Rate;
        //                        }

        //                        if (rateAndBandDCT != null && rateAndBandDCT.ContainsKey(rateId))
        //                        {
        //                            List<TblQuotaDeclarationTO> rateBandList = rateAndBandDCT[rateId];
        //                            if (rateBandList != null)
        //                            {
        //                                var rateBandObj = rateBandList.Where(o => o.OrgId == tblOrganizationTO.IdOrganization).FirstOrDefault();
        //                                if (rateBandObj != null)
        //                                {
        //                                    brandRateDtlTO.RateBand = rateBandObj.RateBand;
        //                                    brandRateDtlTO.LastAllocQty = rateBandObj.AllocQty; //Sudhir[25-6-2018] Added For Madhav
        //                                    brandRateDtlTO.ValidUpto = rateBandObj.ValidUpto; //Sudhir[25-6-2018] Added For Madhav
        //                                    brandRateDtlTO.BalanceQty = rateBandObj.BalanceQty; //Sudhir[25-6-2018] Added For Madhav


        //                                }
        //                            }
        //                        }
        //                    }

        //                    tblOrganizationTO.BrandRateDtlTOList.Add(brandRateDtlTO);
        //                }

        //            }
        //        }

        //        return orgList;
        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }
        //    finally
        //    {

        //    }
        //}

        public TblOrganizationTO SelectTblOrganizationTO(Int32 idOrganization)
        {
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                conn.Open();
                TblOrganizationTO tblOrganizationTO = _iTblOrganizationDAO.SelectTblOrganization(idOrganization, conn, tran);
                if (tblOrganizationTO != null)
                {
                    List<TblOrgLicenseDtlTO> orgLicenseDtlTOList = _iTblOrgLicenseDtlDAO.SelectAllTblOrgLicenseDtl(tblOrganizationTO.IdOrganization, conn, tran);
                    tblOrganizationTO.OrgLicenseDtlTOList = orgLicenseDtlTOList;
                    List<TblPersonTO> tblPersonTOlist = null;
                    tblPersonTOlist = _iTblPersonDAO.SelectAllTblPersonByOrganization(idOrganization);
                    tblOrganizationTO.PersonList = tblPersonTOlist;
                }
                return tblOrganizationTO;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "SelectTblOrganizationTO");
                return null;
            }
            finally
            {
                conn.Close();
            }
        }

        public List<TblOrganizationTO> SelectExistingAllTblOrganizationByRefIds(Int32 orgId, String overdueRefId, String enqRefId)
        {
            return _iTblOrganizationDAO.SelectExistingAllTblOrganizationByRefIds(orgId, overdueRefId, enqRefId);
        }

        public TblOrganizationTO SelectTblOrganizationTO(Int32 idOrganization, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblOrganizationDAO.SelectTblOrganization(idOrganization, conn, tran);

        }

       #region Add pagination for dealr display list changed by binal
        public List<TblOrganizationTO> SelectAllChildOrganizationList(int orgTypeId, int parentId, int dealerlistType, int PageNumber, int RowsPerPage, string strsearchtxt, string dealerId, string villageName, string districtId)
        {
            return _iTblOrganizationDAO.SelectAllTblOrganization(orgTypeId, parentId, dealerlistType,  PageNumber,  RowsPerPage, strsearchtxt, dealerId, villageName, districtId);
        }
        #endregion

        public List<TblOrganizationTO> SelectAllTblOrganizationList(Constants.OrgTypeE orgTypeE,int parentId)
        {
            return _iTblOrganizationDAO.SelectAllTblOrganization(orgTypeE,parentId );
        }

        #region Add pagination for distributer display list changed by binal
        public List<TblOrganizationTO> SelectAllTblOrganizationList(Constants.OrgTypeE orgTypeE, int parentId, int PageNumber, int RowsPerPage, string strsearchtxt)
        {
            return _iTblOrganizationDAO.SelectAllTblOrganization(orgTypeE, parentId, PageNumber, RowsPerPage, strsearchtxt);
        }
        #endregion
        public List<TblOrganizationTO> SelectAllTblOrganizationList(Constants.OrgTypeE orgTypeE, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblOrganizationDAO.SelectAllTblOrganization(orgTypeE, conn, tran);
        }

        public List<DropDownTO> SelectAllOrganizationListForDropDown(Constants.OrgTypeE orgTypeE, List<TblUserRoleTO> userRoleTOList, String DivisionIdsStr = "", Int32 orgGrpType = 0, Int32 IsAllowOrderBy = 0, string searchText = null, bool isFilter = false)
        {
            TblUserRoleTO tblUserRoleTO = new TblUserRoleTO();
            if (userRoleTOList != null && userRoleTOList.Count > 0)
            {
                tblUserRoleTO = _iTblUserRoleBL.SelectUserRoleTOAccToPriority(userRoleTOList);
            }
            return _iTblOrganizationDAO.SelectAllOrganizationListForDropDown(orgTypeE, tblUserRoleTO, DivisionIdsStr, orgGrpType, IsAllowOrderBy, searchText, isFilter);
        }
        public List<DropDownTO> SelectSupplierListForDropDown(Constants.OrgTypeE orgTypeE)
        {
            return _iTblOrganizationDAO.SelectSupplierListForDropDown(orgTypeE);
        }
        //Priyanka [10-09-2018] : Added to get the Organization list against RM.
        public List<DropDownTO> SelectAllOrganizationListForDropDownForRM(Constants.OrgTypeE orgTypeE, Int32 RMId, List<TblUserRoleTO> userRoleTOList)
        {
            TblUserRoleTO userRoleTO = new TblUserRoleTO();
            if (userRoleTOList != null && userRoleTOList.Count > 0)
            {
                userRoleTO = _iTblUserRoleBL.SelectUserRoleTOAccToPriority(userRoleTOList);
            }

            return _iTblOrganizationDAO.SelectAllOrganizationListForDropDownForRM(orgTypeE, RMId, userRoleTO);
        }

        public List<DropDownTO> SelectAllSpecialCnfListForDropDown(List<TblUserRoleTO> tblUserRoleTOList)
        {
            TblUserRoleTO userRoleTO = new TblUserRoleTO();
            if (tblUserRoleTOList != null && tblUserRoleTOList.Count > 0)
            {
                userRoleTO = _iTblUserRoleBL.SelectUserRoleTOAccToPriority(tblUserRoleTOList);
            }
            return _iTblOrganizationDAO.SelectAllSpecialCnfListForDropDown(userRoleTO);
        }

        public List<DropDownTO> SelectDealerListForDropDown(Int32 cnfId, List<TblUserRoleTO> tblUserRoleTOList)
        {
            TblConfigParamsTO tblConfigParamsTO = _iTblConfigParamsBL.SelectTblConfigParamsTO(Constants.CP_DEFAULT_MATE_COMP_ORGID);
            if (tblConfigParamsTO != null)
            {
                if (cnfId.ToString() == tblConfigParamsTO.ConfigParamVal)
                {
                    cnfId = 0;
                }
            }
            TblUserRoleTO tblUserRoleTO = new TblUserRoleTO();
            if (tblUserRoleTOList != null && tblUserRoleTOList.Count > 0)
            {
                tblUserRoleTO = _iTblUserRoleBL.SelectUserRoleTOAccToPriority(tblUserRoleTOList);
            }
            return _iTblOrganizationDAO.SelectDealerListForDropDown(cnfId, tblUserRoleTO);
        }

        public List<DropDownTO> GetDealerForLoadingDropDownList(Int32 cnfId)
        {
            return _iTblOrganizationDAO.GetDealerForLoadingDropDownList(cnfId);
        }

        public Dictionary<int, string> SelectRegisteredMobileNoDCT(String orgIds, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblOrganizationDAO.SelectRegisteredMobileNoDCT(orgIds, conn, tran);
        }

        public Dictionary<int, string> SelectRegisteredMobileNoDCTByOrgType(String orgTypeIds, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblOrganizationDAO.SelectRegisteredMobileNoDCTByOrgType(orgTypeIds, conn, tran);
        }
        //Aditee
        public ResultMessage PrintSuperWisorReport(TblSupervisorTO supervisorInfoObj)
        {
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                DataTable headerDT = new DataTable();
                DataTable ownerDetailsDT = new DataTable();
                DataTable KYCDetailsDT = new DataTable();
                DataTable organisationDT = new DataTable();
                DataTable addressDT = new DataTable();

                DataTable contactDetailsDT = new DataTable();
                DataTable orgLicenseDtlDT = new DataTable();

                DataSet printDataSet = new DataSet();
                headerDT.TableName = "headerDT";
                ownerDetailsDT.TableName = "ownerDetailsDT";
                KYCDetailsDT.TableName = "KYCDetailsDT";
                organisationDT.TableName = "organisationDT";
                addressDT.TableName = "addressDT";

                contactDetailsDT.TableName = "contactDetailsDT";
                orgLicenseDtlDT.TableName = "orgLicenseDtlDT";

                headerDT.Columns.Add("FirstName");
                headerDT.Columns.Add("MiddleName");
                headerDT.Columns.Add("LastName");

                headerDT.Columns.Add("PersonType");
                headerDT.Columns.Add("MobileNo");
                headerDT.Columns.Add("personPhoneNo");
                headerDT.Columns.Add("AlternateMobNo");
                headerDT.Columns.Add("DateOfBirth");
                headerDT.Columns.Add("PrimaryEmail");
                headerDT.Columns.Add("AlternateEmail");
                Int32 RowCount = 0;
                headerDT.Rows.Add();
                RowCount = headerDT.Rows.Count - 1;

                headerDT.Rows[RowCount]["FirstName"] = "First Name";
                headerDT.Rows[RowCount]["MiddleName"] = "Middle Name";
                headerDT.Rows[RowCount]["LastName"] = "Last Name";
                headerDT.Rows[RowCount]["PersonType"] = "Person Type";
                headerDT.Rows[RowCount]["MobileNo"] = "Mobile No";
                headerDT.Rows[RowCount]["AlternateMobNo"] = "Alternate Mob No";

                headerDT.Rows[RowCount]["personPhoneNo"] = "Phone No";
                headerDT.Rows[RowCount]["PrimaryEmail"] = "Primary Email";
                headerDT.Rows[RowCount]["AlternateEmail"] = "Alternate Email";

                headerDT.Columns.Add("FirmName");
                headerDT.Columns.Add("FirmType");

                headerDT.Columns.Add("RegisteredMobileNos");
                headerDT.Columns.Add("PhoneNo");
                headerDT.Columns.Add("FaxNo");
                headerDT.Columns.Add("EmailID");
                headerDT.Columns.Add("Website");

                headerDT.Rows[0]["FirmName"] = "Firm Name";
                headerDT.Rows[0]["FirmType"] = "Firm Type";

                headerDT.Rows[0]["RegisteredMobileNos"] = "Registered Mobile No";
                headerDT.Rows[0]["PhoneNo"] = "Phone No";
                headerDT.Rows[0]["FaxNo"] = "Fax No";
                headerDT.Rows[0]["EmailID"] = "EmailID";
                headerDT.Rows[0]["Website"] = "Website";

                headerDT.Columns.Add("StreetName");
                headerDT.Columns.Add("AreaName");
                headerDT.Columns.Add("PlotNo");
                headerDT.Columns.Add("PinCode");
                headerDT.Columns.Add("CountryName");
                headerDT.Columns.Add("StateName");
                headerDT.Columns.Add("DistrictName");
                headerDT.Columns.Add("TalukaName");
                headerDT.Columns.Add("VillageName");
                headerDT.Columns.Add("AddressType");

                headerDT.Rows[RowCount]["AreaName"] = "Area Name";
                headerDT.Rows[RowCount]["PlotNo"] = "Plot No";
                headerDT.Rows[RowCount]["StreetName"] = "Street Name";
                headerDT.Rows[RowCount]["PinCode"] = "PinCode";
                headerDT.Rows[RowCount]["CountryName"] = "Country Name";
                headerDT.Rows[RowCount]["StateName"] = "State Name";
                headerDT.Rows[RowCount]["DistrictName"] = "District Name";
                headerDT.Rows[RowCount]["TalukaName"] = "Taluka Name";
                headerDT.Rows[RowCount]["VillageName"] = "Village Name";
                headerDT.Rows[RowCount]["AddressType"] = "Address Type";

                headerDT.Columns.Add("AgreementSign");
                headerDT.Columns.Add("ChequeReceived");
                headerDT.Columns.Add("KYCCompleted");

                headerDT.Rows[RowCount]["AgreementSign"] = "Agreement Sign";
                headerDT.Rows[RowCount]["ChequeReceived"] = "Cheque Received";
                headerDT.Rows[RowCount]["KYCCompleted"] = "KYC Completed";

                headerDT.Columns.Add("LicenseName");
                headerDT.Columns.Add("LicenceNo");
                headerDT.Columns.Add("SalutationName");
                headerDT.Rows[RowCount]["LicenseName"] = "License Name";
                headerDT.Rows[RowCount]["LicenceNo"] = "Licence No";
                headerDT.Rows[RowCount]["SalutationName"] = "Salutation Name";

                addressDT.Columns.Add("PlotNo");
                addressDT.Columns.Add("StreetName");
                addressDT.Columns.Add("AreaName");
                addressDT.Columns.Add("PinCode");
                addressDT.Columns.Add("CountryName");
                addressDT.Columns.Add("StateName");
                addressDT.Columns.Add("DistrictName");
                addressDT.Columns.Add("TalukaName");
                addressDT.Columns.Add("VillageName");
                addressDT.Columns.Add("AddressType");
                KYCDetailsDT.Columns.Add("AgreementSign");
                KYCDetailsDT.Columns.Add("ChequeReceived");
                KYCDetailsDT.Columns.Add("KYCCompleted");
                organisationDT.Columns.Add("RegisteredMobileNos");

                organisationDT.Columns.Add("PhoneNo");
                organisationDT.Columns.Add("FaxNo");
                organisationDT.Columns.Add("EmailID");
                organisationDT.Columns.Add("Website");
                organisationDT.Columns.Add("FirmName");
                organisationDT.Columns.Add("FirmType");

                orgLicenseDtlDT.Columns.Add("LicenseName");
                orgLicenseDtlDT.Columns.Add("LicenseNo");
                ownerDetailsDT.Columns.Add("SalutationName");
                ownerDetailsDT.Columns.Add("FirstName");
                ownerDetailsDT.Columns.Add("MiddleName");
                ownerDetailsDT.Columns.Add("LastName");
                ownerDetailsDT.Columns.Add("PersonType");
                ownerDetailsDT.Columns.Add("MobileNo");
                ownerDetailsDT.Columns.Add("personPhoneNo");
                ownerDetailsDT.Columns.Add("AlternateMobNo");
                ownerDetailsDT.Columns.Add("DateOfBirth");
                ownerDetailsDT.Columns.Add("PrimaryEmail");
                ownerDetailsDT.Columns.Add("AlternateEmail");
                ownerDetailsDT.Columns.Add("Photo", typeof(byte[]));

                List<DropDownTO> PersonTypelist = _iTblPersonDAO.SelectDimPersonTypesDropdownList();
                var personTypeVal = PersonTypelist.Where(x => x.Value == supervisorInfoObj.PersonId).FirstOrDefault();
                ownerDetailsDT.Rows.Add();
                int PersonRowCount = ownerDetailsDT.Rows.Count - 1;
                ownerDetailsDT.Rows[PersonRowCount]["SalutationName"] = supervisorInfoObj.PersonTO.SalutationName;
                ownerDetailsDT.Rows[PersonRowCount]["FirstName"] = supervisorInfoObj.PersonTO.FirstName;
                ownerDetailsDT.Rows[PersonRowCount]["MiddleName"] = supervisorInfoObj.PersonTO.MidName;
                ownerDetailsDT.Rows[PersonRowCount]["LastName"] = supervisorInfoObj.PersonTO.LastName;
                if (personTypeVal != null)
                    ownerDetailsDT.Rows[PersonRowCount]["PersonType"] = personTypeVal.Text;
                ownerDetailsDT.Rows[PersonRowCount]["MobileNo"] = supervisorInfoObj.PersonTO.MobileNo;
                ownerDetailsDT.Rows[PersonRowCount]["AlternateMobNo"] = supervisorInfoObj.PersonTO.AlternateMobNo;
                ownerDetailsDT.Rows[PersonRowCount]["personPhoneNo"] = supervisorInfoObj.PersonTO.PhoneNo;
                ownerDetailsDT.Rows[PersonRowCount]["PrimaryEmail"] = supervisorInfoObj.PersonTO.PrimaryEmail;
                ownerDetailsDT.Rows[PersonRowCount]["AlternateEmail"] = supervisorInfoObj.PersonTO.AlternateEmail;
                if (supervisorInfoObj.PersonTO.DateOfBirth != DateTime.MinValue)
                {
                    var dateonly = supervisorInfoObj.PersonTO.DateOfBirth.ToString("d");
                    ownerDetailsDT.Rows[PersonRowCount]["DateOfBirth"] = dateonly;
                }
                byte[] PhotoCodeInBytes = null;
                if (supervisorInfoObj.PersonTO.PhotoBase64 != null)
                {
                    PhotoCodeInBytes = Convert.FromBase64String(supervisorInfoObj.PersonTO.PhotoBase64);
                }
                if (PhotoCodeInBytes != null)
                    ownerDetailsDT.Rows[PersonRowCount]["Photo"] = PhotoCodeInBytes;

                printDataSet.Tables.Add(headerDT);
                printDataSet.Tables.Add(ownerDetailsDT);
                printDataSet.Tables.Add(KYCDetailsDT);
                printDataSet.Tables.Add(contactDetailsDT);
                printDataSet.Tables.Add(organisationDT);
                printDataSet.Tables.Add(addressDT);

                printDataSet.Tables.Add(orgLicenseDtlDT);





                String templateFilePath = GetTemplateFilePath(supervisorInfoObj.PersonTO.OrgTypeId);
                if (String.IsNullOrEmpty(templateFilePath))
                {
                    resultMessage.DefaultBehaviour("Failed to Get template file path - GetTemplateFilePath");
                    return resultMessage;
                }


                String fileName = "Doc-" + DateTime.Now.Ticks;

                //download location for rewrite  template file
                String saveLocation = AppDomain.CurrentDomain.BaseDirectory + fileName + ".xls";
                Boolean IsProduction = true;
                TblConfigParamsTO tblConfigParamsTO = _iTblConfigParamsBL.SelectTblConfigParamsValByName("IS_PRODUCTION_ENVIRONMENT_ACTIVE");
                if (tblConfigParamsTO != null)
                {
                    if (Convert.ToInt32(tblConfigParamsTO.ConfigParamVal) == 0)
                    {
                        IsProduction = false;
                    }
                }
                resultMessage = _iRunReport.GenrateMktgInvoiceReport(printDataSet, templateFilePath, saveLocation, Constants.ReportE.PDF_DONT_OPEN, IsProduction);
                if (resultMessage.MessageType == ResultMessageE.Information)
                {
                    String filePath = String.Empty;
                    if (resultMessage.Tag != null && resultMessage.Tag.GetType() == typeof(String))
                    {

                        filePath = resultMessage.Tag.ToString();
                    }
                    String fileName1 = Path.GetFileName(saveLocation);
                    Byte[] bytes = File.ReadAllBytes(filePath);
                    if (bytes != null && bytes.Length > 0)
                    {
                        resultMessage.Tag = bytes;

                        string resFname = Path.GetFileNameWithoutExtension(saveLocation);
                        string directoryName;
                        directoryName = Path.GetDirectoryName(saveLocation);
                        string[] fileEntries = Directory.GetFiles(directoryName, "*Bill*");
                        string[] filesList = Directory.GetFiles(directoryName, "*Bill*");

                        foreach (string file in filesList)
                        {
                            //if (file.ToUpper().Contains(resFname.ToUpper()))
                            {
                                File.Delete(file);
                            }
                        }
                    }
                    if (resultMessage.MessageType == ResultMessageE.Information)
                    {
                        resultMessage.DefaultSuccessBehaviour(resultMessage.Tag);
                    }
                }
                else
                {
                    resultMessage.Text = "Something wents wrong please try again";
                    resultMessage.DisplayMessage = "Something wents wrong please try again";
                    resultMessage.Result = 0;
                }
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "");
                return resultMessage;
            }
            finally
            {

            }

        }
        public ResultMessage PrintOrganisationReport(TblOrganizationTO organisationObj)
        {
            Int32 OrganisationId = organisationObj.IdOrganization;
            Int32 OrgTypeId = organisationObj.OrgTypeId;
            Int32 StatusId = organisationObj.StatusId;
            Int32 SuppDivGroupId = organisationObj.SuppDivGroupId;
            //Int32 AddrTypeId = organisationObj.AddrId;
            //Int32 PersonTypeId = organisationObj.PersonList[0].PersonTypeId;
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                DataTable headerDT = new DataTable();
                DataTable organisationDT = new DataTable();
                DataTable addressDT = new DataTable();
                DataTable bankDetailsDT = new DataTable();
                DataTable contactDetailsDT = new DataTable();
                DataTable ownerDetailsDT = new DataTable();
                DataTable KYCDetailsDT = new DataTable();
                DataTable orgLicenseDtlDT = new DataTable();


                DataSet printDataSet = new DataSet();
                Int32 RowCount = 0;
                headerDT.TableName = "headerDT";
                organisationDT.TableName = "organisationDT";
                addressDT.TableName = "addressDT";
                bankDetailsDT.TableName = "bankDetailsDT";
                contactDetailsDT.TableName = "contactDetailsDT";
                ownerDetailsDT.TableName = "ownerDetailsDT";
                KYCDetailsDT.TableName = "KYCDetailsDT";
                orgLicenseDtlDT.TableName = "orgLicenseDtlDT";

                TblOrganizationTO tblOrganizationTOObj = SelectTblOrganizationTO(OrganisationId);
                if (tblOrganizationTOObj == null)
                {
                    resultMessage.DefaultBehaviour("Failed to get organisation details");
                    return resultMessage;
                }


                List<DimStatusTO> statusList = _iDimStatusBL.SelectAllDimStatusList((Int32)Constants.TransactionTypeE.DealerCat);
                var dealerCat = statusList.Where(x => x.IdStatus == StatusId).FirstOrDefault();

                List<DropDownTO> DropDownTOList = _iDimensionDAO.GetSupplierDivisionGroupDDL();
                var DivGroupId = DropDownTOList.Where(x => x.Value == SuppDivGroupId).FirstOrDefault();

                headerDT.Columns.Add("FirmName");
                headerDT.Columns.Add("CreditLimit");
                headerDT.Columns.Add("RegisteredMobileNos");
                headerDT.Columns.Add("CdStructure");
                headerDT.Columns.Add("Note");
                headerDT.Columns.Add("DigitalSign");
                headerDT.Columns.Add("DeliveryPeriod");
                headerDT.Columns.Add("PhoneNo");
                headerDT.Columns.Add("FaxNo");
                headerDT.Columns.Add("EmailID");
                headerDT.Columns.Add("Website");
                headerDT.Columns.Add("BrandName");
                headerDT.Columns.Add("DeclaredRate");
                headerDT.Columns.Add("FirmType");
                headerDT.Columns.Add("BalanceQuota");
                headerDT.Columns.Add("FirmCode");
                headerDT.Columns.Add("dealerCat");
                headerDT.Columns.Add("division");
                headerDT.Rows.Add();
                RowCount = headerDT.Rows.Count - 1;
                headerDT.Rows[0]["FirmName"] = "Firm Name";
                headerDT.Rows[0]["BalanceQuota"] = "Balance Quota";
                headerDT.Rows[0]["DeclaredRate"] = "Declared Rate";
                headerDT.Rows[0]["CdStructure"] = "Cd Structure";
                headerDT.Rows[0]["Note"] = "Note";
                headerDT.Rows[0]["DeliveryPeriod"] = "Delivery Period";
                headerDT.Rows[0]["DigitalSign"] = "Digital Sign";
                headerDT.Rows[0]["FirmCode"] = "FirmCode";
                headerDT.Rows[0]["FirmType"] = "FirmType";
                headerDT.Rows[0]["FirmType"] = "CreditLimit";
                headerDT.Rows[0]["RegisteredMobileNos"] = "Registered Mobile No";
                headerDT.Rows[0]["PhoneNo"] = "Phone No";
                headerDT.Rows[0]["FaxNo"] = "Fax No";
                headerDT.Rows[0]["EmailID"] = "EmailID";
                headerDT.Rows[0]["Website"] = "Website";
                headerDT.Rows[0]["dealerCat"] = "dealer Category";
                headerDT.Rows[0]["division"] = "Division";

                headerDT.Columns.Add("StreetName");
                headerDT.Columns.Add("AreaName");
                headerDT.Columns.Add("PlotNo");
                headerDT.Columns.Add("PinCode");
                headerDT.Columns.Add("CountryName");
                headerDT.Columns.Add("StateName");
                headerDT.Columns.Add("DistrictName");
                headerDT.Columns.Add("TalukaName");
                headerDT.Columns.Add("VillageName");
                headerDT.Columns.Add("AddressType");

                headerDT.Rows[RowCount]["AreaName"] = "Area Name";
                headerDT.Rows[RowCount]["PlotNo"] = "Plot No";
                headerDT.Rows[RowCount]["StreetName"] = "Street Name";
                headerDT.Rows[RowCount]["PinCode"] = "PinCode";
                headerDT.Rows[RowCount]["CountryName"] = "Country Name";
                headerDT.Rows[RowCount]["StateName"] = "State Name";
                headerDT.Rows[RowCount]["DistrictName"] = "District Name";
                headerDT.Rows[RowCount]["TalukaName"] = "Taluka Name";
                headerDT.Rows[RowCount]["VillageName"] = "Village Name";
                headerDT.Rows[RowCount]["AddressType"] = "Address Type";

                //owner details
                headerDT.Columns.Add("FirstName");
                headerDT.Columns.Add("MiddleName");
                headerDT.Columns.Add("LastName");

                headerDT.Columns.Add("PersonType");
                headerDT.Columns.Add("MobileNo");
                headerDT.Columns.Add("personPhoneNo");
                headerDT.Columns.Add("AlternateMobNo");
                headerDT.Columns.Add("DateOfBirth");
                headerDT.Columns.Add("PrimaryEmail");
                headerDT.Columns.Add("AlternateEmail");

                headerDT.Rows[RowCount]["FirstName"] = "First Name";
                headerDT.Rows[RowCount]["MiddleName"] = "Middle Name";
                headerDT.Rows[RowCount]["LastName"] = "Last Name";
                headerDT.Rows[RowCount]["PersonType"] = "Person Type";
                headerDT.Rows[RowCount]["MobileNo"] = "Mobile No";
                headerDT.Rows[RowCount]["AlternateMobNo"] = "Alternate Mob No";

                headerDT.Rows[RowCount]["personPhoneNo"] = "Phone No";
                headerDT.Rows[RowCount]["PrimaryEmail"] = "Primary Email";
                headerDT.Rows[RowCount]["AlternateEmail"] = "Alternate Email";

                headerDT.Columns.Add("BankName");
                headerDT.Columns.Add("AccountType");
                headerDT.Columns.Add("IfscCode");
                headerDT.Columns.Add("BranchName");
                headerDT.Columns.Add("BranchCode");
                headerDT.Columns.Add("AccountNo");
                headerDT.Columns.Add("NameOnCheque");

                headerDT.Rows[RowCount]["BankName"] = "Bank Name";
                headerDT.Rows[RowCount]["AccountType"] = "Account Type";
                headerDT.Rows[RowCount]["IfscCode"] = "Ifsc Code";
                headerDT.Rows[RowCount]["BranchName"] = "Branch Name";
                headerDT.Rows[RowCount]["AccountNo"] = "Account No";
                headerDT.Rows[RowCount]["NameOnCheque"] = "Name On Cheque";
                headerDT.Columns.Add("AgreementSign");
                headerDT.Columns.Add("ChequeReceived");
                headerDT.Columns.Add("KYCCompleted");

                headerDT.Rows[RowCount]["AgreementSign"] = "Agreement Sign";
                headerDT.Rows[RowCount]["ChequeReceived"] = "Cheque Received";
                headerDT.Rows[RowCount]["KYCCompleted"] = "KYC Completed";
                // for commercial
                //headerDT.Columns.Add("PANNo");
                //headerDT.Columns.Add("GSTINNo");
                headerDT.Columns.Add("LicenseName");
                headerDT.Columns.Add("LicenceNo");
                headerDT.Columns.Add("SalutationName");
                //headerDT.Rows[RowCount]["PANNo"] = "PAN No";
                //headerDT.Rows[RowCount]["GSTINNo"] = "GSTIN No";
                headerDT.Rows[RowCount]["LicenseName"] = "License Name";
                headerDT.Rows[RowCount]["LicenceNo"] = "Licence No";
                headerDT.Rows[RowCount]["SalutationName"] = "Salutation Name";
                organisationDT.Columns.Add("FirmName");
                organisationDT.Columns.Add("CreditLimit");
                organisationDT.Columns.Add("PhoneNo");
                organisationDT.Columns.Add("FaxNo");
                organisationDT.Columns.Add("EmailID");
                organisationDT.Columns.Add("Website");
                organisationDT.Columns.Add("RegisteredMobileNos");
                organisationDT.Columns.Add("DeclaredRate");
                organisationDT.Columns.Add("BalanceQuota");
                organisationDT.Columns.Add("ValidUpto");
                organisationDT.Columns.Add("CdStructure");
                organisationDT.Columns.Add("Note");
                organisationDT.Columns.Add("DeliveryPeriod");
                organisationDT.Columns.Add("DigitalSign");
                organisationDT.Columns.Add("FirmCode");
                organisationDT.Columns.Add("FirmType");
                organisationDT.Columns.Add("dealerCat");
                organisationDT.Columns.Add("division");

                if (tblOrganizationTOObj != null)
                {
                    organisationDT.Rows.Add();
                    organisationDT.Rows[RowCount]["FirmName"] = tblOrganizationTOObj.FirmName;
                    organisationDT.Rows[RowCount]["CreditLimit"] = tblOrganizationTOObj.CreditLimit;
                    organisationDT.Rows[RowCount]["RegisteredMobileNos"] = tblOrganizationTOObj.RegisteredMobileNos;
                    organisationDT.Rows[RowCount]["FirmCode"] = tblOrganizationTOObj.FirmCode;
                    organisationDT.Rows[RowCount]["FirmType"] = tblOrganizationTOObj.FirmType;
                    organisationDT.Rows[RowCount]["PhoneNo"] = tblOrganizationTOObj.PhoneNo;
                    organisationDT.Rows[RowCount]["FaxNo"] = tblOrganizationTOObj.FaxNo;
                    organisationDT.Rows[RowCount]["EmailID"] = tblOrganizationTOObj.EmailAddr;
                    organisationDT.Rows[RowCount]["Website"] = tblOrganizationTOObj.Website;
                    organisationDT.Rows[RowCount]["DigitalSign"] = tblOrganizationTOObj.DigitalSign;
                    organisationDT.Rows[RowCount]["DeclaredRate"] = tblOrganizationTOObj.DeclaredRate;
                    organisationDT.Rows[RowCount]["BalanceQuota"] = tblOrganizationTOObj.BalanceQuota;
                    organisationDT.Rows[RowCount]["ValidUpto"] = tblOrganizationTOObj.ValidUpto;
                    organisationDT.Rows[RowCount]["CdStructure"] = tblOrganizationTOObj.CdStructure;
                    organisationDT.Rows[RowCount]["Note"] = tblOrganizationTOObj.Remark;
                    organisationDT.Rows[RowCount]["DeliveryPeriod"] = tblOrganizationTOObj.DeliveryPeriod;
                    //if (dealerCat != null)
                    //    organisationDT.Rows[0]["dealerCat"] = dealerCat.StatusDesc;
                    if (DivGroupId != null)
                        organisationDT.Rows[RowCount]["division"] = DivGroupId.Value;
                }

                //MemoryStream ms = new MemoryStream(bytes1);
                //System.Drawing.Image myImg = System.Drawing.Image.FromStream(ms);


                //Address Details-----------------------------

                // addressDT.Columns.Add("PlotNo");
                addressDT.Columns.Add("PlotNo");
                addressDT.Columns.Add("StreetName");
                addressDT.Columns.Add("AreaName");
                addressDT.Columns.Add("PinCode");
                addressDT.Columns.Add("CountryName");
                addressDT.Columns.Add("StateName");
                addressDT.Columns.Add("DistrictName");
                addressDT.Columns.Add("TalukaName");
                addressDT.Columns.Add("VillageName");
                addressDT.Columns.Add("AddressType");
                List<TblAddressTO> tblAddressTOList = _iTblAddressDAO.SelectOrgAddressList(OrganisationId);
                Int32 AddressCnt = 2;
                TblConfigParamsTO tblConfigParamsTOTemplateAddressCount = _iTblConfigParamsBL.SelectTblConfigParamsValByName(Constants.ADDRESS_COUNT_PRINT_ORGANISATION_MASTER_TEMPLATE);
                if (tblConfigParamsTOTemplateAddressCount != null)
                {
                    AddressCnt = Convert.ToInt32(tblConfigParamsTOTemplateAddressCount.ConfigParamVal);
                }
                string typeAddress = (Int32)Constants.AddressTypeE.FACTORY_ADDRESS + "," + (Int32)Constants.AddressTypeE.OFFICE_ADDRESS;
                TblConfigParamsTO tblConfigParamsTOTemplateAddressType = _iTblConfigParamsBL.SelectTblConfigParamsValByName(Constants.ADDRESS_TYPE_PRINT_ORGANISATION_MASTER_TEMPLATE);
                int[] addressType = new int[100];
                if (tblConfigParamsTOTemplateAddressType != null)
                {
                    typeAddress = tblConfigParamsTOTemplateAddressType.ConfigParamVal.ToString();
                }
                if (!string.IsNullOrEmpty(typeAddress))
                {
                    addressType = typeAddress.Split(',').Select(n => Convert.ToInt32(n)).ToArray();
                }
                if (tblAddressTOList != null && tblAddressTOList.Count > 0)
                {
                    List<DropDownTO> AddressDropDownTOList = _iDimensionDAO.SelectAddressTypeListForDropDown();
                    for (int m = 0; m < addressType.Length; m++)
                    {
                        if (AddressCnt == 0)
                            break;
                        var matchTO = tblAddressTOList.Where(w => w.AddrTypeId == addressType[m]).FirstOrDefault();
                        if (matchTO != null)
                        {
                            var addressTypeNameTO = AddressDropDownTOList.Where(x => x.Value == matchTO.AddrTypeId).FirstOrDefault();
                            addressDT.Rows.Add();
                            int AddressRowCount = addressDT.Rows.Count - 1;
                            addressDT.Rows[AddressRowCount]["PlotNo"] = matchTO.PlotNo;
                            addressDT.Rows[AddressRowCount]["StreetName"] = matchTO.StreetName;
                            addressDT.Rows[AddressRowCount]["AreaName"] = matchTO.AreaName;
                            addressDT.Rows[AddressRowCount]["PinCode"] = matchTO.Pincode;
                            addressDT.Rows[AddressRowCount]["CountryName"] = matchTO.CountryName;
                            addressDT.Rows[AddressRowCount]["StateName"] = matchTO.StateName;
                            addressDT.Rows[AddressRowCount]["DistrictName"] = matchTO.DistrictName;
                            addressDT.Rows[AddressRowCount]["TalukaName"] = matchTO.TalukaName;
                            addressDT.Rows[AddressRowCount]["VillageName"] = matchTO.VillageName;
                            if (addressType != null)
                                addressDT.Rows[AddressRowCount]["AddressType"] = addressTypeNameTO.Text;
                            AddressCnt -= 1;
                        }
                    }
                }
                //Bank Details
                bankDetailsDT.Columns.Add("BankName");
                bankDetailsDT.Columns.Add("AccountType");
                bankDetailsDT.Columns.Add("IfscCode");
                bankDetailsDT.Columns.Add("BranchName");
                bankDetailsDT.Columns.Add("AccountNo");
                bankDetailsDT.Columns.Add("NameOnCheque");
                List<TblOrgBankDetailsTO> tblOrgBankDetailsTOlist = _iTblOrgBankDetailsDAO.SelectOrgBankDetailsList(OrganisationId);


                if (tblOrgBankDetailsTOlist != null && tblOrgBankDetailsTOlist.Count > 0)
                {
                    for (int i = 0; i < tblOrgBankDetailsTOlist.Count; i++)
                    {
                        bankDetailsDT.Rows.Add();
                        bankDetailsDT.Rows[i]["BankName"] = tblOrgBankDetailsTOlist[i].BankName;
                        bankDetailsDT.Rows[i]["AccountType"] = tblOrgBankDetailsTOlist[i].AccountTypeName;
                        bankDetailsDT.Rows[i]["IfscCode"] = tblOrgBankDetailsTOlist[i].IfscCode;
                        bankDetailsDT.Rows[i]["BranchName"] = tblOrgBankDetailsTOlist[i].BranchName;
                        bankDetailsDT.Rows[i]["AccountNo"] = tblOrgBankDetailsTOlist[i].AccountNo;
                        bankDetailsDT.Rows[i]["NameOnCheque"] = tblOrgBankDetailsTOlist[i].NameOnCheque;

                    }
                }



                // List<TblPersonTO> tblPersonTOlist = _iTblPersonDAO.SelectAllTblPersonByOrganization(OrganisationId);
                List<TblPersonTO> tblPersonTOlist = null;
                tblPersonTOlist = _iTblPersonDAO.SelectAllTblPersonByOrganization(OrganisationId);
                List<TblPersonTO> MultipersonList = null;
                MultipersonList = _iTblPersonDAO.SelectMultipleTblPersonByOrganization(OrganisationId);

                if (tblPersonTOlist != null && MultipersonList != null)
                {
                    MultipersonList.ForEach(ele =>
                    {
                        var list = tblPersonTOlist.Where(e => e.IdPerson == ele.IdPerson).ToList();
                        if (list == null || list.Count == 0)
                            tblPersonTOlist.Add(ele);
                    });

                }
                ownerDetailsDT.Columns.Add("SalutationName");
                ownerDetailsDT.Columns.Add("FirstName");
                ownerDetailsDT.Columns.Add("MiddleName");
                ownerDetailsDT.Columns.Add("LastName");
                ownerDetailsDT.Columns.Add("PersonType");
                ownerDetailsDT.Columns.Add("MobileNo");
                ownerDetailsDT.Columns.Add("personPhoneNo");
                ownerDetailsDT.Columns.Add("AlternateMobNo");
                ownerDetailsDT.Columns.Add("DateOfBirth");
                ownerDetailsDT.Columns.Add("PrimaryEmail");
                ownerDetailsDT.Columns.Add("AlternateEmail");
                ownerDetailsDT.Columns.Add("Photo", typeof(byte[]));

                Int32 PersonCnt = 2;
                TblConfigParamsTO tblConfigParamsTOTemplatePersonCount = _iTblConfigParamsBL.SelectTblConfigParamsValByName(Constants.PERSON_COUNT_PRINT_ORGANISATION_MASTER_TEMPLATE);
                if (tblConfigParamsTOTemplatePersonCount != null)
                {
                    PersonCnt = Convert.ToInt32(tblConfigParamsTOTemplatePersonCount.ConfigParamVal);
                }
                string typePerson = (Int32)Constants.VisitPersonE.FIRST_OWNER + "," + (Int32)Constants.VisitPersonE.SECOND_OWNER;
                TblConfigParamsTO tblConfigParamsTOTemplatePersonType = _iTblConfigParamsBL.SelectTblConfigParamsValByName(Constants.PERSON_TYPE_PRINT_ORGANISATION_MASTER_TEMPLATE);
                int[] personType = new int[100];
                if (tblConfigParamsTOTemplatePersonType != null)
                {
                    typePerson = tblConfigParamsTOTemplatePersonType.ConfigParamVal.ToString();
                }
                if (!string.IsNullOrEmpty(typePerson))
                {
                    personType = typePerson.Split(',').Select(n => Convert.ToInt32(n)).ToArray();
                }
                String TransactionType = organisationObj.ScreenName;
                Int32 ModuleId = (Int32)Constants.ModuleType.SIMPLIDELIVER;

                if (tblPersonTOlist != null && tblPersonTOlist.Count > 0)
                {
                    List<DropDownTO> PersonTypelist = _iTblPersonDAO.SelectDimPersonTypesDropdownList();
                    for (int m = 0; m < personType.Length; m++)
                    {
                        if (PersonCnt == 0)
                            break;
                        var matchTO = tblPersonTOlist.Where(w => w.PersonTypeId == personType[m]).FirstOrDefault();
                        if (matchTO != null)
                        {
                            byte[] PhotoCodeInBytes = null;
                            String PageElementId = "personInfo" + matchTO.CommonAttachId;
                            List<TblAddonsFunDtlsTO> PersonImageList = _iTblAddonsFunDtlsDAO.SelectAddonDetailsList(OrganisationId, ModuleId, TransactionType, PageElementId, null);
                            if (PersonImageList != null && PersonImageList.Count > 0)
                            {
                                TblAddonsFunDtlsTO photocode = PersonImageList.Where(x => x.FunRefId == Constants.Fun_Ref_Id).FirstOrDefault();
                                if (photocode != null)
                                {
                                    WebClient wc = new WebClient();
                                    PhotoCodeInBytes = wc.DownloadData(photocode.FunRefVal);
                                }
                            }

                            //var personTypeVal = PersonTypelist.Where(x => x.Value == tblPersonTOlist[m].PersonTypeId).FirstOrDefault();
                            var personTypeVal = PersonTypelist.Where(x => x.Value == matchTO.PersonTypeId).FirstOrDefault();
                            ownerDetailsDT.Rows.Add();
                            int PersonRowCount = ownerDetailsDT.Rows.Count - 1;
                            ownerDetailsDT.Rows[PersonRowCount]["SalutationName"] = matchTO.SalutationName;
                            ownerDetailsDT.Rows[PersonRowCount]["FirstName"] = matchTO.FirstName;
                            ownerDetailsDT.Rows[PersonRowCount]["MiddleName"] = matchTO.MidName;
                            ownerDetailsDT.Rows[PersonRowCount]["LastName"] = matchTO.LastName;
                            if (personTypeVal != null)
                                ownerDetailsDT.Rows[PersonRowCount]["PersonType"] = personTypeVal.Text;
                            ownerDetailsDT.Rows[PersonRowCount]["MobileNo"] = matchTO.MobileNo;
                            ownerDetailsDT.Rows[PersonRowCount]["AlternateMobNo"] = matchTO.AlternateMobNo;
                            ownerDetailsDT.Rows[PersonRowCount]["personPhoneNo"] = matchTO.PhoneNo;
                            ownerDetailsDT.Rows[PersonRowCount]["PrimaryEmail"] = matchTO.PrimaryEmail;
                            ownerDetailsDT.Rows[PersonRowCount]["AlternateEmail"] = matchTO.AlternateEmail;
                            if (matchTO.DateOfBirth != DateTime.MinValue)
                            {
                                var dateonly = matchTO.DateOfBirth.ToString("d");
                                ownerDetailsDT.Rows[PersonRowCount]["DateOfBirth"] = dateonly;
                            }

                            if (PhotoCodeInBytes != null)
                                ownerDetailsDT.Rows[PersonRowCount]["Photo"] = PhotoCodeInBytes;
                            PersonCnt -= 1;
                        }
                    }


                }
                //for KYC Details
                KYCDetailsDT.Columns.Add("AgreementSign");
                KYCDetailsDT.Columns.Add("ChequeReceived");
                KYCDetailsDT.Columns.Add("KYCCompleted");

                List<TblKYCDetailsTO> tblKYCDetailsTOList = _iTblKYCDetailsBL.SelectTblKYCDetailsTOByOrgId(OrganisationId);
                if (tblKYCDetailsTOList != null && tblKYCDetailsTOList.Count > 0)
                {
                    for (int i = 0; i < tblKYCDetailsTOList.Count; i++)
                    {
                        KYCDetailsDT.Rows.Add();
                        if (tblKYCDetailsTOList[i].AggrSign == 1)
                            KYCDetailsDT.Rows[i]["AgreementSign"] = "Yes";
                        else
                            KYCDetailsDT.Rows[i]["AgreementSign"] = "No";
                        if (tblKYCDetailsTOList[i].ChequeRcvd == 1)
                            KYCDetailsDT.Rows[i]["ChequeReceived"] = "Yes";
                        else
                            KYCDetailsDT.Rows[i]["ChequeReceived"] = "No";
                        if (tblKYCDetailsTOList[i].KYCCompleted == 1)
                            KYCDetailsDT.Rows[i]["KYCCompleted"] = "Yes";
                        else
                            KYCDetailsDT.Rows[i]["KYCCompleted"] = "No";
                    }
                }

                orgLicenseDtlDT.Columns.Add("LicenseName");
                orgLicenseDtlDT.Columns.Add("LicenseNo");
                List<TblOrgLicenseDtlTO> tblOrgLicenseDtlTOList = _iTblOrgLicenseDtlDAO.SelectAllTblOrgLicenseDtl(OrganisationId);
                if (tblOrgLicenseDtlTOList != null && tblOrgLicenseDtlTOList.Count > 0)
                {
                    var matchToPAN = tblOrgLicenseDtlTOList.Where(w => w.LicenseId == (Int32)Constants.CommercialLicenseE.PAN_NO).FirstOrDefault();
                    int licenceRowCnt;
                    if (matchToPAN != null)
                    {
                        //if (!string.IsNullOrEmpty(matchToPAN.LicenseValue))
                        if (matchToPAN.LicenseValue != "0")
                        {
                            orgLicenseDtlDT.Rows.Add();
                            licenceRowCnt = orgLicenseDtlDT.Rows.Count - 1;
                            orgLicenseDtlDT.Rows[licenceRowCnt]["LicenseName"] = matchToPAN.LicenseName;
                            orgLicenseDtlDT.Rows[licenceRowCnt]["LicenseNo"] = matchToPAN.LicenseValue;
                        }
                    }

                    var matchToGSTIN = tblOrgLicenseDtlTOList.Where(w => w.LicenseId == (Int32)Constants.CommercialLicenseE.IGST_NO).FirstOrDefault();
                    if (matchToGSTIN != null) //|| matchToGSTIN.LicenseValue != 0
                    {
                        if (matchToGSTIN.LicenseValue != "0")
                        {
                            orgLicenseDtlDT.Rows.Add();
                            licenceRowCnt = orgLicenseDtlDT.Rows.Count - 1;
                            orgLicenseDtlDT.Rows[licenceRowCnt]["LicenseName"] = matchToGSTIN.LicenseName;
                            orgLicenseDtlDT.Rows[licenceRowCnt]["LicenseNo"] = matchToGSTIN.LicenseValue;
                        }
                    }
                    var matchToADHAR = tblOrgLicenseDtlTOList.Where(w => w.LicenseId == (Int32)Constants.CommercialLicenseE.AADHAR_NO).FirstOrDefault();
                    if (matchToADHAR != null)
                    {
                        if (matchToADHAR.LicenseValue != "0")
                        {
                            orgLicenseDtlDT.Rows.Add();
                            licenceRowCnt = orgLicenseDtlDT.Rows.Count - 1;
                            orgLicenseDtlDT.Rows[licenceRowCnt]["LicenseName"] = matchToADHAR.LicenseName;
                            orgLicenseDtlDT.Rows[licenceRowCnt]["LicenseNo"] = matchToADHAR.LicenseValue;
                        }
                    }

                }


                printDataSet.Tables.Add(headerDT);
                printDataSet.Tables.Add(organisationDT);
                printDataSet.Tables.Add(addressDT);
                printDataSet.Tables.Add(ownerDetailsDT);
                printDataSet.Tables.Add(bankDetailsDT);
                printDataSet.Tables.Add(KYCDetailsDT);
                printDataSet.Tables.Add(orgLicenseDtlDT);



                String templateFilePath = GetTemplateFilePath(OrgTypeId);
                if (String.IsNullOrEmpty(templateFilePath))
                {
                    resultMessage.DefaultBehaviour("Failed to Get template file path - GetTemplateFilePath");
                    return resultMessage;
                }


                String fileName = "Doc-" + DateTime.Now.Ticks;

                //download location for rewrite  template file
                String saveLocation = AppDomain.CurrentDomain.BaseDirectory + fileName + ".xls";
                Boolean IsProduction = true;
                TblConfigParamsTO tblConfigParamsTO = _iTblConfigParamsBL.SelectTblConfigParamsValByName("IS_PRODUCTION_ENVIRONMENT_ACTIVE");
                if (tblConfigParamsTO != null)
                {
                    if (Convert.ToInt32(tblConfigParamsTO.ConfigParamVal) == 0)
                    {
                        IsProduction = false;
                    }
                }
                resultMessage = _iRunReport.GenrateMktgInvoiceReport(printDataSet, templateFilePath, saveLocation, Constants.ReportE.PDF_DONT_OPEN, IsProduction);
                if (resultMessage.MessageType == ResultMessageE.Information)
                {
                    String filePath = String.Empty;
                    if (resultMessage.Tag != null && resultMessage.Tag.GetType() == typeof(String))
                    {

                        filePath = resultMessage.Tag.ToString();
                    }
                    String fileName1 = Path.GetFileName(saveLocation);
                    Byte[] bytes = File.ReadAllBytes(filePath);
                    if (bytes != null && bytes.Length > 0)
                    {
                        resultMessage.Tag = bytes;

                        string resFname = Path.GetFileNameWithoutExtension(saveLocation);
                        string directoryName;
                        directoryName = Path.GetDirectoryName(saveLocation);
                        string[] fileEntries = Directory.GetFiles(directoryName, "*Bill*");
                        string[] filesList = Directory.GetFiles(directoryName, "*Bill*");

                        foreach (string file in filesList)
                        {
                            //if (file.ToUpper().Contains(resFname.ToUpper()))
                            {
                                File.Delete(file);
                            }
                        }
                    }
                    if (resultMessage.MessageType == ResultMessageE.Information)
                    {
                        resultMessage.DefaultSuccessBehaviour(resultMessage.Tag);
                    }
                }
                else
                {
                    resultMessage.Text = "Something wents wrong please try again";
                    resultMessage.DisplayMessage = "Something wents wrong please try again";
                    resultMessage.Result = 0;
                }
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "");
                return resultMessage;
            }
            finally
            {

            }
        }
        public String GetTemplateFilePath(Int32 OrgTypeId)
        {
            string templateFilePath = "";
            TblConfigParamsTO tblConfigParamsTOTemplateName = _iTblConfigParamsBL.SelectTblConfigParamsValByName(Constants.MASTER_IS_SAME_ORGANISATION_TEMPLATE_USE);
            if (tblConfigParamsTOTemplateName != null)
            {
                if (Convert.ToInt32(tblConfigParamsTOTemplateName.ConfigParamVal) == 1)
                {
                    templateFilePath = _iDimReportTemplateBL.SelectReportFullName(Constants.Organisation_Report_NAME);
                }
                else
                {
                    switch (OrgTypeId)
                    {
                        case (Int32)Constants.OrgTypeE.DEALER:
                            templateFilePath = _iDimReportTemplateBL.SelectReportFullName(Constants.Dealer_Report_Name);
                            break;

                        //case (Int32)Constants.OrgTypeE.DEALER:
                        //    templateFilePath = _iDimReportTemplateBL.SelectReportFullName(Constants.Distributor_Report_Name);
                        //    break;
                        case (Int32)Constants.OrgTypeE.TRANSPOTER:
                            templateFilePath = _iDimReportTemplateBL.SelectReportFullName(Constants.Transporter_Report_Name);
                            break;
                        case (Int32)Constants.OrgTypeE.COMPETITOR:
                            templateFilePath = _iDimReportTemplateBL.SelectReportFullName(Constants.Competitor_Report_Name);
                            break;
                        case (Int32)Constants.OrgTypeE.PURCHASE_COMPETITOR:
                            templateFilePath = _iDimReportTemplateBL.SelectReportFullName(Constants.PurchaseCompetitor_Report_Name);
                            break;
                        case (Int32)Constants.OrgTypeE.SUPPLIER:
                            templateFilePath = _iDimReportTemplateBL.SelectReportFullName(Constants.Supplier_Report_Name);
                            break;
                        //case (Int32)Constants.OrgTypeE.DEALER:
                        //    templateFilePath = _iDimReportTemplateBL.SelectReportFullName(Constants.Supervisor_Report_Name);
                        //    break;
                        case (Int32)Constants.OrgTypeE.INFLUENCER:
                            templateFilePath = _iDimReportTemplateBL.SelectReportFullName(Constants.Influencer_Report_Name);
                            break;
                        case (Int32)Constants.OrgTypeE.INTERNAL:
                            templateFilePath = _iDimReportTemplateBL.SelectReportFullName(Constants.InternalOrganization_Report_Name);
                            break;
                        case (Int32)Constants.OrgTypeE.BANK:
                            templateFilePath = _iDimReportTemplateBL.SelectReportFullName(Constants.Bank_Report_Name);
                            break;
                    }
                }
            }
            return templateFilePath;
        }

        public List<OrgExportRptTO> SelectAllOrgListToExport(Int32 orgTypeId, Int32 parentId)
        {

            List<OrgExportRptTO> list = _iTblOrganizationDAO.SelectAllOrgListToExport(orgTypeId, parentId);
            if (list != null && orgTypeId == (int)Constants.OrgTypeE.DEALER)
                list = list.OrderBy(a => a.CnfName).ThenBy(d => d.FirmName).ToList();
            else if (list != null)
                list = list.OrderBy(a => a.FirmName).ToList();

            return list;

            

        }

        // [2021-02-03] Added by Gokul
        public ResultMessage GetAllOrgDetailsToExport(Int32 orgTypeId, Int32 parentId)
        {
            ResultMessage resultMessage = new ResultMessage();

            try
            {
                DataSet dataSet = new DataSet();
                DataTable OrgListToExport = new DataTable();
                List<OrgExportRptTO> list = _iTblOrganizationDAO.SelectAllOrgListToExport(orgTypeId, parentId);
                if (list != null && orgTypeId == (int)Constants.OrgTypeE.DEALER)
                    list = list.OrderBy(a => a.CnfName).ThenBy(d => d.FirmName).ToList();
                else if (list != null)
                    list = list.OrderBy(a => a.FirmName).ToList();


                //Added By gokul
                if (list != null && list.Count > 0)
                {
                    OrgListToExport = Common.ToDataTable(list);
                    OrgListToExport.TableName = "OrganizationRptDT";
                    dataSet.Tables.Add(OrgListToExport);

                    DimOrgTypeTO dimOrgType = _iDimOrgTypeBL.SelectDimOrgTypeTO(orgTypeId);
                    String ReportTemplateName = dimOrgType.ExportRptTemplateName;
                    String templateFilePath = _iDimReportTemplateBL.SelectReportFullName(ReportTemplateName);
                    String fileName = "Doc-" + DateTime.Now.Ticks;
                    String saveLocation = AppDomain.CurrentDomain.BaseDirectory + fileName + ".xls";
                    Boolean IsProduction = true;

                    TblConfigParamsTO tblConfigParamsTO = _iTblConfigParamsDAO.SelectTblConfigParamsValByName("IS_PRODUCTION_ENVIRONMENT_ACTIVE");
                    if (tblConfigParamsTO != null)
                    {
                        if (Convert.ToInt32(tblConfigParamsTO.ConfigParamVal) == 0)
                        {
                            IsProduction = false;
                        }
                    }
                    resultMessage = _iRunReport.GenrateMktgInvoiceReport(dataSet, templateFilePath, saveLocation, Constants.ReportE.EXCEL_DONT_OPEN, IsProduction);
                    if (resultMessage.MessageType == ResultMessageE.Information)
                    {
                        String filePath = String.Empty;
                        if (resultMessage.Tag != null && resultMessage.Tag.GetType() == typeof(String))
                        {

                            filePath = resultMessage.Tag.ToString();
                        }
                        //driveName + path;
                        int returnPath = 0;
                        if (returnPath != 1)
                        {
                            String fileName1 = Path.GetFileName(saveLocation);
                            Byte[] bytes = File.ReadAllBytes(filePath);
                            if (bytes != null && bytes.Length > 0)
                            {
                                resultMessage.Tag = bytes;

                                string resFname = Path.GetFileNameWithoutExtension(saveLocation);
                                string directoryName;
                                directoryName = Path.GetDirectoryName(saveLocation);
                                string[] fileEntries = Directory.GetFiles(directoryName, "*Doc*");
                                string[] filesList = Directory.GetFiles(directoryName, "*Doc*");

                                foreach (string file in filesList)
                                {
                                    //if (file.ToUpper().Contains(resFname.ToUpper()))
                                    {
                                        File.Delete(file);
                                    }
                                }
                            }

                            if (resultMessage.MessageType == ResultMessageE.Information)
                            {
                                resultMessage.DefaultSuccessBehaviour(resultMessage.Tag);
                                //return resultMessage;

                            }
                        }

                    }
                    else
                    {
                        resultMessage.Text = "Something wents wrong please try again";
                        resultMessage.DisplayMessage = "Something wents wrong please try again";
                        resultMessage.Result = 0;
                    }
                }
               
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "");
                return resultMessage;
            }
         
        }

        /// <summary>
        /// [2017-11-17] Vijaymala:Added To get organization list of particular region;
        /// </summary>
        /// <param name="orgTypeId"></param>
        /// <param name="districtId"></param>
        /// <returns></returns>
        public List<TblOrganizationTO> SelectOrganizationListByRegion(Int32 orgTypeId, Int32 districtId)
        {
            return _iTblOrganizationDAO.SelectOrganizationListByRegion(orgTypeId, districtId);
        }

        public TblOrganizationTO SelectTblOrganizationTOByEnqRefId(String enq_ref_id)
        {
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                conn.Open();
                return _iTblOrganizationDAO.SelectTblOrganizationTOByEnqRefId(enq_ref_id, conn, tran);
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "SelectTblOrganizationTO");
                return null;
            }
            finally
            {
                conn.Close();
            }
        }

        //Sudhir[23-APR-2018] Added for Check OrgName And Phone no is Already Exist or Not.
        public ResultMessage CheckOrgNameOrPhoneNoIsExist(String OrgName, String PhoneNo)
        {
            ResultMessage resultMessage = new ResultMessage();
            resultMessage.Result = 1;
            resultMessage.Text = "Valid Name And Phone Number";
            resultMessage.DisplayMessage = "Valid Name And Phone Number";
            resultMessage.MessageType = ResultMessageE.Information;
            Boolean isExistOrNot = false;
            try
            {
                List<TblOrganizationTO> allOrganizationList = _iTblOrganizationDAO.SelectAllOrganizationListV2();
                if (allOrganizationList != null && allOrganizationList.Count > 0)
                {
                    if (OrgName != String.Empty || PhoneNo != String.Empty)
                    {
                        if (OrgName != String.Empty && OrgName != null)
                        {
                            isExistOrNot = allOrganizationList.Any(str => str.FirmName.Trim() == OrgName.Trim());
                            if (isExistOrNot)
                            {
                                resultMessage.Text = "Organization Name is Already Exist";
                                resultMessage.MessageType = ResultMessageE.Error;
                                resultMessage.Result = -1;
                                resultMessage.DisplayMessage = "Organization Name is Already Exist";
                                return resultMessage;
                            }
                        }

                        if (PhoneNo != String.Empty && PhoneNo != null)
                        {
                            isExistOrNot = allOrganizationList.Any(str => str.PhoneNo == PhoneNo.Trim());
                            if (isExistOrNot)
                            {
                                resultMessage.Text = "Phone Number is Already Exist";
                                resultMessage.MessageType = ResultMessageE.Error;
                                resultMessage.Result = -1;
                                resultMessage.DisplayMessage = "Phone Number is Already Exist";
                                return resultMessage;
                            }
                        }

                        return resultMessage;
                    }
                    return resultMessage;
                }
                return resultMessage;
            }
            catch (Exception ex)
            {
                return resultMessage;
            }
        }

        /// <summary>
        /// Sudhir[26-July-2018] --Add this Method for District & field officer link to be establish. 
        ///                        Regional manger can see his field office visit list. 
        ///                        Also field office can see their own visits
        /// </summary>
        /// <param name="cnfId"></param>
        /// <returns></returns>
        public List<DropDownTO> SelectDealerListForDropDownForCRM(Int32 cnfId, TblUserRoleTO tblUserRoleTO,int orgTypeId=0)
        {
            TblConfigParamsTO tblConfigParamsTO = _iTblConfigParamsBL.SelectTblConfigParamsTO(Constants.CP_DEFAULT_MATE_COMP_ORGID);
            if (tblConfigParamsTO != null)
            {
                if (cnfId.ToString() == tblConfigParamsTO.ConfigParamVal)
                {
                    cnfId = 0;
                }
            }

            return _iTblOrganizationDAO.SelectDealerListForDropDownForCRM(cnfId, tblUserRoleTO,orgTypeId);
        }

        public List<DropDownTO> SelectSalesEngineerListForDropDown(Int32 orgId)
        {
            return _iTblOrganizationDAO.SelectSalesEngineerListForDropDown(orgId);
        }

        #endregion

        #region Insertion
        public int InsertTblOrganization(TblOrganizationTO tblOrganizationTO)
        {
            return _iTblOrganizationDAO.InsertTblOrganization(tblOrganizationTO);
        }

        public int InsertTblOrganization(TblOrganizationTO tblOrganizationTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblOrganizationDAO.InsertTblOrganization(tblOrganizationTO, conn, tran);
        }

        public ResultMessage SaveNewOrganization(TblOrganizationTO tblOrganizationTO)
        {
            ResultMessage rMessage = new StaticStuff.ResultMessage();
            rMessage.MessageType = ResultMessageE.None;
            rMessage.Text = "Not Entered Into Loop";
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;

            try
            {

                conn.Open();
                tran = conn.BeginTransaction();

                #region Check Duplicate Supplier Name
                tblOrganizationTO.FirmName = StaticStuff.Constants.removeUnwantedSpaces(tblOrganizationTO.FirmName);
                Int32 IsAllowDuplicateFirmName = 0;
                TblConfigParamsTO AllowDupFirmNameTO = _iTblConfigParamsDAO.SelectTblConfigParamsValByName(Constants.IS_ALLOW_DUPLICATE_ORGANISATION_FIRM_NAME);
                if (AllowDupFirmNameTO != null)
                {
                    if (!String.IsNullOrEmpty(AllowDupFirmNameTO.ConfigParamVal))
                    {
                        if (Convert.ToInt32(AllowDupFirmNameTO.ConfigParamVal) == 1)
                        {
                            IsAllowDuplicateFirmName = Convert.ToInt32(AllowDupFirmNameTO.ConfigParamVal);
                        }
                    }
                }
                if (tblOrganizationTO.FirmName != null && IsAllowDuplicateFirmName == 0)
                {
                    rMessage = _iTblOrganizationDAO.SelectFirmName(tblOrganizationTO,false,conn,tran);
                    if (rMessage.Result == 0)
                    {
                        tran.Rollback();
                        rMessage.DefaultBehaviour("Organization name already exist with Id = " + rMessage.Text+ ". Organization Name should be Unique");
                        return rMessage;
                    }
                }
                #endregion
                //#region Check Duplication For Mobile numbers
                ////For Org Mobile Number
                //if (tblOrganizationTO.RegisteredMobileNos != null && !String.IsNullOrEmpty(tblOrganizationTO.RegisteredMobileNos))
                //{

                //    Boolean isDuplicateMobileNo = _iTblOrgBankDetailsDAO.isDuplicateMobileNo(tblOrganizationTO.RegisteredMobileNos,(int)Constants.MobileDupChecktypeE.OrganizationRegMobNo);
                //    if (isDuplicateMobileNo)
                //    {
                //        tran.Rollback();
                //        rMessage.DefaultBehaviour("Registration Mobile No is already assigned to other organization");
                //        return rMessage;
                //    }

                //}

                ////For Contact Details
                //if (tblOrganizationTO.PhoneNo != null && !String.IsNullOrEmpty(tblOrganizationTO.PhoneNo))
                //{

                //    Boolean isDuplicateMobileNo = _iTblOrgBankDetailsDAO.isDuplicateMobileNo(tblOrganizationTO.PhoneNo, (int)Constants.MobileDupChecktypeE.ContactDetailsMobNo);
                //    if (isDuplicateMobileNo)
                //    {
                //        tran.Rollback();
                //        rMessage.DefaultBehaviour("Phone No of Contact Details is already assigned to other organization");
                //        return rMessage;
                //    }

                //}

                ////For Person Mobile Number
                //if (tblOrganizationTO.PersonList != null && tblOrganizationTO.PersonList.Count > 0)
                //{
                //    for (int j = 0; j < tblOrganizationTO.PersonList.Count; j++)
                //    {
                //        TblPersonTO personTo = tblOrganizationTO.PersonList[j];
                //        Boolean isDuplicateMobileNo = _iTblOrgBankDetailsDAO.isDuplicateMobileNo(personTo.MobileNo, (int)Constants.MobileDupChecktypeE.PersonMobNo);
                //        if (isDuplicateMobileNo)
                //        {
                //            tran.Rollback();
                //            rMessage.DefaultBehaviour("Mobile No of "+personTo.FirstName+" " + personTo.LastName + " is already assigned to other Person ");
                //            return rMessage;
                //        }
                //    }
                //}

                ////For Person Alternate mobile number
                //if (tblOrganizationTO.PersonList != null && tblOrganizationTO.PersonList.Count > 0)
                //{
                //    for (int j = 0; j < tblOrganizationTO.PersonList.Count; j++)
                //    {
                //        TblPersonTO personTo = tblOrganizationTO.PersonList[j];
                //        Boolean isDuplicateMobileNo = _iTblOrgBankDetailsDAO.isDuplicateMobileNo(personTo.AlternateMobNo, (int)Constants.MobileDupChecktypeE.AltPersonMobNo);
                //        if (isDuplicateMobileNo)
                //        {
                //            tran.Rollback();
                //            rMessage.DefaultBehaviour(" Alternate Mobile No of " + personTo.FirstName + " " + personTo.LastName + " is already assigned to other Person ");
                //            return rMessage;
                //        }
                //    }
                //}

                //#endregion
                Int32 IsLiscenseAgaintsAddress = 0;
                TblConfigParamsTO tblConfigParams = _iTblConfigParamsDAO.SelectTblConfigParamsValByName(Constants.CP_LICENSE_AGAINST_ADDRESS);
                if (tblConfigParams != null)
                {
                    if (Convert.ToInt32(tblConfigParams.ConfigParamVal) == 1)
                    {
                        IsLiscenseAgaintsAddress = 1;
                    }
                }
                #region check Duplication for GST no
                if (IsLiscenseAgaintsAddress == 1)
                {
                    if (tblOrganizationTO.AddressList != null && tblOrganizationTO.AddressList.Count > 0)
                    {
                        for (int i = 0; i < tblOrganizationTO.AddressList.Count; i++)
                        {
                            if (tblOrganizationTO.AddressList[i].OrgLicenseDtlTOList != null && tblOrganizationTO.AddressList[i].OrgLicenseDtlTOList.Count > 0)
                            {
                                TblOrgLicenseDtlTO gstLicenseTO = tblOrganizationTO.AddressList[i].OrgLicenseDtlTOList.Where(e => e.LicenseId == (int)Constants.CommercialLicenseE.IGST_NO).FirstOrDefault();
                                if (gstLicenseTO != null && !String.IsNullOrEmpty(gstLicenseTO.LicenseValue) && gstLicenseTO.LicenseValue != "0")
                                {
                                    Boolean isDuplicateLicenseValue = _iTblOrgLicenseDtlDAO.isDuplicateLicenseValue((int)Constants.CommercialLicenseE.IGST_NO, gstLicenseTO.LicenseValue, tblOrganizationTO.OrgTypeId);
                                    if (isDuplicateLicenseValue)
                                    {
                                        tran.Rollback();
                                        rMessage.DefaultBehaviour(gstLicenseTO.LicenseValue + " this GST NO is already assigned to other organization");
                                        return rMessage;
                                    }
                                }
                                TblOrgLicenseDtlTO panLicenseTO = tblOrganizationTO.AddressList[i].OrgLicenseDtlTOList.Where(e => e.LicenseId == (int)Constants.CommercialLicenseE.PAN_NO).FirstOrDefault();
                                if (panLicenseTO != null && !String.IsNullOrEmpty(panLicenseTO.LicenseValue) && panLicenseTO.LicenseValue != "0")
                                {
                                    Boolean isDuplicateLicenseValue = _iTblOrgLicenseDtlDAO.isDuplicateLicenseValue((int)Constants.CommercialLicenseE.PAN_NO, panLicenseTO.LicenseValue, tblOrganizationTO.OrgTypeId);
                                    if (isDuplicateLicenseValue)
                                    {
                                        tran.Rollback();
                                        rMessage.DefaultBehaviour(panLicenseTO.LicenseValue + " this PAN NO is already assigned to other organization");
                                        return rMessage;
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    if (tblOrganizationTO.OrgLicenseDtlTOList != null && tblOrganizationTO.OrgLicenseDtlTOList.Count > 0)
                    {
                        TblOrgLicenseDtlTO gstLicenseTO = tblOrganizationTO.OrgLicenseDtlTOList.Where(e => e.LicenseId == (int)Constants.CommercialLicenseE.IGST_NO).FirstOrDefault();
                        if (gstLicenseTO != null && !String.IsNullOrEmpty(gstLicenseTO.LicenseValue) && gstLicenseTO.LicenseValue != "0")
                        {
                            Boolean isDuplicateLicenseValue = _iTblOrgLicenseDtlDAO.isDuplicateLicenseValue((int)Constants.CommercialLicenseE.IGST_NO, gstLicenseTO.LicenseValue, tblOrganizationTO.OrgTypeId);
                            if (isDuplicateLicenseValue)
                            {
                                tran.Rollback();
                                rMessage.DefaultBehaviour(gstLicenseTO.LicenseValue + " this GST NO is already assigned to other organization");
                                return rMessage;
                            }
                        }
                        TblOrgLicenseDtlTO panLicenseTO = tblOrganizationTO.OrgLicenseDtlTOList.Where(e => e.LicenseId == (int)Constants.CommercialLicenseE.PAN_NO).FirstOrDefault();
                        if (panLicenseTO != null && !String.IsNullOrEmpty(panLicenseTO.LicenseValue) && panLicenseTO.LicenseValue != "0")
                        {
                            Boolean isDuplicateLicenseValue = _iTblOrgLicenseDtlDAO.isDuplicateLicenseValue((int)Constants.CommercialLicenseE.PAN_NO, panLicenseTO.LicenseValue, tblOrganizationTO.OrgTypeId);
                            if (isDuplicateLicenseValue)
                            {
                                tran.Rollback();
                                rMessage.DefaultBehaviour(panLicenseTO.LicenseValue + " this PAN NO is already assigned to other organization");
                                return rMessage;
                            }
                        }
                    }
                }

                #endregion
                rMessage = SaveOrganization(tblOrganizationTO, conn, tran);
                

                if (rMessage.MessageType == ResultMessageE.Error)
                {
                    tran.Rollback();
                    rMessage.DefaultBehaviour(rMessage.Text);
                    return rMessage;
                }

                //Added By Gokul [04-03-21]
                if (tblOrganizationTO.PMId>0)
                {   
                    
                    TblPurchaseManagerSupplierTO tblPurchaseManagerSupplierTO = new TblPurchaseManagerSupplierTO();
                    tblPurchaseManagerSupplierTO.CreatedOn = tblOrganizationTO.CreatedOn;
                    tblPurchaseManagerSupplierTO.CreatedBy = tblOrganizationTO.CreatedBy;
                    tblPurchaseManagerSupplierTO.IsChecked = true;
                    tblPurchaseManagerSupplierTO.IsActive = 1;
                    //Add Newly added supplier ID
                    tblPurchaseManagerSupplierTO.OrganizationId = tblOrganizationTO.IdOrganization;
                    tblPurchaseManagerSupplierTO.IsDefaultPM = 1;
                    tblPurchaseManagerSupplierTO.UserId = tblOrganizationTO.PMId;
                    int result = _iTblPurchaseManagerSupplierBL.InsertUpdateTblPurchaseManagerSupplier(tblPurchaseManagerSupplierTO,conn,tran);
                    if (result!=1)
                    {
                        tran.Rollback();
                        rMessage.DefaultBehaviour(rMessage.Text);
                        return rMessage;
                    }
                }

                tran.Commit();
                rMessage.MessageType = ResultMessageE.Information;
                rMessage.Text = "Record Saved Sucessfully";
                rMessage.Result = 1;
                rMessage.Tag = tblOrganizationTO;
                return rMessage;
            }
            catch (Exception ex)
            {
                rMessage.MessageType = ResultMessageE.Error;
                rMessage.Text = "Exception Error In Method SaveNewOrganization";
                rMessage.Tag = ex;
                return rMessage;
            }
            finally
            {
                conn.Close();
            }
        }
        //Priyanka [22-08-2019]
        public ResultMessage SaveNewOrganizationList(List<TblOrganizationTO> tblOrganizationTOList)
        {
            ResultMessage rMessage = new StaticStuff.ResultMessage();
            rMessage.MessageType = ResultMessageE.None;
            rMessage.Text = "Not Entered Into Loop";
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            string DuplicateGSTINSUpplierList = string.Empty;
            try
            {
                if (tblOrganizationTOList != null && tblOrganizationTOList.Count > 0)
                {
                    for (int i = 0; i < tblOrganizationTOList.Count; i++)
                    {
                        TblOrganizationTO tblOrganizationTO = tblOrganizationTOList[i];
                        Int32 IsLiscenseAgaintsAddress = 0;
                        Boolean isDuplicateGSTINLicenseValue = false;
                        TblConfigParamsTO tblConfigParams = _iTblConfigParamsDAO.SelectTblConfigParamsValByName(Constants.CP_LICENSE_AGAINST_ADDRESS);
                        if (tblConfigParams != null)
                        {
                            if (Convert.ToInt32(tblConfigParams.ConfigParamVal) == 1)
                            {
                                IsLiscenseAgaintsAddress = 1;
                            }
                        }
                        conn.Open();
                        tran = conn.BeginTransaction();
                        #region check Duplication for GST no
                        if (IsLiscenseAgaintsAddress == 1)
                        {
                            if (tblOrganizationTO.AddressList != null && tblOrganizationTO.AddressList.Count > 0)
                            {
                                for (int j = 0; j < tblOrganizationTO.AddressList.Count; j++)
                                {
                                    if (tblOrganizationTO.AddressList[j].OrgLicenseDtlTOList != null && tblOrganizationTO.AddressList[j].OrgLicenseDtlTOList.Count > 0)
                                    {
                                        TblOrgLicenseDtlTO gstLicenseTO = tblOrganizationTO.AddressList[j].OrgLicenseDtlTOList.Where(e => e.LicenseId == (int)Constants.CommercialLicenseE.IGST_NO).FirstOrDefault();
                                        if (gstLicenseTO != null && !String.IsNullOrEmpty(gstLicenseTO.LicenseValue) && gstLicenseTO.LicenseValue != "0")
                                        {
                                            Boolean isDuplicateLicenseValue = _iTblOrgLicenseDtlDAO.isDuplicateLicenseValue((int)Constants.CommercialLicenseE.IGST_NO, gstLicenseTO.LicenseValue, tblOrganizationTO.OrgTypeId, conn, tran);
                                            if (isDuplicateLicenseValue)
                                            {
                                                isDuplicateGSTINLicenseValue = true;
                                                DuplicateGSTINSUpplierList += tblOrganizationTO.FirmName + ","; ;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (tblOrganizationTO.OrgLicenseDtlTOList != null && tblOrganizationTO.OrgLicenseDtlTOList.Count > 0)
                            {
                                TblOrgLicenseDtlTO gstLicenseTO = tblOrganizationTO.OrgLicenseDtlTOList.Where(e => e.LicenseId == (int)Constants.CommercialLicenseE.IGST_NO).FirstOrDefault();
                                if (gstLicenseTO != null && !String.IsNullOrEmpty(gstLicenseTO.LicenseValue) && gstLicenseTO.LicenseValue != "0")
                                {
                                    Boolean isDuplicateLicenseValue = _iTblOrgLicenseDtlDAO.isDuplicateLicenseValue((int)Constants.CommercialLicenseE.IGST_NO, gstLicenseTO.LicenseValue, tblOrganizationTO.OrgTypeId, conn, tran);
                                    if (isDuplicateLicenseValue)
                                    {
                                        isDuplicateGSTINLicenseValue = true;
                                        DuplicateGSTINSUpplierList += tblOrganizationTO.FirmName + ",";
                                    }
                                }
                            }
                        }

                        if (isDuplicateGSTINLicenseValue)
                        {
                            //if (DuplicateGSTINSUpplierList != null && !string.IsNullOrEmpty(DuplicateGSTINSUpplierList))
                            //{
                            //    rMessage.DefaultBehaviour(" Following Suppliers GSTIN No are already assign to another supplier :" + DuplicateGSTINSUpplierList);
                            //}
                            conn.Close();
                            tblOrganizationTOList[i].ErrorMsg = "GSTIN No are already assign to another supplier";
                            continue;
                        }
                        #endregion
                        rMessage = SaveOrganization(tblOrganizationTO, conn, tran);
                        if (rMessage.MessageType != ResultMessageE.Information && rMessage.MessageType != ResultMessageE.None)
                        {
                            tran.Rollback();
                            conn.Close();
                            tblOrganizationTOList[i].ErrorMsg = rMessage.Text;
                            continue;
                        }
                        //else if (DuplicateGSTINSUpplierList != null && !string.IsNullOrEmpty(DuplicateGSTINSUpplierList))
                        //{
                        //    rMessage.DefaultBehaviour(" Following Suppliers GSTIN No are already assign to anoth supplier :" + DuplicateGSTINSUpplierList);
                        //}
                        tblOrganizationTOList[i].SrNo = -1;
                        tran.Commit();
                        conn.Close();
                    }
                }

                //tran.Commit();
                //if (rMessage.Result == 1)
                //{
                //    rMessage.MessageType = ResultMessageE.Information;
                //    rMessage.Text = "Record Saved Sucessfully";
                //    rMessage.Result = 1;
                //}
                // rMessage.Tag = tblOrganizationTO;
                //rMessage.DefaultSuccessBehaviour();
                if(tblOrganizationTOList.Any(x=>x.SrNo != -1))
                {
                    rMessage.DefaultBehaviour();
                }

                rMessage.data = tblOrganizationTOList;
                return rMessage;
            }
            catch (Exception ex)
            {
                rMessage.MessageType = ResultMessageE.Error;
                rMessage.Text = "Exception Error In Method SaveNewOrganization";
                rMessage.Tag = ex;
                return rMessage;
            }
            finally
            {
                conn.Close();
            }
        }
        //Priyanka [22-08-2019] : Added to save new organization.
        public ResultMessage SaveOrganization(TblOrganizationTO tblOrganizationTO, SqlConnection conn, SqlTransaction tran)
        {
            ResultMessage rMessage = new ResultMessage();
            int result = 0;
            Boolean updateOrgYn = false;
            TblPersonTO firstOwnerPersonTO = null;
            TblPersonTO secondOwnerPersonTO = null;
            try
            {
                Int32 IsLiscenseAgaintsAddress = 0;
                TblConfigParamsTO tblConfigParams = _iTblConfigParamsDAO.SelectTblConfigParamsValByName(Constants.CP_LICENSE_AGAINST_ADDRESS);
                if (tblConfigParams != null)
                {
                    if (Convert.ToInt32(tblConfigParams.ConfigParamVal) == 1)
                    {
                        IsLiscenseAgaintsAddress = 1;
                    }
                }
                if (tblOrganizationTO.TdsPct > 0)
                {
                    tblOrganizationTO.IsTcsApplicable =(int) Constants.OtherTaxTypeE.TDS;
                }
                #region 1. Create Organization First

                result = InsertTblOrganization(tblOrganizationTO, conn, tran);
                if (result != 1)
                {
                    //tran.Rollback();
                    rMessage.MessageType = ResultMessageE.Error;
                    rMessage.Text = "Error While InsertTblOrganization in Method SaveNewOrganization";
                    return rMessage;
                }

                #endregion

                #region 1.1 If OrgTypeE = Competitor Then Save its brand details

                if (tblOrganizationTO.OrgTypeE == Constants.OrgTypeE.COMPETITOR)
                {
                    if (tblOrganizationTO.CompetitorExtTOList == null || tblOrganizationTO.CompetitorExtTOList.Count == 0)
                    {
                        //tran.Rollback();
                        rMessage.MessageType = ResultMessageE.Error;
                        rMessage.Text = "Error While Competitor Brand List Found Null in Method SaveNewOrganization";
                        return rMessage;
                    }

                    for (int b = 0; b < tblOrganizationTO.CompetitorExtTOList.Count; b++)
                    {
                        tblOrganizationTO.CompetitorExtTOList[b].OrgId = tblOrganizationTO.IdOrganization;
                        tblOrganizationTO.CompetitorExtTOList[b].MfgCompanyName = tblOrganizationTO.FirmName;
                        result = _iTblCompetitorExtDAO.InsertTblCompetitorExt(tblOrganizationTO.CompetitorExtTOList[b], conn, tran);
                        if (result != 1)
                        {
                            //tran.Rollback();
                            rMessage.MessageType = ResultMessageE.Error;
                            rMessage.Text = "Error While InsertTblCompetitorExt Competitor Brand List in Method SaveNewOrganization";
                            return rMessage;
                        }
                    }
                }

                #endregion

                //  Priyanka[16 - 02 - 18] : Added to Save the Purchase Competitor Material & Grade Details.
                #region 1.2 If OrgTypeE = Purchase Competitor Then Save its Material & Grade details

                if (tblOrganizationTO.OrgTypeE == Constants.OrgTypeE.PURCHASE_COMPETITOR)
                {
                    if (tblOrganizationTO.PurchaseCompetitorExtTOList == null || tblOrganizationTO.PurchaseCompetitorExtTOList.Count == 0)
                    {
                        //tran.Rollback();
                        rMessage.MessageType = ResultMessageE.Error;
                        rMessage.Text = "Error While Purchase Competitor Material List Found Null in Method SaveNewOrganization";
                        return rMessage;
                    }

                    for (int b = 0; b < tblOrganizationTO.PurchaseCompetitorExtTOList.Count; b++)
                    {
                        tblOrganizationTO.PurchaseCompetitorExtTOList[b].OrganizationId = tblOrganizationTO.IdOrganization;
                        result = _iTblPurchaseCompetitorExtDAO.InsertTblPurchaseCompetitorExt(tblOrganizationTO.PurchaseCompetitorExtTOList[b], conn, tran);
                        if (result != 1)
                        {
                            //tran.Rollback();
                            rMessage.MessageType = ResultMessageE.Error;
                            rMessage.Text = "Error While InsertTblPurchaseCompetitorExt Competitor Material List in Method SaveNewOrganization";
                            return rMessage;
                        }
                    }
                }

                #endregion

                #region 2. Create New Persons and Update Owner Person in tblOrganization
                if (tblOrganizationTO.PersonList != null && tblOrganizationTO.PersonList.Count > 0)

                    tblOrganizationTO.PersonList = tblOrganizationTO.PersonList.Where(e => e.isActive == 1).ToList();
                if (tblOrganizationTO.PersonList != null && tblOrganizationTO.PersonList.Count > 0)
                {
                    for (int i = 0; i < tblOrganizationTO.PersonList.Count; i++)
                    {
                        TblPersonTO personTO = tblOrganizationTO.PersonList[i];
                        personTO.CreatedBy = tblOrganizationTO.CreatedBy;
                        personTO.CreatedOn = tblOrganizationTO.CreatedOn;

                        if (personTO.DobDay > 0 && personTO.DobMonth > 0 && personTO.DobYear > 0)
                        {
                            personTO.DateOfBirth = new DateTime(personTO.DobYear, personTO.DobMonth, personTO.DobDay);
                        }
                        else if (personTO.DateOfBirth != null && personTO.DateOfBirth != new DateTime())
                        {

                            personTO.DateOfBirth = personTO.DateOfBirth.AddDays(1);
                        }
                        else
                        {
                            personTO.DateOfBirth = DateTime.MinValue;
                        }
                        //if (personTO.PersonTypeId == (int)Constants.VisitPersonE.FIRST_OWNER)
                        if (personTO.IsDefault1 == 1)
                        {
                            personTO.Comments = "First Owner - " + tblOrganizationTO.FirmName;
                            tblOrganizationTO.FirstOwnerPersonId = personTO.IdPerson;
                            tblOrganizationTO.FirstOwnerName = personTO.FirstName + " " + personTO.LastName;
                            firstOwnerPersonTO = personTO;
                            updateOrgYn = true;
                        }
                        //else if (personTO.PersonTypeId == (int)Constants.VisitPersonE.SECOND_OWNER)
                        else if(personTO.IsDefault2 == 1)
                        {
                            personTO.Comments = "Second Owner - " + tblOrganizationTO.FirmName;
                            tblOrganizationTO.SecondOwnerPersonId = personTO.IdPerson;
                            tblOrganizationTO.SecondOwnerName = personTO.FirstName + " " + personTO.LastName;
                            updateOrgYn = true;
                            secondOwnerPersonTO = personTO;
                        }

                        result = _iTblPersonDAO.InsertTblPerson(personTO, conn, tran);
                        if (result != 1)
                        {
                            //tran.Rollback();
                            rMessage.MessageType = ResultMessageE.Error;
                            rMessage.Text = "Error While InsertTblPerson in Method SaveNewOrganization ";
                            return rMessage;
                        }
                        TblOrgPersonDtlsTO orgPersonTO = new TblOrgPersonDtlsTO();
                        orgPersonTO.IsActive = 1;
                        orgPersonTO.OrganizationId = tblOrganizationTO.IdOrganization;
                        orgPersonTO.PersonTypeId = personTO.PersonTypeId;
                        orgPersonTO.CreatedBy = tblOrganizationTO.CreatedBy;
                        orgPersonTO.CreatedOn = tblOrganizationTO.CreatedOn;
                        orgPersonTO.PersonId = personTO.IdPerson;
                        result = _iTblOrgPersonDtlsBL.InsertTblOrgPersonDtls(orgPersonTO, conn, tran);
                        if (result != 1)
                        {
                            //tran.Rollback();
                            rMessage.MessageType = ResultMessageE.Error;
                            rMessage.Text = "Error While InsertTblPerson in Method SaveNewOrganization ";
                            return rMessage;
                        }

                        //if (personTO.PersonTypeId == (int)Constants.VisitPersonE.FIRST_OWNER)
                        if (personTO.IsDefault1 == 1)
                        {
                            tblOrganizationTO.FirstOwnerPersonId = personTO.IdPerson;
                            tblOrganizationTO.FirstOwnerName = personTO.FirstName + " " + personTO.LastName;
                            firstOwnerPersonTO = personTO;
                            updateOrgYn = true;
                        }
                        //else if (personTO.SeqNo == (int)Constants.VisitPersonE.SECOND_OWNER)
                        else if (personTO.IsDefault2 == 1)
                        {
                            tblOrganizationTO.SecondOwnerPersonId = personTO.IdPerson;
                            tblOrganizationTO.SecondOwnerName = personTO.FirstName + " " + personTO.LastName;
                            updateOrgYn = true;
                        }

                    }

                }

                #endregion

                #region 3. Add Address Details

                List<TblOrgAddressTO> tblOrgAddressTOList = new List<Models.TblOrgAddressTO>();
                tblOrganizationTO.AddressList = tblOrganizationTO.AddressList.Where(e => e.IsActive == 1).ToList();

                if (tblOrganizationTO.AddressList != null && tblOrganizationTO.AddressList.Count > 0)
                {
                    for (int i = 0; i < tblOrganizationTO.AddressList.Count; i++)
                    {
                        TblAddressTO addressTO = tblOrganizationTO.AddressList[i];
                        addressTO.CreatedBy = tblOrganizationTO.CreatedBy;
                        addressTO.CreatedOn = tblOrganizationTO.CreatedOn;
                        if (addressTO.CountryId == 0)
                            addressTO.CountryId = Constants.DefaultCountryID;
                        //if (addressTO.StateId == 0)
                        //    addressTO.StateId = Constants.defaultstateID;

                        //if (addressTO.DistrictId == 0)
                        //    addressTO.DistrictId = Constants.defaultDistrictID;
                        //if (addressTO.Pincode == null)
                        //    addressTO.Pincode = Constants.defaultPinCode;

                        if (addressTO.DistrictId == 0 && !string.IsNullOrEmpty(addressTO.DistrictName))
                        {
                            //Insert New Taluka
                            CommonDimensionsTO districtDimensionTO = new CommonDimensionsTO();
                            districtDimensionTO.ParentId = addressTO.StateId;
                            districtDimensionTO.DimensionName = addressTO.DistrictName;
                            List<DropDownTO> districtList = _iDimensionDAO.SelectDistrictByName(addressTO.DistrictName, addressTO.StateId, conn, tran);
                            if (districtList != null && districtList.Count > 0)
                            {
                                addressTO.DistrictId = districtList[0].Value;
                            }
                            else
                            {
                                if (districtDimensionTO.ParentId > 0)
                                {
                                    result = _iDimensionDAO.InsertDistrict(districtDimensionTO, conn, tran);
                                    if (result != 1)
                                    {
                                        //  tran.Rollback();
                                        rMessage.MessageType = ResultMessageE.Error;
                                        rMessage.Text = "Error While InsertDistrict in Method SaveNewOrganization";
                                        return rMessage;
                                    }
                                    addressTO.DistrictId = districtDimensionTO.IdDimension;
                                }
                            }
                        }

                        if (addressTO.TalukaId == 0 && !string.IsNullOrEmpty(addressTO.TalukaName))
                        {
                            //Insert New Taluka
                            CommonDimensionsTO talukaDimensionTO = new CommonDimensionsTO();
                            talukaDimensionTO.ParentId = addressTO.DistrictId;
                            talukaDimensionTO.DimensionName = addressTO.TalukaName;
                            List<DropDownTO> talukaList = _iDimensionDAO.SelectTalukaByName(addressTO.TalukaName, addressTO.DistrictId, conn, tran);
                            if (talukaList != null && talukaList.Count > 0)
                            {
                                addressTO.TalukaId = talukaList[0].Value;
                            }
                            else
                            {
                                if (talukaDimensionTO.ParentId > 0)
                                {
                                    result = _iDimensionDAO.InsertTaluka(talukaDimensionTO, conn, tran);
                                    if (result != 1)
                                    {
                                        // tran.Rollback();
                                        rMessage.MessageType = ResultMessageE.Error;
                                        rMessage.Text = "Error While InsertTaluka in Method SaveNewOrganization";
                                        return rMessage;
                                    }
                                    addressTO.TalukaId = talukaDimensionTO.IdDimension;
                                }
                            }
                        }

                        result = _iTblAddressDAO.InsertTblAddress(addressTO, conn, tran);
                        if (result != 1)
                        {
                            // tran.Rollback();
                            rMessage.MessageType = ResultMessageE.Error;
                            rMessage.Text = "Error While InsertTblAddress in Method SaveNewOrganization";
                            return rMessage;
                        }

                        TblOrgAddressTO tblOrgAddressTO = addressTO.GetTblOrgAddressTO();
                        tblOrgAddressTO.OrganizationId = tblOrganizationTO.IdOrganization;
                        //Priyanka [23-09-2019]: Added to set the address Type.
                        tblOrgAddressTO.AddrTypeId = addressTO.AddrTypeId;
                        result = _iTblOrgAddressDAO.InsertTblOrgAddress(tblOrgAddressTO, conn, tran);
                        if (result != 1)
                        {
                            // tran.Rollback();
                            rMessage.MessageType = ResultMessageE.Error;
                            rMessage.Text = "Error While InsertTblOrgAddress in Method SaveNewOrganization";
                            return rMessage;
                        }

                        if (addressTO.IsDefault == 1)
                        {
                            updateOrgYn = true;
                            tblOrganizationTO.AddrId = addressTO.IdAddr;
                        }
                        if (IsLiscenseAgaintsAddress == 1)
                        {
                            if (addressTO.OrgLicenseDtlTOList != null && addressTO.OrgLicenseDtlTOList.Count > 0)
                            {
                                for (int ol = 0; ol < addressTO.OrgLicenseDtlTOList.Count; ol++)
                                {
                                    if (addressTO.OrgLicenseDtlTOList[ol].LicenseValue != null && !string.IsNullOrEmpty(addressTO.OrgLicenseDtlTOList[ol].LicenseValue))
                                    {
                                        if (addressTO.OrgLicenseDtlTOList[ol].LicenseValue != "0")
                                        {
                                            addressTO.OrgLicenseDtlTOList[ol].OrganizationId = tblOrganizationTO.IdOrganization;
                                            addressTO.OrgLicenseDtlTOList[ol].CreatedBy = tblOrganizationTO.CreatedBy;
                                            addressTO.OrgLicenseDtlTOList[ol].CreatedOn = tblOrganizationTO.CreatedOn;
                                            addressTO.OrgLicenseDtlTOList[ol].AddressId = addressTO.IdAddr;
                                            result = _iTblOrgLicenseDtlDAO.InsertTblOrgLicenseDtl(addressTO.OrgLicenseDtlTOList[ol], conn, tran);
                                            if (result != 1)
                                            {
                                                // tran.Rollback();
                                                rMessage.MessageType = ResultMessageE.Error;
                                                rMessage.Text = "Error While InsertTblOrgLicenseDtl for Users in Method SaveNewOrganization ";
                                                return rMessage;
                                            }

                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                #endregion

                #region 4. Save Organization Commercial Licences
                if (tblOrganizationTO.OrgLicenseDtlTOList != null && tblOrganizationTO.OrgLicenseDtlTOList.Count > 0)
                {
                    if (IsLiscenseAgaintsAddress == 0)
                    {
                        for (int ol = 0; ol < tblOrganizationTO.OrgLicenseDtlTOList.Count; ol++)
                        {
                            if (tblOrganizationTO.OrgLicenseDtlTOList[ol].LicenseValue != null && !string.IsNullOrEmpty(tblOrganizationTO.OrgLicenseDtlTOList[ol].LicenseValue))
                            {
                                if (tblOrganizationTO.OrgLicenseDtlTOList[ol].LicenseValue != "0")
                                {
                                    tblOrganizationTO.OrgLicenseDtlTOList[ol].OrganizationId = tblOrganizationTO.IdOrganization;
                                    tblOrganizationTO.OrgLicenseDtlTOList[ol].CreatedBy = tblOrganizationTO.CreatedBy;
                                    tblOrganizationTO.OrgLicenseDtlTOList[ol].CreatedOn = tblOrganizationTO.CreatedOn;
                                    tblOrganizationTO.OrgLicenseDtlTOList[ol].AddressId = tblOrganizationTO.AddrId;
                                    result = _iTblOrgLicenseDtlDAO.InsertTblOrgLicenseDtl(tblOrganizationTO.OrgLicenseDtlTOList[ol], conn, tran);
                                    if (result != 1)
                                    {
                                        // tran.Rollback();
                                        rMessage.MessageType = ResultMessageE.Error;
                                        rMessage.Text = "Error While InsertTblOrgLicenseDtl for Users in Method SaveNewOrganization ";
                                        return rMessage;
                                    }

                                }
                            }
                        }
                    }
                }
                #endregion

                #region Save KYC details

                if (tblOrganizationTO.KYCDetailsTO != null)
                {

                    tblOrganizationTO.KYCDetailsTO.OrganizationId = tblOrganizationTO.IdOrganization;
                    tblOrganizationTO.KYCDetailsTO.CreatedBy = tblOrganizationTO.CreatedBy;
                    tblOrganizationTO.KYCDetailsTO.CreatedOn = tblOrganizationTO.CreatedOn;
                    tblOrganizationTO.KYCDetailsTO.IsActive = 1;

                    result = _iTblKYCDetailsBL.InsertTblKYCDetails(tblOrganizationTO.KYCDetailsTO, conn, tran);
                    if (result != 1)
                    {
                        // tran.Rollback();
                        rMessage.MessageType = ResultMessageE.Error;
                        rMessage.Text = "Error While InsertTblKYCDetails for Users in Method SaveNewOrganization ";
                        return rMessage;
                    }

                }
                #endregion

                #region Save Bank Details for Organization
                if (tblOrganizationTO.BankDetailsList != null && tblOrganizationTO.BankDetailsList.Count > 0)
                {
                    for (int i = 0; i < tblOrganizationTO.BankDetailsList.Count; i++)
                    {
                        TblOrgBankDetailsTO bankDetailsTO = tblOrganizationTO.BankDetailsList[i];
                        bankDetailsTO.CreatedBy = tblOrganizationTO.CreatedBy;
                        bankDetailsTO.CreatedOn = tblOrganizationTO.CreatedOn;
                        bankDetailsTO.BankOrgId = bankDetailsTO.BankOrgId;
                        bankDetailsTO.OrgId = tblOrganizationTO.IdOrganization;

                        //bankDetailsTO.IsDefault = 1;

                        result = _iTblOrgBankDetailsDAO.InsertTblOrgBankDetails(bankDetailsTO, conn, tran);
                        if (result != 1)
                        {
                            // tran.Rollback();
                            rMessage.MessageType = ResultMessageE.Error;
                            rMessage.Text = "Error While InsertTblOrgBankDetails for Users in Method SaveNewOrganization ";
                            return rMessage;
                        }
                    }
                }
                #endregion

                #region Save Accounting Details
                if (tblOrganizationTO.TblOrgAccountTaxTO != null)
                {
                    //if (tblOrganizationTO.TblOrgAccountTaxTO.SubjectToHoldingTax)
                    {
                        tblOrganizationTO.TblOrgAccountTaxTO.OrganizationId = tblOrganizationTO.IdOrganization;
                        result = _itblOrgAccountTaxDAO.InsertTblOrgAccountTax(tblOrganizationTO.TblOrgAccountTaxTO, conn, tran);
                        if (result != 1)
                        {
                            // tran.Rollback();
                            rMessage.MessageType = ResultMessageE.Error;
                            rMessage.Text = "Error While InsertTblOrgAccountTax for Users in Method SaveNewOrganization ";
                            return rMessage;
                        }

                        else
                        {
                            if (tblOrganizationTO.TblOrgAccountTaxTO.TblOrgAccountTaxDtlsList != null && tblOrganizationTO.TblOrgAccountTaxTO.TblOrgAccountTaxDtlsList.Count > 0)
                            {
                                tblOrganizationTO.TblOrgAccountTaxTO.TblOrgAccountTaxDtlsList.ForEach(res =>
                                {
                                    res.OrgAccountTaxId = tblOrganizationTO.TblOrgAccountTaxTO.IdOrgAccountTax;
                                });
                                for (int i = 0; i < tblOrganizationTO.TblOrgAccountTaxTO.TblOrgAccountTaxDtlsList.Count; i++)
                                {
                                    if (tblOrganizationTO.TblOrgAccountTaxTO.TblOrgAccountTaxDtlsList[i].IsActive == true)
                                    {
                                        result = _iTblOrgAccountTaxDtlsDAO.InsertTblOrgAccountTaxDtls(tblOrganizationTO.TblOrgAccountTaxTO.TblOrgAccountTaxDtlsList[i], conn, tran);
                                        if (result != 1)
                                        {
                                            // tran.Rollback();
                                            rMessage.MessageType = ResultMessageE.Error;
                                            rMessage.Text = "Error While InsertTblOrgAccountTaxDtls for Users in Method SaveNewOrganization ";
                                            return rMessage;
                                        }
                                    }
                                        
                                }
                            }
                        }
                    }

                }
                #endregion

                #region 5. Is Address Or Concern Person Found then Update in tblOrganization

                if (updateOrgYn)
                {
                    tblOrganizationTO.UpdatedBy = tblOrganizationTO.CreatedBy;
                    tblOrganizationTO.UpdatedOn = tblOrganizationTO.CreatedOn;
                    result = UpdateTblOrganization(tblOrganizationTO, conn, tran);
                    if (result != 1)
                    {
                        //  tran.Rollback();
                        rMessage.MessageType = ResultMessageE.Error;
                        rMessage.Text = "Error While UpdateTblOrganization for Persons in Method SaveNewOrganization ";
                        return rMessage;
                    }
                }
                #endregion

                #region If Dealer then Manage Cnf & Dealer Relationship

                if (tblOrganizationTO.OrgTypeE == Constants.OrgTypeE.DEALER)
                {
                    if (tblOrganizationTO.ParentId > 0)
                    {
                        TblCnfDealersTO tblCnfDealersTO = new TblCnfDealersTO();
                        tblCnfDealersTO.CnfOrgId = tblOrganizationTO.ParentId;
                        tblCnfDealersTO.DealerOrgId = tblOrganizationTO.IdOrganization;
                        tblCnfDealersTO.CreatedBy = tblOrganizationTO.CreatedBy;
                        tblCnfDealersTO.CreatedOn = tblOrganizationTO.CreatedOn;
                        tblCnfDealersTO.IsActive = 1;
                        tblCnfDealersTO.Remark = "Primary C&F";
                        result = _iTblCnfDealersBL.InsertTblCnfDealers(tblCnfDealersTO, conn, tran);
                        if (result != 1)
                        {
                            // tran.Rollback();
                            rMessage.MessageType = ResultMessageE.Error;
                            rMessage.Text = "Error While InsertTblCnfDealers for CnfDealer in Method SaveNewOrganization ";
                            return rMessage;
                        }

                        if (tblOrganizationTO.CnfDealersTOList != null)
                        {
                            for (int c = 0; c < tblOrganizationTO.CnfDealersTOList.Count; c++)
                            {
                                tblOrganizationTO.CnfDealersTOList[c].DealerOrgId = tblOrganizationTO.IdOrganization;
                                tblOrganizationTO.CnfDealersTOList[c].CreatedBy = tblOrganizationTO.CreatedBy;
                                tblOrganizationTO.CnfDealersTOList[c].CreatedOn = tblOrganizationTO.CreatedOn;
                                tblOrganizationTO.CnfDealersTOList[c].IsActive = 1;
                                tblOrganizationTO.CnfDealersTOList[c].IsSpecialCnf = 1;
                                tblOrganizationTO.CnfDealersTOList[c].Remark = "Special C&F";
                                result = _iTblCnfDealersBL.InsertTblCnfDealers(tblOrganizationTO.CnfDealersTOList[c], conn, tran);
                                if (result != 1)
                                {
                                    //   tran.Rollback();
                                    rMessage.MessageType = ResultMessageE.Error;
                                    rMessage.Text = "Error While InsertTblCnfDealers for Special C&F in Method SaveNewOrganization ";
                                    return rMessage;
                                }
                            }
                        }
                    }
                }

                #endregion

                #region 7. User Creation Based On Orgtype settings defined in the masters

                DimOrgTypeTO orgTypeTO = _iDimOrgTypeDAO.SelectDimOrgType(tblOrganizationTO.OrgTypeId, conn, tran);
                if (orgTypeTO != null && orgTypeTO.CreateUserYn == 1 && orgTypeTO.DefaultRoleId > 0)
                {
                    TblUserRoleTO tblUserRoleTO = _iTblUserRoleBL.SelectTblUserRoleTO(orgTypeTO.DefaultRoleId);
                    if (tblUserRoleTO == null)
                    {
                        List<TblUserRoleTO> tblUserRoleTOList = _iTblUserRoleBL.SelectAllTblUserRoleList();
                        if (tblUserRoleTOList != null && tblUserRoleTOList.Count>0)
                        {
                            orgTypeTO.DefaultRoleId = tblUserRoleTOList[0].RoleId;
                        }
                    }
                    if (tblOrganizationTO.PersonList != null && tblOrganizationTO.PersonList.Count > 0)
                    {
                        for (int i = 0; i < tblOrganizationTO.PersonList.Count; i++)
                        {
                            TblPersonTO personInfo = tblOrganizationTO.PersonList[i];
                            if (personInfo.IsUserCreate == 1)
                            {
                                rMessage = CreateNewOrganizationUser(tblOrganizationTO, personInfo, orgTypeTO, tblOrganizationTO.CreatedBy, tblOrganizationTO.CreatedOn, conn, tran);

                                if (rMessage.MessageType != ResultMessageE.Information)
                                {
                                    //tran.Rollback();
                                    rMessage.DefaultBehaviour(rMessage.Text);
                                    return rMessage;
                                }
                            }

                        }
                    }
                    //if (tblOrganizationTO.FirstOwnerPersonId > 0)
                    //{
                    //    rMessage = CreateNewOrganizationUser(tblOrganizationTO, firstOwnerPersonTO, orgTypeTO, tblOrganizationTO.CreatedBy, tblOrganizationTO.CreatedOn, conn, tran);
                    //    if (rMessage.MessageType != ResultMessageE.Information)
                    //    {
                    //      //  tran.Rollback();
                    //        rMessage.DefaultBehaviour(rMessage.Text);
                    //        return rMessage;
                    //    }
                    //}

                    //if (tblOrganizationTO.SecondOwnerPersonId > 0)
                    //{
                    //    rMessage = CreateNewOrganizationUser(tblOrganizationTO, secondOwnerPersonTO, orgTypeTO, tblOrganizationTO.CreatedBy, tblOrganizationTO.CreatedOn, conn, tran);
                    //    if (rMessage.MessageType != ResultMessageE.Information)
                    //    {
                    //     //   tran.Rollback();
                    //        rMessage.DefaultBehaviour("Error While Org User Creation");
                    //        return rMessage;
                    //    }
                    //}
                }
                //else
                //{
                //    //   tran.Rollback();
                //    rMessage.DefaultSuccessBehaviour();
                //    return rMessage;

                //    // Do Nothing. allow to save the record.
                //}
                #endregion

                #region SAP Integration

                TblConfigParamsTO tblConfigParamsTOSAPService = _iTblConfigParamsDAO.SelectTblConfigParams(Constants.SAPB1_SERVICES_ENABLE);
                if (tblConfigParamsTOSAPService != null)
                {
                    if (Convert.ToInt32(tblConfigParamsTOSAPService.ConfigParamVal) == 1)
                    {
                        //if (tblOrganizationTO.OrgTypeE == Constants.OrgTypeE.SUPPLIER || tblOrganizationTO.OrgTypeE == Constants.OrgTypeE.DEALER
                        //    || tblOrganizationTO.OrgTypeE == Constants.OrgTypeE.BANK || tblOrganizationTO.OrgTypeE == Constants.OrgTypeE.TRANSPOTER)

                            if (orgTypeTO != null && orgTypeTO.IsTransferToSAP == 1)
                            {
                            ResultMessage sapResult = SaveMasterToSAP(tblOrganizationTO);
                            if (sapResult.Result != 1)
                            {
                                //tran.Rollback();
                                rMessage.DefaultBehaviour(sapResult.Text);
                                return rMessage;
                            }


                            if (tblOrganizationTO.OrgTypeE == Constants.OrgTypeE.BANK)
                            {
                                result = _iTblOrganizationDAO.UpdateTxnIdtoTblOrg(tblOrganizationTO, conn, tran);

                                if (result != 1)
                                {
                                    rMessage.MessageType = ResultMessageE.Error;
                                    rMessage.Text = "Error while updating Bank in SAP ";
                                    return rMessage;
                                }
                            }

                        }
                    }
                }
                #endregion
                rMessage.DefaultSuccessBehaviour("Record Saved Successfully");
                return rMessage;
            }
            catch (Exception ex)
            {
                rMessage.DefaultExceptionBehaviour(ex, "Exception Error In Method SaveNewOrganization");
                return rMessage;
            }
        }
        //Priyanka [16-04-2019]
        public ResultMessage SaveOrganizationRefIds(TblOrganizationTO organizationTO, string loginUserId)
        {
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            resultMessage.MessageType = ResultMessageE.None;
            resultMessage.Text = "Not Entered Into Loop";
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            int result = 0;
            try
            {
                if (!String.IsNullOrEmpty(organizationTO.OverdueRefId))
                {
                    List<TblOrganizationTO> tblOrganizationTOList = SelectExistingAllTblOrganizationByRefIds(organizationTO.IdOrganization, organizationTO.OverdueRefId, null);
                    if (tblOrganizationTOList != null && tblOrganizationTOList.Count > 0)
                    {
                        Int32 overdueisExist = tblOrganizationTOList.Count;
                        if (overdueisExist > 0)
                        {
                            String orgName = String.Join(",", tblOrganizationTOList.Select(s => s.FirmName.ToString()).ToList());
                            resultMessage.MessageType = ResultMessageE.Information;
                            resultMessage.Result = 2;
                            resultMessage.Text = "Overdue Reference Id is already assign to " + orgName;
                            return resultMessage;
                        }
                    }

                    if (organizationTO.EnqRefId == null)
                    {
                        TblOrganizationTO tblOrganizationTOOld = SelectTblOrganizationTO(organizationTO.IdOrganization);
                        if (tblOrganizationTOOld != null)
                        {
                            organizationTO.EnqRefId = tblOrganizationTOOld.EnqRefId;
                        }
                    }
                }
                if (!String.IsNullOrEmpty(organizationTO.EnqRefId))
                {
                    List<TblOrganizationTO> tblOrganizationTOList = SelectExistingAllTblOrganizationByRefIds(organizationTO.IdOrganization, null, organizationTO.EnqRefId);
                    if (tblOrganizationTOList != null && tblOrganizationTOList.Count > 0)
                    {
                        Int32 enqyisExist = tblOrganizationTOList.Count;
                        if (enqyisExist > 0)
                        {
                            String orgName = String.Join(",", tblOrganizationTOList.Select(s => s.FirmName.ToString()).ToList());
                            resultMessage.MessageType = ResultMessageE.Information;
                            resultMessage.Result = 2;
                            resultMessage.Text = "Enquiry Reference Id is already assign to " + orgName;
                            return resultMessage;
                        }
                    }

                    if (organizationTO.OverdueRefId == null)
                    {
                        TblOrganizationTO tblOrganizationTOOld = SelectTblOrganizationTO(organizationTO.IdOrganization);
                        if (tblOrganizationTOOld != null)
                        {
                            organizationTO.OverdueRefId = tblOrganizationTOOld.OverdueRefId;
                        }
                    }
                }
                DateTime serverDate = _iCommon.ServerDateTime;
                organizationTO.UpdatedBy = Convert.ToInt32(loginUserId);
                organizationTO.UpdatedOn = serverDate;

                result = UpdateTblOrganizationRefIds(organizationTO);
                if (result != 1)
                {
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Result = 0;
                    resultMessage.Text = "Error while updating Ref Ids";
                    return resultMessage;
                }
                else
                {
                    resultMessage.MessageType = ResultMessageE.Information;
                    resultMessage.Result = 1;
                    resultMessage.Text = "Update Successfully";
                    return resultMessage;
                }

            }
            catch (Exception ex)
            {
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Text = "Exception Error In Method SaveNewOrganization";
                resultMessage.Tag = ex;
                return resultMessage;
            }
            finally
            {
                conn.Close();
            }
        }
        /// <summary>
        /// Priyanka [19-08-2019]
        /// </summary>
        /// <returns></returns>
        public ResultMessage UpdateAndCreateLoginToSupplliers(ref string invalidIdStr)
        {
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            Boolean isSAPEnable = false;
            ResultMessage resultMessage = new ResultMessage();
            invalidIdStr = null;
            try
            {

                TblConfigParamsTO tblConfigParamsTOSAPService = _iTblConfigParamsDAO.SelectTblConfigParams(Constants.SAPB1_SERVICES_ENABLE);
                if (tblConfigParamsTOSAPService != null)
                {
                    if (Convert.ToInt32(tblConfigParamsTOSAPService.ConfigParamVal) == 1)
                    {
                        isSAPEnable = true;
                    }
                }
                List<TblOrganizationTO> supplierList = SelectAllTblOrganizationList(Constants.OrgTypeE.SUPPLIER,0);
                if (supplierList != null && supplierList.Count > 0)
                {
                    DimOrgTypeTO orgTypeTO = _iDimOrgTypeDAO.SelectAllDimOrgType((Int32)Constants.OrgTypeE.SUPPLIER);
                    for (int i = 0; i < supplierList.Count; i++)
                    {
                        conn.Open();
                        tran = conn.BeginTransaction();
                        TblOrganizationTO tblOrganizationTO = supplierList[i];
                        if (tblOrganizationTO.FirstOwnerPersonId > 0)
                        {
                            TblPersonTO firstOwnerPersonTO = _iTblPersonDAO.SelectTblPerson(tblOrganizationTO.FirstOwnerPersonId, conn, tran);
                            resultMessage = CreateNewOrganizationUser(tblOrganizationTO, firstOwnerPersonTO, orgTypeTO, tblOrganizationTO.CreatedBy, tblOrganizationTO.CreatedOn, conn, tran);
                            if (resultMessage.MessageType != ResultMessageE.Information)
                            {
                                invalidIdStr += tblOrganizationTO.IdOrganization + "- " + resultMessage.Text + ", ";
                                //  tran.Rollback();
                                //  resultMessage.DefaultBehaviour("Error While Org User Creation");
                                //  return resultMessage;
                            }

                            #region SAP Integration
                            if (isSAPEnable)
                            {
                                tblOrganizationTO.PersonList = new List<TblPersonTO>();
                                tblOrganizationTO.PersonList.Add(firstOwnerPersonTO);


                                tblOrganizationTO.AddressList = new List<TblAddressTO>();
                                TblAddressTO tblAddressTO = _iTblAddressDAO.SelectTblAddress(tblOrganizationTO.AddrId);
                                if (tblAddressTO != null)
                                {
                                    tblOrganizationTO.AddressList.Add(tblAddressTO);
                                }

                                List<TblOrgLicenseDtlTO> orgLicenseDtlTOList = _iTblOrgLicenseDtlDAO.SelectAllTblOrgLicenseDtl(tblOrganizationTO.IdOrganization, conn, tran);
                                if (orgLicenseDtlTOList != null && orgLicenseDtlTOList.Count > 0)
                                {
                                    tblOrganizationTO.OrgLicenseDtlTOList = orgLicenseDtlTOList;
                                }

                                ResultMessage sapResult = SaveMasterToSAP(tblOrganizationTO);
                                if (sapResult.Result != 1)
                                {
                                    invalidIdStr += tblOrganizationTO.IdOrganization + "- " + sapResult.Text + ",";
                                    //tran.Rollback();
                                    // resultMessage.DefaultBehaviour("Error While Org User Creation SaveMasterToSAP");
                                    //  return resultMessage;
                                }
                            }
                            #endregion
                        }

                        if (tblOrganizationTO.SecondOwnerPersonId > 0)
                        {
                            TblPersonTO secondOwnerPersonTO = _iTblPersonDAO.SelectTblPerson(tblOrganizationTO.SecondOwnerPersonId, conn, tran);
                            resultMessage = CreateNewOrganizationUser(tblOrganizationTO, secondOwnerPersonTO, orgTypeTO, tblOrganizationTO.CreatedBy, tblOrganizationTO.CreatedOn, conn, tran);
                            if (resultMessage.MessageType != ResultMessageE.Information)
                            {
                                invalidIdStr = ',' + invalidIdStr + tblOrganizationTO.IdOrganization + "- Error While Org User Creation";
                                //tran.Rollback();
                                //resultMessage.DefaultBehaviour("Error While Org User Creation");
                                //return resultMessage;
                            }

                            #region SAP Integration
                            if (isSAPEnable)
                            {
                                tblOrganizationTO.PersonList = new List<TblPersonTO>();
                                tblOrganizationTO.PersonList.Add(secondOwnerPersonTO);
                                tblOrganizationTO.AddressList = new List<TblAddressTO>();
                                TblAddressTO tblAddressTO = _iTblAddressDAO.SelectTblAddress(tblOrganizationTO.AddrId, conn, tran);
                                if (tblAddressTO != null)
                                {
                                    tblOrganizationTO.AddressList.Add(tblAddressTO);
                                }

                                List<TblOrgLicenseDtlTO> orgLicenseDtlTOList = _iTblOrgLicenseDtlDAO.SelectAllTblOrgLicenseDtl(tblOrganizationTO.IdOrganization, conn, tran);
                                if (orgLicenseDtlTOList != null && orgLicenseDtlTOList.Count > 0)
                                {
                                    tblOrganizationTO.OrgLicenseDtlTOList = orgLicenseDtlTOList;
                                }

                                ResultMessage sapResult = SaveMasterToSAP(tblOrganizationTO);
                                if (sapResult.Result != 1)
                                {
                                    invalidIdStr = ',' + invalidIdStr + tblOrganizationTO.IdOrganization + sapResult.Text;
                                    //tran.Rollback();
                                    // resultMessage.DefaultBehaviour("Error While Org User Creation SaveMasterToSAP");
                                    // return resultMessage;
                                }
                            }
                            #endregion
                        }

                        tran.Commit();
                        conn.Close();
                    }
                }

                return resultMessage;
            }
            catch (Exception ex)
            {
                // tran.Rollback();
                //resultMessage.MessageType = ResultMessageE.Error;
                //resultMessage.Text = "Exception Error In Method UpdateAndCreateLoginToSupplliers";
                //resultMessage.Tag = ex;
                return resultMessage;
            }
            finally
            {
                conn.Close();
            }
        }

        private ResultMessage SaveMasterToSAP(TblOrganizationTO organizationTO)
        {
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                Int32 IsLiscenseAgaintsAddress = 0;
                TblConfigParamsTO tblConfigParams = _iTblConfigParamsDAO.SelectTblConfigParamsValByName(Constants.CP_LICENSE_AGAINST_ADDRESS);
                if (tblConfigParams != null)
                {
                    if (Convert.ToInt32(tblConfigParams.ConfigParamVal) == 1)
                    {
                        IsLiscenseAgaintsAddress = 1;
                    }
                }
                List<DropDownTO> AddressTyleList = _iDimensionDAO.SelectAddressTypeListForDropDown();
                if (AddressTyleList == null || AddressTyleList.Count == 0)
                {
                    resultMessage.DefaultBehaviour("Failed to get address type list");
                    return resultMessage;
                }
                List<DropDownTO> currencyList = _iDimensionDAO.SelectCurrencyForDropDown();
                if (currencyList == null || currencyList.Count == 0)
                {
                    resultMessage.DefaultBehaviour("Failed to get currencyList ");
                    return resultMessage;
                }
                List<DropDownTO> orgGroupTypeList = new List<DropDownTO>();
                if (organizationTO.OrgTypeId > 0)
                {
                    orgGroupTypeList = _iDimensionDAO.SelectDimOrgGroupType(organizationTO.OrgTypeId);
                    if (orgGroupTypeList == null || orgGroupTypeList.Count == 0)
                    {
                        resultMessage.DefaultBehaviour("Failed to get orgGroupTypeList ");
                        return resultMessage;
                    }
                }
               
                if (Startup.CompanyObject == null)
                {
                    resultMessage.DefaultBehaviour("SAP Company Object Found NULL");
                    return resultMessage;
                }

                //Sanjay [11-Nov-2019] If Master Entry is bank then call for save banks to SAP Master
                if (organizationTO.OrgTypeE == Constants.OrgTypeE.BANK)
                    return SaveBankToSAP(organizationTO);

                SAPbobsCOM.BusinessPartners oBusinesspartner;

                oBusinesspartner = Startup.CompanyObject.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oBusinessPartners);
                #region General Information

                oBusinesspartner.CardCode = organizationTO.IdOrganization.ToString();
                oBusinesspartner.CardName = organizationTO.FirmName;
                oBusinesspartner.CardForeignName = organizationTO.FirmName;
                if (organizationTO.OrgTypeE == Constants.OrgTypeE.SUPPLIER)
                    oBusinesspartner.CardType = SAPbobsCOM.BoCardTypes.cSupplier;
                else if (organizationTO.OrgTypeE == Constants.OrgTypeE.DEALER)
                    oBusinesspartner.CardType = SAPbobsCOM.BoCardTypes.cCustomer;
                else if (organizationTO.OrgTypeE == Constants.OrgTypeE.TRANSPOTER)//Reshma For Transporter Migrtion
                {
                    oBusinesspartner.CardType = SAPbobsCOM.BoCardTypes.cSupplier;
                    oBusinesspartner.GroupCode = 103;
                }

                oBusinesspartner.Cellular = organizationTO.RegisteredMobileNos;

                oBusinesspartner.Notes = organizationTO.Remark;
                oBusinesspartner.FreeText = organizationTO.Remark;
                oBusinesspartner.AliasName = organizationTO.FirmName;
                oBusinesspartner.EmailAddress = organizationTO.EmailAddr;
                oBusinesspartner.Phone1 = organizationTO.PhoneNo;
                oBusinesspartner.Fax = organizationTO.FaxNo;
                oBusinesspartner.Website = organizationTO.Website;
                oBusinesspartner.SubjectToWithholdingTax = SAPbobsCOM.BoYesNoEnum.tNO;//Reshma[07-08-2020] For avoding PAN License in SAP.

                if(orgGroupTypeList != null && orgGroupTypeList.Count > 0)
                {
                    //Added By Harshala to save Group Code in SAP for  Vendor
                    var orgGrpTypeTO = orgGroupTypeList.Where(x => x.Value == organizationTO.OrgGroupTypeId).FirstOrDefault();
                    if (orgGrpTypeTO != null)
                        oBusinesspartner.GroupCode = Convert.ToInt32(orgGrpTypeTO.MappedTxnId);
                }

                var currencyTO = currencyList.Where(x => x.Value == organizationTO.CurrencyId).FirstOrDefault();
                if (currencyTO != null)
                    oBusinesspartner.Currency = currencyTO.Code;

                #endregion

                organizationTO.AddressList = organizationTO.AddressList.Where(w => w.IsDefault == 1).ToList();
                //List < TblAddressTO > a= organizationTO.AddressList.Where(f => f.AddrTypeId == (int)Constants.AddressTypeE.OFFICE_ADDRESS).ToList();
                #region Address Details
                for (int al = 0; al < organizationTO.AddressList.Count; al++)
                {
                    //oBusinesspartner.Addresses.SetCurrentLine(al);
                    oBusinesspartner.Addresses.AddressType = SAPbobsCOM.BoAddressType.bo_BillTo;
                    var MatchingTO = AddressTyleList.Where(x => x.Value == organizationTO.AddressList[al].AddrTypeId).FirstOrDefault();
                    if (MatchingTO != null)
                        oBusinesspartner.Addresses.AddressName = MatchingTO.Text;
                    else
                        oBusinesspartner.Addresses.AddressName = "Office Adress";
                    oBusinesspartner.Addresses.Block = organizationTO.AddressList[al].PlotNo;
                    oBusinesspartner.Addresses.Street = organizationTO.AddressList[al].StreetName;
                    oBusinesspartner.Addresses.StreetNo = organizationTO.AddressList[al].StreetName;

                    oBusinesspartner.Addresses.Country = organizationTO.AddressList[al].CountryCode;
                    oBusinesspartner.Addresses.State = organizationTO.AddressList[al].StateCode;
                    oBusinesspartner.Addresses.City = organizationTO.AddressList[al].DistrictName;
                    oBusinesspartner.Addresses.AddressName2 = organizationTO.AddressList[al].AreaName;
                    oBusinesspartner.Addresses.AddressName3 = organizationTO.AddressList[al].VillageName;
                    oBusinesspartner.Addresses.ZipCode = organizationTO.AddressList[al].Pincode.ToString();

                    //string gstinno = string.Empty;
                    //string panNo = string.Empty;
                    //Int32 gstTypeId = 0;
                    List<TblOrgLicenseDtlTO> TblOrgLicenseDtlTOList = new List<TblOrgLicenseDtlTO>();
                    if (IsLiscenseAgaintsAddress == 1)
                    {
                        TblOrgLicenseDtlTOList = organizationTO.AddressList[al].OrgLicenseDtlTOList;
                    }
                    else
                    {
                        TblOrgLicenseDtlTOList = organizationTO.OrgLicenseDtlTOList;
                    }
                    if (TblOrgLicenseDtlTOList != null && TblOrgLicenseDtlTOList.Count > 0)
                    {
                        TblOrgLicenseDtlTO gstinnoTO = TblOrgLicenseDtlTOList.Where(x => x.LicenseId == (int)Constants.CommercialLicenseE.IGST_NO).FirstOrDefault();
                        if (gstinnoTO != null && gstinnoTO.LicenseValue != "0")
                        {

                            oBusinesspartner.Addresses.GSTIN = gstinnoTO.LicenseValue;
                            if (gstinnoTO.GstTypeId == 0)
                            {
                                gstinnoTO.GstTypeId = (Int32)SAPbobsCOM.BoGSTRegnTypeEnum.gstRegularTDSISD;
                            }
                            oBusinesspartner.Addresses.GstType = (SAPbobsCOM.BoGSTRegnTypeEnum)gstinnoTO.GstTypeId;
                            //  oBusinesspartner.Addresses.GstType = SAPbobsCOM.BoGSTRegnTypeEnum.gstRegularTDSISD;   //commented by harshala to make it dynamic

                        }

                        TblOrgLicenseDtlTO panNoTO = TblOrgLicenseDtlTOList.Where(x => x.LicenseId == (int)Constants.CommercialLicenseE.PAN_NO).FirstOrDefault();
                        if (panNoTO != null && panNoTO.LicenseValue!= "0")
                        {
                            //panNo = panNoTO.LicenseValue;   //commented by harshala for code optimization
                            oBusinesspartner.FiscalTaxID.SetCurrentLine(0);
                            oBusinesspartner.FiscalTaxID.TaxId0 = panNoTO.LicenseValue;
                            oBusinesspartner.FiscalTaxID.Add();
                        }
                    }


                    //commented by harshala for code optimization

                    //if (panNo != "0" && !string.IsNullOrEmpty(panNo))
                    //{
                    //    oBusinesspartner.FiscalTaxID.SetCurrentLine(0);
                    //    oBusinesspartner.FiscalTaxID.TaxId0 = panNo;
                    //    oBusinesspartner.FiscalTaxID.Add();
                    //}
                    oBusinesspartner.Addresses.Add();
                    oBusinesspartner.Addresses.AddressType = SAPbobsCOM.BoAddressType.bo_ShipTo;
                    var MatchingTOShipTO = AddressTyleList.Where(x => x.Value == organizationTO.AddressList[al].AddrTypeId).FirstOrDefault();
                    if (MatchingTOShipTO != null)
                        oBusinesspartner.Addresses.AddressName = MatchingTOShipTO.Text;
                    else
                        oBusinesspartner.Addresses.AddressName = "Office Adress";
                    oBusinesspartner.Addresses.Block = organizationTO.AddressList[al].PlotNo;
                    oBusinesspartner.Addresses.Street = organizationTO.AddressList[al].StreetName;
                    oBusinesspartner.Addresses.StreetNo = organizationTO.AddressList[al].StreetName;


                    oBusinesspartner.Addresses.Country = organizationTO.AddressList[al].CountryCode;
                    oBusinesspartner.Addresses.State = organizationTO.AddressList[al].StateCode;
                    oBusinesspartner.Addresses.City = organizationTO.AddressList[al].DistrictName;
                    oBusinesspartner.Addresses.AddressName2 = organizationTO.AddressList[al].AreaName;
                    oBusinesspartner.Addresses.AddressName3 = organizationTO.AddressList[al].VillageName;
                    oBusinesspartner.Addresses.ZipCode = organizationTO.AddressList[al].Pincode.ToString();

                    // gstinno = string.Empty;
                    //panNo = string.Empty;
                    TblOrgLicenseDtlTOList = new List<TblOrgLicenseDtlTO>();
                    if (IsLiscenseAgaintsAddress == 1)
                    {
                        TblOrgLicenseDtlTOList = organizationTO.AddressList[al].OrgLicenseDtlTOList;
                    }
                    else
                    {
                        TblOrgLicenseDtlTOList = organizationTO.OrgLicenseDtlTOList;
                    }
                    if (TblOrgLicenseDtlTOList != null && TblOrgLicenseDtlTOList.Count > 0)
                    {
                        //gstinno = organizationTO.OrgLicenseDtlTOList.Where(x => x.LicenseId == (int)Constants.CommercialLicenseE.IGST_NO).FirstOrDefault().LicenseValue;
                        //panNo = organizationTO.OrgLicenseDtlTOList.Where(x => x.LicenseId == (int)Constants.CommercialLicenseE.PAN_NO).FirstOrDefault().LicenseValue;
                        TblOrgLicenseDtlTO gstinnoTO = TblOrgLicenseDtlTOList.Where(x => x.LicenseId == (int)Constants.CommercialLicenseE.IGST_NO).FirstOrDefault();
                        if (gstinnoTO != null && gstinnoTO.LicenseValue !="0")
                        {
                            //gstinno = gstinnoTO.LicenseValue;   //commented by harshala for code optimization
                            oBusinesspartner.Addresses.GSTIN = gstinnoTO.LicenseValue;
                            if (gstinnoTO.GstTypeId == 0)
                            {
                                gstinnoTO.GstTypeId = (Int32)SAPbobsCOM.BoGSTRegnTypeEnum.gstRegularTDSISD;
                            }
                            oBusinesspartner.Addresses.GstType = (SAPbobsCOM.BoGSTRegnTypeEnum)gstinnoTO.GstTypeId;
                        }

                        TblOrgLicenseDtlTO panNoTO = TblOrgLicenseDtlTOList.Where(x => x.LicenseId == (int)Constants.CommercialLicenseE.PAN_NO).FirstOrDefault();
                        if (panNoTO != null && panNoTO.LicenseValue != "0")
                        {
                            //panNo = panNoTO.LicenseValue;   //commented by harshala for code optimization
                            oBusinesspartner.FiscalTaxID.SetCurrentLine(0);
                            oBusinesspartner.FiscalTaxID.TaxId0 = panNoTO.LicenseValue;
                            oBusinesspartner.FiscalTaxID.Add();

                        }
                    }
                    //commented by harshala for code optimization
                    //if (gstinno != "0" && !string.IsNullOrEmpty(gstinno))
                    //{
                    //    oBusinesspartner.Addresses.GSTIN = gstinno;
                    //    oBusinesspartner.Addresses.GstType = SAPbobsCOM.BoGSTRegnTypeEnum.gstRegularTDSISD;
                    //}

                    //if (panNo != "0" && !string.IsNullOrEmpty(panNo))
                    //{
                    //    oBusinesspartner.FiscalTaxID.SetCurrentLine(0);
                    //    oBusinesspartner.FiscalTaxID.TaxId0 = panNo;
                    //    oBusinesspartner.FiscalTaxID.Add();
                    //}
                    if ((organizationTO.AddressList.Count - 1) != al)
                        oBusinesspartner.Addresses.Add();
                }
                #endregion

                #region Contact Person Details
                if (organizationTO.PersonList != null && organizationTO.PersonList.Count > 0)
                {
                    for (int cp = 0; cp < organizationTO.PersonList.Count; cp++)
                    {
                        oBusinesspartner.ContactEmployees.SetCurrentLine(cp);
                        oBusinesspartner.ContactEmployees.FirstName = organizationTO.PersonList[cp].FirstName;
                        oBusinesspartner.ContactEmployees.MiddleName = organizationTO.PersonList[cp].MidName;
                        oBusinesspartner.ContactEmployees.LastName = organizationTO.PersonList[cp].LastName;
                        oBusinesspartner.ContactEmployees.MobilePhone = organizationTO.PersonList[cp].MobileNo;
                        oBusinesspartner.ContactEmployees.E_Mail = organizationTO.PersonList[cp].PrimaryEmail;
                        oBusinesspartner.ContactEmployees.Name = organizationTO.PersonList[cp].FirstName + " " + organizationTO.PersonList[cp].LastName;
                        if (organizationTO.PersonList[cp].SalutationId == (int)Constants.SalutationE.MR)
                            oBusinesspartner.ContactEmployees.Gender = SAPbobsCOM.BoGenderTypes.gt_Male;
                        else if (organizationTO.PersonList[cp].SalutationId == (int)Constants.SalutationE.MRS || organizationTO.PersonList[cp].SalutationId == (int)Constants.SalutationE.MISS)
                            oBusinesspartner.ContactEmployees.Gender = SAPbobsCOM.BoGenderTypes.gt_Female;

                        oBusinesspartner.ContactEmployees.DateOfBirth = organizationTO.PersonList[cp].DateOfBirth;

                        oBusinesspartner.ContactEmployees.Add();
                    }

                }

                #endregion
                //Added By Harshala[29/09/2020]
                #region Accounting Details  

                if(organizationTO.TblOrgAccountTaxTO != null)
                {

                    if (!String.IsNullOrEmpty(organizationTO.TblOrgAccountTaxTO.AccountPayableCode))
                    {
                        oBusinesspartner.DebitorAccount = organizationTO.TblOrgAccountTaxTO.AccountPayableCode;
                        oBusinesspartner.DownPaymentInterimAccount = organizationTO.TblOrgAccountTaxTO.InterimAccountCode; // Account Code
                        oBusinesspartner.DownPaymentClearAct = organizationTO.TblOrgAccountTaxTO.ClearingAccountCode; // Account Code
                    }
                    else
                    {
                        if (orgGroupTypeList[0].Tag != null)
                        {
                            oBusinesspartner.DebitorAccount = Convert.ToString(orgGroupTypeList[0].Tag).ToString();
                        }
                    }
                    if (organizationTO.TblOrgAccountTaxTO.SubjectToHoldingTax)
                    {
                        oBusinesspartner.SubjectToWithholdingTax = BoYesNoEnum.tYES;

                        if (organizationTO.TblOrgAccountTaxTO.ThreasholdOverlook)
                            oBusinesspartner.ThresholdOverlook = BoYesNoEnum.tYES;

                        if (organizationTO.TblOrgAccountTaxTO.SurchargeOverlook)
                            oBusinesspartner.SurchargeOverlook = BoYesNoEnum.tYES;

                        if (organizationTO.TblOrgAccountTaxTO.Accrual)
                            oBusinesspartner.AccrualCriteria = BoYesNoEnum.tYES; // Yes for Accural and No for Cash

                        if (organizationTO.TblOrgAccountTaxTO.Cash)
                            oBusinesspartner.AccrualCriteria = BoYesNoEnum.tNO;// Yes for Accural and No for Cash

                        if (organizationTO.TblOrgAccountTaxTO.TblOrgAccountTaxDtlsList != null && organizationTO.TblOrgAccountTaxTO.TblOrgAccountTaxDtlsList.Count > 0)
                        {
                            for (int i = 0; i < organizationTO.TblOrgAccountTaxTO.TblOrgAccountTaxDtlsList.Count; i++)
                            {
                                oBusinesspartner.BPWithholdingTax.WTCode = organizationTO.TblOrgAccountTaxTO.TblOrgAccountTaxDtlsList[i].WithholdTaxId.ToString();
                                oBusinesspartner.BPWithholdingTax.Add();
                            }
                        }

                        oBusinesspartner.ExpirationDate = organizationTO.TblOrgAccountTaxTO.CertificateExpiryDate;
                        oBusinesspartner.CertificateNumber = organizationTO.TblOrgAccountTaxTO.CerificateNo;
                        oBusinesspartner.NationalInsuranceNum = organizationTO.TblOrgAccountTaxTO.NINumber;

                        oBusinesspartner.TypeReport = (SAPbobsCOM.AssesseeTypeEnum)Convert.ToInt16(organizationTO.TblOrgAccountTaxTO.MappedTxnId);// Assesse type
                    }
                }
                else//Reshma[15-03-21]
                {
                    if(orgGroupTypeList[0].Tag !=null )
                    {
                        oBusinesspartner.DebitorAccount = Convert.ToString(orgGroupTypeList[0].Tag).ToString();
                    }
                }


                #endregion

                int result = oBusinesspartner.Add();
                if (result == 0)
                {
                    resultMessage.DefaultSuccessBehaviour();
                    return resultMessage;
                }
                else
                {
                    string errorinfor = Startup.CompanyObject.GetLastErrorDescription();
                    resultMessage.DefaultBehaviour(errorinfor);
                    return resultMessage;
                }

            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "SaveMasterToSAP");
                return resultMessage;
            }
        }

        /// <summary>
        /// Sanjay [11-Nov-2019] To Save Bank in SAP bank Master
        /// </summary>
        /// <param name="organizationTO"></param>
        /// <returns></returns>
        private ResultMessage SaveBankToSAP(TblOrganizationTO organizationTO)
        {
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                SAPbobsCOM.Banks oBanks;
                oBanks = Startup.CompanyObject.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oBanks);

                oBanks.BankCode = organizationTO.IdOrganization.ToString();
                oBanks.BankName = organizationTO.FirmName;
                oBanks.CountryCode = organizationTO.AddressList[0].CountryCode;
                //oBanks.SwiftNo = organizationTO.SwiftCode;

                int result = oBanks.Add();
                if (result == 0)
                {
                    organizationTO.TxnId = Startup.CompanyObject.GetNewObjectKey();
                    resultMessage.DefaultSuccessBehaviour();

                    return resultMessage;
                }
                else
                {
                    string errorinfor = Startup.CompanyObject.GetLastErrorDescription();
                    resultMessage.DefaultBehaviour(errorinfor);
                    return resultMessage;
                }
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "SaveBankToSAP");
                return resultMessage;
            }
        }

        /// <summary>
        /// Harshala [11-Dec-2019] To Update Bank in SAP bank Master
        /// </summary>
        /// <param name="organizationTO"></param>
        /// <returns></returns>
        private ResultMessage UpdateBankToSAP(TblOrganizationTO organizationTO)
        {
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                SAPbobsCOM.Banks oBanks;
                oBanks = Startup.CompanyObject.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oBanks);
                oBanks.GetByKey(Convert.ToInt32(organizationTO.TxnId));
                // oBanks.BankCode = organizationTO.IdOrganization.ToString();
                oBanks.BankName = organizationTO.FirmName;
                oBanks.CountryCode = organizationTO.AddressList[0].CountryCode;
                //oBanks./*S*/wiftNo = organizationTO.SwiftCode;

                int result = oBanks.Update();
                if (result == 0)
                {
                    resultMessage.DefaultSuccessBehaviour();
                    return resultMessage;
                }
                else
                {
                    string errorinfor = Startup.CompanyObject.GetLastErrorDescription();
                    resultMessage.DefaultBehaviour(errorinfor);
                    return resultMessage;
                }
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "SaveBankToSAP");
                return resultMessage;
            }
        }


        private ResultMessage CreateNewOrganizationUser(TblOrganizationTO tblOrganizationTO, TblPersonTO personTO, DimOrgTypeTO orgTypeTO, Int32 createdBy, DateTime createdOn, SqlConnection conn, SqlTransaction tran)
        {

            ResultMessage rMessage = new ResultMessage();
            int result;
            String userId = _iTblUserBL.CreateUserName(personTO.FirstName, personTO.LastName, conn, tran);
            String pwd = Constants.DefaultPassword;

            TblUserTO userTO = new Models.TblUserTO();
            userTO.UserLogin = userId;
            userTO.UserPasswd = pwd;
            userTO.UserDisplayName = personTO.FirstName + " " + personTO.LastName;
            userTO.IsActive = 1;
            userTO.UserTypeId = personTO.UserTypeId;
            result = _iTblUserBL.InsertTblUser(userTO, conn, tran);

            if (result != 1)
            {
                //tran.Rollback();
                rMessage.MessageType = ResultMessageE.Error;
                rMessage.Text = "Error While InsertTblUser for Users in Method SaveNewOrganization ";
                return rMessage;
            }

            TblUserExtTO tblUserExtTO = new TblUserExtTO();
            if (tblOrganizationTO.AddressList != null && tblOrganizationTO.AddressList.Count > 0)
                tblUserExtTO.AddressId = tblOrganizationTO.AddressList[0].IdAddr;
            if (tblOrganizationTO.AddrId > 0)
                tblUserExtTO.AddressId = tblOrganizationTO.AddrId;
            tblUserExtTO.CreatedBy = createdBy;
            tblUserExtTO.CreatedOn = createdOn;
            if (personTO.IdPerson > 0)
                tblUserExtTO.PersonId = personTO.IdPerson;
            //if (tblOrganizationTO.FirstOwnerPersonId > 0)
            //tblUserExtTO.PersonId = tblOrganizationTO.FirstOwnerPersonId;
            tblUserExtTO.OrganizationId = tblOrganizationTO.IdOrganization;
            tblUserExtTO.UserId = userTO.IdUser;
            tblUserExtTO.Comments = "New " + orgTypeTO.OrgType + " User Created";

            result = _iTblUserExtDAO.InsertTblUserExt(tblUserExtTO, conn, tran);
            if (result != 1)
            {
                //tran.Rollback();
                rMessage.MessageType = ResultMessageE.Error;
                rMessage.Text = "Error While InsertTblUserExt for Users in Method SaveNewOrganization ";
                return rMessage;
            }
            TblUserRoleTO tblUserRoleTO = new TblUserRoleTO();
            tblUserRoleTO.UserId = userTO.IdUser;
            tblUserRoleTO.RoleId = orgTypeTO.DefaultRoleId;
            tblUserRoleTO.IsActive = 1;
            tblUserRoleTO.CreatedBy = createdBy;
            tblUserRoleTO.CreatedOn = createdOn;
            result = _iTblUserRoleBL.InsertTblUserRole(tblUserRoleTO, conn, tran);
            if (result != 1)
            {
                //tran.Rollback();
                rMessage.MessageType = ResultMessageE.Error;
                rMessage.Text = "Error While InsertTblUserRole for orgType=" + orgTypeTO.OrgType + " in Method SaveNewOrganization ";
                return rMessage;
            }

            rMessage.DefaultSuccessBehaviour();
            return rMessage;
        }

        public ResultMessage PostUpdateOverdueExistOrNot(TblOrganizationTO organizationTo, Int32 loginUserId)
        {
            ResultMessage resultMessage = new ResultMessage();
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            int result = 0;
            DateTime serverDate = _iCommon.ServerDateTime;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();

                #region 1. Update table TblOrganization.

                result = UpdateTblOrganization(organizationTo, conn, tran);
                if (result != 1)
                {
                    tran.Rollback();
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Text = "Error While Update TblOrganization in Method UpdateTblOrganization";
                    return resultMessage;
                }
                #endregion

                #region 2. insert into table TblOrgOverdueHistory.

                TblOrgOverdueHistoryTO tblOrgOverdueHistoryTO = new TblOrgOverdueHistoryTO();
                tblOrgOverdueHistoryTO.CreatedBy = loginUserId;
                tblOrgOverdueHistoryTO.CreatedOn = serverDate;
                tblOrgOverdueHistoryTO.OrganizationId = organizationTo.IdOrganization;
                tblOrgOverdueHistoryTO.IsOverdueExist = organizationTo.IsOverdueExist;
                result = _iTblOrgOverdueHistoryDAO.InsertTblOrgOverdueHistory(tblOrgOverdueHistoryTO, conn, tran);
                if (result != 1)
                {
                    tran.Rollback();
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Text = "Error While Update InsertTblOrgOverdueHistory ";
                    return resultMessage;
                }
                #endregion


                tran.Commit();
                resultMessage.MessageType = ResultMessageE.Information;
                resultMessage.Text = "Record Saved Sucessfully";
                resultMessage.Result = 1;
                resultMessage.Tag = organizationTo;
                return resultMessage;

            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "PostUpdateOverdueExistOrNot");
                return resultMessage;
            }
            finally
            {
                conn.Close();

            }
        }


        #endregion

        #region Updation
        public int UpdateTblOrganization(TblOrganizationTO tblOrganizationTO)
        {
            return _iTblOrganizationDAO.UpdateTblOrganization(tblOrganizationTO);
        }

        public int UpdateTblOrganizationRefIds(TblOrganizationTO tblOrganizationTO)
        {
            return _iTblOrganizationDAO.UpdateTblOrganizationRefIds(tblOrganizationTO);
        }

        public int UpdateTblOrganization(TblOrganizationTO tblOrganizationTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblOrganizationDAO.UpdateTblOrganization(tblOrganizationTO, conn, tran);
        }

        public ResultMessage UpdateOrganization(TblOrganizationTO tblOrganizationTO)
        {
            ResultMessage rMessage = new StaticStuff.ResultMessage();
            rMessage.MessageType = ResultMessageE.None;
            rMessage.Text = "Not Entered Into Loop";
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            int result = 0;
            Boolean updateOrgYn = false;
            TblPersonTO firstOwnerPersonTO = null;
            TblPersonTO secondOwnerPersonTO = null;
            conn.Open();
            tran = conn.BeginTransaction();
            try
            {
                //Check multiple spaces in Supplier name
                tblOrganizationTO.FirmName = StaticStuff.Constants.removeUnwantedSpaces(tblOrganizationTO.FirmName);
                #region Check Duplicate Supplier Name
                Int32 IsAllowDuplicateFirmName = 0;
                TblConfigParamsTO AllowDupFirmNameTO = _iTblConfigParamsDAO.SelectTblConfigParamsValByName(Constants.IS_ALLOW_DUPLICATE_ORGANISATION_FIRM_NAME);
                if (AllowDupFirmNameTO != null)
                {
                    if (!String.IsNullOrEmpty(AllowDupFirmNameTO.ConfigParamVal))
                    {
                        if (Convert.ToInt32(AllowDupFirmNameTO.ConfigParamVal) == 1)
                        {
                            IsAllowDuplicateFirmName = Convert.ToInt32(AllowDupFirmNameTO.ConfigParamVal);
                        }
                    }
                }
                if (tblOrganizationTO.FirmName != null && IsAllowDuplicateFirmName == 0)
                {
                    rMessage = _iTblOrganizationDAO.SelectFirmName(tblOrganizationTO,true, conn, tran);
                    if (rMessage.Result == 0)
                    {
                        tran.Rollback();
                        rMessage.DefaultBehaviour("Organization name already exist with Id = " + rMessage.Text + ". Organization Name should be Unique");
                        return rMessage;
                    }
                }
                #endregion

                //#region Check Duplication For Mobile numbers
                ////For Org Mobile Number
                //if (tblOrganizationTO.RegisteredMobileNos != null && !String.IsNullOrEmpty(tblOrganizationTO.RegisteredMobileNos))
                //{

                //    Boolean isDuplicateMobileNo = _iTblOrgBankDetailsDAO.isDuplicateMobileNo(tblOrganizationTO.RegisteredMobileNos, (int)Constants.MobileDupChecktypeE.OrganizationRegMobNo,tblOrganizationTO.IdOrganization);
                //    if (isDuplicateMobileNo)
                //    {
                //       // tran.Rollback();
                //        rMessage.DefaultBehaviour("Registration Mobile No is already assigned to other organization");
                //        return rMessage;

                //    }

                //}

                ////For Contact Details
                //if (tblOrganizationTO.PhoneNo != null && !String.IsNullOrEmpty(tblOrganizationTO.PhoneNo))
                //{

                //    Boolean isDuplicateMobileNo = _iTblOrgBankDetailsDAO.isDuplicateMobileNo(tblOrganizationTO.PhoneNo, (int)Constants.MobileDupChecktypeE.ContactDetailsMobNo,tblOrganizationTO.IdOrganization);
                //    if (isDuplicateMobileNo)
                //    {
                //       // tran.Rollback();
                //        rMessage.DefaultBehaviour("Phone No of Contact Details is already assigned to other organization");
                //        return rMessage;

                //    }

                //}

                ////For Person Mobile Number
                //if (tblOrganizationTO.PersonList != null && tblOrganizationTO.PersonList.Count > 0)
                //{
                //    for (int j = 0; j < tblOrganizationTO.PersonList.Count; j++)
                //    {
                //        TblPersonTO personTo = tblOrganizationTO.PersonList[j];
                //        Boolean isDuplicateMobileNo = _iTblOrgBankDetailsDAO.isDuplicateMobileNo(personTo.MobileNo, (int)Constants.MobileDupChecktypeE.PersonMobNo,tblOrganizationTO.IdOrganization);
                //        if (isDuplicateMobileNo)
                //        {
                //            //tran.Rollback();
                //            rMessage.DefaultBehaviour("Mobile No of Owner " + personTo.FirstName + " " + personTo.LastName + " is already assigned to other Person ");
                //            return rMessage;

                //        }
                //    }
                //}

                ////For Person Alternate mobile number
                //if (tblOrganizationTO.PersonList != null && tblOrganizationTO.PersonList.Count > 0)
                //{
                //    for (int j = 0; j < tblOrganizationTO.PersonList.Count; j++)
                //    {
                //        TblPersonTO personTo = tblOrganizationTO.PersonList[j];
                //        Boolean isDuplicateMobileNo = _iTblOrgBankDetailsDAO.isDuplicateMobileNo(personTo.AlternateMobNo, (int)Constants.MobileDupChecktypeE.AltPersonMobNo,tblOrganizationTO.IdOrganization);
                //        if (isDuplicateMobileNo)
                //        {
                //            tran.Rollback();
                //            rMessage.DefaultBehaviour(" Alternate Mobile No of " + personTo.FirstName + " " + personTo.LastName + " is already assigned to other Person ");
                //            return rMessage;
                //        }
                //    }
                //}

                //#endregion
                Int32 IsLiscenseAgaintsAddress = 0;
                TblConfigParamsTO tblConfigParams = _iTblConfigParamsDAO.SelectTblConfigParamsValByName(Constants.CP_LICENSE_AGAINST_ADDRESS);
                if (tblConfigParams != null)
                {
                    if (Convert.ToInt32(tblConfigParams.ConfigParamVal) == 1)
                    {
                        IsLiscenseAgaintsAddress = 1;
                    }
                }
                #region check Duplication for GST no
                if (IsLiscenseAgaintsAddress == 1)
                {
                    if (tblOrganizationTO.AddressList != null && tblOrganizationTO.AddressList.Count > 0)
                    {
                        for (int i = 0; i < tblOrganizationTO.AddressList.Count; i++)
                        {
                            if (tblOrganizationTO.AddressList[i].OrgLicenseDtlTOList != null && tblOrganizationTO.AddressList[i].OrgLicenseDtlTOList.Count > 0)
                            {
                                TblOrgLicenseDtlTO gstLicenseTO = tblOrganizationTO.AddressList[i].OrgLicenseDtlTOList.Where(e => e.LicenseId == (int)Constants.CommercialLicenseE.IGST_NO).FirstOrDefault();
                                if (gstLicenseTO != null && !String.IsNullOrEmpty(gstLicenseTO.LicenseValue) && gstLicenseTO.LicenseValue != "0")
                                {
                                    Boolean isDuplicateLicenseValue = _iTblOrgLicenseDtlDAO.isDuplicateLicenseValue((int)Constants.CommercialLicenseE.IGST_NO, gstLicenseTO.LicenseValue, tblOrganizationTO.OrgTypeId, tblOrganizationTO.IdOrganization);
                                    if (isDuplicateLicenseValue)
                                    {
                                        rMessage.DefaultBehaviour(gstLicenseTO.LicenseValue + " this GST NO is already assigned to other organization");
                                        return rMessage;
                                    }
                                }
                                TblOrgLicenseDtlTO panLicenseTO = tblOrganizationTO.AddressList[i].OrgLicenseDtlTOList.Where(e => e.LicenseId == (int)Constants.CommercialLicenseE.PAN_NO).FirstOrDefault();
                                if (panLicenseTO != null && !String.IsNullOrEmpty(panLicenseTO.LicenseValue) && panLicenseTO.LicenseValue != "0")
                                {
                                    Boolean isDuplicateLicenseValue = _iTblOrgLicenseDtlDAO.isDuplicateLicenseValue((int)Constants.CommercialLicenseE.PAN_NO, panLicenseTO.LicenseValue, tblOrganizationTO.OrgTypeId, tblOrganizationTO.IdOrganization);
                                    if (isDuplicateLicenseValue)
                                    {
                                        rMessage.DefaultBehaviour(panLicenseTO.LicenseValue + " this PAN NO is already assigned to other organization");
                                        return rMessage;
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    if (tblOrganizationTO.OrgLicenseDtlTOList != null && tblOrganizationTO.OrgLicenseDtlTOList.Count > 0)
                    {
                        TblOrgLicenseDtlTO gstLicenseTO = tblOrganizationTO.OrgLicenseDtlTOList.Where(e => e.LicenseId == (int)Constants.CommercialLicenseE.IGST_NO).FirstOrDefault();
                        if (gstLicenseTO != null && !String.IsNullOrEmpty(gstLicenseTO.LicenseValue) && gstLicenseTO.LicenseValue != "0")
                        {

                            Boolean isDuplicateLicenseValue = _iTblOrgLicenseDtlDAO.isDuplicateLicenseValue((int)Constants.CommercialLicenseE.IGST_NO, gstLicenseTO.LicenseValue, tblOrganizationTO.OrgTypeId, tblOrganizationTO.IdOrganization);
                            if (isDuplicateLicenseValue)
                            {
                                rMessage.DefaultBehaviour(gstLicenseTO.LicenseValue + " this GST NO is already assigned to other organization");
                                return rMessage;
                            }
                        }
                        TblOrgLicenseDtlTO panLicenseTO = tblOrganizationTO.OrgLicenseDtlTOList.Where(e => e.LicenseId == (int)Constants.CommercialLicenseE.PAN_NO).FirstOrDefault();
                        if (panLicenseTO != null && !String.IsNullOrEmpty(panLicenseTO.LicenseValue) && panLicenseTO.LicenseValue != "0")
                        {

                            Boolean isDuplicateLicenseValue = _iTblOrgLicenseDtlDAO.isDuplicateLicenseValue((int)Constants.CommercialLicenseE.PAN_NO, panLicenseTO.LicenseValue, tblOrganizationTO.OrgTypeId, tblOrganizationTO.IdOrganization);
                            if (isDuplicateLicenseValue)
                            {
                                rMessage.DefaultBehaviour(panLicenseTO.LicenseValue + " this PAN NO is already assigned to other organization");
                                return rMessage;
                            }
                        }
                    }
                }


                #endregion
                //conn.Open();
                //tran = conn.BeginTransaction();
                //Saket [2018-02-21] Added to create user while updating the C&F
                Boolean isNewFirstOwner = false;
                Boolean isNewSecondOwner = false;
                if (tblOrganizationTO.FirstOwnerPersonId == 0)
                {
                    isNewFirstOwner = true;
                }
                if (tblOrganizationTO.SecondOwnerPersonId == 0)
                {
                    isNewSecondOwner = true;
                }


                #region 1. Create Organization First

                result = UpdateTblOrganization(tblOrganizationTO, conn, tran);
                if (result != 1)
                {
                    tran.Rollback();
                    rMessage.MessageType = ResultMessageE.Error;
                    rMessage.Text = "Error While UpdateTblOrganization in Method UpdateOrganization";
                    return rMessage;
                }

                #endregion

                #region 1.1 If OrgType=Competitor then update the list
                List<TblCompetitorExtTO> list = _iTblCompetitorExtDAO.SelectAllTblCompetitorExt(tblOrganizationTO.IdOrganization, conn, tran);
                if (tblOrganizationTO.CompetitorExtTOList != null)
                {
                    for (int b = 0; b < tblOrganizationTO.CompetitorExtTOList.Count; b++)
                    {
                        TblCompetitorExtTO existTO = null;

                        if (list != null)
                            existTO = list.Where(l => l.IdCompetitorExt == tblOrganizationTO.CompetitorExtTOList[b].IdCompetitorExt).FirstOrDefault();

                        if (existTO == null)
                        {
                            //Insert New Brand
                            tblOrganizationTO.CompetitorExtTOList[b].OrgId = tblOrganizationTO.IdOrganization;
                            result = _iTblCompetitorExtDAO.InsertTblCompetitorExt(tblOrganizationTO.CompetitorExtTOList[b], conn, tran);
                            if (result != 1)
                            {
                                tran.Rollback();
                                rMessage.MessageType = ResultMessageE.Error;
                                rMessage.Text = "Error While InsertTblCompetitorExt in Method UpdateTblOrganization";
                                return rMessage;
                            }
                        }
                        else
                        {
                            //Update existing brand
                            result = _iTblCompetitorExtDAO.UpdateTblCompetitorExt(tblOrganizationTO.CompetitorExtTOList[b], conn, tran);
                            if (result != 1)
                            {
                                tran.Rollback();
                                rMessage.MessageType = ResultMessageE.Error;
                                rMessage.Text = "Error While UpdateTblCompetitorExt in Method UpdateTblOrganization";
                                return rMessage;
                            }
                        }
                    }
                }

                #endregion

                // Priyanka[16 - 02 - 18] : Added to save the Purchase Competitor Material & Grade Details.
                #region 1.2 If OrgType=Purchase Competitor then update the Material & Grade Details

                List<TblPurchaseCompetitorExtTO> tblPurchaseCompetitorExtTOList = _iTblPurchaseCompetitorExtDAO.SelectAllTblPurchaseCompetitorExt(tblOrganizationTO.IdOrganization, conn, tran);
                if (tblOrganizationTO.PurchaseCompetitorExtTOList != null)
                {
                    for (int b = 0; b < tblOrganizationTO.PurchaseCompetitorExtTOList.Count; b++)
                    {
                        TblPurchaseCompetitorExtTO purchaseCompetitorexistTO = null;

                        if (tblPurchaseCompetitorExtTOList != null)
                            purchaseCompetitorexistTO = tblPurchaseCompetitorExtTOList.Where(l => l.IdPurCompetitorExt == tblOrganizationTO.PurchaseCompetitorExtTOList[b].IdPurCompetitorExt).FirstOrDefault();

                        if (purchaseCompetitorexistTO == null)
                        {
                            //Insert New Material & Grade
                            tblOrganizationTO.PurchaseCompetitorExtTOList[b].OrganizationId = tblOrganizationTO.IdOrganization;
                            result = _iTblPurchaseCompetitorExtDAO.InsertTblPurchaseCompetitorExt(tblOrganizationTO.PurchaseCompetitorExtTOList[b], conn, tran);
                            if (result != 1)
                            {
                                tran.Rollback();
                                rMessage.MessageType = ResultMessageE.Error;
                                rMessage.Text = "Error While InsertTblPurchaseCompetitorExt in Method UpdateTblOrganization";
                                return rMessage;
                            }
                        }
                        else
                        {
                            //Update Material & Grade
                            result = _iTblPurchaseCompetitorExtDAO.UpdateTblPurchaseCompetitorExt(tblOrganizationTO.PurchaseCompetitorExtTOList[b], conn, tran);
                            if (result != 1)
                            {
                                tran.Rollback();
                                rMessage.MessageType = ResultMessageE.Error;
                                rMessage.Text = "Error While UpdateTblPurchaseCompetitorExt in Method UpdateTblOrganization";
                                return rMessage;
                            }
                        }
                    }
                }

                #endregion

                #region 2. Create New Persons and Update Owner Person in tblOrganization

                if (tblOrganizationTO.PersonList != null && tblOrganizationTO.PersonList.Count > 0)
                    tblOrganizationTO.PersonList = tblOrganizationTO.PersonList.Where(e => e.isActive == 1).ToList();
                if (tblOrganizationTO.PersonList != null && tblOrganizationTO.PersonList.Count > 0)
                {
                    for (int i = 0; i < tblOrganizationTO.PersonList.Count; i++)
                    {
                        TblPersonTO personTO = tblOrganizationTO.PersonList[i];

                        if (personTO.DobDay > 0 && personTO.DobMonth > 0 && personTO.DobYear > 0)
                        {
                            personTO.DateOfBirth = new DateTime(personTO.DobYear, personTO.DobMonth, personTO.DobDay);
                        }
                        else if (personTO.DateOfBirth != null && personTO.DateOfBirth != new DateTime())
                        {
                            personTO.DateOfBirth = new DateTime(personTO.DateOfBirth.Year, personTO.DateOfBirth.Month, personTO.DateOfBirth.Day + 1);
                        }
                        else
                        {
                            personTO.DateOfBirth = DateTime.MinValue;
                        }

                        if (personTO.IdPerson == 0)
                        {
                            personTO.CreatedBy = tblOrganizationTO.UpdatedBy;
                            personTO.CreatedOn = tblOrganizationTO.UpdatedOn;
                            result = _iTblPersonDAO.InsertTblPerson(personTO, conn, tran);
                            if (result != 1)
                            {
                                tran.Rollback();
                                rMessage.MessageType = ResultMessageE.Error;
                                rMessage.Text = "Error While InsertTblPerson in Method UpdateOrganization ";
                                return rMessage;
                            }

                            TblOrgPersonDtlsTO orgPersonTO = new TblOrgPersonDtlsTO();
                            orgPersonTO.IsActive = 1;
                            orgPersonTO.OrganizationId = tblOrganizationTO.IdOrganization;
                            orgPersonTO.CreatedBy = tblOrganizationTO.UpdatedBy;
                            orgPersonTO.CreatedOn = tblOrganizationTO.UpdatedOn;

                            orgPersonTO.PersonTypeId = personTO.PersonTypeId;
                            orgPersonTO.PersonId = personTO.IdPerson;
                            result = _iTblOrgPersonDtlsBL.InsertTblOrgPersonDtls(orgPersonTO, conn, tran);
                            if (result != 1)
                            {
                                tran.Rollback();
                                rMessage.MessageType = ResultMessageE.Error;
                                rMessage.Text = "Error While InsertTblPerson in Method SaveNewOrganization ";
                                return rMessage;
                            }
                        }
                        else
                        {
                            result = _iTblPersonDAO.UpdateTblPerson(personTO, conn, tran);
                            if (result != 1)
                            {
                                tran.Rollback();
                                rMessage.MessageType = ResultMessageE.Error;
                                rMessage.Text = "Error While UpdateTblPerson in Method UpdateOrganization ";
                                return rMessage;
                            }


                        }

                        //if (personTO.PersonTypeId == (int)Constants.VisitPersonE.FIRST_OWNER)
                        if (personTO.IsDefault1 == 1)
                        {
                            tblOrganizationTO.FirstOwnerPersonId = personTO.IdPerson;
                            tblOrganizationTO.FirstOwnerName = personTO.FirstName + " " + personTO.LastName;
                            firstOwnerPersonTO = personTO;
                            updateOrgYn = true;
                        }
                        //if (personTO.PersonTypeId == (int)Constants.VisitPersonE.SECOND_OWNER)
                        else if (personTO.IsDefault2 == 1)
                        {
                            tblOrganizationTO.SecondOwnerPersonId = personTO.IdPerson;
                            tblOrganizationTO.SecondOwnerName = personTO.FirstName + " " + personTO.LastName;
                            secondOwnerPersonTO = personTO;
                            updateOrgYn = true;
                        }
                    }

                }

                #endregion

                #region 3. Add Address Details

                List<TblOrgAddressTO> tblOrgAddressTOList = new List<Models.TblOrgAddressTO>();
                if (tblOrganizationTO.AddressList != null && tblOrganizationTO.AddressList.Count > 0)
                {
                    for (int i = 0; i < tblOrganizationTO.AddressList.Count; i++)
                    {
                        TblAddressTO addressTO = tblOrganizationTO.AddressList[i];
                        addressTO.CreatedBy = tblOrganizationTO.UpdatedBy;
                        addressTO.CreatedOn = tblOrganizationTO.UpdatedOn;
                        if (addressTO.CountryId == 0)
                            addressTO.CountryId = Constants.DefaultCountryID;

                        if (addressTO.DistrictId == 0 && !string.IsNullOrEmpty(addressTO.DistrictName))
                        {
                            //Insert New District
                            CommonDimensionsTO districtDimensionTO = new CommonDimensionsTO();
                            districtDimensionTO.ParentId = addressTO.StateId;
                            districtDimensionTO.DimensionName = addressTO.DistrictName;
                            List<DropDownTO> districtList = _iDimensionDAO.SelectDistrictByName(addressTO.DistrictName, addressTO.StateId, conn, tran);
                            if (districtList != null && districtList.Count > 0)
                            {
                                addressTO.DistrictId = districtList[0].Value;
                            }
                            else
                            {
                                result = _iDimensionDAO.InsertDistrict(districtDimensionTO, conn, tran);
                                if (result != 1)
                                {
                                    tran.Rollback();
                                    rMessage.MessageType = ResultMessageE.Error;
                                    rMessage.Text = "Error While InsertDistrict in Method UpdateOrganization";
                                    return rMessage;
                                }
                                addressTO.DistrictId = districtDimensionTO.IdDimension;
                            }
                        }

                        if (addressTO.TalukaId == 0 && !string.IsNullOrEmpty(addressTO.TalukaName))
                        {
                            //Insert New Taluka
                            CommonDimensionsTO talukaDimensionTO = new CommonDimensionsTO();
                            talukaDimensionTO.ParentId = addressTO.DistrictId;
                            talukaDimensionTO.DimensionName = addressTO.TalukaName;
                            List<DropDownTO> talukaList = _iDimensionDAO.SelectTalukaByName(addressTO.TalukaName, addressTO.DistrictId, conn, tran);
                            if (talukaList != null && talukaList.Count > 0)
                            {
                                addressTO.TalukaId = talukaList[0].Value;
                            }
                            else
                            {
                                result = _iDimensionDAO.InsertTaluka(talukaDimensionTO, conn, tran);
                                if (result != 1)
                                {
                                    tran.Rollback();
                                    rMessage.MessageType = ResultMessageE.Error;
                                    rMessage.Text = "Error While InsertTaluka in Method UpdateOrganization";
                                    return rMessage;
                                }

                                addressTO.TalukaId = talukaDimensionTO.IdDimension;
                            }
                        }

                        if (addressTO.IdAddr == 0 && addressTO.IsActive == 1)
                        {
                            result = _iTblAddressDAO.InsertTblAddress(addressTO, conn, tran);
                            if (result != 1)
                            {
                                tran.Rollback();
                                rMessage.MessageType = ResultMessageE.Error;
                                rMessage.Text = "Error While InsertTblAddress in Method UpdateOrganization";
                                return rMessage;
                            }

                            TblOrgAddressTO tblOrgAddressTO = addressTO.GetTblOrgAddressTO();
                            tblOrgAddressTO.OrganizationId = tblOrganizationTO.IdOrganization;

                            result = _iTblOrgAddressDAO.InsertTblOrgAddress(tblOrgAddressTO, conn, tran);
                            if (result != 1)
                            {
                                tran.Rollback();
                                rMessage.MessageType = ResultMessageE.Error;
                                rMessage.Text = "Error While InsertTblOrgAddress in Method UpdateOrganization";
                                return rMessage;
                            }

                            if (addressTO.IsDefault == 1)
                            {
                                updateOrgYn = true;
                                tblOrganizationTO.AddrId = addressTO.IdAddr;
                            }
                            if (IsLiscenseAgaintsAddress == 1)
                            {
                                if (addressTO.OrgLicenseDtlTOList != null && addressTO.OrgLicenseDtlTOList.Count > 0)
                                {
                                    for (int ol = 0; ol < addressTO.OrgLicenseDtlTOList.Count; ol++)
                                    {
                                        if (addressTO.OrgLicenseDtlTOList[ol].LicenseValue != null && !string.IsNullOrEmpty(addressTO.OrgLicenseDtlTOList[ol].LicenseValue))
                                        {
                                            if (addressTO.OrgLicenseDtlTOList[ol].LicenseValue != "0")
                                            {
                                                addressTO.OrgLicenseDtlTOList[ol].OrganizationId = tblOrganizationTO.IdOrganization;
                                                addressTO.OrgLicenseDtlTOList[ol].CreatedBy = tblOrganizationTO.UpdatedBy;
                                                addressTO.OrgLicenseDtlTOList[ol].CreatedOn = tblOrganizationTO.UpdatedOn;
                                                addressTO.OrgLicenseDtlTOList[ol].AddressId = addressTO.IdAddr;
                                                result = _iTblOrgLicenseDtlDAO.InsertTblOrgLicenseDtl(addressTO.OrgLicenseDtlTOList[ol], conn, tran);
                                                if (result != 1)
                                                {
                                                    // tran.Rollback();
                                                    rMessage.MessageType = ResultMessageE.Error;
                                                    rMessage.Text = "Error While InsertTblOrgLicenseDtl for Users in Method SaveNewOrganization ";
                                                    return rMessage;
                                                }

                                            }
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (addressTO.IsDefault == 1)
                            {
                                updateOrgYn = true;
                                tblOrganizationTO.AddrId = addressTO.IdAddr;
                            }

                            result = _iTblAddressDAO.UpdateTblAddress(addressTO, conn, tran);

                            if (result != 1)
                            {
                                tran.Rollback();
                                rMessage.MessageType = ResultMessageE.Error;
                                rMessage.Text = "Error While UpdateTblAddress in Method UpdateOrganization";
                                return rMessage;
                            }
                            TblOrgAddressTO tblOrgAddressTO = addressTO.GetTblOrgAddressTO();
                            tblOrgAddressTO.OrganizationId = tblOrganizationTO.IdOrganization;
                            //Priyanka [23-09-2019]: Added to set the address Type.
                            tblOrgAddressTO.AddrTypeId = addressTO.AddrTypeId;
                            tblOrgAddressTO.IdOrgAddr = addressTO.IdOrgAddr;
                            tblOrgAddressTO.AddressId = addressTO.IdAddr;
                            tblOrgAddressTO.UpdatedBy = tblOrganizationTO.UpdatedBy;
                            tblOrgAddressTO.UpdatedOn = _iCommon.ServerDateTime;
                            result = _iTblOrgAddressDAO.UpdateTblOrgAddress(tblOrgAddressTO, conn, tran);
                            if (result != 1)
                            {
                                // tran.Rollback();
                                rMessage.MessageType = ResultMessageE.Error;
                                rMessage.Text = "Error While InsertTblOrgAddress in Method SaveNewOrganization";
                                return rMessage;
                            }
                            if (IsLiscenseAgaintsAddress == 1)
                            {
                                if (addressTO.OrgLicenseDtlTOList != null && addressTO.OrgLicenseDtlTOList.Count > 0)
                                {
                                    for (int ol = 0; ol < addressTO.OrgLicenseDtlTOList.Count; ol++)
                                    {
                                        if (addressTO.OrgLicenseDtlTOList[ol].LicenseValue != null && !string.IsNullOrEmpty(addressTO.OrgLicenseDtlTOList[ol].LicenseValue))
                                        {
                                            if (addressTO.OrgLicenseDtlTOList[ol].LicenseValue != "0")
                                            {
                                                if (tblOrganizationTO.OrgLicenseDtlTOList[ol].IdOrgLicense == 0)
                                                {
                                                    addressTO.OrgLicenseDtlTOList[ol].OrganizationId = tblOrganizationTO.IdOrganization;
                                                    addressTO.OrgLicenseDtlTOList[ol].CreatedBy = tblOrganizationTO.UpdatedBy;
                                                    addressTO.OrgLicenseDtlTOList[ol].CreatedOn = tblOrganizationTO.UpdatedOn;
                                                    addressTO.OrgLicenseDtlTOList[ol].AddressId = addressTO.IdAddr;
                                                    result = _iTblOrgLicenseDtlDAO.InsertTblOrgLicenseDtl(addressTO.OrgLicenseDtlTOList[ol], conn, tran);
                                                    if (result != 1)
                                                    {
                                                        // tran.Rollback();
                                                        rMessage.MessageType = ResultMessageE.Error;
                                                        rMessage.Text = "Error While InsertTblOrgLicenseDtl for Users in Method SaveNewOrganization ";
                                                        return rMessage;
                                                    }
                                                }
                                                else
                                                {
                                                    addressTO.OrgLicenseDtlTOList[ol].IdOrgLicense = tblOrganizationTO.OrgLicenseDtlTOList[ol].IdOrgLicense; //[2021-06-19] Dhananjay Added
                                                    addressTO.OrgLicenseDtlTOList[ol].OrganizationId = tblOrganizationTO.IdOrganization;
                                                    addressTO.OrgLicenseDtlTOList[ol].AddressId = addressTO.IdAddr;
                                                    result = _iTblOrgLicenseDtlDAO.UpdateTblOrgLicenseDtl(addressTO.OrgLicenseDtlTOList[ol], conn, tran);
                                                    if (result != 1)
                                                    {
                                                        // tran.Rollback();
                                                        rMessage.MessageType = ResultMessageE.Error;
                                                        rMessage.Text = "Error While UpdateTblOrgLicenseDtl";
                                                        return rMessage;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }

                    }
                }
                #endregion

                #region 4. Save Organization Commercial Licences
                if (tblOrganizationTO.OrgLicenseDtlTOList != null && tblOrganizationTO.OrgLicenseDtlTOList.Count > 0)
                {
                    if (IsLiscenseAgaintsAddress == 0)
                    {
                        for (int ol = 0; ol < tblOrganizationTO.OrgLicenseDtlTOList.Count; ol++)
                        {
                            if (!String.IsNullOrEmpty(tblOrganizationTO.OrgLicenseDtlTOList[ol].LicenseValue))
                                if (tblOrganizationTO.OrgLicenseDtlTOList[ol].IdOrgLicense == 0)
                                {
                                    tblOrganizationTO.OrgLicenseDtlTOList[ol].OrganizationId = tblOrganizationTO.IdOrganization;
                                    tblOrganizationTO.OrgLicenseDtlTOList[ol].CreatedBy = tblOrganizationTO.UpdatedBy;
                                    tblOrganizationTO.OrgLicenseDtlTOList[ol].CreatedOn = tblOrganizationTO.UpdatedOn;
                                    tblOrganizationTO.OrgLicenseDtlTOList[ol].AddressId = tblOrganizationTO.AddrId;
                                    result = _iTblOrgLicenseDtlDAO.InsertTblOrgLicenseDtl(tblOrganizationTO.OrgLicenseDtlTOList[ol], conn, tran);
                                    if (result != 1)
                                    {
                                        tran.Rollback();
                                        rMessage.MessageType = ResultMessageE.Error;
                                        rMessage.Text = "Error While InsertTblOrgLicenseDtl for Users in Method UpdateOrganization ";
                                        return rMessage;
                                    }
                                }
                                else
                                {
                                    tblOrganizationTO.OrgLicenseDtlTOList[ol].OrganizationId = tblOrganizationTO.IdOrganization;
                                    tblOrganizationTO.OrgLicenseDtlTOList[ol].AddressId = tblOrganizationTO.AddrId;
                                    result = _iTblOrgLicenseDtlDAO.UpdateTblOrgLicenseDtl(tblOrganizationTO.OrgLicenseDtlTOList[ol], conn, tran);
                                    if (result != 1)
                                    {
                                        tran.Rollback();
                                        rMessage.MessageType = ResultMessageE.Error;
                                        rMessage.Text = "Error While InsertTblOrgLicenseDtl for Users in Method UpdateOrganization ";
                                        return rMessage;
                                    }
                                }
                        }
                    }
                }
                #endregion

                #region 6. Update KYC Details
                //Priyanka [23-10-2018]  : Added to update KYC Details.
                if (tblOrganizationTO.KYCDetailsTO != null)
                {
                    TblKYCDetailsTO tblKYCDetailsTOExt = _iTblKYCDetailsBL.SelectTblKYCDetailsTOByOrg(tblOrganizationTO.IdOrganization);
                    if (tblKYCDetailsTOExt != null)
                    {
                        tblKYCDetailsTOExt.IsActive = 0;
                        tblKYCDetailsTOExt.OrganizationId = tblOrganizationTO.IdOrganization;
                        tblKYCDetailsTOExt.UpdatedOn = tblOrganizationTO.UpdatedOn;
                        tblKYCDetailsTOExt.UpdatedBy = tblOrganizationTO.UpdatedBy;
                        result = _iTblKYCDetailsBL.UpdateTblKYCDetails(tblKYCDetailsTOExt, conn, tran);
                        if (result != 1)
                        {
                            tran.Rollback();
                            rMessage.MessageType = ResultMessageE.Error;
                            rMessage.Text = "Error While UpdateTblKYCDetails for Users in Method UpdateOrganization ";
                            return rMessage;
                        }
                        tblOrganizationTO.KYCDetailsTO.IsActive = 1;
                        tblOrganizationTO.KYCDetailsTO.OrganizationId = tblOrganizationTO.IdOrganization;
                        tblOrganizationTO.KYCDetailsTO.CreatedBy = tblOrganizationTO.UpdatedBy;
                        tblOrganizationTO.KYCDetailsTO.CreatedOn = tblOrganizationTO.UpdatedOn;
                        result = _iTblKYCDetailsBL.InsertTblKYCDetails(tblOrganizationTO.KYCDetailsTO, conn, tran);
                        if (result != 1)
                        {
                            tran.Rollback();
                            rMessage.MessageType = ResultMessageE.Error;
                            rMessage.Text = "Error While InsertTblKYCDetails for Users in Method UpdateOrganization ";
                            return rMessage;
                        }
                    }
                    else
                    {

                        tblOrganizationTO.KYCDetailsTO.OrganizationId = tblOrganizationTO.IdOrganization;
                        tblOrganizationTO.KYCDetailsTO.IsActive = 1;
                        tblOrganizationTO.KYCDetailsTO.CreatedBy = tblOrganizationTO.UpdatedBy;
                        tblOrganizationTO.KYCDetailsTO.CreatedOn = tblOrganizationTO.UpdatedOn;
                        result = _iTblKYCDetailsBL.InsertTblKYCDetails(tblOrganizationTO.KYCDetailsTO, conn, tran);
                        if (result != 1)
                        {
                            tran.Rollback();
                            rMessage.MessageType = ResultMessageE.Error;
                            rMessage.Text = "Error While InsertTblKYCDetails for Users in Method UpdateOrganization ";
                            return rMessage;
                        }
                    }
                }
                #endregion

                #region  Save Organization Bank Details 
                if (tblOrganizationTO.BankDetailsList != null && tblOrganizationTO.BankDetailsList.Count > 0)
                {
                    for (int ol = 0; ol < tblOrganizationTO.BankDetailsList.Count; ol++)
                    {

                        if (tblOrganizationTO.BankDetailsList[ol].IdOrgBankDtls == 0)
                        {
                            TblOrgBankDetailsTO bankDetailsTO = tblOrganizationTO.BankDetailsList[ol];

                            bankDetailsTO.CreatedBy = tblOrganizationTO.UpdatedBy;
                            bankDetailsTO.CreatedOn = tblOrganizationTO.UpdatedOn;
                            bankDetailsTO.BankOrgId = bankDetailsTO.BankOrgId;
                            bankDetailsTO.OrgId = tblOrganizationTO.IdOrganization;
                            result = _iTblOrgBankDetailsDAO.InsertTblOrgBankDetails(bankDetailsTO, conn, tran);
                            if (result != 1)
                            {
                                tran.Rollback();
                                rMessage.MessageType = ResultMessageE.Error;
                                rMessage.Text = "Error While InsertTblOrgBankDetails for Users in Method UpdateOrganization ";
                                return rMessage;
                            }
                        }
                        else
                        {
                            TblOrgBankDetailsTO bankDetailsTO = tblOrganizationTO.BankDetailsList[ol];
                            bankDetailsTO.UpdatedBy = tblOrganizationTO.UpdatedBy;
                            bankDetailsTO.UpdatedOn = tblOrganizationTO.UpdatedOn;
                            bankDetailsTO.BankOrgId = bankDetailsTO.BankOrgId;
                            bankDetailsTO.OrgId = tblOrganizationTO.IdOrganization;
                            result = _iTblOrgBankDetailsDAO.UpdateTblOrgBankDetails(bankDetailsTO, conn, tran);
                            if (result != 1)
                            {
                                tran.Rollback();
                                rMessage.MessageType = ResultMessageE.Error;
                                rMessage.Text = "Error While UpdateTblOrgBankDetails for Users in Method UpdateOrganization ";
                                return rMessage;
                            }
                        }
                    }
                }
                #endregion

                #region Save Accounting Details
                if (tblOrganizationTO.TblOrgAccountTaxTO != null)
                {
                    if (tblOrganizationTO.TblOrgAccountTaxTO.IdOrgAccountTax == 0)
                    {
                        tblOrganizationTO.TblOrgAccountTaxTO.OrganizationId = tblOrganizationTO.IdOrganization;
                        result = _itblOrgAccountTaxDAO.InsertTblOrgAccountTax(tblOrganizationTO.TblOrgAccountTaxTO, conn, tran);
                        if (result != 1)
                        {
                            // tran.Rollback();
                            rMessage.MessageType = ResultMessageE.Error;
                            rMessage.Text = "Error While InsertTblOrgAccountTax for Users in Method SaveNewOrganization ";
                            return rMessage;
                        }

                        else
                        {
                            if (tblOrganizationTO.TblOrgAccountTaxTO.TblOrgAccountTaxDtlsList != null && tblOrganizationTO.TblOrgAccountTaxTO.TblOrgAccountTaxDtlsList.Count > 0)
                            {
                                tblOrganizationTO.TblOrgAccountTaxTO.TblOrgAccountTaxDtlsList.ForEach(res =>
                                {
                                    res.OrgAccountTaxId = tblOrganizationTO.TblOrgAccountTaxTO.IdOrgAccountTax;
                                });
                                for (int i = 0; i < tblOrganizationTO.TblOrgAccountTaxTO.TblOrgAccountTaxDtlsList.Count; i++)
                                {
                                    result = _iTblOrgAccountTaxDtlsDAO.InsertTblOrgAccountTaxDtls(tblOrganizationTO.TblOrgAccountTaxTO.TblOrgAccountTaxDtlsList[i], conn, tran);
                                    if (result != 1)
                                    {
                                        // tran.Rollback();
                                        rMessage.MessageType = ResultMessageE.Error;
                                        rMessage.Text = "Error While InsertTblOrgAccountTaxDtls for Users in Method SaveNewOrganization ";
                                        return rMessage;
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        tblOrganizationTO.TblOrgAccountTaxTO.OrganizationId = tblOrganizationTO.IdOrganization;
                        result = _itblOrgAccountTaxDAO.UpdateTblOrgAccountTax(tblOrganizationTO.TblOrgAccountTaxTO, conn, tran);
                        if (result != 1)
                        {
                            // tran.Rollback();
                            rMessage.MessageType = ResultMessageE.Error;
                            rMessage.Text = "Error While UpdateTblOrgAccountTax for Users in Method SaveNewOrganization ";
                            return rMessage;
                        }

                        else
                        {
                            if (tblOrganizationTO.TblOrgAccountTaxTO.IsInsertTblOrgAccTaxDtls)
                            {
                                result = _iTblOrgAccountTaxDtlsDAO.UpdateTblOrgAccountTaxDtls(tblOrganizationTO.TblOrgAccountTaxTO, conn, tran);
                                if (result == -1)
                                {
                                    rMessage.MessageType = ResultMessageE.Error;
                                    rMessage.Text = "Error While UpdateTblOrgAccountTaxDtls for Users in Method SaveNewOrganization ";
                                    return rMessage;
                                }
                                if (tblOrganizationTO.TblOrgAccountTaxTO.TblOrgAccountTaxDtlsList != null && tblOrganizationTO.TblOrgAccountTaxTO.TblOrgAccountTaxDtlsList.Count > 0)
                                {

                                    tblOrganizationTO.TblOrgAccountTaxTO.TblOrgAccountTaxDtlsList.ForEach(res =>
                                    {
                                        res.OrgAccountTaxId = tblOrganizationTO.TblOrgAccountTaxTO.IdOrgAccountTax;
                                    });



                                    for (int i = 0; i < tblOrganizationTO.TblOrgAccountTaxTO.TblOrgAccountTaxDtlsList.Count; i++)
                                    {
                                        if(tblOrganizationTO.TblOrgAccountTaxTO.TblOrgAccountTaxDtlsList[i].IsActive == true)
                                        {
                                            result = _iTblOrgAccountTaxDtlsDAO.InsertTblOrgAccountTaxDtls(tblOrganizationTO.TblOrgAccountTaxTO.TblOrgAccountTaxDtlsList[i], conn, tran);
                                            if (result != 1)
                                            {
                                                // tran.Rollback();
                                                rMessage.MessageType = ResultMessageE.Error;
                                                rMessage.Text = "Error While InsertTblOrgAccountTaxDtls for Users in Method SaveNewOrganization ";
                                                return rMessage;
                                            }
                                        }
                                       
                                    }
                                }
                            }

                        }
                    }

                }
                #endregion

                #region 7. Is Address Or Concern Person Found then Update in tblOrganization

                if (updateOrgYn)
                {
                    result = UpdateTblOrganization(tblOrganizationTO, conn, tran);
                    if (result != 1)
                    {
                        tran.Rollback();
                        rMessage.MessageType = ResultMessageE.Error;
                        rMessage.Text = "Error While UpdateTblOrganization for Persons in Method UpdateOrganization ";
                        return rMessage;
                    }
                }

                #endregion

                #region 8. If New Organization Type is Cnf Then Auto Create User for First Owner

                /*
                if (tblOrganizationTO.OrgTypeE == Constants.OrgTypeE.C_AND_F_AGENT)
                {
                    if (tblOrganizationTO.FirstOwnerPersonId > 0)
                    {
                        if (isNewFirstOwner)
                        {
                            String userId = _iTblUserBL.CreateUserName(firstOwnerPersonTO.FirstName, firstOwnerPersonTO.LastName, conn, tran);
                            String pwd = Constants.DefaultPassword;

                            TblUserTO userTO = new Models.TblUserTO();
                            userTO.UserLogin = userId;
                            userTO.UserPasswd = pwd;
                            userTO.UserDisplayName = firstOwnerPersonTO.FirstName + " " + firstOwnerPersonTO.LastName;
                            userTO.IsActive = 1;

                            result = BL._iTblUserBL.InsertTblUser(userTO, conn, tran);

                            if (result != 1)
                            {
                                tran.Rollback();
                                rMessage.MessageType = ResultMessageE.Error;
                                rMessage.Text = "Error While InsertTblUser for Users in Method SaveNewOrganization ";
                                return rMessage;
                            }

                            TblUserExtTO tblUserExtTO = new TblUserExtTO();
                            tblUserExtTO.AddressId = tblOrganizationTO.AddrId;
                            tblUserExtTO.CreatedBy = tblOrganizationTO.UpdatedBy;
                            tblUserExtTO.CreatedOn = tblOrganizationTO.UpdatedOn;
                            tblUserExtTO.PersonId = tblOrganizationTO.FirstOwnerPersonId;
                            tblUserExtTO.OrganizationId = tblOrganizationTO.IdOrganization;
                            tblUserExtTO.UserId = userTO.IdUser;
                            tblUserExtTO.Comments = "New C&F User Created";

                            result = BL._iTblUserExtBL.InsertTblUserExt(tblUserExtTO, conn, tran);
                            if (result != 1)
                            {
                                tran.Rollback();
                                rMessage.MessageType = ResultMessageE.Error;
                                rMessage.Text = "Error While InsertTblUserExt for Users in Method SaveNewOrganization ";
                                return rMessage;
                            }

                            TblUserRoleTO tblUserRoleTO = new TblUserRoleTO();
                            tblUserRoleTO.UserId = userTO.IdUser;
                            tblUserRoleTO.RoleId = (int)Constants.SystemRolesE.C_AND_F_AGENT;
                            tblUserRoleTO.IsActive = 1;
                            tblUserRoleTO.CreatedBy = tblOrganizationTO.UpdatedBy;
                            tblUserRoleTO.CreatedOn = tblOrganizationTO.UpdatedOn;

                            result = BL._iTblUserRoleBL.InsertTblUserRole(tblUserRoleTO, conn, tran);
                            if (result != 1)
                            {
                                tran.Rollback();
                                rMessage.MessageType = ResultMessageE.Error;
                                rMessage.Text = "Error While InsertTblUserRole for C&F Users in Method SaveNewOrganization ";
                                return rMessage;
                            }
                        }
                    }
                }
                */
                #endregion

                #region If Dealer then Manage Cnf & Dealer Relationship


                if (tblOrganizationTO.OrgTypeE == Constants.OrgTypeE.DEALER)
                {

                    List<TblCnfDealersTO> exList = _iTblCnfDealersBL.SelectAllActiveCnfDealersList(tblOrganizationTO.IdOrganization, false, conn, tran);
                    //if (exList == null || exList.Count == 0)
                    //{
                    //    tran.Rollback();
                    //    rMessage.MessageType = ResultMessageE.Error;
                    //    rMessage.Text = "C&F and dealer relation not found in Method UpdateOrganization ";
                    //    return rMessage;
                    //}
                    //Sudhir[12-July-2018] Added Condition For CRM Organization Added Who dont have CNF Id 
                    if (exList != null && exList.Count > 0)
                    {
                        var primaryCnfDelTO = exList.Where(a => a.IsActive == 1 && a.IsSpecialCnf == 0).FirstOrDefault();

                        if (primaryCnfDelTO.CnfOrgId != tblOrganizationTO.ParentId)
                        {
                            //update existing record and set to deactive
                            primaryCnfDelTO.IsActive = 0;
                            result = _iTblCnfDealersBL.UpdateTblCnfDealers(primaryCnfDelTO, conn, tran);
                            if (result != 1)
                            {
                                tran.Rollback();
                                rMessage.MessageType = ResultMessageE.Error;
                                rMessage.Text = "Error While UpdateTblCnfDealers for Primary C&F in Method UpdateOrganization ";
                                return rMessage;
                            }

                            //  Insert New Record of Relationship
                            TblCnfDealersTO newTblCnfDealersTO = new TblCnfDealersTO();
                            newTblCnfDealersTO.IsActive = 1;
                            newTblCnfDealersTO.CnfOrgId = tblOrganizationTO.ParentId;
                            newTblCnfDealersTO.DealerOrgId = tblOrganizationTO.IdOrganization;
                            newTblCnfDealersTO.Remark = "Updated Primary C&F";
                            newTblCnfDealersTO.CreatedBy = tblOrganizationTO.UpdatedBy;
                            newTblCnfDealersTO.CreatedOn = tblOrganizationTO.UpdatedOn;
                            result = _iTblCnfDealersBL.InsertTblCnfDealers(newTblCnfDealersTO, conn, tran);
                            if (result != 1)
                            {
                                tran.Rollback();
                                rMessage.MessageType = ResultMessageE.Error;
                                rMessage.Text = "Error While InsertTblCnfDealers for Primary C&F in Method UpdateOrganization ";
                                return rMessage;
                            }
                        }
                    }
                    else
                    {
                        if (tblOrganizationTO.ParentId != 0)
                        {
                            // Insert New Record of Relationship
                            TblCnfDealersTO newTblCnfDealersTO = new TblCnfDealersTO();
                            newTblCnfDealersTO.IsActive = 1;
                            newTblCnfDealersTO.CnfOrgId = tblOrganizationTO.ParentId;
                            newTblCnfDealersTO.DealerOrgId = tblOrganizationTO.IdOrganization;
                            newTblCnfDealersTO.Remark = "Updated Primary C&F";
                            newTblCnfDealersTO.CreatedBy = tblOrganizationTO.UpdatedBy;
                            newTblCnfDealersTO.CreatedOn = tblOrganizationTO.UpdatedOn;
                            result = _iTblCnfDealersBL.InsertTblCnfDealers(newTblCnfDealersTO, conn, tran);
                            if (result != 1)
                            {
                                tran.Rollback();
                                rMessage.MessageType = ResultMessageE.Error;
                                rMessage.Text = "Error While InsertTblCnfDealers for Primary C&F in Method UpdateOrganization ";
                                return rMessage;
                            }
                        }
                        //    tran.Rollback();
                        //    rMessage.MessageType = ResultMessageE.Error;
                        //    rMessage.Text = "C&F and dealer relation not found in Method UpdateOrganization ";
                        //return rMessage;
                    }
                    if (tblOrganizationTO.CnfDealersTOList != null)
                    {
                        for (int c = 0; c < tblOrganizationTO.CnfDealersTOList.Count; c++)
                        {

                            TblCnfDealersTO tblCnfDealersTO = new TblCnfDealersTO();

                            var SpecialTO = exList.Where(a => a.CnfOrgId == tblOrganizationTO.CnfDealersTOList[c].CnfOrgId && a.DealerOrgId == tblOrganizationTO.IdOrganization).FirstOrDefault();

                            if (SpecialTO == null)
                            {
                                tblOrganizationTO.CnfDealersTOList[c].DealerOrgId = tblOrganizationTO.IdOrganization;
                                tblOrganizationTO.CnfDealersTOList[c].CreatedBy = tblOrganizationTO.UpdatedBy;
                                tblOrganizationTO.CnfDealersTOList[c].CreatedOn = tblOrganizationTO.UpdatedOn;
                                tblOrganizationTO.CnfDealersTOList[c].IsActive = 1;
                                tblOrganizationTO.CnfDealersTOList[c].IsSpecialCnf = 1;
                                tblOrganizationTO.CnfDealersTOList[c].Remark = "Special C&F";

                                result = _iTblCnfDealersBL.InsertTblCnfDealers(tblOrganizationTO.CnfDealersTOList[c], conn, tran);

                                if (result != 1)
                                {
                                    tran.Rollback();
                                    rMessage.MessageType = ResultMessageE.Error;
                                    rMessage.Text = "Error While InsertTblCnfDealers for Special C&F in Method UpdateOrganization ";
                                    return rMessage;
                                }
                            }
                        }
                    }
                }


                #endregion

                #region 9. User Creation Based On Orgtype settings defined in the masters

                DimOrgTypeTO orgTypeTO = _iDimOrgTypeDAO.SelectDimOrgType(tblOrganizationTO.OrgTypeId, conn, tran);
                if(orgTypeTO != null)//Aditee Added to check orgtypeto
                {
                    if (orgTypeTO.CreateUserYn == 1 && orgTypeTO.DefaultRoleId > 0)
                    {

                        if (tblOrganizationTO.PersonList != null && tblOrganizationTO.PersonList.Count > 0)
                        {


                            for (int i = 0; i < tblOrganizationTO.PersonList.Count; i++)

                            {
                                TblPersonTO personInfo = tblOrganizationTO.PersonList[i];

                                if (personInfo.IsUserCreate == 1)
                                {

                                    if (personInfo.IdPerson != 0)
                                    {
                                        //Harshala added to check user already created or not
                                        List<TblUserExtTO> createdUserList = _iTblUserExtDAO.SelectTblUserExtByPersonId(personInfo.IdPerson);
                                        if (createdUserList == null)
                                        {
                                            rMessage = CreateNewOrganizationUser(tblOrganizationTO, personInfo, orgTypeTO, tblOrganizationTO.UpdatedBy, tblOrganizationTO.UpdatedOn, conn, tran);
                                            if (rMessage.MessageType != ResultMessageE.Information)
                                            {
                                                tran.Rollback();
                                                rMessage.DefaultBehaviour("Error While Org User Creation");
                                                return rMessage;
                                            }
                                        }
                                    }


                                }

                            }



                        }


                        //if (tblOrganizationTO.FirstOwnerPersonId > 0)
                        //{
                        //    if (isNewFirstOwner)
                        //    {
                        //        rMessage = CreateNewOrganizationUser(tblOrganizationTO, firstOwnerPersonTO, orgTypeTO, tblOrganizationTO.UpdatedBy, tblOrganizationTO.UpdatedOn, conn, tran);
                        //        if (rMessage.MessageType != ResultMessageE.Information)
                        //        {
                        //            tran.Rollback();
                        //            rMessage.DefaultBehaviour("Error While Org User Creation");
                        //            return rMessage;
                        //        }
                        //    }
                        //}

                        //if (tblOrganizationTO.SecondOwnerPersonId > 0)
                        //{
                        //    if (isNewSecondOwner)
                        //    {
                        //        rMessage = CreateNewOrganizationUser(tblOrganizationTO, secondOwnerPersonTO, orgTypeTO, tblOrganizationTO.UpdatedBy, tblOrganizationTO.UpdatedOn, conn, tran);
                        //        if (rMessage.MessageType != ResultMessageE.Information)
                        //        {
                        //            tran.Rollback();
                        //            rMessage.DefaultBehaviour("Error While Org User Creation");
                        //            return rMessage;
                        //        }
                        //    }
                        //}
                    }
                    else
                    {
                        // Do Nothing. allow to save the record.
                    }
                }

              
                #endregion

                #region Update master in SAP
                TblConfigParamsTO tblConfigParamsTOSAPService = _iTblConfigParamsDAO.SelectTblConfigParams(Constants.SAPB1_SERVICES_ENABLE);
                if (tblConfigParamsTOSAPService != null)
                {
                    if (Convert.ToInt32(tblConfigParamsTOSAPService.ConfigParamVal) == 1)
                    {
                    //    if (tblOrganizationTO.OrgTypeE == Constants.OrgTypeE.SUPPLIER
                    //|| tblOrganizationTO.OrgTypeE == Constants.OrgTypeE.DEALER || tblOrganizationTO.OrgTypeE == Constants.OrgTypeE.BANK)

                        if(orgTypeTO !=null && orgTypeTO.IsTransferToSAP==1)
                        {
                            rMessage = UpdateMasterToSAP(tblOrganizationTO);
                            if (rMessage.Result != 1)
                            {
                                tran.Rollback();
                                return rMessage;
                            }

                        }
                    }
                }
                #endregion



                //Added By Gokul [04-03-21]
                if (tblOrganizationTO.PMId > 0)
                {

                    TblPurchaseManagerSupplierTO tblPurchaseManagerSupplierTO = new TblPurchaseManagerSupplierTO();
                    tblPurchaseManagerSupplierTO.CreatedOn = _iCommon.ServerDateTime;
                    tblPurchaseManagerSupplierTO.CreatedBy = tblOrganizationTO.CreatedBy;
                    tblPurchaseManagerSupplierTO.IsChecked = true;
                    tblPurchaseManagerSupplierTO.IsActive = 1;
                    //Add Newly added supplier ID
                    tblPurchaseManagerSupplierTO.OrganizationId = tblOrganizationTO.IdOrganization;
                    tblPurchaseManagerSupplierTO.IsDefaultPM = 1;
                    tblPurchaseManagerSupplierTO.UserId = tblOrganizationTO.PMId;
                    //check isdefaultpurchasemanager is allocated with same pm
                    int tempresult = _iTblPurchaseManagerSupplierBL.selectPurchaseManagerSupplierTo(tblPurchaseManagerSupplierTO, conn, tran);

                    //int tempresult = _itblpurchasemanagersupplierbl.insertupdatetblpurchasemanagersupplier(tblpurchasemanagersupplierto, conn, tran);

                    if (tempresult != 1)
                    {
                        tran.Rollback();
                        rMessage.DefaultBehaviour(rMessage.Text);
                        return rMessage;
                    }
                }
                //if (tblOrganizationTO.pmid > 0)
                //{

                //    tblpurchasemanagersupplierto tblpurchasemanagersupplierto = new tblpurchasemanagersupplierto();
                //    tblpurchasemanagersupplierto.createdon = tblorganizationto.createdon;
                //    tblpurchasemanagersupplierto.createdby = tblorganizationto.createdby;
                //    tblpurchasemanagersupplierto.ischecked = true;
                //    tblpurchasemanagersupplierto.isactive = 1;
                //    tblpurchasemanagersupplierto.isdefaultpm = 1;
                //    //add newly added supplier id
                //    tblpurchasemanagersupplierto.organizationid = tblorganizationto.idorganization;
                //    tblpurchasemanagersupplierto.userid = tblorganizationto.pmid;
                //    //check isdefaultpurchasemanager is allocated with same pm
                //    int tempresult = _itblpurchasemanagersupplierbl.selectpurchasemanagersupplierto(tblpurchasemanagersupplierto, conn, tran);

                //    //int tempresult = _itblpurchasemanagersupplierbl.insertupdatetblpurchasemanagersupplier(tblpurchasemanagersupplierto, conn, tran);
                //    if (tempresult != 1)
                //    {
                //        tran.rollback();
                //        rmessage.defaultbehaviour(rmessage.text);
                //        return rmessage;
                //    }
                //}

                tran.Commit();
                rMessage.MessageType = ResultMessageE.Information;
                rMessage.Text = "Record Saved Sucessfully";
                rMessage.Tag = tblOrganizationTO;
                rMessage.Result = 1;
                return rMessage;
            }
            catch (Exception ex)
            {
                rMessage.MessageType = ResultMessageE.Error;
                rMessage.Text = "Exception Error In Method UpdateOrganization";
                rMessage.Tag = ex;
                return rMessage;
            }
            finally
            {
                conn.Close();
            }
        }


        public ResultMessage DeactivateOrganization(TblOrganizationTO tblOrganizationTO)
        {
            ResultMessage rMessage = new StaticStuff.ResultMessage();
            rMessage.MessageType = ResultMessageE.None;
            rMessage.Text = "Not Entered Into Loop";
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            int result = 0;
            Boolean updateOrgYn = false;
            TblPersonTO firstOwnerPersonTO = null;
            TblPersonTO secondOwnerPersonTO = null;
            try
            {

                conn.Open();
                tran = conn.BeginTransaction();


                result = this.UpdateTblOrganization(tblOrganizationTO, conn, tran);
                if (result != 1)
                {
                    tran.Rollback();
                    rMessage.DefaultBehaviour("Error While Updating Organization");
                    return rMessage;
                }

                if (tblOrganizationTO.OrgTypeE == Constants.OrgTypeE.SUPPLIER
                    || tblOrganizationTO.OrgTypeE == Constants.OrgTypeE.DEALER
                    )
                {
                    TblConfigParamsTO isSapEnable = _iTblConfigParamsBL.SelectTblConfigParamsValByName(Constants.SAPB1_SERVICES_ENABLE);

                    if (isSapEnable != null && isSapEnable.ConfigParamVal != null && (Convert.ToInt32(isSapEnable.ConfigParamVal)) == 1)
                    {
                        if (Startup.CompanyObject == null)
                        {
                            tran.Rollback();
                            rMessage.DefaultBehaviour("SAP Company Object Found NULL");
                            return rMessage;
                        }
                        SAPbobsCOM.BusinessPartners oBusinesspartner;
                        oBusinesspartner = Startup.CompanyObject.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oBusinessPartners);

                        bool getResult = oBusinesspartner.GetByKey(tblOrganizationTO.IdOrganization.ToString());
                        if (!getResult)
                        {
                            tran.Rollback();
                            rMessage.DefaultBehaviour("Could not get Data From SAP");
                            return rMessage;
                        }

                        int removeResult = oBusinesspartner.Remove();
                        if (removeResult != 0)
                        {
                            tran.Rollback();
                            string errorinfor = Startup.CompanyObject.GetLastErrorDescription();
                            rMessage.DefaultBehaviour(errorinfor);
                            return rMessage;
                        }

                    }
                }

                tran.Commit();
                rMessage.DefaultSuccessBehaviour();
                return rMessage;

            }
            catch (Exception ex)
            {
                tran.Rollback();
                rMessage.DefaultExceptionBehaviour(ex, "DeactivateOrganization");
                return rMessage;
            }
            finally
            {
                conn.Close();
            }
        }



        private ResultMessage UpdateMasterToSAP(TblOrganizationTO organizationTO)
        {
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                Int32 IsLiscenseAgaintsAddress = 0;
                TblConfigParamsTO tblConfigParams = _iTblConfigParamsDAO.SelectTblConfigParamsValByName(Constants.CP_LICENSE_AGAINST_ADDRESS);
                if (tblConfigParams != null)
                {
                    if (Convert.ToInt32(tblConfigParams.ConfigParamVal) == 1)
                    {
                        IsLiscenseAgaintsAddress = 1;
                    }
                }
                List<DropDownTO> AddressTyleList = _iDimensionDAO.SelectAddressTypeListForDropDown();
                if (AddressTyleList == null || AddressTyleList.Count == 0)
                {
                    resultMessage.DefaultBehaviour("Failed to get address type list");
                    return resultMessage;
                }
                List<DropDownTO> currencyList = _iDimensionDAO.SelectCurrencyForDropDown();
                if (currencyList == null || currencyList.Count == 0)
                {
                    resultMessage.DefaultBehaviour("Failed to get currencyList ");
                    return resultMessage;
                }
                List<DropDownTO> orgGroupTypeList = new List<DropDownTO>();
                if (organizationTO.OrgGroupTypeId > 0)
                {
                    orgGroupTypeList = _iDimensionDAO.SelectDimOrgGroupType(organizationTO.OrgTypeId);
                    if (orgGroupTypeList == null || currencyList.Count == 0)
                    {
                        resultMessage.DefaultBehaviour("Failed to get orgGroupTypeList ");
                        return resultMessage;
                    }
                }
                
                if (Startup.CompanyObject == null)
                {
                    resultMessage.DefaultBehaviour("SAP Company Object Found NULL");
                    return resultMessage;
                }

                //Harshala [11-Dec-2019] If Master Entry is bank then call for update banks to SAP Master

                if (organizationTO.OrgTypeE == Constants.OrgTypeE.BANK)
                    return UpdateBankToSAP(organizationTO);

                SAPbobsCOM.BusinessPartners oBusinesspartner;
                oBusinesspartner = Startup.CompanyObject.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oBusinessPartners);

                bool getResult = oBusinesspartner.GetByKey(organizationTO.IdOrganization.ToString());
                if (!getResult)
                {
                    //Deepali Added [28-07-2021] to add org if not exists in SAP
                    resultMessage = SaveMasterToSAP(organizationTO);
                    if (resultMessage.Result != 1)
                    {
                        resultMessage.DefaultBehaviour("SaveMasterToSAP  Failed");
                        return resultMessage;
                    }
                    else
                    {
                        getResult = oBusinesspartner.GetByKey(organizationTO.IdOrganization.ToString());
                    }
                }

                #region General Information

                oBusinesspartner.CardCode = organizationTO.IdOrganization.ToString();
                oBusinesspartner.CardName = organizationTO.FirmName;
                oBusinesspartner.CardForeignName = organizationTO.FirmName;
                if (organizationTO.OrgTypeE == Constants.OrgTypeE.SUPPLIER)
                    oBusinesspartner.CardType = SAPbobsCOM.BoCardTypes.cSupplier;
                else if (organizationTO.OrgTypeE == Constants.OrgTypeE.DEALER)
                    oBusinesspartner.CardType = SAPbobsCOM.BoCardTypes.cCustomer;

                oBusinesspartner.Cellular = organizationTO.RegisteredMobileNos;

                oBusinesspartner.Notes = organizationTO.Remark;
                oBusinesspartner.FreeText = organizationTO.Remark;
                oBusinesspartner.AliasName = organizationTO.FirmName;
                oBusinesspartner.EmailAddress = organizationTO.EmailAddr;
                oBusinesspartner.Phone1 = organizationTO.PhoneNo;
                oBusinesspartner.Fax = organizationTO.FaxNo;
                oBusinesspartner.Website = organizationTO.Website;

                if(orgGroupTypeList != null && orgGroupTypeList.Count > 0)
                {
                    //Added By Harshala to save Group Code in SAP for  Vendor
                    var orgGrpTypeTO = orgGroupTypeList.Where(x => x.Value == organizationTO.OrgGroupTypeId).FirstOrDefault();
                    if (orgGrpTypeTO != null)
                        oBusinesspartner.GroupCode = Convert.ToInt32(orgGrpTypeTO.MappedTxnId);
                }
               

                var currencyTO = currencyList.Where(x => x.Value == organizationTO.CurrencyId).FirstOrDefault();
                if (currencyTO != null)
                    oBusinesspartner.Currency = currencyTO.Code;
                #endregion

                #region Address Details
                int index = 0;
                int AddressFound = 1;
                organizationTO.AddressList = organizationTO.AddressList.Where(g => g.IsDefault == 1 && g.IsActive == 1).ToList();
                if (organizationTO.AddressList != null && organizationTO.AddressList.Count > 0)
                {
                    for (int al = 0; al < organizationTO.AddressList.Count; al++)
                    {
                        if (al == 0)
                        {
                            if (string.IsNullOrEmpty(oBusinesspartner.Addresses.AddressName))
                            {
                                AddressFound = 0;
                            }
                        }
                        if (AddressFound == 1)
                            oBusinesspartner.Addresses.SetCurrentLine(index);
                        index++;
                        var MatchingTO = AddressTyleList.Where(x => x.Value == organizationTO.AddressList[al].AddrTypeId).FirstOrDefault();
                        if (MatchingTO != null)
                            oBusinesspartner.Addresses.AddressName = MatchingTO.Text;
                        else
                            oBusinesspartner.Addresses.AddressName = "Office Adress";
                        oBusinesspartner.Addresses.Block = organizationTO.AddressList[al].PlotNo;
                        oBusinesspartner.Addresses.Street = organizationTO.AddressList[al].StreetName;
                        oBusinesspartner.Addresses.StreetNo = organizationTO.AddressList[al].StreetName;


                        oBusinesspartner.Addresses.Country = organizationTO.AddressList[al].CountryCode;
                        oBusinesspartner.Addresses.State = organizationTO.AddressList[al].StateCode;
                        oBusinesspartner.Addresses.City = organizationTO.AddressList[al].DistrictName;
                        oBusinesspartner.Addresses.AddressName2 = organizationTO.AddressList[al].AreaName;
                        oBusinesspartner.Addresses.AddressType = SAPbobsCOM.BoAddressType.bo_BillTo;
                        oBusinesspartner.Addresses.AddressName3 = organizationTO.AddressList[al].VillageName;
                        oBusinesspartner.Addresses.ZipCode = organizationTO.AddressList[al].Pincode.ToString();

                        string gstinno = string.Empty;
                        string panNo = string.Empty;
                        Int32 gstTypeId = 0;
                        List<TblOrgLicenseDtlTO> TblOrgLicenseDtlTOList = new List<TblOrgLicenseDtlTO>();
                        if (IsLiscenseAgaintsAddress == 1)
                        {
                            TblOrgLicenseDtlTOList = organizationTO.AddressList[al].OrgLicenseDtlTOList;
                        }
                        else
                        {
                            TblOrgLicenseDtlTOList = organizationTO.OrgLicenseDtlTOList;
                        }
                        if (TblOrgLicenseDtlTOList != null && TblOrgLicenseDtlTOList.Count > 0)
                        {
                            gstinno = TblOrgLicenseDtlTOList.Where(x => x.LicenseId == (int)Constants.CommercialLicenseE.IGST_NO).FirstOrDefault().LicenseValue;
                            gstTypeId = TblOrgLicenseDtlTOList.Where(x => x.LicenseId == (int)Constants.CommercialLicenseE.IGST_NO).FirstOrDefault().GstTypeId;
                            panNo = TblOrgLicenseDtlTOList.Where(x => x.LicenseId == (int)Constants.CommercialLicenseE.PAN_NO).FirstOrDefault().LicenseValue;
                        }

                        if (gstinno != "0" && !string.IsNullOrEmpty(gstinno))
                        {
                            oBusinesspartner.Addresses.GSTIN = gstinno;
                            if (gstTypeId == 0)
                            {
                                gstTypeId = (Int32)SAPbobsCOM.BoGSTRegnTypeEnum.gstRegularTDSISD;
                            }
                            oBusinesspartner.Addresses.GstType = (SAPbobsCOM.BoGSTRegnTypeEnum)gstTypeId;

                            //oBusinesspartner.Addresses.GstType = SAPbobsCOM.BoGSTRegnTypeEnum.gstRegularTDSISD;
                        }

                        if (panNo != "0" && !string.IsNullOrEmpty(panNo))
                        {
                            oBusinesspartner.FiscalTaxID.SetCurrentLine(0);
                            oBusinesspartner.FiscalTaxID.TaxId0 = panNo;
                            oBusinesspartner.FiscalTaxID.Add();
                        }
                        if (AddressFound == 0)
                            oBusinesspartner.Addresses.Add();
                        if (AddressFound == 1)
                            oBusinesspartner.Addresses.SetCurrentLine(index);
                        index++;
                        var MatchingTOShipTO = AddressTyleList.Where(x => x.Value == organizationTO.AddressList[al].AddrTypeId).FirstOrDefault();
                        if (MatchingTOShipTO != null)
                            oBusinesspartner.Addresses.AddressName = MatchingTOShipTO.Text;
                        else
                            oBusinesspartner.Addresses.AddressName = "Office Adress";
                        oBusinesspartner.Addresses.Block = organizationTO.AddressList[al].PlotNo;
                        oBusinesspartner.Addresses.Street = organizationTO.AddressList[al].StreetName;
                        oBusinesspartner.Addresses.StreetNo = organizationTO.AddressList[al].StreetName;


                        oBusinesspartner.Addresses.Country = organizationTO.AddressList[al].CountryCode;
                        oBusinesspartner.Addresses.State = organizationTO.AddressList[al].StateCode;
                        oBusinesspartner.Addresses.City = organizationTO.AddressList[al].DistrictName;
                        oBusinesspartner.Addresses.AddressName2 = organizationTO.AddressList[al].AreaName;
                        oBusinesspartner.Addresses.AddressType = SAPbobsCOM.BoAddressType.bo_ShipTo;
                        oBusinesspartner.Addresses.AddressName3 = organizationTO.AddressList[al].VillageName;
                        oBusinesspartner.Addresses.ZipCode = organizationTO.AddressList[al].Pincode.ToString();

                        gstinno = string.Empty;
                        panNo = string.Empty;
                        TblOrgLicenseDtlTOList = new List<TblOrgLicenseDtlTO>();
                        if (IsLiscenseAgaintsAddress == 1)
                        {
                            TblOrgLicenseDtlTOList = organizationTO.AddressList[al].OrgLicenseDtlTOList;
                        }
                        else
                        {
                            TblOrgLicenseDtlTOList = organizationTO.OrgLicenseDtlTOList;
                        }
                        if (TblOrgLicenseDtlTOList != null && TblOrgLicenseDtlTOList.Count > 0)
                        {
                            gstinno = TblOrgLicenseDtlTOList.Where(x => x.LicenseId == (int)Constants.CommercialLicenseE.IGST_NO).FirstOrDefault().LicenseValue;
                            gstTypeId = TblOrgLicenseDtlTOList.Where(x => x.LicenseId == (int)Constants.CommercialLicenseE.IGST_NO).FirstOrDefault().GstTypeId;
                            panNo = TblOrgLicenseDtlTOList.Where(x => x.LicenseId == (int)Constants.CommercialLicenseE.PAN_NO).FirstOrDefault().LicenseValue;
                        }

                        if (gstinno != "0" && !string.IsNullOrEmpty(gstinno))
                        {
                            oBusinesspartner.Addresses.GSTIN = gstinno;

                            if (gstTypeId == 0)
                            {
                                gstTypeId = (Int32)SAPbobsCOM.BoGSTRegnTypeEnum.gstRegularTDSISD;
                            }
                            //oBusinesspartner.Addresses.GstType = SAPbobsCOM.BoGSTRegnTypeEnum.gstRegularTDSISD;
                            oBusinesspartner.Addresses.GstType = (SAPbobsCOM.BoGSTRegnTypeEnum)gstTypeId;

                        }

                        if (panNo != "0" && !string.IsNullOrEmpty(panNo))
                        {
                            oBusinesspartner.FiscalTaxID.SetCurrentLine(0);
                            oBusinesspartner.FiscalTaxID.TaxId0 = panNo;
                            oBusinesspartner.FiscalTaxID.Add();
                        }
                        if (AddressFound == 0)
                            oBusinesspartner.Addresses.Add();
                    }
                }
                else
                {
                    oBusinesspartner.Addresses.Delete();
                    oBusinesspartner.Addresses.Delete();
                }

                #endregion

                #region Contact Person Details
                SAPbobsCOM.ContactEmployees sboContacts = oBusinesspartner.ContactEmployees;
                if (sboContacts.Count > 0)
                {
                    
                }
                int indextemp = 0;
                for (int cp = 0; cp < organizationTO.PersonList.Count; cp++)
                {
                    Boolean isNewPerson = true;
                    if (sboContacts.Count > 0)
                    {
                        for (int i = 0; i < sboContacts.Count; i++)//Reshma Added For Supplier Update error of contct person details.
                        {
                            sboContacts.SetCurrentLine(i);
                            if (sboContacts.Name == organizationTO.PersonList[cp].FirstName + " " + organizationTO.PersonList[cp].LastName)
                            {
                                isNewPerson = false;
                                break;
                            }
                            else
                                isNewPerson = true;
                        }
                        if (isNewPerson)
                        {
                            oBusinesspartner.ContactEmployees.SetCurrentLine(sboContacts.Count-1);
                            oBusinesspartner.ContactEmployees.FirstName = organizationTO.PersonList[cp].FirstName;
                            oBusinesspartner.ContactEmployees.MiddleName = organizationTO.PersonList[cp].MidName;
                            oBusinesspartner.ContactEmployees.LastName = organizationTO.PersonList[cp].LastName;
                            oBusinesspartner.ContactEmployees.MobilePhone = organizationTO.PersonList[cp].MobileNo;
                            oBusinesspartner.ContactEmployees.Name = organizationTO.PersonList[cp].FirstName + " " + organizationTO.PersonList[cp].LastName;
                            if (organizationTO.PersonList[cp].SalutationId == (int)Constants.SalutationE.MR)
                                oBusinesspartner.ContactEmployees.Gender = SAPbobsCOM.BoGenderTypes.gt_Male;
                            else if (organizationTO.PersonList[cp].SalutationId == (int)Constants.SalutationE.MRS || organizationTO.PersonList[cp].SalutationId == (int)Constants.SalutationE.MISS)
                                oBusinesspartner.ContactEmployees.Gender = SAPbobsCOM.BoGenderTypes.gt_Female;

                            oBusinesspartner.ContactEmployees.DateOfBirth = organizationTO.PersonList[cp].DateOfBirth;
                            oBusinesspartner.ContactEmployees.E_Mail = organizationTO.PersonList[cp].PrimaryEmail;
                            oBusinesspartner.ContactEmployees.Add();
                            indextemp++;
                        }
                    }
                    else
                    {
                        oBusinesspartner.ContactEmployees.SetCurrentLine(cp);
                        oBusinesspartner.ContactEmployees.FirstName = organizationTO.PersonList[cp].FirstName;
                        oBusinesspartner.ContactEmployees.MiddleName = organizationTO.PersonList[cp].MidName;
                        oBusinesspartner.ContactEmployees.LastName = organizationTO.PersonList[cp].LastName;
                        oBusinesspartner.ContactEmployees.MobilePhone = organizationTO.PersonList[cp].MobileNo;
                        oBusinesspartner.ContactEmployees.Name = organizationTO.PersonList[cp].FirstName + " " + organizationTO.PersonList[cp].LastName;
                        if (organizationTO.PersonList[cp].SalutationId == (int)Constants.SalutationE.MR)
                            oBusinesspartner.ContactEmployees.Gender = SAPbobsCOM.BoGenderTypes.gt_Male;
                        else if (organizationTO.PersonList[cp].SalutationId == (int)Constants.SalutationE.MRS || organizationTO.PersonList[cp].SalutationId == (int)Constants.SalutationE.MISS)
                            oBusinesspartner.ContactEmployees.Gender = SAPbobsCOM.BoGenderTypes.gt_Female;

                        oBusinesspartner.ContactEmployees.DateOfBirth = organizationTO.PersonList[cp].DateOfBirth;
                        oBusinesspartner.ContactEmployees.E_Mail = organizationTO.PersonList[cp].PrimaryEmail;
                        oBusinesspartner.ContactEmployees.Add();
                        indextemp++;
                    }
                }

                #endregion

                //Added By Harshala[29/09/2020]
                #region Accounting Details  
                if (!String.IsNullOrEmpty(organizationTO.TblOrgAccountTaxTO.AccountPayableCode))
                {
                    oBusinesspartner.DebitorAccount = organizationTO.TblOrgAccountTaxTO.AccountPayableCode;
                    oBusinesspartner.DownPaymentInterimAccount = organizationTO.TblOrgAccountTaxTO.InterimAccountCode; // Account Code
                    oBusinesspartner.DownPaymentClearAct = organizationTO.TblOrgAccountTaxTO.ClearingAccountCode; // Account Code
                }
                //AmolG[2021-Jan-14] Commented Below code bcz it gives error when update the WTax
                //Shifted this code after update it.
                //if (organizationTO.TblOrgAccountTaxTO.SubjectToHoldingTax)
                //{
                //    oBusinesspartner.SubjectToWithholdingTax = BoYesNoEnum.tYES;

                //    if (organizationTO.TblOrgAccountTaxTO.ThreasholdOverlook)
                //        oBusinesspartner.ThresholdOverlook = BoYesNoEnum.tYES;
                //    else
                //        oBusinesspartner.ThresholdOverlook = BoYesNoEnum.tNO;

                //    if (organizationTO.TblOrgAccountTaxTO.SurchargeOverlook)
                //        oBusinesspartner.SurchargeOverlook = BoYesNoEnum.tYES;
                //    else
                //        oBusinesspartner.SurchargeOverlook = BoYesNoEnum.tNO;

                //    if (organizationTO.TblOrgAccountTaxTO.Accrual)
                //        oBusinesspartner.AccrualCriteria = BoYesNoEnum.tYES; // Yes for Accural and No for Cash
                //    else
                //        oBusinesspartner.AccrualCriteria = BoYesNoEnum.tNO;

                //    if (organizationTO.TblOrgAccountTaxTO.Cash)
                //        oBusinesspartner.AccrualCriteria = BoYesNoEnum.tNO;// Yes for Accural and No for Cash
                //    else
                //        oBusinesspartner.AccrualCriteria = BoYesNoEnum.tYES;
                //    if (organizationTO.TblOrgAccountTaxTO.TblOrgAccountTaxDtlsList != null && organizationTO.TblOrgAccountTaxTO.TblOrgAccountTaxDtlsList.Count > 0)
                //    {
                //        for (int i = 0; i < organizationTO.TblOrgAccountTaxTO.TblOrgAccountTaxDtlsList.Count; i++)
                //        {
                //            oBusinesspartner.BPWithholdingTax.SetCurrentLine(i);
                //            oBusinesspartner.BPWithholdingTax.WTCode = organizationTO.TblOrgAccountTaxTO.TblOrgAccountTaxDtlsList[i].WithholdTaxId.ToString();
                //            oBusinesspartner.BPWithholdingTax.Add();
                //        }
                //    }

                //    oBusinesspartner.ExpirationDate = organizationTO.TblOrgAccountTaxTO.CertificateExpiryDate;
                //    oBusinesspartner.CertificateNumber = organizationTO.TblOrgAccountTaxTO.CerificateNo;
                //    oBusinesspartner.NationalInsuranceNum = organizationTO.TblOrgAccountTaxTO.NINumber;
                //    //oBusinesspartner.TypeReport = (SAPbobsCOM.AssesseeTypeEnum)organizationTO.TblOrgAccountTaxTO.AssesseeTypeId;//commented by reshma
                //    oBusinesspartner.TypeReport = (SAPbobsCOM.AssesseeTypeEnum)Convert.ToInt16(organizationTO.TblOrgAccountTaxTO.MappedTxnId);// Assesse type
                    
                //}
                //else
                {
                    oBusinesspartner.SubjectToWithholdingTax = BoYesNoEnum.tNO;//Reshma[11-12-2020] For Update with holding tax
                }
                #endregion

                int result = oBusinesspartner.Update();
                if (result == 0)
                {
                    if (organizationTO.TblOrgAccountTaxTO.SubjectToHoldingTax)
                    {
                        oBusinesspartner = Startup.CompanyObject.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oBusinessPartners);

                        getResult = oBusinesspartner.GetByKey(organizationTO.IdOrganization.ToString());
                        if (!getResult)
                        {
                            resultMessage.DefaultBehaviour("Could not get Data From SAP");
                            return resultMessage;
                        }

                        oBusinesspartner.SubjectToWithholdingTax = BoYesNoEnum.tYES;

                        if (organizationTO.TblOrgAccountTaxTO.ThreasholdOverlook)
                            oBusinesspartner.ThresholdOverlook = BoYesNoEnum.tYES;
                        else
                            oBusinesspartner.ThresholdOverlook = BoYesNoEnum.tNO;

                        if (organizationTO.TblOrgAccountTaxTO.SurchargeOverlook)
                            oBusinesspartner.SurchargeOverlook = BoYesNoEnum.tYES;
                        else
                            oBusinesspartner.SurchargeOverlook = BoYesNoEnum.tNO;

                        if (organizationTO.TblOrgAccountTaxTO.Accrual)
                            oBusinesspartner.AccrualCriteria = BoYesNoEnum.tYES; // Yes for Accural and No for Cash
                        else
                            oBusinesspartner.AccrualCriteria = BoYesNoEnum.tNO;

                        if (organizationTO.TblOrgAccountTaxTO.Cash)
                            oBusinesspartner.AccrualCriteria = BoYesNoEnum.tNO;// Yes for Accural and No for Cash
                        else
                            oBusinesspartner.AccrualCriteria = BoYesNoEnum.tYES;

                        if (organizationTO.TblOrgAccountTaxTO.TblOrgAccountTaxDtlsList != null && organizationTO.TblOrgAccountTaxTO.TblOrgAccountTaxDtlsList.Count > 0)
                        {
                            for (int i = 0; i < organizationTO.TblOrgAccountTaxTO.TblOrgAccountTaxDtlsList.Count; i++)
                            {
                                if(organizationTO.TblOrgAccountTaxTO.TblOrgAccountTaxDtlsList[i].IsActive == true)
                                {
                                    oBusinesspartner.BPWithholdingTax.SetCurrentLine(i);
                                    oBusinesspartner.BPWithholdingTax.WTCode = organizationTO.TblOrgAccountTaxTO.TblOrgAccountTaxDtlsList[i].WithholdTaxId.ToString();
                                    oBusinesspartner.BPWithholdingTax.Add();
                                }
                              
                            }
                        }

                        oBusinesspartner.ExpirationDate = organizationTO.TblOrgAccountTaxTO.CertificateExpiryDate;
                        oBusinesspartner.CertificateNumber = organizationTO.TblOrgAccountTaxTO.CerificateNo;
                        oBusinesspartner.NationalInsuranceNum = organizationTO.TblOrgAccountTaxTO.NINumber;
                        //oBusinesspartner.TypeReport = (SAPbobsCOM.AssesseeTypeEnum)organizationTO.TblOrgAccountTaxTO.AssesseeTypeId;//commented by reshma
                        oBusinesspartner.TypeReport = (SAPbobsCOM.AssesseeTypeEnum)Convert.ToInt16(organizationTO.TblOrgAccountTaxTO.MappedTxnId);// Assesse type

                        result = oBusinesspartner.Update();
                        if (result == 0)
                        {
                            resultMessage.DefaultSuccessBehaviour();
                            return resultMessage;
                        }
                        else
                        {
                            string errorinfor = Startup.CompanyObject.GetLastErrorDescription();
                            resultMessage.DefaultBehaviour(errorinfor);
                            return resultMessage;
                        }
                    }
                    resultMessage.DefaultSuccessBehaviour();
                    return resultMessage;
                }
                else
                {
                    string errorinfor = Startup.CompanyObject.GetLastErrorDescription();
                    resultMessage.DefaultBehaviour(errorinfor);
                    return resultMessage;
                }

            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "UPDATEMasterToSAP");
                return resultMessage;
            }
        }

        #endregion

        #region Deletion
        public int DeleteTblOrganization(Int32 idOrganization)
        {
            return _iTblOrganizationDAO.DeleteTblOrganization(idOrganization);
        }

        public int DeleteTblOrganization(Int32 idOrganization, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblOrganizationDAO.DeleteTblOrganization(idOrganization, conn, tran);
        }

        #endregion

        public ResultMessage MigrateOrganizationData()
        {
            ResultMessage resultMessage = new ResultMessage();

            try
            {
                List<TblOrganizationTO> existingOrgList = _iTblOrganizationDAO.SelectAllTblOrganization();
                if(existingOrgList != null && existingOrgList.Count > 0)
                {

                    resultMessage = MigrateAndSaveOrgDetails(existingOrgList);
                    if(resultMessage.MessageType!= ResultMessageE.Information)
                    {
                        resultMessage.DefaultBehaviour();
                        return resultMessage;
                    }
                }

                resultMessage.DefaultSuccessBehaviour();
                return resultMessage;
            }
            catch (Exception ex)
            {

                resultMessage.DefaultExceptionBehaviour(ex, "Error in MigrateOrganizationData()");
                return resultMessage;
            }
         
        }

        public ResultMessage MigrateUserDetails()
        {
            ResultMessage resultMessage = new ResultMessage();
            Int32 result = 0;
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;

            try
            {
                

                List<TblUserTO> tblUserTOList = _iTblUserBL.SelectAllTblUserList(true, String.Empty);

                if (tblUserTOList != null && tblUserTOList.Count > 0)
                {
                    List<TblUserTO> TblUserTOduplicate = tblUserTOList.GroupBy(g => g.UserDisplayName).Select(s => s.FirstOrDefault()).ToList();

                    TblUserTOduplicate = TblUserTOduplicate.Where(w => w.UserDisplayName.TrimEnd().ToUpper() == "Swapnil Patil".TrimEnd().ToUpper()).ToList();

                    if (TblUserTOduplicate != null && TblUserTOduplicate.Count > 0)
                    {
                        for (int i = 0; i < TblUserTOduplicate.Count; i++)
                        {
                            String userDisplayName = TblUserTOduplicate[i].UserDisplayName;

                            List<TblUserTO> duplicateList = tblUserTOList.Where(s => s.UserDisplayName.TrimEnd().ToUpper() == userDisplayName.TrimEnd().ToUpper()).ToList();

                            if (duplicateList == null || duplicateList.Count <= 1)
                            {
                                continue;
                            }

                            if (duplicateList.Count != 2)
                            {
                                continue;
                            }


                            duplicateList = duplicateList.OrderBy(o => o.IdUser).ToList();
                            Int32 oldId = duplicateList[0].IdUser;

                            if (oldId > 1462)
                            { continue; }


                            conn.Open();
                            tran = conn.BeginTransaction();




                            for (int d = 1; d < duplicateList.Count; d++)
                            {
                                TblUserTO newUserTo = duplicateList[d];

                                //Update where existing
                                String updateQuery = String.Empty;

                                updateQuery = "UPDATE tbluserreportingdetails set isActive = " + 0 + " where userId = " + oldId;

                                result = _iTblOrganizationDAO.UpdateTblOrgId(updateQuery, conn, tran);
                                if (result == -1)
                                {
                                    throw new Exception("Error in " + updateQuery);
                                }

                                updateQuery = "UPDATE tbluserreportingdetails set userId = " + oldId + " where userId = " + newUserTo.IdUser;

                                result = _iTblOrganizationDAO.UpdateTblOrgId(updateQuery, conn, tran);
                                if (result == -1)
                                {
                                    throw new Exception("Error in " + updateQuery);
                                }

                                updateQuery = "UPDATE tbluserreportingdetails set reportingTo = " + oldId + " where reportingTo = " + newUserTo.IdUser + " AND isActive = 1";

                                result = _iTblOrganizationDAO.UpdateTblOrgId(updateQuery, conn, tran);
                                if (result == -1)
                                {
                                    throw new Exception("Error in " + updateQuery);
                                }

                                updateQuery = "UPDATE tbluser set isActive = " + 0 + " where iduser = " + newUserTo.IdUser;

                                result = _iTblOrganizationDAO.UpdateTblOrgId(updateQuery, conn, tran);
                                if (result == -1)
                                {
                                    throw new Exception("Error in " + updateQuery);
                                }

                                Int32 oldRoleId = 0;
                                Int32 oldRoleTypeId = 0;
                                List<TblUserRoleTO> UserRoleList = _iTblUserRoleBL.SelectAllActiveUserRoleList(oldId);
                                if (UserRoleList != null && UserRoleList.Count > 0)
                                {
                                    oldRoleId = UserRoleList[0].RoleId;
                                    oldRoleTypeId = UserRoleList[0].RoleTypeId;
                                }

                                Int32 newRoleId = 0;
                                Int32 newRoleTypeId = 0;

                                List <TblUserRoleTO> UserRoleListNew = _iTblUserRoleBL.SelectAllActiveUserRoleList(newUserTo.IdUser);
                                if (UserRoleListNew != null && UserRoleListNew.Count > 0)
                                {

                                    newRoleId = UserRoleListNew[0].RoleId;
                                    newRoleTypeId = UserRoleListNew[0].RoleTypeId;
                                }

                                if (newRoleId > 0 && oldRoleId > 0)
                                {
                                    CopyFromToPermissionTO copyFromToPermissionTO = new CopyFromToPermissionTO();
                                    copyFromToPermissionTO.RoleUserFromId = oldRoleId;
                                    copyFromToPermissionTO.RoleUserToId = newRoleId;
                                    copyFromToPermissionTO.IdFrom = (int)Constants.PermissionTypeE.Role;
                                    copyFromToPermissionTO.IdTo = (int)Constants.PermissionTypeE.Role;
                                    copyFromToPermissionTO.CreatedBy = 1;
                                    copyFromToPermissionTO.CreatedOn = _iCommon.ServerDateTime;
                                    ResultMessage resMsg = _iTblSysElementsBL.SaveCopyPermissions(copyFromToPermissionTO);
                                    if (resMsg == null || resMsg.MessageType != ResultMessageE.Information)
                                    {

                                    }
                                }

                                updateQuery = "UPDATE tbluserrole set isActive = " + 0 + " where userId = " + oldId;

                                result = _iTblOrganizationDAO.UpdateTblOrgId(updateQuery, conn, tran);
                                if (result == -1)
                                {
                                    throw new Exception("Error in " + updateQuery);
                                }

                                updateQuery = "UPDATE tbluserrole set userId = " + oldId + " where userId = " + newUserTo.IdUser;

                                result = _iTblOrganizationDAO.UpdateTblOrgId(updateQuery, conn, tran);
                                if (result == -1)
                                {
                                    throw new Exception("Error in " + updateQuery);
                                }

                                if (oldRoleTypeId > 1)
                                {
                                    updateQuery = "UPDATE tblrole set roleTypeId = " + oldRoleTypeId + " where idRole = " + newRoleId;

                                    result = _iTblOrganizationDAO.UpdateTblOrgId(updateQuery, conn, tran);
                                    if (result == -1)
                                    {
                                        throw new Exception("Error in " + updateQuery);
                                    }
                                }




                            }

                            tran.Commit();

                            conn.Close();
                        }
                    }

                }


                resultMessage.DefaultSuccessBehaviour();
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "Error in MigrateOrganizationData()");
                return resultMessage;
            }
            finally
            { conn.Close(); }

        }



        public ResultMessage  MigrateAndSaveOrgDetails(List<TblOrganizationTO> existingOrgList)
        {
            ResultMessage resultMessage = new ResultMessage();
            Int32 result = 0;
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;

            try
            {

                if (existingOrgList != null && existingOrgList.Count > 0)
                {
                    List<int> ids = new List<int>();

                    #region

                    //ids.Add(25061);
                    //ids.Add(25062);
                    //ids.Add(25063);
                    //ids.Add(25064);
                    //ids.Add(25065);
                    //ids.Add(25078);
                    //ids.Add(25079);
                    //ids.Add(25080);
                    //ids.Add(25081);
                    //ids.Add(25082);
                    //ids.Add(25083);
                    //ids.Add(25084);
                    //ids.Add(25085);
                    //ids.Add(25086);
                    //ids.Add(25087);
                    //ids.Add(25088);
                    //ids.Add(25089);
                    //ids.Add(25090);
                    //ids.Add(25091);
                    //ids.Add(25092);
                    //ids.Add(25093);
                    //ids.Add(25094);
                    //ids.Add(25095);
                    //ids.Add(25096);
                    //ids.Add(25097);
                    //ids.Add(25098);
                    //ids.Add(25099);
                    //ids.Add(25100);
                    //ids.Add(25101);
                    //ids.Add(25102);
                    //ids.Add(25103);
                    //ids.Add(25104);
                    //ids.Add(25105);
                    //ids.Add(25106);
                    //ids.Add(25107);
                    //ids.Add(25108);
                    //ids.Add(25109);
                    //ids.Add(25110);
                    //ids.Add(25111);
                    //ids.Add(25112);
                    //ids.Add(25113);
                    //ids.Add(25114);
                    //ids.Add(25115);
                    //ids.Add(25116);
                    //ids.Add(25117);
                    //ids.Add(25118);
                    //ids.Add(25119);
                    //ids.Add(25120);
                    //ids.Add(25121);
                    //ids.Add(25122);
                    //ids.Add(25123);
                    //ids.Add(25124);
                    //ids.Add(25125);
                    //ids.Add(25126);
                    //ids.Add(25127);
                    //ids.Add(25128);
                    //ids.Add(25129);
                    //ids.Add(25130);
                    //ids.Add(25131);
                    //ids.Add(25132);
                    //ids.Add(25133);
                    //ids.Add(25134);
                    //ids.Add(25135);
                    //ids.Add(25136);
                    //ids.Add(25137);
                    //ids.Add(25138);
                    //ids.Add(25139);
                    //ids.Add(25140);
                    //ids.Add(25141);
                    //ids.Add(25142);
                    //ids.Add(25143);
                    //ids.Add(25144);
                    //ids.Add(25145);
                    //ids.Add(25146);
                    //ids.Add(25147);
                    //ids.Add(25148);
                    //ids.Add(25149);
                    //ids.Add(25150);
                    //ids.Add(25151);
                    //ids.Add(25152);
                    //ids.Add(25153);
                    //ids.Add(25154);
                    //ids.Add(26115);
                    //ids.Add(26116);
                    //ids.Add(26117);
                    //ids.Add(26118);
                    //ids.Add(26119);
                    //ids.Add(26120);
                    //ids.Add(26121);
                    //ids.Add(26122);
                    //ids.Add(26123);
                    //ids.Add(26124);
                    //ids.Add(26125);
                    //ids.Add(26126);
                    //ids.Add(26127);
                    //ids.Add(26128);
                    //ids.Add(26129);
                    //ids.Add(26130);
                    //ids.Add(26131);
                    //ids.Add(26132);
                    //ids.Add(26133);
                    //ids.Add(26134);
                    //ids.Add(26135);
                    //ids.Add(26136);
                    //ids.Add(26137);
                    //ids.Add(26138);
                    //ids.Add(26139);
                    //ids.Add(26151);
                    //ids.Add(26152);
                    //ids.Add(26153);
                    //ids.Add(26154);
                    //ids.Add(26155);
                    //ids.Add(26156);
                    //ids.Add(26157);
                    //ids.Add(26158);
                    //ids.Add(26159);
                    //ids.Add(26160);
                    //ids.Add(26161);
                    //ids.Add(26162);
                    //ids.Add(26163);
                    //ids.Add(26164);
                    //ids.Add(26165);
                    //ids.Add(26166);
                    //ids.Add(26167);
                    //ids.Add(26168);
                    //ids.Add(26169);
                    //ids.Add(26170);
                    //ids.Add(26171);
                    //ids.Add(26172);
                    //ids.Add(26173);
                    //ids.Add(26174);
                    //ids.Add(26175);
                    //ids.Add(26176);
                    //ids.Add(26177);
                    //ids.Add(26178);
                    //ids.Add(26179);
                    //ids.Add(26180);
                    //ids.Add(26181);
                    //ids.Add(26182);
                    //ids.Add(26183);
                    //ids.Add(26184);
                    //ids.Add(26185);
                    //ids.Add(26186);
                    //ids.Add(26187);
                    //ids.Add(26188);
                    //ids.Add(26189);
                    //ids.Add(26190);
                    //ids.Add(26191);
                    //ids.Add(26192);
                    //ids.Add(26193);
                    //ids.Add(27140);
                    //ids.Add(27141);
                    //ids.Add(27142);
                    //ids.Add(27143);
                    //ids.Add(27144);
                    //ids.Add(27145);
                    //ids.Add(27146);
                    //ids.Add(27147);
                    //ids.Add(27148);
                    //ids.Add(27149);
                    //ids.Add(27150);
                    //ids.Add(27151);
                    //ids.Add(27152);
                    //ids.Add(27153);
                    //ids.Add(27154);
                    //ids.Add(27155);
                    //ids.Add(27156);
                    //ids.Add(27157);
                    //ids.Add(27158);
                    //ids.Add(27159);
                    //ids.Add(28150);
                    //ids.Add(28151);
                    //ids.Add(28152);
                    //ids.Add(28153);
                    //ids.Add(28154);
                    //ids.Add(28155);
                    //ids.Add(28156);
                    //ids.Add(28157);
                    //ids.Add(28158);
                    //ids.Add(28159);
                    //ids.Add(28160);
                    //ids.Add(28161);
                    //ids.Add(28162);
                    //ids.Add(28163);
                    //ids.Add(28164);
                    //ids.Add(28165);
                    //ids.Add(28166);
                    //ids.Add(28167);
                    //ids.Add(28168);
                    //ids.Add(28169);
                    //ids.Add(28170);
                    //ids.Add(28171);
                    //ids.Add(28172);
                    //ids.Add(28173);
                    //ids.Add(28174);
                    //ids.Add(28175);
                    //ids.Add(28176);
                    //ids.Add(28177);
                    //ids.Add(28178);
                    //ids.Add(28179);
                    //ids.Add(28180);
                    //ids.Add(28181);
                    //ids.Add(28182);
                    //ids.Add(28183);
                    //ids.Add(28184);
                    //ids.Add(28185);
                    //ids.Add(28186);
                    //ids.Add(28187);
                    //ids.Add(28188);
                    //ids.Add(28189);
                    //ids.Add(28190);
                    //ids.Add(28193);
                    //ids.Add(28194);
                    //ids.Add(28195);
                    //ids.Add(28196);
                    //ids.Add(28197);
                    //ids.Add(28198);
                    //ids.Add(28199);
                    //ids.Add(28200);
                    //ids.Add(28201);
                    //ids.Add(28202);
                    //ids.Add(28203);
                    //ids.Add(28204);
                    //ids.Add(28205);
                    //ids.Add(28206);
                    //ids.Add(28207);
                    //ids.Add(28208);
                    //ids.Add(28209);
                    //ids.Add(28210);
                    //ids.Add(28211);
                    //ids.Add(28212);
                    //ids.Add(28213);
                    //ids.Add(28214);
                    //ids.Add(28215);
                    //ids.Add(28216);
                    //ids.Add(28217);
                    //ids.Add(28218);
                    //ids.Add(28219);
                    //ids.Add(28220);
                    //ids.Add(28221);
                    //ids.Add(28222);
                    //ids.Add(28223);
                    //ids.Add(28224);
                    //ids.Add(28225);
                    //ids.Add(28226);
                    //ids.Add(28227);
                    //ids.Add(28228);
                    //ids.Add(28229);
                    //ids.Add(28230);
                    //ids.Add(28231);
                    //ids.Add(28232);
                    //ids.Add(28233);
                    //ids.Add(28234);
                    //ids.Add(28235);
                    //ids.Add(28236);
                    //ids.Add(28237);
                    //ids.Add(29237);
                    //ids.Add(29238);
                    //ids.Add(29239);
                    //ids.Add(29240);
                    //ids.Add(30237);
                    //ids.Add(30238);
                    //ids.Add(30239);
                    //ids.Add(30240);
                    //ids.Add(30241);
                    //ids.Add(30242);
                    //ids.Add(30243);
                    //ids.Add(30244);
                    //ids.Add(30245);
                    //ids.Add(30246);
                    //ids.Add(30247);
                    //ids.Add(30248);
                    //ids.Add(30249);
                    //ids.Add(30250);
                    //ids.Add(30251);
                    //ids.Add(30252);
                    //ids.Add(30253);
                    //ids.Add(30254);
                    //ids.Add(30255);
                    //ids.Add(30256);
                    //ids.Add(30257);
                    //ids.Add(30258);
                    //ids.Add(30259);
                    //ids.Add(30263);
                    //ids.Add(30264);
                    //ids.Add(30265);
                    //ids.Add(30266);
                    //ids.Add(30267);
                    //ids.Add(30268);
                    //ids.Add(30269);
                    //ids.Add(30270);
                    //ids.Add(31251);
                    //ids.Add(31257);
                    //ids.Add(32252);
                    //ids.Add(32253);
                    //ids.Add(33253);
                    //ids.Add(33254);
                    //ids.Add(33255);
                    //ids.Add(33256);
                    //ids.Add(34256);
                    //ids.Add(35256);
                    //ids.Add(36256);
                    //ids.Add(37256);
                    //ids.Add(38256);
                    //ids.Add(39256);
                    //ids.Add(39257);
                    //ids.Add(40256);
                    //ids.Add(40260);
                    //ids.Add(41256);
                    //ids.Add(42256);
                    //ids.Add(42257);
                    //ids.Add(43256);
                    //ids.Add(44256);
                    //ids.Add(45257);
                    //ids.Add(45258);
                    //ids.Add(45259);
                    //ids.Add(46256);
                    //ids.Add(46257);
                    //ids.Add(46258);
                    //ids.Add(46259);
                    //ids.Add(46260);
                    //ids.Add(46262);
                    //ids.Add(46264);
                    //ids.Add(46265);
                    //ids.Add(46268);
                    //ids.Add(46276);
                    //ids.Add(46283);
                    //ids.Add(46285);
                    //ids.Add(46286);
                    //ids.Add(46287);
                    //ids.Add(46298);
                    //ids.Add(46300);
                    //ids.Add(46301);
                    //ids.Add(46302);
                    //ids.Add(46303);
                    //ids.Add(46307);
                    //ids.Add(46308);
                    //ids.Add(46309);
                    //ids.Add(46310);
                    //ids.Add(46311);
                    //ids.Add(46312);
                    //ids.Add(46313);
                    //ids.Add(46316);
                    //ids.Add(46320);
                    //ids.Add(46321);
                    //ids.Add(46322);
                    //ids.Add(46324);
                    //ids.Add(46325);
                    //ids.Add(46334);
                    //ids.Add(46335);
                    //ids.Add(46336);
                    //ids.Add(46337);
                    //ids.Add(46338);
                    //ids.Add(46339);
                    //ids.Add(46340);
                    //ids.Add(46341);
                    //ids.Add(46342);
                    //ids.Add(46343);
                    //ids.Add(46344);
                    //ids.Add(46345);
                    //ids.Add(46346);
                    //ids.Add(46347);
                    //ids.Add(46348);
                    //ids.Add(46349);
                    //ids.Add(46350);
                    //ids.Add(46351);
                    //ids.Add(47358);
                    //ids.Add(47359);
                    //ids.Add(47360);
                    //ids.Add(47361);
                    //ids.Add(47362);
                    //ids.Add(47364);
                    //ids.Add(47365);
                    //ids.Add(47366);
                    //ids.Add(47367);
                    //ids.Add(47368);
                    //ids.Add(47369);
                    //ids.Add(47370);
                    //ids.Add(47371);
                    //ids.Add(47372);
                    //ids.Add(47373);
                    //ids.Add(47374);
                    //ids.Add(47375);
                    //ids.Add(47376);
                    //ids.Add(47377);
                    //ids.Add(47378);
                    //ids.Add(47379);
                    //ids.Add(47380);
                    //ids.Add(47381);
                    //ids.Add(47382);
                    //ids.Add(47383);
                    //ids.Add(47384);
                    //ids.Add(47385);
                    //ids.Add(47386);
                    //ids.Add(47387);
                    //ids.Add(47388);
                    //ids.Add(47389);
                    //ids.Add(47390);
                    //ids.Add(47391);
                    //ids.Add(47392);
                    //ids.Add(47393);
                    //ids.Add(47394);
                    //ids.Add(47395);
                    //ids.Add(48385);
                    //ids.Add(48386);
                    //ids.Add(48387);
                    //ids.Add(48388);
                    //ids.Add(48389);
                    //ids.Add(48390);
                    //ids.Add(48391);
                    //ids.Add(48392);
                    //ids.Add(48393);
                    //ids.Add(48394);
                    //ids.Add(48395);
                    //ids.Add(48396);
                    //ids.Add(48397);
                    //ids.Add(48398);
                    //ids.Add(48399);
                    //ids.Add(48400);
                    //ids.Add(48401);
                    //ids.Add(48402);
                    //ids.Add(48403);
                    //ids.Add(48404);
                    //ids.Add(48405);
                    //ids.Add(48406);
                    //ids.Add(48407);
                    //ids.Add(48408);
                    //ids.Add(48409);
                    //ids.Add(48410);
                    //ids.Add(48411);
                    //ids.Add(48412);
                    //ids.Add(48413);
                    //ids.Add(48414);
                    //ids.Add(48415);
                    //ids.Add(48416);
                    //ids.Add(48417);
                    //ids.Add(48418);
                    //ids.Add(48419);
                    //ids.Add(48420);
                    //ids.Add(48421);
                    //ids.Add(48422);
                    //ids.Add(48423);
                    //ids.Add(48424);
                    //ids.Add(48425);
                    //ids.Add(48426);
                    //ids.Add(48427);
                    //ids.Add(48428);
                    //ids.Add(48429);
                    //ids.Add(48430);
                    //ids.Add(48431);
                    //ids.Add(48432);
                    //ids.Add(48433);
                    //ids.Add(48434);
                    //ids.Add(48435);
                    //ids.Add(48439);
                    //ids.Add(48440);
                    //ids.Add(48441);
                    //ids.Add(48442);
                    //ids.Add(48443);
                    //ids.Add(48444);
                    //ids.Add(48445);
                    //ids.Add(48446);
                    //ids.Add(48447);
                    //ids.Add(48448);
                    //ids.Add(48449);
                    //ids.Add(48450);
                    //ids.Add(48451);
                    //ids.Add(48452);
                    //ids.Add(48453);
                    //ids.Add(48454);
                    //ids.Add(48455);
                    //ids.Add(48464);
                    //ids.Add(48465);
                    //ids.Add(48467);
                    //ids.Add(48468);
                    //ids.Add(48469);
                    //ids.Add(48477);
                    //ids.Add(48478);
                    //ids.Add(48479);
                    //ids.Add(48480);
                    //ids.Add(48481);
                    //ids.Add(48482);
                    //ids.Add(48483);
                    //ids.Add(48484);
                    //ids.Add(48485);
                    //ids.Add(48486);
                    //ids.Add(48487);
                    //ids.Add(48488);
                    //ids.Add(48489);
                    //ids.Add(48490);
                    //ids.Add(48491);
                    //ids.Add(48492);
                    //ids.Add(48493);
                    //ids.Add(48494);
                    //ids.Add(48495);
                    //ids.Add(48496);
                    //ids.Add(48497);
                    //ids.Add(48498);
                    //ids.Add(48499);
                    //ids.Add(48500);
                    //ids.Add(48501);

                    ids.Add(48472);
                    ids.Add(48473);
                    ids.Add(48474);
                    ids.Add(48475);
                    ids.Add(48476);
                    ids.Add(48466);
                    ids.Add(48456);
                    ids.Add(48463);
                    ids.Add(45256);

                    #endregion

                    for (int i = 0; i < ids.Count; i++)
                    {

                        try
                        {
                            conn.Open();
                            tran = conn.BeginTransaction();

                            TblOrganizationTO existingTO = existingOrgList.Where(a=>a.IdOrganization == ids[i]).FirstOrDefault();
                            if(existingTO == null)
                            {
                                continue;
                            }

                            //Insert New Organization
                            TblOrganizationTO newOrgTO = existingTO.DeepCopy();
                            newOrgTO.Remark = existingTO.IdOrganization.ToString();

                            result = _iTblOrganizationDAO.InsertTblOrganization(newOrgTO, conn, tran);
                            if (result != 1)
                            {
                                throw new Exception("Error in InsertTblOrganization(newOrgTO, conn, tran);");
                            }

                            //Update where existing
                            String updateQuery = String.Empty;

                            updateQuery = "update finalInvoice set dealerOrgId = " + newOrgTO.IdOrganization + " where dealerOrgId = " + existingTO.IdOrganization;

                            result = _iTblOrganizationDAO.UpdateTblOrgId(updateQuery, conn, tran);
                            if (result == -1)
                            {
                                throw new Exception("Error in " + updateQuery);
                            }

                            #region finalInvoice
                            updateQuery = "update finalInvoice set distributorOrgId = " + newOrgTO.IdOrganization + " where distributorOrgId = " + existingTO.IdOrganization;

                            result = _iTblOrganizationDAO.UpdateTblOrgId(updateQuery, conn, tran);
                            if (result == -1)
                            {
                                throw new Exception("Error in " + updateQuery);
                            }

                            updateQuery = "update finalInvoice set transportOrgId = " + newOrgTO.IdOrganization + " where transportOrgId = " + existingTO.IdOrganization;

                            result = _iTblOrganizationDAO.UpdateTblOrgId(updateQuery, conn, tran);
                            if (result == -1)
                            {
                                throw new Exception("Error in " + updateQuery);
                            }

                            #endregion

                            #region tempInvoice
                            updateQuery = "update tempInvoice set dealerOrgId = " + newOrgTO.IdOrganization + " where dealerOrgId = " + existingTO.IdOrganization;

                            result = _iTblOrganizationDAO.UpdateTblOrgId(updateQuery, conn, tran);
                            if (result == -1)
                            {
                                throw new Exception("Error in " + updateQuery);
                            }

                            updateQuery = "update tempInvoice set distributorOrgId = " + newOrgTO.IdOrganization + " where distributorOrgId = " + existingTO.IdOrganization;

                            result = _iTblOrganizationDAO.UpdateTblOrgId(updateQuery, conn, tran);
                            if (result == -1)
                            {
                                throw new Exception("Error in " + updateQuery);
                            }

                            updateQuery = "update tempInvoice set transportOrgId = " + newOrgTO.IdOrganization + " where transportOrgId = " + existingTO.IdOrganization;

                            result = _iTblOrganizationDAO.UpdateTblOrgId(updateQuery, conn, tran);
                            if (result == -1)
                            {
                                throw new Exception("Error in " + updateQuery);
                            }

                            #endregion

                            #region finalInvoiceAddress

                            updateQuery = "update finalInvoiceAddress set billingOrgId = " + newOrgTO.IdOrganization + " where billingOrgId = " + existingTO.IdOrganization;

                            result = _iTblOrganizationDAO.UpdateTblOrgId(updateQuery, conn, tran);
                            if (result == -1)
                            {
                                throw new Exception("Error in " + updateQuery);
                            }

                            updateQuery = "update tempInvoiceAddress set billingOrgId = " + newOrgTO.IdOrganization + " where billingOrgId = " + existingTO.IdOrganization;

                            result = _iTblOrganizationDAO.UpdateTblOrgId(updateQuery, conn, tran);
                            if (result == -1)
                            {
                                throw new Exception("Error in " + updateQuery);
                            }


                            #endregion

                            updateQuery = "update finalLoading set cnfOrgId = " + newOrgTO.IdOrganization + " where cnfOrgId = " + existingTO.IdOrganization;

                            result = _iTblOrganizationDAO.UpdateTblOrgId(updateQuery, conn, tran);
                            if (result == -1)
                            {
                                throw new Exception("Error in " + updateQuery);
                            }

                            updateQuery = "update tempLoading set cnfOrgId = " + newOrgTO.IdOrganization + " where cnfOrgId = " + existingTO.IdOrganization;

                            result = _iTblOrganizationDAO.UpdateTblOrgId(updateQuery, conn, tran);
                            if (result == -1)
                            {
                                throw new Exception("Error in " + updateQuery);
                            }

                            updateQuery = "update finalLoading set transporterOrgId = " + newOrgTO.IdOrganization + " where transporterOrgId = " + existingTO.IdOrganization;

                            result = _iTblOrganizationDAO.UpdateTblOrgId(updateQuery, conn, tran);
                            if (result == -1)
                            {
                                throw new Exception("Error in " + updateQuery);
                            }

                            updateQuery = "update tempLoading set transporterOrgId = " + newOrgTO.IdOrganization + " where transporterOrgId = " + existingTO.IdOrganization;

                            result = _iTblOrganizationDAO.UpdateTblOrgId(updateQuery, conn, tran);
                            if (result == -1)
                            {
                                throw new Exception("Error in " + updateQuery);
                            }

                            updateQuery = "update tempLoadingSlip set dealerOrgId = " + newOrgTO.IdOrganization + " where dealerOrgId = " + existingTO.IdOrganization;

                            result = _iTblOrganizationDAO.UpdateTblOrgId(updateQuery, conn, tran);
                            if (result == -1)
                            {
                                throw new Exception("Error in " + updateQuery);
                            }

                            updateQuery = "update finalLoadingSlip set dealerOrgId = " + newOrgTO.IdOrganization + " where dealerOrgId = " + existingTO.IdOrganization;

                            result = _iTblOrganizationDAO.UpdateTblOrgId(updateQuery, conn, tran);
                            if (result == -1)
                            {
                                throw new Exception("Error in " + updateQuery);
                            }

                            updateQuery = "update tblBookings set cnfOrgId = " + newOrgTO.IdOrganization + " where cnfOrgId = " + existingTO.IdOrganization;

                            result = _iTblOrganizationDAO.UpdateTblOrgId(updateQuery, conn, tran);
                            if (result == -1)
                            {
                                throw new Exception("Error in " + updateQuery);
                            }

                            updateQuery = "update tblBookings set dealerOrgId = " + newOrgTO.IdOrganization + " where dealerOrgId = " + existingTO.IdOrganization;

                            result = _iTblOrganizationDAO.UpdateTblOrgId(updateQuery, conn, tran);
                            if (result == -1)
                            {
                                throw new Exception("Error in " + updateQuery);
                            }

                            updateQuery = "update tblKYCDetails SET organizationId = " + newOrgTO.IdOrganization + " where organizationId = " + existingTO.IdOrganization;

                            result = _iTblOrganizationDAO.UpdateTblOrgId(updateQuery, conn, tran);
                            if (result == -1)
                            {
                                throw new Exception("Error in " + updateQuery);
                            }

                            updateQuery = "update tblLoadingQuotaConfig SET cnfOrgId = " + newOrgTO.IdOrganization + " where cnfOrgId = " + existingTO.IdOrganization;

                            result = _iTblOrganizationDAO.UpdateTblOrgId(updateQuery, conn, tran);
                            if (result == -1)
                            {
                                throw new Exception("Error in " + updateQuery);
                            }

                            updateQuery = "update tblOrgAddress SET organizationId = " + newOrgTO.IdOrganization + " where organizationId = " + existingTO.IdOrganization;

                            result = _iTblOrganizationDAO.UpdateTblOrgId(updateQuery, conn, tran);
                            if (result == -1)
                            {
                                throw new Exception("Error in " + updateQuery);
                            }

                            updateQuery = "update tblOrgLicenseDtl SET organizationId = " + newOrgTO.IdOrganization + " where organizationId = " + existingTO.IdOrganization;

                            result = _iTblOrganizationDAO.UpdateTblOrgId(updateQuery, conn, tran);
                            if (result == -1)
                            {
                                throw new Exception("Error in " + updateQuery);
                            }

                            updateQuery = "update tblOrgPersonDtls SET organizationId = " + newOrgTO.IdOrganization + " where organizationId = " + existingTO.IdOrganization;

                            result = _iTblOrganizationDAO.UpdateTblOrgId(updateQuery, conn, tran);
                            if (result == -1)
                            {
                                throw new Exception("Error in " + updateQuery);
                            }

                            updateQuery = "update tblPurchaseEnquiry SET SupplierId = " + newOrgTO.IdOrganization + " where SupplierId = " + existingTO.IdOrganization;

                            result = _iTblOrganizationDAO.UpdateTblOrgId(updateQuery, conn, tran);
                            if (result == -1)
                            {
                                throw new Exception("Error in " + updateQuery);
                            }

                            updateQuery = "update tblPurchaseInvoice SET SupplierId = " + newOrgTO.IdOrganization + " where SupplierId = " + existingTO.IdOrganization;

                            result = _iTblOrganizationDAO.UpdateTblOrgId(updateQuery, conn, tran);
                            if (result == -1)
                            {
                                throw new Exception("Error in " + updateQuery);
                            }

                            updateQuery = "update tblPurchaseInvoice SET brokerId = " + newOrgTO.IdOrganization + " where brokerId = " + existingTO.IdOrganization;

                            result = _iTblOrganizationDAO.UpdateTblOrgId(updateQuery, conn, tran);
                            if (result == -1)
                            {
                                throw new Exception("Error in " + updateQuery);
                            }

                            updateQuery = "update tblPurchaseInvoice SET transportOrgId = " + newOrgTO.IdOrganization + " where transportOrgId = " + existingTO.IdOrganization;

                            result = _iTblOrganizationDAO.UpdateTblOrgId(updateQuery, conn, tran);
                            if (result == -1)
                            {
                                throw new Exception("Error in " + updateQuery);
                            }

                            updateQuery = "update tblPurchaseInvoiceAddr SET billingPartyOrgId = " + newOrgTO.IdOrganization + " where billingPartyOrgId = " + existingTO.IdOrganization;

                            result = _iTblOrganizationDAO.UpdateTblOrgId(updateQuery, conn, tran);
                            if (result == -1)
                            {
                                throw new Exception("Error in " + updateQuery);
                            }

                            updateQuery = "update tblPurchaseManagerSupplier SET organizationId = " + newOrgTO.IdOrganization + " where organizationId = " + existingTO.IdOrganization;

                            result = _iTblOrganizationDAO.UpdateTblOrgId(updateQuery, conn, tran);
                            if (result == -1)
                            {
                                throw new Exception("Error in " + updateQuery);
                            }

                            updateQuery = "update tblPurchaseScheduleSummary SET SupplierId = " + newOrgTO.IdOrganization + " where SupplierId = " + existingTO.IdOrganization;

                            result = _iTblOrganizationDAO.UpdateTblOrgId(updateQuery, conn, tran);
                            if (result == -1)
                            {
                                throw new Exception("Error in " + updateQuery);
                            }

                            updateQuery = "update tblPurchaseVehicleSpotEntry SET SupplierId = " + newOrgTO.IdOrganization + " where SupplierId = " + existingTO.IdOrganization;

                            result = _iTblOrganizationDAO.UpdateTblOrgId(updateQuery, conn, tran);
                            if (result == -1)
                            {
                                throw new Exception("Error in " + updateQuery);
                            }

                            updateQuery = "update tblPurchaseWeighingStageSummary SET SupplierId = " + newOrgTO.IdOrganization + " where SupplierId = " + existingTO.IdOrganization;

                            result = _iTblOrganizationDAO.UpdateTblOrgId(updateQuery, conn, tran);
                            if (result == -1)
                            {
                                throw new Exception("Error in " + updateQuery);
                            }

                            updateQuery = "update tblUnLoading SET SupplierOrgId = " + newOrgTO.IdOrganization + " where SupplierOrgId = " + existingTO.IdOrganization;

                            result = _iTblOrganizationDAO.UpdateTblOrgId(updateQuery, conn, tran);
                            if (result == -1)
                            {
                                throw new Exception("Error in " + updateQuery);
                            }

                            updateQuery = "update tblUserAreaAllocation SET cnfOrgId = " + newOrgTO.IdOrganization + " where cnfOrgId = " + existingTO.IdOrganization;

                            result = _iTblOrganizationDAO.UpdateTblOrgId(updateQuery, conn, tran);
                            if (result == -1)
                            {
                                throw new Exception("Error in " + updateQuery);
                            }

                            updateQuery = "update tblCnfDealers SET cnfOrgId = " + newOrgTO.IdOrganization + " where cnfOrgId = " + existingTO.IdOrganization;

                            result = _iTblOrganizationDAO.UpdateTblOrgId(updateQuery, conn, tran);
                            if (result == -1)
                            {
                                throw new Exception("Error in " + updateQuery);
                            }

                            updateQuery = "update tblCnfDealers SET dealerOrgId = " + newOrgTO.IdOrganization + " where dealerOrgId = " + existingTO.IdOrganization;

                            result = _iTblOrganizationDAO.UpdateTblOrgId(updateQuery, conn, tran);
                            if (result == -1)
                            {
                                throw new Exception("Error in " + updateQuery);
                            }

                            updateQuery = "update tblCommercialDocAddrDtls SET billingPartyOrgId = " + newOrgTO.IdOrganization + " where billingPartyOrgId =  " + existingTO.IdOrganization;

                            result = _iTblOrganizationDAO.UpdateTblOrgId(updateQuery, conn, tran);
                            if (result == -1)
                            {
                                throw new Exception("Error in " + updateQuery);
                            }

                            updateQuery = " update tblCompetitorUpdates SET dealerId = " + newOrgTO.IdOrganization + " where dealerId =  " + existingTO.IdOrganization;

                            result = _iTblOrganizationDAO.UpdateTblOrgId(updateQuery, conn, tran);
                            if (result == -1)
                            {
                                throw new Exception("Error in " + updateQuery);
                            }

                            updateQuery = "update tblCompetitorUpdates SET competitorOrgId  = " + newOrgTO.IdOrganization + " where competitorOrgId  =  " + existingTO.IdOrganization;

                            result = _iTblOrganizationDAO.UpdateTblOrgId(updateQuery, conn, tran);
                            if (result == -1)
                            {
                                throw new Exception("Error in " + updateQuery);
                            }

                            updateQuery = "update tblCommercialDocSchedule SET transporterOrgId = " + newOrgTO.IdOrganization + " where transporterOrgId = " + existingTO.IdOrganization;

                            result = _iTblOrganizationDAO.UpdateTblOrgId(updateQuery, conn, tran);
                            if (result == -1)
                            {
                                throw new Exception("Error in " + updateQuery);
                            }
















                            result = _iTblOrganizationDAO.DeleteTblOrganization(existingTO.IdOrganization, conn, tran);
                            if (result == -1)
                            {
                                throw new Exception("Error in " + updateQuery);
                            }

                            if (result >=1 )
                            {
                                tran.Commit();
                            }
                        }
                        catch (Exception ex)
                        {
                            throw new Exception("Error");
                        }
                        finally
                        {
                            conn.Close();
                        }


                    }
                }

                resultMessage.DefaultSuccessBehaviour();
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "Error in  MigrateAndSaveOrgDetails(List<TblOrganizationTO> existingOrgList,SqlConnection conn,SqlTransaction tran)");
                return resultMessage;


            }
        }

        //Chetan[2020-12-14] added for deactive duplication Supplier
        public ResultMessage DeactiveDuplicationSupplier(bool isForSupplier)
        {
            ResultMessage resultMessage = new ResultMessage();
            string tranFailOrgIdStr = string.Empty;
            string deactiveOrgIdStr = string.Empty;
            string activeInScrapOrgIdStr = string.Empty;
            string activeInSapOrgIdStr = string.Empty;
            string errorMsgStr = "Data Save Sucessfully....";
            try
            {
                DataTable duplicateSupplierDT = _iTblOrganizationDAO.GetDuplicateSupplierDT(isForSupplier);
                if (duplicateSupplierDT != null && duplicateSupplierDT.Rows.Count > 0)
                {
                    for (int i = 0; i < duplicateSupplierDT.Rows.Count; i++)
                    {
                        string orgName = string.Empty;
                        if (duplicateSupplierDT.Columns.Contains("firmname") && duplicateSupplierDT.Rows[i]["firmname"] != DBNull.Value)
                        {
                            orgName = Convert.ToString(duplicateSupplierDT.Rows[i]["firmname"]);
                            List<TblOrganizationTO> tblOrganizationTOList = _iTblOrganizationDAO.SelectAllTblOrganization(orgName);
                            if (tblOrganizationTOList != null && tblOrganizationTOList.Count > 0)
                            {
                                ////chetan[2020-12-15] for Supplier 
                                if (isForSupplier)
                                {
                                    string duplicateOrgId = string.Empty;
                                    for (int c = 0; c < tblOrganizationTOList.Count; c++)
                                    {
                                        TblOrganizationTO tblOrganizationTO = tblOrganizationTOList[c];

                                        DataTable PurchaseManagerSupplierDT = _iTblOrganizationDAO.GetTblPurchaseManagerSupplier(tblOrganizationTO.IdOrganization);
                                        if (PurchaseManagerSupplierDT != null && PurchaseManagerSupplierDT.Rows.Count > 0)
                                        {
                                            activeInScrapOrgIdStr += tblOrganizationTO.IdOrganization + ",";
                                            continue;
                                        }
                                        else
                                        {
                                            DataTable supplierSAPDtlDT = _iTblOrganizationDAO.GetSupplierPresentInSAP(tblOrganizationTO.IdOrganization);
                                            if (supplierSAPDtlDT != null && supplierSAPDtlDT.Rows.Count > 0)
                                            {
                                                activeInSapOrgIdStr += tblOrganizationTO.IdOrganization + ",";
                                            }
                                            else
                                            {

                                                duplicateOrgId += tblOrganizationTO.IdOrganization + ",";
                                            }
                                        }

                                    }
                                    duplicateOrgId = duplicateOrgId.TrimEnd(',');
                                    int result =_iTblOrganizationDAO.DeactiveTblOrganization(duplicateOrgId);
                                    if (result < 1)
                                    {
                                        tranFailOrgIdStr += duplicateOrgId+",";
                                    }
                                    else
                                    {
                                        deactiveOrgIdStr += duplicateOrgId + ",";
                                    }
                                }
                                //chetan[2020-12-15] Other than Supplier 
                                else
                                {
                                    string duplicateOrgId = string.Empty;
                                    tblOrganizationTOList = tblOrganizationTOList.OrderBy(o => o.IdOrganization).ToList();
                                    for (int c = 1; c < tblOrganizationTOList.Count; c++)
                                    {
                                        duplicateOrgId += tblOrganizationTOList[c].IdOrganization + ",";
                                    }
                                    duplicateOrgId = duplicateOrgId.TrimEnd(',');
                                    int result =_iTblOrganizationDAO.DeactiveTblOrganization(duplicateOrgId);
                                    if (result < 1)
                                    {
                                        tranFailOrgIdStr += duplicateOrgId + ",";
                                    }
                                    else
                                    {
                                        deactiveOrgIdStr += duplicateOrgId + ",";
                                    }
                                }
                            }
                            else
                            {
                                errorMsgStr += "tblOrganizationTO list not found for Firm Name:-" + orgName + ",";
                            }
                        }
                    }

                    int finalResult = _iTblOrganizationDAO.InsertTblDeactiveDuplicationSupplierDtl(tranFailOrgIdStr, deactiveOrgIdStr, activeInScrapOrgIdStr, activeInSapOrgIdStr);
                    if (finalResult < 1)
                    {
                        errorMsgStr += "Error in Insert in InsertTblDeactiveDuplicationSupplierDtl.";
                        errorMsgStr += "tranFailOrgIdStr=" + tranFailOrgIdStr + ",deactiveOrgIdStr=" + deactiveOrgIdStr + ",activeInScrapOrgIdStr=" + activeInScrapOrgIdStr + ",activeInSapOrgIdStr=" + activeInSapOrgIdStr;
                        resultMessage.DefaultBehaviour();
                        resultMessage.DisplayMessage = errorMsgStr;
                        return resultMessage;
                    }

                }
                resultMessage.DefaultSuccessBehaviour();
                resultMessage.DisplayMessage = errorMsgStr;
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "DeactiveDuplicationSupplier");
                return resultMessage;
            }
        }

        public ResultMessage DeactivateInvalidAddressesAttachedToOrg(int orgTypeId, Int32 index = 0)
        { int tempIndex = 0;
            ResultMessage resultMessage = new ResultMessage();           
            try
            {
                List<DropDownTO> orgToList = _iTblOrganizationDAO.SelectAllOrganizationList(orgTypeId);  
                if(orgToList!= null && orgToList.Count>0)
                {
                    for (int i = index; i < orgToList.Count; i++)
                    {
                        tempIndex = i;
                        List<TblAddressTO> tblAddressTOList = _iTblAddressDAO.SelectOrgAddressList(orgToList[i].Value);
                        List<TblOrgLicenseDtlTO> tblOrgLicenseDtlTOList = _iTblOrgLicenseDtlDAO.SelectAllTblOrgLicenseDtl(orgToList[i].Value);
                        if (tblOrgLicenseDtlTOList != null && tblOrgLicenseDtlTOList.Count > 0)
                        {
                            var matchToPAN = tblOrgLicenseDtlTOList.Where(w => w.LicenseId == (Int32)Constants.CommercialLicenseE.PAN_NO).FirstOrDefault();
                            if (matchToPAN != null)
                            {
                                if (matchToPAN.LicenseValue != "0")
                                {

                                }
                            }

                            var matchToGSTIN = tblOrgLicenseDtlTOList.Where(w => w.LicenseId == (Int32)Constants.CommercialLicenseE.IGST_NO).FirstOrDefault();
                            if (matchToGSTIN != null) //|| matchToGSTIN.LicenseValue != 0
                            {
                                if (matchToGSTIN.LicenseValue != "0")
                                {

                                }
                            }
                            var matchToADHAR = tblOrgLicenseDtlTOList.Where(w => w.LicenseId == (Int32)Constants.CommercialLicenseE.AADHAR_NO).FirstOrDefault();
                            if (matchToADHAR != null)
                            {
                                if (matchToADHAR.LicenseValue != "0")
                                {

                                }
                            }

                        }


                        SAPbobsCOM.BusinessPartners oBusinesspartner;
                        oBusinesspartner = Startup.CompanyObject.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oBusinessPartners);
                        oBusinesspartner.GetByKey(orgToList[i].Value.ToString());
                        int AddressFound = 1;
                        if (string.IsNullOrEmpty(oBusinesspartner.Addresses.AddressName))
                        {
                            AddressFound = 0;
                        }
                        if (AddressFound == 0)
                        {
                            #region Address Details  
                            int addedAdd = 0;
                            for (int al = 0; al < tblAddressTOList.Count; al++)
                            {

                                if (((tblAddressTOList[al].PlotNo != null && tblAddressTOList[al].PlotNo != "") 
                                    || (tblAddressTOList[al].StreetName != null && tblAddressTOList[al].StreetName != "") 
                                    || (tblAddressTOList[al].CountryCode != null && tblAddressTOList[al].CountryCode != "")
                                    || (tblAddressTOList[al].StateCode != null && tblAddressTOList[al].StateCode != "") 
                                    || (tblAddressTOList[al].DistrictName != null && tblAddressTOList[al].DistrictName != "")
                                    
                                    || (tblAddressTOList[al].AreaName != null && tblAddressTOList[al].AreaName != "" ) 
                                    || tblAddressTOList[al].Pincode != 0) && addedAdd==0)
                                {
                                    addedAdd = 1;
                                    //oBusinesspartner.Addresses.SetCurrentLine(al);
                                    oBusinesspartner.Addresses.AddressType = SAPbobsCOM.BoAddressType.bo_BillTo;
                                    oBusinesspartner.Addresses.AddressName = "Office Adress";
                                    oBusinesspartner.Addresses.Block = tblAddressTOList[al].PlotNo;
                                    oBusinesspartner.Addresses.Street = tblAddressTOList[al].StreetName;
                                    oBusinesspartner.Addresses.StreetNo = tblAddressTOList[al].StreetName;

                                    oBusinesspartner.Addresses.Country = tblAddressTOList[al].CountryCode;
                                    oBusinesspartner.Addresses.State = tblAddressTOList[al].StateCode;
                                    oBusinesspartner.Addresses.City = tblAddressTOList[al].DistrictName;
                                    oBusinesspartner.Addresses.AddressName2 = tblAddressTOList[al].AreaName;
                                    oBusinesspartner.Addresses.AddressName3 = tblAddressTOList[al].VillageName;
                                    oBusinesspartner.Addresses.ZipCode = tblAddressTOList[al].Pincode.ToString();

                                    if (tblOrgLicenseDtlTOList != null && tblOrgLicenseDtlTOList.Count > 0)
                                    {
                                        TblOrgLicenseDtlTO gstinnoTO = tblOrgLicenseDtlTOList.Where(x => x.LicenseId == (int)Constants.CommercialLicenseE.IGST_NO).FirstOrDefault();
                                        if (gstinnoTO != null && gstinnoTO.LicenseValue != "0")
                                        {

                                            oBusinesspartner.Addresses.GSTIN = gstinnoTO.LicenseValue;
                                            if (gstinnoTO.GstTypeId == 0)
                                            {
                                                gstinnoTO.GstTypeId = (Int32)SAPbobsCOM.BoGSTRegnTypeEnum.gstRegularTDSISD;
                                            }
                                            oBusinesspartner.Addresses.GstType = (SAPbobsCOM.BoGSTRegnTypeEnum)gstinnoTO.GstTypeId;
                                            //  oBusinesspartner.Addresses.GstType = SAPbobsCOM.BoGSTRegnTypeEnum.gstRegularTDSISD;   //commented by harshala to make it dynamic

                                        }

                                        TblOrgLicenseDtlTO panNoTO = tblOrgLicenseDtlTOList.Where(x => x.LicenseId == (int)Constants.CommercialLicenseE.PAN_NO).FirstOrDefault();
                                        if (panNoTO != null && panNoTO.LicenseValue != "0")
                                        {
                                            //panNo = panNoTO.LicenseValue;   //commented by harshala for code optimization
                                            oBusinesspartner.FiscalTaxID.SetCurrentLine(0);
                                            oBusinesspartner.FiscalTaxID.TaxId0 = panNoTO.LicenseValue;
                                            oBusinesspartner.FiscalTaxID.Add();
                                        }
                                    }

                                    oBusinesspartner.Addresses.Add();
                                    oBusinesspartner.Addresses.AddressType = SAPbobsCOM.BoAddressType.bo_ShipTo;
                                    oBusinesspartner.Addresses.AddressName = "Office Adress";
                                    oBusinesspartner.Addresses.Block = tblAddressTOList[al].PlotNo;
                                    oBusinesspartner.Addresses.Street = tblAddressTOList[al].StreetName;
                                    oBusinesspartner.Addresses.StreetNo = tblAddressTOList[al].StreetName;


                                    oBusinesspartner.Addresses.Country = tblAddressTOList[al].CountryCode;
                                    oBusinesspartner.Addresses.State = tblAddressTOList[al].StateCode;
                                    oBusinesspartner.Addresses.City = tblAddressTOList[al].DistrictName;
                                    oBusinesspartner.Addresses.AddressName2 = tblAddressTOList[al].AreaName;
                                    oBusinesspartner.Addresses.AddressName3 = tblAddressTOList[al].VillageName;
                                    oBusinesspartner.Addresses.ZipCode = tblAddressTOList[al].Pincode.ToString();

                                    if (tblOrgLicenseDtlTOList != null && tblOrgLicenseDtlTOList.Count > 0)
                                    {
                                        TblOrgLicenseDtlTO gstinnoTO = tblOrgLicenseDtlTOList.Where(x => x.LicenseId == (int)Constants.CommercialLicenseE.IGST_NO).FirstOrDefault();
                                        if (gstinnoTO != null && gstinnoTO.LicenseValue != "0")
                                        {
                                            //gstinno = gstinnoTO.LicenseValue;   //commented by harshala for code optimization
                                            oBusinesspartner.Addresses.GSTIN = gstinnoTO.LicenseValue;
                                            if (gstinnoTO.GstTypeId == 0)
                                            {
                                                gstinnoTO.GstTypeId = (Int32)SAPbobsCOM.BoGSTRegnTypeEnum.gstRegularTDSISD;
                                            }
                                            oBusinesspartner.Addresses.GstType = (SAPbobsCOM.BoGSTRegnTypeEnum)gstinnoTO.GstTypeId;
                                        }

                                        TblOrgLicenseDtlTO panNoTO = tblOrgLicenseDtlTOList.Where(x => x.LicenseId == (int)Constants.CommercialLicenseE.PAN_NO).FirstOrDefault();
                                        if (panNoTO != null && panNoTO.LicenseValue != "0")
                                        {
                                            //panNo = panNoTO.LicenseValue;   //commented by harshala for code optimization
                                            oBusinesspartner.FiscalTaxID.SetCurrentLine(0);
                                            oBusinesspartner.FiscalTaxID.TaxId0 = panNoTO.LicenseValue;
                                            oBusinesspartner.FiscalTaxID.Add();

                                        }
                                    }
                                    if ((tblAddressTOList.Count - 1) != al)
                                        oBusinesspartner.Addresses.Add();
                                }

                                else
                                {
                                    if ((tblAddressTOList[al].PlotNo == null || tblAddressTOList[al].PlotNo == "")
                                   && (tblAddressTOList[al].StreetName == null || tblAddressTOList[al].StreetName == "")
                                   && (tblAddressTOList[al].CountryCode == null || tblAddressTOList[al].CountryCode == "")
                                   && (tblAddressTOList[al].StateCode == null || tblAddressTOList[al].StateCode == "")
                                   && (tblAddressTOList[al].DistrictName == null || tblAddressTOList[al].DistrictName == "")
                                   && (tblAddressTOList[al].VillageName == null || tblAddressTOList[al].VillageName == "")
                                   && (tblAddressTOList[al].AreaName == null || tblAddressTOList[al].AreaName == "")
                                   && tblAddressTOList[al].Pincode == 0)
                                    {
                                        resultMessage.Result = _iTblOrgAddressDAO.DeleteTblOrgAddress(tblAddressTOList[al].IdAddr);
                                        if (resultMessage.Result > 0)
                                        {
                                            resultMessage.Result = _iTblAddressDAO.DeleteTblAddress(tblAddressTOList[al].IdAddr);
                                        }
                                    }
                                }
                            }

                            #endregion

                            int result = oBusinesspartner.Update();
                            if (result == 0)
                            {                                
                            }
                            else
                            {
                                string errorinfor = Startup.CompanyObject.GetLastErrorDescription();
                                resultMessage.DefaultBehaviour(errorinfor + " Index - " + tempIndex);
                                return resultMessage;
                            }
                        }
                        else
                        {
                            for (int al = 0; al < tblAddressTOList.Count; al++)
                            {
                                if ((tblAddressTOList[al].PlotNo == null || tblAddressTOList[al].PlotNo == "")
                                   && (tblAddressTOList[al].StreetName == null || tblAddressTOList[al].StreetName == "")
                                   && (tblAddressTOList[al].CountryCode == null || tblAddressTOList[al].CountryCode == "")
                                   && (tblAddressTOList[al].StateCode == null || tblAddressTOList[al].StateCode == "")
                                   && (tblAddressTOList[al].DistrictName == null || tblAddressTOList[al].DistrictName == "")
                                   && (tblAddressTOList[al].VillageName == null || tblAddressTOList[al].VillageName == "")
                                   && (tblAddressTOList[al].AreaName == null || tblAddressTOList[al].AreaName == "")
                                   && tblAddressTOList[al].Pincode == 0) { 

                                    resultMessage.Result = _iTblOrgAddressDAO.DeleteTblOrgAddress(tblAddressTOList[al].IdAddr);
                                    if (resultMessage.Result > 0)
                                    {
                                        resultMessage.Result = _iTblAddressDAO.DeleteTblAddress(tblAddressTOList[al].IdAddr);
                                    }
                                }
                            }
                        }
                    }

                }
                resultMessage.DefaultSuccessBehaviour();
                resultMessage.Tag = tempIndex;
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "DeactiveDuplicationSupplier Index -" + tempIndex);
                return resultMessage;
            }
        }


        public ResultMessage SetAddressTypeAttachedToOrg(int orgTypeId, Int32 index = 0)
        {
            int tempIndex = 0;
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                List<DropDownTO> orgToList = _iTblOrganizationDAO.SelectAllOrganizationList(orgTypeId);
                if (orgToList != null && orgToList.Count > 0)
                {
                    for (int i = index; i < orgToList.Count; i++)
                    {
                        tempIndex = i;
                        List<TblAddressTO> tblAddressTOList = _iTblAddressDAO.SelectOrgAddressList(orgToList[i].Value);
                        if (tblAddressTOList != null && tblAddressTOList.Count > 0)
                        {
                            if (Convert.ToInt32(orgToList[i].MappedTxnId) == 0)
                            {
                                orgToList[i].MappedTxnId = tblAddressTOList[0].IdAddr.ToString();
                            }
                            //else
                            //{
                            //    continue;
                            //}
                            List<TblAddressTO> tblAddressTOListTemp = tblAddressTOList.Where(w => w.IdAddr != Convert.ToInt32(orgToList[i].MappedTxnId)).ToList();
                            if (tblAddressTOListTemp != null && tblAddressTOListTemp.Count > 0)
                            {
                                for (int j = 0; j < tblAddressTOListTemp.Count; j++)
                                {
                                    int result = _iTblAddressDAO.UpdateTblAddressType(tblAddressTOListTemp[j].IdOrgAddr);

                                    if (result != 1)
                                    {
                                        resultMessage.DefaultBehaviour(" Address Type update failed - " + tempIndex);
                                        return resultMessage;
                                    }
                                }
                            }
                        }
                    }

                }
                resultMessage.DefaultSuccessBehaviour();
                resultMessage.Tag = tempIndex;
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "DeactiveDuplicationSupplier Index -" + tempIndex);
                return resultMessage;
            }
        }


        public ResultMessage PostSendAPKLink(List<DropDownTO> aPKLinkToOrgList,Int32 loginUserId)
        {
            ResultMessage resultMessage = new ResultMessage();
            Int32 result = 0;
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();

                List<TblSmsTO> tblSmsTOList = new List<TblSmsTO>();
                TblVersionTO tblVersionTO = _iTblVersionBL.SelectLatestVersionTO();
                string strErrorMsg = "";
                if (aPKLinkToOrgList != null && aPKLinkToOrgList.Count > 0)
                {
                    for (int i = 0; i < aPKLinkToOrgList.Count; i++)
                    {
                        TblOrganizationTO tblOrganizationTo = SelectTblOrganizationTO(aPKLinkToOrgList[i].Value);

                        List<TblPersonTO> list = _iTblPersonBL.SelectAllPersonListByOrganizationV2(tblOrganizationTo.IdOrganization, 0);
                        list = list.GroupBy(g => g.IdPerson).Select(s => s.FirstOrDefault()).ToList();

                        string userCredentials = "";
                        if (list != null && list.Count > 0)
                        {
                            for (int per = 0; per < list.Count; per++)
                            {
                                if (list[per].UserId > 0)
                                {
                                    if (list[per].MobileNo != null && list[per].MobileNo != "91" && list[per].MobileNo != "")
                                    {
                                        string UserNamePsw = "";
                                        TblUserTO tblUser = _iTblUserBL.SelectTblUserTO(list[per].UserId);
                                        if (tblUser != null)
                                        {
                                            userCredentials += " UserName :" + tblUser.UserLogin + " Password :" + tblUser.UserPasswd + "\n";
                                            UserNamePsw = "\n UserName :" + tblUser.UserLogin + " Password :" + tblUser.UserPasswd + "\n";
                                        }

                                        #region Comment

                                        //CommonAlertTo commonAlertToUser = new CommonAlertTo();
                                        //commonAlertToUser.AlertAction = "";
                                        //commonAlertToUser.SourceEntityId = aPKLinkToOrgList[i].Value;
                                        //commonAlertToUser.RaisedBy = loginUserId;
                                        //commonAlertToUser.RaisedOn = _iCommon.ServerDateTime; ;
                                        //commonAlertToUser.EffectiveFromDate = _iCommon.ServerDateTime;
                                        //commonAlertToUser.EffectiveToDate = _iCommon.ServerDateTime;
                                        //TblAlertDefinitionTO alertDefTOUser = _iTblAlertDefinitionBL.SelectTblAlertDefinitionTO(Constants.OrgAlertDefId, conn, tran);
                                        //if (alertDefTOUser == null)
                                        //{
                                        //    resultMessage.Text = "TblAlertDefinitionTO Found NULL. Alert Definition is not given for this alert";
                                        //    resultMessage.MessageType = ResultMessageE.Information;
                                        //    resultMessage.Result = 1;
                                        //    return resultMessage;
                                        //}

                                        //Int32 alertdefIdUser = Convert.ToInt32(Constants.OrgAlertDefId);
                                        //commonAlertToUser.AlertComment = alertDefTOUser.DefaultAlertTxt.Replace("@Link", tblVersionTO.UrlPath.ToString());
                                        //if (UserNamePsw == null || UserNamePsw == "")
                                        //{
                                        //    commonAlertToUser.AlertComment = commonAlertToUser.AlertComment.Replace("@UserLoginDetails", "Credentials Not Found");
                                        //}
                                        //else
                                        //{
                                        //    commonAlertToUser.AlertComment = commonAlertToUser.AlertComment.Replace("@UserLoginDetails", UserNamePsw.ToString());
                                        //}
                                        //TblSmsTO smsTOUser = new TblSmsTO();
                                        //smsTOUser.MobileNo = list[per].MobileNo;
                                        //smsTOUser.SourceTxnDesc = tblOrganizationTo.IdOrganization.ToString();
                                        //smsTOUser.SmsTxt = commonAlertToUser.AlertComment;
                                        //tblSmsTOList.Add(smsTOUser);

                                        #endregion
                                    }

                                    #region Comment

                                    //if (list[per].AlternateMobNo != null && list[per].AlternateMobNo != "91" && list[per].AlternateMobNo != "")
                                    //{
                                    //    string UserNamePsw = "";
                                    //    TblUserTO tblUser = _iTblUserBL.SelectTblUserTO(list[per].UserId);
                                    //    if (tblUser != null)
                                    //    {
                                    //        userCredentials += " UserName :" + tblUser.UserLogin + " Password :" + tblUser.UserPasswd + "\n";
                                    //        UserNamePsw = "\n UserName :" + tblUser.UserLogin + " Password :" + tblUser.UserPasswd + "\n";
                                    //    }


                                    //    CommonAlertTo commonAlertToUser = new CommonAlertTo();
                                    //    commonAlertToUser.AlertAction = "";
                                    //    commonAlertToUser.SourceEntityId = aPKLinkToOrgList[i].Value;
                                    //    commonAlertToUser.RaisedBy = loginUserId;
                                    //    commonAlertToUser.RaisedOn = _iCommon.ServerDateTime; ;
                                    //    commonAlertToUser.EffectiveFromDate = _iCommon.ServerDateTime;
                                    //    commonAlertToUser.EffectiveToDate = _iCommon.ServerDateTime;
                                    //    TblAlertDefinitionTO alertDefTOUser = _iTblAlertDefinitionBL.SelectTblAlertDefinitionTO(Constants.OrgAlertDefId, conn, tran);
                                    //    if (alertDefTOUser == null)
                                    //    {
                                    //        resultMessage.Text = "TblAlertDefinitionTO Found NULL. Alert Definition is not given for this alert";
                                    //        resultMessage.MessageType = ResultMessageE.Information;
                                    //        resultMessage.Result = 1;
                                    //        return resultMessage;
                                    //    }

                                    //    Int32 alertdefIdUser = Convert.ToInt32(Constants.OrgAlertDefId);
                                    //    commonAlertToUser.AlertComment = alertDefTOUser.DefaultAlertTxt.Replace("@Link", tblVersionTO.UrlPath.ToString());
                                    //    if (UserNamePsw == null || UserNamePsw == "")
                                    //    {
                                    //        commonAlertToUser.AlertComment = commonAlertToUser.AlertComment.Replace("@UserLoginDetails", "Credentials Not Found");
                                    //    }
                                    //    else
                                    //    {
                                    //        commonAlertToUser.AlertComment = commonAlertToUser.AlertComment.Replace("@UserLoginDetails", UserNamePsw.ToString());
                                    //    }
                                    //    TblSmsTO smsTOUser = new TblSmsTO();
                                    //    smsTOUser.MobileNo = list[per].AlternateMobNo;
                                    //    smsTOUser.SourceTxnDesc = tblOrganizationTo.IdOrganization.ToString();
                                    //    smsTOUser.SmsTxt = commonAlertToUser.AlertComment;
                                    //    tblSmsTOList.Add(smsTOUser);
                                    //}
                                    //else
                                    //{
                                    //    strErrorMsg += " Mobile No Not Added for Person " + list[per].FirstName + " " + list[per].LastName + " Against Organization :" + tblOrganizationTo.FirmName + " ,    \r\n";
                                    //}
                                    #endregion

                                }
                                else
                                {
                                    strErrorMsg += " User Login Not Created for Person " + list[per].FirstName + " " + list[per].LastName + " Against Organization :" + tblOrganizationTo.FirmName + " ,    \r\n";
                                }
                            }

                        }
                        else
                        {
                            strErrorMsg += " Concern Person are not available to Organization :" + tblOrganizationTo.FirmName + " ,       \r\n";
                        }
                        if (tblOrganizationTo.RegisteredMobileNos != null && tblOrganizationTo.RegisteredMobileNos != "91" && tblOrganizationTo.RegisteredMobileNos != "")
                        {
                            #region  Send Notifications & SMSs

                            CommonAlertTo commonAlertTo = new CommonAlertTo();
                            commonAlertTo.AlertAction = "";
                            commonAlertTo.SourceEntityId = aPKLinkToOrgList[i].Value;
                            commonAlertTo.RaisedBy = loginUserId;
                            commonAlertTo.RaisedOn = _iCommon.ServerDateTime; ;
                            commonAlertTo.EffectiveFromDate = _iCommon.ServerDateTime;
                            commonAlertTo.EffectiveToDate = _iCommon.ServerDateTime;

                            // 1. Get Alert Definition
                            TblAlertDefinitionTO alertDefTO = _iTblAlertDefinitionBL.SelectTblAlertDefinitionTO(Constants.OrgAlertDefId, conn, tran);
                            if (alertDefTO == null)
                            {
                                resultMessage.Text = "TblAlertDefinitionTO Found NULL. Alert Definition is not given for this alert";
                                resultMessage.MessageType = ResultMessageE.Information;
                                resultMessage.Result = 1;
                                return resultMessage;
                            }

                            Int32 alertdefId = Convert.ToInt32(Constants.OrgAlertDefId);
                            commonAlertTo.AlertComment = alertDefTO.DefaultAlertTxt.Replace("@Link", tblVersionTO.UrlPath.ToString());
                            if (userCredentials == null || userCredentials == "")
                            {
                                commonAlertTo.AlertComment = commonAlertTo.AlertComment.Replace("@UserLoginDetails", "Credentials Not Found");
                            }
                            else
                            {
                                userCredentials = "\n"+ userCredentials;
                                commonAlertTo.AlertComment = commonAlertTo.AlertComment.Replace("@UserLoginDetails", userCredentials.ToString());
                            }
                            TblSmsTO smsTO = new TblSmsTO();
                            smsTO.MobileNo = tblOrganizationTo.RegisteredMobileNos;
                            smsTO.SourceTxnDesc = tblOrganizationTo.IdOrganization.ToString();
                            smsTO.SmsTxt = commonAlertTo.AlertComment;
                            tblSmsTOList.Add(smsTO);
                            #endregion
                        }                       

                    }

                }


                #region Send SMS

                TblConfigParamsTO smsActivationConfTO = _iTblConfigParamsBL.SelectTblConfigParamsTO(Constants.SMS_SUBSCRIPTION_ACTIVATION, conn, tran);
                Int32 smsActive = 0;
                if (smsActivationConfTO != null)
                    smsActive = Convert.ToInt32(smsActivationConfTO.ConfigParamVal);

                if (smsActive == 1)
                {
                    if (tblSmsTOList != null && tblSmsTOList.Count > 0)
                    {
                        for (int sms = 0; sms < tblSmsTOList.Count; sms++)
                        {
                            if (tblSmsTOList[sms].MobileNo != null && tblSmsTOList[sms].MobileNo.Length >= 10)
                            {
                                String smsResponse = _iVitplSMS.SendSMSAsync(tblSmsTOList[sms]);
                                tblSmsTOList[sms].ReplyTxt = smsResponse;
                                tblSmsTOList[sms].SentOn = _iCommon.ServerDateTime;

                                result = _iTblSmsBL.InsertTblSms(tblSmsTOList[sms], conn, tran);
                            }
                        }
                    }
                    else
                    {
                        resultMessage.DefaultSuccessBehaviour();
                        resultMessage.DisplayMessage = "Mobile number Or Concern Person not found";
                        resultMessage.Text = "Mobile number Or Concern Person not found";
                        return resultMessage;
                    }
                }
                else
                {
                    resultMessage.DefaultSuccessBehaviour();
                    resultMessage.Text = "SMS Subscription is not active";
                    resultMessage.DisplayMessage = "SMS Subscription is not active";
                    return resultMessage;
                }

                #endregion


                resultMessage.DefaultSuccessBehaviour();
                if (strErrorMsg != null && strErrorMsg != "")
                {
                    resultMessage.Text = strErrorMsg;
                    resultMessage.DisplayMessage = strErrorMsg;
                }
                else
                {
                    resultMessage.Text = "APK Link and Login Credentials Sent Successfully";
                    resultMessage.DisplayMessage = "APK Link and Login Credentials Sent Successfully";
                }
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "DeactiveDuplicationSupplier Index -");
                return resultMessage;
            }
            finally
            {
                conn.Close();
            }
        }

        public ResultMessage CheckIfOrgIsAvailableInSAP(int orgId)
        {
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                if (orgId > 0)
                {
                    TblConfigParamsTO isSapEnable = _iTblConfigParamsBL.SelectTblConfigParamsValByName(Constants.SAPB1_SERVICES_ENABLE);
                    if (isSapEnable != null && isSapEnable.ConfigParamVal != null && (Convert.ToInt32(isSapEnable.ConfigParamVal)) == 1)
                    {
                        DropDownTO dropDownTOOrg = _iTblOrganizationDAO.CheckIfOrgIsAvailableInSAP(orgId);
                     
                        if (dropDownTOOrg != null && dropDownTOOrg.Value > 0)
                        {
                            List<TblAddressTO> tblAddressTOList = _iTblAddressDAO.SelectOrgAddressList(orgId);
                            List<DropDownTO> dropDownTOAddrList = _iTblOrganizationDAO.CheckIfOrgAddressIsAvailableInSAP(orgId);
                            if (tblAddressTOList == null || tblAddressTOList.Count <= 0)
                            {
                                resultMessage.DefaultBehaviour();
                                resultMessage.DisplayMessage = "Supplier Address Details not found, Please update.";
                                resultMessage.Text = "Supplier Address Details not found, Please update.";
                                return resultMessage;
                            }

                           


                                if (dropDownTOAddrList != null && dropDownTOAddrList.Count > 0)
                            {
                                string errorMsg = "";
                                DropDownTO dropDownTOTempAdd = dropDownTOAddrList.Where(w => w.Tag.ToString() == orgId.ToString()).FirstOrDefault();
                                //dropDownTOOrgCurrencyList[0].Text

                                List<DropDownTO> dropDownTOOrgCurrencyList = _iTblOrganizationDAO.CheckIfOrgCurrencyIsAvailableInSAP(orgId);
                                if (dropDownTOOrgCurrencyList != null && dropDownTOOrgCurrencyList.Count > 0)
                                {
                                    if (Convert.ToString(dropDownTOOrgCurrencyList[0].Text) == "INR") // Add By Samadhan 6 Apr 2023
                                    {
                                        if (dropDownTOTempAdd != null && Convert.ToInt32(dropDownTOTempAdd.Tag) > 0)
                                        {
                                            List<DropDownTO> dropDownTOTempGSTIN = dropDownTOAddrList.Where(w => w.Text != null).ToList();
                                            List<DropDownTO> dropDownTOTempGSTType = dropDownTOAddrList.Where(w => w.Value > 0).ToList();
                                            if (dropDownTOTempGSTIN == null || dropDownTOTempGSTIN.Count < 2)
                                            {
                                                errorMsg = "GSTIN ";
                                            }
                                            if (dropDownTOTempGSTType == null || dropDownTOTempGSTType.Count < 2)
                                            {
                                                if (errorMsg != null && errorMsg != "")
                                                {
                                                    errorMsg = errorMsg + ", GST Type ";
                                                }
                                                else
                                                {
                                                    errorMsg = "GST Type ";
                                                }
                                            }
                                            if (errorMsg != null && errorMsg != "")
                                            {
                                                resultMessage.DefaultBehaviour();
                                                resultMessage.Text = "Supplier " + errorMsg + "Details not found in SAP, Please update the same .";
                                                resultMessage.DisplayMessage = "Supplier " + errorMsg + "Details not found in SAP, Please update the same .";
                                                return resultMessage;
                                            }
                                        }
                                    }
                                }
                              
                            }
                            else
                            {
                                resultMessage.DefaultBehaviour();
                                resultMessage.DisplayMessage = "Supplier Address Details not found in SAP, Please update the same .";
                                resultMessage.Text = "Supplier Address Details not found in SAP, Please update the same .";
                                return resultMessage;
                            }
                        }
                        else
                        {
                            resultMessage.DefaultBehaviour();
                            resultMessage.DisplayMessage = "Supplier Details not found in SAP, Please update the same .";
                            resultMessage.Text = "Supplier Details not found in SAP, Please update the same .";
                            return resultMessage;



                        }
                    }
                }
                resultMessage.DefaultSuccessBehaviour();
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "CheckIfOrgIsAvailableInSAP");
                return resultMessage;
            }
            finally
            {
            }
        }

        public ResultMessage UpdateOrgLicenseDtls()
        {
            ResultMessage resultMessage = new ResultMessage();
            Int32 result = 0;
            try
            {
                List<TblOrgLicenseDtlTO> orgList = _iTblOrgLicenseDtlDAO.SelectAllOrgList();
                if(orgList != null && orgList.Count > 0)
                {
                    orgList = orgList.Take(50).ToList();

                    for (int i = 0; i < orgList.Count; i++)
                    {
                        TblOrgLicenseDtlTO tempTO = orgList[i];

                        TblOrganizationTO tblOrganizationTO = _iTblOrganizationDAO.SelectTblOrganizationTO(tempTO.OrganizationId);
                        if(tblOrganizationTO != null)
                        {
                            tempTO.AddressId = tblOrganizationTO.AddrId;
                            result = _iTblOrgLicenseDtlDAO.UpdateOrgLicense(tempTO);
                            if(result == -1)
                            {
                                //throw new Exception("Error in UpdateOrgLicense(tempTO);");
                                continue;
                            }

                        }

                    }
                }
                
                resultMessage.DefaultSuccessBehaviour();
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "UpdateOrgLicenseDtls");
                return resultMessage;

            }
        }

        public List<TblOrganizationTO> SelectAllChildOrganizationList(int orgTypeId, int parentId, int dealerlistType)
        {
            throw new NotImplementedException();
        }
    }

}
