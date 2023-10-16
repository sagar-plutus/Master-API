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

namespace ODLMWebAPI.BL
{ 
    public class TblMaterialBL : ITblMaterialBL
    {
        private readonly ITblMaterialDAO _iTblMaterialDAO;
        public TblMaterialBL(ITblMaterialDAO iTblMaterialDAO)
        {
            _iTblMaterialDAO = iTblMaterialDAO;
        }
        #region Selection

        public List<TblMaterialTO> SelectAllTblMaterialList()
        {
            return  _iTblMaterialDAO.SelectAllTblMaterial();
           
        }
        public List<DropDownTO> SelectAllMaterialListForDropDown()
        {
            List<DropDownTO> list = _iTblMaterialDAO.SelectAllMaterialForDropDown();
            if(list!=null )
            {
                //Dictionary<string, object> testDCT = new Dictionary<string, object>();
                //List<DimProdSpecTO> dimProdSpecTOList = BL.DimProdSpecBL.SelectAllDimProdSpecList();
                //if (dimProdSpecTOList != null && dimProdSpecTOList.Count > 0)
                //{
                //    for (int i = 0; i < dimProdSpecTOList.Count; i++)
                //    {
                //        testDCT.Add(dimProdSpecTOList[i].ProdSpecDesc, dimProdSpecTOList[i].IdProdSpec);
                //    }
                //}
                //else return null;

                //var testV = GetDynamicObject(testDCT);
                //for (int i = 0; i < list.Count; i++)
                //{
                //    list[i].Tag = testV;
                //}
            }

            return list;

        }

        public dynamic GetDynamicObject(Dictionary<string, object> properties)
        {
            return new VDynObject(properties);
        }

        public TblMaterialTO SelectTblMaterialTO(Int32 idMaterial)
        {
            return _iTblMaterialDAO.SelectTblMaterial(idMaterial);
          
        }

        /// <summary>
        /// Vijaymala[12-09-2017] Added To Get Material Type List
        /// </summary>
        /// <returns></returns>
        public List<DropDownTO> SelectMaterialTypeDropDownList()
        {
            List<DropDownTO> list = _iTblMaterialDAO.SelectMaterialTypeDropDownList();
            return list;

        }


        #endregion

        #region Insertion
        public int InsertTblMaterial(TblMaterialTO tblMaterialTO)
        {
            return _iTblMaterialDAO.InsertTblMaterial(tblMaterialTO);
        }

        public int InsertTblMaterial(TblMaterialTO tblMaterialTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblMaterialDAO.InsertTblMaterial(tblMaterialTO, conn, tran);
        }

        #region Selection
        public  DataTable SelectAllSizeTestingDtl()
        {
            return _iTblMaterialDAO.SelectAllSizeTestingDtl();
        }

        public  List<SizeTestingDtlTO> SelectAllSizeTestingDtlList()
        {
            DataTable sizeTestingDtlTODT = _iTblMaterialDAO.SelectAllSizeTestingDtl();
            return ConvertDTToList(sizeTestingDtlTODT);
        }

        public  SizeTestingDtlTO SelectSizeTestingDtlTO()
        {
            DataTable sizeTestingDtlTODT = _iTblMaterialDAO.SelectSizeTestingDtl();
            List<SizeTestingDtlTO> sizeTestingDtlTOList = ConvertDTToList(sizeTestingDtlTODT);
            if (sizeTestingDtlTOList != null && sizeTestingDtlTOList.Count == 1)
                return sizeTestingDtlTOList[0];
            else
                return null;
        }

        List<SizeTestingDtlTO> ConvertDTToList(DataTable sizeTestingDtlTODT)
        {
            List<SizeTestingDtlTO> sizeTestingDtlTOList = new List<SizeTestingDtlTO>();
            if (sizeTestingDtlTODT != null)
            {
                for (int rowCount = 0; rowCount < sizeTestingDtlTODT.Rows.Count; rowCount++)
                {
                    SizeTestingDtlTO sizeTestingDtlTONew = new SizeTestingDtlTO();
                    if (sizeTestingDtlTODT.Rows[rowCount]["createOn"] != DBNull.Value)
                        sizeTestingDtlTONew.CreateOn = Convert.ToDateTime(sizeTestingDtlTODT.Rows[rowCount]["createOn"].ToString());
                    if (sizeTestingDtlTODT.Rows[rowCount]["idTestDtl"] != DBNull.Value)
                        sizeTestingDtlTONew.IdTestDtl = Convert.ToInt32(sizeTestingDtlTODT.Rows[rowCount]["idTestDtl"].ToString());
                    if (sizeTestingDtlTODT.Rows[rowCount]["materialId"] != DBNull.Value)
                        sizeTestingDtlTONew.MaterialId = Convert.ToInt32(sizeTestingDtlTODT.Rows[rowCount]["materialId"].ToString());
                    if (sizeTestingDtlTODT.Rows[rowCount]["createdBy"] != DBNull.Value)
                        sizeTestingDtlTONew.CreatedBy = Convert.ToInt32(sizeTestingDtlTODT.Rows[rowCount]["createdBy"].ToString());
                    if (sizeTestingDtlTODT.Rows[rowCount]["isActive"] != DBNull.Value)
                        sizeTestingDtlTONew.IsActive = Convert.ToInt32(sizeTestingDtlTODT.Rows[rowCount]["isActive"].ToString());
                    if (sizeTestingDtlTODT.Rows[rowCount]["TestingDate"] != DBNull.Value)
                        sizeTestingDtlTONew.TestingDate = Convert.ToDateTime(sizeTestingDtlTODT.Rows[rowCount]["TestingDate"].ToString());
                    if (sizeTestingDtlTODT.Rows[rowCount]["ChemC"] != DBNull.Value)
                        sizeTestingDtlTONew.ChemC = Convert.ToDecimal(sizeTestingDtlTODT.Rows[rowCount]["ChemC"].ToString());
                    if (sizeTestingDtlTODT.Rows[rowCount]["ChemS"] != DBNull.Value)
                        sizeTestingDtlTONew.ChemS = Convert.ToDecimal(sizeTestingDtlTODT.Rows[rowCount]["ChemS"].ToString());
                    if (sizeTestingDtlTODT.Rows[rowCount]["ChemP"] != DBNull.Value)
                        sizeTestingDtlTONew.ChemP = Convert.ToDecimal(sizeTestingDtlTODT.Rows[rowCount]["ChemP"].ToString());
                    if (sizeTestingDtlTODT.Rows[rowCount]["MechProof"] != DBNull.Value)
                        sizeTestingDtlTONew.MechProof = Convert.ToDecimal(sizeTestingDtlTODT.Rows[rowCount]["MechProof"].ToString());
                    if (sizeTestingDtlTODT.Rows[rowCount]["MechTen"] != DBNull.Value)
                        sizeTestingDtlTONew.MechTen = Convert.ToDecimal(sizeTestingDtlTODT.Rows[rowCount]["MechTen"].ToString());
                    if (sizeTestingDtlTODT.Rows[rowCount]["MechElon"] != DBNull.Value)
                        sizeTestingDtlTONew.MechElon = Convert.ToDecimal(sizeTestingDtlTODT.Rows[rowCount]["MechElon"].ToString());
                    if (sizeTestingDtlTODT.Rows[rowCount]["MechTEle"] != DBNull.Value)
                        sizeTestingDtlTONew.MechTEle = Convert.ToDecimal(sizeTestingDtlTODT.Rows[rowCount]["MechTEle"].ToString());
                    if (sizeTestingDtlTODT.Rows[rowCount]["CastNo"] != DBNull.Value)
                        sizeTestingDtlTONew.CastNo = Convert.ToString(sizeTestingDtlTODT.Rows[rowCount]["CastNo"].ToString());
                    if (sizeTestingDtlTODT.Rows[rowCount]["Grade"] != DBNull.Value)
                        sizeTestingDtlTONew.Grade = Convert.ToString(sizeTestingDtlTODT.Rows[rowCount]["Grade"].ToString());

                    if (sizeTestingDtlTODT.Rows[rowCount]["ChemCE"] != DBNull.Value)
                        sizeTestingDtlTONew.ChemCE = Convert.ToDecimal(sizeTestingDtlTODT.Rows[rowCount]["ChemCE"].ToString());
                    if (sizeTestingDtlTODT.Rows[rowCount]["ChemT"] != DBNull.Value)
                        sizeTestingDtlTONew.ChemT = Convert.ToDecimal(sizeTestingDtlTODT.Rows[rowCount]["ChemT"].ToString());

                    sizeTestingDtlTOList.Add(sizeTestingDtlTONew);
                }
            }
            return sizeTestingDtlTOList;
        }

        #endregion
        #endregion
        #region Insertion
        public  int InsertSizeTestingDtl(TblMaterialTO sizeTestingDtlTO)
        {
            return _iTblMaterialDAO.InsertSizeTestingDtlV2(sizeTestingDtlTO);
        }

        public  int InsertSizeTestingDtl(TblMaterialTO sizeTestingDtlTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblMaterialDAO.InsertSizeTestingDtl(sizeTestingDtlTO, conn, tran);
        }

        #endregion
        #region Updation
        public  int UpdateSizeTestingDtl(SizeTestingDtlTO sizeTestingDtlTO)
        {
            return _iTblMaterialDAO.UpdateSizeTestingDtl(sizeTestingDtlTO);
        }

        public  int UpdateSizeTestingDtl(SizeTestingDtlTO sizeTestingDtlTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblMaterialDAO.UpdateSizeTestingDtl(sizeTestingDtlTO, conn, tran);
        }

        #endregion
        #region Updation
        public int UpdateTblMaterial(TblMaterialTO tblMaterialTO)
        {
            return _iTblMaterialDAO.UpdateTblMaterial(tblMaterialTO);
        }

        public int UpdateTblMaterial(TblMaterialTO tblMaterialTO, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblMaterialDAO.UpdateTblMaterial(tblMaterialTO, conn, tran);
        }

        #endregion
        
        #region Deletion
        public int DeleteTblMaterial(Int32 idMaterial)
        {
            return _iTblMaterialDAO.DeleteTblMaterial(idMaterial);
        }

        public int DeleteTblMaterial(Int32 idMaterial, SqlConnection conn, SqlTransaction tran)
        {
            return _iTblMaterialDAO.DeleteTblMaterial(idMaterial, conn, tran);
        }

        #endregion
        
    }
}
