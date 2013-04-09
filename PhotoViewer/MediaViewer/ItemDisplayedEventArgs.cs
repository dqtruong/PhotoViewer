
using System;

namespace PhotoViewer.MediaViewer
{
    /// <summary>
    /// Represents the index into the Items collection currently displayed by a MediaViewer.
    /// </summary>
    public class ItemDisplayedEventArgs : EventArgs
    {
        public int ItemIndex { get; private set; }

        public ItemDisplayedEventArgs(int itemIndex)
        {
            ItemIndex = itemIndex;
        }
    }
}
