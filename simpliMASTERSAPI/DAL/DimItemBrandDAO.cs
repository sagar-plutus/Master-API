using ODLMWebAPI.BL.Interfaces;
using simpliMASTERSAPI.DAL.Interfaces;
using simpliMASTERSAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ODLMWebAPI.StaticStuff;
using System.Data.SqlClient;
using ODLMWebAPI.Models;
using System.Data;

namespace simpliMASTERSAPI.DAL
{
    public class DimItemBrandDAO : IDimItemBrandDAO
    {
        private readonly IConnectionString _iConnectionString;
        public DimItemBrandDAO(IConnectionString iConnectionString)
        {
            _iConnectionString = iConnectionString;
        }

        #region Insertion
        public List<DropDownTO> CheckBrandExistsOrNot(String itemBrandDesc, Int32 itemMakeId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = null;
            SqlDataReader dateReader = null;
            try
            {
                conn.Open();
                String sqlQuery = "SELECT * FROM dimItemBrand WHERE itemBrandDesc= @ItemBrandDesc AND itemMakeId  = @ItemMakeId AND isActive = 1";
                cmdSelect = new SqlCommand(sqlQuery, conn);
                cmdSelect.Parameters.Add("@ItemBrandDesc", System.Data.SqlDbType.NVarChar).Value = itemBrandDesc;
                cmdSelect.Parameters.Add("@ItemMakeId", System.Data.SqlDbType.Int).Value = itemMakeId;
                dateReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DropDownTO> dropDownTOList = new List<DropDownTO>();
                while (dateReader.Read())
                {
                
                    DropDownTO dropDownTONew = new DropDownTO();
                    if (dateReader["idItemBrand"] != DBNull.Value)
                        dropDownTONew.Value = Convert.ToInt32(dateReader["idItemBrand"].ToString());
                    if (dateReader["itemBrandDesc"] != DBNull.Value)
                        dropDownTONew.Text = Convert.ToString(dateReader["itemBrandDesc"].ToString());
                    if (dateReader["itemMakeId"] != DBNull.Value)
                        dropDownTONew.Tag = Convert.ToString(dateReader["itemMakeId"].ToString());

                    dropDownTOList.Add(dropDownTONew);
                }

                return dropDownTOList;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                dateReader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }
        public int InsertDimItemBrand(DimItemBrandTO dimItemBrandTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteInsertCommand(dimItemBrandTO, cmdUpdate);
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

        public int ExecuteInsertCommand(DimItemBrandTO dimItemBrandTO, SqlCommand cmdInsert)
        {
           
            String sqlQuery = @" INSERT INTO [dimItemBrand]( " +

                                "  [itemBrandDesc]" +
                                "  ,[itemMakeId]" +
                                " ,[isActive]" +
                                " )" +
                    " VALUES (" +

                                "  @ItemBrandDesc " +
                                " ,@ItemMakeId " +
                                " ,@IsActive " +

                                " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            //cmdInsert.Parameters.Add("@IdItemBrand", System.Data.SqlDbType.Int).Value = dimItemBrandTO.idItemBrand;
            cmdInsert.Parameters.Add("@ItemBrandDesc", System.Data.SqlDbType.NVarChar).Value = dimItemBrandTO.ItemBrandDesc;
            cmdInsert.Parameters.Add("@ItemMakeId", System.Data.SqlDbType.Int).Value = dimItemBrandTO.ItemMakeId;
            cmdInsert.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = dimItemBrandTO.IsActive;

            if (cmdInsert.ExecuteNonQuery() == 1)
            {
                cmdInsert.CommandText = Constants.IdentityColumnQuery;
                dimItemBrandTO.IdItemBrand = Convert.ToInt32(cmdInsert.ExecuteScalar());
                return 1;
            }
            else return 0;
        }


        public List<DropDownTO> CheckProcessExistsOrNot(String ProcessName)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = null;
            SqlDataReader dateReader = null;
            try
            {
                conn.Open();
                String sqlQuery = "SELECT * FROM dimProcessMaster WHERE ProcessName= @ProcessName AND isActive = 1";
                cmdSelect = new SqlCommand(sqlQuery, conn);
                cmdSelect.Parameters.Add("@ProcessName", System.Data.SqlDbType.NVarChar).Value = ProcessName;               
                dateReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DropDownTO> dropDownTOList = new List<DropDownTO>();
                while (dateReader.Read())
                {

                    DropDownTO dropDownTONew = new DropDownTO();
                  
                    if (dateReader["ProcessName"] != DBNull.Value)
                        dropDownTONew.Text = Convert.ToString(dateReader["ProcessName"].ToString());                

                    dropDownTOList.Add(dropDownTONew);
                }

                return dropDownTOList;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                dateReader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public List<DropDownTO> CheckMaterialGradeExistsOrNot(String MaterialGradeName)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = null;
            SqlDataReader dateReader = null;
            try
            {
                conn.Open();
                String sqlQuery = "SELECT * FROM dimMaterialGrade WHERE MaterialGradeName= @MaterialGradeName AND isActive = 1";
                cmdSelect = new SqlCommand(sqlQuery, conn);
                cmdSelect.Parameters.Add("@MaterialGradeName", System.Data.SqlDbType.NVarChar).Value = MaterialGradeName;
                dateReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DropDownTO> dropDownTOList = new List<DropDownTO>();
                while (dateReader.Read())
                {

                    DropDownTO dropDownTONew = new DropDownTO();

                    if (dateReader["MaterialGradeName"] != DBNull.Value)
                        dropDownTONew.Text = Convert.ToString(dateReader["MaterialGradeName"].ToString());

                    dropDownTOList.Add(dropDownTONew);
                }

                return dropDownTOList;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                dateReader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public List<DropDownTO> CheckCategoryExistsOrNot(String CategoryName)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = null;
            SqlDataReader dateReader = null;
            try
            {
                conn.Open();
                String sqlQuery = "SELECT * FROM dimCategory WHERE CategoryName= @CategoryName AND isActive = 1";
                cmdSelect = new SqlCommand(sqlQuery, conn);
                cmdSelect.Parameters.Add("@CategoryName", System.Data.SqlDbType.NVarChar).Value = CategoryName;
                dateReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DropDownTO> dropDownTOList = new List<DropDownTO>();
                while (dateReader.Read())
                {

                    DropDownTO dropDownTONew = new DropDownTO();

                    if (dateReader["CategoryName"] != DBNull.Value)
                        dropDownTONew.Text = Convert.ToString(dateReader["CategoryName"].ToString());

                    dropDownTOList.Add(dropDownTONew);
                }

                return dropDownTOList;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                dateReader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }
        public int InsertDimProcess(DimProcessMasterTO dimProcessMasterTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteInsertCommandForProcessMaster(dimProcessMasterTO, cmdUpdate);
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
        public int InsertDimMaterialGrade(DimMaterialGradeTO dimMaterialGradeTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteInsertCommandForMaterialGrade(dimMaterialGradeTO, cmdUpdate);
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
        public int InsertDimCategory(DimCategoryTO dimCategoryTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteInsertCommandForCategory(dimCategoryTO, cmdUpdate);
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

        public int ExecuteInsertCommandForProcessMaster(DimProcessMasterTO dimProcessMasterTO, SqlCommand cmdInsert)
        {

            String sqlQuery = @" INSERT INTO [dimProcessMaster]( " +

                                "  [ProcessName]" +                              
                                " ,[isActive]" +
                                " )" +
                    " VALUES (" +

                                "  @ProcessName " +                              
                                " ,@IsActive " +

                                " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;           
            cmdInsert.Parameters.Add("@ProcessName", System.Data.SqlDbType.NVarChar).Value = dimProcessMasterTO.ProcessName;           
            cmdInsert.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = dimProcessMasterTO.IsActive;

            if (cmdInsert.ExecuteNonQuery() == 1)
            {
                cmdInsert.CommandText = Constants.IdentityColumnQuery;
                dimProcessMasterTO.IdProcess= Convert.ToInt32(cmdInsert.ExecuteScalar());
                return 1;
            }
            else return 0;
        }
        public int ExecuteInsertCommandForMaterialGrade(DimMaterialGradeTO dimMaterialGradeTO, SqlCommand cmdInsert)
        {

            String sqlQuery = @" INSERT INTO [dimMaterialGrade]( " +

                                "  [MaterialGradeName]" +
                                " ,[isActive]" +
                                " )" +
                    " VALUES (" +

                                "  @MaterialGradeName " +
                                " ,@IsActive " +

                                " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;
            cmdInsert.Parameters.Add("@MaterialGradeName", System.Data.SqlDbType.NVarChar).Value = dimMaterialGradeTO.MaterialGradeName;
            cmdInsert.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = dimMaterialGradeTO.IsActive;

            if (cmdInsert.ExecuteNonQuery() == 1)
            {
                cmdInsert.CommandText = Constants.IdentityColumnQuery;
                dimMaterialGradeTO.IdMaterialGrade = Convert.ToInt32(cmdInsert.ExecuteScalar());
                return 1;
            }
            else return 0;
        }
        public int ExecuteInsertCommandForCategory(DimCategoryTO dimCategoryTO, SqlCommand cmdInsert)
        {

            String sqlQuery = @" INSERT INTO [dimCategory]( " +

                                "  [CategoryName]" +
                                " ,[isActive]" +
                                " )" +
                    " VALUES (" +

                                "  @CategoryName " +
                                " ,@IsActive " +

                                " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;
            cmdInsert.Parameters.Add("@CategoryName", System.Data.SqlDbType.NVarChar).Value = dimCategoryTO.CategoryName;
            cmdInsert.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = dimCategoryTO.IsActive;

            if (cmdInsert.ExecuteNonQuery() == 1)
            {
                cmdInsert.CommandText = Constants.IdentityColumnQuery;
                dimCategoryTO.IdCategory = Convert.ToInt32(cmdInsert.ExecuteScalar());
                return 1;
            }
            else return 0;
        }
        public List<DimProcessMasterTO> SelectDimProcessType()
        {

            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = null;
            String sqlQuery = string.Empty;
            try
            {

                conn.Open();

                sqlQuery = "SELECT * FROM dimProcessMaster where isActive=1 ";


                cmdSelect = new SqlCommand(sqlQuery, conn);
                SqlDataReader dateReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DimProcessMasterTO> DimProcessTypeTOList = new List<DimProcessMasterTO>();
                while (dateReader.Read())
                {
                    DimProcessMasterTO DimProcessTypeTONew = new DimProcessMasterTO();

                    if (dateReader["idProcess"] != DBNull.Value)
                        DimProcessTypeTONew.IdProcess = Convert.ToInt32(dateReader["idProcess"].ToString());
                 
                    if (dateReader["ProcessName"] != DBNull.Value)
                        DimProcessTypeTONew.ProcessName = Convert.ToString(dateReader["ProcessName"].ToString());

                    if (dateReader["isActive"] != DBNull.Value)
                        DimProcessTypeTONew.IsActive = Convert.ToInt32(dateReader["isActive"].ToString());
                    DimProcessTypeTOList.Add(DimProcessTypeTONew);


                }

                if (dateReader != null)
                    dateReader.Dispose();

                return DimProcessTypeTOList;
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
        public List<DimCategoryTO> SelectDimCategory()
        {

            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = null;
            String sqlQuery = string.Empty;
            try
            {

                conn.Open();

                sqlQuery = "SELECT * FROM dimCategory where isActive=1 ";


                cmdSelect = new SqlCommand(sqlQuery, conn);
                SqlDataReader dateReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DimCategoryTO> DimCategoryTOList = new List<DimCategoryTO>();
                while (dateReader.Read())
                {
                    DimCategoryTO DimCategoryTONew = new DimCategoryTO();

                    if (dateReader["idCategory"] != DBNull.Value)
                        DimCategoryTONew.IdCategory = Convert.ToInt32(dateReader["idCategory"].ToString());

                    if (dateReader["CategoryName"] != DBNull.Value)
                        DimCategoryTONew.CategoryName = Convert.ToString(dateReader["CategoryName"].ToString());

                    if (dateReader["isActive"] != DBNull.Value)
                        DimCategoryTONew.IsActive = Convert.ToInt32(dateReader["isActive"].ToString());
                    DimCategoryTOList.Add(DimCategoryTONew);


                }

                if (dateReader != null)
                    dateReader.Dispose();

                return DimCategoryTOList;
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
        public List<DimMaterialGradeTO> SelectDimMaterialGrade()
        {

            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = null;
            String sqlQuery = string.Empty;
            try
            {

                conn.Open();

                sqlQuery = "SELECT * FROM dimMaterialGrade where isActive=1 ";


                cmdSelect = new SqlCommand(sqlQuery, conn);
                SqlDataReader dateReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DimMaterialGradeTO> DimMaterialGradeTOList = new List<DimMaterialGradeTO>();
                while (dateReader.Read())
                {
                    DimMaterialGradeTO DimMaterialGradeTONew = new DimMaterialGradeTO();

                    if (dateReader["idMaterialGrade"] != DBNull.Value)
                        DimMaterialGradeTONew.IdMaterialGrade = Convert.ToInt32(dateReader["idMaterialGrade"].ToString());

                    if (dateReader["MaterialGradeName"] != DBNull.Value)
                        DimMaterialGradeTONew.MaterialGradeName = Convert.ToString(dateReader["MaterialGradeName"].ToString());

                    if (dateReader["isActive"] != DBNull.Value)
                        DimMaterialGradeTONew.IsActive = Convert.ToInt32(dateReader["isActive"].ToString());
                    DimMaterialGradeTOList.Add(DimMaterialGradeTONew);


                }

                if (dateReader != null)
                    dateReader.Dispose();

                return DimMaterialGradeTOList;
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
        #endregion
    }
}
