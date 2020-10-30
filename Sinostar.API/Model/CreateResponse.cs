namespace Sinostar.API.Model
{
    public class CreateResponse
    {
        public string ask { get; set; }

        public string message { get; set; }

        public string reference_no { get; set; }

        public string order_no { get; set; }

        public ResponseError Error { get; set; }
    }
}