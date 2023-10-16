using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Collections;
using System.Text;
using System.Data;

using ODLMWebAPI.BL.Interfaces;
using ODLMWebAPI.DAL.Interfaces;
using ODLMWebAPI.Models;

namespace ODLMWebAPI.BL
{
    public class DimAllocationTypeBL: IDimAllocationTypeBL
    {

        private readonly IDimAllocationTypeDAO _idimAllocationTypeDAO;
        public DimAllocationTypeBL(IDimAllocationTypeDAO idimAllocationTypeDAO) {

            _idimAllocationTypeDAO = idimAllocationTypeDAO;
        }

        #region Selection


        public List<DropDownTO> getDynamicMaster(DimAllocationTypeTO allocTypeTO)
        {

            return _idimAllocationTypeDAO.getDynamicMaster(allocTypeTO);
        }

        public  List<DimAllocationTypeTO> SelectAllDimAllocationTypeList()
        {
          return _idimAllocationTypeDAO.SelectAllDimAllocationType();

        }

       public  DimAllocationTypeTO SelectDimAllocationTypeTO(Int32 idAllocType)
        {
           return _idimAllocationTypeDAO.SelectDimAllocationType(idAllocType);
          
        }

        
        #endregion
        
        #region Insertion
        public  int InsertDimAllocationType(DimAllocationTypeTO dimAllocationTypeTO)
        {

            return _idimAllocationTypeDAO.InsertDimAllocationType(dimAllocationTypeTO);
        }

        public  int InsertDimAllocationType(DimAllocationTypeTO dimAllocationTypeTO, SqlConnection conn, SqlTransaction tran)
        {
            return _idimAllocationTypeDAO.InsertDimAllocationType(dimAllocationTypeTO, conn, tran);
        }

        #endregion
        
        #region Updation
        public  int UpdateDimAllocationType(DimAllocationTypeTO dimAllocationTypeTO)
        {
            return _idimAllocationTypeDAO.UpdateDimAllocationType(dimAllocationTypeTO);
        }

        public  int UpdateDimAllocationType(DimAllocationTypeTO dimAllocationTypeTO, SqlConnection conn, SqlTransaction tran)
        {
            return _idimAllocationTypeDAO.UpdateDimAllocationType(dimAllocationTypeTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public  int DeleteDimAllocationType(Int32 idAllocType)
        {
            return _idimAllocationTypeDAO.DeleteDimAllocationType(idAllocType);
        }

        public  int DeleteDimAllocationType(Int32 idAllocType, SqlConnection conn, SqlTransaction tran)
        {
            return _idimAllocationTypeDAO.DeleteDimAllocationType(idAllocType, conn, tran);
        }

        #endregion
        
    }
}
