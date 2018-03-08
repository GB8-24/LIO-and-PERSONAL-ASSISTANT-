using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JJB.Classes_Json
{
    public class CourierTrack
    {
        public Meta meta { get; set; }
        public Data data { get; set; }
    }

    public class Meta
    {
        public int code { get; set; }
    }

    public class Data
    {
        public Tracking tracking { get; set; }
    }

    public class Tracking
    {
        public string id { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        public DateTime last_updated_at { get; set; }
        public string tracking_number { get; set; }
        public string slug { get; set; }
        public bool active { get; set; }
        public object[] android { get; set; }
        public object custom_fields { get; set; }
        public object customer_name { get; set; }
        public int delivery_time { get; set; }
        public string destination_country_iso3 { get; set; }
        public object[] emails { get; set; }
        public object expected_delivery { get; set; }
        public object[] ios { get; set; }
        public object note { get; set; }
        public object order_id { get; set; }
        public object order_id_path { get; set; }
        public object origin_country_iso3 { get; set; }
        public int shipment_package_count { get; set; }
        public DateTime shipment_pickup_date { get; set; }
        public DateTime shipment_delivery_date { get; set; }
        public string shipment_type { get; set; }
        public object shipment_weight { get; set; }
        public object shipment_weight_unit { get; set; }
        public object signed_by { get; set; }
        public object[] smses { get; set; }
        public string source { get; set; }
        public string tag { get; set; }
        public string title { get; set; }
        public int tracked_count { get; set; }
        public string unique_token { get; set; }
        public Checkpoint[] checkpoints { get; set; }
        public object tracking_account_number { get; set; }
        public object tracking_destination_country { get; set; }
        public object tracking_key { get; set; }
        public object tracking_postal_code { get; set; }
        public object tracking_ship_date { get; set; }
    }

    public class Checkpoint
    {
        public string slug { get; set; }
        public object city { get; set; }
        public DateTime created_at { get; set; }
        public string location { get; set; }
        public object country_name { get; set; }
        public string message { get; set; }
        public string country_iso3 { get; set; }
        public string tag { get; set; }
        public DateTime checkpoint_time { get; set; }
        public object[] coordinates { get; set; }
        public object state { get; set; }
        public object zip { get; set; }
    }

    public class Rootobject2
    {
        public Tracking1 tracking1 { get; set; }

    }

    public class Tracking1
    {
        public string slug1 { get; set; }
        public string tracking_number1 { get; set; }
    }
}