namespace WareHouseSys.DBModels
{
    public class ToolInventoryBody
    {
        
        public string OrderNo {get;set;}

        public string SerialNo {get;set;}

        public string MaterialNo {get;set;}

        public int Quantity {get;set;}

        public string KeepMan {get;set;}

        public int? FirstCheckQty {get;set;}

        public int? SecondCheckQty {get;set;}

        public string Note {get;set;}

    }
}
