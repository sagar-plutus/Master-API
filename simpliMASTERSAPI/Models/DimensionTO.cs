﻿using System;
using System.Collections.Generic;
using System.Text;
using ODLMWebAPI.StaticStuff;
using ODLMWebAPI.StaticStuff;

namespace ODLMWebAPI.Models
{

    public class DimensionTO
    {
        #region Declaration

        Int32 idDimension;
        String displayName;
        String dimensionValue;
        Int32 isActive;
        Int32 isApplyDimension;
        #endregion


        #region Constructor
        public DimensionTO() { }

        public int IdDimension { get => idDimension; set => idDimension = value; }
        public string DisplayName { get => displayName; set => displayName = value; }
        public string DimensionValue { get => dimensionValue; set => dimensionValue = value; }
        public int IsActive { get => isActive; set => isActive = value; }
        public int IsApplyDimension { get => isApplyDimension; set => isApplyDimension = value; }

        #endregion

        #region GetSet
        #endregion
    }
}
