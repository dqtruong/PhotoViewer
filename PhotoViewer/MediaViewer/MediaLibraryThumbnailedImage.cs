
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Media.PhoneExtensions;
using System;
using System.IO;

namespace PhotoViewer.MediaViewer
{
    public class MediaLibraryThumbnailedImage : IThumbnailedImage
    {
        /// <summary>
        /// The Picture object that this instance represents.
        /// </summary>
        public Picture Picture { get; private set; }

        public MediaLibraryThumbnailedImage(Picture picture)
        {
            this.Picture = picture;
        }

        /// <summary>
        /// Returns a Stream representing the thumbnail image.
        /// </summary>
        /// <returns>Stream of the thumbnail image.</returns>
        public Stream GetThumbnailImage()
        {
            return this.Picture.GetPreviewImage();
        }

        /// <summary>
        /// Returns a Stream representing the full resolution image.
        /// </summary>
        /// <returns>Stream of the full resolution image.</returns>
        public Stream GetImage()
        {
            return this.Picture.GetImage();
        }

        /// <summary>
        /// Represents the time the photo was taken, useful for sorting photos.
        /// </summary>
        public DateTime DateTaken
        {
            get
            {
                return this.Picture.Date;
            }
        }
    }
}
