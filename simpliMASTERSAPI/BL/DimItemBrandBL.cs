using ODLMWebAPI.Models;
using ODLMWebAPI.StaticStuff;
using simpliMASTERSAPI.BL.Interfaces;
using simpliMASTERSAPI.DAL.Interfaces;
using simpliMASTERSAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace simpliMASTERSAPI.BL
{
    public class DimItemBrandBL: IDimItemBrandBL
    {
        private readonly IDimItemBrandDAO _iDimItemBrandDAO;
        public DimItemBrandBL(IDimItemBrandDAO iDimItemBrandDAO)
        {
            _iDimItemBrandDAO = iDimItemBrandDAO;
        }

        #region insertion
        //public int InsertDimItemBrand(DimItemBrandTO dimItemBrandTO)
        //{
        //    dimItemBrandTO.IsActive = 1;
        //    return _iDimItemBrandDAO.InsertDimItemBrand(dimItemBrandTO);

        //}
        public ResultMessage InsertDimItemBrand(DimItemBrandTO dimItemBrandTO)
        {
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                dimItemBrandTO.ItemBrandDesc = dimItemBrandTO.ItemBrandDesc.Trim();

                #region Chech Make Already exist or not
                List<DropDownTO> itemMakeList = _iDimItemBrandDAO.CheckBrandExistsOrNot(dimItemBrandTO.ItemBrandDesc, dimItemBrandTO.ItemMakeId);
                if (itemMakeList != null && itemMakeList.Count > 0)
                {
                    resultMessage.DefaultBehaviour("Brand already added.");
                    // resultMessage.DisplayMessage = "Make Item already added.";
                    // resultMessage.Result = 2;
                    return resultMessage;
                }
                #endregion

                Int32 result = 0;
                dimItemBrandTO.IsActive = 1;
                #region Add Item Brand
                result = _iDimItemBrandDAO.InsertDimItemBrand(dimItemBrandTO);
                Int32 idItemBrand = dimItemBrandTO.IdItemBrand;
                if (result == 0)
                {
                    resultMessage.DefaultBehaviour("Add Item Brand Failed - InsertDimItemBrand");
                    return resultMessage;
                }
                #endregion
                resultMessage.DefaultSuccessBehaviour(idItemBrand);
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "");
                return resultMessage;
            }
        }

        // Add By samadhan 24 Nov 2022
        public ResultMessage InsertDimProcess(string ProcessName)
        {
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                DimProcessMasterTO dimProcessMasterTO = new DimProcessMasterTO();

                Int32 result = 0;
                dimProcessMasterTO.IsActive = 1;
                dimProcessMasterTO.ProcessName = ProcessName;


                #region Chech Make Already exist or not
                List<DropDownTO> itemMakeList = _iDimItemBrandDAO.CheckProcessExistsOrNot(ProcessName);
                if (itemMakeList != null && itemMakeList.Count > 0)
                {
                    resultMessage.DefaultBehaviour("Process Name already added.");
                    
                    return resultMessage;
                }
                #endregion
                #region Add Item Brand

                
                result = _iDimItemBrandDAO.InsertDimProcess(dimProcessMasterTO);
                Int32 idProcess = dimProcessMasterTO.IdProcess;
                if (result == 0)
                {
                    resultMessage.DefaultBehaviour("Add Process Failed - InsertDimProcess");
                    return resultMessage;
                }
                #endregion
                resultMessage.DefaultSuccessBehaviour(idProcess);
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "");
                return resultMessage;
            }
        }

        public List<DimProcessMasterTO> SelectDimProcessType()
        {
            return _iDimItemBrandDAO.SelectDimProcessType();
        }
        public List<DimCategoryTO> SelectDimCategory()
        {
            return _iDimItemBrandDAO.SelectDimCategory();
        }
        public List<DimMaterialGradeTO> SelectDimMaterialGrade()
        {
            return _iDimItemBrandDAO.SelectDimMaterialGrade();
        }
        public ResultMessage InsertDimMaterialGrade(string MaterialGradeName)
        {
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                DimMaterialGradeTO dimMaterialGradeTO = new DimMaterialGradeTO();

                Int32 result = 0;
                dimMaterialGradeTO.IsActive = 1;
                dimMaterialGradeTO.MaterialGradeName = MaterialGradeName;


                #region Chech Make Already exist or not
                List<DropDownTO> itemMakeList = _iDimItemBrandDAO.CheckMaterialGradeExistsOrNot(MaterialGradeName);
                if (itemMakeList != null && itemMakeList.Count > 0)
                {
                    resultMessage.DefaultBehaviour("Material Grade Name already added.");

                    return resultMessage;
                }
                #endregion
                #region Add Item Brand


                result = _iDimItemBrandDAO.InsertDimMaterialGrade(dimMaterialGradeTO);
                Int32 idProcess = dimMaterialGradeTO.IdMaterialGrade;
                if (result == 0)
                {
                    resultMessage.DefaultBehaviour("Add Process Failed - InsertDimProcess");
                    return resultMessage;
                }
                #endregion
                resultMessage.DefaultSuccessBehaviour(idProcess);
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "");
                return resultMessage;
            }
        }
        public ResultMessage InsertDimCategory(string CategoryName)
        {
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                DimCategoryTO dimCategoryTO = new DimCategoryTO();

                Int32 result = 0;
                dimCategoryTO.IsActive = 1;
                dimCategoryTO.CategoryName = CategoryName;


                #region Chech Make Already exist or not
                List<DropDownTO> itemMakeList = _iDimItemBrandDAO.CheckCategoryExistsOrNot(CategoryName);
                if (itemMakeList != null && itemMakeList.Count > 0)
                {
                    resultMessage.DefaultBehaviour("Category Grade Name already added.");

                    return resultMessage;
                }
                #endregion
                #region Add Item Brand

                result = _iDimItemBrandDAO.InsertDimCategory(dimCategoryTO);
                Int32 idCategory = dimCategoryTO.IdCategory;
                if (result == 0)
                {
                    resultMessage.DefaultBehaviour("Add Category Failed - InsertDimCategory");
                    return resultMessage;
                }
                #endregion
                resultMessage.DefaultSuccessBehaviour(idCategory);
                return resultMessage;
            }
            catch (Exception ex)
            {
                resultMessage.DefaultExceptionBehaviour(ex, "");
                return resultMessage;
            }
        }
        #endregion
    }
}
