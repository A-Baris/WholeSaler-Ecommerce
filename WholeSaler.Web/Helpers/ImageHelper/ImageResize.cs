using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using System.IO;
using System.Threading.Tasks;
namespace WholeSaler.Web.Helpers.ImageHelper
{
    public class ImageResize
    {
        public static async Task ResizeImage(Stream sourceStream, Stream destinationStream, int desiredWidth, int desiredHeight)
        {
            using (Image image = await Image.LoadAsync(sourceStream))
            {
                image.Mutate(x => x.Resize(new ResizeOptions
                {
                    Size = new Size(desiredWidth, desiredHeight),
                    Mode = ResizeMode.Max // or other resize mode as per your requirement
                }));

                // Save the resized image to the destination stream
                await image.SaveAsync(destinationStream, new SixLabors.ImageSharp.Formats.Jpeg.JpegEncoder());
            }
        }

    }
}
