using ODLMWebAPI.Models;
using ODLMWebAPI.StaticStuff;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace simpliMASTERSAPI.BL.Interfaces
{
    public interface IStockEmailBL
    {
        ResultMessage SendPurchaseOrderFromMail(SendMail sendMail);
    }
}
