namespace BakeX_WebAPI.Models
{
  public class Business
{
    public int PostedById { get; set; }
    public string BusinessAddress { get; set; }
    public byte[] ProfileImage { get; set; }
    public string BusinessName { get; set; }

   public string ProfileImageBase64 { get; set; }


}

}
