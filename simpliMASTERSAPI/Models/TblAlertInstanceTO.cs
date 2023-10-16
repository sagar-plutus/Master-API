using ODLMWebAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Text;
using ODLMWebAPI.StaticStuff;

namespace ODLMWebAPI.Models
{
    public class TblAlertInstanceTO
    {
        #region Declarations
        Int32 idAlertInstance;
        Int32 raisedBy;
        Int32 sourceEntityId;
        Int32 isAutoReset;
        Int32 alertDefinitionId;
        Int32 emailId;
        Int32 smsId;
        Int32 parentAlertId;
        Int32 isActive;
        DateTime effectiveFromDate;
        DateTime effectiveToDate;
        DateTime raisedOn;
        DateTime escalationOn;
        String alertComment;
        String sourceDisplayId;
        String alertAction;
        List<TblSmsTO> smsTOList;
        List<TblAlertUsersTO> alertUsersTOList;
        List<String> broadCastinguserList;
        String raisedByUserName;
        Int32 smsText;
        String smsComment;

        //Sanjay [21 Sept 2018] To Reset Alerts given
        AlertsToReset alertsToReset;

        String emailComment;
        String whatsAppComment;
        Int32 isOverrideAlertConfig;
        String whatsAppFile;

        #endregion

        #region Constructor
        public TblAlertInstanceTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdAlertInstance
        {
            get { return idAlertInstance; }
            set { idAlertInstance = value; }
        }
        public Int32 RaisedBy
        {
            get { return raisedBy; }
            set { raisedBy = value; }
        }
        public Int32 SourceEntityId
        {
            get { return sourceEntityId; }
            set { sourceEntityId = value; }
        }
        public Int32 IsAutoReset
        {
            get { return isAutoReset; }
            set { isAutoReset = value; }
        }
        public Int32 AlertDefinitionId
        {
            get { return alertDefinitionId; }
            set { alertDefinitionId = value; }
        }
        public Int32 EmailId
        {
            get { return emailId; }
            set { emailId = value; }
        }
        public Int32 SmsId
        {
            get { return smsId; }
            set { smsId = value; }
        }
        public Int32 ParentAlertId
        {
            get { return parentAlertId; }
            set { parentAlertId = value; }
        }
        public Int32 IsActive
        {
            get { return isActive; }
            set { isActive = value; }
        }
        public DateTime EffectiveFromDate
        {
            get { return effectiveFromDate; }
            set { effectiveFromDate = value; }
        }
        public DateTime EffectiveToDate
        {
            get { return effectiveToDate; }
            set { effectiveToDate = value; }
        }
        public DateTime RaisedOn
        {
            get { return raisedOn; }
            set { raisedOn = value; }
        }
        public DateTime EscalationOn
        {
            get { return escalationOn; }
            set { escalationOn = value; }
        }
        public String AlertComment
        {
            get { return alertComment; }
            set { alertComment = value; }
        }
        public String SourceDisplayId
        {
            get { return sourceDisplayId; }
            set { sourceDisplayId = value; }
        }
        public String AlertAction
        {
            get { return alertAction; }
            set { alertAction = value; }
        }

        public List<TblSmsTO> SmsTOList { get => smsTOList; set => smsTOList = value; }
        public List<TblAlertUsersTO> AlertUsersTOList { get => alertUsersTOList; set => alertUsersTOList = value; }
        public string RaisedByUserName { get => raisedByUserName; set => raisedByUserName = value; }

        public String RaisedOnStr
        {
            get { return raisedOn.ToString(Constants.DefaultDateFormat); }
        }

        public int SmsText { get => smsText; set => smsText = value; }
        public string SmsComment { get => smsComment; set => smsComment = value; }

        /// <summary>
        ///Sanjay [21 Sept 2018] To Reset Alerts given
        /// </summary>
        public AlertsToReset AlertsToReset { get => alertsToReset; set => alertsToReset = value; }
        public List<string> BroadCastinguserList { get => broadCastinguserList; set => broadCastinguserList = value; }
        public string EmailComment { get => emailComment; set => emailComment = value; }
        public string WhatsAppComment { get => whatsAppComment; set => whatsAppComment = value; }
        public string WhatsAppFile { get => whatsAppFile; set => whatsAppFile = value; }
        public int IsOverrideAlertConfig { get => isOverrideAlertConfig; set => isOverrideAlertConfig = value; }
        #endregion
    }
    public class CommonAlertTo
    {
        public String AlertComment { get; set; }
        public String AlertAction { get; set; }
        public int SourceEntityId { get; set; }
        public int RaisedBy { get; set; }
        public List<int> NotificationUserList { get; set; }
        public DateTime RaisedOn { get; set; }
        public DateTime EffectiveFromDate { get; set; }
        public DateTime EffectiveToDate { get; set; }
        public int OrgId { get; set; }
        public int AlertDefinitionId { get; set; }
    }
    public class apiData
    {
        public TblAlertInstanceTO tblAlertInstanceTO = new TblAlertInstanceTO();
    }
    /// <summary>
    /// Sanjay [22 Sept 2018]. Class to hold alert definitions with or without source transanctions to reset
    /// Object of this class will be passed from alertinstance object for resetting any prev alert if required
    /// </summary>
    public class AlertsToReset
    {
        List<int> alertDefIdList=new List<int>();
        List<ResetAlertInstanceTO> resetAlertInstanceTOList = new List<ResetAlertInstanceTO>();


        /// <summary>
        /// Sanjay [21 Sept 2018] This is List of AlertDefId To Reset Alert. It can be null
        /// </summary>
        public List<int> AlertDefIdList { get => alertDefIdList; set => alertDefIdList = value; }
        /// <summary>
        /// Sanjay [21 Sept 2018] This is Dictionary of AlertDefId and Source Entity Txn Id To Reset Alert. It can be null
        /// </summary>
        public List<ResetAlertInstanceTO> ResetAlertInstanceTOList { get => resetAlertInstanceTOList; set => resetAlertInstanceTOList = value; }

      
    }

    /// <summary>
    /// Sanjay [21 Sept 2018] To Define inner alert txn and def specific instances to reset
    /// It can be container object inside AlertsToReset
    /// </summary>
    public class ResetAlertInstanceTO
    {
        int alertDefinitionId;
        int sourceEntityTxnId;
        int userId;

        public int AlertDefinitionId { get => alertDefinitionId; set => alertDefinitionId = value; }
        public int SourceEntityTxnId { get => sourceEntityTxnId; set => sourceEntityTxnId = value; }
        public int UserId { get => userId; set => userId = value; }
    }
}
