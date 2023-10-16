﻿using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using ODLMWebAPI.Models;
using ODLMWebAPI.StaticStuff;
using ODLMWebAPI.DAL.Interfaces;
using ODLMWebAPI.BL.Interfaces;

namespace ODLMWebAPI.DAL
{
    public class DimProdSpecDescDAO : IDimProdSpecDescDAO
    {
        private readonly IConnectionString _iConnectionString;
        public DimProdSpecDescDAO(IConnectionString iConnectionString)
        {
            _iConnectionString = iConnectionString;
        }
        #region Methods
        public String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT * FROM [dimProdSpec] where isActive=1";
            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public List<DimProdSpecDescTO> SelectAllDimProdSpecDesc()
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " ORDER BY displaySequence ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader rdr = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DimProdSpecDescTO> list = ConvertDTToList(rdr);
                rdr.Dispose();
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

        public DimProdSpecDescTO SelectDimProdSpecDesc(Int32 idProdSpec)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE idProdSpec = " + idProdSpec + " ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader rdr = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DimProdSpecDescTO> list = ConvertDTToList(rdr);
                rdr.Dispose();
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
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public List<DimProdSpecDescTO> ConvertDTToList(SqlDataReader dimDimProdSpecTODT)
        {
            List<DimProdSpecDescTO> dimProdSpecTOList = new List<DimProdSpecDescTO>();
            if (dimDimProdSpecTODT != null)
            {
                while (dimDimProdSpecTODT.Read())
                {
                    DimProdSpecDescTO DimProdSpecTONew = new DimProdSpecDescTO();
                    if (dimDimProdSpecTODT["idProdSpec"] != DBNull.Value)
                        DimProdSpecTONew.IdProductSpecDesc = Convert.ToInt32(dimDimProdSpecTODT["idProdSpec"].ToString());
                    if (dimDimProdSpecTODT["isActive"] != DBNull.Value)
                        DimProdSpecTONew.IsActive = Convert.ToInt32(dimDimProdSpecTODT["isActive"].ToString());
                    if (dimDimProdSpecTODT["prodSpecDesc"] != DBNull.Value)
                        DimProdSpecTONew.ProdSpecDesc = Convert.ToString(dimDimProdSpecTODT["prodSpecDesc"].ToString());
                    if (dimDimProdSpecTODT["displaySequence"] != DBNull.Value)
                        DimProdSpecTONew.DisplaySequence = Convert.ToInt32(dimDimProdSpecTODT["displaySequence"].ToString());
                    dimProdSpecTOList.Add(DimProdSpecTONew);
                }
            }
            return dimProdSpecTOList;
        }

        public int SelectDimProdSpecDescription()
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteSelectCommand(cmdInsert);
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

        #endregion

        #region Insertion
        public int InsertDimProdSpecDesc(DimProdSpecDescTO dimProdSpecDescTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(dimProdSpecDescTO, cmdInsert);
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

        public int InsertDimProdSpecDesc(DimProdSpecDescTO dimProdSpecDescTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(dimProdSpecDescTO, cmdInsert);
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

        public int ExecuteInsertionCommand(DimProdSpecDescTO dimProdSpecDescTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @"INSERT INTO [dimProdSpec](" +
                           " [idProdSpec]" +
            " ,[isActive]" +
            " ,[prodSpecDesc]" +
            ", [displaySequence]" +
            " )" +
             " VALUES (" + "@idProdSpec"+
                            " ,@isActive " +
                            " ,@prodSpecDesc " +
                            " ,@displaySequence " +
                            " )";            
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            cmdInsert.Parameters.Add("@idProdSpec", System.Data.SqlDbType.Int).Value = dimProdSpecDescTO.IdProductSpecDesc;
            cmdInsert.Parameters.Add("@isActive", System.Data.SqlDbType.Int).Value = 1;
            cmdInsert.Parameters.Add("@prodSpecDesc", System.Data.SqlDbType.NVarChar).Value = dimProdSpecDescTO.ProdSpecDesc;
            cmdInsert.Parameters.Add("@displaySequence", System.Data.SqlDbType.NVarChar).Value = dimProdSpecDescTO.DisplaySequence;
            return cmdInsert.ExecuteNonQuery();
            
        }


        /// <summary>
        /// Added by vinod for the selection of the max records on Dated :12/12/2017
        /// </summary>
        /// <param name="cmdInsert"></param>
        /// <returns></returns>
        public int ExecuteSelectCommand(SqlCommand cmdInsert)
        {
            String sqlQuery = @"SELECT MAX(idProdSpec+1) FROM [dimProdSpec]"; /// where isActive = 1
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;
            int resultSet = Convert.ToInt32(cmdInsert.ExecuteScalar());
            return resultSet;
        }

        #endregion

        #region Updation
        public int UpdateDimProdSpecDesc(DimProdSpecDescTO dimProdSpecDescTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(dimProdSpecDescTO, cmdUpdate);
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

        public int UpdateDimProdSpecDesc(DimProdSpecDescTO dimProdSpecDescTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(dimProdSpecDescTO, cmdUpdate);
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

        public int ExecuteUpdationCommand(DimProdSpecDescTO dimProdSpecDescTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [dimProdSpec] SET " +
            " [isActive] = @isActive" +
            " ,[prodSpecDesc] = @prodSpecDesc" +
            " ,[displaySequence] = @displaySequence" +
             " ,[isMigration]= 0" +
            " WHERE idProdSpec=" + dimProdSpecDescTO.IdProductSpecDesc;

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@isActive", System.Data.SqlDbType.Int).Value = 1;
            cmdUpdate.Parameters.Add("@prodSpecDesc", System.Data.SqlDbType.NVarChar).Value = dimProdSpecDescTO.ProdSpecDesc;
            cmdUpdate.Parameters.Add("@displaySequence", System.Data.SqlDbType.Int).Value = dimProdSpecDescTO.DisplaySequence;
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion

        #region Deletion

        public int UpdateDimProdSpecDescription(DimProdSpecDescTO dimProdSpecDescTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdateCommand(dimProdSpecDescTO, cmdUpdate);
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

        public int UpdateDimProdSpecDescription(DimProdSpecDescTO dimProdSpecDescTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdateCommand(dimProdSpecDescTO, cmdUpdate);
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

        public int ExecuteUpdateCommand(DimProdSpecDescTO dimProdSpecDescTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [dimProdSpec] SET " +
            " [isActive] = @isActive" +
            " ,[prodSpecDesc] = @prodSpecDesc" +
            " ,[displaySequence] = @displaySequence" +
            " WHERE idProdSpec=" + dimProdSpecDescTO.IdProductSpecDesc;

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@isActive", System.Data.SqlDbType.Int).Value = 0;
            cmdUpdate.Parameters.Add("@prodSpecDesc", System.Data.SqlDbType.NVarChar).Value = dimProdSpecDescTO.ProdSpecDesc;
            cmdUpdate.Parameters.Add("@displaySequence", System.Data.SqlDbType.Int).Value = dimProdSpecDescTO.DisplaySequence;
            return cmdUpdate.ExecuteNonQuery();
        }


        #endregion
    }
}
