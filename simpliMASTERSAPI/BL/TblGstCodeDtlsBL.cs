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
using simpliMASTERSAPI.Models;

namespace ODLMWebAPI.BL
{
    public class TblGstCodeDtlsBL : ITblGstCodeDtlsBL
    {
        private readonly ITblGstCodeDtlsDAO _iTblGstCodeDtlsDAO;
        private readonly ITblTaxRatesDAO _iTblTaxRatesDAO;
        private readonly ITblProdGstCodeDtlsDAO _iTblProdGstCodeDtlsDAO;
        private readonly ITblConfigParamsDAO _iTblConfigParamsDAO;
        private readonly IConnectionString _iConnectionString;
        private readonly IDimensionDAO _iDimensionDAO;
        public TblGstCodeDtlsBL(IDimensionDAO iDimensionDAO, ITblConfigParamsDAO iTblConfigParamsDAO, IConnectionString iConnectionString, ITblProdGstCodeDtlsDAO iTblProdGstCodeDtlsDAO, ITblTaxRatesDAO iTblTaxRatesDAO, ITblGstCodeDtlsDAO iTblGstCodeDtlsDAO)
        {
            _iTblGstCodeDtlsDAO = iTblGstCodeDtlsDAO;
            _iTblTaxRatesDAO = iTblTaxRatesDAO;
            _iTblProdGstCodeDtlsDAO = iTblProdGstCodeDtlsDAO;
            _iConnectionString = iConnectionString;
            _iTblConfigParamsDAO = iTblConfigParamsDAO;
            _iDimensionDAO = iDimensionDAO;
        }
        #region Selection

        public List<TblGstCodeDtlsTO> SelectAllTblGstCodeDtlsList()
        {
            return _iTblGstCodeDtlsDAO.SelectAllTblGstCodeDtls();
        }

        public TblGstCodeDtlsTO SelectTblGstCodeDtlsTO(Int32 idGstCode)
        {
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                return _iTblGstCodeDtlsDAO.SelectTblGstCodeDtls(idGstCode, conn, tran);
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                conn.Close();
            }
        }
        public List<TblGstCodeDtlsTO> SearchGSTCodeDetails(string searchedStr = "", Int32 searchCriteria = 0, Int32 codeTypeId = 0)
        {
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                // return _iTblGstCodeDtlsDAO.SelectTblGstCodeDtls(idGstCode, conn, tran);
                List<TblGstCodeDtlsTO> list = _iTblGstCodeDtlsDAO.SearchGSTCodeDetails(searchedStr, searchCriteria, codeTypeId, conn, tran);
                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                conn.Close();
            }
        }

        public TblGstCodeDtlsTO SelectTblGstCodeDtlsTO(Int32 idGstCode, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblGstCodeDtlsDAO.SelectTblGstCodeDtls(idGstCode, conn, tran);
        }

        public TblGstCodeDtlsTO SelectGstCodeDtlsTO(Int32 prodCatId, Int32 prodSpecId, Int32 materialId, Int32 prodItemId)
        {
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                return _iTblGstCodeDtlsDAO.SelectGstCodeDtlsTO(prodCatId, prodSpecId, materialId, prodItemId, conn, tran);
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                conn.Close();
            }
        }

        public List<TblGstCodeDtlsTO> SelectAllTblGstCodeDtlsList(Int32 gstCodeId = 0)
        {
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                return _iTblGstCodeDtlsDAO.SelectTblGstCodeDtlsList(gstCodeId, conn, tran);
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                conn.Close();
            }
        }

        public TblGstCodeDtlsTO SelectGstCodeDtlsTO(Int32 prodCatId, Int32 prodSpecId, Int32 materialId, Int32 prodItemId, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblGstCodeDtlsDAO.SelectGstCodeDtlsTO(prodCatId, prodSpecId, materialId, prodItemId, conn, tran);
        }

        #endregion

        #region Insertion
        public int InsertTblGstCodeDtls(TblGstCodeDtlsTO tblGstCodeDtlsTO)
        {
            return _iTblGstCodeDtlsDAO.InsertTblGstCodeDtls(tblGstCodeDtlsTO);
        }

        public int InsertTblGstCodeDtls(TblGstCodeDtlsTO tblGstCodeDtlsTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblGstCodeDtlsDAO.InsertTblGstCodeDtls(tblGstCodeDtlsTO, conn, tran);
        }
        public string getSAPMappedGstTaxCode(int TaxTypeId, double TaxPct)
        {
            switch (TaxPct)
            {
                case (Double)Constants.SAPTaxCodePerEnum.ZERO_PER:
                    if (TaxTypeId == (Int32)Constants.TaxTypeE.IGST)
                        return Constants.SAPTaxCodeEnum.ZERO_PER_IGST;
                    else
                        return Constants.SAPTaxCodeEnum.ZERO_PER_SCGST;
                    break;
                case (Double)Constants.SAPTaxCodePerEnum.TWENTY_EIGHT_PER:
                    if (TaxTypeId == (Int32)Constants.TaxTypeE.IGST)
                        return Constants.SAPTaxCodeEnum.TWENTY_EIGHT_PER_IGST;
                    else
                        return Constants.SAPTaxCodeEnum.TWENTY_EIGHT_PER_SCGST;
                    break;
                case (Double)Constants.SAPTaxCodePerEnum.TWELEVE_PER:
                    if (TaxTypeId == (Int32)Constants.TaxTypeE.IGST)
                        return Constants.SAPTaxCodeEnum.TWELEVE_PER_IGST;
                    else
                        return Constants.SAPTaxCodeEnum.TWELEVE_PER_SCGST;
                    break;
                case (Double)Constants.SAPTaxCodePerEnum.FIVE_PER:
                    if (TaxTypeId == (Int32)Constants.TaxTypeE.IGST)
                        return Constants.SAPTaxCodeEnum.FIVE_PER_IGST;
                    else
                        return Constants.SAPTaxCodeEnum.FIVE_PER_SCGST;
                    break;
                case (Double)Constants.SAPTaxCodePerEnum.EIGHTEEN_PER:
                    if (TaxTypeId == (Int32)Constants.TaxTypeE.IGST)
                        return Constants.SAPTaxCodeEnum.EIGHTEEN_PER_IGST;
                    else
                        return Constants.SAPTaxCodeEnum.EIGHTEEN_PER_SCGST;
                    break;
                default:
                    return "";
                    break;
            }
        }
        public ResultMessage MigrateGstCodeToSAP()
        {
            ResultMessage resultMessage = new ResultMessage();

            List<TblGstCodeDtlsTO> tblGstCodeDtlsTOList = _iTblGstCodeDtlsDAO.SelectAllTblServiceGstCodeDtlsForMigration();

            List<ResultMessage> ErrorList = new List<ResultMessage>();
            if (tblGstCodeDtlsTOList != null && tblGstCodeDtlsTOList.Count > 0)
            {

                for (int i = 0; i < tblGstCodeDtlsTOList.Count; i++)
                {



                    TblGstCodeDtlsTO gstCodeDtlsTO = tblGstCodeDtlsTOList[i];


                    //gstCodeDtlsTO.CodeDesc = Convert.ToString(dt.Rows[i]["1"]);
                    //gstCodeDtlsTO.CodeNumber = Convert.ToString(dt.Rows[i]["1"]);
                    //gstCodeDtlsTO.TaxPct = Convert.ToDouble(dt.Rows[i]["1"]);
                    //gstCodeDtlsTO.CodeTypeId = Convert.ToInt32(dt.Rows[i]["1"]);

                    gstCodeDtlsTO.IsActive = 1;
                    gstCodeDtlsTO.CreatedBy = 1;
                    gstCodeDtlsTO.CreatedOn = new DateTime(2019, 04, 01);
                    gstCodeDtlsTO.UpdatedBy = 1;
                    gstCodeDtlsTO.UpdatedOn = new DateTime(2019, 04, 01);
                    gstCodeDtlsTO.EffectiveFromDt = new DateTime(2019, 04, 01);


                    gstCodeDtlsTO.TaxRatesTOList = new List<TblTaxRatesTO>();

                    TblTaxRatesTO igstTO = new TblTaxRatesTO();
                    igstTO.TaxTypeId = 1;
                    igstTO.TaxPct = 100;
                    igstTO.CreatedOn = new DateTime(2019, 04, 01);
                    igstTO.CreatedBy = 1;
                    igstTO.EffectiveFromDt = igstTO.CreatedOn;
                    igstTO.IsActive = 1;
                    gstCodeDtlsTO.TaxRatesTOList.Add(igstTO);



                    TblTaxRatesTO cgstTO = new TblTaxRatesTO();
                    cgstTO.TaxTypeId = 2;
                    cgstTO.TaxPct = 50;
                    cgstTO.CreatedOn = new DateTime(2019, 04, 01);
                    cgstTO.CreatedBy = 1;
                    cgstTO.EffectiveFromDt = cgstTO.CreatedOn;
                    cgstTO.IsActive = 1;
                    gstCodeDtlsTO.TaxRatesTOList.Add(cgstTO);



                    TblTaxRatesTO sgstTO = new TblTaxRatesTO();
                    sgstTO.TaxTypeId = 3;
                    sgstTO.TaxPct = 50;
                    sgstTO.CreatedOn = new DateTime(2019, 04, 01);
                    sgstTO.CreatedBy = 1;
                    sgstTO.EffectiveFromDt = sgstTO.CreatedOn;
                    sgstTO.IsActive = 1;
                    gstCodeDtlsTO.TaxRatesTOList.Add(sgstTO);

                    resultMessage = SaveNewGSTCodeDetails(gstCodeDtlsTO);
                    if (resultMessage == null || resultMessage.MessageType != ResultMessageE.Information)
                    {
                        resultMessage.data = gstCodeDtlsTO;
                        ErrorList.Add(resultMessage);
                    }
                }
            }
            resultMessage.DefaultSuccessBehaviour();
            resultMessage.data = ErrorList;
            return resultMessage;
        }

        public ResultMessage MigrateSacGstCodeToSAP()
        {
            ResultMessage resultMessage = new ResultMessage();

            List<TblGstCodeDtlsTO> tblGstCodeDtlsTOList = _iTblGstCodeDtlsDAO.SelectAllTblServiceGstCodeDtlsForMigration();

            List<ResultMessage> ErrorList = new List<ResultMessage>();
            if (tblGstCodeDtlsTOList != null && tblGstCodeDtlsTOList.Count > 0)
            {

                for (int i = 0; i < tblGstCodeDtlsTOList.Count; i++)
                {



                    TblGstCodeDtlsTO gstCodeDtlsTO = tblGstCodeDtlsTOList[i];


                    //gstCodeDtlsTO.CodeDesc = Convert.ToString(dt.Rows[i]["1"]);
                    //gstCodeDtlsTO.CodeNumber = Convert.ToString(dt.Rows[i]["1"]);
                    //gstCodeDtlsTO.TaxPct = Convert.ToDouble(dt.Rows[i]["1"]);
                    //gstCodeDtlsTO.CodeTypeId = Convert.ToInt32(dt.Rows[i]["1"]);

                    gstCodeDtlsTO.IsActive = 1;
                    gstCodeDtlsTO.CreatedBy = 1;
                    gstCodeDtlsTO.CreatedOn = new DateTime(2019, 04, 01);
                    gstCodeDtlsTO.UpdatedBy = 1;
                    gstCodeDtlsTO.UpdatedOn = new DateTime(2019, 04, 01);
                    gstCodeDtlsTO.EffectiveFromDt = new DateTime(2019, 04, 01);


                    gstCodeDtlsTO.TaxRatesTOList = new List<TblTaxRatesTO>();

                    TblTaxRatesTO igstTO = new TblTaxRatesTO();
                    igstTO.TaxTypeId = 1;
                    igstTO.TaxPct = 100;
                    igstTO.CreatedOn = new DateTime(2019, 04, 01);
                    igstTO.CreatedBy = 1;
                    igstTO.EffectiveFromDt = igstTO.CreatedOn;
                    igstTO.IsActive = 1;
                    gstCodeDtlsTO.TaxRatesTOList.Add(igstTO);



                    TblTaxRatesTO cgstTO = new TblTaxRatesTO();
                    cgstTO.TaxTypeId = 2;
                    cgstTO.TaxPct = 50;
                    cgstTO.CreatedOn = new DateTime(2019, 04, 01);
                    cgstTO.CreatedBy = 1;
                    cgstTO.EffectiveFromDt = cgstTO.CreatedOn;
                    cgstTO.IsActive = 1;
                    gstCodeDtlsTO.TaxRatesTOList.Add(cgstTO);



                    TblTaxRatesTO sgstTO = new TblTaxRatesTO();
                    sgstTO.TaxTypeId = 3;
                    sgstTO.TaxPct = 50;
                    sgstTO.CreatedOn = new DateTime(2019, 04, 01);
                    sgstTO.CreatedBy = 1;
                    sgstTO.EffectiveFromDt = sgstTO.CreatedOn;
                    sgstTO.IsActive = 1;
                    gstCodeDtlsTO.TaxRatesTOList.Add(sgstTO);

                    resultMessage = SaveNewGSTCodeDetails(gstCodeDtlsTO);
                    if (resultMessage == null || resultMessage.MessageType != ResultMessageE.Information)
                    {
                        resultMessage.data = gstCodeDtlsTO;
                        ErrorList.Add(resultMessage);
                    }
                }
            }
            resultMessage.DefaultSuccessBehaviour();
            resultMessage.data = ErrorList;
            return resultMessage;
        }
        public ResultMessage PostImportGstCodeDetails(DataTable dataTable)
        {
            ResultMessage resultMsg = new ResultMessage();
            try
            {
                #region 1. Convert Datatable to TblGstCodeDtlsTO List
                List<String> ErrorCodeList = new List<string>();
                List<TblGstCodeDtlsTO> tempTblGstCodeDtlsTOList = ConvertDTToList(dataTable);
                List<TblGstCodeDtlsTO> TblGstCodeDtlsTOList = new List<TblGstCodeDtlsTO>();
                if (tempTblGstCodeDtlsTOList == null || tempTblGstCodeDtlsTOList.Count == 0)
                {
                    resultMsg.DefaultBehaviour("No Records Found");
                    return resultMsg;
                }
                List<TblGstCodeDtlsTO> ErrorTblGstCodeDtlsTOList = tempTblGstCodeDtlsTOList.Where(w => String.IsNullOrEmpty(w.CodeNumber) || String.IsNullOrEmpty(w.TaxPctStr) || (w.CodeTypeId != 1 && w.CodeTypeId != 2)).ToList();
                if (ErrorTblGstCodeDtlsTOList != null && ErrorTblGstCodeDtlsTOList.Count > 0)
                {
                    ErrorTblGstCodeDtlsTOList.ForEach(element =>
                    {
                        ErrorCodeList.Add(element.CodeNumber);
                    });
                }
                tempTblGstCodeDtlsTOList = tempTblGstCodeDtlsTOList.Where(w => !String.IsNullOrEmpty(w.CodeNumber) && !String.IsNullOrEmpty(w.TaxPctStr) && (w.CodeTypeId == 1 || w.CodeTypeId == 2)).ToList();
                if (tempTblGstCodeDtlsTOList != null && tempTblGstCodeDtlsTOList.Count > 0)
                {
                    for (int i = 0; i < tempTblGstCodeDtlsTOList.Count; i++)
                    {
                        TblGstCodeDtlsTO tempTblGstCodeDtlsTO = new TblGstCodeDtlsTO();
                        tempTblGstCodeDtlsTO = tempTblGstCodeDtlsTOList[i];
                        tempTblGstCodeDtlsTO.CodeNumber = tempTblGstCodeDtlsTO.CodeNumber.Replace("[", "");
                        tempTblGstCodeDtlsTO.CodeNumber = tempTblGstCodeDtlsTO.CodeNumber.Replace("]", "");
                        tempTblGstCodeDtlsTO.TaxPctStr = tempTblGstCodeDtlsTO.TaxPctStr.Replace("%", "");
                        List<String> CodeNumberList = tempTblGstCodeDtlsTO.CodeNumber.Split(",").ToList();
                        if (CodeNumberList != null && CodeNumberList.Count > 0)
                        {
                            for (int j = 0; j < CodeNumberList.Count; j++)
                            {
                                CodeNumberList[j] = CodeNumberList[j].Trim();
                                if (int.TryParse(CodeNumberList[j], out int n) && int.TryParse(tempTblGstCodeDtlsTO.TaxPctStr, out int g))
                                {
                                    TblGstCodeDtlsTO tblGstCodeDtlsTO = new TblGstCodeDtlsTO();
                                    tblGstCodeDtlsTO.CodeTypeId = tempTblGstCodeDtlsTO.CodeTypeId;
                                    tblGstCodeDtlsTO.CodeDesc = tempTblGstCodeDtlsTO.CodeDesc;
                                    tblGstCodeDtlsTO.CodeNumber = CodeNumberList[j];
                                    tblGstCodeDtlsTO.TaxPct = Convert.ToDouble(tempTblGstCodeDtlsTO.TaxPctStr);
                                    TblGstCodeDtlsTOList.Add(tblGstCodeDtlsTO);
                                }
                                else
                                {
                                    ErrorCodeList.Add(CodeNumberList[j]);
                                }
                            }
                        }
                    }
                }
                #endregion
                if (TblGstCodeDtlsTOList != null && TblGstCodeDtlsTOList.Count > 0)
                {
                    for (int k = 0; k < TblGstCodeDtlsTOList.Count; k++)
                    {
                        TblGstCodeDtlsTO gstCodeDtlsTO = TblGstCodeDtlsTOList[k];

                        gstCodeDtlsTO.IsActive = 1;
                        gstCodeDtlsTO.CreatedBy = 1;
                        gstCodeDtlsTO.CreatedOn = new DateTime(2019, 04, 01);
                        gstCodeDtlsTO.UpdatedBy = 1;
                        gstCodeDtlsTO.UpdatedOn = new DateTime(2019, 04, 01);
                        gstCodeDtlsTO.EffectiveFromDt = new DateTime(2019, 04, 01);

                        gstCodeDtlsTO.TaxRatesTOList = new List<TblTaxRatesTO>();

                        TblTaxRatesTO igstTO = new TblTaxRatesTO();
                        igstTO.TaxTypeId = 1;
                        igstTO.TaxPct = 100;
                        igstTO.CreatedOn = new DateTime(2019, 04, 01);
                        igstTO.CreatedBy = 1;
                        igstTO.EffectiveFromDt = igstTO.CreatedOn;
                        igstTO.IsActive = 1;
                        gstCodeDtlsTO.TaxRatesTOList.Add(igstTO);

                        TblTaxRatesTO cgstTO = new TblTaxRatesTO();
                        cgstTO.TaxTypeId = 2;
                        cgstTO.TaxPct = 50;
                        cgstTO.CreatedOn = new DateTime(2019, 04, 01);
                        cgstTO.CreatedBy = 1;
                        cgstTO.EffectiveFromDt = cgstTO.CreatedOn;
                        cgstTO.IsActive = 1;
                        gstCodeDtlsTO.TaxRatesTOList.Add(cgstTO);

                        TblTaxRatesTO sgstTO = new TblTaxRatesTO();
                        sgstTO.TaxTypeId = 3;
                        sgstTO.TaxPct = 50;
                        sgstTO.CreatedOn = new DateTime(2019, 04, 01);
                        sgstTO.CreatedBy = 1;
                        sgstTO.EffectiveFromDt = sgstTO.CreatedOn;
                        sgstTO.IsActive = 1;
                        gstCodeDtlsTO.TaxRatesTOList.Add(sgstTO);

                        resultMsg = PostImportGstCodeDetails(gstCodeDtlsTO);
                        if (resultMsg == null || resultMsg.MessageType != ResultMessageE.Information)
                        {
                            ErrorCodeList.Add(gstCodeDtlsTO.CodeNumber);
                        }
                    }
                }
                resultMsg.DefaultSuccessBehaviour();
                resultMsg.data = ErrorCodeList;
                return resultMsg;

            }
            catch (Exception ex)
            {
                resultMsg.DefaultExceptionBehaviour(ex, "PostImportGstCodeDetails");
                return resultMsg;
            }
            finally
            {

            }
        }
        public ResultMessage PostImportGstCodeDetails(TblGstCodeDtlsTO gstCodeDtlsTO)
        {
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            ResultMessage resultMsg = new ResultMessage();
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                int SAPServiceEnable = 0;
                TblConfigParamsTO tblConfigParamsTOSAPService = _iTblConfigParamsDAO.SelectTblConfigParams(Constants.SAPB1_SERVICES_ENABLE);
                if (tblConfigParamsTOSAPService != null)
                {
                    if (Convert.ToInt32(tblConfigParamsTOSAPService.ConfigParamVal) == 1)
                    {
                        SAPServiceEnable = 1;
                    }
                }
                if (gstCodeDtlsTO.CodeNumber.Length > 8 || gstCodeDtlsTO.CodeNumber.Length < 4)
                {
                    tran.Rollback();
                    resultMsg.DefaultBehaviour("Invalid Tax Code");
                    return resultMsg;
                }
                int result = 0;
                TblGstCodeDtlsTO existingTblGstCodeDtlsTO = _iTblGstCodeDtlsDAO.SelectTblGstCodeDtls(gstCodeDtlsTO.CodeNumber, gstCodeDtlsTO.CodeTypeId, conn, tran);
                if (existingTblGstCodeDtlsTO == null)
                {
                    result = InsertTblGstCodeDtls(gstCodeDtlsTO, conn, tran);
                    if (result != 1)
                    {
                        tran.Rollback();
                        resultMsg.DefaultBehaviour("Error While InsertTblGstCodeDtls");
                        return resultMsg;
                    }
                }
                else
                {
                    existingTblGstCodeDtlsTO.TaxRatesTOList = _iTblTaxRatesDAO.SelectAllTblTaxRates(existingTblGstCodeDtlsTO.IdGstCode, conn, tran);
                    if (existingTblGstCodeDtlsTO.TaxRatesTOList == null || existingTblGstCodeDtlsTO.TaxRatesTOList.Count == 0)
                    {
                        tran.Rollback();
                        resultMsg.DefaultBehaviour("Failed To Get Tax Rate Details List");
                        return resultMsg;
                    }
                    gstCodeDtlsTO.IdGstCode = existingTblGstCodeDtlsTO.IdGstCode;
                    gstCodeDtlsTO.TaxRatesTOList = existingTblGstCodeDtlsTO.TaxRatesTOList;
                    result = UpdateTblGstCodeDtls(gstCodeDtlsTO, conn, tran);
                    if (result != 1)
                    {
                        tran.Rollback();
                        resultMsg.DefaultBehaviour("Error While InsertTblGstCodeDtls");
                        return resultMsg;
                    }
                }

                if (gstCodeDtlsTO.TaxRatesTOList == null)
                {
                    tran.Rollback();
                    resultMsg.DefaultBehaviour("TaxRatesTOList Found NULL");
                    return resultMsg;
                }

                for (int i = 0; i < gstCodeDtlsTO.TaxRatesTOList.Count; i++)
                {
                    gstCodeDtlsTO.TaxRatesTOList[i].CreatedBy = gstCodeDtlsTO.CreatedBy;
                    gstCodeDtlsTO.TaxRatesTOList[i].CreatedOn = gstCodeDtlsTO.CreatedOn;
                    gstCodeDtlsTO.TaxRatesTOList[i].UpdatedBy = gstCodeDtlsTO.CreatedBy;
                    gstCodeDtlsTO.TaxRatesTOList[i].UpdatedOn = gstCodeDtlsTO.CreatedOn;
                    gstCodeDtlsTO.TaxRatesTOList[i].IsActive = 1;
                    gstCodeDtlsTO.TaxRatesTOList[i].GstCodeId = gstCodeDtlsTO.IdGstCode;
                    if (SAPServiceEnable == 1)
                    {
                        gstCodeDtlsTO.TaxRatesTOList[i].SapTaxCode = getSAPMappedGstTaxCode(gstCodeDtlsTO.TaxRatesTOList[i].TaxTypeId, gstCodeDtlsTO.TaxPct);
                        if (gstCodeDtlsTO.TaxRatesTOList[i].SapTaxCode == "")
                        {
                            tran.Rollback();
                            resultMsg.DefaultBehaviour("Get Mapped SAP Tax Code Failed - Invalid Percentage Value");
                            return resultMsg;
                        }
                    }
                    if (existingTblGstCodeDtlsTO == null)
                    {
                        result = _iTblTaxRatesDAO.InsertTblTaxRates(gstCodeDtlsTO.TaxRatesTOList[i], conn, tran);
                        if (result != 1)
                        {
                            tran.Rollback();
                            resultMsg.DefaultBehaviour("Error While InsertTblTaxRates");
                            return resultMsg;
                        }
                    }
                    else
                    {
                        result = _iTblTaxRatesDAO.UpdateTblTaxRates(gstCodeDtlsTO.TaxRatesTOList[i], conn, tran);
                        if (result != 1)
                        {
                            tran.Rollback();
                            resultMsg.DefaultBehaviour("Error While InsertTblTaxRates");
                            return resultMsg;
                        }
                    }

                }
                if (SAPServiceEnable == 1 && existingTblGstCodeDtlsTO == null)
                {
                    Int32 absEntry;
                    gstCodeDtlsTO.Chapter = gstCodeDtlsTO.CodeNumber.Substring(0, 2);
                    gstCodeDtlsTO.Heading = gstCodeDtlsTO.CodeNumber.Substring(2, 2);
                    gstCodeDtlsTO.SubHeading = gstCodeDtlsTO.CodeNumber.Remove(0, 4);
                    gstCodeDtlsTO.ChapterID = gstCodeDtlsTO.Chapter + "." + gstCodeDtlsTO.Heading;
                    if (!String.IsNullOrEmpty(gstCodeDtlsTO.SubHeading))
                        gstCodeDtlsTO.ChapterID = gstCodeDtlsTO.ChapterID + "." + gstCodeDtlsTO.SubHeading;
                    if (gstCodeDtlsTO.CodeDesc.Length > 50)
                        gstCodeDtlsTO.CodeDesc = gstCodeDtlsTO.CodeDesc.Substring(0, 50);
                    result = GetIdentityOfSAPTable("AbsEntry", "OCHP");
                    if (result == -1)
                    {
                        tran.Rollback();
                        resultMsg.DefaultBehaviour("Error While GetIdentityOfSAPTable");
                        return resultMsg;
                    }
                    gstCodeDtlsTO.AbsEntry = result;

                    result = _iTblGstCodeDtlsDAO.InsertGstCodeDtlsInSAP(gstCodeDtlsTO);
                    if (result != 1)
                    {
                        tran.Rollback();
                        resultMsg.DefaultBehaviour("Error While InsertGstCodeDtlsInSAP");
                        return resultMsg;
                    }
                    gstCodeDtlsTO.SapHsnCode = gstCodeDtlsTO.AbsEntry.ToString();
                    result = _iTblGstCodeDtlsDAO.UpdateMappedHsnCodeOfGstDtls(gstCodeDtlsTO, conn, tran);
                    if (result != 1)
                    {
                        tran.Rollback();
                        resultMsg.DefaultBehaviour("Error While UpdateMappedHsnCodeOfGstDtls");
                        return resultMsg;
                    }
                }
                tran.Commit();
                resultMsg.DefaultSuccessBehaviour();
                return resultMsg;

            }
            catch (Exception ex)
            {
                tran.Rollback();
                resultMsg.DefaultExceptionBehaviour(ex, "PostImportGstCodeDetails");
                return resultMsg;
            }
            finally
            {
                conn.Close();
            }
        }
        public List<TblGstCodeDtlsTO> ConvertDTToList(DataTable tblGstCodeDtlsTODT)
        {
            List<TblGstCodeDtlsTO> tblGstCodeDtlsTOList = new List<TblGstCodeDtlsTO>();
            if (tblGstCodeDtlsTODT != null)
            {
                for (int rowCount = 0; rowCount < tblGstCodeDtlsTODT.Rows.Count; rowCount++)
                {
                    TblGstCodeDtlsTO tblGstCodeDtlsTO = new TblGstCodeDtlsTO();
                    if (tblGstCodeDtlsTODT.Rows[rowCount]["Code"] != DBNull.Value)
                        tblGstCodeDtlsTO.CodeNumber = Convert.ToString(tblGstCodeDtlsTODT.Rows[rowCount]["Code"].ToString());

                    if (tblGstCodeDtlsTODT.Rows[rowCount]["Description"] != DBNull.Value)
                        tblGstCodeDtlsTO.CodeDesc = Convert.ToString(tblGstCodeDtlsTODT.Rows[rowCount]["Description"].ToString());

                    if (tblGstCodeDtlsTODT.Rows[rowCount]["Type"] != DBNull.Value)
                        tblGstCodeDtlsTO.CodeTypeId = Convert.ToInt32(tblGstCodeDtlsTODT.Rows[rowCount]["Type"].ToString());

                    if (tblGstCodeDtlsTODT.Rows[rowCount]["Percentage"] != DBNull.Value)
                        tblGstCodeDtlsTO.TaxPctStr = Convert.ToString(tblGstCodeDtlsTODT.Rows[rowCount]["Percentage"].ToString());

                    tblGstCodeDtlsTOList.Add(tblGstCodeDtlsTO);
                }
            }
            return tblGstCodeDtlsTOList;
        }
        public ResultMessage SaveNewGSTCodeDetails(TblGstCodeDtlsTO gstCodeDtlsTO)
        {
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            ResultMessage resultMsg = new ResultMessage();
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                int SAPServiceEnable = 0;
                TblConfigParamsTO tblConfigParamsTOSAPService = _iTblConfigParamsDAO.SelectTblConfigParams(Constants.SAPB1_SERVICES_ENABLE);
                if (tblConfigParamsTOSAPService != null)
                {
                    if (Convert.ToInt32(tblConfigParamsTOSAPService.ConfigParamVal) == 1)
                    {
                        SAPServiceEnable = 1;
                    }
                }
                if (gstCodeDtlsTO.CodeNumber.Length > 8 || gstCodeDtlsTO.CodeNumber.Length < 4)
                {
                    tran.Rollback();
                    resultMsg.DefaultBehaviour("Invalid Tax Code");
                    return resultMsg;
                }
                int result = InsertTblGstCodeDtls(gstCodeDtlsTO, conn, tran);
                if (result != 1)
                {
                    tran.Rollback();
                    resultMsg.DefaultBehaviour("Error While InsertTblGstCodeDtls");
                    return resultMsg;
                }

                if (gstCodeDtlsTO.TaxRatesTOList == null)
                {
                    tran.Rollback();
                    resultMsg.DefaultBehaviour("TaxRatesTOList Found NULL");
                    return resultMsg;
                }

                for (int i = 0; i < gstCodeDtlsTO.TaxRatesTOList.Count; i++)
                {
                    gstCodeDtlsTO.TaxRatesTOList[i].CreatedBy = gstCodeDtlsTO.CreatedBy;
                    gstCodeDtlsTO.TaxRatesTOList[i].CreatedOn = gstCodeDtlsTO.CreatedOn;
                    gstCodeDtlsTO.TaxRatesTOList[i].IsActive = 1;
                    gstCodeDtlsTO.TaxRatesTOList[i].GstCodeId = gstCodeDtlsTO.IdGstCode;
                    if (SAPServiceEnable == 1)
                    {
                        gstCodeDtlsTO.TaxRatesTOList[i].SapTaxCode = getSAPMappedGstTaxCode(gstCodeDtlsTO.TaxRatesTOList[i].TaxTypeId, gstCodeDtlsTO.TaxPct);
                        if (gstCodeDtlsTO.TaxRatesTOList[i].SapTaxCode == "")
                        {
                            tran.Rollback();
                            resultMsg.DefaultBehaviour("Get Mapped SAP Tax Code Failed - Invalid Percentage Value");
                            return resultMsg;
                        }
                    }

                    result = _iTblTaxRatesDAO.InsertTblTaxRates(gstCodeDtlsTO.TaxRatesTOList[i], conn, tran);
                    if (result != 1)
                    {
                        tran.Rollback();
                        resultMsg.DefaultBehaviour("Error While InsertTblTaxRates");
                        return resultMsg;
                    }
                }

                if (SAPServiceEnable == 1 && gstCodeDtlsTO.CodeTypeId == 1)
                {
                    Int32 absEntry;
                    gstCodeDtlsTO.Chapter = gstCodeDtlsTO.CodeNumber.Substring(0, 2);
                    gstCodeDtlsTO.Heading = gstCodeDtlsTO.CodeNumber.Substring(2, 2);
                    gstCodeDtlsTO.SubHeading = gstCodeDtlsTO.CodeNumber.Remove(0, 4);
                    gstCodeDtlsTO.ChapterID = gstCodeDtlsTO.Chapter + "." + gstCodeDtlsTO.Heading;
                    if (!String.IsNullOrEmpty(gstCodeDtlsTO.SubHeading))
                        gstCodeDtlsTO.ChapterID = gstCodeDtlsTO.ChapterID + "." + gstCodeDtlsTO.SubHeading;
                    if (gstCodeDtlsTO.CodeDesc.Length > 50)
                        gstCodeDtlsTO.CodeDesc = gstCodeDtlsTO.CodeDesc.Substring(0, 50);
                    //Reshma Added For already migrated data.
                    Int32 absEntry1 = GetGSTDataSAPTable(gstCodeDtlsTO.ChapterID);
                    if (absEntry1 == 0)
                    {
                        result = GetIdentityOfSAPTable("AbsEntry", "OCHP");
                        if (result == -1)
                        {
                            tran.Rollback();
                            resultMsg.DefaultBehaviour("Error While GetIdentityOfSAPTable");
                            return resultMsg;
                        }
                        gstCodeDtlsTO.AbsEntry = result;

                        result = _iTblGstCodeDtlsDAO.InsertGstCodeDtlsInSAP(gstCodeDtlsTO);
                        if (result != 1)
                        {
                            tran.Rollback();
                            resultMsg.DefaultBehaviour("Error While InsertGstCodeDtlsInSAP");
                            return resultMsg;
                        }
                        gstCodeDtlsTO.SapHsnCode = gstCodeDtlsTO.AbsEntry.ToString();
                    }
                    else
                        gstCodeDtlsTO.SapHsnCode = absEntry1.ToString();

                    result = _iTblGstCodeDtlsDAO.UpdateMappedHsnCodeOfGstDtls(gstCodeDtlsTO, conn, tran);
                    if (result != 1)
                    {
                        tran.Rollback();
                        resultMsg.DefaultBehaviour("Error While UpdateMappedHsnCodeOfGstDtls");
                        return resultMsg;
                    }

                }
                else if (SAPServiceEnable == 1 && gstCodeDtlsTO.CodeTypeId == 2)
                {
                    double absEntry;
                    //gstCodeDtlsTO.Chapter = gstCodeDtlsTO.CodeNumber.Substring(0, 2);
                    //gstCodeDtlsTO.Heading = gstCodeDtlsTO.CodeNumber.Substring(2, 2);
                    //gstCodeDtlsTO.SubHeading = gstCodeDtlsTO.CodeNumber.Remove(0, 4);
                    //gstCodeDtlsTO.ChapterID = gstCodeDtlsTO.Chapter + "." + gstCodeDtlsTO.Heading;
                    //if (!String.IsNullOrEmpty(gstCodeDtlsTO.SubHeading))
                    //    gstCodeDtlsTO.ChapterID = gstCodeDtlsTO.ChapterID + "." + gstCodeDtlsTO.SubHeading;
                    //if (gstCodeDtlsTO.CodeDesc.Length > 50)
                    //    gstCodeDtlsTO.CodeDesc = gstCodeDtlsTO.CodeDesc.Substring(0, 50);
                    //Reshma Added For already migrated data.
                    double absEntry1 = GetGSTDataSAPFromSACTable(gstCodeDtlsTO.CodeNumber);
                    if (absEntry1 == 0)
                    {
                        result = GetIdentityOfSAPTable("AbsEntry", "OSAC");
                        if (result == -1)
                        {
                            tran.Rollback();
                            resultMsg.DefaultBehaviour("Error While GetIdentityOfSAPTable");
                            return resultMsg;
                        }
                        gstCodeDtlsTO.AbsEntry = result;

                        result = _iTblGstCodeDtlsDAO.InsertGstCodeDtlsInSAPV2(gstCodeDtlsTO);
                        if (result != 1)
                        {
                            tran.Rollback();
                            resultMsg.DefaultBehaviour("Error While InsertGstCodeDtlsInSAP");
                            return resultMsg;
                        }
                        gstCodeDtlsTO.SapHsnCode = gstCodeDtlsTO.AbsEntry.ToString();
                    }
                    else
                        gstCodeDtlsTO.SapHsnCode = absEntry1.ToString();

                    result = _iTblGstCodeDtlsDAO.UpdateMappedHsnCodeOfGstDtls(gstCodeDtlsTO, conn, tran);
                    if (result != 1)
                    {
                        tran.Rollback();
                        resultMsg.DefaultBehaviour("Error While UpdateMappedHsnCodeOfGstDtls");
                        return resultMsg;
                    }

                }
                tran.Commit();
                resultMsg.DefaultSuccessBehaviour();
                return resultMsg;

            }
            catch (Exception ex)
            {
                tran.Rollback();
                resultMsg.DefaultExceptionBehaviour(ex, "SaveNewGSTCodeDetails");
                return resultMsg;
            }
            finally
            {
                conn.Close();
            }
        }
        public Int32 GetIdentityOfSAPTable(string columnName, string tableName)
        {
            var query = "SELECT name FROM sys.identity_columns WHERE OBJECT_NAME(object_id) = " + "'" + tableName + "'";
            int result = _iDimensionDAO.GetMaxCountOfSAPTable(columnName, tableName);
            if (result == -1)
                return result;
            return result + 1;
        }

        public Int32 GetGSTDataSAPTable(string chapterId)
        {
            string query = "SELECT AbsEntry FROM ochp WHERE ChapterID = " + "'" + chapterId + "'";
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.SAP_CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = null;
            try
            {
                conn.Open();
                string Query = query;
                cmdSelect = new SqlCommand(Query, conn);
                object dateReader = cmdSelect.ExecuteScalar();
                return Convert.ToInt32(dateReader);
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        private double GetGSTDataSAPFromSACTable(string chapterId)
        {
            string query = "SELECT AbsEntry FROM OSAC WHERE ServCode = " + "'" + chapterId + "'";
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.SAP_CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = null;
            try
            {
                conn.Open();
                string Query = query;
                cmdSelect = new SqlCommand(Query, conn);
                object dateReader = cmdSelect.ExecuteScalar();
                return Convert.ToDouble(dateReader);
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        #endregion

        #region Updation
        public int UpdateTblGstCodeDtls(TblGstCodeDtlsTO tblGstCodeDtlsTO)
        {
            return _iTblGstCodeDtlsDAO.UpdateTblGstCodeDtls(tblGstCodeDtlsTO);
        }

        public int UpdateTblGstCodeDtls(TblGstCodeDtlsTO tblGstCodeDtlsTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblGstCodeDtlsDAO.UpdateTblGstCodeDtls(tblGstCodeDtlsTO, conn, tran);
        }

        public ResultMessage UpdateNewGSTCodeDetails(TblGstCodeDtlsTO gstCodeDtlsTO)
        {
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            ResultMessage resultMsg = new ResultMessage();
            try
            {
                TblGstCodeDtlsTO existingGstCodeDtlsTO = SelectTblGstCodeDtlsTO(gstCodeDtlsTO.IdGstCode);

                conn.Open();
                tran = conn.BeginTransaction();

                if (existingGstCodeDtlsTO == null)
                {
                    tran.Rollback();
                    resultMsg.DefaultBehaviour("existingGstCodeDtlsTO found NULL");
                    return resultMsg;
                }
                int result = 0;
                List<TblTaxRatesTO> list = _iTblTaxRatesDAO.SelectAllTblTaxRates(existingGstCodeDtlsTO.IdGstCode, conn, tran);
                List<TblProdGstCodeDtlsTO> prodItemList = _iTblProdGstCodeDtlsDAO.SelectAllTblProdGstCodeDtls(existingGstCodeDtlsTO.IdGstCode, conn, tran);
                if (existingGstCodeDtlsTO.TaxPct != gstCodeDtlsTO.TaxPct)
                {
                    existingGstCodeDtlsTO.EffectiveToDt = gstCodeDtlsTO.UpdatedOn;
                    existingGstCodeDtlsTO.IsActive = 0;
                    existingGstCodeDtlsTO.UpdatedBy = gstCodeDtlsTO.UpdatedBy;
                    existingGstCodeDtlsTO.UpdatedOn = gstCodeDtlsTO.UpdatedOn;

                    result = UpdateTblGstCodeDtls(existingGstCodeDtlsTO, conn, tran);
                    if (result != 1)
                    {
                        tran.Rollback();
                        resultMsg.DefaultBehaviour("Error While InsertTblGstCodeDtls");
                        return resultMsg;
                    }

                    //Deactivate all taxes

                    for (int t = 0; t < list.Count; t++)
                    {
                        list[t].IsActive = 0;
                        list[t].EffectiveToDt = existingGstCodeDtlsTO.EffectiveToDt;
                        list[t].UpdatedBy = existingGstCodeDtlsTO.UpdatedBy;
                        list[t].UpdatedOn = existingGstCodeDtlsTO.UpdatedOn;
                        result = _iTblTaxRatesDAO.UpdateTblTaxRates(list[t], conn, tran);

                        if (result != 1)
                        {
                            tran.Rollback();
                            resultMsg.DefaultBehaviour("Error While UpdateTblTaxRates");
                            return resultMsg;
                        }
                    }

                    for (int pi = 0; pi < prodItemList.Count; pi++)
                    {
                        prodItemList[pi].IsActive = 0;
                        prodItemList[pi].EffectiveTodt = existingGstCodeDtlsTO.EffectiveToDt;
                        prodItemList[pi].UpdatedOn = existingGstCodeDtlsTO.UpdatedOn;
                        prodItemList[pi].UpdatedBy = existingGstCodeDtlsTO.UpdatedBy;
                        result = _iTblProdGstCodeDtlsDAO.UpdateTblProdGstCodeDtls(prodItemList[pi], conn, tran);
                        if (result != 1)
                        {
                            tran.Rollback();
                            resultMsg.DefaultBehaviour("Error While UpdateTblTaxRates");
                            return resultMsg;
                        }
                    }

                    // Insert New 
                    gstCodeDtlsTO.IsActive = 1;
                    gstCodeDtlsTO.CreatedBy = gstCodeDtlsTO.UpdatedBy;
                    gstCodeDtlsTO.CreatedOn = gstCodeDtlsTO.UpdatedOn;
                    gstCodeDtlsTO.EffectiveFromDt = gstCodeDtlsTO.UpdatedOn;
                    gstCodeDtlsTO.EffectiveToDt = DateTime.MinValue;
                    result = InsertTblGstCodeDtls(gstCodeDtlsTO, conn, tran);
                    if (result != 1)
                    {
                        tran.Rollback();
                        resultMsg.DefaultBehaviour("Error While InsertTblGstCodeDtls");
                        return resultMsg;
                    }
                    result = _iTblGstCodeDtlsDAO.UpdateMappedHsnCodeOfGstDtls(gstCodeDtlsTO, conn, tran);
                    if (result != 1)
                    {
                        tran.Rollback();
                        resultMsg.DefaultBehaviour("Error While UpdateMappedHsnCodeOfGstDtls");
                        return resultMsg;
                    }
                    if (gstCodeDtlsTO.TaxRatesTOList == null)
                    {
                        tran.Rollback();
                        resultMsg.DefaultBehaviour("TaxRatesTOList Found NULL");
                        return resultMsg;
                    }

                    for (int i = 0; i < gstCodeDtlsTO.TaxRatesTOList.Count; i++)
                    {
                        gstCodeDtlsTO.TaxRatesTOList[i].GstCodeId = gstCodeDtlsTO.IdGstCode;
                        gstCodeDtlsTO.TaxRatesTOList[i].CreatedBy = gstCodeDtlsTO.CreatedBy;
                        gstCodeDtlsTO.TaxRatesTOList[i].CreatedOn = gstCodeDtlsTO.CreatedOn;
                        gstCodeDtlsTO.TaxRatesTOList[i].EffectiveFromDt = gstCodeDtlsTO.EffectiveFromDt;
                        gstCodeDtlsTO.TaxRatesTOList[i].EffectiveToDt = DateTime.MinValue;
                        gstCodeDtlsTO.TaxRatesTOList[i].SapTaxCode = getSAPMappedGstTaxCode(gstCodeDtlsTO.TaxRatesTOList[i].TaxTypeId, gstCodeDtlsTO.TaxPct);
                        result = _iTblTaxRatesDAO.InsertTblTaxRates(gstCodeDtlsTO.TaxRatesTOList[i], conn, tran);
                        if (result != 1)
                        {
                            tran.Rollback();
                            resultMsg.DefaultBehaviour("Error While InsertTblTaxRates");
                            return resultMsg;
                        }
                    }

                    for (int pi = 0; pi < prodItemList.Count; pi++)
                    {
                        prodItemList[pi].IsActive = 1;
                        prodItemList[pi].EffectiveFromDt = gstCodeDtlsTO.EffectiveFromDt;
                        prodItemList[pi].CreatedOn = gstCodeDtlsTO.CreatedOn;
                        prodItemList[pi].CreatedBy = gstCodeDtlsTO.CreatedBy;
                        prodItemList[pi].EffectiveTodt = DateTime.MinValue;
                        prodItemList[pi].GstCodeId = gstCodeDtlsTO.IdGstCode;
                        result = _iTblProdGstCodeDtlsDAO.InsertTblProdGstCodeDtls(prodItemList[pi], conn, tran);
                        if (result != 1)
                        {
                            tran.Rollback();
                            resultMsg.DefaultBehaviour("Error While InsertTblProdGstCodeDtls");
                            return resultMsg;
                        }
                    }
                }
                else
                {

                    result = UpdateTblGstCodeDtls(gstCodeDtlsTO, conn, tran);
                    if (result != 1)
                    {
                        tran.Rollback();
                        resultMsg.DefaultBehaviour("Error While UpdateTblGstCodeDtls");
                        return resultMsg;
                    }

                    if (gstCodeDtlsTO.TaxRatesTOList == null)
                    {
                        tran.Rollback();
                        resultMsg.DefaultBehaviour("TaxRatesTOList Found NULL");
                        return resultMsg;
                    }

                    for (int i = 0; i < gstCodeDtlsTO.TaxRatesTOList.Count; i++)
                    {

                        var existingTaxRateTO = list.Where(a => a.TaxTypeId == gstCodeDtlsTO.TaxRatesTOList[i].TaxTypeId && a.IsActive == 1).FirstOrDefault();
                        if (existingTaxRateTO != null)
                        {
                            if (existingTaxRateTO.TaxPct != gstCodeDtlsTO.TaxRatesTOList[i].TaxPct)
                            {
                                existingTaxRateTO.IsActive = 0;
                                existingTaxRateTO.UpdatedBy = gstCodeDtlsTO.UpdatedBy;
                                existingTaxRateTO.UpdatedOn = gstCodeDtlsTO.UpdatedOn;
                                existingTaxRateTO.EffectiveToDt = gstCodeDtlsTO.UpdatedOn;

                                result = _iTblTaxRatesDAO.UpdateTblTaxRates(existingTaxRateTO, conn, tran);
                                if (result != 1)
                                {
                                    tran.Rollback();
                                    resultMsg.DefaultBehaviour("Error While UpdateTblTaxRates");
                                    return resultMsg;
                                }

                                //Insert New One
                                gstCodeDtlsTO.TaxRatesTOList[i].EffectiveFromDt = gstCodeDtlsTO.UpdatedOn;
                                gstCodeDtlsTO.TaxRatesTOList[i].CreatedBy = gstCodeDtlsTO.UpdatedBy;
                                gstCodeDtlsTO.TaxRatesTOList[i].CreatedOn = gstCodeDtlsTO.UpdatedOn;
                                result = _iTblTaxRatesDAO.InsertTblTaxRates(gstCodeDtlsTO.TaxRatesTOList[i], conn, tran);
                                if (result != 1)
                                {
                                    tran.Rollback();
                                    resultMsg.DefaultBehaviour("Error While InsertTblTaxRates");
                                    return resultMsg;
                                }
                            }
                            else
                            {
                                gstCodeDtlsTO.TaxRatesTOList[i].UpdatedBy = gstCodeDtlsTO.UpdatedBy;
                                gstCodeDtlsTO.TaxRatesTOList[i].UpdatedOn = gstCodeDtlsTO.UpdatedOn;
                                result = _iTblTaxRatesDAO.UpdateTblTaxRates(gstCodeDtlsTO.TaxRatesTOList[i], conn, tran);
                                if (result != 1)
                                {
                                    tran.Rollback();
                                    resultMsg.DefaultBehaviour("Error While UpdateTblTaxRates");
                                    return resultMsg;
                                }
                            }
                        }
                        else
                        {
                            gstCodeDtlsTO.TaxRatesTOList[i].CreatedBy = gstCodeDtlsTO.UpdatedBy;
                            gstCodeDtlsTO.TaxRatesTOList[i].CreatedOn = gstCodeDtlsTO.UpdatedOn;
                            result = _iTblTaxRatesDAO.InsertTblTaxRates(gstCodeDtlsTO.TaxRatesTOList[i], conn, tran);
                            if (result != 1)
                            {
                                tran.Rollback();
                                resultMsg.DefaultBehaviour("Error While InsertTblTaxRates");
                                return resultMsg;
                            }
                        }
                    }
                }

                tran.Commit();
                resultMsg.DefaultSuccessBehaviour();
                return resultMsg;

            }
            catch (Exception ex)
            {
                tran.Rollback();
                resultMsg.DefaultExceptionBehaviour(ex, "UpdateNewGSTCodeDetails");
                return resultMsg;
            }
            finally
            {
                conn.Close();
            }
        }

        //Priyanka [26-06-2019] : Added for update the gst code details against existing item.
        //public ResultMessage UpdateNewGSTCodeDetailsForProductItem(TblGstCodeDtlsTO gstCodeDtlsTO, SqlConnection conn, SqlTransaction tran)
        //{
        //  //  SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
        //  //  SqlTransaction tran = null;
        //    ResultMessage resultMsg = new ResultMessage();
        //    try
        //    {
        //        TblGstCodeDtlsTO existingGstCodeDtlsTO = SelectTblGstCodeDtlsTO(gstCodeDtlsTO.IdGstCode);

        //   //     conn.Open();
        //        tran = conn.BeginTransaction();

        //        if (existingGstCodeDtlsTO == null)
        //        {
        //            tran.Rollback();
        //            resultMsg.DefaultBehaviour("existingGstCodeDtlsTO found NULL");
        //            return resultMsg;
        //        }
        //        int result = 0;
        //        List<TblTaxRatesTO> list = _iTblTaxRatesDAO.SelectAllTblTaxRates(existingGstCodeDtlsTO.IdGstCode, conn, tran);
        //        List<TblProdGstCodeDtlsTO> prodItemList = _iTblProdGstCodeDtlsDAO.SelectAllTblProdGstCodeDtls(existingGstCodeDtlsTO.IdGstCode, conn, tran);
        //        if (existingGstCodeDtlsTO.TaxPct != gstCodeDtlsTO.TaxPct)
        //        {
        //            existingGstCodeDtlsTO.EffectiveToDt = gstCodeDtlsTO.UpdatedOn;
        //            existingGstCodeDtlsTO.IsActive = 0;
        //            existingGstCodeDtlsTO.UpdatedBy = gstCodeDtlsTO.UpdatedBy;
        //            existingGstCodeDtlsTO.UpdatedOn = gstCodeDtlsTO.UpdatedOn;

        //            result = UpdateTblGstCodeDtls(existingGstCodeDtlsTO, conn, tran);
        //            if (result != 1)
        //            {
        //                tran.Rollback();
        //                resultMsg.DefaultBehaviour("Error While InsertTblGstCodeDtls");
        //                return resultMsg;
        //            }

        //            //Deactivate all taxes

        //            for (int t = 0; t < list.Count; t++)
        //            {
        //                list[t].IsActive = 0;
        //                list[t].EffectiveToDt = existingGstCodeDtlsTO.EffectiveToDt;
        //                list[t].UpdatedBy = existingGstCodeDtlsTO.UpdatedBy;
        //                list[t].UpdatedOn = existingGstCodeDtlsTO.UpdatedOn;
        //                result = _iTblTaxRatesDAO.UpdateTblTaxRates(list[t], conn, tran);

        //                if (result != 1)
        //                {
        //                    tran.Rollback();
        //                    resultMsg.DefaultBehaviour("Error While UpdateTblTaxRates");
        //                    return resultMsg;
        //                }
        //            }

        //            for (int pi = 0; pi < prodItemList.Count; pi++)
        //            {
        //                prodItemList[pi].IsActive = 0;
        //                prodItemList[pi].EffectiveTodt = existingGstCodeDtlsTO.EffectiveToDt;
        //                prodItemList[pi].UpdatedOn = existingGstCodeDtlsTO.UpdatedOn;
        //                prodItemList[pi].UpdatedBy = existingGstCodeDtlsTO.UpdatedBy;
        //                result = _iTblProdGstCodeDtlsDAO.UpdateTblProdGstCodeDtls(prodItemList[pi], conn, tran);
        //                if (result != 1)
        //                {
        //                    tran.Rollback();
        //                    resultMsg.DefaultBehaviour("Error While UpdateTblTaxRates");
        //                    return resultMsg;
        //                }
        //            }

        //            // Insert New 
        //            gstCodeDtlsTO.IsActive = 1;
        //            gstCodeDtlsTO.CreatedBy = gstCodeDtlsTO.UpdatedBy;
        //            gstCodeDtlsTO.CreatedOn = gstCodeDtlsTO.UpdatedOn;
        //            gstCodeDtlsTO.EffectiveFromDt = gstCodeDtlsTO.UpdatedOn;
        //            gstCodeDtlsTO.EffectiveToDt = DateTime.MinValue;
        //            result = InsertTblGstCodeDtls(gstCodeDtlsTO, conn, tran);
        //            if (result != 1)
        //            {
        //                tran.Rollback();
        //                resultMsg.DefaultBehaviour("Error While InsertTblGstCodeDtls");
        //                return resultMsg;
        //            }

        //            if (gstCodeDtlsTO.TaxRatesTOList == null)
        //            {
        //                tran.Rollback();
        //                resultMsg.DefaultBehaviour("TaxRatesTOList Found NULL");
        //                return resultMsg;
        //            }

        //            for (int i = 0; i < gstCodeDtlsTO.TaxRatesTOList.Count; i++)
        //            {
        //                gstCodeDtlsTO.TaxRatesTOList[i].GstCodeId = gstCodeDtlsTO.IdGstCode;
        //                gstCodeDtlsTO.TaxRatesTOList[i].CreatedBy = gstCodeDtlsTO.CreatedBy;
        //                gstCodeDtlsTO.TaxRatesTOList[i].CreatedOn = gstCodeDtlsTO.CreatedOn;
        //                gstCodeDtlsTO.TaxRatesTOList[i].EffectiveFromDt = gstCodeDtlsTO.EffectiveFromDt;
        //                gstCodeDtlsTO.TaxRatesTOList[i].EffectiveToDt = DateTime.MinValue;
        //                result = _iTblTaxRatesDAO.InsertTblTaxRates(gstCodeDtlsTO.TaxRatesTOList[i], conn, tran);
        //                if (result != 1)
        //                {
        //                    tran.Rollback();
        //                    resultMsg.DefaultBehaviour("Error While InsertTblTaxRates");
        //                    return resultMsg;
        //                }
        //            }

        //            for (int pi = 0; pi < prodItemList.Count; pi++)
        //            {
        //                prodItemList[pi].IsActive = 1;
        //                prodItemList[pi].EffectiveFromDt = gstCodeDtlsTO.EffectiveFromDt;
        //                prodItemList[pi].CreatedOn = gstCodeDtlsTO.CreatedOn;
        //                prodItemList[pi].CreatedBy = gstCodeDtlsTO.CreatedBy;
        //                prodItemList[pi].EffectiveTodt = DateTime.MinValue;
        //                prodItemList[pi].GstCodeId = gstCodeDtlsTO.IdGstCode;
        //                result = _iTblProdGstCodeDtlsDAO.InsertTblProdGstCodeDtls(prodItemList[pi], conn, tran);
        //                if (result != 1)
        //                {
        //                    tran.Rollback();
        //                    resultMsg.DefaultBehaviour("Error While InsertTblProdGstCodeDtls");
        //                    return resultMsg;
        //                }
        //            }
        //        }
        //        else
        //        {

        //            result = UpdateTblGstCodeDtls(gstCodeDtlsTO, conn, tran);
        //            if (result != 1)
        //            {
        //                tran.Rollback();
        //                resultMsg.DefaultBehaviour("Error While UpdateTblGstCodeDtls");
        //                return resultMsg;
        //            }

        //            if (gstCodeDtlsTO.TaxRatesTOList == null)
        //            {
        //                tran.Rollback();
        //                resultMsg.DefaultBehaviour("TaxRatesTOList Found NULL");
        //                return resultMsg;
        //            }

        //            for (int i = 0; i < gstCodeDtlsTO.TaxRatesTOList.Count; i++)
        //            {

        //                var existingTaxRateTO = list.Where(a => a.TaxTypeId == gstCodeDtlsTO.TaxRatesTOList[i].TaxTypeId && a.IsActive == 1).FirstOrDefault();
        //                if (existingTaxRateTO != null)
        //                {
        //                    if (existingTaxRateTO.TaxPct != gstCodeDtlsTO.TaxRatesTOList[i].TaxPct)
        //                    {
        //                        existingTaxRateTO.IsActive = 0;
        //                        existingTaxRateTO.UpdatedBy = gstCodeDtlsTO.UpdatedBy;
        //                        existingTaxRateTO.UpdatedOn = gstCodeDtlsTO.UpdatedOn;
        //                        existingTaxRateTO.EffectiveToDt = gstCodeDtlsTO.UpdatedOn;

        //                        result = _iTblTaxRatesDAO.UpdateTblTaxRates(existingTaxRateTO, conn, tran);
        //                        if (result != 1)
        //                        {
        //                            tran.Rollback();
        //                            resultMsg.DefaultBehaviour("Error While UpdateTblTaxRates");
        //                            return resultMsg;
        //                        }

        //                        //Insert New One
        //                        gstCodeDtlsTO.TaxRatesTOList[i].EffectiveFromDt = gstCodeDtlsTO.UpdatedOn;
        //                        gstCodeDtlsTO.TaxRatesTOList[i].CreatedBy = gstCodeDtlsTO.UpdatedBy;
        //                        gstCodeDtlsTO.TaxRatesTOList[i].CreatedOn = gstCodeDtlsTO.UpdatedOn;
        //                        result = _iTblTaxRatesDAO.InsertTblTaxRates(gstCodeDtlsTO.TaxRatesTOList[i], conn, tran);
        //                        if (result != 1)
        //                        {
        //                            tran.Rollback();
        //                            resultMsg.DefaultBehaviour("Error While InsertTblTaxRates");
        //                            return resultMsg;
        //                        }
        //                    }
        //                    else
        //                    {
        //                        gstCodeDtlsTO.TaxRatesTOList[i].UpdatedBy = gstCodeDtlsTO.UpdatedBy;
        //                        gstCodeDtlsTO.TaxRatesTOList[i].UpdatedOn = gstCodeDtlsTO.UpdatedOn;
        //                        result = _iTblTaxRatesDAO.UpdateTblTaxRates(gstCodeDtlsTO.TaxRatesTOList[i], conn, tran);
        //                        if (result != 1)
        //                        {
        //                            tran.Rollback();
        //                            resultMsg.DefaultBehaviour("Error While UpdateTblTaxRates");
        //                            return resultMsg;
        //                        }
        //                    }
        //                }
        //                else
        //                {
        //                    gstCodeDtlsTO.TaxRatesTOList[i].CreatedBy = gstCodeDtlsTO.UpdatedBy;
        //                    gstCodeDtlsTO.TaxRatesTOList[i].CreatedOn = gstCodeDtlsTO.UpdatedOn;
        //                    result = _iTblTaxRatesDAO.InsertTblTaxRates(gstCodeDtlsTO.TaxRatesTOList[i], conn, tran);
        //                    if (result != 1)
        //                    {
        //                        tran.Rollback();
        //                        resultMsg.DefaultBehaviour("Error While InsertTblTaxRates");
        //                        return resultMsg;
        //                    }
        //                }
        //            }
        //        }

        //      //  tran.Commit();
        //        resultMsg.DefaultSuccessBehaviour();
        //        return resultMsg;

        //    }
        //    catch (Exception ex)
        //    {
        //        tran.Rollback();
        //        resultMsg.DefaultExceptionBehaviour(ex, "UpdateNewGSTCodeDetails");
        //        return resultMsg;
        //    }
        //    finally
        //    {
        //     //   conn.Close();
        //    }
        //}

        #endregion

        #region Deletion
        public int DeleteTblGstCodeDtls(Int32 idGstCode)
        {
            return _iTblGstCodeDtlsDAO.DeleteTblGstCodeDtls(idGstCode);
        }

        public int DeleteTblGstCodeDtls(Int32 idGstCode, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblGstCodeDtlsDAO.DeleteTblGstCodeDtls(idGstCode, conn, tran);
        }



        #endregion

    }
}
