using System;
using System.Device.Location;
using System.Globalization;

namespace IglaClub.Web.Helpers
{
    public class GeoCoordinateUtility
    {
        public GeoCoordinate ParseGeoCoordinate(string input)
        {
            if (String.IsNullOrEmpty(input))
            {
                throw new ArgumentException("input");
            }

            if (input == "Unknown")
            {
                return GeoCoordinate.Unknown;
            }

            // GeoCoordinate.ToString() uses InvariantCulture, so the doubles will use '.'
            // for decimal placement, even in european environments
            string[] parts = input.Split(',');

            if (parts.Length != 2)
            {
                throw new ArgumentException("Invalid format");
            }

            double latitude = Double.Parse(parts[0], CultureInfo.InvariantCulture);
            double longitude = Double.Parse(parts[1], CultureInfo.InvariantCulture);

            return new GeoCoordinate(latitude, longitude);
        }
    }
}