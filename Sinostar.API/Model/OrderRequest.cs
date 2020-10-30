using System.Collections.Generic;

namespace Sinostar.API.Model
{
    public class OrderRequest
    {
        /// <summary>
        /// 
        /// </summary>
        public string reference_no { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string shipping_method { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string country_code { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public double order_weight { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int order_pieces { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Consignee Consignee { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Shipper Shipper { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<ItemArrItem> ItemArr { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<VolumeItem> Volume { get; set; }
    }
}