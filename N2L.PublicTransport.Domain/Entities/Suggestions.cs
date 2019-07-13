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

        public Suggestions(string imageUrl, string title, string descriptionPlace)
        {
            ImageUrl = imageUrl;
            Title = title;
            DescriptionPlace = descriptionPlace;
        }
    }
}
