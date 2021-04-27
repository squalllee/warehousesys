using ExcuteJobList.Models.RECV;
using System.Collections.Generic;
using WareHouseSys.Models;

namespace Models.RECV
{
    public class RECV01:parentObj
    {
        public string 收貨單號 { set; get; }
        public string 契約編號 { set; get; }
        public string 契約金額 { set; get; }
        public string 採購方式 { set; get; }
        public string 預算來源 { set; get; }
        public string 採購承辦人 { set; get; }
        public string 案名 { set; get; }
        public string 需求人員 { set; get; }
        public string 廠商名稱 { set; get; }
        public string 廠商聯絡人 { set; get; }
        public string 電話 { set; get; }
        public string 手機 { set; get; }
        public string 交貨批次 { set; get; }
        public string 履約期限 { set; get; }
        public string 預計送貨日 { set; get; }
        public string 交貨地點 { set; get; }
        public string 採購承辦人ID { get; set; }


        public List<recvMaterial> AR0001 { get; set; }
    }
}
