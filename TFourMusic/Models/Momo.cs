namespace TFourMusic.Models
{
    public class Momo
    {
        public string RequestId { get; set; }
        public string OrderId { get; set; }
        public int  ErrorCode { get; set; }
        public string  Message { get; set; }
        public string LocalMessage { get; set; }
        public string  RequestType { get; set; }
        public string PayUrl { get; set; }
        public string QrCodeUrl { get; set; }
        public string DeepLink { get; set; }
        public string DeepLinkWebInApp { get; set; }
        public string Signature { get; set; }
    }
}
