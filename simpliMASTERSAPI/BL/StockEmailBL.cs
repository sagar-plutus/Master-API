using ODLMWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using ODLMWebAPI.StaticStuff;
using ODLMWebAPI.BL.Interfaces;
using ODLMWebAPI.DAL.Interfaces;
using simpliMASTERSAPI.BL.Interfaces;

namespace simpliMASTERSAPI.BL
{
    public class StockEmailBL: IStockEmailBL
    {
        private readonly IConnectionString _iConnectionString;
        private readonly ITblPersonDAO _iTblPersonDAO;
        private readonly ISendMailBL _iSendMailBL;
        private readonly ICommon _iCommon;
        private readonly ITblEmailHistoryDAO _iTblEmailHistoryDAO;
        public StockEmailBL(IConnectionString iConnectionString , ITblPersonDAO iTblPersonDAO, ISendMailBL iSendMailBL, ICommon iCommon, ITblEmailHistoryDAO iTblEmailHistoryDAO)
        {
            _iTblEmailHistoryDAO = iTblEmailHistoryDAO;
            _iCommon = iCommon;
            _iSendMailBL = iSendMailBL;
            _iTblPersonDAO = iTblPersonDAO;
            _iConnectionString = iConnectionString;
        }
        #region Send PO Email
        public ResultMessage SendPurchaseOrderFromMail(SendMail sendMail)
        {
            SqlConnection conn = new SqlConnection(_iConnectionString.GetConnectionString(Constants.CONNECTION_STRING));
            SqlTransaction tran = null;
            int result = 0;
            try
            {
                ResultMessage resultMessage = new ResultMessage();
                conn.Open();
                tran = conn.BeginTransaction();
                if (sendMail.IsUpdatePerson)
                {
                    result = _iTblPersonDAO.UpdateTblPerson(sendMail.PersonInfo, conn, tran);

                    if (result != 1)
                    {
                        tran.Rollback();
                        resultMessage.DefaultBehaviour("Error while Updating Person");
                        return resultMessage;
                    }
                }
                if (sendMail.To == null)
                {
                     resultMessage.DefaultBehaviour("send mail TO not found");
                    return resultMessage;
                }
                
                    sendMail.Subject = "Purchase Order-" + sendMail.InvoiceNumber ;

               ResultMessage res  = _iSendMailBL.SendEmail(sendMail,"PO.pdf");
                if (res.Result != 1)
                {
                    tran.Rollback();
                    resultMessage = res;
                    return resultMessage;
                }
                TblEmailHistoryTO tblEmailHistoryTO = new TblEmailHistoryTO();
                tblEmailHistoryTO.SendBy = sendMail.From;
                tblEmailHistoryTO.SendTo = sendMail.To;
                tblEmailHistoryTO.SendOn = _iCommon.ServerDateTime;
                tblEmailHistoryTO.CreatedBy = sendMail.CreatedBy;

                result = _iTblEmailHistoryDAO.InsertTblEmailHistory(tblEmailHistoryTO, conn, tran);
                if (result != 1)
                {
                    
                    tran.Rollback();
                    resultMessage.DefaultBehaviour("Error in Insert History");
                }

                tran.Commit();
                resultMessage.DefaultSuccessBehaviour();
                return resultMessage;
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                conn.Close();
            }

        }
        #endregion

    }
}
