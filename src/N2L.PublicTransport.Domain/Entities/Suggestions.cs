using System;
using System.Collections.Generic;
using System.Text;

namespace N2L.PublicTransport.Domain.Entities
{
    public class Suggestions
    {
        public string ImageUrl { get; set; }
        public string Title { get; set; }
        public string DescriptionPlace { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public Suggestions(string imageUrl, string title, string descriptionPlace, double latitude, double longitude)
        {
            ImageUrl = imageUrl;
            Title = title;
            DescriptionPlace = descriptionPlace;
            Latitude = latitude;
            Longitude = longitude;
        }
    }
}
