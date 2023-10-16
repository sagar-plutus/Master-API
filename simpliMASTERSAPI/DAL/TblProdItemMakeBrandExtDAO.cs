using ODLMWebAPI.Models;
using ODLMWebAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using ODLMWebAPI.DAL.Interfaces;
using ODLMWebAPI.BL.Interfaces;

namespace ODLMWebAPI.DAL
{
    public class TblProdItemMakeBrandExtDAO : ITblProdItemMakeBrandExtDAO
    {
        private readonly IConnectionString _iConnectionString;
        public TblProdItemMakeBrandExtDAO(IConnectionString iConnectionString)
        {
            _iConnectionString = iConnectionString;
        }

        #region Methods
        public  String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT * FROM [tblProdItemMakeBrandExt]"; 
            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public List<TblProdItemMakeBrandExtTO> SelectAllTblProdItemMakeBrandExt()
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;

            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery();
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idProdItemMakeBrandExt", System.Data.SqlDbType.Int).Value = tblProdItemMakeBrandExtTO.IdProdItemMakeBrandExt;
                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblProdItemMakeBrandExtTO> list = ConvertDTToList(reader);
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

        public TblProdItemMakeBrandExtTO SelectTblProdItemMakeBrandExt(int idProdItemMakeBrandExt)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;

            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery()+ " WHERE idProdItemMakeBrandExt = " + idProdItemMakeBrandExt +" ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idProdItemMakeBrandExt", System.Data.SqlDbType.Int).Value = tblProdItemMakeBrandExtTO.IdProdItemMakeBrandExt;
                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblProdItemMakeBrandExtTO> list = ConvertDTToList(reader);
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



        public List<TblProdItemMakeBrandExtTO> SelectAllTblProdItemMakeBrandExtByProdItem(int prodItemId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;

            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery();
                if (prodItemId != 0)
                    cmdSelect.CommandText += " where prodItemId= " +prodItemId;

                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idProdItemMakeBrandExt", System.Data.SqlDbType.Int).Value = tblProdItemMakeBrandExtTO.IdProdItemMakeBrandExt;
                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblProdItemMakeBrandExtTO> list = ConvertDTToList(reader);
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

        public List<TblProdItemMakeBrandExtTO> SelectAllTblProdItemMakeBrandExt(SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;

            try
            {
                cmdSelect.CommandText = SqlSelectQuery();
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idProdItemMakeBrandExt", System.Data.SqlDbType.Int).Value = tblProdItemMakeBrandExtTO.IdProdItemMakeBrandExt;
                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblProdItemMakeBrandExtTO> list = ConvertDTToList(reader);
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


        public List<TblProdItemMakeBrandExtTO> ConvertDTToList(SqlDataReader tblProdItemMakeBrandExtTODT)
        {
            List<TblProdItemMakeBrandExtTO> tblProdItemMakeBrandExtTOList = new List<TblProdItemMakeBrandExtTO>();
            if (tblProdItemMakeBrandExtTODT != null)
            {
                while (tblProdItemMakeBrandExtTODT.Read())
                {
                    TblProdItemMakeBrandExtTO tblProdItemMakeBrandExtTONew = new TblProdItemMakeBrandExtTO();
                    if (tblProdItemMakeBrandExtTODT["createdOn"] != DBNull.Value)
                        tblProdItemMakeBrandExtTONew.CreatedOn = Convert.ToDateTime(tblProdItemMakeBrandExtTODT["createdOn"].ToString());
                    if (tblProdItemMakeBrandExtTODT["idProdItemMakeBrandExt"] != DBNull.Value)
                        tblProdItemMakeBrandExtTONew.IdProdItemMakeBrandExt = Convert.ToInt32(tblProdItemMakeBrandExtTODT["idProdItemMakeBrandExt"].ToString());
                    if (tblProdItemMakeBrandExtTODT["prodItemId"] != DBNull.Value)
                        tblProdItemMakeBrandExtTONew.ProdItemId = Convert.ToInt32(tblProdItemMakeBrandExtTODT["prodItemId"].ToString());
                    if (tblProdItemMakeBrandExtTODT["itemMakeId"] != DBNull.Value)
                        tblProdItemMakeBrandExtTONew.ItemMakeId = Convert.ToInt32(tblProdItemMakeBrandExtTODT["itemMakeId"].ToString());
                    if (tblProdItemMakeBrandExtTODT["itemBrandId"] != DBNull.Value)
                        tblProdItemMakeBrandExtTONew.ItemBrandId = Convert.ToInt32(tblProdItemMakeBrandExtTODT["itemBrandId"].ToString());
                    if (tblProdItemMakeBrandExtTODT["isDefaultMake"] != DBNull.Value)
                        tblProdItemMakeBrandExtTONew.IsDefaultMake = Convert.ToInt32(tblProdItemMakeBrandExtTODT["isDefaultMake"].ToString());
                    if (tblProdItemMakeBrandExtTODT["createdBy"] != DBNull.Value)
                        tblProdItemMakeBrandExtTONew.CreatedBy = Convert.ToInt32(tblProdItemMakeBrandExtTODT["createdBy"].ToString());
                    if (tblProdItemMakeBrandExtTODT["rackNo"] != DBNull.Value)
                        tblProdItemMakeBrandExtTONew.RackNo = Convert.ToString(tblProdItemMakeBrandExtTODT["rackNo"].ToString());
                    if (tblProdItemMakeBrandExtTODT["xBinLocation"] != DBNull.Value)
                        tblProdItemMakeBrandExtTONew.XBinLocation = Convert.ToString(tblProdItemMakeBrandExtTODT["xBinLocation"].ToString());
                    if (tblProdItemMakeBrandExtTODT["yBinLocation"] != DBNull.Value)
                        tblProdItemMakeBrandExtTONew.YBinLocation = Convert.ToString(tblProdItemMakeBrandExtTODT["yBinLocation"].ToString());
                    if (tblProdItemMakeBrandExtTODT["catLogNo"] != DBNull.Value)
                        tblProdItemMakeBrandExtTONew.CatLogNo = Convert.ToString(tblProdItemMakeBrandExtTODT["catLogNo"].ToString());
                    tblProdItemMakeBrandExtTOList.Add(tblProdItemMakeBrandExtTONew);
                }
            }
            return tblProdItemMakeBrandExtTOList;
        }

        #endregion

        #region Insertion
        public int InsertTblProdItemMakeBrandExt(TblProdItemMakeBrandExtTO tblProdItemMakeBrandExtTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblProdItemMakeBrandExtTO, cmdInsert);
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

        public  int InsertTblProdItemMakeBrandExt(TblProdItemMakeBrandExtTO tblProdItemMakeBrandExtTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblProdItemMakeBrandExtTO, cmdInsert);
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

        public  int ExecuteInsertionCommand(TblProdItemMakeBrandExtTO tblProdItemMakeBrandExtTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblProdItemMakeBrandExt]( " + 
            "  [createdOn]" +
            " ,[prodItemId]" +
            " ,[itemMakeId]" +
            " ,[itemBrandId]" +
            " ,[isDefaultMake]" +
            " ,[createdBy]" +
            " ,[rackNo]" +
            " ,[xBinLocation]" +
            " ,[yBinLocation]" +
            " ,[catLogNo]" +
            " )" +
" VALUES (" +
            "  @CreatedOn " +
            " ,@ProdItemId " +
            " ,@ItemMakeId " +
            " ,@ItemBrandId " +
            " ,@IsDefaultMake " +
            " ,@CreatedBy " +
            " ,@RackNo " +
            " ,@XBinLocation " +
            " ,@YBinLocation " +
            " ,@CatLogNo " + 
            " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblProdItemMakeBrandExtTO.CreatedOn;
            //cmdInsert.Parameters.Add("@IdProdItemMakeBrandExt", System.Data.SqlDbType.Int).Value = tblProdItemMakeBrandExtTO.IdProdItemMakeBrandExt;
            cmdInsert.Parameters.Add("@ProdItemId", System.Data.SqlDbType.Int).Value =Constants.GetSqlDataValueNullForBaseValue(tblProdItemMakeBrandExtTO.ProdItemId);
            cmdInsert.Parameters.Add("@ItemMakeId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblProdItemMakeBrandExtTO.ItemMakeId);
            cmdInsert.Parameters.Add("@ItemBrandId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblProdItemMakeBrandExtTO.ItemBrandId);
            cmdInsert.Parameters.Add("@IsDefaultMake", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblProdItemMakeBrandExtTO.IsDefaultMake);
            cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblProdItemMakeBrandExtTO.CreatedBy);
            cmdInsert.Parameters.Add("@RackNo", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblProdItemMakeBrandExtTO.RackNo);
            cmdInsert.Parameters.Add("@XBinLocation", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblProdItemMakeBrandExtTO.XBinLocation);
            cmdInsert.Parameters.Add("@YBinLocation", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblProdItemMakeBrandExtTO.YBinLocation);
            cmdInsert.Parameters.Add("@CatLogNo", System.Data.SqlDbType.NVarChar).Value = Constants.GetSqlDataValueNullForBaseValue(tblProdItemMakeBrandExtTO.CatLogNo);
            return cmdInsert.ExecuteNonQuery();
        }
        #endregion
        
        #region Updation
        public  int UpdateTblProdItemMakeBrandExt(TblProdItemMakeBrandExtTO tblProdItemMakeBrandExtTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblProdItemMakeBrandExtTO, cmdUpdate);
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

        public  int UpdateTblProdItemMakeBrandExt(TblProdItemMakeBrandExtTO tblProdItemMakeBrandExtTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblProdItemMakeBrandExtTO, cmdUpdate);
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

        public  int ExecuteUpdationCommand(TblProdItemMakeBrandExtTO tblProdItemMakeBrandExtTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblProdItemMakeBrandExt] SET " + 
            "  [createdOn] = @CreatedOn" +           
            " ,[prodItemId]= @ProdItemId" +
            " ,[itemMakeId]= @ItemMakeId" +
            " ,[itemBrandId]= @ItemBrandId" +
            " ,[isDefaultMake]= @IsDefaultMake" +
            " ,[createdBy]= @CreatedBy" +
            " ,[rackNo]= @RackNo" +
            " ,[xBinLocation]= @XBinLocation" +
            " ,[yBinLocation]= @YBinLocation" +
            " ,[catLogNo] = @CatLogNo" +
            " WHERE 1 = 1 "; 

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblProdItemMakeBrandExtTO.CreatedOn;
            //cmdUpdate.Parameters.Add("@IdProdItemMakeBrandExt", System.Data.SqlDbType.Int).Value = tblProdItemMakeBrandExtTO.IdProdItemMakeBrandExt;
            cmdUpdate.Parameters.Add("@ProdItemId", System.Data.SqlDbType.Int).Value = tblProdItemMakeBrandExtTO.ProdItemId;
            cmdUpdate.Parameters.Add("@ItemMakeId", System.Data.SqlDbType.Int).Value = tblProdItemMakeBrandExtTO.ItemMakeId;
            cmdUpdate.Parameters.Add("@ItemBrandId", System.Data.SqlDbType.Int).Value = tblProdItemMakeBrandExtTO.ItemBrandId;
            cmdUpdate.Parameters.Add("@IsDefaultMake", System.Data.SqlDbType.Int).Value = tblProdItemMakeBrandExtTO.IsDefaultMake;
            cmdUpdate.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblProdItemMakeBrandExtTO.CreatedBy;
            cmdUpdate.Parameters.Add("@RackNo", System.Data.SqlDbType.NVarChar).Value = tblProdItemMakeBrandExtTO.RackNo;
            cmdUpdate.Parameters.Add("@XBinLocation", System.Data.SqlDbType.NVarChar).Value = tblProdItemMakeBrandExtTO.XBinLocation;
            cmdUpdate.Parameters.Add("@YBinLocation", System.Data.SqlDbType.NVarChar).Value = tblProdItemMakeBrandExtTO.YBinLocation;
            cmdUpdate.Parameters.Add("@CatLogNo", System.Data.SqlDbType.NVarChar).Value = tblProdItemMakeBrandExtTO.CatLogNo;
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion
        
        #region Deletion
        public  int DeleteTblProdItemMakeBrandExt(int idProdItemMakeBrandExt)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idProdItemMakeBrandExt, cmdDelete);
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

        public  int DeleteTblProdItemMakeBrandExt(int idProdItemMakeBrandExt, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idProdItemMakeBrandExt, cmdDelete);
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

        public  int ExecuteDeletionCommand(int idProdItemMakeBrandExt, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tblProdItemMakeBrandExt] " +
            " WHERE idProdItemMakeBrandExt = " + idProdItemMakeBrandExt +"";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idProdItemMakeBrandExt", System.Data.SqlDbType.Int).Value = tblProdItemMakeBrandExtTO.IdProdItemMakeBrandExt;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion
        
    }
}
