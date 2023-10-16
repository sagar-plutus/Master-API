using ODLMWebAPI.Models;
using simpliMASTERSAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace simpliMASTERSAPI.DAL.Interfaces
{
    public interface IDimItemBrandDAO
    {
        int InsertDimItemBrand(DimItemBrandTO dimItemBrandTO);
        List<DropDownTO> CheckBrandExistsOrNot(String itemBrandDesc ,int ItemMakeId);
        
        int InsertDimProcess(DimProcessMasterTO dimProcessMasterTO);
        int InsertDimMaterialGrade(DimMaterialGradeTO dimMaterialGradeTO);
        int InsertDimCategory(DimCategoryTO dimCategoryTO);
        List<DropDownTO> CheckProcessExistsOrNot(String ProcessName);
        List<DimProcessMasterTO> SelectDimProcessType();
        List<DropDownTO> CheckMaterialGradeExistsOrNot(String MaterialGradeName);
        List<DropDownTO> CheckCategoryExistsOrNot(String CategoryName);
        List<DimCategoryTO> SelectDimCategory();
        List<DimMaterialGradeTO> SelectDimMaterialGrade();
    }
}
