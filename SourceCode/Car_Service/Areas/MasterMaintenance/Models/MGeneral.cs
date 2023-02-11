namespace CarService.Areas.MasterMaintenance.Models
{
    public class MGeneral
    {
        public int ID { get; set; }
        public int TypeID { get; set; }
        public string TypeDesc { get; set; }
        public string Value { get; set; }
        public string UpdateDate { get; set; }
    }
    public class MTypes
    {
        public int ID { get; set; }
        public string Type { get; set; }
        public string UpdateDate { get; set; }
    }
}
