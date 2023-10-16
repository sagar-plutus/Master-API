using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using ODLMWebAPI.Models;
using ODLMWebAPI.StaticStuff;
using ODLMWebAPI.BL.Interfaces;
using ODLMWebAPI.DAL.Interfaces;

namespace ODLMWebAPI.DAL
{
    public class TblRecycleDocumentDAO : ITblRecycleDocumentDAO
    {
        private readonly IConnectionString _iConnectionString;
        public TblRecycleDocumentDAO(IConnectionString iConnectionString)
        {
            _iConnectionString = iConnectionString;
        }
        #region Methods
        public  String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT * FROM [tblRecycleDocument]"; 
            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public  List<TblRecycleDocumentTO> SelectAllTblRecycleDocument()
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

                //cmdSelect.Parameters.Add("@idRecycleDocument", System.Data.SqlDbType.Int).Value = tblRecycleDocumentTO.IdRecycleDocument;
               SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblRecycleDocumentTO> list = ConvertDTToList(sqlReader);
                sqlReader.Dispose();
                sqlReader.Close();
                return list;
            }
            catch(Exception ex)
            {
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public  List<TblRecycleDocumentTO>  SelectTblRecycleDocument(Int32 idRecycleDocument)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery()+ " WHERE idRecycleDocument = " + idRecycleDocument +" ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idRecycleDocument", System.Data.SqlDbType.Int).Value = tblRecycleDocumentTO.IdRecycleDocument;
                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblRecycleDocumentTO> list = ConvertDTToList(sqlReader);
                sqlReader.Dispose();
                sqlReader.Close();
                return list;
            }
            catch(Exception ex)
            {
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }

         public  List<TblRecycleDocumentTO> SelectRecycleDocumentList(string txnId,Int32 txnTypeId,Int32 isActive,SqlConnection conn,SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                if(isActive==1)
                    cmdSelect.CommandText = SqlSelectQuery()+ " WHERE txnId IN ( " + txnId + ") AND txnTypeId= " + txnTypeId + " AND isActive=1";
                else
                    cmdSelect.CommandText = SqlSelectQuery()+ " WHERE txnId IN ( " + txnId + ") AND txnTypeId= " + txnTypeId ;

                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idRecycleDocument", System.Data.SqlDbType.Int).Value = tblRecycleDocumentTO.IdRecycleDocument;
                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblRecycleDocumentTO> list = ConvertDTToList(sqlReader);
                sqlReader.Dispose();
                sqlReader.Close();
                return list;
            }
            catch(Exception ex)
            {
                return null;
            }
            finally
            {
                cmdSelect.Dispose();
            }
        }

        public  List<TblRecycleDocumentTO> SelectAllTblRecycleDocument(SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                cmdSelect.CommandText = SqlSelectQuery();
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idRecycleDocument", System.Data.SqlDbType.Int).Value = tblRecycleDocumentTO.IdRecycleDocument;
                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblRecycleDocumentTO> list = ConvertDTToList(sqlReader);
                sqlReader.Dispose();
                sqlReader.Close();
                return list;
            }
            catch(Exception ex)
            {
                return null;
            }
            finally
            {
                cmdSelect.Dispose();
            }
        }

         public  List<TblRecycleDocumentTO> ConvertDTToList(SqlDataReader tblRecycleDocumentTODT )
        {
            List<TblRecycleDocumentTO> tblRecycleDocumentTOList = new List<TblRecycleDocumentTO>();
            if (tblRecycleDocumentTODT != null)
            {
                while(tblRecycleDocumentTODT.Read())
                {
                    TblRecycleDocumentTO tblRecycleDocumentTONew = new TblRecycleDocumentTO();
                    if(tblRecycleDocumentTODT ["idRecycleDocument"] != DBNull.Value)
                        tblRecycleDocumentTONew.IdRecycleDocument = Convert.ToInt32( tblRecycleDocumentTODT ["idRecycleDocument"].ToString());
                    if(tblRecycleDocumentTODT ["documentId"] != DBNull.Value)
                        tblRecycleDocumentTONew.DocumentId = Convert.ToInt32( tblRecycleDocumentTODT ["documentId"].ToString());
                    if(tblRecycleDocumentTODT ["txnId"] != DBNull.Value)
                        tblRecycleDocumentTONew.TxnId = Convert.ToInt32( tblRecycleDocumentTODT ["txnId"].ToString());
                    if(tblRecycleDocumentTODT ["txnTypeId"] != DBNull.Value)
                        tblRecycleDocumentTONew.TxnTypeId = Convert.ToInt32( tblRecycleDocumentTODT ["txnTypeId"].ToString());
                    if(tblRecycleDocumentTODT ["createdBy"] != DBNull.Value)
                        tblRecycleDocumentTONew.CreatedBy = Convert.ToInt32( tblRecycleDocumentTODT ["createdBy"].ToString());
                    if(tblRecycleDocumentTODT ["updatedBy"] != DBNull.Value)
                        tblRecycleDocumentTONew.UpdatedBy = Convert.ToInt32( tblRecycleDocumentTODT ["updatedBy"].ToString());
                    if(tblRecycleDocumentTODT ["isActive"] != DBNull.Value)
                        tblRecycleDocumentTONew.IsActive = Convert.ToInt32( tblRecycleDocumentTODT ["isActive"].ToString());
                    if(tblRecycleDocumentTODT ["createdOn"] != DBNull.Value)
                        tblRecycleDocumentTONew.CreatedOn = Convert.ToDateTime( tblRecycleDocumentTODT ["createdOn"].ToString());
                    if(tblRecycleDocumentTODT ["updatedOn"] != DBNull.Value)
                        tblRecycleDocumentTONew.UpdatedOn = Convert.ToDateTime( tblRecycleDocumentTODT ["updatedOn"].ToString());
                    tblRecycleDocumentTOList.Add(tblRecycleDocumentTONew);
                }
            }
            return tblRecycleDocumentTOList;
        }

        #endregion
        
        #region Insertion
        public  int InsertTblRecycleDocument(TblRecycleDocumentTO tblRecycleDocumentTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblRecycleDocumentTO, cmdInsert);
            }
            catch(Exception ex)
            {
                return 0;
            }
            finally
            {
                conn.Close();
                cmdInsert.Dispose();
            }
        }

        public  int InsertTblRecycleDocument(TblRecycleDocumentTO tblRecycleDocumentTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblRecycleDocumentTO, cmdInsert);
            }
            catch(Exception ex)
            {
                return 0;
            }
            finally
            {
                cmdInsert.Dispose();
            }
        }

        public  int ExecuteInsertionCommand(TblRecycleDocumentTO tblRecycleDocumentTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblRecycleDocument]( " + 
            //"  [idRecycleDocument]" +
            "  [documentId]" +
            " ,[txnId]" +
            " ,[txnTypeId]" +
            " ,[createdBy]" +
            " ,[updatedBy]" +
            " ,[isActive]" +
            " ,[createdOn]" +
            " ,[updatedOn]" +
            " )" +
" VALUES (" +
            //"  @IdRecycleDocument " +
            "  @DocumentId " +
            " ,@TxnId " +
            " ,@TxnTypeId " +
            " ,@CreatedBy " +
            " ,@UpdatedBy " +
            " ,@IsActive " +
            " ,@CreatedOn " +
            " ,@UpdatedOn " + 
            " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            //cmdInsert.Parameters.Add("@IdRecycleDocument", System.Data.SqlDbType.Int).Value = tblRecycleDocumentTO.IdRecycleDocument;
            cmdInsert.Parameters.Add("@DocumentId", System.Data.SqlDbType.Int).Value = tblRecycleDocumentTO.DocumentId;
            cmdInsert.Parameters.Add("@TxnId", System.Data.SqlDbType.Int).Value = tblRecycleDocumentTO.TxnId;
            cmdInsert.Parameters.Add("@TxnTypeId", System.Data.SqlDbType.Int).Value = tblRecycleDocumentTO.TxnTypeId;
            cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblRecycleDocumentTO.CreatedBy;
            cmdInsert.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblRecycleDocumentTO.UpdatedBy);
            cmdInsert.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = tblRecycleDocumentTO.IsActive;
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblRecycleDocumentTO.CreatedOn;
            cmdInsert.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value =Constants.GetSqlDataValueNullForBaseValue(tblRecycleDocumentTO.UpdatedOn);
            return cmdInsert.ExecuteNonQuery();
        }
        #endregion
        
        #region Updation
        public  int UpdateTblRecycleDocument(TblRecycleDocumentTO tblRecycleDocumentTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblRecycleDocumentTO, cmdUpdate);
            }
            catch(Exception ex)
            {
                return 0;
            }
            finally
            {
                conn.Close();
                cmdUpdate.Dispose();
            }
        }

        public  int UpdateTblRecycleDocument(TblRecycleDocumentTO tblRecycleDocumentTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblRecycleDocumentTO, cmdUpdate);
            }
            catch(Exception ex)
            {
                return 0;
            }
            finally
            {
                cmdUpdate.Dispose();
            }
        }

        public  int ExecuteUpdationCommand(TblRecycleDocumentTO tblRecycleDocumentTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblRecycleDocument] SET " + 
            //"  [idRecycleDocument] = @IdRecycleDocument" +
            "  [documentId]= @DocumentId" +
            " ,[txnId]= @TxnId" +
            " ,[txnTypeId]= @TxnTypeId" +
            " ,[createdBy]= @CreatedBy" +
            " ,[updatedBy]= @UpdatedBy" +
            " ,[isActive]= @IsActive" +
            " ,[createdOn]= @CreatedOn" +
            " ,[updatedOn] = @UpdatedOn" +
            " WHERE idRecycleDocument = @IdRecycleDocument "; 

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdRecycleDocument", System.Data.SqlDbType.Int).Value = tblRecycleDocumentTO.IdRecycleDocument;
            cmdUpdate.Parameters.Add("@DocumentId", System.Data.SqlDbType.Int).Value = tblRecycleDocumentTO.DocumentId;
            cmdUpdate.Parameters.Add("@TxnId", System.Data.SqlDbType.Int).Value = tblRecycleDocumentTO.TxnId;
            cmdUpdate.Parameters.Add("@TxnTypeId", System.Data.SqlDbType.Int).Value = tblRecycleDocumentTO.TxnTypeId;
            cmdUpdate.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblRecycleDocumentTO.CreatedBy;
            cmdUpdate.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = tblRecycleDocumentTO.UpdatedBy;
            cmdUpdate.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = tblRecycleDocumentTO.IsActive;
            cmdUpdate.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblRecycleDocumentTO.CreatedOn;
            cmdUpdate.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = tblRecycleDocumentTO.UpdatedOn;
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion
        
        #region Deletion
        public  int DeleteTblRecycleDocument(Int32 idRecycleDocument)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idRecycleDocument, cmdDelete);
            }
            catch(Exception ex)
            {
                return 0;
            }
            finally
            {
                conn.Close();
                cmdDelete.Dispose();
            }
        }

        public  int DeleteTblRecycleDocument(Int32 idRecycleDocument, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idRecycleDocument, cmdDelete);
            }
            catch(Exception ex)
            {
                return 0;
            }
            finally
            {
                cmdDelete.Dispose();
            }
        }

        public  int ExecuteDeletionCommand(Int32 idRecycleDocument, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tblRecycleDocument] " +
            " WHERE idRecycleDocument = " + idRecycleDocument +"";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idRecycleDocument", System.Data.SqlDbType.Int).Value = tblRecycleDocumentTO.IdRecycleDocument;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion

        public List<TblPurchaseScheduleSummaryTO> SelectPurchaseScheduleSummaryTOByVehicleNo(String vehicleNo, Int32 actualRootScheduleId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            String approvalTypeStr = string.Empty;
            String statusIdStr = Convert.ToInt32(Constants.TranStatusE.New) + "," + Convert.ToInt32(Constants.TranStatusE.VEHICLE_OUT) + "," + Convert.ToInt32(Constants.TranStatusE.DELETE_VEHICLE)
            //Prajakta[2019-05-13] Added
            + "," + Convert.ToInt32(Constants.TranStatusE.VEHICLE_REJECTED_AFTER_WEIGHING)
            + "," + Convert.ToInt32(Constants.TranStatusE.VEHICLE_REJECTED_BEFORE_WEIGHING)
            + "," + Convert.ToInt32(Constants.TranStatusE.VEHICLE_REJECTED_AFTER_GROSS_WEIGHT)
            + "," + Convert.ToInt32(Constants.TranStatusE.VEHICLE_CANCELED);
            String spotEntryCond = string.Empty;
            try
            {
                conn.Open();

                if (actualRootScheduleId != 0)
                {
                    spotEntryCond = "AND rootScheduleId NOT IN (" + actualRootScheduleId + ")";
                }

                // cmdSelect.CommandText = SelectQuery() + " WHERE vehicleNo = '" + vehicleNo + "' AND statusId NOT IN(" + statusIdStr + ") " +
                //                         "AND isActive =1 " + spotEntryCond;

                cmdSelect.CommandText = SelectQueryLight() + " WHERE vehicleNo = '" + vehicleNo + "' AND statusId NOT IN(" + statusIdStr + ") " +
                                       "AND isActive =1 " + spotEntryCond;

                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;


                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                //List<TblPurchaseScheduleSummaryTO> list = ConvertDTToList(sqlReader);
                List<TblPurchaseScheduleSummaryTO> list = ConvertDTToListLight(sqlReader);

                sqlReader.Dispose();
                if (list != null)
                    return list;
                else return null;
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

        public String SelectQueryLight()
        {
            //tblLocation.locationDesc,
            String sqlSelectQry = " select * from tblPurchaseScheduleSummary ";
            return sqlSelectQry;

        }

        public List<TblPurchaseScheduleSummaryTO> ConvertDTToListLight(SqlDataReader tblPurchaseScheduleSummaryTODT)
        {
            List<TblPurchaseScheduleSummaryTO> tblPurchaseScheduleSummaryTOList = new List<TblPurchaseScheduleSummaryTO>();
            if (tblPurchaseScheduleSummaryTODT != null)
            {
                while (tblPurchaseScheduleSummaryTODT.Read())
                {
                    TblPurchaseScheduleSummaryTO tblPurchaseScheduleSummaryTONew = new TblPurchaseScheduleSummaryTO();
                    if (tblPurchaseScheduleSummaryTODT["idPurchaseScheduleSummary"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.IdPurchaseScheduleSummary = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["idPurchaseScheduleSummary"].ToString());
                    if (tblPurchaseScheduleSummaryTODT["purchaseEnquiryId"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.PurchaseEnquiryId = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["purchaseEnquiryId"].ToString());
                    if (tblPurchaseScheduleSummaryTODT["supplierId"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.SupplierId = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["supplierId"].ToString());
                    if (tblPurchaseScheduleSummaryTODT["statusId"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.StatusId = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["statusId"].ToString());
                    if (tblPurchaseScheduleSummaryTODT["createdBy"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.CreatedBy = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["createdBy"].ToString());
                    if (tblPurchaseScheduleSummaryTODT["updatedBy"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.UpdatedBy = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["updatedBy"].ToString());
                    if (tblPurchaseScheduleSummaryTODT["scheduleDate"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.ScheduleDate = Convert.ToDateTime(tblPurchaseScheduleSummaryTODT["scheduleDate"].ToString());
                    if (tblPurchaseScheduleSummaryTODT["createdOn"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.CreatedOn = Convert.ToDateTime(tblPurchaseScheduleSummaryTODT["createdOn"].ToString());
                    if (tblPurchaseScheduleSummaryTODT["updatedOn"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.UpdatedOn = Convert.ToDateTime(tblPurchaseScheduleSummaryTODT["updatedOn"].ToString());
                    if (tblPurchaseScheduleSummaryTODT["qty"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.Qty = Convert.ToDouble(tblPurchaseScheduleSummaryTODT["qty"].ToString());
                    if (tblPurchaseScheduleSummaryTODT["scheduleQty"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.OrgScheduleQty = Convert.ToDouble(tblPurchaseScheduleSummaryTODT["scheduleQty"].ToString());
                    if (tblPurchaseScheduleSummaryTODT["calculatedMetalCost"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.CalculatedMetalCost = Convert.ToDouble(tblPurchaseScheduleSummaryTODT["calculatedMetalCost"].ToString());
                    if (tblPurchaseScheduleSummaryTODT["baseMetalCost"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.BaseMetalCost = Convert.ToDouble(tblPurchaseScheduleSummaryTODT["baseMetalCost"].ToString());
                    if (tblPurchaseScheduleSummaryTODT["padta"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.Padta = Convert.ToDouble(tblPurchaseScheduleSummaryTODT["padta"].ToString());
                    if (tblPurchaseScheduleSummaryTODT["calculatedMetalCostForEnquiry"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.CalculatedMetalCostForNC = Convert.ToDouble(tblPurchaseScheduleSummaryTODT["calculatedMetalCostForEnquiry"].ToString());
                    if (tblPurchaseScheduleSummaryTODT["baseMetalCostForEnquiry"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.BaseMetalCostForNC = Convert.ToDouble(tblPurchaseScheduleSummaryTODT["baseMetalCostForEnquiry"].ToString());
                    if (tblPurchaseScheduleSummaryTODT["padtaForEnquiry"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.PadtaForNC = Convert.ToDouble(tblPurchaseScheduleSummaryTODT["padtaForEnquiry"].ToString());

                    if (tblPurchaseScheduleSummaryTODT["vehicleNo"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.VehicleNo = Convert.ToString(tblPurchaseScheduleSummaryTODT["vehicleNo"].ToString());
                    if (tblPurchaseScheduleSummaryTODT["remark"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.Remark = Convert.ToString(tblPurchaseScheduleSummaryTODT["remark"].ToString());

                    if (tblPurchaseScheduleSummaryTODT["cOrNCId"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.COrNc = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["cOrNCId"].ToString());

                    if (tblPurchaseScheduleSummaryTODT["narrationId"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.NarrationId = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["narrationId"].ToString());


                    if (tblPurchaseScheduleSummaryTODT["engineerId"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.EngineerId = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["engineerId"].ToString());

                    if (tblPurchaseScheduleSummaryTODT["supervisorId"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.SupervisorId = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["supervisorId"].ToString());

                    if (tblPurchaseScheduleSummaryTODT["photographerId"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.PhotographerId = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["photographerId"].ToString());

                    if (tblPurchaseScheduleSummaryTODT["vehicleTypeId"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.VehicleTypeId = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["vehicleTypeId"].ToString());

                    if (tblPurchaseScheduleSummaryTODT["freight"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.Freight = Convert.ToDouble(tblPurchaseScheduleSummaryTODT["freight"].ToString());

                    if (tblPurchaseScheduleSummaryTODT["containerNo"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.ContainerNo = Convert.ToString(tblPurchaseScheduleSummaryTODT["containerNo"]);

                    if (tblPurchaseScheduleSummaryTODT["lotSize"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.LotSize = Convert.ToString(tblPurchaseScheduleSummaryTODT["lotSize"]);

                    if (tblPurchaseScheduleSummaryTODT["locationId"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.LocationId = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["locationId"]);


                    if (tblPurchaseScheduleSummaryTODT["vehicleCatId"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.VehicleCatId = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["vehicleCatId"]);


                    if (tblPurchaseScheduleSummaryTODT["vehiclePhaseId"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.VehiclePhaseId = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["vehiclePhaseId"]);


                    if (tblPurchaseScheduleSummaryTODT["isActive"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.IsActive = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["isActive"]);

                    if (tblPurchaseScheduleSummaryTODT["parentPurchaseScheduleSummaryId"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.ParentPurchaseScheduleSummaryId = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["parentPurchaseScheduleSummaryId"]);

                    if (tblPurchaseScheduleSummaryTODT["location"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.Location = Convert.ToString(tblPurchaseScheduleSummaryTODT["location"]);

                    if (tblPurchaseScheduleSummaryTODT["rootScheduleId"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.RootScheduleId = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["rootScheduleId"]);


                    if (tblPurchaseScheduleSummaryTODT["isRecovery"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.IsRecovery = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["isRecovery"]);
                    if (tblPurchaseScheduleSummaryTODT["recoveryBy"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.RecoveryBy = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["recoveryBy"]);
                    if (tblPurchaseScheduleSummaryTODT["recoveryOn"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.RecoveryOn = Convert.ToDateTime(tblPurchaseScheduleSummaryTODT["recoveryOn"]);

                    if (tblPurchaseScheduleSummaryTODT["isWeighing"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.IsWeighing = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["isWeighing"]);

                    // Deepali[07-02-2019] Added
                    if (tblPurchaseScheduleSummaryTODT["graderId"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.GraderId = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["graderId"].ToString());


                    if (tblPurchaseScheduleSummaryTODT["isGradingCompleted"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.IsGradingCompleted = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["isGradingCompleted"].ToString());

                    if (tblPurchaseScheduleSummaryTODT["isCorrectionCompleted"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.IsCorrectionCompleted = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["isCorrectionCompleted"].ToString());

                    if (tblPurchaseScheduleSummaryTODT["isUnloadingCompleted"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.IsUnloadingCompleted = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["isUnloadingCompleted"].ToString());

                    // Priyanka[28-01-2019] Added
                    if (tblPurchaseScheduleSummaryTODT["commercialApproval"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.CommercialApproval = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["commercialApproval"].ToString());

                    //Priyanka [18-02-2019]
                    if (tblPurchaseScheduleSummaryTODT["commercialVerified"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.CommercialVerified = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["commercialVerified"].ToString());

                    //Prajakta[2019-02-27] Added
                    if (tblPurchaseScheduleSummaryTODT["isBoth"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.IsBoth = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["isBoth"].ToString());


                    if (tblPurchaseScheduleSummaryTODT["isFixed"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.IsFixed = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["isFixed"].ToString());

                    if (tblPurchaseScheduleSummaryTODT["transportAmtPerMT"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.TransportAmtPerMT = Convert.ToDouble(tblPurchaseScheduleSummaryTODT["transportAmtPerMT"].ToString());

                    //Prajakta[2019-05-08] Added
                    if (tblPurchaseScheduleSummaryTODT["rejectedQty"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.RejectedQty = Convert.ToDouble(tblPurchaseScheduleSummaryTODT["rejectedQty"].ToString());

                    if (tblPurchaseScheduleSummaryTODT["rejectedBy"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.RejectedBy = Convert.ToInt32(tblPurchaseScheduleSummaryTODT["rejectedBy"].ToString());

                    if (tblPurchaseScheduleSummaryTODT["rejectedOn"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.RejectedOn = Convert.ToDateTime(tblPurchaseScheduleSummaryTODT["rejectedOn"].ToString());


                    if (tblPurchaseScheduleSummaryTODT["vehRejectReasonId"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.VehRejectReasonId = (tblPurchaseScheduleSummaryTODT["vehRejectReasonId"].ToString());

                    if (tblPurchaseScheduleSummaryTODT["corretionCompletedOn"] != DBNull.Value)
                        tblPurchaseScheduleSummaryTONew.CorretionCompletedOn = Convert.ToDateTime(tblPurchaseScheduleSummaryTODT["corretionCompletedOn"].ToString());


                    tblPurchaseScheduleSummaryTOList.Add(tblPurchaseScheduleSummaryTONew);
                }
            }
            return tblPurchaseScheduleSummaryTOList;
        }



        //////
    }
}
