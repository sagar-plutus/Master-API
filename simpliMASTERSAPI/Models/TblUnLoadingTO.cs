﻿using ODLMWebAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ODLMWebAPI.StaticStuff;

namespace ODLMWebAPI.Models
{
    public class TblUnLoadingTO
    {
        #region Declarations
        Int32 idUnLoading;
        Int32 supplierOrgId;
        Int32 descriptionId;
        Int32 createdBy;
        Int32 updatedBy;
        DateTime createdOn;
        DateTime updatedOn;
        Double totalUnLoadingQty;
        String refNo;
        String vehicleNo;
        String description;
        String remark;
        Int32 statusId;
        Int32 moduleId;
        String status;
        DateTime statusDate;
        Int32 tranRefId;
        List<TblUnLoadingItemDetTO> UnLoadingItemDetTOlist;
        #endregion

        #region Constructor
        public TblUnLoadingTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdUnLoading
        {
            get { return idUnLoading; }
            set { idUnLoading = value; }
        }
        public Int32 SupplierOrgId
        {
            get { return supplierOrgId; }
            set { supplierOrgId = value; }
        }

        public Int32 TranRefId
        {
            get { return tranRefId; }
            set { tranRefId = value; }
        }

        public Int32 ModuleId
        {
            get { return moduleId; }
            set { moduleId = value; }
        }
        public Int32 DescriptionId
        {
            get { return descriptionId; }
            set { descriptionId = value; }
        }
        public Int32 CreatedBy
        {
            get { return createdBy; }
            set { createdBy = value; }
        }
        public Int32 UpdatedBy
        {
            get { return updatedBy; }
            set { updatedBy = value; }
        }
        public DateTime CreatedOn
        {
            get { return createdOn; }
            set { createdOn = value; }
        }
        public DateTime UpdatedOn
        {
            get { return updatedOn; }
            set { updatedOn = value; }
        }
        public Double TotalUnLoadingQty
        {
            get { return totalUnLoadingQty; }
            set { totalUnLoadingQty = value; }
        }
        public String RefNo
        {
            get { return refNo; }
            set { refNo = value; }
        }
        public String VehicleNo
        {
            get { return vehicleNo; }
            set { vehicleNo = value; }
        }
        public String Description
        {
            get { return description; }
            set { description = value; }
        }
        public String Remark
        {
            get { return remark; }
            set { remark = value; }
        }

        public Int32 StatusId
        {
            get { return statusId; }
            set { statusId = value; }
        }

        public DateTime StatusDate
        {
            get { return statusDate; }
            set { statusDate = value; }
        }


        public List<TblUnLoadingItemDetTO> UnLoadingItemDetTOList
        {
            get { return UnLoadingItemDetTOlist; }
            set { UnLoadingItemDetTOlist = value; }
        }

        public String Status
        {
            get { return status; }
            set { status = value; }
        }

        public Constants.TranStatusE TranStatusE
        {
            get
            {
                Constants.TranStatusE tranStatusE = Constants.TranStatusE.UNLOADING_NEW;
                if (Enum.IsDefined(typeof(Constants.TranStatusE), statusId))
                {
                    tranStatusE = (Constants.TranStatusE)statusId;
                }
                return tranStatusE;

            }
            set
            {
                statusId = (int)value;
            }
        }

        #endregion
    }
}
