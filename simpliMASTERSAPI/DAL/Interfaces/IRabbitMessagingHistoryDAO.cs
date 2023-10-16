using ODLMWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace simpliMASTERSAPI.DAL.Interfaces
{
   public interface IRabbitMessagingHistoryDAO
    {
        RabbitMessagingHistoryTO SelectRabbitMessagingHistory(Int32 sourceId, Int32 rabbitTransId);
        int InsertRabbitMessagingHistory(RabbitMessagingHistoryTO rabbitMessagingHistoryTO, SqlConnection conn, SqlTransaction tran);
        int InsertRabbitMessagingHistory(RabbitMessagingHistoryTO rabbitMessagingHistoryTO);
    }
}
