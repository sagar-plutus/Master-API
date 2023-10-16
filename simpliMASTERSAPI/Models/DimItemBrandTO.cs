using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace simpliMASTERSAPI.Models
{
    public class DimItemBrandTO
    {
        #region
         int idItemBrand;
         string itemBrandDesc;
         int itemMakeId;
         int isActive;
        #endregion
        #region Constructor
        public DimItemBrandTO()
        {
        }

        #endregion

        public int IdItemBrand { get => idItemBrand; set => idItemBrand = value; }
        public string ItemBrandDesc { get => itemBrandDesc; set => itemBrandDesc = value; }
        public int ItemMakeId { get => itemMakeId; set => itemMakeId = value; }
        public int IsActive { get => isActive; set => isActive = value; }
    }

    public class DimProcessMasterTO
    {
        #region
        int idProcess;
        string processName;       
        int isActive;
        #endregion
        #region Constructor
        public DimProcessMasterTO()
        {
        }

        #endregion

        public int IdProcess { get => idProcess; set => idProcess = value; }
        public string ProcessName { get => processName; set => processName = value; }       
        public int IsActive { get => isActive; set => isActive = value; }
    }

    public class DimMaterialGradeTO
    {
        public int IdMaterialGrade { get; set; }
        public string MaterialGradeName { get; set; }
        public int IsActive { get; set; }
    }

    public class DimCategoryTO
    {
        public int IdCategory { get; set; }
        public string CategoryName { get; set; }
        public int IsActive { get; set; }
    }


}
