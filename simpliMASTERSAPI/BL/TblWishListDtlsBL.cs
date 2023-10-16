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
using simpliMASTERSAPI.BL.Interfaces;
using simpliMASTERSAPI.Models;
using simpliMASTERSAPI.DAL.Interfaces;
using ODLMWebAPI.BL;
using System.IO;

namespace simpliMASTERSAPI.BL
{
    public class TblWishListDtlsBL : ITblWishListDtlsBL
    {
        private readonly ITblWishListDtlsDAO _iTblWishListDtlsDAO;
        private readonly IConnectionString _iConnectionString;
        private readonly ITblUserDAO _iTblUserDAO;
        private readonly IRunReport _iRunReport;

        public TblWishListDtlsBL(ITblWishListDtlsDAO iTblWishListDtlsDAO, IConnectionString iConnectionString, ITblUserDAO iTblUserDAO, IRunReport iRunReport)
        {
            _iTblWishListDtlsDAO = iTblWishListDtlsDAO;
            _iConnectionString = iConnectionString;
            _iTblUserDAO = iTblUserDAO;
            _iRunReport = iRunReport;
        }

        #region  Wishlist Test 
        public int InsertTblWishlistDtls(TblWishListDtlsTO wishlistDtlsTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblWishListDtlsDAO.InsertTblWishlistDtls(wishlistDtlsTO);
        }
        public ResultMessage SaveWishlistDetails(TblWishListDtlsTO wishlistDtlsTO)
        {
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            ResultMessage resultMsg = new ResultMessage();
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                int result = 0;
                if (wishlistDtlsTO.UserId > 0)
                {
                    result = InsertTblWishlistDtls(wishlistDtlsTO, conn, tran);
                }
                if (result != 1)
                {
                    tran.Rollback();
                    if (result == 0)
                    {
                        resultMsg.DefaultBehaviour("Please Enter User Login Id");
                    }
                    else
                    {
                        resultMsg.DefaultBehaviour("Error While InsertTblWishlistDtls");
                    }
                    resultMsg.Result = 0;
                    return resultMsg;
                }

                tran.Commit();
                resultMsg.DefaultSuccessBehaviour();
                resultMsg.Result = 1;
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

        public List<TblWishListDtlsTO> SelectTblWishListDtlsTO(Int32 userId = 0)
        {
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();

                return _iTblWishListDtlsDAO.SelectTblWishListDtls(userId, conn, tran);
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

        public TblWishListDtlsTO SelectTblWishListDtlsTOById(Int32 wishlistId)
        {
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                return _iTblWishListDtlsDAO.SelectTblWishListDtlsById(wishlistId, conn, tran);
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

        public ResultMessage UpdateWishListDtls(TblWishListDtlsTO tblWishListDtls)
        {
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            ResultMessage resultMsg = new ResultMessage();
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                int result = _iTblWishListDtlsDAO.UpdatetblWishListDtl(tblWishListDtls);
                if (result != 1)
                {
                    tran.Rollback();
                    resultMsg.DefaultBehaviour("Error While UpdatetblWishListDtl");
                    resultMsg.data = 0;
                    resultMsg.Result = 0;
                    return resultMsg;
                }

                tran.Commit();
                //resultMsg.DefaultSuccessBehaviour();
                resultMsg.Result = 1;
                resultMsg.Text = "Success - Record Updated Successfully";
                resultMsg.DisplayMessage = "Success - Record Updated Successfully";
                resultMsg.data = 1;
                return resultMsg;

            }
            catch (Exception ex)
            {
                tran.Rollback();
                resultMsg.DefaultExceptionBehaviour(ex, "UpdateWishListDtls");
                resultMsg.data = 0;
                return resultMsg;
            }
            finally
            {
                conn.Close();
            }


        }

        public ResultMessage DeleteTblWishListDtls(TblWishListDtlsTO tblWishListDtls)
        {
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            ResultMessage resultMsg = new ResultMessage();
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                int result = _iTblWishListDtlsDAO.DeleteTblWishListDtl(tblWishListDtls);
                if (result != 1)
                {
                    tran.Rollback();
                    resultMsg.DefaultBehaviour("Error While DeleteTblWishListDtl");
                    resultMsg.data = 0;
                    resultMsg.Result = 0;
                    return resultMsg;
                }

                tran.Commit();
                //resultMsg.DefaultSuccessBehaviour();
                resultMsg.Result = 1;
                resultMsg.Text = "Success - Record Deleted Successfully";
                resultMsg.DisplayMessage = "Success - Record Deleted Successfully";
                resultMsg.data = 1;
                return resultMsg;

            }
            catch (Exception ex)
            {
                tran.Rollback();
                resultMsg.DefaultExceptionBehaviour(ex, "DeleteTblWishListDtls");
                resultMsg.data = 0;
                return resultMsg;
            }
            finally
            {
                conn.Close();
            }

        }

        public List<TblUserTO> SelectTblUserDtlsTO(Int32 PageNumber, Int32 RowsPerPage, String strsearchtxt, Int32 UserWishlistId = 0)
        {
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                return _iTblWishListDtlsDAO.SelectTblUserDtls(PageNumber, RowsPerPage, strsearchtxt, UserWishlistId, conn, tran);
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

        #region Export to excel data

        public ResultMessage GetAllUserWishlistDetailsToExport(Int32 userId,string userWishlistIds)
        {
            ResultMessage resultMessage = new ResultMessage();
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            try
            {
                DataSet dataSet = new DataSet();
                List<TblUserTO> Userlist =  _iTblWishListDtlsDAO.SelectAllUserWishlistDetailsToExport (userId, userWishlistIds);
                List<TblWishListDtlsTO> list = _iTblWishListDtlsDAO.SelectAllUserWishlistDetailsToExport(userWishlistIds);
               
                if (Userlist != null && Userlist.Count > 0)
                {
                    DataTable userToExport = new DataTable();
                    userToExport = Common.ToDataTable(Userlist);
                    userToExport.TableName = "userTODT";
                    dataSet.Tables.Add(userToExport);

                    if (list != null && list.Count > 0)
                    {
                        DataTable wishListToExport = new DataTable();

                        wishListToExport = Common.ToDataTable(list);
                        wishListToExport.TableName = "wishlistTODT";
                        dataSet.Tables.Add(wishListToExport);

                    }

                    //DimOrgTypeTO dimOrgType = SelectDimOrgTypeTO(orgTypeId);
                    //String ReportTemplateName = dimOrgType.ExportRptTemplateName;
                    String ReportTemplateName = "UserWishlistReport";
                    String templateFilePath = SelectReportFullName(ReportTemplateName);
                    //String templateFilePath = "C:\\Templates\\BRMUAT\\UserWishlistReport.template.xls";
                    String fileName = "Doc-" + DateTime.Now.Ticks;
                    String saveLocation = AppDomain.CurrentDomain.BaseDirectory + fileName + ".xls";

                    Boolean IsProduction = true;
                    resultMessage = _iRunReport.GenrateMktgInvoiceReport(dataSet, templateFilePath, saveLocation, Constants.ReportE.EXCEL_DONT_OPEN, IsProduction);
                    if (resultMessage.MessageType == ResultMessageE.Information)
                    {
                        String filePath = String.Empty;
                        if (resultMessage.Tag != null && resultMessage.Tag.GetType() == typeof(String))
                        {

                            filePath = resultMessage.Tag.ToString();
                        }

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
                                    {
                                        File.Delete(file);
                                    }
                                }
                            }

                            if (resultMessage.MessageType == ResultMessageE.Information)
                            {
                                resultMessage.Text = "data exported successfully";
                                resultMessage.DisplayMessage = "data exported successfully";
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

        public DimOrgTypeTO SelectDimOrgTypeTO(Int32 userId)
        {
            SqlConnection connection = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction transaction = null;
            try
            {
                connection.Open();
                transaction = connection.BeginTransaction();
                return _iTblWishListDtlsDAO.SelectDimOrgType(userId, connection, transaction);
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                connection.Close();
            }
        }

        public String SelectReportFullName(String reportName)
        {
            String reportFullName = null;

            //MstReportTemplateTO mstReportTemplateTO = MstReportTemplateDAO.SelectMstReportTemplateTO(reportName);
            DimReportTemplateTO dimReportTemplateTO = SelectDimReportTemplateTO(reportName);
            if (dimReportTemplateTO != null)
            {

                TblConfigParamsTO templatePath = SelectTblConfigParamsValByName("REPORT_TEMPLATE_FOLDER_PATH");
                //object templatePath = BL.MstCsParamBL.GetValue("TEMP_REPORT_PATH");//For Testing Pramod InputRemovalExciseReport

                if (templatePath != null)
                    return templatePath.ConfigParamVal + dimReportTemplateTO.ReportFileName + "." + dimReportTemplateTO.ReportFileExtension;
            }
            return reportFullName;
        }

        public DimReportTemplateTO SelectDimReportTemplateTO(String reportName)
        {
            return _iTblWishListDtlsDAO.SelectDimReportTemplate(reportName);
        }

        public TblConfigParamsTO SelectTblConfigParamsValByName(string configParamName)
        {
            return _iTblWishListDtlsDAO.SelectTblConfigParamsValByName(configParamName);
        }
        
        #endregion
        

        #endregion
    }
}
