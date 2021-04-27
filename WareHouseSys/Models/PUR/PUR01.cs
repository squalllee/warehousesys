using System.Collections.Generic;

namespace WareHouseSys.Models.PUR
{
    public class PUR01: parentObj
    {
        public string 契約金額已稅 { get; set; }
        public string 契約金額未稅 { get; set; }
        public string 預算來源 { get; set; }
        public string 採購單位 { get; set; }
        public string 採購承辦人 { get; set; }
        public string 廠商名稱 { get; set; }
        public string 廠商聯絡人 { get; set; }
        public string 廠商電話 { get; set; }
        public string 手機 { get; set; }
        public string CreateDate { get; set; }
        public string 需求單號 { get; set; }
        public List<purMaterial> AR0001 { get; set; }
    }
}