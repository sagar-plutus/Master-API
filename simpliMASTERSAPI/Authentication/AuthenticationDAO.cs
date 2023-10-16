﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using ODLMWebAPI.StaticStuff;
using ODLMWebAPI.BL.Interfaces;

namespace ODLMWebAPI.Authentication
{
    public class AuthenticationDAO : IAuthenticationDAO
    { 
        private readonly IConnectionString _iConnectionString;
        public AuthenticationDAO(IConnectionString iConnectionString)
        {
            _iConnectionString = iConnectionString;
        }
        #region Selection
        public byte[] SelectAuthenticationData(string param)
        {
            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdSelect = new SqlCommand();
            byte[] paramValue = null;
            try
            {
                conn.Open();
                cmdSelect.CommandText = "Select "+ param + " from tblAuthenticationParams";
                cmdSelect.Connection = conn;
                cmdSelect.CommandType = System.Data.CommandType.Text;

                SqlDataReader sqlReader = cmdSelect.ExecuteReader(CommandBehavior.Default);
                if (sqlReader != null)
                {
                    while (sqlReader.Read())
                    {
                        if (sqlReader[param] != DBNull.Value)
                            paramValue = (byte[])sqlReader[param];
                    }
                }
                return paramValue;
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

        #region Insertion
        public int InsertAuthenticationData(byte[] authServerURL, byte[] clientId, byte[] clientSecret, byte[] scope)
        {

            String sqlConnStr = _iConnectionString.GetConnectionString(Constants.CONNECTION_STRING);
            SqlConnection conn = new SqlConnection(sqlConnStr);
            SqlCommand cmdInsert = new SqlCommand();
            try
            {
                conn.Open();
                cmdInsert.Connection = conn;
                return ExecuteInsertionCommand(authServerURL, clientId, clientSecret, scope, cmdInsert);
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

        public int ExecuteInsertionCommand(byte[] authServerURL, byte[] clientId, byte[] clientSecret, byte[] scope, SqlCommand cmdInsert)
        {
            String sqlQuery = @" INSERT INTO [tblAuthenticationParams]( " +
            " [authenticationURL]" +
            " ,[clientId]" +
            " ,[clientSecret]" +
            " ,[scope]" +
            " ,[isActive]" +
            " )" +
            " VALUES (" +
            " @AuthenticationURL " +
            " ,@ClientId " +
            " ,@ClientSecret " +
            " ,@Scope " +
            " ,@IsActive " +
            " )";
            cmdInsert.CommandText = sqlQuery;
            cmdInsert.CommandType = System.Data.CommandType.Text;

            cmdInsert.Parameters.Add("@AuthenticationURL", System.Data.SqlDbType.VarBinary).Value = authServerURL;
            cmdInsert.Parameters.Add("@ClientId", System.Data.SqlDbType.VarBinary).Value = clientId;
            cmdInsert.Parameters.Add("@ClientSecret", System.Data.SqlDbType.VarBinary).Value = clientSecret;
            cmdInsert.Parameters.Add("@Scope", System.Data.SqlDbType.VarBinary).Value = scope;
            cmdInsert.Parameters.Add("@IsActive", System.Data.SqlDbType.Int).Value = 1;

            return cmdInsert.ExecuteNonQuery();
        }
        #endregion

    }
}
