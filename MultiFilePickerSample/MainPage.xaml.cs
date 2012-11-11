using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

using Windows.Storage.Pickers;
using Windows.Storage;
using Windows.UI.Xaml.Media.Imaging;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

// 空白ページのアイテム テンプレートについては、http://go.microsoft.com/fwlink/?LinkId=234238 を参照してください

namespace MultiFilePickerSample
{
    /// <summary>
    /// それ自体で使用できる空白ページまたはフレーム内に移動できる空白ページ。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public ObservableCollection<ImageData> _list = new ObservableCollection<ImageData>();

        public MainPage()
        {
            this.InitializeComponent();

            this.DataContext = this._list;
        }

        /// <summary>
        /// このページがフレームに表示されるときに呼び出されます。
        /// </summary>
        /// <param name="e">このページにどのように到達したかを説明するイベント データ。Parameter 
        /// プロパティは、通常、ページを構成するために使用します。</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            FileOpenPicker openPicker = new FileOpenPicker();
            openPicker.ViewMode = PickerViewMode.List;
            openPicker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;

            openPicker.FileTypeFilter.Add(".jpg");
            openPicker.FileTypeFilter.Add(".jpeg");
            openPicker.FileTypeFilter.Add(".png");
            
            IReadOnlyList<StorageFile> files = await openPicker.PickMultipleFilesAsync();

            foreach(var file in files)
            {
                var stream = await file.OpenAsync(Windows.Storage.FileAccessMode.Read);

                BitmapImage image = new BitmapImage();
                image.SetSource(stream);

                ImageData data = new ImageData();
                data.image = image;
                data.name = file.DisplayName;

                this._list.Add(data);
            }
        }
    }

    public class ImageData : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var eventHandler = this.PropertyChanged;
            if (eventHandler != null)
            {
                eventHandler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private BitmapImage _image;

        public BitmapImage image
        {
            get { return _image; }
            set
            {
                _image = value;
                this.OnPropertyChanged("image");
            }
        }

        private string _name;

        public string name
        {
            get { return _name; }
            set
            {
                _name = value;
                this.OnPropertyChanged("name");
            }
        }
        
        
    }
}
