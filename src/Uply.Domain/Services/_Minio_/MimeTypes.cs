using System.Net.Mime;

namespace Infrastructure.Minio;

public static class MimeTypes
{
    public static readonly string[] ImageTypes =
    [
        MediaTypeNames.Image.Avif,
        MediaTypeNames.Image.Bmp,
        MediaTypeNames.Image.Gif,
        MediaTypeNames.Image.Icon,
        MediaTypeNames.Image.Png,
        MediaTypeNames.Image.Jpeg,
        MediaTypeNames.Image.Svg,
        MediaTypeNames.Image.Webp
    ];
}
