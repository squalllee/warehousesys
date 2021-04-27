using WareHouseSys.Models;

namespace WareHouseSys.ViewModels
{
    public class RecvBodyViewModel: MaterialBase
    {
        public string OrderNo { get; set; }

        public string SerialNo { get; set; }

        public string ReceiveStatus { get; set; }

        public string ReceiveMan { get; set; }

        public string StorageId { get; set; }

        //public string TemporaryStorage { get; set; }

        public string Note { get; set; }
    }
}