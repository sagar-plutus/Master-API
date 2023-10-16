using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace simpliMASTERSAPI.Models
{
    public class TenantTO
    {
        public String TenantId { get; set; }

        public String AuthKey { get; set; }
        public String Url { get; set; }
        public String IdUser { get; set; }

    }

    public class TenantAuthTO 
    {
        public TenantAuthTO (String authKey,String url)
        {
            this.AuthKey = authKey;
            this.Url = url;
        }

        public String AuthKey { get; set; }
        public String Url { get; set; }

        
    }


   
}
