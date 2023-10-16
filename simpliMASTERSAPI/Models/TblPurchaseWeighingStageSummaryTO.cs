using ODLMWebAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Text;

namespace ODLMWebAPI.Models
{
    public class TblPurchaseWeighingStageSummaryTO
    {
        #region Declarations
        Int32 idPurchaseWeighingStage;
        Int32 weighingMachineId;
        Int32 supplierId;
        Int32 createdBy;
        Int32 updatedBy;
        DateTime createdOn;
        DateTime updatedOn;
        Double grossWeightMT;
        Double actualWeightMT;
        Double netWeightMT;
        String rstNumber;
        String vehicleNo;
        Int32 purchaseScheduleSummaryId;
        Int32 weightMeasurTypeId;
        string weightMeasurTypeName;
        Int32 weightStageId;
        Int32 machineCalibrationId;

        Boolean isUpdateIsWeigingFlag;

        List<TblPurchaseUnloadingDtlTO> purchaseUnloadingDtlTOList = new List<TblPurchaseUnloadingDtlTO>();
        List<TblPurchaseGradingDtlsTO> purchaseGradingDtlsTOList = new List<TblPurchaseGradingDtlsTO>();

        int isValid;
        Double recoveryPer;
        Int32 recoveryBy;
        DateTime recoveryOn;
        Int32 isRecConfirm;

        Boolean isSaveWtStage;

        TblPurchaseWeighingStageSummaryTO tblPurchaseWeighingStageSummaryTO = null;


        #endregion

        #region Constructor
        public TblPurchaseWeighingStageSummaryTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdPurchaseWeighingStage
        {
            get { return idPurchaseWeighingStage; }
            set { idPurchaseWeighingStage = value; }
        }
        public Int32 WeighingMachineId
        {
            get { return weighingMachineId; }
            set { weighingMachineId = value; }
        }
        public Int32 SupplierId
        {
            get { return supplierId; }
            set { supplierId = value; }
        }
        public Int32 IsValid
        {
            get { return isValid; }
            set { isValid = value; }
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
        public Double GrossWeightMT
        {
            get { return grossWeightMT; }
            set { grossWeightMT = value; }
        }
        public Double ActualWeightMT
        {
            get { return actualWeightMT; }
            set { actualWeightMT = value; }
        }
        public Double NetWeightMT
        {
            get { return netWeightMT; }
            set { netWeightMT = value; }
        }
        public String RstNumber
        {
            get { return rstNumber; }
            set { rstNumber = value; }
        }
        public String VehicleNo
        {
            get { return vehicleNo; }
            set { vehicleNo = value; }
        }
        public Int32 PurchaseScheduleSummaryId
        {
            get { return purchaseScheduleSummaryId; }
            set { purchaseScheduleSummaryId = value; }
        }
        public Int32 WeightMeasurTypeId
        {
            get { return weightMeasurTypeId; }
            set { weightMeasurTypeId = value; }
        }

        public string WeightMeasurTypeName
        {
            get { return weightMeasurTypeName; }
            set { weightMeasurTypeName = value; }
        }
        public Int32 WeightStageId
        {
            get { return weightStageId; }
            set { weightStageId = value; }
        }

        public Int32 MachineCalibrationId
        {
            get { return machineCalibrationId; }
            set { machineCalibrationId = value; }
        }
        public List<TblPurchaseUnloadingDtlTO> PurchaseUnloadingDtlTOList
        {
            get { return purchaseUnloadingDtlTOList; }
            set { purchaseUnloadingDtlTOList = value; }
        }
        public List<TblPurchaseGradingDtlsTO> PurchaseGradingDtlsTOList
        {
            get { return purchaseGradingDtlsTOList; }
            set { purchaseGradingDtlsTOList = value; }
        }

        public double RecoveryPer { get => recoveryPer; set => recoveryPer = value; }
        public int RecoveryBy { get => recoveryBy; set => recoveryBy = value; }
        public DateTime RecoveryOn { get => recoveryOn; set => recoveryOn = value; }
        public int IsRecConfirm { get => isRecConfirm; set => isRecConfirm = value; }
        public Boolean IsUpdateIsWeigingFlag { get => isUpdateIsWeigingFlag; set => isUpdateIsWeigingFlag = value; }

        public Boolean IsSaveWtStage { get => isSaveWtStage; set => isSaveWtStage = value; }

        public TblPurchaseWeighingStageSummaryTO DeepCopy()
        {
            TblPurchaseWeighingStageSummaryTO other = (TblPurchaseWeighingStageSummaryTO)this.MemberwiseClone();
            return other;
        }

        #endregion
    }
}
