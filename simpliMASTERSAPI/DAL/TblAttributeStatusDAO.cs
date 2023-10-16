
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using ODLMWebAPI.Models;
using ODLMWebAPI.StaticStuff;
using ODLMWebAPI.BL.Interfaces;
using ODLMWebAPI.DAL.Interfaces;

namespace ODLMWebAPI.DAL
{
    public class TblAttributeStatusDAO : ITblAttributeStatusDAO
    {
        private readonly IConnectionString _iConnectionString;
        public TblAttributeStatusDAO(IConnectionString iConnectionString)
        {
            _iConnectionString = iConnectionString;
        }
        #region Methods
        //Added by minal For first Priority to AttributeDisplayName in AttributeStatus
        public String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT attributes.attributeName," +
                                  " CASE WHEN attrStatus.attributeDisplayName IS NOT NULL THEN attrStatus.attributeDisplayName ELSE attributes.attributeDisplayName END AS attributeDisplayName," +
                                  " attrStatus.*," +
                                  " attributes.idAttribute AS idAttribute," +
                                  " attConfig.pageId,attConfig.isSrcIdFilter FROM tblAttributeStatus attrStatus " +
                                   " LEFT JOIN tblAttributes attributes ON attributes.idAttribute = attrStatus.attributeId " +
                                   " LEFT JOIN dimAttributeConfig attConfig ON attConfig.idAttributeConfig = attributes.attributeConfigId  ";


            return sqlSelectQry;
        }

        //Added by minal For first Priority to AttributeDisplayName in AttributeStatus
        public String SqlSelectLabelQuery()
        {
            String sqlSelectQry = "  SELECT attributes.attributeName, " +
                                   " CASE WHEN attrStatus.attributeDisplayName IS NOT NULL THEN attrStatus.attributeDisplayName ELSE attributes.attributeDisplayName END AS attributeDisplayName, " +
                                   " attrStatus.*, " +
                                   " attributes.idAttribute AS idAttribute , " +
                                   " attConfig.pageId,attConfig.isSrcIdFilter,crmLabel.lagId,crmLabel.valueLabel  FROM tblAttributeStatus attrStatus " +
                                   " LEFT JOIN tblAttributes attributes ON attributes.idAttribute = attrStatus.attributeId " +
                                   " LEFT JOIN dimAttributeConfig attConfig ON attConfig.idAttributeConfig = attributes.attributeConfigId  " +
                                   " LEFT JOIN tblCRMLabel crmLabel on attributes.idAttribute = crmLabel.attributeId";


            return sqlSelectQry;
        }


        #endregion

        #region Selection


        public List<AttributePageTO> SelectAllAttributePages()
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader rdr = null;
            try
            {
                conn.Open();

                cmdSelect.CommandText = "select attconf.*,pages.pageDesc from dimAttributeConfig attconf" +
                     " left join tblPages pages ON pages.idPage = attconf.pageId ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                rdr = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<AttributePageTO> list = ConvertDTToPageList(rdr);
                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                rdr.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        /// <summary>
        /// to get PurchaseManager List
        /// </summary>
        /// <returns></returns>
        public List<AttributeStatusTO> SelectAllAttributeStatusList(Int32 pageId, int orgTypeId = 0)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader rdr = null;
            try
            {
                conn.Open();

                cmdSelect.CommandText = SqlSelectQuery() + " where attrStatus.isActive= 1 and attConfig.pageId = " + pageId + " ";

                if (orgTypeId > 0)
                {
                    cmdSelect.CommandText += " and attrStatus.orgTypeId= " + orgTypeId + " ";
                }

                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                rdr = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<AttributeStatusTO> list = ConvertDTToList(rdr);
                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                rdr.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }

        }

        public List<AttributeStatusTO> SelectAllAttributeStatusLabelList(Int32 pageId, int orgTypeId = 0)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader rdr = null;
            try
            {
                conn.Open();

                cmdSelect.CommandText = SqlSelectLabelQuery() + " where attrStatus.isActive= 1 and attConfig.pageId = " + pageId + " ";

                if (orgTypeId > 0)
                {
                    cmdSelect.CommandText += " and attrStatus.orgTypeId= " + orgTypeId + " ";
                }

                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                rdr = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<AttributeStatusTO> list = ConvertDTToListForLabel(rdr);
                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if(rdr != null)
                    rdr.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }

        }

        //selecting list of Attributes to show on UI
        public List<AttributeStatusTO> AllAttributeListForUI(Int32 pageId, int orgTypeId = 0)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader rdr = null;
            try
            {
                conn.Open();
                String andClause = "";
                if (orgTypeId > 0)
                    andClause = "and tblAttStatus.orgTypeId = " + orgTypeId;

                //String sqlQuery = " select ATTS.idAttribute,ATTS.attributeDisplayName,ATTS.attributeName,attConfig.pageId,attConfig.isSrcIdFilter,tblAttStatus.* from" +
                //" tblAttributes ATTS" +
                //  " LEFT JOIN dimAttributeConfig attConfig ON attConfig.idAttributeConfig = ATTS.attributeConfigId " +
                //" LEFT JOIN  tblAttributeStatus tblAttStatus ON ATTS.idAttribute = tblAttStatus.attributeId " + andClause;

                //Added by minal For first Priority to AttributeDisplayName in AttributeStatus
                String sqlQuery = " select ATTS.idAttribute AS idAttribute," +
                    " CASE WHEN tblAttStatus.attributeDisplayName IS NOT NULL THEN tblAttStatus.attributeDisplayName ELSE ATTS.attributeDisplayName END AS attributeDisplayName, " +
                    " ATTS.attributeName,attConfig.pageId,attConfig.isSrcIdFilter,tblAttStatus.* from" +
                " tblAttributes ATTS" +
                  " LEFT JOIN dimAttributeConfig attConfig ON attConfig.idAttributeConfig = ATTS.attributeConfigId " +
                " LEFT JOIN  tblAttributeStatus tblAttStatus ON ATTS.idAttribute = tblAttStatus.attributeId " + andClause;

                cmdSelect.CommandText = sqlQuery + " where attConfig.pageId = " + pageId + "  ";

                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                rdr = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<AttributeStatusTO> list = ConvertDTToList(rdr);
                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                rdr.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }

        }

        //Check Data Exists Or Not
        public List<AttributeStatusTO> CheckDataExistsOrNot(AttributeStatusTO attributeTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader sqlReader = null;
            try
            {
                conn.Open();


                cmdSelect.CommandText = SqlSelectQuery() +
                                        " where attrStatus.pageId = " + attributeTO.PageId + " and attrStatus.attributeId = " + attributeTO.AttributeId + " and attrStatus.isActive=1 ";

                if (attributeTO.OrgTypeId > 0)
                {
                    String addOrgTypeId = "and orgTypeId=" + attributeTO.OrgTypeId + " ";
                    cmdSelect.CommandText += addOrgTypeId;
                }
                else
                {
                    String addOrgTypeId = "and orgTypeId is null";
                    cmdSelect.CommandText += addOrgTypeId;
                }



                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<AttributeStatusTO> list = ConvertDTToList(sqlReader);
                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                sqlReader.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }
        }


        public List<AttributePageTO> ConvertDTToPageList(SqlDataReader AttributePageTODT)
        {
            List<AttributePageTO> AttributePageTOList = new List<AttributePageTO>();
            if (AttributePageTODT != null)
            {
                while (AttributePageTODT.Read())
                {
                    AttributePageTO AttributePageTONew = new AttributePageTO();
                    if (AttributePageTODT["pageId"] != DBNull.Value)
                        AttributePageTONew.PageId = Convert.ToInt32(AttributePageTODT["pageId"].ToString());

                    if (AttributePageTODT["isSrcIdFilter"] != DBNull.Value)
                        AttributePageTONew.IsSrcFilter = Convert.ToInt32(AttributePageTODT["isSrcIdFilter"].ToString());

                    if (AttributePageTODT["pageDesc"] != DBNull.Value)
                        AttributePageTONew.PageName = Convert.ToString(AttributePageTODT["pageDesc"].ToString());

                    if (AttributePageTODT["tableName"] != DBNull.Value)
                        AttributePageTONew.TableName = Convert.ToString(AttributePageTODT["tableName"].ToString());

                    if (AttributePageTODT["idParam"] != DBNull.Value)
                        AttributePageTONew.IdParam = Convert.ToString(AttributePageTODT["idParam"].ToString());

                    if (AttributePageTODT["nameParam"] != DBNull.Value)
                        AttributePageTONew.NameParam = Convert.ToString(AttributePageTODT["nameParam"].ToString());



                    AttributePageTOList.Add(AttributePageTONew);
                }
            }
            return AttributePageTOList;
        }


        public List<DropDownTO> GetAttributeSrcList(AttributePageTO attrSrcTO)
        {

            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            SqlDataReader rdr = null;
            try
            {
                conn.Open();

                cmdSelect.CommandText = "select " + attrSrcTO.IdParam + " as Value , " + attrSrcTO.NameParam + " as Text from "
                    + attrSrcTO.TableName;


                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                List<DropDownTO> list = new List<DropDownTO>();
                rdr = cmdSelect.ExecuteReader(CommandBehavior.Default);


                while (rdr.Read())
                {
                    DropDownTO dynSrcTO = new DropDownTO();
                    if (rdr["Value"] != DBNull.Value)
                        dynSrcTO.Value = Convert.ToInt32(rdr["Value"].ToString());
                    if (rdr["Text"] != DBNull.Value)
                        dynSrcTO.Text = Convert.ToString(rdr["Text"].ToString());
                    list.Add(dynSrcTO);

                }

                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                rdr.Dispose();
                conn.Close();
                cmdSelect.Dispose();
            }



        }
       
        public List<AttributeStatusTO> ConvertDTToList(SqlDataReader AttributeStatusTODT)
        {
            List<AttributeStatusTO> AttributeStatusTOList = new List<AttributeStatusTO>();
            if (AttributeStatusTODT != null)
            {
                while (AttributeStatusTODT.Read())
                {
                    AttributeStatusTO AttributeStatusTONew = new AttributeStatusTO();


                    if (AttributeStatusTODT["attributeName"] != DBNull.Value)
                        AttributeStatusTONew.AttributeName = Convert.ToString(AttributeStatusTODT["attributeName"].ToString());

                    if (AttributeStatusTODT["attributeDisplayName"] != DBNull.Value)
                        AttributeStatusTONew.AttributeDisplayName = Convert.ToString(AttributeStatusTODT["attributeDisplayName"].ToString());

                    if (AttributeStatusTODT["idAttributesStatus"] != DBNull.Value)
                        AttributeStatusTONew.IdAttributesStatus = Convert.ToInt32(AttributeStatusTODT["idAttributesStatus"].ToString());

                    if (AttributeStatusTODT["pageId"] != DBNull.Value)
                        AttributeStatusTONew.PageId = Convert.ToInt32(AttributeStatusTODT["pageId"].ToString());

                    if (AttributeStatusTODT["orgTypeId"] != DBNull.Value)
                        AttributeStatusTONew.OrgTypeId = Convert.ToInt32(AttributeStatusTODT["orgTypeId"].ToString());

                    if (AttributeStatusTODT["idAttribute"] != DBNull.Value)
                        AttributeStatusTONew.AttributeId = Convert.ToInt32(AttributeStatusTODT["idAttribute"].ToString());

                    if (AttributeStatusTODT["isVisible"] != DBNull.Value)
                        AttributeStatusTONew.IsVisible = Convert.ToInt32(AttributeStatusTODT["isVisible"].ToString());

                    if (AttributeStatusTODT["isMandatory"] != DBNull.Value)
                        AttributeStatusTONew.IsMandatory = Convert.ToInt32(AttributeStatusTODT["isMandatory"].ToString());

                    if (AttributeStatusTODT["isSrcIdFilter"] != DBNull.Value)
                        AttributeStatusTONew.IsSrcFilter = Convert.ToInt32(AttributeStatusTODT["isSrcIdFilter"].ToString());

                    if (AttributeStatusTODT["isActive"] != DBNull.Value)
                        AttributeStatusTONew.IsActive = Convert.ToInt32(AttributeStatusTODT["isActive"].ToString());                    

                    AttributeStatusTOList.Add(AttributeStatusTONew);
                }
            }
            return AttributeStatusTOList;
        }

        public List<AttributeStatusTO> ConvertDTToListForLabel(SqlDataReader AttributeStatusTODT)
        {
            List<AttributeStatusTO> AttributeStatusTOList = new List<AttributeStatusTO>();
            if (AttributeStatusTODT != null)
            {
                while (AttributeStatusTODT.Read())
                {
                    AttributeStatusTO AttributeStatusTONew = new AttributeStatusTO();


                    if (AttributeStatusTODT["attributeName"] != DBNull.Value)
                        AttributeStatusTONew.AttributeName = Convert.ToString(AttributeStatusTODT["attributeName"].ToString());

                    if (AttributeStatusTODT["attributeDisplayName"] != DBNull.Value)
                        AttributeStatusTONew.AttributeDisplayName = Convert.ToString(AttributeStatusTODT["attributeDisplayName"].ToString());

                    if (AttributeStatusTODT["idAttributesStatus"] != DBNull.Value)
                        AttributeStatusTONew.IdAttributesStatus = Convert.ToInt32(AttributeStatusTODT["idAttributesStatus"].ToString());

                    if (AttributeStatusTODT["pageId"] != DBNull.Value)
                        AttributeStatusTONew.PageId = Convert.ToInt32(AttributeStatusTODT["pageId"].ToString());

                    if (AttributeStatusTODT["orgTypeId"] != DBNull.Value)
                        AttributeStatusTONew.OrgTypeId = Convert.ToInt32(AttributeStatusTODT["orgTypeId"].ToString());

                    if (AttributeStatusTODT["idAttribute"] != DBNull.Value)
                        AttributeStatusTONew.AttributeId = Convert.ToInt32(AttributeStatusTODT["idAttribute"].ToString());

                    if (AttributeStatusTODT["isVisible"] != DBNull.Value)
                        AttributeStatusTONew.IsVisible = Convert.ToInt32(AttributeStatusTODT["isVisible"].ToString());

                    if (AttributeStatusTODT["isMandatory"] != DBNull.Value)
                        AttributeStatusTONew.IsMandatory = Convert.ToInt32(AttributeStatusTODT["isMandatory"].ToString());

                    if (AttributeStatusTODT["isSrcIdFilter"] != DBNull.Value)
                        AttributeStatusTONew.IsSrcFilter = Convert.ToInt32(AttributeStatusTODT["isSrcIdFilter"].ToString());

                    if (AttributeStatusTODT["isActive"] != DBNull.Value)
                        AttributeStatusTONew.IsActive = Convert.ToInt32(AttributeStatusTODT["isActive"].ToString());

                    if (AttributeStatusTODT["lagId"] != DBNull.Value)
                        AttributeStatusTONew.lagId = Convert.ToInt32(AttributeStatusTODT["lagId"].ToString());

                    if (AttributeStatusTODT["valueLabel"] != DBNull.Value)
                        AttributeStatusTONew.valueLabel = Convert.ToString(AttributeStatusTODT["valueLabel"]);

                    AttributeStatusTOList.Add(AttributeStatusTONew);
                }
            }
            return AttributeStatusTOList;
        }

        #endregion


        #region Insertion


        public int InsertTblAttributeStatus(AttributeStatusTO tblAttributeStatusTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblAttributeStatusTO, cmdInsert);
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

        public int ExecuteInsertionCommand(AttributeStatusTO tblAttributeStatusTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblAttributeStatus]( " +
            // "  [idAttributesStatus]" +

            " [orgTypeId]" +
              ",[pageId]" +
            " ,[isVisible]" +
            " ,[isMandatory]" +
            " ,[attributeId]" +
            " ,[isActive]" +
            " ,[createdBy]" +
            " ,[createdOn]" +
            " ,[attributeDisplayName]" +
            " )" +
" VALUES (" +
            //  "  @IdAttributesStatus " +

            " @OrgTypeId " +
             " ,@PageId " +
            " ,@IsVisible " +
            " ,@IsMandatory " +
            " ,@attributeId " +
            " ,@IsActive " +
            " ,@CreatedBy " +
            " ,@CreatedOn " +
            " , @AttributeDisplayName" +
            " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            cmdInsert.Parameters.Add("@AttributeDisplayName", System.Data.SqlDbType.NVarChar).Value = tblAttributeStatusTO.AttributeDisplayName;
            cmdInsert.Parameters.Add("@OrgTypeId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblAttributeStatusTO.OrgTypeId);
            cmdInsert.Parameters.Add("@PageId", System.Data.SqlDbType.Int).Value = Constants.GetSqlDataValueNullForBaseValue(tblAttributeStatusTO.PageId);
            cmdInsert.Parameters.Add("@IsVisible", System.Data.SqlDbType.Int).Value = tblAttributeStatusTO.IsVisible;
            cmdInsert.Parameters.Add("@IsMandatory", System.Data.SqlDbType.Int).Value = tblAttributeStatusTO.IsMandatory;
            cmdInsert.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = 1;
            cmdInsert.Parameters.Add("@attributeId", System.Data.SqlDbType.Int).Value = tblAttributeStatusTO.AttributeId;
            cmdInsert.Parameters.Add("@CreatedBy", System.Data.SqlDbType.Int).Value = tblAttributeStatusTO.CreatedBy;
            cmdInsert.Parameters.Add("@CreatedOn", System.Data.SqlDbType.DateTime).Value = tblAttributeStatusTO.CreatedOn;
            return cmdInsert.ExecuteNonQuery();
        }
        #endregion

        #region Updation

        public int UpdateTblAttributeStatus(AttributeStatusTO tblAttributeStatusTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblAttributeStatusTO, cmdUpdate);
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

        public int ExecuteUpdationCommand(AttributeStatusTO tblAttributeStatusTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblAttributeStatus] SET " +
            " attributeDisplayName = @AttributeDisplayName " +
            " ,[isVisible]= @IsVisible" +
            " ,[isMandatory]= @IsMandatory" +
            " ,[isActive]= @IsActive" +
            " ,[updatedBy]= @UpdatedBy" +
            " ,[updatedOn]= @UpdatedOn" +
            " WHERE idAttributesStatus = @IdAttributesStatus ";

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@AttributeDisplayName", System.Data.SqlDbType.NVarChar).Value = tblAttributeStatusTO.AttributeDisplayName;
            cmdUpdate.Parameters.Add("@IdAttributesStatus", System.Data.SqlDbType.Int).Value = tblAttributeStatusTO.IdAttributesStatus;
            cmdUpdate.Parameters.Add("@IsVisible", System.Data.SqlDbType.Int).Value = tblAttributeStatusTO.IsVisible;
            cmdUpdate.Parameters.Add("@IsMandatory", System.Data.SqlDbType.Int).Value = tblAttributeStatusTO.IsMandatory;
            cmdUpdate.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = tblAttributeStatusTO.IsActive;
            cmdUpdate.Parameters.Add("@UpdatedBy", System.Data.SqlDbType.Int).Value = tblAttributeStatusTO.UpdatedBy;
            cmdUpdate.Parameters.Add("@UpdatedOn", System.Data.SqlDbType.DateTime).Value = tblAttributeStatusTO.UpdatedOn;
            return cmdUpdate.ExecuteNonQuery();
        }


        public int UpdateEditedAttributeName(AttributeStatusTO attributeTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteEditAttributeNameUpdationCommand(attributeTO, cmdUpdate);
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

        public int UpdateEditedAttributeNameForAttrStatus(AttributeStatusTO attributeTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteEditAttributeNameForAttrStatusUpdationCommand(attributeTO, cmdUpdate);
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

        public int ExecuteEditAttributeNameUpdationCommand(AttributeStatusTO attributeTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblAttributes] SET " +

            " [attributeDisplayName]= @AttributeDisplayName" +

            " WHERE idAttribute = @IdAttribute ";

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdAttribute", System.Data.SqlDbType.Int).Value = attributeTO.AttributeId;
            cmdUpdate.Parameters.Add("@AttributeDisplayName", System.Data.SqlDbType.NVarChar).Value = attributeTO.AttributeDisplayName;

            return cmdUpdate.ExecuteNonQuery();
        }

        public int ExecuteEditAttributeNameForAttrStatusUpdationCommand(AttributeStatusTO attributeTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblAttributeStatus] SET " +

            " [attributeDisplayName]= @AttributeDisplayName" +

            " WHERE idAttributesStatus = @IdAttributesStatus ";

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@IdAttributesStatus", System.Data.SqlDbType.Int).Value = attributeTO.IdAttributesStatus;
            cmdUpdate.Parameters.Add("@AttributeDisplayName", System.Data.SqlDbType.NVarChar).Value = attributeTO.AttributeDisplayName;

            return cmdUpdate.ExecuteNonQuery();
        }

        #endregion

    }
}

