using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.WindowsPresentation;
using System.Device.Location;

namespace OOP_3
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>

    public partial class MainWindow : Window
    {
        private List<MapObject> objects = new List<MapObject>();
        private List<MapObject> objectsCopy = new List<MapObject>();
        private List<PointLatLng> points = new List<PointLatLng>();
        private PointLatLng point;

        //private Human human;
        //private Car car;

        private void MapLoaded(object sender, RoutedEventArgs e)
        {
            GMaps.Instance.Mode = AccessMode.ServerAndCache;
            Map.MapProvider = OpenStreetMapProvider.Instance;

            Map.MinZoom = 2;
            Map.MaxZoom = 17;
            Map.Zoom = 15;

            Map.Position = new PointLatLng(55.012823, 82.950359);

            Map.MouseWheelZoomType = MouseWheelZoomType.MousePositionAndCenter;
            Map.CanDragMap = true;
            Map.DragButton = MouseButton.Right;
        }

        private void Map_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            point = Map.FromLocalToLatLng((int)e.GetPosition(Map).X, (int)e.GetPosition(Map).Y);

            points.Add(point);

            foreach (MapObject obj in objectsCopy)
            {
                if (objSearch.IsChecked == true && obj.getDistance(point) <= 500)
                {
                    searchResult.Items.Add(obj.getTitle());
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            switch (objSelect.SelectedIndex)
            {
                case 0:
                    Location location = new Location(objName.Text, point);
                    objects.Add(location);
                    break;
                case 1:
                    Car car = new Car(objName.Text, point);
                    objects.Add(car);
                    break;
                case 2:
                    Human human = new Human(objName.Text, point);
                    objects.Add(human);
                    break;
                case 3:
                    Route route = new Route(objName.Text, points);
                    objects.Add(route);
                    break;
                case 4:
                    Area area = new Area(objName.Text, points);
                    objects.Add(area);
                    break;
            }

            //Удаляем для адекватной отрисовки маршрута и области, ибо херня при неудаленных поинтах отрисуется
            points.Clear(); 

            if (objectCreate.IsChecked == true)
            {
                foreach (MapObject obj in objects)
                {
                    Map.Markers.Add(obj.getMarker());
                    objectsCopy.Add(obj);
                }
            }

            //Коприуем лист обЪектов в другой лист, а старый удаляем, иначе области адекватно рисоваться не будут, а лист еще пригодится для реализации поиска
            objects.Clear(); 
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Map.Markers.Clear();
            objectsCopy.Clear();
            searchResult.Items.Clear();
            searchBox.Clear();
            objName.Clear();
        }

        //Фокусировка будет на последнем созданном объекте, если тайтлы нескольких обьектов совпадают
        private void Search_Apply_Click(object sender, RoutedEventArgs e)
        {
            foreach (MapObject obj in objectsCopy)
            {
                if (searchBox.Text == obj.getTitle())
                {
                    searchResult.Items.Add(obj.getTitle());
                    Map.Position = obj.getFocus();
                }
            }
        }

        private void searchResult_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            foreach (MapObject obj in objectsCopy)
            {
                if (objectsCopy.IndexOf(obj) == searchResult.SelectedIndex)
                    Map.Position = obj.getFocus();
            }
        }
    }
}
