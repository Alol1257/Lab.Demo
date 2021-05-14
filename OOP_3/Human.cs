using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.WindowsPresentation;
using System.Device.Location;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows;

namespace OOP_3
{
    class Human : MapObject
    {
        private PointLatLng point;
        private PointLatLng destinationPoint;
        public GMapMarker marker;

        private event EventHandler passSeated;

        public Human(string title, PointLatLng point) : base(title)
        {
            this.point = point;
        }

        public override double getDistance(PointLatLng point)
        {
            GeoCoordinate c1 = new GeoCoordinate(point.Lat, point.Lng);
            GeoCoordinate c2 = new GeoCoordinate(this.point.Lat, this.point.Lng);

            return c1.GetDistanceTo(c2);
        }

        public override PointLatLng getFocus()
        {
            return point;
        }

        public override GMapMarker getMarker()
        {
            GMapMarker marker = new GMapMarker(point)
            {
                Shape = new Image
                {
                    Width = 32,
                    Height = 32,
                    ToolTip = this.getTitle(),
                    Source = new BitmapImage(new Uri("C:/Users/user/Desktop/Lab.Demo/OOP_3/icons/Human.png")) // путь к пикче, но из за этого на др пк не работает из за разных путей
                }
            };

            return marker;
        }
        public void setPosition(PointLatLng point)
        {
            this.point = point;
        }


        public PointLatLng getDestination()
        {
            return destinationPoint;
        }

        public PointLatLng getPosition()
        {
            return point;
        }

        public void moveTo(PointLatLng point)
        {
            destinationPoint = point;
        }

        public void CarArrived(object sender, EventArgs e)
        {
            passSeated?.Invoke(this, EventArgs.Empty);
            MessageBox.Show("Машина прибыла.");
        }
    }
}
