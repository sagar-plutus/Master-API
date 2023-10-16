using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ODLMWebAPI.Models
{
    public class PurchaseRequestTO
    {
        Int32 idPurchaseRequest;
        String purchaseRequestUser;
        Int32 series;
        DateTime requestDate;
        DateTime docDate;
        DateTime docDueDate;

        List<PurchaseRequestItemTO> purchaseRequestItemTOList;

        public int IdPurchaseRequest { get => idPurchaseRequest; set => idPurchaseRequest = value; }
        public String PurchaseRequestUser { get => purchaseRequestUser; set => purchaseRequestUser = value; }
        public int Series { get => series; set => series = value; }
        public DateTime DocDate { get => docDate; set => docDate = value; }
        public DateTime DocDueDate { get => docDueDate; set => docDueDate = value; }
        public DateTime RequestDate { get => requestDate; set => requestDate = value; }
        public List<PurchaseRequestItemTO> PurchaseRequestItemTOList { get => purchaseRequestItemTOList; set => purchaseRequestItemTOList = value; }
    }

    public class PurchaseRequestItemTO
    {
        String itemCode;
        Double requiredQuantity;
        String supplierCatNum;
        String warehouseCode;
        String locationCode;

        public string ItemCode { get => itemCode; set => itemCode = value; }
        public double RequiredQuantity { get => requiredQuantity; set => requiredQuantity = value; }
        public string SupplierCatNum { get => supplierCatNum; set => supplierCatNum = value; }
        public string WarehouseCode { get => WarehouseCode1; set => WarehouseCode1 = value; }
        public string WarehouseCode1 { get => warehouseCode; set => warehouseCode = value; }
        public string LocationCode { get => locationCode; set => locationCode = value; }
    }        
                                           

}
