using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PruebasHbeds.Models.ViewModels
{
    public class HotelViewModel
    {
        public int Code { get; set; }
        public string Name { get; set; }
        public string CountryCode { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public string Web { get; set; }
        public string Email { get; set; }

    }
}