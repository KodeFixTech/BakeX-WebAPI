namespace BakeX_WebAPI.Models
{
    public class StateDistrict
    {
        public string StateName { get; set; }
        public IEnumerable<District> Districts { get; set; }
    }

   
}
