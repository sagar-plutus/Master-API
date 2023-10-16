using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Collections;
using System.Text;
using System.Data;
using ODLMWebAPI.DAL;
using ODLMWebAPI.Models;
using ODLMWebAPI.StaticStuff;
using System.Linq;
using ODLMWebAPI.BL.Interfaces;
using ODLMWebAPI.DAL.Interfaces;

namespace ODLMWebAPI.BL
{
    public class TblEmailConfigrationBL : ITblEmailConfigrationBL
    {
        private readonly ITblEmailConfigrationDAO _iTblEmailConfigrationDAO;
        private readonly ISendMailBL _iSendMailBL;

        public TblEmailConfigrationBL(ITblEmailConfigrationDAO iTblEmailConfigrationDAO, ISendMailBL iSendMailBL)
        {
            _iTblEmailConfigrationDAO = iTblEmailConfigrationDAO;
            _iSendMailBL = iSendMailBL;
        }
        #region Selection
        public List<TblEmailConfigrationTO> SelectAllDimEmailConfigration()
        {
            return _iTblEmailConfigrationDAO.SelectAllDimEmailConfigration();
        }

        public List<TblEmailConfigrationTO> SelectAllDimEmailConfigrationList()
        {
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                List<TblEmailConfigrationTO> list = _iTblEmailConfigrationDAO.SelectAllDimEmailConfigration();
                if (list != null)
                    return list;
                else
                    return null;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "SelectAllDimEmailConfigrationList");
                return null;
            }
        }

        public TblEmailConfigrationTO SelectDimEmailConfigrationTO()
        {
            TblEmailConfigrationTO dimEmailConfigrationTODT = _iTblEmailConfigrationDAO.SelectDimEmailConfigrationIsActive();
            if (dimEmailConfigrationTODT != null)
            {
                return dimEmailConfigrationTODT;
            }
            else
            {
                return null;
            }
        }

        #endregion

        #region Insertion
        public int InsertDimEmailConfigration(TblEmailConfigrationTO dimEmailConfigrationTO)
        {
            return _iTblEmailConfigrationDAO.InsertDimEmailConfigration(dimEmailConfigrationTO);
        }

        public int InsertDimEmailConfigration(TblEmailConfigrationTO dimEmailConfigrationTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblEmailConfigrationDAO.InsertDimEmailConfigration(dimEmailConfigrationTO, conn, tran);
        }

        #endregion

        #region Updation
        public int UpdateDimEmailConfigration(TblEmailConfigrationTO dimEmailConfigrationTO)
        {
            return _iTblEmailConfigrationDAO.UpdateDimEmailConfigration(dimEmailConfigrationTO);
        }

        public int UpdateDimEmailConfigration(TblEmailConfigrationTO dimEmailConfigrationTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblEmailConfigrationDAO.UpdateDimEmailConfigration(dimEmailConfigrationTO, conn, tran);
        }

        #endregion

        #region Deletion
        public int DeleteDimEmailConfigration(Int32 idEmailConfig)
        {
            return _iTblEmailConfigrationDAO.DeleteDimEmailConfigration(idEmailConfig);
        }

        public int DeleteDimEmailConfigration(Int32 idEmailConfig, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblEmailConfigrationDAO.DeleteDimEmailConfigration(idEmailConfig, conn, tran);
        }

        #endregion

        public ResultMessage SendTestEmail(SendMail sendMail, TblEmailConfigrationTO dimEmailConfigrationTO)
        {
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                if (String.IsNullOrEmpty(sendMail.Message))
                {
                    sendMail.Message = "Test Mail";
                }
                sendMail.BodyContent = "<p>" + sendMail.Message + "</p>";
                sendMail.Subject = "Test Mail";
                return _iSendMailBL.SendEmail(sendMail, dimEmailConfigrationTO);

            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "SendTestEmail(TblEmailConfigrationTO dimEmailConfigrationTO, ComposeEmailTO composeEmailTO)");
                return resultMessage;
            }

            return resultMessage;
        }


    }
}
