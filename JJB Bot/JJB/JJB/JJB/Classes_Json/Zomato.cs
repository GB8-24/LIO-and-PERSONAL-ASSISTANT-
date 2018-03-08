using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JJB.Classes_Json
{

    [Serializable]
    public class LocationAPI
    {
        public Location_Suggestions[] location_suggestions { get; set; }
        public string status { get; set; }
        public int has_more { get; set; }
        public int has_total { get; set; }
    }
    [Serializable]
    public class Location_Suggestions
    {
        public string entity_type { get; set; }
        public int entity_id { get; set; }
        public string title { get; set; }
        public float latitude { get; set; }
        public float longitude { get; set; }
        public int city_id { get; set; }
        public string city_name { get; set; }
        public int country_id { get; set; }
        public string country_name { get; set; }
    }

    [Serializable]
    public class LocationDetails
    {
        public string popularity { get; set; }
        public string nightlife_index { get; set; }
        public string[] nearby_res { get; set; }
        public string[] top_cuisines { get; set; }
        public string popularity_res { get; set; }
        public string nightlife_res { get; set; }
        public string subzone { get; set; }
        public int subzone_id { get; set; }
        public string city { get; set; }
        public LocationInfo location { get; set; }
        public int num_restaurant { get; set; }
        public Best_Rated_Restaurant[] best_rated_restaurant { get; set; }
    }
    [Serializable]
    public class LocationInfo
    {
        public string entity_type { get; set; }
        public string entity_id { get; set; }
        public string title { get; set; }
        public float latitude { get; set; }
        public float longitude { get; set; }
        public int city_id { get; set; }
        public string city_name { get; set; }
        public int country_id { get; set; }
        public string country_name { get; set; }
    }
    [Serializable]
    public class Best_Rated_Restaurant
    {
        public Restaurant restaurant { get; set; }
    }
    [Serializable]
    public class Restaurant
    {
        public R R { get; set; }
        public string apikey { get; set; }
        public string id { get; set; }
        public string name { get; set; }
        public string url { get; set; }
        public Location1 location { get; set; }
        public string cuisines { get; set; }
        public int average_cost_for_two { get; set; }
        public int price_range { get; set; }
        public string currency { get; set; }
        public object[] offers { get; set; }
        public string thumb { get; set; }
        public User_Rating user_rating { get; set; }
        public string photos_url { get; set; }
        public string menu_url { get; set; }
        public string featured_image { get; set; }
        public int has_online_delivery { get; set; }
        public int is_delivering_now { get; set; }
        public string deeplink { get; set; }
        public string order_url { get; set; }
        public string order_deeplink { get; set; }
        public int has_table_booking { get; set; }
        public string events_url { get; set; }
        public Zomato_Events[] zomato_events { get; set; }
    }
    [Serializable]
    public class R
    {
        public int res_id { get; set; }
    }
    [Serializable]
    public class Location1
    {
        public string address { get; set; }
        public string locality { get; set; }
        public string city { get; set; }
        public int city_id { get; set; }
        public string latitude { get; set; }
        public string longitude { get; set; }
        public string zipcode { get; set; }
        public int country_id { get; set; }
    }
    [Serializable]
    public class User_Rating
    {
        public string aggregate_rating { get; set; }
        public string rating_text { get; set; }
        public string rating_color { get; set; }
        public string votes { get; set; }
    }
    [Serializable]
    public class Zomato_Events
    {
        public Event _event { get; set; }
    }
    [Serializable]
    public class Event
    {
        public int event_id { get; set; }
        public string friendly_start_date { get; set; }
        public string friendly_end_date { get; set; }
        public string start_date { get; set; }
        public string end_date { get; set; }
        public string end_time { get; set; }
        public string start_time { get; set; }
        public int is_active { get; set; }
        public string date_added { get; set; }
        public Photo[] photos { get; set; }
        public object[] restaurants { get; set; }
        public int is_valid { get; set; }
        public string share_url { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string display_time { get; set; }
        public string display_date { get; set; }
        public int is_end_time_set { get; set; }
        public string disclaimer { get; set; }
        public int event_category { get; set; }
        public string event_category_name { get; set; }
        public string book_link { get; set; }
    }
    [Serializable]
    public class Photo
    {
        public Photo1 photo { get; set; }
    }
    [Serializable]
    public class Photo1
    {
        public string url { get; set; }
        public string thumb_url { get; set; }
        public int order { get; set; }
        public string md5sum { get; set; }
        public int photo_id { get; set; }
        public long uuid { get; set; }
        public string type { get; set; }
    }



    [Serializable]
    public class SpecificSearch
    {
        public int results_found { get; set; }
        public int results_start { get; set; }
        public int results_shown { get; set; }
        public Restaurant2[] restaurants { get; set; }
    }
    [Serializable]
    public class Restaurant2
    {
        public Restaurant1 restaurant { get; set; }
    }
    [Serializable]
    public class Restaurant1
    {
        public R R { get; set; }
        public string apikey { get; set; }
        public string id { get; set; }
        public string name { get; set; }
        public string url { get; set; }
        public Location2 location { get; set; }
        public string cuisines { get; set; }
        public int average_cost_for_two { get; set; }
        public int price_range { get; set; }
        public string currency { get; set; }
        public object[] offers { get; set; }
        public string thumb { get; set; }
        public User_Rating user_rating { get; set; }
        public string photos_url { get; set; }
        public string menu_url { get; set; }
        public string featured_image { get; set; }
        public int has_online_delivery { get; set; }
        public int is_delivering_now { get; set; }
        public string deeplink { get; set; }
        public int has_table_booking { get; set; }
        public string events_url { get; set; }
        public object[] establishment_types { get; set; }
    }

    //public class R
    //{
    //    public int res_id { get; set; }
    //}
    [Serializable]
    public class Location2
    {
        public string address { get; set; }
        public string locality { get; set; }
        public string city { get; set; }
        public int city_id { get; set; }
        public string latitude { get; set; }
        public string longitude { get; set; }
        public string zipcode { get; set; }
        public int country_id { get; set; }
    }

    //public class User_Rating
    //{
    //    public string aggregate_rating { get; set; }
    //    public string rating_text { get; set; }
    //    public string rating_color { get; set; }
    //    public string votes { get; set; }
    //}
}