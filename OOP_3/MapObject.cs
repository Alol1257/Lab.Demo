using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.WindowsPresentation;
using System.Device.Location;

namespace OOP_3
{
    public abstract class MapObject
    {
        private string title;

        private DateTime crationDate;

        protected MapObject(string title)
        {
            this.title = title;
            crationDate = DateTime.Now;
        }

        public string getTitle()
        {
            return title;
        }

        public DateTime getCreationDate()
        {
            return crationDate;
        }

        public abstract double getDistance(PointLatLng point);

        public abstract PointLatLng getFocus();

        public abstract GMapMarker getMarker();
    }
}
