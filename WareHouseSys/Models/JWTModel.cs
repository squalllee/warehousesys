namespace WareHouseSys.Models
{
    public class JWTModel
    {
        public string iss { get; set; }
        public string aud { get; set; }
        public string iat { get; set; }
        public string exp { get; set; }
        public string sid { get; set; }
        public string username { get; set; }
    }
}