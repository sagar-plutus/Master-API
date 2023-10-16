using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace simpliMASTERSAPI.Models
{
    public class DimItemMakeTO
    {
        #region
        int idItemMake;
        string itemMakeDesc;
        int isActive;
        #endregion
        #region Constructor
        public DimItemMakeTO()
        {
        }

        #endregion
        public int IdItemMake { get => idItemMake; set => idItemMake = value; }
        public string ItemMakeDesc { get => itemMakeDesc; set => itemMakeDesc = value; }
        public int IsActive { get => isActive; set => isActive = value; }

     
    }
}
