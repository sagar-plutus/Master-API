using ODLMWebAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Text;
using ODLMWebAPI.StaticStuff;

namespace ODLMWebAPI.Models
{
    public class TblAlertActionDtlTO
    {
        #region Declarations
        Int32 idAlertActionDtl;
        Int32 alertInstanceId;
        Int32 userId;
        Int32 snoozeTime;
        Int32 snoozeCount;
        DateTime acknowledgedOn;
        DateTime snoozeOn;
        DateTime resetDate;
        String alertComment;
        String defDesc;
        String deviceId;
        String[] deviceList;
        #endregion

        #region Constructor
        public TblAlertActionDtlTO()
        {
        }
        public TblAlertActionDtlTO Clone()
        {
            return (TblAlertActionDtlTO)this.MemberwiseClone();
        }

        #endregion

        #region GetSet
        public Int32 IdAlertActionDtl
        {
            get { return idAlertActionDtl; }
            set { idAlertActionDtl = value; }
        }
        public Int32 AlertInstanceId
        {
            get { return alertInstanceId; }
            set { alertInstanceId = value; }
        }
        public Int32 UserId
        {
            get { return userId; }
            set { userId = value; }
        }
        public Int32 SnoozeTime
        {
            get { return snoozeTime; }
            set { snoozeTime = value; }
        }
        public Int32 SnoozeCount
        {
            get { return snoozeCount; }
            set { snoozeCount = value; }
        }
        public DateTime AcknowledgedOn
        {
            get { return acknowledgedOn; }
            set { acknowledgedOn = value; }
        }
        public DateTime SnoozeOn
        {
            get { return snoozeOn; }
            set { snoozeOn = value; }
        }
        public DateTime ResetDate
        {
            get { return resetDate; }
            set { resetDate = value; }
        }

        public string AlertComment { get => alertComment; set => alertComment = value; }
        public string DefDesc { get => defDesc; set => defDesc = value; }
        public string DeviceId { get => deviceId; set => deviceId = value; }
        public string[] DeviceList { get => deviceList; set => deviceList = value; }
        #endregion
    }
}
