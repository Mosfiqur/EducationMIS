using Catfood.Shapefile;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnicefEducationMIS.ShapeFileImport2
{
    class Program
    {
        static void Main(string[] args)
        {
            var campRepository = new CampRepository();
            var shapeFileService = new ShapeFileService();
            var campNotFoundList = new List<string>();

            //campRepository.TrancateCampCoordinate();

            var fullDirectory = shapeFileService.ShapeFilePathCheck();

            var camps = campRepository.GetCamps();

            using (Shapefile shapefile = new Shapefile(fullDirectory))
            {
                shapeFileService.ShapeFileInitialDataPrint(shapefile);

                // enumerate all shapes
                foreach (Shape shape in shapefile)
                {
                    var campCoordinate = new CampCoordinate();

                    Console.WriteLine("----------------------------------------");
                    Console.WriteLine("Shape {0:n0}, Type {1}", shape.RecordNumber, shape.Type);
                    string[] metadataNames = shape.GetMetadataNames();
                    string campSSID = null;
                    if (metadataNames != null)
                    {
                        campSSID = shape.GetMetadata(metadataNames[0]);
                    }
                    
                    shapeFileService.ShapeMetaDataPrint(shape, camps, campCoordinate);
                    
                    if(!(campCoordinate.CampId == 0))
                    {
                        campRepository.DeleteParticularCampData(campCoordinate.CampId);
                        shapeFileService.EnumeratingAllShapes(shape, campCoordinate, campRepository);
                    }
                    else
                    {
                        campNotFoundList.Add($"{campSSID} Not Found");
                    }

                    Console.WriteLine("----------------------------------------");
                    Console.WriteLine();
                }
            }
            shapeFileService.WriteToJsonFile();

            campNotFoundList.ForEach(item => Console.WriteLine(item));
            
            Console.WriteLine("Done");
            Console.ReadKey();
            Console.WriteLine();
            campRepository.Dispose();
        }
    }
}
