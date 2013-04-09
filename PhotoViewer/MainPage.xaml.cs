using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using PhotoViewer.Resources;
using Microsoft.Xna.Framework.Media;
using PhotoViewer.MediaViewer;
using System.Collections.ObjectModel;
using System.Windows.Media.Animation;

namespace PhotoViewer
{
    public partial class MainPage : PhoneApplicationPage
    {
        // 构造函数
        public MainPage()
        {
            InitializeComponent();
            this.Loaded += MainPage_Loaded;
            // 用于本地化 ApplicationBar 的示例代码
            //BuildLocalizedApplicationBar();
        }

        void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            this.OrientationChanged += MainPage_OrientationChanged;
            InitList();
        }

        void MainPage_OrientationChanged(object sender, OrientationChangedEventArgs e)
        {
            RotateIamgeViewer(e.Orientation);
        }

        private void InitList()
        {
            var media = new MediaLibrary();
            var list = media.Pictures;
            List<MediaLibraryThumbnailedImage> imgSources = new List<MediaLibraryThumbnailedImage>();
            foreach (var pic in list)
            {
                imgSources.Add(new MediaLibraryThumbnailedImage(pic));
            }
            mediaViewer.Items = new ObservableCollection<object>(imgSources);
            media.Dispose();
            media = null;
        }

        private void RotateIamgeViewer(PageOrientation orientation)
        {
            var duration = new Duration(TimeSpan.FromSeconds(0.5));
            var sb = new Storyboard();
            sb.Duration = duration;

            var da = new DoubleAnimation();
            da.Duration = duration;

            sb.Children.Add(da);
            //Storyboard.SetTarget(da, mediaViewerTranform);
            Storyboard.SetTarget(da, mediaViewerTransform);
            Storyboard.SetTargetProperty(da, new PropertyPath("Rotation"));

            if (orientation == PageOrientation.Landscape ||
                orientation == PageOrientation.LandscapeLeft ||
                orientation == PageOrientation.LandscapeRight)
            {
                da.From = 90;
                da.To = 0;
            }
            else if (orientation == PageOrientation.Portrait ||
                orientation == PageOrientation.PortraitDown ||
                orientation == PageOrientation.PortraitUp)
            {
                da.From = -90;
                da.To = 0;
            }
            sb.Begin();
        }
        

        // 用于生成本地化 ApplicationBar 的示例代码
        //private void BuildLocalizedApplicationBar()
        //{
        //    // 将页面的 ApplicationBar 设置为 ApplicationBar 的新实例。
        //    ApplicationBar = new ApplicationBar();

        //    // 创建新按钮并将文本值设置为 AppResources 中的本地化字符串。
        //    ApplicationBarIconButton appBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/appbar.add.rest.png", UriKind.Relative));
        //    appBarButton.Text = AppResources.AppBarButtonText;
        //    ApplicationBar.Buttons.Add(appBarButton);

        //    // 使用 AppResources 中的本地化字符串创建新菜单项。
        //    ApplicationBarMenuItem appBarMenuItem = new ApplicationBarMenuItem(AppResources.AppBarMenuItemText);
        //    ApplicationBar.MenuItems.Add(appBarMenuItem);
        //}
    }
}