﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.DashboardModels
{
    public class QuotaAndRateInfo
    {
        #region Declaration

        private Double totalQuota;
        private Double declaredRate;
        private Double avgRateBand;
        private String displayName;


        #endregion

        #region Get Set

        public double TotalQuota { get => totalQuota; set => totalQuota = value; }
        public double DeclaredRate { get => declaredRate; set => declaredRate = value; }
        public double AvgRateBand { get => avgRateBand; set => avgRateBand = value; }
        public String DisplayName { get => displayName; set => displayName = value; }

        #endregion
    }
}
