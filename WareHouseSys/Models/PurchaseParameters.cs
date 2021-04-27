namespace WareHouseSys.Models
{
    public class PurchaseParameters
    {
        public int page { set; get; }
        public int rows { set; get; }
        public string sort { get; set; }
        public string order { get; set; }
        public string PurchaseNo { set; get; }
        public string CreateDateTimeStart { set; get; }
        public string CreateDateTimeEnd { set; get; }
    }
}