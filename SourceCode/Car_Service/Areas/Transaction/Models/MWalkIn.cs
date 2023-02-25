using System.Collections.Generic;

namespace CarService.Areas.Transaction.Models
{
    public class MWalkIn
    {
        public int ID { get; set; }
        public int JODetailID { get; set; }
        public string JONo { get; set; }
        public string UserID { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string ContactNo { get; set; }
        public string EmailAddress { get; set; }
        public string Type { get; set; }
        public string ServiceID { get; set; }
        public string Startdate { get; set; }
        public string Enddate { get; set; }
        public string Remarks { get; set; }
        public string ServiceName { get; set; }
        public string Worker { get; set; }
        public int WorkerID { get; set; }
        public string Price { get; set; }

    }
    public class MJO_Detail
    {
        public int ServiceID { get; set; }
        public double Price { get; set; }
    }
}
