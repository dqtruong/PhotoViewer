
using System.Collections.Generic;

namespace PhotoViewer.MediaViewer
{
    /// <summary>
    /// Compares two IThumbnailedImage instances by DateTaken
    /// </summary>
    public class ThumbnailedImageDateComparer : IComparer<IThumbnailedImage>
    {
        public ThumbnailedImageDateComparer()
        {
        }

        /// <summary>
        /// Compare two IThumbnailedImage instances by DateTaken
        /// </summary>
        /// <param name="x">First IThumbnailedImage to examine</param>
        /// <param name="y">IThumbnailedImage to compare to the first IThumbnailedImage</param>
        /// <returns></returns>
        public int Compare(IThumbnailedImage x, IThumbnailedImage y)
        {
            return x.DateTaken.CompareTo(y.DateTaken);
        }
    }
}
