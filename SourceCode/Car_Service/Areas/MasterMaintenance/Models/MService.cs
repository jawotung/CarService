using System.Collections.Generic;

namespace CarService.Areas.MasterMaintenance.Models
{
    public class MService
    {
        public int ID { get; set; }
        public string ServiceName	 { get; set; }
        public string Duration { get; set; }
        public string DurationTo { get; set; }
        public string DurationFrom { get; set; }
        public string Amount { get; set; }
        public int Position { get; set; }
        public string PositionName { get; set; }
    }
}
