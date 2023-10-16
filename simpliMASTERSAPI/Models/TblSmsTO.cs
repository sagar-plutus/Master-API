using System;
using System.Collections.Generic;
using System.Text;

namespace ODLMWebAPI.Models
{
    public class TblSmsTO
    {
        #region Declarations
        Int32 idSms;
        Int32 alertInstanceId;
        DateTime sentOn;
        String mobileNo;
        String smsTxt;
        String replyTxt;
        String sourceTxnDesc;
        #endregion

        #region Constructor
        public TblSmsTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdSms
        {
            get { return idSms; }
            set { idSms = value; }
        }
        public Int32 AlertInstanceId
        {
            get { return alertInstanceId; }
            set { alertInstanceId = value; }
        }
        public DateTime SentOn
        {
            get { return sentOn; }
            set { sentOn = value; }
        }
        public String MobileNo
        {
            get { return mobileNo; }
            set { mobileNo = value; }
        }
        public String SmsTxt
        {
            get { return smsTxt; }
            set { smsTxt = value; }
        }
        public String ReplyTxt
        {
            get { return replyTxt; }
            set { replyTxt = value; }
        }
        public String SourceTxnDesc
        {
            get { return sourceTxnDesc; }
            set { sourceTxnDesc = value; }
        }
        #endregion
    }
    public class WhatsAppMsgTO
    {
        #region Declarations
        public string appid { get; set; }
        public string deliverychannel { get; set; }
        public MessageTO message { get; set; }
        public List<DestinationTO> destination { get; set; }
        #endregion

        #region Constructor
        public WhatsAppMsgTO()
        {
        }
        #endregion
        public WhatsAppMsgTO DeepCopy()
        {
            WhatsAppMsgTO other = (WhatsAppMsgTO)this.MemberwiseClone();
            return other;
        }
    }
    public class MessageTO
    {
        #region Declarations
        public string template { get; set; }
        public ParametersTO parameters { get; set; }
        #endregion

        #region Constructor
        public MessageTO()
        {
        }
        #endregion
    }
    public class ParametersTO
    {
        #region Declarations
        public string variable1 { get; set; }
        public string variable2 { get; set; }
        #endregion

        #region Constructor
        public ParametersTO()
        {
        }
        #endregion
    }
    public class DestinationTO
    {
        #region Declarations
        public List<string> waid { get; set; }
        #endregion

        #region Constructor
        public DestinationTO()
        {
        }
        #endregion
    }
}
