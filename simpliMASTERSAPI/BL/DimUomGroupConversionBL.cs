using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Collections;
using System.Text;
using System.Data;
using ODLMWebAPI.DAL;
using ODLMWebAPI.Models;
using ODLMWebAPI.StaticStuff;
using ODLMWebAPI.BL.Interfaces;
using ODLMWebAPI.DAL.Interfaces;
using SAPbobsCOM;

namespace ODLMWebAPI.BL
{
    public class DimUomGroupConversionBL : IDimUomGroupConversionBL
    {
        private readonly IDimUomGroupConversionDAO _iDimUomGroupConversionDAO;
        public DimUomGroupConversionBL(IDimUomGroupConversionDAO iDimUomGroupConversionDAO)
        {
            _iDimUomGroupConversionDAO = iDimUomGroupConversionDAO;
        }
        #region Selection
        public List<DimUomGroupConversionTO> SelectAllDimUomGroupConversion()
        {
            return _iDimUomGroupConversionDAO.SelectAllDimUomGroupConversion();
        }
        public List<DimUomGroupConversionTO> SelectAllDimUomGroupConversionList()
        {
            return _iDimUomGroupConversionDAO.SelectAllDimUomGroupConversion();
        }
        public DimUomGroupConversionTO SelectDimUomGroupConversionTO(Int32 idUomConversion)
        {
            return _iDimUomGroupConversionDAO.SelectDimUomGroupConversion(idUomConversion);
        }

        #endregion

        #region Insertion
        public int InsertDimUomGroupConversion(DimUomGroupConversionTO dimUomGroupConversionTO)
        {
            return _iDimUomGroupConversionDAO.InsertDimUomGroupConversion(dimUomGroupConversionTO);
        }
        public int InsertDimUomGroupConversion(DimUomGroupConversionTO dimUomGroupConversionTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iDimUomGroupConversionDAO.InsertDimUomGroupConversion(dimUomGroupConversionTO, conn, tran);
        }

        #endregion

        #region Updation
        public int UpdateDimUomGroupConversion(DimUomGroupConversionTO dimUomGroupConversionTO)
        {
            return _iDimUomGroupConversionDAO.UpdateDimUomGroupConversion(dimUomGroupConversionTO);
        }
        public int UpdateDimUomGroupConversion(DimUomGroupConversionTO dimUomGroupConversionTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iDimUomGroupConversionDAO.UpdateDimUomGroupConversion(dimUomGroupConversionTO, conn, tran);
        }

        #endregion

        #region Deletion
        public int DeleteDimUomGroupConversion(Int32 idUomConversion)
        {
            return _iDimUomGroupConversionDAO.DeleteDimUomGroupConversion(idUomConversion);
        }
        public int DeleteDimUomGroupConversion(Int32 idUomConversion, SqlConnection conn, SqlTransaction tran)
        {
            return _iDimUomGroupConversionDAO.DeleteDimUomGroupConversion(idUomConversion, conn, tran);
        }

        public List<DimUomGroupConversionTO> SelectAllDimUomGroupConversion(SqlConnection conn, SqlTransaction tran)
        {
            throw new NotImplementedException();
        }

        public List<DimUomGroupConversionTO> GetAllUOMGroupConversionListByGroupId(Int32 uomGroupId)
        {
            return _iDimUomGroupConversionDAO.GetAllUOMGroupConversionListByGroupId(uomGroupId);
        }

        public DimUomGroupConversionTO SelectDimUomGroupConversion(int idUomConversion)
        {
            throw new NotImplementedException();
        }

        #endregion

    }
}
