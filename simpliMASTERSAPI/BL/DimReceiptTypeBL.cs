using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Collections;
using System.Text;
using System.Data;
using simpliMASTERSAPI.DAL.Interfaces;
using ODLMWebAPI.TO;
using simpliMASTERSAPI.BL.Interfaces;
using ODLMWebAPI.BL.Interfaces;
using ODLMWebAPI.Models;

namespace ODLMWebAPI.BL
{
    public class DimReceiptTypeBL: IDimReceiptTypeBL
    {
        private readonly IDimReceiptTypeDAO _iDimReceiptTypeDAO;
        private readonly ITblLoginBL _iTblLoginBL;
        public DimReceiptTypeBL(IDimReceiptTypeDAO iDimReceiptTypeDAO, ITblLoginBL iTblLoginBL)
        {
            _iDimReceiptTypeDAO = iDimReceiptTypeDAO;
            _iTblLoginBL = iTblLoginBL;
        }
        #region Selection
        public  DataTable SelectAllDimReceiptType()
        {
            return _iDimReceiptTypeDAO.SelectAllDimReceiptType();
        }

        public List<DimReceiptTypeTO> SelectAllDimReceiptTypeList(int userId)
        {
            DataTable dimReceiptTypeTODT = _iDimReceiptTypeDAO.SelectAllDimReceiptType();
            List<DimReceiptTypeTO> dimReceiptTypeTOList = new List<DimReceiptTypeTO>();
            List<DimReceiptTypeTO> dimReceiptTypeTOTempList = ConvertDTToList(dimReceiptTypeTODT);
            if (dimReceiptTypeTOTempList != null && dimReceiptTypeTOTempList.Count > 0)
            {
                TblUserTO tblUserTO = _iTblLoginBL.getPermissionsOnModule(userId, 0);
                if (tblUserTO != null)
                {
                    for (int i = 0; i < dimReceiptTypeTOTempList.Count; i++)
                    {
                        if (dimReceiptTypeTOTempList[i].SysElementId == 0)
                            continue;
                        if (tblUserTO.SysEleAccessDCT[dimReceiptTypeTOTempList[i].SysElementId] == "RW")
                        {
                            dimReceiptTypeTOList.Add(dimReceiptTypeTOTempList[i]);
                        }
                    }
                    return dimReceiptTypeTOList;
                }
            }
            return dimReceiptTypeTOList;
        }

        public  DimReceiptTypeTO SelectDimReceiptTypeTO(Int32 idReceiptType)
        {
            DataTable dimReceiptTypeTODT = _iDimReceiptTypeDAO.SelectDimReceiptType(idReceiptType);
            List<DimReceiptTypeTO> dimReceiptTypeTOList = ConvertDTToList(dimReceiptTypeTODT);
            if(dimReceiptTypeTOList != null && dimReceiptTypeTOList.Count == 1)
                return dimReceiptTypeTOList[0];
            else
                return null;
        }

        public  List<DimReceiptTypeTO> ConvertDTToList(DataTable dimReceiptTypeTODT )
        {
            List<DimReceiptTypeTO> dimReceiptTypeTOList = new List<DimReceiptTypeTO>();
            if (dimReceiptTypeTODT != null)
            {
                for (int rowCount = 0; rowCount < dimReceiptTypeTODT.Rows.Count; rowCount++)
                {
                    DimReceiptTypeTO dimReceiptTypeTONew = new DimReceiptTypeTO();
                    if(dimReceiptTypeTODT.Rows[rowCount]["idReceiptType"] != DBNull.Value)
                        dimReceiptTypeTONew.IdReceiptType = Convert.ToInt32( dimReceiptTypeTODT.Rows[rowCount]["idReceiptType"].ToString());
                    if(dimReceiptTypeTODT.Rows[rowCount]["sysElementId"] != DBNull.Value)
                        dimReceiptTypeTONew.SysElementId = Convert.ToInt32( dimReceiptTypeTODT.Rows[rowCount]["sysElementId"].ToString());
                    if(dimReceiptTypeTODT.Rows[rowCount]["isActive"] != DBNull.Value)
                        dimReceiptTypeTONew.IsActive = Convert.ToBoolean( dimReceiptTypeTODT.Rows[rowCount]["isActive"].ToString());
                    if(dimReceiptTypeTODT.Rows[rowCount]["receiptTypeName"] != DBNull.Value)
                        dimReceiptTypeTONew.ReceiptTypeName = Convert.ToString( dimReceiptTypeTODT.Rows[rowCount]["receiptTypeName"].ToString());
                    if(dimReceiptTypeTODT.Rows[rowCount]["receiptTypeDesc"] != DBNull.Value)
                        dimReceiptTypeTONew.ReceiptTypeDesc = Convert.ToString( dimReceiptTypeTODT.Rows[rowCount]["receiptTypeDesc"].ToString());
                    dimReceiptTypeTOList.Add(dimReceiptTypeTONew);
                }
            }
            return dimReceiptTypeTOList;
        }

        #endregion
        
        #region Insertion
        public  int InsertDimReceiptType(DimReceiptTypeTO dimReceiptTypeTO)
        {
            return _iDimReceiptTypeDAO.InsertDimReceiptType(dimReceiptTypeTO);
        }

        public  int InsertDimReceiptType(DimReceiptTypeTO dimReceiptTypeTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iDimReceiptTypeDAO.InsertDimReceiptType(dimReceiptTypeTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public  int UpdateDimReceiptType(DimReceiptTypeTO dimReceiptTypeTO)
        {
            return _iDimReceiptTypeDAO.UpdateDimReceiptType(dimReceiptTypeTO);
        }

        public  int UpdateDimReceiptType(DimReceiptTypeTO dimReceiptTypeTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iDimReceiptTypeDAO.UpdateDimReceiptType(dimReceiptTypeTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public  int DeleteDimReceiptType(Int32 idReceiptType)
        {
            return _iDimReceiptTypeDAO.DeleteDimReceiptType(idReceiptType);
        }

        public  int DeleteDimReceiptType(Int32 idReceiptType, SqlConnection conn, SqlTransaction tran)
        {
            return _iDimReceiptTypeDAO.DeleteDimReceiptType(idReceiptType, conn, tran);
        }

        #endregion
        
    }
}
