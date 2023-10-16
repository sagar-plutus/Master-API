using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Collections;
using System.Text;
using System.Data;
using ODLMWebAPI.BL.Interfaces;
using ODLMWebAPI.DAL.Interfaces;
using ODLMWebAPI.Models;
using ODLMWebAPI.StaticStuff;
using Newtonsoft.Json;
using System.Linq;

namespace ODLMWebAPI.BL
{
    public class TblStoreAccessBL: ITblStoreAccessBL
    {
        private readonly ICommon _iCommon;
        private readonly ITblStoreAccessDAO _iTblStoreAccessDAO;
        private readonly IConnectionString _iConnectionString;
        public TblStoreAccessBL(ICommon _iCommon, ITblStoreAccessDAO iTblStoreAccessDAO, IConnectionString _iConnectionString)
        {
            this._iCommon = _iCommon;
            _iTblStoreAccessDAO = iTblStoreAccessDAO;
            this._iConnectionString = _iConnectionString;
        }

        #region Selection
        public List<TblStoreAccessTO> SelectAllTblStoreAccess(Int32 userId)
        {
            return _iTblStoreAccessDAO.SelectAllTblStoreAccess(userId);
        }

        //public  List<TblStoreAccessTO> SelectAllTblStoreAccessList()
        //{
        //    return _iTblStoreAccessDAO.SelectAllTblStoreAccess();
        //}
        public List<DropDownTO> GetStoresLocationDropDownList(Int32 userId)
        {
            int deptId = 0;
            List<DropDownTO> dropDownToList = new List<DropDownTO>();
            List < DropDownTO > dropDownList= JsonConvert.DeserializeObject<List<DropDownTO>>(this._iCommon.selectUserReportingListOnUserId(userId));
            
            if(dropDownList != null && dropDownList.Count > 0)
            {
                    deptId = dropDownList[0].Value;
            }
               List<TblStoreAccessTO> tblStoreAccessTOList = _iTblStoreAccessDAO.SelectAllTblStoreAccess(userId, deptId);
            if (tblStoreAccessTOList != null && tblStoreAccessTOList.Count > 0)
            {
                tblStoreAccessTOList = tblStoreAccessTOList.GroupBy(g => g.WarehouseId).Select(s => s.FirstOrDefault()).ToList();

                for (int i = 0; i < tblStoreAccessTOList.Count; i++)
                {
                    DropDownTO dropDownTO = new DropDownTO();
                    dropDownTO.Value = tblStoreAccessTOList[i].WarehouseId;
                    dropDownTO.Text = tblStoreAccessTOList[i].ParentDesc + "-" + tblStoreAccessTOList[i].LocationDesc ;
                    dropDownTO.Tag = tblStoreAccessTOList[i].ParentLocId.ToString();
                    dropDownTO.MappedTxnId = tblStoreAccessTOList[i].MappedTxnId;

                    dropDownToList.Add(dropDownTO);
                }
              
            }
            return dropDownToList;
            // return _iTblStoreAccessDAO.SelectAllTblStoreAccess(userId, deptId);
        }
        public TblStoreAccessTO SelectTblStoreAccessTO(Int32 idStoreAccess)
        {
            return _iTblStoreAccessDAO.SelectTblStoreAccess(idStoreAccess);
        }

     
        #endregion
        
        #region Insertion
        public ResultMessage InsertTblStoreAccess(TblStoreAccessTO tblStoreAccessTO)
        {
            int result = 0;
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            resultMessage.MessageType = ResultMessageE.None;
            if (tblStoreAccessTO.DeptId!=0 && tblStoreAccessTO.DeptId != null && tblStoreAccessTO.UserId > 0)
            {
                tblStoreAccessTO.DeptId = -1;
            }
           
            result = _iTblStoreAccessDAO.InsertTblStoreAccess(tblStoreAccessTO);
            if (result != 1)
            {
                resultMessage.DefaultBehaviour("Error While InsertTblStoreAccess");
                return resultMessage;
            }
            resultMessage.DefaultSuccessBehaviour();
            return resultMessage;
        }

        public  int InsertTblStoreAccess(TblStoreAccessTO tblStoreAccessTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblStoreAccessDAO.InsertTblStoreAccess(tblStoreAccessTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public ResultMessage UpdateTblStoreAccess(TblStoreAccessTO tblStoreAccessTO)
        {
            int result = 0;
            ResultMessage resultMessage = new StaticStuff.ResultMessage();
            resultMessage.MessageType = ResultMessageE.None;
            result= _iTblStoreAccessDAO.UpdateTblStoreAccess(tblStoreAccessTO);
            if (result != 1)
            {
                resultMessage.DefaultBehaviour("Error While UpdateTblStoreAccess");
                return resultMessage;
            }
            resultMessage.DefaultSuccessBehaviour();
            return resultMessage;
        }

        public  int UpdateTblStoreAccess(TblStoreAccessTO tblStoreAccessTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblStoreAccessDAO.UpdateTblStoreAccess(tblStoreAccessTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public  int DeleteTblStoreAccess(Int32 idStoreAccess)
        {
            return _iTblStoreAccessDAO.DeleteTblStoreAccess(idStoreAccess);
        }

        public  int DeleteTblStoreAccess(Int32 idStoreAccess, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblStoreAccessDAO.DeleteTblStoreAccess(idStoreAccess, conn, tran);
        }

        #endregion
        
    }
}
