using Catfood.Shapefile;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnicefEducationMIS.ShapeFileImport2
{
    public class ShapeFileService
    {
        private List<CampCoordinate> _campCoordinates = new List<CampCoordinate>();
        public string ShapeFilePathCheck()
        {
            var fileName = ConfigurationManager.AppSettings.Get("fileName");
            string shapeFilePath = @"ShapeFiles\"+fileName;
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            var fullDirectory = baseDirectory + shapeFilePath;

            // Pass the path to the shapefile in as the command line argument
            if (!File.Exists(fullDirectory))
            {
                Console.WriteLine("");
                throw new FileNotFoundException("<shapefile with extension .shp> File not found");
            }
            return fullDirectory;
        }

        public void EnumeratingAllShapes(Shape shape,CampCoordinate campCoordinate,CampRepository campRepository)
        {
            switch (shape.Type)
            {
                case ShapeType.Point:
                    // a point is just a single x/y point
                    ShapePoint shapePoint = shape as ShapePoint;
                    campCoordinate.Longitude = Convert.ToDouble(shapePoint.Point.X);
                    campCoordinate.Latitude = Convert.ToDouble(shapePoint.Point.Y);
                    //campRepository.InsertCampCoordinate(campCoordinate);
                    _campCoordinates.Add(new CampCoordinate()
                    {
                        CampId = campCoordinate.CampId,
                        Longitude = campCoordinate.Longitude,
                        Latitude = campCoordinate.Latitude
                    });

                    Console.WriteLine("Point={0},{1}", shapePoint.Point.X, shapePoint.Point.Y);
                    break;

                case ShapeType.Polygon:
                    // a polygon contains one or more parts - each part is a list of points which
                    // are clockwise for boundaries and anti-clockwise for holes 
                    // see http://www.esri.com/library/whitepapers/pdfs/shapefile.pdf
                    var count = 0;
                    ShapePolygon shapePolygon = shape as ShapePolygon;
                    foreach (PointD[] part in shapePolygon.Parts)
                    {
                        Console.WriteLine("Polygon part:");
                        foreach (PointD point in part)
                        {
                            campCoordinate.Longitude = Convert.ToDouble(point.X);
                            campCoordinate.Latitude = Convert.ToDouble(point.Y);

                            //campRepository.InsertCampCoordinate(campCoordinate);
                            _campCoordinates.Add(new CampCoordinate() { 
                                CampId = campCoordinate.CampId,
                                Longitude = campCoordinate.Longitude,
                                Latitude = campCoordinate.Latitude,
                                SequenceNumber = ++count
                            });
                            
                            Console.WriteLine("{0}, {1}", point.X, point.Y);
                        }
                        Console.WriteLine();
                    }

                    break;

                default:
                    break;
            }
        }

        public void WriteToJsonFile()
        {
            var path = AppDomain.CurrentDomain.BaseDirectory + "campCoordinate.json";// @"D:\Kaz Software Office Work\Unicef API\unicefapi\src\UnicefEducationMIS.Web\wwwroot\data\campCoordinate.json";
            
            var campCoordinateJson = JsonConvert.SerializeObject(_campCoordinates, Formatting.Indented);
            File.WriteAllText(path, campCoordinateJson);
            Console.WriteLine(_campCoordinates.Count() +" Total Data Imported to JSON File");
        }

        public void ShapeMetaDataPrint(Shape shape,List<Dictionary<string,object>> camps,
            CampCoordinate campCoordinate)
        {
            // each shape may have associated metadata
            string[] metadataNames = shape.GetMetadataNames();
            if (metadataNames != null)
            {
                string campSSID = shape.GetMetadata(metadataNames[0]);
                var camp = camps.FirstOrDefault(x => x.ContainsKey("SSID") && x.ContainsValue(campSSID));
                if (camp != null)
                {
                    var filter = camp.Select(x => new { x.Key, x.Value });
                    campCoordinate.CampId = Convert.ToInt32(filter.ElementAt(0).Value);
                }

                Console.WriteLine("Metadata:");
                foreach (string metadataName in metadataNames)
                {
                    Console.WriteLine("{0}={1} ({2})", metadataName, shape.GetMetadata(metadataName), shape.DataRecord.GetDataTypeName(shape.DataRecord.GetOrdinal(metadataName)));
                }
                Console.WriteLine();
            }
        }

        public void ShapeFileInitialDataPrint(Shapefile shapefile)
        {
            // a shapefile contains one type of shape (and possibly null shapes)
            Console.WriteLine("Type: {0}, Shapes: {1:n0}", shapefile.Type, shapefile.Count);

            // a shapefile also defines a bounding box for all shapes in the file
            Console.WriteLine("Bounds: {0},{1} -> {2},{3}",
                shapefile.BoundingBox.Left,
                shapefile.BoundingBox.Top,
                shapefile.BoundingBox.Right,
                shapefile.BoundingBox.Bottom);
            Console.WriteLine();
        }
    }
}
