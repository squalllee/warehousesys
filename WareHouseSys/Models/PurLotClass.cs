namespace WareHouseSys.Models
{
    public class PurLotClass
    {
        public string PurchaseNo { set; get; }
        public string DeliveryLot { set; get; }
        public int Qty { set; get; }
        public bool IsClose { set; get; }
    }
}