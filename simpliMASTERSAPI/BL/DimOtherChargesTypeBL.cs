using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Collections;
using System.Text;
using System.Data;
using ODLMWebAPI.DAL.Interfaces;
using ODLMWebAPI.Models;
using ODLMWebAPI.BL.Interfaces;

namespace ODLMWebAPI.BL
{
    public class DimOtherChargesTypeBL: IDimOtherChargesTypeBL
    {
        private readonly IDimOtherChargesTypeDAO _iDimOtherChargesTypeDAO;
        public DimOtherChargesTypeBL(IDimOtherChargesTypeDAO iDimOtherChargesTypeDAO)
        {
            _iDimOtherChargesTypeDAO = iDimOtherChargesTypeDAO;
        }

        #region Selection
        public  DataTable SelectAllDimOtherChargesType()
        {
            return _iDimOtherChargesTypeDAO.SelectAllDimOtherChargesType();
        }

        public  List<DimOtherChargesTypeTO> SelectAllDimOtherChargesTypeList()
        {
            DataTable dimOtherChargesTypeTODT = _iDimOtherChargesTypeDAO.SelectAllDimOtherChargesType();
            return ConvertDTToList(dimOtherChargesTypeTODT);
        }

        public  DimOtherChargesTypeTO SelectDimOtherChargesTypeTO(Int32 idOtherChargesType)
        {
            DataTable dimOtherChargesTypeTODT = _iDimOtherChargesTypeDAO.SelectDimOtherChargesType(idOtherChargesType);
            List<DimOtherChargesTypeTO> dimOtherChargesTypeTOList = ConvertDTToList(dimOtherChargesTypeTODT);
            if(dimOtherChargesTypeTOList != null && dimOtherChargesTypeTOList.Count == 1)
                return dimOtherChargesTypeTOList[0];
            else
                return null;
        }

        public  List<DimOtherChargesTypeTO> ConvertDTToList(DataTable dimOtherChargesTypeTODT )
        {
            List<DimOtherChargesTypeTO> dimOtherChargesTypeTOList = new List<DimOtherChargesTypeTO>();
            if (dimOtherChargesTypeTODT != null)
            {
                for (int rowCount = 0; rowCount < dimOtherChargesTypeTODT.Rows.Count; rowCount++)
                {
                    DimOtherChargesTypeTO dimOtherChargesTypeTONew = new DimOtherChargesTypeTO();
                    if(dimOtherChargesTypeTODT.Rows[rowCount]["idOtherChargesType"] != DBNull.Value)
                        dimOtherChargesTypeTONew.IdOtherChargesType = Convert.ToInt32( dimOtherChargesTypeTODT.Rows[rowCount]["idOtherChargesType"].ToString());
                    if(dimOtherChargesTypeTODT.Rows[rowCount]["isActive"] != DBNull.Value)
                        dimOtherChargesTypeTONew.IsActive = Convert.ToBoolean( dimOtherChargesTypeTODT.Rows[rowCount]["isActive"].ToString());
                    if(dimOtherChargesTypeTODT.Rows[rowCount]["otherChargesName"] != DBNull.Value)
                        dimOtherChargesTypeTONew.OtherChargesName = Convert.ToString( dimOtherChargesTypeTODT.Rows[rowCount]["otherChargesName"].ToString());
                    dimOtherChargesTypeTOList.Add(dimOtherChargesTypeTONew);
                }
            }
            return dimOtherChargesTypeTOList;
        }

        #endregion
        
        #region Insertion
        public  int InsertDimOtherChargesType(DimOtherChargesTypeTO dimOtherChargesTypeTO)
        {
            return _iDimOtherChargesTypeDAO.InsertDimOtherChargesType(dimOtherChargesTypeTO);
        }

        public  int InsertDimOtherChargesType(DimOtherChargesTypeTO dimOtherChargesTypeTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iDimOtherChargesTypeDAO.InsertDimOtherChargesType(dimOtherChargesTypeTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public  int UpdateDimOtherChargesType(DimOtherChargesTypeTO dimOtherChargesTypeTO)
        {
            return _iDimOtherChargesTypeDAO.UpdateDimOtherChargesType(dimOtherChargesTypeTO);
        }

        public  int UpdateDimOtherChargesType(DimOtherChargesTypeTO dimOtherChargesTypeTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iDimOtherChargesTypeDAO.UpdateDimOtherChargesType(dimOtherChargesTypeTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public  int DeleteDimOtherChargesType(Int32 idOtherChargesType)
        {
            return _iDimOtherChargesTypeDAO.DeleteDimOtherChargesType(idOtherChargesType);
        }

        public int DeleteDimOtherChargesType(Int32 idOtherChargesType, SqlConnection conn, SqlTransaction tran)
        {
            return _iDimOtherChargesTypeDAO.DeleteDimOtherChargesType(idOtherChargesType, conn, tran);
        }

        #endregion
        
    }
}
