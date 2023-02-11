using System.Collections.Generic;

namespace CarService.Areas.MasterMaintenance.Models
{
    public class MWorker
    {
        public int ID { get; set; }
        public string WorkerID	 { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public string Password { get; set; }
        public int Position { get; set; }
        public string Gender { get; set; }
        public string GenderName { get; set; }
        public string Email { get; set; }
        public string ContactNo { get; set; }
        public string PositionName { get; set; }
    }
    public class MWorkerData
    {
        public List<MWorker> Data { get; set; }
    }
}
