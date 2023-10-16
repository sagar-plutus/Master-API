using ODLMWebAPI.StaticStuff;
using simpliMASTERSAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace simpliMASTERSAPI.BL.Interfaces
{
    public interface IDimItemBrandBL
    {
        ResultMessage InsertDimItemBrand(DimItemBrandTO dimItemBrandTO);
        ResultMessage InsertDimProcess(string ProcessName);
        ResultMessage InsertDimMaterialGrade(string MaterialGradeName);
        ResultMessage InsertDimCategory(string CategoryName);

        List<DimProcessMasterTO> SelectDimProcessType();
        //public List<DimProcessMasterTO> SelectDimProcessType()
        //{
        //    return _iDimensionDAO.SelectDimProcessType();
        //}

            // Add By Samadhan 11 May 2023 
        List<DimCategoryTO> SelectDimCategory();
        List<DimMaterialGradeTO> SelectDimMaterialGrade();
    }
}
