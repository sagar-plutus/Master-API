using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Collections;
using System.Text;
using System.Data;
using simpliMASTERSAPI.DAL.Interfaces;
using ODLMWebAPI.Models;
using simpliMASTERSAPI.BL.Interfaces;

namespace ODLMWebAPI.BL
{
    public class DimPaymentTypesBL: IDimPaymentTypesBL
    {
        private readonly IDimPaymentTypesDAO _iDimPaymentTypesDAO;
        public DimPaymentTypesBL(IDimPaymentTypesDAO iDimPaymentTypesDAO)
        {
            _iDimPaymentTypesDAO = iDimPaymentTypesDAO;
        }
        #region Selection
        public  DataTable SelectAllDimPaymentTypes()
        {
            return _iDimPaymentTypesDAO.SelectAllDimPaymentTypes();
        }

        public  List<DimPaymentTypesTO> SelectAllDimPaymentTypesList()
        {
            DataTable dimPaymentTypesTODT = _iDimPaymentTypesDAO.SelectAllDimPaymentTypes();
            return ConvertDTToList(dimPaymentTypesTODT);
        }

        public  DimPaymentTypesTO SelectDimPaymentTypesTO(Int32 idPayType)
        {
            DataTable dimPaymentTypesTODT = _iDimPaymentTypesDAO.SelectDimPaymentTypes(idPayType);
            List<DimPaymentTypesTO> dimPaymentTypesTOList = ConvertDTToList(dimPaymentTypesTODT);
            if(dimPaymentTypesTOList != null && dimPaymentTypesTOList.Count == 1)
                return dimPaymentTypesTOList[0];
            else
                return null;
        }

        public  List<DimPaymentTypesTO> ConvertDTToList(DataTable dimPaymentTypesTODT )
        {
            List<DimPaymentTypesTO> dimPaymentTypesTOList = new List<DimPaymentTypesTO>();
            if (dimPaymentTypesTODT != null)
            {
                for (int rowCount = 0; rowCount < dimPaymentTypesTODT.Rows.Count; rowCount++)
                {
                    DimPaymentTypesTO dimPaymentTypesTONew = new DimPaymentTypesTO();
                    if(dimPaymentTypesTODT.Rows[rowCount]["idPayType"] != DBNull.Value)
                        dimPaymentTypesTONew.IdPayType = Convert.ToInt32( dimPaymentTypesTODT.Rows[rowCount]["idPayType"].ToString());
                    if(dimPaymentTypesTODT.Rows[rowCount]["permissionId"] != DBNull.Value)
                        dimPaymentTypesTONew.PermissionId = Convert.ToInt32( dimPaymentTypesTODT.Rows[rowCount]["permissionId"].ToString());
                    if(dimPaymentTypesTODT.Rows[rowCount]["isActive"] != DBNull.Value)
                        dimPaymentTypesTONew.IsActive = Convert.ToBoolean( dimPaymentTypesTODT.Rows[rowCount]["isActive"].ToString());
                    if(dimPaymentTypesTODT.Rows[rowCount]["payTypeName"] != DBNull.Value)
                        dimPaymentTypesTONew.PayTypeName = Convert.ToString( dimPaymentTypesTODT.Rows[rowCount]["payTypeName"].ToString());
                    if(dimPaymentTypesTODT.Rows[rowCount]["payTypeDec"] != DBNull.Value)
                        dimPaymentTypesTONew.PayTypeDec = Convert.ToString( dimPaymentTypesTODT.Rows[rowCount]["payTypeDec"].ToString());
                    dimPaymentTypesTOList.Add(dimPaymentTypesTONew);
                }
            }
            return dimPaymentTypesTOList;
        }

        #endregion
        
        #region Insertion
        public  int InsertDimPaymentTypes(DimPaymentTypesTO dimPaymentTypesTO)
        {
            return _iDimPaymentTypesDAO.InsertDimPaymentTypes(dimPaymentTypesTO);
        }

        public  int InsertDimPaymentTypes(DimPaymentTypesTO dimPaymentTypesTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iDimPaymentTypesDAO.InsertDimPaymentTypes(dimPaymentTypesTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public  int UpdateDimPaymentTypes(DimPaymentTypesTO dimPaymentTypesTO)
        {
            return _iDimPaymentTypesDAO.UpdateDimPaymentTypes(dimPaymentTypesTO);
        }

        public  int UpdateDimPaymentTypes(DimPaymentTypesTO dimPaymentTypesTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iDimPaymentTypesDAO.UpdateDimPaymentTypes(dimPaymentTypesTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public  int DeleteDimPaymentTypes(Int32 idPayType)
        {
            return _iDimPaymentTypesDAO.DeleteDimPaymentTypes(idPayType);
        }

        public  int DeleteDimPaymentTypes(Int32 idPayType, SqlConnection conn, SqlTransaction tran)
        {
            return _iDimPaymentTypesDAO.DeleteDimPaymentTypes(idPayType, conn, tran);
        }

        #endregion
        
    }
}
