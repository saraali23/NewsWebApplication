namespace NewsWebApp.Helpers
{
    public static class ImageTransformationHelper
    {
        public static string StringFromByteArray(byte[] bytes)
        {
            string url = "";
            if (bytes != null && bytes.Length > 0)
            {
                string img = Convert.ToBase64String(bytes, 0, bytes.Length);
                url = "data:image/jpeg;base64," + img;
            }
            if (bytes == null)
            {
                url = "~/NoImage.png";
            }
            return url;
        }

        public static byte[]? IFormImageToByteArray(IFormFile image)
        {
            byte[]? imageArr = null;
            using (var memoryStream = new MemoryStream())
            {
                if (image != null)
                {
                    image.CopyTo(memoryStream);

                    // Upload the file if less than 2 MB
                    if (memoryStream.Length < 2097152)
                    {
                        imageArr = memoryStream.ToArray();
                    }
                    else
                    {
                        throw new Exception("File is too large");
                    }
                }
                return imageArr;
                

            }

        }
    }
}
