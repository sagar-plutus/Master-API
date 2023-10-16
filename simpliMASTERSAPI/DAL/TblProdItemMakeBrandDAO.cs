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
    public class TblProdItemMakeBrandDAO : ITblProdItemMakeBrandDAO
    {
        private readonly IConnectionString _iConnectionString;
        public TblProdItemMakeBrandDAO(IConnectionString iConnectionString)
        {
            _iConnectionString = iConnectionString;
        }

        #region Methods
        public String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT * FROM [tblProdItemMakeBrand]"; 
            return sqlSelectQry;
        }

        public String SqlSelectQuery1()
        {
            String sqlSelectQry = " SELECT tblProdItemMakeBrand.brandId,dimItemBrand.itemBrandDesc,isDefaultMake"+
                                  " FROM tblProdItemMakeBrand tblProdItemMakeBrand"+
                                  " LEFT JOIN dimItemBrand dimItemBrand ON dimItemBrand.idItemBrand = tblProdItemMakeBrand.brandId";
            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public List<TblProdItemMakeBrandTO> SelectAllTblProdItemMakeBrand()
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

                //cmdSelect.Parameters.Add("@idProdMakeBrand", System.Data.SqlDbType.Int).Value = tblProdItemMakeBrandTO.IdProdMakeBrand;
                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblProdItemMakeBrandTO> list = ConvertDTToList(reader);
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

        public List<TblProdItemMakeBrandTO> SelectProdItemMakeBrandByProdItemBrandId(TblProdItemMakeBrandTO tblProdItemMakeBrandTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE prodItemId = " + tblProdItemMakeBrandTO.ProdItemId + " AND brandId = " + tblProdItemMakeBrandTO.BrandId + " AND createdBy = " + tblProdItemMakeBrandTO.CreatedBy + "";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                cmdSelect.Parameters.Add("@ProdItemId", System.Data.SqlDbType.Int).Value = tblProdItemMakeBrandTO.ProdItemId;
                cmdSelect.Parameters.Add("@BrandId", System.Data.SqlDbType.Int).Value = tblProdItemMakeBrandTO.BrandId;                
                cmdSelect.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblProdItemMakeBrandTO.CreatedBy;                
                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblProdItemMakeBrandTO> list = ConvertDTToList(reader);
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

        public List<TblProdItemMakeBrandTO> SelectTblProdItemMakeBrand(Int32 idProdMakeBrand)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery()+ " WHERE idProdMakeBrand = " + idProdMakeBrand +" ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idProdMakeBrand", System.Data.SqlDbType.Int).Value = tblProdItemMakeBrandTO.IdProdMakeBrand;
                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblProdItemMakeBrandTO> list = ConvertDTToList(reader);
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

        public List<TblProdItemMakeBrandTO> SelectedProdItemMakeBrand(Int32 prodItemId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery1() + " WHERE prodItemId = " + prodItemId + " ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idProdMakeBrand", System.Data.SqlDbType.Int).Value = tblProdItemMakeBrandTO.IdProdMakeBrand;
                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblProdItemMakeBrandTO> list = ConvertDTToListForSelected(reader);
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

        public List<TblProdItemMakeBrandTO> SelectAllTblProdItemMakeBrand(SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                cmdSelect.CommandText = SqlSelectQuery();
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idProdMakeBrand", System.Data.SqlDbType.Int).Value = tblProdItemMakeBrandTO.IdProdMakeBrand;
                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblProdItemMakeBrandTO> list = ConvertDTToList(reader);
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

        public List<TblProdItemMakeBrandTO> ConvertDTToListForSelected(SqlDataReader tblProdItemMakeBrandTODT)
        {
            List<TblProdItemMakeBrandTO> tblProdItemMakeBrandTOList = new List<TblProdItemMakeBrandTO>();
            if (tblProdItemMakeBrandTODT != null)
            {
                while (tblProdItemMakeBrandTODT.Read())
                {
                    TblProdItemMakeBrandTO tblProdItemMakeBrandTONew = new TblProdItemMakeBrandTO();
                    //if (tblProdItemMakeBrandTODT["idProdMakeBrand"] != DBNull.Value)
                    //    tblProdItemMakeBrandTONew.IdProdMakeBrand = Convert.ToInt32(tblProdItemMakeBrandTODT["idProdMakeBrand"].ToString());
                    //if (tblProdItemMakeBrandTODT["prodItemId"] != DBNull.Value)
                    //    tblProdItemMakeBrandTONew.ProdItemId = Convert.ToInt32(tblProdItemMakeBrandTODT["prodItemId"].ToString());
                    if (tblProdItemMakeBrandTODT["brandId"] != DBNull.Value)
                        tblProdItemMakeBrandTONew.BrandId = Convert.ToInt32(tblProdItemMakeBrandTODT["brandId"].ToString());
                    if (tblProdItemMakeBrandTODT["itemBrandDesc"] != DBNull.Value)
                        tblProdItemMakeBrandTONew.ItemBrandDesc = Convert.ToString(tblProdItemMakeBrandTODT["itemBrandDesc"].ToString());
                    if (tblProdItemMakeBrandTODT["isDefaultMake"] != DBNull.Value)
                        tblProdItemMakeBrandTONew.IsDefaultMake = Convert.ToInt32(tblProdItemMakeBrandTODT["isDefaultMake"].ToString());
                    //if (tblProdItemMakeBrandTODT["createdBy"] != DBNull.Value)
                    //    tblProdItemMakeBrandTONew.CreatedBy = Convert.ToInt32(tblProdItemMakeBrandTODT["createdBy"].ToString());
                    //if (tblProdItemMakeBrandTODT["createdOn"] != DBNull.Value)
                    //    tblProdItemMakeBrandTONew.CreatedOn = Convert.ToDateTime(tblProdItemMakeBrandTODT["createdOn"].ToString());
                    tblProdItemMakeBrandTOList.Add(tblProdItemMakeBrandTONew);
                }
            }
            return tblProdItemMakeBrandTOList;
        }

        public List<TblProdItemMakeBrandTO> ConvertDTToList(SqlDataReader tblProdItemMakeBrandTODT)
        {
            List<TblProdItemMakeBrandTO> tblProdItemMakeBrandTOList = new List<TblProdItemMakeBrandTO>();
            if (tblProdItemMakeBrandTODT != null)
            {
                while (tblProdItemMakeBrandTODT.Read())
                {
                    TblProdItemMakeBrandTO tblProdItemMakeBrandTONew = new TblProdItemMakeBrandTO();
                    if (tblProdItemMakeBrandTODT["idProdMakeBrand"] != DBNull.Value)
                        tblProdItemMakeBrandTONew.IdProdMakeBrand = Convert.ToInt32(tblProdItemMakeBrandTODT["idProdMakeBrand"].ToString());
                    if (tblProdItemMakeBrandTODT["prodItemId"] != DBNull.Value)
                        tblProdItemMakeBrandTONew.ProdItemId = Convert.ToInt32(tblProdItemMakeBrandTODT["prodItemId"].ToString());
                    if (tblProdItemMakeBrandTODT["brandId"] != DBNull.Value)
                        tblProdItemMakeBrandTONew.BrandId = Convert.ToInt32(tblProdItemMakeBrandTODT["brandId"].ToString());
                    if (tblProdItemMakeBrandTODT["isDefaultMake"] != DBNull.Value)
                        tblProdItemMakeBrandTONew.IsDefaultMake = Convert.ToInt32(tblProdItemMakeBrandTODT["isDefaultMake"].ToString());
                    if (tblProdItemMakeBrandTODT["createdBy"] != DBNull.Value)
                        tblProdItemMakeBrandTONew.CreatedBy = Convert.ToInt32(tblProdItemMakeBrandTODT["createdBy"].ToString());
                    if (tblProdItemMakeBrandTODT["createdOn"] != DBNull.Value)
                        tblProdItemMakeBrandTONew.CreatedOn = Convert.ToDateTime(tblProdItemMakeBrandTODT["createdOn"].ToString());
                    tblProdItemMakeBrandTOList.Add(tblProdItemMakeBrandTONew);
                }
            }
            return tblProdItemMakeBrandTOList;
        }

        #endregion

        #region Insertion
        public int InsertTblProdItemMakeBrand(TblProdItemMakeBrandTO tblProdItemMakeBrandTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblProdItemMakeBrandTO, cmdInsert);
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

        public int InsertTblProdItemMakeBrand(TblProdItemMakeBrandTO tblProdItemMakeBrandTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblProdItemMakeBrandTO, cmdInsert);
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

        public int ExecuteInsertionCommand(TblProdItemMakeBrandTO tblProdItemMakeBrandTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblProdItemMakeBrand]( " + 
            //"  [idProdMakeBrand]" +
            "  [prodItemId]" +
            " ,[brandId]" +
            " ,[isDefaultMake]" +
            " ,[createdBy]" +
            " ,[createdOn]" +
            " )" +
" VALUES (" +
            //"  @IdProdMakeBrand " +
            "  @ProdItemId " +
            " ,@BrandId " +
            " ,@IsDefaultMake " +
            " ,@CreatedBy " +
            " ,@CreatedOn " + 
            " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            //cmdInsert.Parameters.Add("@IdProdMakeBrand", System.Data.SqlDbType.Int).Value = tblProdItemMakeBrandTO.IdProdMakeBrand;
            cmdInsert.Parameters.Add("@ProdItemId", System.Data.SqlDbType.Int).Value = tblProdItemMakeBrandTO.ProdItemId;
            cmdInsert.Parameters.Add("@BrandId", System.Data.SqlDbType.Int).Value = tblProdItemMakeBrandTO.BrandId;
            cmdInsert.Parameters.Add("@IsDefaultMake", System.Data.SqlDbType.Int).Value = tblProdItemMakeBrandTO.IsDefaultMake;
            cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblProdItemMakeBrandTO.CreatedBy;
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblProdItemMakeBrandTO.CreatedOn;
            return cmdInsert.ExecuteNonQuery();
        }
        #endregion
        
        #region Updation
        public int UpdateTblProdItemMakeBrand(TblProdItemMakeBrandTO tblProdItemMakeBrandTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);            
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblProdItemMakeBrandTO, cmdUpdate);
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

        public int UpdateTblProdItemMakeBrand(TblProdItemMakeBrandTO tblProdItemMakeBrandTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblProdItemMakeBrandTO, cmdUpdate);
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

        public int ExecuteUpdationCommand(TblProdItemMakeBrandTO tblProdItemMakeBrandTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblProdItemMakeBrand] SET " +
            //"  [idProdMakeBrand] = @IdProdMakeBrand" +
            //" ,[prodItemId]= @ProdItemId" +
            " ,[brandId]= @BrandId" +
            " ,[isDefaultMake]= @IsDefaultMake" +
            " ,[createdBy]= @CreatedBy" +
            " ,[createdOn] = @CreatedOn" +
            "   WHERE [prodItemId] = @ProdItemId " +
            "  AND [brandId] = @BrandId";

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            //cmdUpdate.Parameters.Add("@IdProdMakeBrand", System.Data.SqlDbType.Int).Value = tblProdItemMakeBrandTO.IdProdMakeBrand;
            cmdUpdate.Parameters.Add("@ProdItemId", System.Data.SqlDbType.Int).Value = tblProdItemMakeBrandTO.ProdItemId;
            cmdUpdate.Parameters.Add("@BrandId", System.Data.SqlDbType.Int).Value = tblProdItemMakeBrandTO.BrandId;
            cmdUpdate.Parameters.Add("@IsDefaultMake", System.Data.SqlDbType.Int).Value = tblProdItemMakeBrandTO.IsDefaultMake;
            cmdUpdate.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblProdItemMakeBrandTO.CreatedBy;
            cmdUpdate.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblProdItemMakeBrandTO.CreatedOn;
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion
        
        #region Deletion
        public int DeleteTblProdItemMakeBrand(Int32 idProdMakeBrand)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idProdMakeBrand, cmdDelete);
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

        public int DeleteTblProdItemMakeBrand(Int32 idProdMakeBrand, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idProdMakeBrand, cmdDelete);
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

        public int ExecuteDeletionCommand(Int32 idProdMakeBrand, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tblProdItemMakeBrand] " +
            " WHERE idProdMakeBrand = " + idProdMakeBrand +"";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idProdMakeBrand", System.Data.SqlDbType.Int).Value = tblProdItemMakeBrandTO.IdProdMakeBrand;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion
        
    }
}
