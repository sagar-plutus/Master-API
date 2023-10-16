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
    public class DimItemMakeBL : IDimItemMakeBL
    {
        private readonly IDimItemMakeDAO _iDimItemMakeDAO;
        public DimItemMakeBL(IDimItemMakeDAO iDimItemMakeDAO)
        {
            _iDimItemMakeDAO = iDimItemMakeDAO;
        }
         
        #region insertion
        //public int InsertDimItemMake(DimItemMakeTO dimItemMakeTO)
        //{
        //    dimItemMakeTO.IsActive = 1;
        //    return _iDimItemMakeDAO.InsertDimItemMake(dimItemMakeTO);

        //}
        public ResultMessage InsertDimItemMake(DimItemMakeTO dimItemMakeTO)
        {
            ResultMessage resultMessage = new ResultMessage();
            try
            {
                dimItemMakeTO.ItemMakeDesc = dimItemMakeTO.ItemMakeDesc.Trim();
               
                #region Chech Make Already exist or not
                List<DropDownTO> itemMakeList = _iDimItemMakeDAO.CheckMakeExistsOrNot(dimItemMakeTO.ItemMakeDesc);
                    if (itemMakeList != null && itemMakeList.Count > 0)
                    {
                        resultMessage.DefaultBehaviour("Make Item already added.");
                       // resultMessage.DisplayMessage = "Make Item already added.";
                       // resultMessage.Result = 2;
                        return resultMessage;
                    }
                #endregion

                Int32 result = 0;
                dimItemMakeTO.IsActive = 1;
                #region Add Item Make
                result = _iDimItemMakeDAO.InsertDimItemMake(dimItemMakeTO);
                Int32 idItemMake = dimItemMakeTO.IdItemMake;
                if (result == 0)
                {
                    resultMessage.DefaultBehaviour("Add Item Make Failed - InsertDimItemMake");
                    return resultMessage;
                }
                #endregion
                resultMessage.DefaultSuccessBehaviour(idItemMake);
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
