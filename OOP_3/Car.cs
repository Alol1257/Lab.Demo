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
using System.Threading;
using System.Windows.Threading;

namespace OOP_3
{
    public class Car : MapObject
    {
        private PointLatLng point;
        private Route route;
        private List<Human> passengers;
        private GMapMarker marker;

        public event EventHandler Follow;
        public event EventHandler Arrived;

        public Car(string title, PointLatLng point) : base(title)
        {
            this.point = point;
            //passengers.Clear();
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
                    Source = new BitmapImage(new Uri("C:/Users/user/Desktop/Lab.Demo/OOP_3/icons/Car.png")) // путь к пикче, но из за этого на др пк не работает из за разных путей
                }
            };

            return marker;
        }

        public GMapMarker moveTo(PointLatLng endPoint)
        {
            RoutingProvider routingProvider = GMapProviders.OpenStreetMap;
            
            MapRoute route = routingProvider.GetRoute(
            point, 
            endPoint, 
            false, 
            false, 
            (int)15);

            List<PointLatLng> routePoints = route.Points;

            this.route = new Route("",routePoints);

            return this.route.getMarker();
        }

        private void moveByRoute() //100 проц неправильно
        {
            foreach (var point in route.getPoints())
            {
                Application.Current.Dispatcher.Invoke(delegate
                {
                    marker.Position = point;
                    this.point = point;

                    foreach (Human pass in passengers)
                    {
                        if (pass != null)
                        {
                            pass.setPosition(point);
                            pass.marker.Position = point;
                        }

                        Follow?.Invoke(this, null);

                        Thread.Sleep(500);

                        if (pass == null)
                        {
                            Arrived?.Invoke(this, null);
                        }
                        else
                        {
                            Human passenger = pass;
                            passengers = null;
                        }
                    }
                    
                });
                

            }
        }

        public void passSeated(object sender, EventArgs e)
        {
            passengers = (List<Human>)sender;
            
            foreach (Human pass in passengers)
            {
                Application.Current.Dispatcher.Invoke(delegate { moveTo(pass.getDestination()); });
            }
        }
    }
}
