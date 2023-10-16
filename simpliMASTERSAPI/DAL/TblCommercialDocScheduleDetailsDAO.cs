using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using ODLMWebAPI.Models;
using ODLMWebAPI.BL.Interfaces;
using ODLMWebAPI.DAL.Interfaces;
using ODLMWebAPI.StaticStuff;

namespace ODLMWebAPI.DAL
{
    class TblCommercialDocScheduleDetailsDAO : ITblCommercialDocScheduleDetailsDAO
    {
        private readonly IConnectionString _iConnectionString;

        /// <summary>
        /// Sanjay [27-Sept-2019] Main Constructor For Injecting Dependencies
        /// </summary>
        /// <param name="iConnectionString"></param>
        public TblCommercialDocScheduleDetailsDAO(IConnectionString iConnectionString)
        {
            _iConnectionString = iConnectionString;

        }
        #region Methods
        public  String SqlSelectQuery()
        {
            String sqlSelectQry = "select tblCommercialDocSchedule.*,tblCommercialDocScheduleDetails.idCommercialDocScheduleDetails,tblCommercialDocScheduleDetails.commecialDocumentId,tblCommercialDocScheduleDetails.commercialDocScheduleId,tblCommercialDocScheduleDetails.commericalDocItemDtlsId,tblCommercialDocScheduleDetails.productItemId,txnQty,tblCommercialDocScheduleDetails.pendingScheduleQty,tblCommercialDocScheduleDetails.scheduledQty,tblCommercialDocScheduleDetails.isActive as isActiveItem from tblCommercialDocScheduleDetails tblCommercialDocScheduleDetails left join  " +
                                  " tblCommercialDocSchedule tblCommercialDocSchedule on tblCommercialDocSchedule.idCommercialDocSchedule = tblCommercialDocScheduleDetails.commercialDocScheduleId ";
                
            return sqlSelectQry;
        }
        #endregion

        #region Selection


        public List<TblCommercialDocScheduleDetailsTO> SelectAllTblCommercialDocSchedule(string Postr)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader rdr = null;

            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery();
                cmdSelect.CommandText += " Where 1 = 1 ";
                cmdSelect.CommandText += " and  tblCommercialDocScheduleDetails.commecialDocumentId in ( " + Postr + " )  and isnull(tblCommercialDocSchedule.isActive,0) = 1 ";
                //and isnull(tblCommercialDocScheduleDetails.isActive,0) = 1";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idCommercialDocSchedule", System.Data.SqlDbType.Int).Value = tblCommercialDocScheduleTO.IdCommercialDocSchedule;
                rdr = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblCommercialDocScheduleDetailsTO> list = ConvertDTToList(rdr);
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

        public List<TblCommercialDocScheduleDetailsTO> SelectAllTblCommercialDocScheduleDetails(Int32 commercialDocScheduleId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader rdr = null;

            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " where isnull(tblCommercialDocScheduleDetails.isActive,0) =1 and commercialDocScheduleId = " + commercialDocScheduleId;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                rdr = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblCommercialDocScheduleDetailsTO> list = ConvertDTToList(rdr);
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

        public List<TblCommercialDocScheduleDetailsTO> SelectTblCommercialDocScheduleDetailsList(Int64 commercialDocScheduleId,SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader rdr = null;
            try
            {
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE tblCommercialDocScheduleDetails.isActive=1 and tblCommercialDocScheduleDetails.commercialDocScheduleId = " + commercialDocScheduleId + " ";

                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idCommercialDocScheduleDetails", System.Data.SqlDbType.Int).Value = tblCommercialDocScheduleDetailsTO.IdCommercialDocScheduleDetails;
                rdr = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblCommercialDocScheduleDetailsTO> list = ConvertDTToList(rdr);
                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (rdr != null)
                    rdr.Dispose();
                cmdSelect.Dispose();
            }
        }


        public List<TblCommercialDocScheduleDetailsTO> SelectTblCommercialDocScheduleDetailsList(Int64 commercialDocScheduleId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader rdr = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE tblCommercialDocScheduleDetails.isActive=1 and tblCommercialDocScheduleDetails.commercialDocScheduleId = " + commercialDocScheduleId + " ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idCommercialDocScheduleDetails", System.Data.SqlDbType.Int).Value = tblCommercialDocScheduleDetailsTO.IdCommercialDocScheduleDetails;
                rdr = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblCommercialDocScheduleDetailsTO> list = ConvertDTToList(rdr);
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

        public TblCommercialDocScheduleDetailsTO SelectTblCommercialDocScheduleDetails(Int64 idCommercialDocScheduleDetails)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader rdr = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery()+ " WHERE idCommercialDocScheduleDetails = " + idCommercialDocScheduleDetails +" ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idCommercialDocScheduleDetails", System.Data.SqlDbType.Int).Value = tblCommercialDocScheduleDetailsTO.IdCommercialDocScheduleDetails;
                rdr = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblCommercialDocScheduleDetailsTO> list = ConvertDTToList(rdr);
                return list[0];
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


        public double SelectPendingScheduleQtyFromPOnoAgainstItemId(long commecialDocumentId, int productItemId, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader rdr = null;
            double scheduledQty = 0;
            try
            {
                cmdSelect.CommandText = "select SUM(scheduledQty) as scheduledQty from tblCommercialDocScheduleDetails where commecialDocumentId = " + commecialDocumentId
                    + " and isActive = 1 and productItemId ="+productItemId;
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                rdr = cmdSelect.ExecuteReader(CommandBehavior.Default);
                while (rdr.Read())
                {
                    if (rdr["scheduledQty"] != DBNull.Value)
                        scheduledQty = Convert.ToDouble(rdr["scheduledQty"].ToString());
                }

                return scheduledQty;
            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                if (rdr != null)
                    rdr.Dispose();

                cmdSelect.Dispose();
            }
        }

        public List<TblCommercialDocScheduleDetailsTO> SelectAllTblCommercialDocScheduleDetails(SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader rdr = null;
            try
            {
                cmdSelect.CommandText = SqlSelectQuery();
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idCommercialDocScheduleDetails", System.Data.SqlDbType.Int).Value = tblCommercialDocScheduleDetailsTO.IdCommercialDocScheduleDetails;
                rdr = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblCommercialDocScheduleDetailsTO> list = ConvertDTToList(rdr);
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


        public List<TblCommercialDocScheduleDetailsTO> ConvertDTToList(SqlDataReader tblCommercialDocScheduleDetailsTODT)
        {
            List<TblCommercialDocScheduleDetailsTO> tblCommercialDocScheduleDetailsTOList = new List<TblCommercialDocScheduleDetailsTO>();
            if (tblCommercialDocScheduleDetailsTODT != null)
            {
                while (tblCommercialDocScheduleDetailsTODT.Read()) {
                    TblCommercialDocScheduleDetailsTO tblCommercialDocScheduleDetailsTONew = new TblCommercialDocScheduleDetailsTO();
                    if (tblCommercialDocScheduleDetailsTODT["idCommercialDocScheduleDetails"] != DBNull.Value)
                        tblCommercialDocScheduleDetailsTONew.IdCommercialDocScheduleDetails = Convert.ToInt32(tblCommercialDocScheduleDetailsTODT["idCommercialDocScheduleDetails"].ToString());
                    if (tblCommercialDocScheduleDetailsTODT["commercialDocScheduleId"] != DBNull.Value)
                        tblCommercialDocScheduleDetailsTONew.CommercialDocScheduleId = Convert.ToInt32(tblCommercialDocScheduleDetailsTODT["commercialDocScheduleId"].ToString());
                    if (tblCommercialDocScheduleDetailsTODT["productItemId"] != DBNull.Value)
                        tblCommercialDocScheduleDetailsTONew.ProductItemId = Convert.ToInt32(tblCommercialDocScheduleDetailsTODT["productItemId"].ToString());
                    if (tblCommercialDocScheduleDetailsTODT["txnQty"] != DBNull.Value)
                        tblCommercialDocScheduleDetailsTONew.TxnQty = Convert.ToDouble(tblCommercialDocScheduleDetailsTODT["txnQty"].ToString());
                    if (tblCommercialDocScheduleDetailsTODT["scheduledQty"] != DBNull.Value)
                        tblCommercialDocScheduleDetailsTONew.ScheduledQty = Convert.ToDouble(tblCommercialDocScheduleDetailsTODT["scheduledQty"].ToString());
                    if (tblCommercialDocScheduleDetailsTODT["pendingScheduleQty"] != DBNull.Value)
                        tblCommercialDocScheduleDetailsTONew.PendingScheduleQty = Convert.ToDouble(tblCommercialDocScheduleDetailsTODT["PendingScheduleQty"].ToString());
                    if (tblCommercialDocScheduleDetailsTODT["commecialDocumentId"] != DBNull.Value)
                        tblCommercialDocScheduleDetailsTONew.CommecialDocumentId = Convert.ToInt64(tblCommercialDocScheduleDetailsTODT["commecialDocumentId"].ToString());
                    if (tblCommercialDocScheduleDetailsTODT["commericalDocItemDtlsId"] != DBNull.Value)
                        tblCommercialDocScheduleDetailsTONew.CommericalDocItemDtlsId = Convert.ToInt64(tblCommercialDocScheduleDetailsTODT["commericalDocItemDtlsId"].ToString());
                    if (tblCommercialDocScheduleDetailsTODT["transactionTypeId"] != DBNull.Value)
                        tblCommercialDocScheduleDetailsTONew.TransactionTypeId = Convert.ToInt32(tblCommercialDocScheduleDetailsTODT["transactionTypeId"].ToString());
                    if (tblCommercialDocScheduleDetailsTODT["transporterOrgId"] != DBNull.Value)
                        tblCommercialDocScheduleDetailsTONew.TransporterOrgId = Convert.ToInt32(tblCommercialDocScheduleDetailsTODT["transporterOrgId"].ToString());
                    if (tblCommercialDocScheduleDetailsTODT["createdBy"] != DBNull.Value)
                        tblCommercialDocScheduleDetailsTONew.CreatedBy = Convert.ToInt32(tblCommercialDocScheduleDetailsTODT["createdBy"].ToString());
                    if (tblCommercialDocScheduleDetailsTODT["updatedBy"] != DBNull.Value)
                        tblCommercialDocScheduleDetailsTONew.UpdatedBy = Convert.ToInt32(tblCommercialDocScheduleDetailsTODT["updatedBy"].ToString());
                    if (tblCommercialDocScheduleDetailsTODT["statusId"] != DBNull.Value)
                        tblCommercialDocScheduleDetailsTONew.StatusId = Convert.ToInt32(tblCommercialDocScheduleDetailsTODT["statusId"].ToString());
                    if (tblCommercialDocScheduleDetailsTODT["statusBy"] != DBNull.Value)
                        tblCommercialDocScheduleDetailsTONew.StatusBy = Convert.ToInt32(tblCommercialDocScheduleDetailsTODT["statusBy"].ToString());
                    if (tblCommercialDocScheduleDetailsTODT["scheduleDate"] != DBNull.Value)
                        tblCommercialDocScheduleDetailsTONew.ScheduleDate = Convert.ToDateTime(tblCommercialDocScheduleDetailsTODT["scheduleDate"].ToString());
                    if (tblCommercialDocScheduleDetailsTODT["createdOn"] != DBNull.Value)
                        tblCommercialDocScheduleDetailsTONew.CreatedOn = Convert.ToDateTime(tblCommercialDocScheduleDetailsTODT["createdOn"].ToString());
                    if (tblCommercialDocScheduleDetailsTODT["UpdatedOn"] != DBNull.Value)
                        tblCommercialDocScheduleDetailsTONew.UpdatedOn = Convert.ToDateTime(tblCommercialDocScheduleDetailsTODT["UpdatedOn"].ToString());
                    if (tblCommercialDocScheduleDetailsTODT["statusDate"] != DBNull.Value)
                        tblCommercialDocScheduleDetailsTONew.StatusDate = Convert.ToDateTime(tblCommercialDocScheduleDetailsTODT["statusDate"].ToString());
                    if (tblCommercialDocScheduleDetailsTODT["vehicleNo"] != DBNull.Value)
                        tblCommercialDocScheduleDetailsTONew.VehicleNo = Convert.ToString(tblCommercialDocScheduleDetailsTODT["vehicleNo"].ToString());
                    if (tblCommercialDocScheduleDetailsTODT["transporterName"] != DBNull.Value)
                        tblCommercialDocScheduleDetailsTONew.TransporterName = Convert.ToString(tblCommercialDocScheduleDetailsTODT["transporterName"].ToString());
                    if (tblCommercialDocScheduleDetailsTODT["isActive"] != DBNull.Value)
                        tblCommercialDocScheduleDetailsTONew.IsActive = Convert.ToInt32(tblCommercialDocScheduleDetailsTODT["isActive"].ToString());
                    if (tblCommercialDocScheduleDetailsTODT["isActiveItem"] != DBNull.Value)
                        tblCommercialDocScheduleDetailsTONew.IsActiveItem = Convert.ToInt32(tblCommercialDocScheduleDetailsTODT["isActiveItem"].ToString());


                    tblCommercialDocScheduleDetailsTOList.Add(tblCommercialDocScheduleDetailsTONew);
                }
            }
            return tblCommercialDocScheduleDetailsTOList;
        }


        #endregion

        #region Insertion
        public  int InsertTblCommercialDocScheduleDetails(TblCommercialDocScheduleDetailsTO tblCommercialDocScheduleDetailsTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblCommercialDocScheduleDetailsTO, cmdInsert);
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

        public  int InsertTblCommercialDocScheduleDetails(TblCommercialDocScheduleDetailsTO tblCommercialDocScheduleDetailsTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblCommercialDocScheduleDetailsTO, cmdInsert);
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

        public  int ExecuteInsertionCommand(TblCommercialDocScheduleDetailsTO tblCommercialDocScheduleDetailsTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblCommercialDocScheduleDetails]( " + 
            "  [commercialDocScheduleId]" +
            " ,[productItemId]" +
            " ,[isActive]" +
            " ,[txnQty]" +
            " ,[scheduledQty]" +
            " ,[pendingScheduleQty]" +
            " ,[commecialDocumentId]" +
            " ,[commericalDocItemDtlsId]" +
            " )" +
" VALUES (" +
            "  @CommercialDocScheduleId " +
            " ,@ProductItemId " +
            " ,@IsActive " +
            " ,@TxnQty " +
            " ,@ScheduledQty " +
            " ,@PendingScheduleQty " +
            " ,@CommecialDocumentId " +
            " ,@CommericalDocItemDtlsId " + 
            " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            cmdInsert.Parameters.Add("@CommercialDocScheduleId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblCommercialDocScheduleDetailsTO.CommercialDocScheduleId);
            cmdInsert.Parameters.Add("@ProductItemId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblCommercialDocScheduleDetailsTO.ProductItemId);
            cmdInsert.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = tblCommercialDocScheduleDetailsTO.IsActiveItem;
            cmdInsert.Parameters.Add("@TxnQty", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblCommercialDocScheduleDetailsTO.TxnQty);
            cmdInsert.Parameters.Add("@ScheduledQty", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblCommercialDocScheduleDetailsTO.ScheduledQty);
            cmdInsert.Parameters.Add("@PendingScheduleQty", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblCommercialDocScheduleDetailsTO.PendingScheduleQty);
            cmdInsert.Parameters.Add("@CommecialDocumentId", System.Data.SqlDbType.BigInt).Value = Constants.GetSqlDataValueNullForBaseValue(tblCommercialDocScheduleDetailsTO.CommecialDocumentId);
            cmdInsert.Parameters.Add("@CommericalDocItemDtlsId", System.Data.SqlDbType.BigInt).Value = Constants.GetSqlDataValueNullForBaseValue(tblCommercialDocScheduleDetailsTO.CommericalDocItemDtlsId);
            return cmdInsert.ExecuteNonQuery();
        }
        #endregion
        
        #region Updation
        public  int UpdateTblCommercialDocScheduleDetails(TblCommercialDocScheduleDetailsTO tblCommercialDocScheduleDetailsTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblCommercialDocScheduleDetailsTO, cmdUpdate);
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

       public  int UpdatePendingQtyofAllSchedule(long commecialDocumentId, int productItemId, double pendingScheduleQty, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                String sqlQuery = @" UPDATE [tblCommercialDocScheduleDetails] SET " +           
                  " [PendingScheduleQty]= @PendingScheduleQty" +
                  " WHERE 1 = 1 and commecialDocumentId=@CommecialDocumentId and productItemId =@ProductItemId";

                cmdUpdate.CommandText = sqlQuery;
                cmdUpdate.CommandType = System.Data.CommandType.Text;

                cmdUpdate.Parameters.Add("@ProductItemId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(productItemId);
                cmdUpdate.Parameters.Add("@PendingScheduleQty", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(pendingScheduleQty);
                cmdUpdate.Parameters.Add("@CommecialDocumentId", System.Data.SqlDbType.BigInt).Value = Constants.GetSqlDataValueNullForBaseValue(commecialDocumentId);
                return cmdUpdate.ExecuteNonQuery();

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

        public int UpdateTblCommercialDocScheduleDetails(TblCommercialDocScheduleDetailsTO tblCommercialDocScheduleDetailsTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblCommercialDocScheduleDetailsTO, cmdUpdate);
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

        public  int ExecuteUpdationCommand(TblCommercialDocScheduleDetailsTO tblCommercialDocScheduleDetailsTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblCommercialDocScheduleDetails] SET " + 
            "  [commercialDocScheduleId]= @CommercialDocScheduleId" +
            " ,[productItemId]= @ProductItemId" +
            " ,[isActive]= @IsActive" +
            " ,[txnQty]= @TxnQty" +
            " ,[scheduledQty]= @ScheduledQty" +
            " ,[PendingScheduleQty]= @PendingScheduleQty" +
            " ,[commecialDocumentId]= @CommecialDocumentId" +
            " ,[commericalDocItemDtlsId] = @CommericalDocItemDtlsId" +
            " WHERE 1 = 1 and idCommercialDocScheduleDetails=@IdCommercialDocScheduleDetails"; 

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdCommercialDocScheduleDetails", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblCommercialDocScheduleDetailsTO.IdCommercialDocScheduleDetails);
            cmdUpdate.Parameters.Add("@CommercialDocScheduleId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblCommercialDocScheduleDetailsTO.CommercialDocScheduleId);
            cmdUpdate.Parameters.Add("@ProductItemId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblCommercialDocScheduleDetailsTO.ProductItemId);
            cmdUpdate.Parameters.Add("@TxnQty", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblCommercialDocScheduleDetailsTO.TxnQty);
            cmdUpdate.Parameters.Add("@ScheduledQty", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblCommercialDocScheduleDetailsTO.ScheduledQty);
            cmdUpdate.Parameters.Add("@PendingScheduleQty", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblCommercialDocScheduleDetailsTO.PendingScheduleQty);
            cmdUpdate.Parameters.Add("@CommecialDocumentId", System.Data.SqlDbType.BigInt).Value = Constants.GetSqlDataValueNullForBaseValue(tblCommercialDocScheduleDetailsTO.CommecialDocumentId);
            cmdUpdate.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = tblCommercialDocScheduleDetailsTO.IsActiveItem;
            cmdUpdate.Parameters.Add("@CommericalDocItemDtlsId", System.Data.SqlDbType.BigInt).Value = Constants.GetSqlDataValueNullForBaseValue(tblCommercialDocScheduleDetailsTO.CommericalDocItemDtlsId);
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion
        
        #region Deletion
        public  int DeleteTblCommercialDocScheduleDetails(Int32 idCommercialDocScheduleDetails)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idCommercialDocScheduleDetails, cmdDelete);
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

        public  int DeleteTblCommercialDocScheduleDetails(Int32 idCommercialDocScheduleDetails, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idCommercialDocScheduleDetails, cmdDelete);
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

        public  int ExecuteDeletionCommand(Int32 idCommercialDocScheduleDetails, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tblCommercialDocScheduleDetails] " +
            " WHERE idCommercialDocScheduleDetails = " + idCommercialDocScheduleDetails +"";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idCommercialDocScheduleDetails", System.Data.SqlDbType.Int).Value = tblCommercialDocScheduleDetailsTO.IdCommercialDocScheduleDetails;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion
        
    }
}
