using System;
using System.Collections.Generic;
using System.Text;

namespace ODLMWebAPI.Models
{
    public class RabbitMessagingHistoryTO
    {
        #region Declarations
        Int32 idRabbitMessagingHistory;
        Int32 rabbitTransId;
        Int32 sourceId;
        #endregion

        #region Constructor
        public RabbitMessagingHistoryTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdRabbitMessagingHistory
        {
            get { return idRabbitMessagingHistory; }
            set { idRabbitMessagingHistory = value; }
        }
        public Int32 RabbitTransId
        {
            get { return rabbitTransId; }
            set { rabbitTransId = value; }
        }
        public Int32 SourceId
        {
            get { return sourceId; }
            set { sourceId = value; }
        }
        #endregion
    }
}
