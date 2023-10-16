using ODLMWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using ODLMWebAPI.DAL.Interfaces;
using ODLMWebAPI.BL.Interfaces;
using ODLMWebAPI.StaticStuff;
namespace ODLMWebAPI.DAL
{
    public class TblReportsDAO : ITblReportsDAO
    {
        private readonly IConnectionString _iConnectionString;
        public TblReportsDAO(IConnectionString iConnectionString)
        {
            _iConnectionString = iConnectionString;
        }
        #region Methods
        public String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT * FROM [tblReports]";
            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public List<TblReportsTO> SelectAllModuleWiseReport(Int32 moduleId, Int32 transId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE moduleId = " + moduleId;

                if(transId > 0)
                {
                    cmdSelect.CommandText += " AND transactionTypeId = " + transId;
                }

                cmdSelect.CommandText += " AND isActive = 1 " + " ORDER BY reportName ASC ";

                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblReportsTO> list = ConvertDTToList(sqlReader);
                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }


        public List<TblReportsTO> SelectAllTblReports()
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery();
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblReportsTO> list = ConvertDTToList(sqlReader);
                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public TblReportsTO SelectTblReports(Int32 idReports)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE idReports = " + idReports + " ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblReportsTO> list = ConvertDTToList(sqlReader);
                return list[0];
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public List<TblReportsTO> SelectAllTblReports(SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                cmdSelect.CommandText = SqlSelectQuery();
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblReportsTO> list = ConvertDTToList(sqlReader);
                return list;
            }
            catch (Exception ex)
            {

                return null;
            }
            finally
            {
                cmdSelect.Dispose();
            }
        }

        #endregion

        #region Insertion
        public int InsertTblReports(TblReportsTO tblReportsTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblReportsTO, cmdInsert);
            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                conn.Close();
                cmdInsert.Dispose();
            }
        }

        public int InsertTblReports(TblReportsTO tblReportsTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblReportsTO, cmdInsert);
            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                cmdInsert.Dispose();
            }
        }

        public int ExecuteInsertionCommand(TblReportsTO tblReportsTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblReports]( " +
            //"  [idReports]" +
            " [moduleId]" +
            " ,[isActive]" +
            " ,[createdBy]" +
            " ,[updatedBy]" +
            " ,[createdOn]" +
            " ,[updatedOn]" +
            " ,[reportName]" +
            " ,[apiName]" +
            " ,[sqlQuery]" +
            " ,[transactionTypeId]" +
            " ,[isCustomizedRpt]" +
            " ,[customizeApiCall]" +
            " ,[totalMetaData]" +
            " )" +
" VALUES (" +
            //"  @IdReports " +
            " @ModuleId " +
            " ,@IsActive " +
            " ,@CreatedBy " +
            " ,@UpdatedBy " +
            " ,@CreatedOn " +
            " ,@UpdatedOn " +
            " ,@ReportName " +
            " ,@ApiName " +
            " ,@SqlQuery " +
            " ,@transactionTypeId" +
            " ,@isCustomizedRpt" +
            " ,@customizeApiCall" +
            " ,@totalMetaData" +
            " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            // cmdInsert.Parameters.Add("@IdReports", System.Data.SqlDbType.Int).Value = tblReportsTO.IdReports;
            cmdInsert.Parameters.Add("@ModuleId", System.Data.SqlDbType.Int).Value = tblReportsTO.ModuleId;
            cmdInsert.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = tblReportsTO.IsActive;
            cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblReportsTO.CreatedBy;
            cmdInsert.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = tblReportsTO.UpdatedBy;
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblReportsTO.CreatedOn;
            cmdInsert.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = tblReportsTO.UpdatedOn;
            cmdInsert.Parameters.Add("@ReportName", System.Data.SqlDbType.NVarChar).Value = tblReportsTO.ReportName;
            cmdInsert.Parameters.Add("@ApiName", System.Data.SqlDbType.NVarChar).Value = tblReportsTO.ApiName;
            cmdInsert.Parameters.Add("@SqlQuery", System.Data.SqlDbType.NVarChar).Value = tblReportsTO.SqlQuery;
            cmdInsert.Parameters.Add("@transactionTypeId", System.Data.SqlDbType.Int).Value = tblReportsTO.TransactionTypeId;
            cmdInsert.Parameters.Add("@isCustomizedRpt", System.Data.SqlDbType.Int).Value = tblReportsTO.IsCustomizedRpt;
            cmdInsert.Parameters.Add("@customizeApiCall", System.Data.SqlDbType.NVarChar).Value = tblReportsTO.CustomizeApiCall;
            cmdInsert.Parameters.Add("@totalMetaData", System.Data.SqlDbType.NVarChar).Value = tblReportsTO.TotalMetaData;
            return cmdInsert.ExecuteNonQuery();
        }
        #endregion

        #region Updation
        public int UpdateTblReports(TblReportsTO tblReportsTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblReportsTO, cmdUpdate);
            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                conn.Close();
                cmdUpdate.Dispose();
            }
        }

        public int UpdateTblReports(TblReportsTO tblReportsTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblReportsTO, cmdUpdate);
            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                cmdUpdate.Dispose();
            }
        }

        public int ExecuteUpdationCommand(TblReportsTO tblReportsTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblReports] SET " +
            "  [idReports] = @IdReports" +
            " ,[moduleId]= @ModuleId" +
            " ,[isActive]= @IsActive" +
            " ,[createdBy]= @CreatedBy" +
            " ,[updatedBy]= @UpdatedBy" +
            " ,[createdOn]= @CreatedOn" +
            " ,[updatedOn]= @UpdatedOn" +
            " ,[reportName]= @ReportName" +
            " ,[apiName]= @ApiName" +
            " ,[sqlQuery] = @SqlQuery" +
            " ,[transactionTypeId] = @transactionTypeId" +
            " ,[isCustomizedRpt] = @isCustomizedRpt" +
            " ,[customizeApiCall] = @customizeApiCall" +
            " ,[totalMetaData] = @totalMetaData" +
            " WHERE 1 = 2 ";

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdReports", System.Data.SqlDbType.Int).Value = tblReportsTO.IdReports;
            cmdUpdate.Parameters.Add("@ModuleId", System.Data.SqlDbType.Int).Value = tblReportsTO.ModuleId;
            cmdUpdate.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = tblReportsTO.IsActive;
            cmdUpdate.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblReportsTO.CreatedBy;
            cmdUpdate.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = tblReportsTO.UpdatedBy;
            cmdUpdate.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblReportsTO.CreatedOn;
            cmdUpdate.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = tblReportsTO.UpdatedOn;
            cmdUpdate.Parameters.Add("@ReportName", System.Data.SqlDbType.NVarChar).Value = tblReportsTO.ReportName;
            cmdUpdate.Parameters.Add("@ApiName", System.Data.SqlDbType.NVarChar).Value = tblReportsTO.ApiName;
            cmdUpdate.Parameters.Add("@SqlQuery", System.Data.SqlDbType.NVarChar).Value = tblReportsTO.SqlQuery;
            cmdUpdate.Parameters.Add("@transactionTypeId", System.Data.SqlDbType.Int).Value = tblReportsTO.TransactionTypeId;
            cmdUpdate.Parameters.Add("@isCustomizedRpt", System.Data.SqlDbType.Int).Value = tblReportsTO.IsCustomizedRpt;
            cmdUpdate.Parameters.Add("@customizeApiCall", System.Data.SqlDbType.NVarChar).Value = tblReportsTO.CustomizeApiCall;
            cmdUpdate.Parameters.Add("@totalMetaData", System.Data.SqlDbType.NVarChar).Value = tblReportsTO.TotalMetaData;
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion

        #region Deletion
        public int DeleteTblReports(Int32 idReports)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idReports, cmdDelete);
            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                conn.Close();
                cmdDelete.Dispose();
            }
        }

        public int DeleteTblReports(Int32 idReports, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idReports, cmdDelete);
            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                cmdDelete.Dispose();
            }
        }

        public int ExecuteDeletionCommand(Int32 idReports, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tblReports] " +
            " WHERE idReports = " + idReports + "";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idReports", System.Data.SqlDbType.Int).Value = tblReportsTO.IdReports;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion

        public List<TblReportsTO> ConvertDTToList(SqlDataReader tblReportsTODT)
        {
            List<TblReportsTO> tblReportsTOList = new List<TblReportsTO>();
            if (tblReportsTODT != null)
            {
                while (tblReportsTODT.Read())
                {
                    TblReportsTO tblReportsTONew = new TblReportsTO();
                    if (tblReportsTODT["idReports"] != DBNull.Value)
                        tblReportsTONew.IdReports = Convert.ToInt32(tblReportsTODT["idReports"].ToString());
                    if (tblReportsTODT["moduleId"] != DBNull.Value)
                        tblReportsTONew.ModuleId = Convert.ToInt32(tblReportsTODT["moduleId"].ToString());
                    if (tblReportsTODT["isActive"] != DBNull.Value)
                        tblReportsTONew.IsActive = Convert.ToInt32(tblReportsTODT["isActive"].ToString());
                    if (tblReportsTODT["createdBy"] != DBNull.Value)
                        tblReportsTONew.CreatedBy = Convert.ToInt32(tblReportsTODT["createdBy"].ToString());
                    if (tblReportsTODT["updatedBy"] != DBNull.Value)
                        tblReportsTONew.UpdatedBy = Convert.ToInt32(tblReportsTODT["updatedBy"].ToString());
                    if (tblReportsTODT["createdOn"] != DBNull.Value)
                        tblReportsTONew.CreatedOn = Convert.ToDateTime(tblReportsTODT["createdOn"].ToString());
                    if (tblReportsTODT["updatedOn"] != DBNull.Value)
                        tblReportsTONew.UpdatedOn = Convert.ToDateTime(tblReportsTODT["updatedOn"].ToString());
                    if (tblReportsTODT["reportName"] != DBNull.Value)
                        tblReportsTONew.ReportName = Convert.ToString(tblReportsTODT["reportName"].ToString());
                    if (tblReportsTODT["apiName"] != DBNull.Value)
                        tblReportsTONew.ApiName = Convert.ToString(tblReportsTODT["apiName"].ToString());
                    if (tblReportsTODT["sqlQuery"] != DBNull.Value)
                        tblReportsTONew.SqlQuery = Convert.ToString(tblReportsTODT["sqlQuery"].ToString());
                    if (tblReportsTODT["whereClause"] != DBNull.Value)
                        tblReportsTONew.WhereClause = Convert.ToString(tblReportsTODT["whereClause"].ToString());
                    if (tblReportsTODT["transactionTypeId"] != DBNull.Value)
                        tblReportsTONew.TransactionTypeId = Convert.ToInt32(tblReportsTODT["transactionTypeId"].ToString());
                    if (tblReportsTODT["isCustomizedRpt"] != DBNull.Value)
                        tblReportsTONew.IsCustomizedRpt = Convert.ToInt32(tblReportsTODT["isCustomizedRpt"].ToString());
                    if (tblReportsTODT["customizeApiCall"] != DBNull.Value)
                        tblReportsTONew.CustomizeApiCall = Convert.ToString(tblReportsTODT["customizeApiCall"].ToString());
                    if (tblReportsTODT["totalMetaData"] != DBNull.Value)
                        tblReportsTONew.TotalMetaData = Convert.ToString(tblReportsTODT["totalMetaData"].ToString());
                    if (tblReportsTODT["orderByStr"] != DBNull.Value)
                        tblReportsTONew.OrderByStr = Convert.ToString(tblReportsTODT["orderByStr"].ToString());

                    if (tblReportsTODT["roleTypeId"] != DBNull.Value)
                        tblReportsTONew.RoleTypeId = Convert.ToString(tblReportsTODT["roleTypeId"].ToString());

                    if (tblReportsTODT["roleTypeCond"] != DBNull.Value)
                        tblReportsTONew.RoleTypeCond = Convert.ToString(tblReportsTODT["roleTypeCond"].ToString());
                    if (tblReportsTODT["ignoreColumns"] != DBNull.Value)
                        tblReportsTONew.IgnoreColumns = Convert.ToString(tblReportsTODT["ignoreColumns"].ToString());

                    if (tblReportsTODT["sysEleId"] != DBNull.Value)
                        tblReportsTONew.SysEleId = Convert.ToInt32(tblReportsTODT["sysEleId"].ToString());

                    tblReportsTOList.Add(tblReportsTONew);
                }
            }
            return tblReportsTOList;
        }



        public List<DynamicReportTO> GetDynamicSqlData(string connectionstring, string sql)
        {
            SqlConnection conn = new SqlConnection(connectionstring);
            SqlCommand comm = new SqlCommand(sql, conn);
            try
            {
                conn.Open();
                SqlDataReader sqlReader = comm.ExecuteReader();
                return SqlDataReaderToList(sqlReader);
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                comm.Dispose();
                conn.Close();

            }
        }

        public List<DynamicReportTO> SqlDataReaderToList(SqlDataReader reader)
        {
            try
            {
                List<DynamicReportTO> list = new List<DynamicReportTO>();

                while (reader.Read())
                {
                    DynamicReportTO dynamicReportTO = new DynamicReportTO();
                    List<DropDownTO> dropDownList = new List<DropDownTO>();
                    for (var i = 0; i < reader.FieldCount; i++)
                    {
                        DropDownTO dropDownTO = new DropDownTO();
                        dropDownTO.Text = reader.GetName(i);
                        dropDownTO.Tag = reader[i];
                        dropDownList.Add(dropDownTO);
                    }
                    dynamicReportTO.DropDownList = dropDownList;
                    list.Add(dynamicReportTO);
                }
                return list;
            }
            catch (Exception ex)
            {

                throw;
            }

        }


        //Added by minal 02 April 2021

        public DataTable SelectTallyStockTransferReportList(DateTime frmDt, DateTime toDt,String roleTypeCond)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            string selectQuery = String.Empty;
            DataTable dt = new DataTable();
            try
            {
                conn.Open();
                selectQuery = " WITH cte_TallyStockEnquiry AS  " +
                    " ( " +
                    " SELECT loading.createdOn AS date,'Stock Journal' AS voucherType, productItem.itemName AS SourceConsumptionSTOCKITEM ,  " +
                    " LEFT(org.firmName,3) AS SourceConsumptionGODOWNCAndF,CAST(ROUND(ISNULL(itemDetails.invoiceQty,0),3) AS NUMERIC(36,3)) AS SourceConsumptionQTY, " +
                    " CAST(ROUND(ISNULL(itemDetails.rate,0),3) AS NUMERIC(36,3)) AS SourceConsumptionRate, " +
                    " CAST(ROUND(ISNULL(((itemDetails.invoiceQty)*(itemDetails.rate)),0),2) AS NUMERIC(36,2)) AS SourceConsumptionAMOUNT, " +
                    " productItem.itemName AS DestinationProductionSTOCKITEM,LEFT(orgDealer.firmName,3) AS DestinationProductionGODOWNDelerMappedtoCAndF, " +
                    " CAST(ROUND(ISNULL(itemDetails.invoiceQty,0),3) AS NUMERIC(36,3)) AS DestinationProductionQTY, " +
                    " CAST(ROUND(ISNULL(itemDetails.rate,0),3) AS NUMERIC(36,3)) AS DestinationProductionRate," +
                    " CAST(ROUND(ISNULL(((itemDetails.invoiceQty)*(itemDetails.rate)),0),2) AS NUMERIC(36,2)) AS DestinationProductionAMOUNT, " +
                    " UPPER(loadingSlip.vehicleNo) + ' + ' + CAST(itemDetails.invoiceQty AS NVARCHAR(10)) AS NARRATION  " +
                    " FROM tempLoading loading " +
                    " LEFT JOIN tempLoadingSlip loadingSlip ON loadingSlip.loadingId = loading.idLoading   " +
                    " LEFT JOIN tempLoadingSlipExt lExt ON lExt.loadingSlipId = loadingSlip.idLoadingSlip " +
                    " LEFT JOIN tempInvoiceItemDetails itemDetails ON itemDetails.loadingSlipExtId = lExt.idLoadingSlipExt  " +
                    " LEFT JOIN tblProdGstCodeDtls prodGstCodeDtls ON prodGstCodeDtls.idProdGstCode = itemDetails.prodGstCodeId " +
                    " LEFT JOIN tblProductItem productItem ON productItem.idProdItem = prodGstCodeDtls.prodItemId " +
                    " LEFT JOIN tblOrganization org ON org.idOrganization = loading.cnfOrgId  " +
                    " LEFT JOIN tblOrganization orgDealer ON orgDealer.idOrganization = loadingSlip.dealerOrgId  " +
                    " WHERE CAST(loading.createdOn AS DATE) BETWEEN  CAST(@fromDate AS DATE) AND CAST(@toDate AS DATE) " +
                    " AND   org.isInternalCnf = 1 AND loadingSlip.statusId = 17 ";

                if (!String.IsNullOrEmpty(roleTypeCond))
                {
                    selectQuery += roleTypeCond;
                }

     selectQuery += " ) " +
                    ""+
                    " SELECT FORMAT(date,'dd/MM/yyyy') AS DATE,voucherType AS 'VOUCHER TYPE',SourceConsumptionSTOCKITEM AS'STOCK ITEM'," +
                    " SourceConsumptionGODOWNCAndF AS 'GODOWN [C & F]',CAST(SourceConsumptionQTY AS NVARCHAR) AS 'QTY'," +
                    " CAST(SourceConsumptionRate AS NVARCHAR) AS 'RATE',CAST(SourceConsumptionAMOUNT AS NVARCHAR) AS 'AMOUNT'," +
                    " DestinationProductionSTOCKITEM AS 'STOCK ITEM A',DestinationProductionGODOWNDelerMappedtoCAndF AS 'GODOWN A'," +
                    " CAST(DestinationProductionQTY AS NVARCHAR) AS 'QTY A',CAST(DestinationProductionRate AS NVARCHAR) AS 'RATE A', " +
                    " CAST(DestinationProductionAMOUNT AS NVARCHAR) AS 'AMOUNT A', NARRATION AS 'NARRATION',0 AS isTotalRow " +
                    " FROM cte_TallyStockEnquiry " +
                    " ORDER BY CONVERT(DateTime,date,101)  ASC ";

                cmdSelect.CommandText = selectQuery;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                cmdSelect.Parameters.Add("@fromDate", System.Data.SqlDbType.Date).Value = frmDt;
                cmdSelect.Parameters.Add("@toDate", System.Data.SqlDbType.Date).Value = toDt;
                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                dt.Load(reader);

                return dt;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (reader != null) reader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public List<TblWBRptTO> SelectWBForPurchaseReportList(DateTime frmDt, DateTime toDt)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            string selectQuery = String.Empty;
            //DateTime sysDate = Constants.ServerDateTime;
            try
            {
                conn.Open();
                selectQuery = " DECLARE @Temp TABLE(idR INT IDENTITY NOT NULL,wBID NVARCHAR(100),userID NVARCHAR(100),orignalRSTNo NVARCHAR(200),additionalRSTNo NVARCHAR(200),date NVARCHAR(20),time NVARCHAR(20),materialType NVARCHAR(500)," +
                    " materialSubType NVARCHAR(1000),grossWeight DECIMAL(18,2),firstWeight DECIMAL(18,2),secondWeight DECIMAL(18,2),thirdWeight DECIMAL(18,2),forthWeight DECIMAL(18,2),fifthWeight DECIMAL(18,2),sixthWeight DECIMAL(18,2)," +
                    " seventhWeight DECIMAL(18,2),tareWeight DECIMAL(18,2),netWeight DECIMAL(18,2),loadOrUnload NVARCHAR(50),fromLocation NVARCHAR(100),toLocation NVARCHAR(100),transactionType NVARCHAR(100),vehicleNumber NVARCHAR(100),vehicleStatus NVARCHAR(100),billType NVARCHAR(100),vehicleID NVARCHAR(100)," +
                    " statusId INT,isActive INT,rootScheduleId INT,idPurchaseScheduleSummary INT)" +
                    " DECLARE @Temp1 TABLE (  idR1 INT IDENTITY NOT NULL,rootScheduleId INT)" +
                    " INSERT INTO @Temp " +
                    " SELECT purchaseWeighingStageSummary.machineName AS wBID,purchaseWeighingStageSummary.userDisplayName AS userID,'-' AS orignalRSTNo, " +
                    " '-' AS additionalRSTNo,FORMAT(tareWt.createdOn,'dd/MM/yyyy') AS date,CONVERT(CHAR(5),tareWt.createdOn, 108) AS time," +
                    " prodClassification.prodClassDesc AS materialType,materialSubType = (STUFF((SELECT DISTINCT ',' + productItem.itemName FROM tblProductItem productItem " +
                    " LEFT JOIN tblPurchaseScheduleDetails purchaseScheduleDetails ON purchaseScheduleDetails.prodItemId = productItem.idProdItem " +
                    " WHERE  purchaseScheduleDetails.purchaseScheduleSummaryId = purchaseScheduleSummary.idPurchaseScheduleSummary" +
                    " FOR XML PATH(''), TYPE ).value('.', 'NVARCHAR(MAX)') ,1,1,'')),grossWt.grossWeightMT AS grossWeight," +
                    " wtStage1.actualWeightMT AS firstWeight,wtStage2.actualWeightMT AS secondWeight,wtStage3.actualWeightMT AS thirdWeight," +
                    " wtStage4.actualWeightMT AS forthWeight,wtStage5.actualWeightMT AS fifthWeight,wtStage6.actualWeightMT AS sixthWeight," +
                    " wtStage7.actualWeightMT AS seventhWeight,tareWt.actualWeightMT AS tareWeight," +
                    " CASE WHEN ((ISNULL(grossWt.grossWeightMT,0)) - (ISNULL(tareWt.actualWeightMT,0))) < 0 THEN 0" +
                    " ELSE ((ISNULL(grossWt.grossWeightMT,0)) - (ISNULL(tareWt.actualWeightMT,0))) END AS netWeight ," +
                    " 'Unload' AS loadOrUnload, " +
                    " CASE WHEN purchaseVehicleSpotEntry.location IS NOT NULL THEN purchaseVehicleSpotEntry.location ELSE purchaseScheduleSummary.location END AS fromLocation, " +
                    " 'Jalna' AS toLocation,'Purchase' AS transactionType, " +
                    " purchaseScheduleSummary.vehicleNo AS vehicleNumber,dimStatus.statusDesc AS vehicleStatus," +
                    " CASE WHEN purchaseScheduleSummary.cOrNCId = 1 THEN 'Order' WHEN purchaseScheduleSummary.cOrNCId = 0 THEN 'Enquiry' ELSE '' END AS billType," +
                    " purchaseScheduleSummary.rootScheduleId AS vehicleID,purchaseScheduleSummary.statusId,purchaseScheduleSummary.isActive,purchaseScheduleSummary.rootScheduleId,purchaseScheduleSummary.idPurchaseScheduleSummary  " +
                    " FROM tblPurchaseScheduleSummary purchaseScheduleSummary" +
                    " LEFT JOIN " +
                    "           (" +
                    "               SELECT tblPurchaseWeighingStageSummary.purchaseScheduleSummaryId,weighingMachine.machineName,tblUser.userDisplayName " +
                    "               FROM tblPurchaseWeighingStageSummary tblPurchaseWeighingStageSummary " +
                    "               LEFT JOIN tblWeighingMachine weighingMachine ON weighingMachine.idWeighingMachine = tblPurchaseWeighingStageSummary.weighingMachineId " +
                    "               LEFT JOIN tblUser tblUser ON tblUser.idUser = tblPurchaseWeighingStageSummary.createdBy " +
                    "               WHERE tblPurchaseWeighingStageSummary.weightMeasurTypeId = 1" +
                    "           ) AS purchaseWeighingStageSummary  " +
                    " ON ISNULL(purchaseScheduleSummary.rootScheduleId,purchaseScheduleSummary.idPurchaseScheduleSummary) = purchaseWeighingStageSummary.purchaseScheduleSummaryId " +
                    " LEFT JOIN tblPurchaseEnquiry purchaseEnquiry ON purchaseEnquiry.idPurchaseEnquiry = purchaseScheduleSummary.purchaseEnquiryId " +
                    " LEFT JOIN tblProdClassification prodClassification ON prodClassification.idProdClass = purchaseEnquiry.prodClassId " +
                    " LEFT JOIN tblPurchaseWeighingStageSummary grossWt ON grossWt.purchaseScheduleSummaryId = ISNULL(purchaseScheduleSummary.rootScheduleId, purchaseScheduleSummary.idPurchaseScheduleSummary) AND grossWt.weightStageId = 0 AND grossWt.weightMeasurTypeId = 3 " +
                    " LEFT JOIN tblPurchaseWeighingStageSummary wtStage1 ON wtStage1.purchaseScheduleSummaryId = ISNULL(purchaseScheduleSummary.rootScheduleId, purchaseScheduleSummary.idPurchaseScheduleSummary) AND wtStage1.weightStageId = 1 AND wtStage1.weightMeasurTypeId = 2 " +
                    " LEFT JOIN tblPurchaseWeighingStageSummary wtStage2 ON wtStage2.purchaseScheduleSummaryId = ISNULL(purchaseScheduleSummary.rootScheduleId, purchaseScheduleSummary.idPurchaseScheduleSummary) AND wtStage2.weightStageId = 2 AND wtStage2.weightMeasurTypeId = 2 " +
                    " LEFT JOIN tblPurchaseWeighingStageSummary wtStage3 ON wtStage3.purchaseScheduleSummaryId = ISNULL(purchaseScheduleSummary.rootScheduleId, purchaseScheduleSummary.idPurchaseScheduleSummary) AND wtStage3.weightStageId = 3 AND wtStage3.weightMeasurTypeId = 2 " +
                    " LEFT JOIN tblPurchaseWeighingStageSummary wtStage4 ON wtStage4.purchaseScheduleSummaryId = ISNULL(purchaseScheduleSummary.rootScheduleId, purchaseScheduleSummary.idPurchaseScheduleSummary) AND wtStage4.weightStageId = 4 AND wtStage4.weightMeasurTypeId = 2 " +
                    " LEFT JOIN tblPurchaseWeighingStageSummary wtStage5 ON wtStage5.purchaseScheduleSummaryId = ISNULL(purchaseScheduleSummary.rootScheduleId, purchaseScheduleSummary.idPurchaseScheduleSummary) AND wtStage5.weightStageId = 5 AND wtStage5.weightMeasurTypeId = 2 " +
                    " LEFT JOIN tblPurchaseWeighingStageSummary wtStage6 ON wtStage6.purchaseScheduleSummaryId = ISNULL(purchaseScheduleSummary.rootScheduleId, purchaseScheduleSummary.idPurchaseScheduleSummary) AND wtStage6.weightStageId = 6 AND wtStage6.weightMeasurTypeId = 2 " +
                    " LEFT JOIN tblPurchaseWeighingStageSummary wtStage7 ON wtStage7.purchaseScheduleSummaryId = ISNULL(purchaseScheduleSummary.rootScheduleId, purchaseScheduleSummary.idPurchaseScheduleSummary) AND wtStage7.weightStageId = 7 AND wtStage7.weightMeasurTypeId = 2 " +
                    " LEFT JOIN tblPurchaseWeighingStageSummary tareWt ON tareWt.purchaseScheduleSummaryId = ISNULL(purchaseScheduleSummary.rootScheduleId, purchaseScheduleSummary.idPurchaseScheduleSummary) AND tareWt.weightMeasurTypeId = 1 " +
                    " LEFT JOIN tblPurchaseVehicleSpotEntry purchaseVehicleSpotEntry ON purchaseVehicleSpotEntry.purchaseScheduleSummaryId = ISNULL(purchaseScheduleSummary.rootScheduleId, purchaseScheduleSummary.idPurchaseScheduleSummary)" +
                    " LEFT JOIN dimStatus dimStatus ON dimStatus.idStatus = purchaseScheduleSummary.statusId" +
                    " WHERE CAST(purchaseScheduleSummary.createdOn AS DATE) BETWEEN CAST(@fromDate AS DATE) AND CAST(@toDate AS DATE)" +
                    " AND purchaseScheduleSummary.isActive = 1 " +
                    "" +
                    " INSERT INTO @Temp1 (rootScheduleId)" +
                    " SELECT rootScheduleId FROM @Temp" +
                    "" +
                    " DECLARE @VarID INT" +
                    " SET     @VarID = (SELECT ISNULL(COUNT(ISNULL(IdR1,0)),0) FROM @Temp1)" +
                    " DECLARE @VarRid INT" +
                    " SELECT TOP(1) @VarRid = IdR1 FROM @Temp1" +
                    " WHILE @VarID !=0" +
                    " BEGIN" +
                    ""+
                    "  DECLARE @statusId INT" +
                    "  DECLARE @vehiclePhaseId INT" +
                    "  DECLARE @rootScheduleId  INT" +
                    "  SELECT  @rootScheduleId = rootScheduleId FROM @Temp1 WHERE   IdR1 = @VarRid" +
                    "" +
                    " SELECT @statusId = tblPurchaseScheduleSummary.statusId,@vehiclePhaseId = tblPurchaseScheduleSummary.vehiclePhaseId " +
                    " FROM  tblPurchaseScheduleSummary tblPurchaseScheduleSummary" +
                    " WHERE tblPurchaseScheduleSummary.idPurchaseScheduleSummary = (SELECT MAX(purchaseScheduleSummarytbl.idPurchaseScheduleSummary) FROM tblPurchaseScheduleSummary purchaseScheduleSummarytbl WHERE purchaseScheduleSummarytbl.rootScheduleId = @rootScheduleId)" +
                    ""+
                    "  IF @statusId = 509 AND @vehiclePhaseId = 2 " +
                    "  BEGIN" +
                    "  UPDATE @Temp SET materialSubType = (STUFF((SELECT DISTINCT ',' + productItem.itemName " +
                    "  FROM tblProductItem productItem " +
                    "  LEFT JOIN tblPurchaseScheduleDetails purchaseScheduleDetails ON purchaseScheduleDetails.prodItemId = productItem.idProdItem" +
                    "  WHERE  purchaseScheduleDetails.purchaseScheduleSummaryId IN (SELECT idPurchaseScheduleSummary FROM tblPurchaseScheduleSummary tblPurchaseScheduleSummary " +
                    "  WHERE tblPurchaseScheduleSummary.rootScheduleId = @rootScheduleId AND tblPurchaseScheduleSummary.statusId = 509 AND tblPurchaseScheduleSummary.vehiclePhaseId = 2) FOR XML PATH(''), TYPE ).value('.', 'NVARCHAR(MAX)') ,1,1,'')) " +
                    "  WHERE rootScheduleId = @rootScheduleId " +
                    ""+
                    " UPDATE @Temp SET vehicleStatus = 'Grading Completed' WHERE rootScheduleId = @rootScheduleId " +
                    " END" +
                    ""+
                    " IF @statusId = 510 AND @vehiclePhaseId = 2" +
                    " BEGIN" +
                    "  UPDATE @Temp SET vehicleStatus = 'Vehicle Out' WHERE rootScheduleId = @rootScheduleId" +
                    " END"+
                    ""+
                    "  IF @statusId = 509 AND @vehiclePhaseId = 3 " +
                    "  BEGIN" +
                    "  UPDATE @Temp SET materialSubType = (STUFF((SELECT DISTINCT ',' + productItem.itemName " +
                    "  FROM tblProductItem productItem " +
                    "  LEFT JOIN tblPurchaseScheduleDetails purchaseScheduleDetails ON purchaseScheduleDetails.prodItemId = productItem.idProdItem" +
                    "  WHERE  purchaseScheduleDetails.purchaseScheduleSummaryId IN (SELECT idPurchaseScheduleSummary FROM tblPurchaseScheduleSummary tblPurchaseScheduleSummary " +
                    "  WHERE tblPurchaseScheduleSummary.rootScheduleId = @rootScheduleId AND tblPurchaseScheduleSummary.statusId = 509 AND tblPurchaseScheduleSummary.vehiclePhaseId = 3) FOR XML PATH(''), TYPE ).value('.', 'NVARCHAR(MAX)') ,1,1,'')) " +
                    "  WHERE rootScheduleId = @rootScheduleId " +
                    "" +
                    " UPDATE @Temp SET vehicleStatus = 'Recovery Completed' WHERE rootScheduleId = @rootScheduleId " +
                    " END" +
                    "" +
                    " IF @statusId = 510 AND @vehiclePhaseId = 3" +
                    " BEGIN" +
                    "   UPDATE @Temp SET vehicleStatus = 'Vehicle Out' WHERE rootScheduleId = @rootScheduleId" +
                    " END" +
                    ""+
                    " IF @statusId = 509 AND @vehiclePhaseId = 4 " +
                    " BEGIN" +
                    " UPDATE @Temp SET materialSubType = (STUFF((SELECT DISTINCT ',' + productItem.itemName " +
                    " FROM tblProductItem productItem " +
                    " LEFT JOIN tblPurchaseScheduleDetails purchaseScheduleDetails ON purchaseScheduleDetails.prodItemId = productItem.idProdItem" +
                    "  WHERE  purchaseScheduleDetails.purchaseScheduleSummaryId IN (SELECT idPurchaseScheduleSummary FROM tblPurchaseScheduleSummary tblPurchaseScheduleSummary " +
                    " WHERE tblPurchaseScheduleSummary.rootScheduleId = @rootScheduleId AND tblPurchaseScheduleSummary.statusId = 509 AND tblPurchaseScheduleSummary.vehiclePhaseId = 4) FOR XML PATH(''), TYPE ).value('.', 'NVARCHAR(MAX)') ,1,1,'')) " +
                    " WHERE rootScheduleId = @rootScheduleId " +
                    " " +
                    " UPDATE @Temp SET vehicleStatus = 'Correction Completed' WHERE rootScheduleId = @rootScheduleId " +
                    " END" +
                    ""+
                    " IF @statusId = 510 AND @vehiclePhaseId = 4" +
                    " BEGIN" +
                    "  UPDATE @Temp SET vehicleStatus = 'Vehicle Out' WHERE rootScheduleId = @rootScheduleId" +
                    " END" +
                    ""+
                    " "+
                    " DELETE @Temp1 WHERE IdR1 = @VarRid    " +
                    " SET  @VarID = (SELECT ISNULL(COUNT(ISNULL(IdR1,0)),0) FROM @Temp1) " +
                    " SELECT TOP(1) @VarRid = IdR1 FROM @Temp1" +
                    " END" +
                    ""+
                    " SELECT wBID,userID,orignalRSTNo,additionalRSTNo,date,time,materialType,materialSubType,grossWeight,firstWeight,secondWeight,thirdWeight,forthWeight," +
                    " fifthWeight,sixthWeight,seventhWeight,tareWeight,netWeight," +
                    " loadOrUnload,fromLocation,toLocation,transactionType,UPPER(vehicleNumber) AS vehicleNumber,vehicleStatus,billType,vehicleID" +
                    " FROM @Temp";
                cmdSelect.CommandText = selectQuery;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                cmdSelect.Parameters.Add("@fromDate", System.Data.SqlDbType.Date).Value = frmDt;
                cmdSelect.Parameters.Add("@toDate", System.Data.SqlDbType.Date).Value = toDt;
                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblWBRptTO> list = ConvertDTToListForRPTWBReport(reader);

                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (reader != null) reader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public List<TblWBRptTO> SelectWBForSaleReportList(DateTime frmDt, DateTime toDt)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            string selectQuery = String.Empty;
            //DateTime sysDate = Constants.ServerDateTime;
            try
            {
                conn.Open();
                selectQuery = " DECLARE @Temp TABLE ( idR1 INT IDENTITY NOT NULL,loadingId INT)" +
                    " DECLARE @Temp1 TABLE( idR2 INT IDENTITY NOT NULL,rowNumber INT,idWeightMeasure INT,loadingId INT,intermediateWt DECIMAL(18,2))" +
                    " DECLARE @Temp2 TABLE(idR3 INT IDENTITY NOT NULL,loadingId INT,wBID NVARCHAR(100),userID NVARCHAR(100),orignalRSTNo NVARCHAR(200),additionalRSTNo NVARCHAR(200),date NVARCHAR(20),time NVARCHAR(20),materialType NVARCHAR(500)," +
                    " materialSubType NVARCHAR(1000),grossWeight DECIMAL(18,2),firstWeight DECIMAL(18,2),secondWeight DECIMAL(18,2),thirdWeight DECIMAL(18,2),forthWeight DECIMAL(18,2),fifthWeight DECIMAL(18,2),sixthWeight DECIMAL(18,2)," +
                    " seventhWeight DECIMAL(18,2),tareWeight DECIMAL(18,2),netWeight DECIMAL(18,2),loadOrUnload NVARCHAR(50),fromLocation NVARCHAR(100),toLocation NVARCHAR(100),transactionType NVARCHAR(100),vehicleNumber NVARCHAR(100),vehicleStatus NVARCHAR(100),billType NVARCHAR(100),vehicleID NVARCHAR(100))" +
                    " INSERT INTO @Temp (loadingId) SELECT loading.idLoading FROM tempLoading loading WHERE CAST(loading.createdOn AS DATE) BETWEEN CAST(@fromDate AS DATE) AND CAST(@toDate AS DATE) " +
                    " INSERT INTO @Temp2 " +
                    " (loadingId,wBID,userID,orignalRSTNo,additionalRSTNo,date,time,materialType,materialSubType,grossWeight,tareWeight,netWeight," +
                    " loadOrUnload,fromLocation,toLocation,transactionType,vehicleNumber,vehicleStatus,billType,vehicleID)" +
                    " SELECT loading.idLoading, weighingMeasures.machineName AS wBID, weighingMeasures.userDisplayName AS userID," +
                    " '-' AS orignalRSTNo,'-' AS additionalRSTNo,FORMAT(weighingMeasuresTareWt.createdOn,'dd/MM/yyyy') AS date,CONVERT(CHAR(5),weighingMeasuresTareWt.createdOn, 108) AS time," +
                    " CASE WHEN loading.loadingType = 1 THEN 'TMT' WHEN loading.loadingType = 2 THEN 'Other' ELSE '' END AS materialType,'-' AS materialSubType, " +
                    " weighingMeasuresGrossWt.weightMT AS grossWeight,weighingMeasuresTareWt.weightMT AS tareWeight, " +
                    " CASE WHEN ((ISNULL(weighingMeasuresGrossWt.weightMT,0)) - (ISNULL(weighingMeasuresTareWt.weightMT,0))) < 0 THEN 0" +
                    " ELSE ((ISNULL(weighingMeasuresGrossWt.weightMT,0)) - (ISNULL(weighingMeasuresTareWt.weightMT,0))) END AS netWeight," +
                    " 'Load' AS loadOrUnload,'-' AS fromLocation ,'Jalna' AS toLocation,'Sale' AS transactionType," +
                    " weighingMeasuresTareWt.vehicleNo AS vehicleNumber, dimStatus.statusName AS vehicleStatus,'-' AS billType,loading.loadingSlipNo AS vehicleId " +
                    " FROM tempLoading loading " +
                    " LEFT JOIN (" +
                    "              SELECT ROW_NUMBER() OVER(PARTITION BY tempWeighingMeasures.loadingId ORDER BY tblWeighingMachine.machineName ASC) AS row_number," +
                    "              tempWeighingMeasures.idWeightMeasure,tblWeighingMachine.machineName,tempWeighingMeasures.loadingId,tblUser.userDisplayName" +
                    "              FROM tempWeighingMeasures tempWeighingMeasures" +
                    "              LEFT JOIN tblWeighingMachine tblWeighingMachine ON tblWeighingMachine.idWeighingMachine = tempWeighingMeasures.weighingMachineId" +
                    "              LEFT JOIN tblUser tblUser ON tblUser.idUser = tempWeighingMeasures.createdBy " +
                    "              WHERE tempWeighingMeasures.weightMeasurTypeId = 1 " +
                    "           ) AS weighingMeasures" +
                    " ON weighingMeasures.loadingId = loading.idLoading AND weighingMeasures.row_number = 1" +
                    " LEFT JOIN (" +
                    " SELECT ROW_NUMBER() OVER(PARTITION BY tempWeighingMeasures.loadingId ORDER BY tempWeighingMeasures.idWeightMeasure ASC) AS row_number," +
                    " tempWeighingMeasures.idWeightMeasure,tempWeighingMeasures.loadingId,tempWeighingMeasures.weightMT" +
                    " FROM tempWeighingMeasures tempWeighingMeasures" +
                    " WHERE tempWeighingMeasures.weightMeasurTypeId = 3" +
                    "  ) AS weighingMeasuresGrossWt" +
                    " ON weighingMeasuresGrossWt.loadingId = loading.idLoading AND weighingMeasuresGrossWt.row_number = 1" +
                    " LEFT JOIN (" +
                    " SELECT ROW_NUMBER() OVER(PARTITION BY tempWeighingMeasures.loadingId ORDER BY tempWeighingMeasures.idWeightMeasure DESC) AS row_number," +
                    " tempWeighingMeasures.idWeightMeasure,tempWeighingMeasures.loadingId,tempWeighingMeasures.weightMT,tempWeighingMeasures.vehicleNo,tempWeighingMeasures.createdOn" +
                    " FROM tempWeighingMeasures tempWeighingMeasures" +
                    " WHERE tempWeighingMeasures.weightMeasurTypeId = 1" +
                    " ) AS weighingMeasuresTareWt" +
                    " ON weighingMeasuresTareWt.loadingId = loading.idLoading AND weighingMeasuresTareWt.row_number = 1" +                   
                    " LEFT JOIN dimStatus dimStatus ON dimStatus.idStatus = loading.statusId" +
                    " WHERE CAST(loading.createdOn AS DATE) BETWEEN CAST(@fromDate AS DATE) AND CAST(@toDate AS DATE)" +
                    "  " +
                    " DECLARE @VarID INT" +
                    " SET  @VarID = (SELECT ISNULL(COUNT(ISNULL(IdR1,0)),0) FROM @Temp)" +
                    " DECLARE @VarRid INT" +
                    " SELECT TOP(1) @VarRid = IdR1 FROM @Temp" +
                    " WHILE @VarID !=0" +
                    "    BEGIN" +
                    "       DECLARE @loadingId  INT" +
                    "            SELECT  @loadingId = loadingId FROM @Temp WHERE IdR1 = @VarRid" +
                    " INSERT INTO @Temp1 (rowNumber,idWeightMeasure,loadingId,intermediateWt)" +
                    " SELECT ROW_NUMBER() OVER(PARTITION BY tempWeighingMeasures.loadingId ORDER BY tempWeighingMeasures.idWeightMeasure ASC) AS row_number," +
                    " tempWeighingMeasures.idWeightMeasure,tempWeighingMeasures.loadingId,tempWeighingMeasures.weightMT" +
                    " FROM tempWeighingMeasures tempWeighingMeasures" +
                    " WHERE tempWeighingMeasures.weightMeasurTypeId = 2 AND tempWeighingMeasures.loadingId =@loadingId " +
                    " " +
                    " UPDATE @Temp2 SET firstWeight = ( SELECT ISNULL(intermediateWt,0) FROM @Temp1 WHERE rowNumber = 1 AND loadingId = @loadingId) WHERE loadingId = @loadingId " +
                    " UPDATE @Temp2 SET secondWeight = ( SELECT ISNULL(intermediateWt,0) FROM @Temp1 WHERE rowNumber = 2 AND loadingId = @loadingId) WHERE loadingId = @loadingId" +
                    " UPDATE @Temp2 SET thirdWeight = ( SELECT ISNULL(intermediateWt,0) FROM @Temp1 WHERE rowNumber = 3 AND loadingId = @loadingId) WHERE loadingId = @loadingId" +
                    " UPDATE @Temp2 SET forthWeight = ( SELECT ISNULL(intermediateWt,0) FROM @Temp1 WHERE rowNumber = 4 AND loadingId = @loadingId) WHERE loadingId = @loadingId" +
                    " UPDATE @Temp2 SET fifthWeight = ( SELECT ISNULL(intermediateWt,0) FROM @Temp1 WHERE rowNumber = 5 AND loadingId = @loadingId) WHERE loadingId = @loadingId" +
                    " UPDATE @Temp2 SET sixthWeight = ( SELECT ISNULL(intermediateWt,0) FROM @Temp1 WHERE rowNumber = 6 AND loadingId = @loadingId) WHERE loadingId = @loadingId" +
                    " UPDATE @Temp2 SET seventhWeight = ( SELECT ISNULL(intermediateWt,0) FROM @Temp1 WHERE rowNumber = 7 AND loadingId = @loadingId) WHERE loadingId = @loadingId" +
                    " " +
                    " DELETE @Temp WHERE IdR1 = @VarRid" +
                    " SET  @VarID = (SELECT ISNULL(COUNT(ISNULL(IdR1,0)),0) FROM @Temp) " +
                    " SELECT TOP(1) @VarRid = IdR1 FROM @Temp " +
                    " END" +
                    " SELECT wBID,userID,orignalRSTNo,additionalRSTNo,date,time,materialType,materialSubType,grossWeight,firstWeight,secondWeight,thirdWeight,forthWeight," +
                    " fifthWeight,sixthWeight,seventhWeight,tareWeight,netWeight," +
                    " loadOrUnload,fromLocation,toLocation,transactionType,UPPER(vehicleNumber) AS vehicleNumber,vehicleStatus,billType,vehicleID" +
                    " FROM @Temp2";

                cmdSelect.CommandText = selectQuery;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                cmdSelect.Parameters.Add("@fromDate", System.Data.SqlDbType.Date).Value = frmDt;
                cmdSelect.Parameters.Add("@toDate", System.Data.SqlDbType.Date).Value = toDt;
                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblWBRptTO> list = ConvertDTToListForRPTWBReport(reader);

                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (reader != null) reader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public List<TblWBRptTO> SelectWBForUnloadReportList(DateTime frmDt, DateTime toDt)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            string selectQuery = String.Empty;
            //DateTime sysDate = Constants.ServerDateTime;
            try
            {
                conn.Open();
                selectQuery = " DECLARE @Temp TABLE (idR1 INT IDENTITY NOT NULL,unLoadingId INT)" +
                    " DECLARE @Temp1 TABLE( idR2 INT IDENTITY NOT NULL,rowNumber INT,idWeightMeasure INT,unLoadingId INT,intermediateWt DECIMAL(18,3))" +
                    " DECLARE @Temp2 TABLE(idR3 INT IDENTITY NOT NULL,unLoadingId INT,wBID NVARCHAR(100),userID NVARCHAR(100),orignalRSTNo NVARCHAR(200),additionalRSTNo NVARCHAR(200),date NVARCHAR(20),time NVARCHAR(20),materialType NVARCHAR(500)," +
                    " materialSubType NVARCHAR(1000),grossWeight DECIMAL(18,2),firstWeight DECIMAL(18,2),secondWeight DECIMAL(18,2),thirdWeight DECIMAL(18,2),forthWeight DECIMAL(18,2),fifthWeight DECIMAL(18,2),sixthWeight DECIMAL(18,2)," +
                    " seventhWeight DECIMAL(18,2),tareWeight DECIMAL(18,2),netWeight DECIMAL(18,2),loadOrUnload NVARCHAR(50),fromLocation NVARCHAR(100),toLocation NVARCHAR(100),transactionType NVARCHAR(100),vehicleNumber NVARCHAR(100),vehicleStatus NVARCHAR(100),billType NVARCHAR(100),vehicleID NVARCHAR(100))" +
                    " INSERT INTO @Temp (unLoadingId) SELECT tblUnLoading.idUnLoading FROM tblUnLoading tblUnLoading WHERE CAST(tblUnLoading.createdOn AS DATE) BETWEEN CAST(@fromDate AS DATE) AND CAST(@toDate AS DATE) " +
                    " INSERT INTO @Temp2 " +
                    " (unLoadingId,wBID,userID,orignalRSTNo,additionalRSTNo,date,time,materialType,materialSubType,grossWeight,tareWeight,netWeight," +
                    " loadOrUnload,fromLocation,toLocation,transactionType,vehicleNumber,vehicleStatus,billType,vehicleID)" +
                    " SELECT tblUnLoading.idUnLoading,weighingMeasures.machineName,weighingMeasures.userDisplayName," +
                    " '-' AS orignalRSTNo,'-' AS additionalRSTNo,FORMAT(weighingMeasuresGrossWt.createdOn,'dd/MM/yyyy') AS date," +
                    " CONVERT(CHAR(5),weighingMeasuresGrossWt.createdOn, 108) AS time,'Other Unloading' AS materialType,tblUnLoading.remark AS materialSubType, " +
                    " weighingMeasuresGrossWt.weightMT AS grossWeight,weighingMeasuresTareWt.weightMT AS tareWeight," +
                    " CASE WHEN ((ISNULL(weighingMeasuresGrossWt.weightMT,0)) - (ISNULL(weighingMeasuresTareWt.weightMT,0))) < 0 THEN 0 " +
                    " ELSE ((ISNULL(weighingMeasuresGrossWt.weightMT,0)) - (ISNULL(weighingMeasuresTareWt.weightMT,0))) END AS netWeight," +
                    " 'Unload' AS loadOrUnload,'-' AS fromLocation ,'Jalna' AS toLocation,'-' AS transactionType," +
                    " tblUnLoading.vehicleNo AS vehicleNumber, '-' AS vehicleStatus,'-' AS billType,tblUnLoading.idUnLoading AS vehicleId " +
                    " FROM tblUnLoading tblUnLoading " +
                    " LEFT JOIN (" +
                    "              SELECT ROW_NUMBER() OVER(PARTITION BY tempWeighingMeasures.unLoadingId ORDER BY tblWeighingMachine.machineName DESC) AS row_number," +
                    "              tempWeighingMeasures.idWeightMeasure,tempWeighingMeasures.unLoadingId,tblWeighingMachine.machineName,tblUser.userDisplayName" +
                    "              FROM tempWeighingMeasures tempWeighingMeasures" +
                    "              LEFT JOIN tblWeighingMachine tblWeighingMachine ON tblWeighingMachine.idWeighingMachine = tempWeighingMeasures.weighingMachineId" +
                    "              LEFT JOIN tblUser tblUser ON tblUser.idUser = tempWeighingMeasures.createdBy " +
                    "              WHERE tempWeighingMeasures.weightMeasurTypeId = 1 " +
                    "           ) AS weighingMeasures" +
                    " ON weighingMeasures.unLoadingId = tblUnLoading.idUnLoading AND weighingMeasures.row_number = 1" +
                    " LEFT JOIN (" +
                    " SELECT ROW_NUMBER() OVER(PARTITION BY tempWeighingMeasures.unLoadingId ORDER BY tempWeighingMeasures.idWeightMeasure DESC) AS row_number," +
                    " tempWeighingMeasures.idWeightMeasure,tempWeighingMeasures.unLoadingId,tempWeighingMeasures.weightMT,tempWeighingMeasures.createdOn" +
                    " FROM tempWeighingMeasures tempWeighingMeasures" +
                    " WHERE tempWeighingMeasures.weightMeasurTypeId = 3" +
                    "  ) AS weighingMeasuresGrossWt" +
                    " ON weighingMeasuresGrossWt.unLoadingId = tblUnLoading.idUnLoading AND weighingMeasuresGrossWt.row_number = 1" +
                    " LEFT JOIN (" +
                    " SELECT ROW_NUMBER() OVER(PARTITION BY tempWeighingMeasures.unLoadingId ORDER BY tempWeighingMeasures.idWeightMeasure ASC) AS row_number," +
                    " tempWeighingMeasures.idWeightMeasure,tempWeighingMeasures.unLoadingId,tempWeighingMeasures.weightMT" +
                    " FROM tempWeighingMeasures tempWeighingMeasures" +
                    " WHERE tempWeighingMeasures.weightMeasurTypeId = 1" +
                    " ) AS weighingMeasuresTareWt" +
                    " ON weighingMeasuresTareWt.unLoadingId = tblUnLoading.idUnLoading AND weighingMeasuresTareWt.row_number = 1" +
                    " WHERE CAST(tblUnLoading.createdOn AS Date) BETWEEN CAST(@fromDate AS DATE) AND CAST(@toDate AS DATE)" +
                    "  " +
                    " DECLARE @VarID INT" +
                    " SET  @VarID = (SELECT ISNULL(COUNT(ISNULL(IdR1,0)),0) FROM @Temp)" +
                    " DECLARE @VarRid INT" +
                    " SELECT TOP(1) @VarRid = IdR1 FROM @Temp" +
                    " WHILE @VarID !=0" +
                    "    BEGIN" +
                    "       DECLARE @UnLoadingId  INT" +
                    "            SELECT  @UnLoadingId = unLoadingId FROM @Temp WHERE IdR1 = @VarRid" +
                    " INSERT INTO @Temp1 (rowNumber,idWeightMeasure,unLoadingId,intermediateWt)" +
                    " SELECT ROW_NUMBER() OVER(PARTITION BY tempWeighingMeasures.unLoadingId ORDER BY tempWeighingMeasures.idWeightMeasure ASC) AS row_number," +
                    " tempWeighingMeasures.idWeightMeasure,tempWeighingMeasures.unLoadingId,tempWeighingMeasures.weightMT" +
                    " FROM tempWeighingMeasures tempWeighingMeasures" +
                    " WHERE tempWeighingMeasures.weightMeasurTypeId = 2 AND tempWeighingMeasures.unLoadingId =@UnLoadingId " +
                    " " +
                    " UPDATE @Temp2 SET firstWeight = ( SELECT ISNULL(intermediateWt,0) FROM @Temp1 WHERE rowNumber = 1 AND unLoadingId =@UnLoadingId) WHERE unLoadingId =@UnLoadingId " +
                    " UPDATE @Temp2 SET secondWeight = ( SELECT ISNULL(intermediateWt,0) FROM @Temp1 WHERE rowNumber = 2 AND unLoadingId =@UnLoadingId) WHERE unLoadingId =@UnLoadingId" +
                    " UPDATE @Temp2 SET thirdWeight = ( SELECT ISNULL(intermediateWt,0) FROM @Temp1 WHERE rowNumber = 3 AND unLoadingId =@UnLoadingId) WHERE unLoadingId =@UnLoadingId" +
                    " UPDATE @Temp2 SET forthWeight = ( SELECT ISNULL(intermediateWt,0) FROM @Temp1 WHERE rowNumber = 4 AND unLoadingId =@UnLoadingId) WHERE unLoadingId =@UnLoadingId" +
                    " UPDATE @Temp2 SET fifthWeight = ( SELECT ISNULL(intermediateWt,0) FROM @Temp1 WHERE rowNumber = 5 AND unLoadingId =@UnLoadingId) WHERE unLoadingId =@UnLoadingId" +
                    " UPDATE @Temp2 SET sixthWeight = ( SELECT ISNULL(intermediateWt,0) FROM @Temp1 WHERE rowNumber = 6 AND unLoadingId =@UnLoadingId) WHERE unLoadingId =@UnLoadingId" +
                    " UPDATE @Temp2 SET seventhWeight = ( SELECT ISNULL(intermediateWt,0) FROM @Temp1 WHERE rowNumber = 7 AND unLoadingId =@UnLoadingId) WHERE unLoadingId =@UnLoadingId" +
                    " " +
                    " DELETE @Temp WHERE IdR1 = @VarRid" +
                    " SET  @VarID = (SELECT ISNULL(COUNT(ISNULL(IdR1,0)),0) FROM @Temp) " +
                    " SELECT TOP(1) @VarRid = IdR1 FROM @Temp " +
                    " END" +
                    " SELECT wBID,userID,orignalRSTNo,additionalRSTNo,date,time,materialType,materialSubType,grossWeight,firstWeight,secondWeight,thirdWeight,forthWeight," +
                    " fifthWeight,sixthWeight,seventhWeight,tareWeight,netWeight," +
                    " loadOrUnload,fromLocation,toLocation,transactionType,UPPER(vehicleNumber) AS vehicleNumber,vehicleStatus,billType,vehicleID" +
                    " FROM @Temp2";

                cmdSelect.CommandText = selectQuery;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                cmdSelect.Parameters.Add("@fromDate", System.Data.SqlDbType.Date).Value = frmDt;
                cmdSelect.Parameters.Add("@toDate", System.Data.SqlDbType.Date).Value = toDt;
                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblWBRptTO> list = ConvertDTToListForRPTWBReport(reader);

                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (reader != null) reader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        /// <summary>
        /// Minal[02-April-2021] Added This method to convert dt to rpt WB Report List
        /// </summary>
        /// <param name="tblInvoiceRptTODT"></param>
        /// <returns></returns>
        public List<TblWBRptTO> ConvertDTToListForRPTWBReport(SqlDataReader tblWBRptTOTODT)
        {
            List<TblWBRptTO> TblWBRptTOList = new List<TblWBRptTO>();
            try
            {
                if (tblWBRptTOTODT != null)
                {

                    while (tblWBRptTOTODT.Read())
                    {
                        TblWBRptTO tblWBRptTONew = new TblWBRptTO();
                        //for (int i = 0; i < tblWBRptTOTODT.FieldCount; i++)
                        //{
                        //    if (tblWBRptTOTODT.GetName(i).Equals("wBID"))
                        //    {
                                if (tblWBRptTOTODT["wBID"] != DBNull.Value)
                                    tblWBRptTONew.WBID = Convert.ToString(tblWBRptTOTODT["wBID"].ToString());
                            //}
                            //if (tblWBRptTOTODT.GetName(i).Equals("userID"))
                            //{
                                if (tblWBRptTOTODT["userID"] != DBNull.Value)
                                    tblWBRptTONew.UserID = Convert.ToString(tblWBRptTOTODT["userID"].ToString());
                            //}
                            //if (tblWBRptTOTODT.GetName(i).Equals("orignalRSTNo"))
                            //{
                                if (tblWBRptTOTODT["orignalRSTNo"] != DBNull.Value)
                                    tblWBRptTONew.OrignalRSTNo = Convert.ToString(tblWBRptTOTODT["orignalRSTNo"].ToString());
                            //}
                            //if (tblWBRptTOTODT.GetName(i).Equals("additionalRSTNo"))
                            //{
                                if (tblWBRptTOTODT["orignalRSTNo"] != DBNull.Value)
                                    tblWBRptTONew.AdditionalRSTNo = Convert.ToString(tblWBRptTOTODT["additionalRSTNo"].ToString());
                            //}
                            //if (tblWBRptTOTODT.GetName(i).Equals("date"))
                            //{
                                if (tblWBRptTOTODT["date"] != DBNull.Value)
                                    tblWBRptTONew.Date = Convert.ToString(tblWBRptTOTODT["date"].ToString());
                            //}
                            //if (tblWBRptTOTODT.GetName(i).Equals("time"))
                            //{
                                if (tblWBRptTOTODT["time"] != DBNull.Value)
                                    tblWBRptTONew.Time = Convert.ToString(tblWBRptTOTODT["time"].ToString());
                            //}
                            //if (tblWBRptTOTODT.GetName(i).Equals("materialType"))
                            //{
                                if (tblWBRptTOTODT["materialType"] != DBNull.Value)
                                    tblWBRptTONew.MaterialType = Convert.ToString(tblWBRptTOTODT["materialType"].ToString());
                            //}
                            //if (tblWBRptTOTODT.GetName(i).Equals("materialSubType"))
                            //{
                                if (tblWBRptTOTODT["materialSubType"] != DBNull.Value)
                                    tblWBRptTONew.MaterialSubType = Convert.ToString(tblWBRptTOTODT["materialSubType"].ToString());
                            //}
                            //if (tblWBRptTOTODT.GetName(i).Equals("grossWeight"))
                            //{
                                if (tblWBRptTOTODT["grossWeight"] != DBNull.Value)
                                    tblWBRptTONew.GrossWeight = Convert.ToDecimal(tblWBRptTOTODT["grossWeight"].ToString());
                            //}
                            //if (tblWBRptTOTODT.GetName(i).Equals("firstWeight"))
                            //{
                                if (tblWBRptTOTODT["firstWeight"] != DBNull.Value)
                                    tblWBRptTONew.FirstWeight = Convert.ToDecimal(tblWBRptTOTODT["firstWeight"].ToString());
                            //}
                            //if (tblWBRptTOTODT.GetName(i).Equals("secondWeight"))
                            //{
                                if (tblWBRptTOTODT["secondWeight"] != DBNull.Value)
                                    tblWBRptTONew.SecondWeight = Convert.ToDecimal(tblWBRptTOTODT["secondWeight"].ToString());
                            //}
                            //if (tblWBRptTOTODT.GetName(i).Equals("thirdWeight"))
                            //{
                                if (tblWBRptTOTODT["thirdWeight"] != DBNull.Value)
                                    tblWBRptTONew.ThirdWeight = Convert.ToDecimal(tblWBRptTOTODT["thirdWeight"].ToString());
                            //}
                            //if (tblWBRptTOTODT.GetName(i).Equals("forthWeight"))
                            //{
                                if (tblWBRptTOTODT["forthWeight"] != DBNull.Value)
                                    tblWBRptTONew.ForthWeight = Convert.ToDecimal(tblWBRptTOTODT["forthWeight"].ToString());
                            //}
                            //if (tblWBRptTOTODT.GetName(i).Equals("fifthWeight"))
                            //{
                                if (tblWBRptTOTODT["fifthWeight"] != DBNull.Value)
                                    tblWBRptTONew.FifthWeight = Convert.ToDecimal(tblWBRptTOTODT["fifthWeight"].ToString());
                            //}
                            //if (tblWBRptTOTODT.GetName(i).Equals("sixthWeight"))
                            //{
                                if (tblWBRptTOTODT["sixthWeight"] != DBNull.Value)
                                    tblWBRptTONew.SixthWeight = Convert.ToDecimal(tblWBRptTOTODT["sixthWeight"].ToString());
                            //}
                            //if (tblWBRptTOTODT.GetName(i).Equals("seventhWeight"))
                            //{
                                if (tblWBRptTOTODT["seventhWeight"] != DBNull.Value)
                                    tblWBRptTONew.SeventhWeight = Convert.ToDecimal(tblWBRptTOTODT["seventhWeight"].ToString());
                            //}
                            //if (tblWBRptTOTODT.GetName(i).Equals("tareWeight"))
                            //{
                                if (tblWBRptTOTODT["tareWeight"] != DBNull.Value)
                                    tblWBRptTONew.TareWeight = Convert.ToDecimal(tblWBRptTOTODT["tareWeight"].ToString());
                            //}
                            //if (tblWBRptTOTODT.GetName(i).Equals("netWeight"))
                            //{
                                if (tblWBRptTOTODT["netWeight"] != DBNull.Value)
                                    tblWBRptTONew.NetWeight = Convert.ToDecimal(tblWBRptTOTODT["netWeight"].ToString());
                            //}
                            //if (tblWBRptTOTODT.GetName(i).Equals("loadOrUnload"))
                            //{
                                if (tblWBRptTOTODT["loadOrUnload"] != DBNull.Value)
                                    tblWBRptTONew.LoadOrUnload = Convert.ToString(tblWBRptTOTODT["loadOrUnload"].ToString());
                            //}
                            //if (tblWBRptTOTODT.GetName(i).Equals("fromLocation"))
                            //{
                                if (tblWBRptTOTODT["fromLocation"] != DBNull.Value)
                                    tblWBRptTONew.FromLocation = Convert.ToString(tblWBRptTOTODT["fromLocation"].ToString());
                            //}
                            //if (tblWBRptTOTODT.GetName(i).Equals("toLocation"))
                            //{
                                if (tblWBRptTOTODT["toLocation"] != DBNull.Value)
                                    tblWBRptTONew.ToLocation = Convert.ToString(tblWBRptTOTODT["toLocation"].ToString());
                            //}
                            //if (tblWBRptTOTODT.GetName(i).Equals("transactionType"))
                            //{
                                if (tblWBRptTOTODT["transactionType"] != DBNull.Value)
                                    tblWBRptTONew.TransactionType = Convert.ToString(tblWBRptTOTODT["transactionType"].ToString());
                            //}
                            //if (tblWBRptTOTODT.GetName(i).Equals("vehicleNumber"))
                            //{
                                if (tblWBRptTOTODT["vehicleNumber"] != DBNull.Value)
                                    tblWBRptTONew.VehicleNumber = Convert.ToString(tblWBRptTOTODT["vehicleNumber"].ToString());
                            //}
                            //if (tblWBRptTOTODT.GetName(i).Equals("vehicleStatus"))
                            //{
                                if (tblWBRptTOTODT["vehicleStatus"] != DBNull.Value)
                                    tblWBRptTONew.VehicleStatus = Convert.ToString(tblWBRptTOTODT["vehicleStatus"].ToString());
                            //}
                            //if (tblWBRptTOTODT.GetName(i).Equals("billType"))
                            //{
                                if (tblWBRptTOTODT["billType"] != DBNull.Value)
                                    tblWBRptTONew.BillType = Convert.ToString(tblWBRptTOTODT["billType"].ToString());
                            //}
                            //if (tblWBRptTOTODT.GetName(i).Equals("vehicleID"))
                            //{
                                if (tblWBRptTOTODT["vehicleID"] != DBNull.Value)
                                    tblWBRptTONew.VehicleID = Convert.ToString(tblWBRptTOTODT["vehicleID"].ToString());
                            //}
                      //  }

                        TblWBRptTOList.Add(tblWBRptTONew);

                    }
                }
                // return TblWBRptTOList;
                return TblWBRptTOList;
            }
            catch (Exception ex)
            {

                return null;
            }
        }


        //Added by minal
    }
}
