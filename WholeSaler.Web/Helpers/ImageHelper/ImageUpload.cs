namespace WholeSaler.Web.Helpers.ImageHelper
{
    public class ImageUpload
    {
        public static string ImageChangeName(string imageName)
        {



            string newFileName = "";

            string uniqueName = Guid.NewGuid().ToString();

            var fileArray = imageName.Split('.');

            var extension = fileArray[fileArray.Length - 1];

            if (extension == "png" || extension == "jpg" || extension == "jpeg")
            {

                newFileName = uniqueName + "." + extension;

                return newFileName;
            }
            else
            {
                return "0";
            }


        }
    }
}
