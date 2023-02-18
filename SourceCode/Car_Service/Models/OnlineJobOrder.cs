
namespace CarService.Models
{
    public class OnlineJobOrder
    {
        public int ID { get; set; }
        public string UserID { get; set; }
        public string Password { get; set; }
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
    }
}