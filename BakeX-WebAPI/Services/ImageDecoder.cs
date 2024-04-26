namespace BakeX_WebAPI.Services
{
    public class ImageDecoder
    {

        public byte[] DecodeBase64Image (string base64Image)
        {
            if(base64Image.StartsWith("data:image"))
            {
                int commaIndex = base64Image.IndexOf (',');
                if(commaIndex != -1)
                {
                    base64Image = base64Image.Substring(commaIndex + 1);
                }
            }

            return Convert.FromBase64String (base64Image);
        }
    }
}
