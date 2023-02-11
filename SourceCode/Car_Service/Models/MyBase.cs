namespace CarService.Models
{
    public class MyBase
    {
        public int CreateID { get; set; }
        public string CreateDate { get; set; }
        public string CreateUser { get; set; }
        public int UpdateID { get; set; }
        public string UpdateDate { get; set; }
        public string UpdateUser { get; set; }
        public int IsDeleted { get; set; }
    }
}