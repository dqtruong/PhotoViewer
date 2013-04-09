
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.IO;

namespace PhotoViewer.MediaViewer
{
    /// <summary>
    /// Knowns how to display an IThumbnailedImage, picking the thumbnail or 
    /// full resolution image to display based on the container size.  The
    /// IThumbnailedImage to display should be assigned to the DataContext
    /// property.
    /// </summary>
    public class ThumbnailedImageViewer : Control
    {
        private enum ImageBindingState { ScreenSizeThumbnail, FullSizePhoto }
        private ImageBindingState _imageBindingState = ImageBindingState.ScreenSizeThumbnail;
        private Image _image = null;
        private FrameworkElement _placeholder = null;

        private BitmapImage _thumbnailBitmapImage = null;
        private ImageSource _thumbnailImageSource = null;
        internal BitmapImage _fullResolutionBitmapImage = null;
        internal ImageSource _fullResolutionImageSource = null;
        private Stream _thumbnailStream = null;
        private Stream _fullResolutionStream = null;

        public ThumbnailedImageViewer()
        {
            DefaultStyleKey = typeof(ThumbnailedImageViewer);

            // Register for DataContext change notifications
            //
            DependencyProperty dataContextDependencyProperty = System.Windows.DependencyProperty.RegisterAttached("DataContextProperty", typeof(object), typeof(FrameworkElement), new System.Windows.PropertyMetadata(OnDataContextPropertyChanged));
            SetBinding(dataContextDependencyProperty, new Binding());

        }

        protected override Size MeasureOverride(Size availableSize)
        {
            if (DataContext is IThumbnailedImage)
            {
                if ((_imageBindingState == ImageBindingState.ScreenSizeThumbnail) &&
                    (_image.Visibility == System.Windows.Visibility.Visible) &&      // make sure the image is loaded before measuring its size
                    (CurrentImageSizeIsTooSmall(availableSize)))
                {
                    BeginLoadingFullResolutionImage();
                }
            }

            return base.MeasureOverride(availableSize);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _image = GetTemplateChild("Image") as Image;
            _placeholder = GetTemplateChild("Placeholder") as FrameworkElement;

            ShowPlaceholder();
        }

        private static void OnDataContextPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ThumbnailedImageViewer mediaItemViewer = (ThumbnailedImageViewer)d;

            IThumbnailedImage newPhoto = e.NewValue as IThumbnailedImage;

            mediaItemViewer.ClearImageSources();

            if (e.NewValue != null)
            {
                mediaItemViewer.ShowPlaceholder();
                mediaItemViewer.BeginLoadingThumbnail();
            }
            else
            {
                mediaItemViewer._image.Source = null;
            }
        }

        private void OnThumbnailOpened(object sender, EventArgs e)
        {
            _image.Source = null;
            _image.Source = _thumbnailImageSource;

            ClearImageSources();

            HidePlaceholder();
            InvalidateMeasure();
        }

        private void OnFullSizeImageOpened(object sender, EventArgs e)
        {
            _image.Source = null;
            _image.Source = _fullResolutionImageSource;

            ClearImageSources();

            HidePlaceholder();
            InvalidateMeasure();
        }

        private void ClearImageSources()
        {
            if (_thumbnailBitmapImage != null)
            {
                if(_thumbnailStream!=null)
                    _thumbnailStream.Dispose();
               
                _thumbnailStream = null;
                _thumbnailBitmapImage.ImageOpened -= OnThumbnailOpened;
            }
            _thumbnailBitmapImage = null;

            if (_fullResolutionBitmapImage != null)
            {
                if(_fullResolutionStream!=null)
                    _fullResolutionStream.Dispose();
              
                _fullResolutionStream = null;
                _fullResolutionBitmapImage.ImageOpened -= OnFullSizeImageOpened;
                _fullResolutionBitmapImage.ImageOpened -= OnFullSizeImageOpened;
            }
            if (_fullResolutionBitmapImage != null)
                //_fullResolutionBitmapImage.UriSource = null;
            _fullResolutionBitmapImage = null;

            _thumbnailImageSource = null;
            _fullResolutionImageSource = null;
        }

        private bool CurrentImageSizeIsTooSmall(Size availableSize)
        {
            BitmapImage source = _image.Source as BitmapImage;

            if (source == null)
            {
                return true;
            }

            bool toReturn = ((source.PixelWidth < availableSize.Width) && (source.PixelHeight < availableSize.Height));

            if (toReturn)
            {
                //Tracing.Trace("MediaItemViewer.CurrentImageSizeIsTooSmall() - switching from thumbnail to full res photo because the thumbnail is too small (" + source.PixelWidth + ", " + source.PixelHeight + ") for the available size (" + availableSize + ")");
            }

            return toReturn;
        }

        private void BeginLoadingThumbnail()
        {
            if (DataContext is IThumbnailedImage == false)
            {
                return;
            }

            //Tracing.Trace("MediaItemViewer.BeginLoadingThumbnail()");

            if (_thumbnailBitmapImage != null)
            {
                _thumbnailBitmapImage.ImageOpened -= OnThumbnailOpened;
            }
            _thumbnailBitmapImage = null;

            _thumbnailBitmapImage = new BitmapImage();
            _thumbnailBitmapImage.ImageOpened += OnThumbnailOpened;
            _thumbnailBitmapImage.CreateOptions = BitmapCreateOptions.BackgroundCreation;
            //_thumbnailStream = ((IThumbnailedImage)DataContext).GetThumbnailImage();
            //_thumbnailBitmapImage.SetSource(_thumbnailStream);
            _thumbnailBitmapImage.SetSource(((IThumbnailedImage)DataContext).GetThumbnailImage());
            _thumbnailImageSource = _thumbnailBitmapImage;
            _imageBindingState = ImageBindingState.ScreenSizeThumbnail;
        }

        private void BeginLoadingFullResolutionImage()
        {
            if (DataContext is IThumbnailedImage == false)
            {
                return;
            }
            
            //Tracing.Trace("MediaItemViewer.BeginLoadingFullResolutionImage()");

            if (_fullResolutionBitmapImage != null)
            {
                _fullResolutionBitmapImage.ImageOpened -= OnFullSizeImageOpened;
            }
            _fullResolutionBitmapImage = null;

            _fullResolutionBitmapImage = new BitmapImage();
            _fullResolutionBitmapImage.ImageOpened += OnFullSizeImageOpened;
            _fullResolutionBitmapImage.CreateOptions = BitmapCreateOptions.BackgroundCreation;
            _fullResolutionStream = ((IThumbnailedImage)DataContext).GetImage();
            _fullResolutionBitmapImage.SetSource(_fullResolutionStream);
            _fullResolutionStream.Dispose();
            GC.Collect();
            //_fullResolutionBitmapImage.SetSource(((IThumbnailedImage)DataContext).GetImage());
            _fullResolutionImageSource = _fullResolutionBitmapImage;
            _imageBindingState = ImageBindingState.FullSizePhoto;
        }

        private void ShowPlaceholder()
        {
            if (_placeholder != null)
            {
                _placeholder.Visibility = System.Windows.Visibility.Visible;
            }
            if (_image != null)
            {
                _image.Visibility = System.Windows.Visibility.Collapsed;
            }
        }

        private void HidePlaceholder()
        {
            if (_image != null)
            {
                _image.Visibility = System.Windows.Visibility.Visible;
            }
            if (_placeholder != null)
            {
                _placeholder.Visibility = System.Windows.Visibility.Collapsed;
            }
        }

        internal void ClearImage()
        {
            ClearImageSources();
            if(_image!=null)
            _image.Source = null;
            _placeholder = null;
            if (_thumbnailBitmapImage != null)
                _thumbnailBitmapImage.UriSource = null;
            _thumbnailBitmapImage = null;
            _thumbnailImageSource = null;
            if (_fullResolutionBitmapImage != null)
                _fullResolutionBitmapImage.UriSource = null;
            _fullResolutionBitmapImage = null;
            _fullResolutionImageSource = null;
            _thumbnailStream = null;
            _fullResolutionStream = null;
            _image = null;
        }

        private void CheckMemory(string str)
        {
            var mem = string.Format("{0:N}", Microsoft.Phone.Info.DeviceStatus.ApplicationCurrentMemoryUsage / 1024/ 1024);
            System.Diagnostics.Debug.WriteLine(str +"£º"+ mem+" MB\n");
        }
    }
}
