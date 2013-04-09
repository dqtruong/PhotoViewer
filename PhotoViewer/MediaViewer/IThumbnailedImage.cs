
using System;
using System.IO;

namespace PhotoViewer.MediaViewer
{
    public interface IThumbnailedImage
    {
        Stream GetThumbnailImage();
        Stream GetImage();
        DateTime DateTaken { get; }
    }
}
