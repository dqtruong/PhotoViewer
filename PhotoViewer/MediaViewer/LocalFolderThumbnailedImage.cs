
using System;
using System.IO;
using System.IO.IsolatedStorage;

namespace PhotoViewer.MediaViewer
{
    public class LocalFolderThumbnailedImage: IThumbnailedImage
    {
        /// <summary>
        /// The path and file name of the full resolution image file.
        /// </summary>
        public string ImageFileName { get; private set; }
        /// <summary>
        /// The path and file name of the thumbnail resolution image file.
        /// </summary>
        public string ThumbnailFileName { get; private set; }

        private DateTime? _dateTaken = null;

        public LocalFolderThumbnailedImage(string imageFileName, string thumbnailFileName)
        {
            ImageFileName = imageFileName;
            ThumbnailFileName = thumbnailFileName;
        }

        /// <summary>
        /// Returns a Stream representing the thumbnail image.
        /// </summary>
        /// <returns>Stream of the thumbnail image.</returns>
        public Stream GetThumbnailImage()
        {
            Stream thumbnailFileStream = null;
            using (IsolatedStorageFile store = IsolatedStorageFile.GetUserStoreForApplication())
            {
                try
                {

                    thumbnailFileStream = store.OpenFile(
                        ThumbnailFileName,
                        FileMode.Open,
                        FileAccess.Read,
                        FileShare.Delete | FileShare.Read);

                    thumbnailFileStream.Seek(0, SeekOrigin.Begin);
                }
                catch
                {

                }
                store.Dispose();
            }
            
   
            return thumbnailFileStream;
        }

        /// <summary>
        /// Returns a Stream representing the full resolution image.
        /// </summary>
        /// <returns>Stream of the full resolution image.</returns>
        public Stream GetImage()
        {

            Stream imageFileStream = null;
           
                using (IsolatedStorageFile store = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    try
                    {
                        imageFileStream = store.OpenFile(
                            ImageFileName,
                            FileMode.Open,
                            FileAccess.Read,
                            FileShare.Delete | FileShare.Read);
                        imageFileStream.Seek(0, SeekOrigin.Begin);
                    }
                    catch
                    {

                    }
                    store.Dispose();
            }

            return imageFileStream;
        }

        /// <summary>
        /// Represents the time the photo was taken, useful for sorting photos.
        /// </summary>
        public DateTime DateTaken
        {
            get
            {
                if (_dateTaken == null)
                {
                    using (IsolatedStorageFile store = IsolatedStorageFile.GetUserStoreForApplication())
                    {
                        _dateTaken = store.GetCreationTime(ImageFileName).DateTime;
                    }
                }

                return _dateTaken.Value;
            }
        }
    }
}
