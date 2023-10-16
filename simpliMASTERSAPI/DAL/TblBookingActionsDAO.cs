using ODLMWebAPI.Models;
using simpliMASTERSAPI.DAL.Interfaces;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System;
using ODLMWebAPI.StaticStuff;

namespace simpliMASTERSAPI.DAL
{
    public class TblBookingActionsDAO : ITblBookingActionsDAO
    {
        public TblBookingActionsTO SelectLatestBookingActionTO(SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                cmdSelect.CommandText = "SELECT TOP 1 * FROM tblBookingActions ORDER BY statusDate DESC";
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblBookingActionsTO> list = ConvertDTToList(reader);
                reader.Dispose();
                if (list != null && list.Count == 1)
                    return list[0];
                else return null;
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

        public int InsertTblBookingActions(TblBookingActionsTO tblBookingActionsTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblBookingActionsTO, cmdInsert);
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                cmdInsert.Dispose();
            }
        }

        public static int ExecuteInsertionCommand(TblBookingActionsTO tblBookingActionsTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblBookingActions]( " +
                                "  [isAuto]" +
                                " ,[statusBy]" +
                                " ,[statusDate]" +
                                " ,[bookingStatus]" +
                                " )" +
                    " VALUES (" +
                                "  @IsAuto " +
                                " ,@StatusBy " +
                                " ,@StatusDate " +
                                " ,@BookingStatus " +
                                " )";

            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            //cmdInsert.Parameters.Add("@IdBookingAction", System.Data.SqlDbType.Int).Value = tblBookingActionsTO.IdBookingAction;
            cmdInsert.Parameters.Add("@IsAuto", System.Data.SqlDbType.Int).Value = tblBookingActionsTO.IsAuto;
            cmdInsert.Parameters.Add("@StatusBy", System.Data.SqlDbType.Int).Value = tblBookingActionsTO.StatusBy;
            cmdInsert.Parameters.Add("@StatusDate", System.Data.SqlDbType.DateTime).Value = tblBookingActionsTO.StatusDate;
            cmdInsert.Parameters.Add("@BookingStatus", System.Data.SqlDbType.VarChar).Value = tblBookingActionsTO.BookingStatus;
            if (cmdInsert.ExecuteNonQuery() == 1)
            {
                cmdInsert.CommandText = Constants.IdentityColumnQuery;
                tblBookingActionsTO.IdBookingAction = Convert.ToInt32(cmdInsert.ExecuteScalar());
                return 1;
            }
            else return 0;
        }

        public static List<TblBookingActionsTO> ConvertDTToList(SqlDataReader tblBookingActionsTODT)
        {
            List<TblBookingActionsTO> tblBookingActionsTOList = new List<TblBookingActionsTO>();
            if (tblBookingActionsTODT != null)
            {
                while (tblBookingActionsTODT.Read())
                {
                    TblBookingActionsTO tblBookingActionsTONew = new TblBookingActionsTO();
                    if (tblBookingActionsTODT["idBookingAction"] != DBNull.Value)
                        tblBookingActionsTONew.IdBookingAction = Convert.ToInt32(tblBookingActionsTODT["idBookingAction"].ToString());
                    if (tblBookingActionsTODT["isAuto"] != DBNull.Value)
                        tblBookingActionsTONew.IsAuto = Convert.ToInt32(tblBookingActionsTODT["isAuto"].ToString());
                    if (tblBookingActionsTODT["statusBy"] != DBNull.Value)
                        tblBookingActionsTONew.StatusBy = Convert.ToInt32(tblBookingActionsTODT["statusBy"].ToString());
                    if (tblBookingActionsTODT["statusDate"] != DBNull.Value)
                        tblBookingActionsTONew.StatusDate = Convert.ToDateTime(tblBookingActionsTODT["statusDate"].ToString());
                    if (tblBookingActionsTODT["bookingStatus"] != DBNull.Value)
                        tblBookingActionsTONew.BookingStatus = Convert.ToString(tblBookingActionsTODT["bookingStatus"].ToString());
                    tblBookingActionsTOList.Add(tblBookingActionsTONew);
                }
            }
            return tblBookingActionsTOList;
        }
    }
}
