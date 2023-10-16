using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using ODLMWebAPI.Models;
using ODLMWebAPI.StaticStuff;
using ODLMWebAPI.DAL.Interfaces;
using ODLMWebAPI.BL.Interfaces;
using simpliMASTERSAPI.MessageQueuePayloads;
using AutoMapper;
using rabbitMessaging;

namespace ODLMWebAPI.DAL
{
    public class DimMstDesignationDAO : IDimMstDesignationDAO
    {
        private readonly IConnectionString _iConnectionString;

        public DimMstDesignationDAO(IConnectionString iConnectionString)
        {
           
            _iConnectionString = iConnectionString;
        }
        #region Methods
        public String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT * FROM [dimMstDesignation]"; 
            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public List<DimMstDesignationTO> SelectAllDimMstDesignation()
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            String sqlQuery = String.Empty;
            try
            {
                conn.Open();
                sqlQuery = "SELECT * FROM [dimMstDesignation] WHERE  isVisible=1";
                cmdSelect.CommandText = sqlQuery;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                //cmdSelect.Parameters.Add("@idDesignation", System.Data.SqlDbType.Int).Value = dimMstDesignationTO.IdDesignation;
                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DimMstDesignationTO> list = ConvertDTToList(reader);

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

        public DimMstDesignationTO SelectDimMstDesignation(Int32 idDesignation)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader reader = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE idDesignation = " + idDesignation + " ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                reader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DimMstDesignationTO> list = ConvertDTToList(reader);
                if (list != null && list.Count == 1)
                    return list[0];

                return null;
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






        // Vaibhav [25-Sep-2017] added to get all designations for drop down
        public List<DropDownTO> SelectAllDesignationForDropDownList()
        {

            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = CommandType.Text;
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE isVisible = 1 ";

                conn.Open();
                SqlDataReader departmentTODT = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DropDownTO> dropDownTOList = new List<DropDownTO>();

                if (departmentTODT != null)
                {
                    while (departmentTODT.Read())
                    {
                        DropDownTO dropDownTO = new DropDownTO();
                        if (departmentTODT["idDesignation"] != DBNull.Value)
                            dropDownTO.Value = Convert.ToInt32(departmentTODT["idDesignation"].ToString());
                        if (departmentTODT["designationDesc"] != DBNull.Value)
                            dropDownTO.Text = Convert.ToString(departmentTODT["designationDesc"].ToString());
                        dropDownTOList.Add(dropDownTO);
                    }
                }
                return dropDownTOList;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "SelectAllDesignationForDropDown");
                return null;
            }
            finally
            {
                conn.Close();
            }
        }

        public DropDownTO SelectDesignationOnOrgId(Int32 orgId)
        {

            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = CommandType.Text;
                cmdSelect.CommandText =
                    "select desg.idDesignation,desg.designationDesc " +
                    " from dimMstDesignation desg inner join tblOrgStructure org ON  " +
                    " org.designationId = desg.idDesignation " +
                     " WHERE org.idOrgStructure =  "+orgId;

                conn.Open();
                SqlDataReader departmentTODT = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DropDownTO> dropDownTOList = new List<DropDownTO>();

                if (departmentTODT != null)
                {
                    while (departmentTODT.Read())
                    {
                        DropDownTO dropDownTO = new DropDownTO();
                        if (departmentTODT["idDesignation"] != DBNull.Value)
                            dropDownTO.Value = Convert.ToInt32(departmentTODT["idDesignation"].ToString());
                        if (departmentTODT["designationDesc"] != DBNull.Value)
                            dropDownTO.Text = Convert.ToString(departmentTODT["designationDesc"].ToString());
                        dropDownTOList.Add(dropDownTO);
                    }

                }
                if (dropDownTOList == null)
                    return null;
                else
                    return dropDownTOList[0];
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "SelectAllDesignationForDropDown");
                return null;
            }
            finally
            {
                conn.Close();
            }
        }

        //Harshala[14/09/2020] added to avoid duplicate entry for Designation
        public Boolean IsDuplicateDesignationFound(String desgName)
        {

            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = CommandType.Text;
                cmdSelect.CommandText =
                    "select Count(*) AS duplicateDesgnationCount FROM dimmstDesignation WHERE isVisible=1 and designationDesc ='" + desgName +"'";
              
                conn.Open();
                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                int count = 0;
                if (sqlReader != null)
                {
                    while (sqlReader.Read())
                    {
                        if (sqlReader["duplicateDesgnationCount"] != DBNull.Value)
                            count = Convert.ToInt32(sqlReader["duplicateDesgnationCount"].ToString());
                    }

                }
                if (count>0)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "IsDuplicateDesignationFound");
                return false;
            }
            finally
            {
                conn.Close();
            }
        }

        public List<DimMstDesignationTO> ConvertDTToList(SqlDataReader dimMstDesignationTODT)
        {

            List<DimMstDesignationTO> dimMstDesignationTOList = new List<DimMstDesignationTO>();
            if (dimMstDesignationTODT != null)
            {
                while (dimMstDesignationTODT.Read())
                {
                    DimMstDesignationTO dimMstDesignationTONew = new DimMstDesignationTO();
                    if (dimMstDesignationTODT["idDesignation"] != DBNull.Value)
                        dimMstDesignationTONew.IdDesignation = Convert.ToInt32(dimMstDesignationTODT["idDesignation"].ToString());
                    if (dimMstDesignationTODT["createdBy"] != DBNull.Value)
                        dimMstDesignationTONew.CreatedBy = Convert.ToInt32(dimMstDesignationTODT["createdBy"].ToString());
                    if (dimMstDesignationTODT["updatedBy"] != DBNull.Value)
                        dimMstDesignationTONew.UpdatedBy = Convert.ToInt32(dimMstDesignationTODT["updatedBy"].ToString());
                    if (dimMstDesignationTODT["noticePeriodInMonth"] != DBNull.Value)
                        dimMstDesignationTONew.NoticePeriodInMonth = Convert.ToInt32(dimMstDesignationTODT["noticePeriodInMonth"].ToString());
                    if (dimMstDesignationTODT["isVisible"] != DBNull.Value)
                        dimMstDesignationTONew.IsVisible = Convert.ToInt32(dimMstDesignationTODT["isVisible"].ToString());
                    if (dimMstDesignationTODT["createdOn"] != DBNull.Value)
                        dimMstDesignationTONew.CreatedOn = Convert.ToDateTime(dimMstDesignationTODT["createdOn"].ToString());
                    if (dimMstDesignationTODT["updatedOn"] != DBNull.Value)
                        dimMstDesignationTONew.UpdatedOn = Convert.ToDateTime(dimMstDesignationTODT["updatedOn"].ToString());
                    if (dimMstDesignationTODT["designationDesc"] != DBNull.Value)
                        dimMstDesignationTONew.DesignationDesc = Convert.ToString(dimMstDesignationTODT["designationDesc"].ToString());
                    if (dimMstDesignationTODT["remark"] != DBNull.Value)
                        dimMstDesignationTONew.Remark = Convert.ToString(dimMstDesignationTODT["remark"].ToString());
                    if (dimMstDesignationTODT["deactivatedBy"] != DBNull.Value)
                        dimMstDesignationTONew.DeactivatedBy = Convert.ToInt32(dimMstDesignationTODT["deactivatedBy"].ToString());
                    if (dimMstDesignationTODT["deactivatedOn"] != DBNull.Value)
                        dimMstDesignationTONew.DeactivatedOn = Convert.ToDateTime(dimMstDesignationTODT["deactivatedOn"].ToString());
                    dimMstDesignationTOList.Add(dimMstDesignationTONew);
                }
                  
                
            }
            return dimMstDesignationTOList;
        }
     
        

        #endregion
        
        #region Insertion
        public int InsertDimMstDesignation(DimMstDesignationTO dimMstDesignationTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                int result= ExecuteInsertionCommand(dimMstDesignationTO, cmdInsert);
                
                return result;

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

        public int InsertDimMstDesignation(DimMstDesignationTO dimMstDesignationTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                int result = ExecuteInsertionCommand(dimMstDesignationTO, cmdInsert);
               
                return result; 
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

        public int ExecuteInsertionCommand(DimMstDesignationTO dimMstDesignationTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [dimMstDesignation]( " + 
            //"  [idDesignation]" +
            " [createdBy]" +
            " ,[updatedBy]" +
             ",[deactivatedBy]" +
            " ,[noticePeriodInMonth]" +
            " ,[isVisible]" +
            " ,[createdOn]" +
            " ,[updatedOn]" +
            " ,[deactivatedOn]" +
            " ,[designationDesc]" +
            " ,[remark]" +
            " )" +
" VALUES (" +
            //"  @IdDesignation " +
            " @CreatedBy " +
            " ,@UpdatedBy " +
            " ,@DeactivatedBy " +
            " ,@NoticePeriodInMonth " +
            " ,@IsVisible " +
            " ,@CreatedOn " +
            " ,@UpdatedOn " +
            " ,@DeactivatedOn " +
            " ,@DesignationDesc " +
            " ,@Remark " + 
            " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            //cmdInsert.Parameters.Add("@IdDesignation", System.Data.SqlDbType.Int).Value = dimMstDesignationTO.IdDesignation;
            cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = dimMstDesignationTO.CreatedBy;
            cmdInsert.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(dimMstDesignationTO.UpdatedBy);
            cmdInsert.Parameters.Add("@DeactivatedBy", System.Data.SqlDbType.Int).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(dimMstDesignationTO.DeactivatedBy);
            cmdInsert.Parameters.Add("@NoticePeriodInMonth", System.Data.SqlDbType.Int).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(dimMstDesignationTO.NoticePeriodInMonth); 
            cmdInsert.Parameters.Add("@IsVisible", System.Data.SqlDbType.Int).Value = dimMstDesignationTO.IsVisible;
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = dimMstDesignationTO.CreatedOn;
            cmdInsert.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(dimMstDesignationTO.UpdatedOn);
            cmdInsert.Parameters.Add("@DeactivatedOn", System.Data.SqlDbType.DateTime).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(dimMstDesignationTO.DeactivatedOn);
            cmdInsert.Parameters.Add("@DesignationDesc", System.Data.SqlDbType.NVarChar).Value = dimMstDesignationTO.DesignationDesc;
            cmdInsert.Parameters.Add("@Remark", System.Data.SqlDbType.NVarChar).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(dimMstDesignationTO.Remark);
       if (cmdInsert.ExecuteNonQuery() == 1)
            {
                cmdInsert.CommandText = Constants.IdentityColumnQuery;
                dimMstDesignationTO.IdDesignation = Convert.ToInt32(cmdInsert.ExecuteScalar());
                return 1;
            }
            else return 0;
        }
        #endregion
        
        #region Updation
        public int UpdateDimMstDesignation(DimMstDesignationTO dimMstDesignationTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                
                int result = ExecuteUpdationCommand(dimMstDesignationTO, cmdUpdate);
               
                return result;
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

        public int UpdateDimMstDesignation(DimMstDesignationTO dimMstDesignationTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                int result = ExecuteUpdationCommand(dimMstDesignationTO, cmdUpdate);
               
                return result;
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

        public int ExecuteUpdationCommand(DimMstDesignationTO dimMstDesignationTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [dimMstDesignation] SET " + 
            //"  [idDesignation] = @IdDesignation" +
            " [createdBy]= @CreatedBy" +
            " ,[updatedBy]= @UpdatedBy" +
            " ,[deactivatedBy]= @DeactivatedBy" +
            " ,[noticePeriodInMonth]= @NoticePeriodInMonth" +
            " ,[isVisible]= @IsVisible" +
            " ,[createdOn]= @CreatedOn" +
            " ,[updatedOn]= @UpdatedOn" +
            " ,[deactivatedOn]= @DeactivatedOn" +
            " ,[designationDesc]= @DesignationDesc" +
            " ,[remark] = @Remark" +
            " WHERE [idDesignation] = @IdDesignation"; 

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdDesignation", System.Data.SqlDbType.Int).Value = dimMstDesignationTO.IdDesignation;
            cmdUpdate.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = dimMstDesignationTO.CreatedBy;
            cmdUpdate.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(dimMstDesignationTO.UpdatedBy);
            cmdUpdate.Parameters.Add("@DeactivatedBy", System.Data.SqlDbType.Int).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(dimMstDesignationTO.DeactivatedBy);
            cmdUpdate.Parameters.Add("@NoticePeriodInMonth", System.Data.SqlDbType.Int).Value =  StaticStuff.Constants.GetSqlDataValueNullForBaseValue(dimMstDesignationTO.NoticePeriodInMonth);
            cmdUpdate.Parameters.Add("@IsVisible", System.Data.SqlDbType.Int).Value = dimMstDesignationTO.IsVisible;
            cmdUpdate.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = dimMstDesignationTO.CreatedOn;
            cmdUpdate.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(dimMstDesignationTO.UpdatedOn);
            cmdUpdate.Parameters.Add("@DeactivatedOn", System.Data.SqlDbType.DateTime).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue(dimMstDesignationTO.DeactivatedOn);
            cmdUpdate.Parameters.Add("@DesignationDesc", System.Data.SqlDbType.NVarChar).Value = dimMstDesignationTO.DesignationDesc;
            cmdUpdate.Parameters.Add("@Remark", System.Data.SqlDbType.NVarChar).Value = StaticStuff.Constants.GetSqlDataValueNullForBaseValue( dimMstDesignationTO.Remark);
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion
        
        #region Deletion
        public int DeleteDimMstDesignation(Int32 idDesignation)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(idDesignation, cmdDelete);
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

        public int DeleteDimMstDesignation(Int32 idDesignation, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(idDesignation, cmdDelete);
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

        public int ExecuteDeletionCommand(Int32 idDesignation, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [dimMstDesignation] " +
            " WHERE idDesignation = " + idDesignation +"";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@idDesignation", System.Data.SqlDbType.Int).Value = dimMstDesignationTO.IdDesignation;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion
        
    }
}
