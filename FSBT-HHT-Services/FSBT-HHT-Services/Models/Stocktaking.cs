
namespace FSBT_HHT_Services.Models
{
    public class Stocktaking
    {
        public string StocktakingID { get; set; }
        public int ScanMode { get; set; }
        public string LocationCode { get; set; }
        public string Barcode { get; set; }
        public decimal Quantity { get; set; }
        public int UnitCode { get; set; }
        public string Flag { get; set; }
        public string Description { get; set; }
        public string SKUCode { get; set; }
        public string ExBarcode { get; set; }
        public string InBarcode { get; set; }
        public bool SKUMode { get; set; }
        public string HHTName { get; set; }
        public string HHTID { get; set; }
        public string DepartmentCode { get; set; }
        public string CreateBy { get; set; }
        public string UpdateBy { get; set; }
    }
}
