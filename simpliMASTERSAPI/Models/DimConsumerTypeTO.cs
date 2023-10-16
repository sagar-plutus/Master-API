using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.Models
{
    public class DimConsumerTypeTO
    {
        #region Declarations
        Int32 idConsumer;
        String consumerType;
        Int32 orgTypeId;
        Int32 displaySeqNo;
        Int32 isActive;      

        #endregion

        #region Constructor
        public DimConsumerTypeTO()
        {
        }

        #endregion

        #region GetSet
        public Int32 IdConsumer
        {
            get { return idConsumer; }
            set { idConsumer = value; }
        }
        public String ConsumerType
        {
            get { return consumerType; }
            set { consumerType = value; }
        }
        public Int32 OrgTypeId
        {
            get { return orgTypeId; }
            set { orgTypeId = value; }
        }
        public Int32 DisplaySeqNo
        {
            get { return displaySeqNo; }
            set { displaySeqNo = value; }
        }

        public Int32 IsActive
        {
            get { return isActive; }
            set { isActive = value; }
        }       

        #endregion
    }
}
