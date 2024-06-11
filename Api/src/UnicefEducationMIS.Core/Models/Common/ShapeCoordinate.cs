namespace UnicefEducationMIS.Core.Models.Common
{
    public class ShapeCoordinate : Coordinate
    {
        public int SequenceNo { get; set; }

        public ShapeCoordinate()
        {

        }
        public ShapeCoordinate(decimal lat, decimal lng) : base(lat, lng)
        {
        }

        public ShapeCoordinate(string lat, string lng) : base(lat, lng)
        {
        }
    }
}