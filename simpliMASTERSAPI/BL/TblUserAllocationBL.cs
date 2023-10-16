using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Collections;
using System.Text;
using System.Data;
using TO;
using ODLMWebAPI.BL.Interfaces;
using ODLMWebAPI.DAL.Interfaces;
using System.Linq;
using ODLMWebAPI.StaticStuff;
using ODLMWebAPI.Models;

namespace ODLMWebAPI.BL
{
    public class TblUserAllocationBL: ITblUserAllocationBL
    {
        private readonly ITblUserAllocationDAO _iTblUserAllocationDAO;
        private readonly ICommon _iCommon;

        public TblUserAllocationBL(ITblUserAllocationDAO iTblUserAllocationDAO, ICommon iCommon)
        {
            _iTblUserAllocationDAO = iTblUserAllocationDAO;
            _iCommon = iCommon;
        }
        #region Selection
        public List<TblUserAllocationTO> GetUserAllocationList(Int32? userId ,Int32? allocTypeId, Int32? refId)
        {
            return _iTblUserAllocationDAO.GetUserAllocationList(userId, allocTypeId,refId);
        }


      

        #region Insertion
        public ResultMessage SaveTblUserAllocation(TblUserAllocationTO tblAllocationTO)
        {
            int result=0;

            ResultMessage resultMessage = new ResultMessage();
            tblAllocationTO.CreatedOn = _iCommon.ServerDateTime;
            //  tblAllocationTO.UpdatedOn = _iCommon.ServerDateTime;
            if (!(tblAllocationTO.UserId > 0 && tblAllocationTO.RefId > 0 && tblAllocationTO.AllocTypeId > 0))
            {

                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Text = "API : Invalid Input";
                resultMessage.Result = -1;
                return resultMessage;
            }
            List<TblUserAllocationTO> allocatedList = GetUserAllocationList(tblAllocationTO.UserId, tblAllocationTO.AllocTypeId,null);
            if (allocatedList != null && allocatedList.Count > 0)

            {
                List<TblUserAllocationTO>  checkList= allocatedList.Where(e => e.RefId == tblAllocationTO.RefId).ToList();
                if (checkList != null && checkList.Count > 0)
                {
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Text = "Cannot Reallocate To same Configuration";
                    resultMessage.Result = -1;
                    return resultMessage;
                }
                }
                result= _iTblUserAllocationDAO.InsertTblUserAllocation(tblAllocationTO);
            if (result != 1)
            {

                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Text = "API : Something";
                resultMessage.Result = -1;
                return resultMessage;
            }
            resultMessage.DefaultSuccessBehaviour();
            return resultMessage;


        }

        public ResultMessage UpdateTblUserAllocation(TblUserAllocationTO tblAllocationTO)
        {
            int result = 0;

            ResultMessage resultMessage = new ResultMessage();
            tblAllocationTO.UpdatedOn = _iCommon.ServerDateTime;
            //  tblAllocationTO.UpdatedOn = _iCommon.ServerDateTime;
            if (!(tblAllocationTO.UserId > 0 && tblAllocationTO.RefId > 0 && tblAllocationTO.AllocTypeId > 0))
            {
                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Text = "API : Invalid Input";
                resultMessage.Result = -1;
                return resultMessage;
            }
            List<TblUserAllocationTO> allocatedList = GetUserAllocationList(tblAllocationTO.UserId, tblAllocationTO.AllocTypeId,null);
            if (allocatedList != null && allocatedList.Count > 0)
            {
                List<TblUserAllocationTO> checkList = allocatedList.Where(e => e.RefId == tblAllocationTO.RefId
                        && e.IdUserAlloc != tblAllocationTO.IdUserAlloc).ToList();
                if (checkList != null && checkList.Count > 0)
                {
                    resultMessage.MessageType = ResultMessageE.Error;
                    resultMessage.Text = "API : Already Allocated";
                    resultMessage.Result = -1;
                    return resultMessage;
                }
            }
            result = _iTblUserAllocationDAO.UpdateTblUserAllocation(tblAllocationTO);
            if (result != 1)
            {

                resultMessage.MessageType = ResultMessageE.Error;
                resultMessage.Text = "API : Something";
                resultMessage.Result = -1;
                return resultMessage;
            }
            resultMessage.DefaultSuccessBehaviour();
            return resultMessage;


        }

        public int SaveTblUserAllocation(TblUserAllocationTO tblAllocationTO, SqlConnection conn, SqlTransaction tran)
        {
            tblAllocationTO.CreatedOn = _iCommon.ServerDateTime;
            //  tblAllocationTO.UpdatedOn = _iCommon.ServerDateTime;
            if (!(tblAllocationTO.UserId > 0 && tblAllocationTO.RefId > 0 && tblAllocationTO.AllocTypeId > 0))
            {

                return 0;
            }
            List<TblUserAllocationTO> allocatedList = GetUserAllocationList(tblAllocationTO.UserId, tblAllocationTO.AllocTypeId,null);
            if (allocatedList != null && allocatedList.Count > 0)

            {
                List<TblUserAllocationTO> checkList = allocatedList.Where(e => e.RefId == tblAllocationTO.RefId).ToList();
                if (checkList != null && checkList.Count > 0)
                {
                    return 0;
                }
            }
            return _iTblUserAllocationDAO.InsertTblUserAllocation(tblAllocationTO, conn, tran);
        }

        #endregion

        #endregion

    }
}
