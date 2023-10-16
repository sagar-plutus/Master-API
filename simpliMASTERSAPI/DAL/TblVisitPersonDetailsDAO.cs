﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using ODLMWebAPI.StaticStuff;
using ODLMWebAPI.Models;
using ODLMWebAPI.DAL.Interfaces;
using ODLMWebAPI.BL.Interfaces;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;

namespace ODLMWebAPI
{ 
    public class TblVisitPersonDetailsDAO : ITblVisitPersonDetailsDAO
    {
        private readonly IConnectionString _iConnectionString;
        public TblVisitPersonDetailsDAO(IConnectionString iConnectionString)
        {
            _iConnectionString = iConnectionString;
        }
        #region Methods
        public String SqlSelectQuery()
        {
            String sqlSelectQry = " SELECT * FROM [tblPersonVisitDetails]";
            return sqlSelectQry;
        }
        #endregion

        #region Selection
        public List<TblVisitPersonDetailsTO> SelectAllTblVisitPersonDetails(int visitTypeId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                conn.Open();

                String sqlQuery = " SELECT person.idPerson, Concat(firstName,' ',lastName) as 'displayName', mobileNo,alternateMobNo,primaryEmail, " +
                                  " visitpersondetails.personTypeId "+ //visitpersondetails.visitId  " +
                                  " FROM tblVisitPersonDetails visitpersondetails INNER JOIN dimVisitPersonType  visitpersontype " +
                                  " ON visitpersontype.idPersonType = visitpersondetails.personTypeId INNER JOIN tblPerson person " +
                                  " ON person.idPerson = visitpersondetails.personId "+
                                  " INNER JOIN tblVisitDetails tblvisitdetials ON tblvisitdetials.idVisit=visitpersondetails.visitId "+
                                  " WHERE tblvisitdetials.visitTypeId = " + visitTypeId;

                cmdSelect.CommandText = sqlQuery;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader visitPersonDetailsDT = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblVisitPersonDetailsTO> list = ConvertDTToList(visitPersonDetailsDT);
                return list;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "SelectAllTblVisitPersonDetails");
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        //Added to get personDtls for offline [Hrushikesh]
        public List<TblVisitPersonDetailsTO> SelectPersonDetailsForOffline(String ids)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            ResultMessage resultMessage = new ResultMessage();
            try
            {

                conn.Open();
                //String sqlQuery = " SELECT person.idPerson," +
                //                     " CASE " +
                //                     " WHEN organization.firmName IS NOT NULL THEN(ISNULL(person.firstName, '') + ' ' + ISNULL(person.lastName, '')) + ',' + organization.firmName " +
                //                     " WHEN organization2.firmName IS NOT NULL THEN(ISNULL(person.firstName, '') + ' ' + ISNULL(person.lastName, '')) + ',' + organization2.firmName " +
                //                     " END " +
                //                     " AS Name, " +
                //                     "  ISNULL(CRMPersonLinking.personTypeId, orgPersonDtls.personTypeId) AS personTypeId,orgPersonDtls.organizationId " +
                //                      " FROM tblPerson person" +
                //                      " LEFT JOIN tblCRMPersonLinking CRMPersonLinking ON person.idPerson = CRMPersonLinking.personId " +
                //                      " LEFT JOIN tblOrgPersonDtls orgPersonDtls ON orgPersonDtls.personId = person.idPerson " +
                //                      " LEFT JOIN tblOrganization organization ON person.idPerson=organization.firstOwnerPersonId OR person.idPerson=organization.secondOwnerPersonId " +
                //                      " LEFT JOIN tblOrganization organization2 ON orgPersonDtls.organizationId = organization2.idOrganization ";
                //String whereCond = " WHERE orgPersonDtls.personTypeId in ( " + ids + " )";

                String sqlQuery = " SELECT person.idPerson, CASE WHEN organization.firmName IS NOT NULL" +
                                " THEN(ISNULL(person.firstName, '') + ' ' + ISNULL(person.lastName, '')) + ',' + organization.firmName END AS Name,  " +
                                " ISNULL(CRMPersonLinking.personTypeId, " +
                                " orgPersonDtls.personTypeId) AS personTypeId, orgPersonDtls.organizationId from tblorganization organization " +
                                " LEFT JOIN tblOrgPersonDtls orgPersonDtls ON orgPersonDtls.organizationId = organization.idOrganization " +
                                " LEFT JOIN tblperson person ON orgPersonDtls.personId = person.idPerson " +
                                " LEFT JOIN tblCRMPersonLinking CRMPersonLinking ON person.idPerson = CRMPersonLinking.personId " +
                                " WHERE orgPersonDtls.isActive = 1 AND organization.isActive = 1 and organization.orgTypeId NOT IN(6) ";
                String whereCond = " AND  orgPersonDtls.personTypeId in ( " + ids + " )";

                if (!String.IsNullOrEmpty(ids))
                {
                    sqlQuery = sqlQuery + whereCond;
                }
                cmdSelect.CommandText = sqlQuery;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;
                SqlDataReader visitPersonDetailsDT = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<TblVisitPersonDetailsTO> tblVisitPersonDetailsTOList = new List<TblVisitPersonDetailsTO>();
                if (visitPersonDetailsDT != null)
                {
                    while (visitPersonDetailsDT.Read())
                    {
                        TblVisitPersonDetailsTO tblVisitPersonDetailsTO = new TblVisitPersonDetailsTO();
                        if (visitPersonDetailsDT["idPerson"] != DBNull.Value && visitPersonDetailsDT["personTypeId"] != DBNull.Value)
                        {
                            tblVisitPersonDetailsTO.IdPerson = Convert.ToInt32(visitPersonDetailsDT["idPerson"].ToString() + visitPersonDetailsDT["personTypeId"].ToString());
                            tblVisitPersonDetailsTO.PersonId = Convert.ToInt32(visitPersonDetailsDT["idPerson"].ToString() + visitPersonDetailsDT["personTypeId"].ToString());
                        }
                        if (visitPersonDetailsDT["Name"] != DBNull.Value)
                            tblVisitPersonDetailsTO.DisplayName = Convert.ToString(visitPersonDetailsDT["Name"].ToString());
                        if (visitPersonDetailsDT["personTypeId"] != DBNull.Value)
                            tblVisitPersonDetailsTO.PersonTypeId = Convert.ToInt32(visitPersonDetailsDT["personTypeId"].ToString());
                        if (visitPersonDetailsDT["organizationId"] != DBNull.Value)
                            tblVisitPersonDetailsTO.OrganizationId = Convert.ToInt32(visitPersonDetailsDT["organizationId"].ToString());
                        tblVisitPersonDetailsTOList.Add(tblVisitPersonDetailsTO);
                    }
                }
                return tblVisitPersonDetailsTOList;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "SelectAllTblVisitPersonDetails");
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        //Sudhir - Added for Get All Visit Person TypeList.
        public List<DropDownTO> SelectAllVisitPersonTypeList()
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                conn.Open();

                String sqlQuery = " SELECT * FROM dimPersonType ";

                cmdSelect.CommandText = sqlQuery;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader visitPersonDetailsDT = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DropDownTO> dropDownTOList = new List<DropDownTO>();
                if (visitPersonDetailsDT != null)
                {
                    while (visitPersonDetailsDT.Read())
                    {
                        DropDownTO dropDownTO = new DropDownTO();
                        if (visitPersonDetailsDT["idPersonType"] != DBNull.Value)
                            dropDownTO.Value = Convert.ToInt32(visitPersonDetailsDT["idPersonType"].ToString());
                        if (visitPersonDetailsDT["personType"] != DBNull.Value)
                            dropDownTO.Text = Convert.ToString(visitPersonDetailsDT["personType"].ToString());
                        dropDownTOList.Add(dropDownTO);
                    }
                }
                return dropDownTOList;
            }
            catch(Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "SelectAllTblVisitPersonDetails");
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public List<DropDownTO> SelectVisitPersonDropDownListOnPersonType(Int32 personType, string searchText = null,bool isFilter = false)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                conn.Open();

                String sqlQuery = " with cte_VisitPersonDropDown as(SELECT person.idPerson," +
                                  " CASE " +
                                  " WHEN organization.firmName IS NOT NULL THEN(ISNULL(person.firstName, '') + ' ' + ISNULL(person.lastName, '')) + ',' + organization.firmName " +
                                  " WHEN organization2.firmName IS NOT NULL THEN(ISNULL(person.firstName, '') + ' ' + ISNULL(person.lastName, '')) + ',' + organization2.firmName " +
                                  " END " +
                                  " AS Name, " +
                                  "  ISNULL(CRMPersonLinking.personTypeId, orgPersonDtls.personTypeId) AS personTypeId " +
                                   " FROM tblPerson person" +
                                   " LEFT JOIN tblCRMPersonLinking CRMPersonLinking ON person.idPerson = CRMPersonLinking.personId " +
                                   " LEFT JOIN tblOrgPersonDtls orgPersonDtls ON orgPersonDtls.personId = person.idPerson " +
                                   " LEFT JOIN tblOrganization organization ON person.idPerson=organization.firstOwnerPersonId" +
                                   " LEFT JOIN tblOrganization organization3 on person.idPerson = organization3.secondOwnerPersonId" +
                                   " LEFT JOIN tblOrganization organization2 ON orgPersonDtls.organizationId = organization2.idOrganization " +
                                   " WHERE " +
                                   //" CRMPersonLinking.personTypeId = " + personType + " OR orgPersonDtls.personTypeId = " + personType + " Order BY person.idPerson ";
                                   "(orgPersonDtls.personTypeId = " + personType + ") AND(CRMPersonLinking.personTypeId =" + personType + ")   " +
                                   "AND(organization.orgTypeId NOT IN(9) OR organization2.orgTypeId NOT IN(9) or organization3.orgTypeId not in (9))) select " + (isFilter  && string.IsNullOrWhiteSpace(searchText) ? "top 10 " : "" ) + "* from cte_VisitPersonDropDown ";

                if(!string.IsNullOrWhiteSpace(searchText))
                {
                    sqlQuery += "where Name like '%"+ searchText + "%' ";
                }

                sqlQuery += "Order BY idPerson";

                cmdSelect.CommandText = sqlQuery;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader visitPersonDetailsDT = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DropDownTO> dropDownTOList = new List<DropDownTO>();
                if (visitPersonDetailsDT != null)
                {
                    while (visitPersonDetailsDT.Read())
                    {
                        DropDownTO dropDownTO = new DropDownTO();
                        if (visitPersonDetailsDT["idPerson"] != DBNull.Value)
                            dropDownTO.Value = Convert.ToInt32(visitPersonDetailsDT["idPerson"].ToString());
                        if (visitPersonDetailsDT["Name"] != DBNull.Value)
                            dropDownTO.Text = Convert.ToString(visitPersonDetailsDT["Name"].ToString());
                        if (visitPersonDetailsDT["personTypeId"] != DBNull.Value)
                            dropDownTO.Tag = Convert.ToString(visitPersonDetailsDT["personTypeId"].ToString());
                        dropDownTOList.Add(dropDownTO);
                    }
                }
                return dropDownTOList;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "SelectAllTblVisitPersonDetails");
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        public DataTable SelectTblPersonVisitDetails(Int32 personId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                conn.Open();
                cmdSelect.CommandText = SqlSelectQuery() + " WHERE personId = " + personId + " ";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

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

        public DataTable SelectAllTblPersonVisitDetails(SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdSelect = new SqlCommand();
            try
            {
                cmdSelect.CommandText = SqlSelectQuery();
                cmdSelect.Connection = conn;
                cmdSelect.Transaction = tran;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                return null;
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

        public List<TblVisitPersonDetailsTO> ConvertDTToList(SqlDataReader visitPersonDetailsDT)
        {
            List<TblVisitPersonDetailsTO> visitPersonDetailsTOList = new List<TblVisitPersonDetailsTO>();
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                if (visitPersonDetailsDT != null)
                {
                    while (visitPersonDetailsDT.Read())
                    {
                        TblVisitPersonDetailsTO visitPersonDetailsTONew = new TblVisitPersonDetailsTO(); 
                        if (visitPersonDetailsDT["idPerson"] != DBNull.Value)
                            visitPersonDetailsTONew.IdPerson = Convert.ToInt32(visitPersonDetailsDT["idPerson"].ToString());
                        if (visitPersonDetailsDT["displayName"] != DBNull.Value)
                            visitPersonDetailsTONew.DisplayName = visitPersonDetailsDT["displayName"].ToString();
                        //if (visitPersonDetailsDT["midName"] != DBNull.Value)
                        //    visitPersonDetailsTONew.MidName = visitPersonDetailsDT["midName"].ToString();
                        //if (visitPersonDetailsDT["lastName"] != DBNull.Value)
                        //    visitPersonDetailsTONew.LastName = visitPersonDetailsDT["lastName"].ToString();
                        if (visitPersonDetailsDT["mobileNo"] != DBNull.Value)
                            visitPersonDetailsTONew.MobileNo = visitPersonDetailsDT["mobileNo"].ToString();
                        if (visitPersonDetailsDT["alternateMobNo"] != DBNull.Value)
                            visitPersonDetailsTONew.AlternateMobNo = visitPersonDetailsDT["alternateMobNo"].ToString();
                        if (visitPersonDetailsDT["primaryEmail"] != DBNull.Value)
                            visitPersonDetailsTONew.PrimaryEmail = visitPersonDetailsDT["primaryEmail"].ToString();
                        if (visitPersonDetailsDT["personTypeId"] != DBNull.Value)
                            visitPersonDetailsTONew.PersonTypeId = Convert.ToInt32(visitPersonDetailsDT["personTypeId"].ToString());
                        //if (visitPersonDetailsDT["visitId"] != DBNull.Value)
                        //    visitPersonDetailsTONew.VisitId = Convert.ToInt32(visitPersonDetailsDT["visitId"].ToString());
                       
                        visitPersonDetailsTOList.Add(visitPersonDetailsTONew);
                    }
                }
                return visitPersonDetailsTOList;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "ConvertDTToList");
                return null;
            }
        }


        public int SelectVisitPersonCount(int visitId,int personId,int persontypeId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                conn.Open();

                String sqlQuery = " SELECT COUNT(personId) tblVisitPersonDetails WHERE personId " + personId + " AND personTypeId = " + persontypeId + " AND visitId =" + visitId;

                cmdSelect.CommandText = sqlQuery;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                return cmdSelect.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "SelectAllTblVisitPersonDetails");
                return -1;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        //Sudhir[17-July-2-2018] Added For More Filtering Data by PersonType and OrganizationId.
        public List<DropDownTO> SelectVisitPersonDropDownListOnPersonType(Int32 personType, int? organizationId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                string whereCond = String.Empty;
                if (organizationId > 0)
                    whereCond = " AND (orgPersonDtls.organizationId=" + organizationId + ")";
                else
                    whereCond = String.Empty;

                conn.Open();

                //String sqlQuery = " SELECT CRMPersonLinking.personId,(ISNULL(person.firstName,'') +' '+ ISNULL(person.lastName,'')) as Name,CRMPersonLinking.personTypeId FROM tblCRMPersonLinking " +
                //    " CRMPersonLinking " +
                //    "LEFT JOIN tblPerson person ON CRMPersonLinking.personId = person.idPerson  " +
                //    "LEFT JOIN tblOrgPersonDtls orgPersonDtls ON CRMPersonLinking.personId = orgPersonDtls.personId" +
                //    " WHERE CRMPersonLinking.personTypeId = "+ personType + " OR orgPersonDtls.personTypeId="+ personType;



                String sqlQuery = " SELECT person.idPerson," +
                                  " CASE " +
                                  " WHEN organization.firmName IS NOT NULL THEN(ISNULL(person.firstName, '') + ' ' + ISNULL(person.lastName, '')) + ',' + organization.firmName " +
                                  " WHEN organization2.firmName IS NOT NULL THEN(ISNULL(person.firstName, '') + ' ' + ISNULL(person.lastName, '')) + ',' + organization2.firmName " +
                                  " END " +
                                  " AS Name, " +
                                  "  ISNULL(CRMPersonLinking.personTypeId, orgPersonDtls.personTypeId) AS personTypeId " +
                                   " FROM tblPerson person" +
                                   " inner JOIN tblCRMPersonLinking CRMPersonLinking ON person.idPerson = CRMPersonLinking.personId " +
                                   " inner JOIN tblOrgPersonDtls orgPersonDtls ON orgPersonDtls.personId = person.idPerson " +
                                   " LEFT JOIN tblOrganization organization ON person.idPerson=organization.firstOwnerPersonId " +
                                   " left JOIN tblOrganization organization3 ON person.idPerson=organization3.secondOwnerPersonId   " +
                                   " inner JOIN tblOrganization organization2 ON orgPersonDtls.organizationId = organization2.idOrganization " +
                                   " WHERE " +
                                   //" CRMPersonLinking.personTypeId = " + personType + " OR orgPersonDtls.personTypeId = " + personType + " Order BY person.idPerson ";
                                   "(orgPersonDtls.personTypeId = " + personType + ") AND(CRMPersonLinking.personTypeId =" + personType + ")  " +
                                   //" AND(organization.orgTypeId NOT IN(9) " + " OR    organization2.orgTypeId NOT IN(9)) " + //Sudhir[16-July-2018] Commmentd
                                   whereCond + " Order BY person.idPerson";
                cmdSelect.CommandText = sqlQuery;
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader visitPersonDetailsDT = cmdSelect.ExecuteReader(CommandBehavior.Default);
                List<DropDownTO> dropDownTOList = new List<DropDownTO>();
                if (visitPersonDetailsDT != null)
                {
                    while (visitPersonDetailsDT.Read())
                    {
                        DropDownTO dropDownTO = new DropDownTO();
                        if (visitPersonDetailsDT["idPerson"] != DBNull.Value)
                            dropDownTO.Value = Convert.ToInt32(visitPersonDetailsDT["idPerson"].ToString());
                        if (visitPersonDetailsDT["Name"] != DBNull.Value)
                            dropDownTO.Text = Convert.ToString(visitPersonDetailsDT["Name"].ToString());
                        if (visitPersonDetailsDT["personTypeId"] != DBNull.Value)
                            dropDownTO.Tag = Convert.ToString(visitPersonDetailsDT["personTypeId"].ToString());
                        dropDownTOList.Add(dropDownTO);
                    }
                }
                return dropDownTOList;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "SelectAllTblVisitPersonDetails");
                return null;
            }
            finally
            {
                conn.Close();
                cmdSelect.Dispose();
            }
        }

        #endregion

        #region Insertion
        public int InsertTblPersonVisitDetails(TblVisitPersonDetailsTO tblVisitPersonDetailsTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(tblVisitPersonDetailsTO, cmdInsert);
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                conn.Close();
                cmdInsert.Dispose();
            }
        }

        public int InsertTblVisitPersonDetails(TblVisitPersonDetailsTO tblVisitPersonDetailsTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdInsert = new SqlCommand();
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                cmdInsert.Connection = conn;
                cmdInsert.Transaction = tran;
                return ExecuteInsertionCommand(tblVisitPersonDetailsTO, cmdInsert);
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "InsertTblPersonVisitDetails");
                return -1;
            }
            finally
            {
                cmdInsert.Dispose();
            }
        }

        public int ExecuteInsertionCommand(TblVisitPersonDetailsTO tblVisitPersonDetailsTO, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblVisitPersonDetails]( " +
            "  [personId]" +
            " ,[personTypeId]" +
            " ,[visitId]" +
            " )" +
            " VALUES (" +
            "  @PersonId " +
            " ,@PersonTypeId " +
            " ,@VisitId " +
            " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            cmdInsert.Parameters.Add("@PersonId", System.Data.SqlDbType.Int).Value = tblVisitPersonDetailsTO.PersonId;
            cmdInsert.Parameters.Add("@PersonTypeId", System.Data.SqlDbType.Int).Value = tblVisitPersonDetailsTO.PersonTypeId;
            cmdInsert.Parameters.Add("@VisitId", System.Data.SqlDbType.Int).Value = tblVisitPersonDetailsTO.VisitId;
            return cmdInsert.ExecuteNonQuery();
        }
        #endregion

        #region Updation
        public int UpdateTblPersonVisitDetails(TblVisitPersonDetailsTO tblVisitPersonDetailsTO)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdUpdate = new SqlCommand();
            try
            {
                conn.Open();
                cmdUpdate.Connection = conn;
                return ExecuteUpdationCommand(tblVisitPersonDetailsTO, cmdUpdate);
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                conn.Close();
                cmdUpdate.Dispose();
            }
        }

        public int UpdateTblPersonVisitDetails(TblVisitPersonDetailsTO tblVisitPersonDetailsTO, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdUpdate = new SqlCommand();
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                cmdUpdate.Connection = conn;
                cmdUpdate.Transaction = tran;
                return ExecuteUpdationCommand(tblVisitPersonDetailsTO, cmdUpdate);
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "UpdateTblPersonVisitDetails");
                return -1;
            }
            finally
            {
                cmdUpdate.Dispose();
            }
        }

        public int ExecuteUpdationCommand(TblVisitPersonDetailsTO tblVisitPersonDetailsTO, SqlCommand cmdUpdate)
        {
            String sqlQuery = @" UPDATE [tblVisitPersonDetails] SET " +
            "  [personId] = @PersonId" +

            " WHERE [personTypeId]= @PersonTypeId AND  [visitId] = @VisitId";

            cmdUpdate.CommandText = sqlQuery;
            cmdUpdate.CommandType = System.Data.CommandType.Text;

            cmdUpdate.Parameters.Add("@PersonId", System.Data.SqlDbType.Int).Value = tblVisitPersonDetailsTO.PersonId;
            cmdUpdate.Parameters.Add("@PersonTypeId", System.Data.SqlDbType.Int).Value = tblVisitPersonDetailsTO.PersonTypeId;
            cmdUpdate.Parameters.Add("@VisitId", System.Data.SqlDbType.Int).Value = tblVisitPersonDetailsTO.VisitId;
            return cmdUpdate.ExecuteNonQuery();
        }
        #endregion

        #region Deletion
        public int DeleteTblPersonVisitDetails(Int32 personId)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                conn.Open();
                cmdDelete.Connection = conn;
                return ExecuteDeletionCommand(personId, cmdDelete);
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                conn.Close();
                cmdDelete.Dispose();
            }
        }

        public int DeleteTblPersonVisitDetails(Int32 personId, SqlConnection conn, SqlTransaction tran)
        {
            SqlCommand cmdDelete = new SqlCommand();
            try
            {
                cmdDelete.Connection = conn;
                cmdDelete.Transaction = tran;
                return ExecuteDeletionCommand(personId, cmdDelete);
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                cmdDelete.Dispose();
            }
        }

        public int ExecuteDeletionCommand(Int32 personId, SqlCommand cmdDelete)
        {
            cmdDelete.CommandText = "DELETE FROM [tblPersonVisitDetails] " +
            " WHERE personId = " + personId + "";
            cmdDelete.CommandType = System.Data.CommandType.Text;

            //cmdDelete.Parameters.Add("@personId", System.Data.SqlDbType.Int).Value = tblPersonVisitDetailsTO.PersonId;
            return cmdDelete.ExecuteNonQuery();
        }
        #endregion

    }
}
