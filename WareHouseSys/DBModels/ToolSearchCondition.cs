namespace WareHouseSys.DBModels
{
    public class WorkSearchCondition
    {
        public int page { set; get; }
        public int rows { set; get; }
        public string sort { get; set; }
        public string order { get; set; }
        public string WorkNo { get; set; }
    }
}