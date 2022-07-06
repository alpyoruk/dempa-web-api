namespace CarSalesAPI.Models
{
    public class CarDetailsModel
    {
        public int ID { get; set; }
        public int BrandID { get; set; }
        public int SeriesID { get; set; }
        public int ModelID { get; set; }
        public int Year { get; set; }
        public int FuelID { get; set; }
        public int GearID { get; set; }
        public string Kilometer { get; set; }
        public int CaseID { get; set; }
        public int Power { get; set; }
        public int Capacity { get; set; }
        public int TractionID { get; set; }
        public string Color { get; set; }
        public int PlateID { get; set; }
        public int CarID { get; set; }

        public string BrandName;
        public string SeriesName;
        public string ModelName;
        public string FuelName;
        public string GearName;
        public string CaseName;
        public string TractionName;
        public string PlateName;
    }
}