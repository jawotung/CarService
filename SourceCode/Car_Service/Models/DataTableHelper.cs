namespace CarService.Models
{
    public class DataTableHelper
    {
        public object GetPropertyValue(object obj, string name)
        {
            return obj?.GetType()
            .GetProperty(name)
            .GetValue(obj, null);
        }
    }
}
