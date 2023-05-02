using ArcGIS.Core.CIM;
using ArcGIS.Core.Data;
using ArcGIS.Core.Geometry;
using ArcGIS.Desktop.Catalog;
using ArcGIS.Desktop.Core;
using ArcGIS.Desktop.Editing;
using ArcGIS.Desktop.Extensions;
using ArcGIS.Desktop.Framework;
using ArcGIS.Desktop.Framework.Contracts;
using ArcGIS.Desktop.Framework.Dialogs;
using ArcGIS.Desktop.Framework.Threading.Tasks;
using ArcGIS.Desktop.Layouts;
using ArcGIS.Desktop.Mapping;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StreetPictoViewPro
{
    internal class StreetView : MapTool
    {
        protected override void OnToolMouseDown(MapViewMouseButtonEventArgs e)
        {
            if (e.ChangedButton == System.Windows.Input.MouseButton.Left)
                e.Handled = true; //Handle the event args to get the call to the corresponding async method
        }

        protected internal string Keytip { get; }

        protected override Task HandleMouseDownAsync(MapViewMouseButtonEventArgs e)
        {
            return QueuedTask.Run(() =>
            {
                // Convert the clicked point in client coordinates to the corresponding map coordinates.
                MapPoint mapPoint = MapView.Active.ClientToMap(e.ClientPoint);

                // convert to lon/lat
                MapPoint coords = (MapPoint)GeometryEngine.Instance.Project(mapPoint, SpatialReferences.WGS84);

                // open a web browser
                string url = string.Format("http://maps.google.com/?cbll={0},{1}&cbp=12,0,0,0,5&layer=c", coords.Y, coords.X);
                Process.Start(new ProcessStartInfo(url) { UseShellExecute = true});

                FrameworkApplication.SetCurrentToolAsync("esri_mapping_exploreTool");
            });
        }
    }
}

