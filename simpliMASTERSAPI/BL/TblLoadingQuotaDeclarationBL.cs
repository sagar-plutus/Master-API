using ODLMWebAPI.Models;
using simpliMASTERSAPI.BL.Interfaces;
using System.Collections.Generic;
using System;
using System.Data.SqlClient;
using simpliMASTERSAPI.DAL.Interfaces;

namespace simpliMASTERSAPI.BL
{
    public class TblLoadingQuotaDeclarationBL : ITblLoadingQuotaDeclarationBL
    {
        private readonly ITblLoadingQuotaDeclarationDAO _tblLoadingQuotaDeclarationDAO;
        public TblLoadingQuotaDeclarationBL(ITblLoadingQuotaDeclarationDAO tblLoadingQuotaDeclarationDAO)
        {
            _tblLoadingQuotaDeclarationDAO= tblLoadingQuotaDeclarationDAO;
        }
        public  int InsertTblLoadingQuotaDeclaration(TblLoadingQuotaDeclarationTO tblLoadingQuotaDeclarationTO, SqlConnection conn, SqlTransaction tran)
        {
            return _tblLoadingQuotaDeclarationDAO.InsertTblLoadingQuotaDeclaration(tblLoadingQuotaDeclarationTO, conn, tran);
        }
        public  List<TblLoadingQuotaDeclarationTO> SelectLatestCalculatedLoadingQuotaDeclarationList(DateTime stockDate, Int32 cnfOrgId, SqlConnection conn, SqlTransaction tran)
        {
            return _tblLoadingQuotaDeclarationDAO.SelectLatestCalculatedLoadingQuotaDeclarationList(stockDate, cnfOrgId, conn, tran);
        }
    }
}
