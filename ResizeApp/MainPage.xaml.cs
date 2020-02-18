using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace ResizeApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public class GridViewCustomPanel : Panel
    {
        private double maxWidth;
        private double maxHeight;

        protected override Size ArrangeOverride(Size finalSize)
        {
            var x = 0.0;
            var y = 0.0;
            foreach (var child in Children)
            {
                if ((maxWidth + x) > finalSize.Width)
                {
                    x = 0;
                    y += maxHeight;
                }
                var newpos = new Rect(x, y, maxWidth, maxHeight);
                child.Arrange(newpos);
                x += maxWidth;
            }
            return finalSize;
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            foreach (var child in Children)
            {
                child.Measure(availableSize);

                var desirtedwidth = child.DesiredSize.Width;
                if (desirtedwidth > maxWidth)
                    maxWidth = desirtedwidth;

                var desiredheight = child.DesiredSize.Height;
                if (desiredheight > maxHeight)
                    maxHeight = desiredheight;
            }
            var itemperrow = Math.Floor(availableSize.Width / maxWidth);
            var rows = Math.Ceiling(Children.Count / itemperrow);
            return new Size(itemperrow * maxWidth, maxHeight * rows);
        }
    }

    public class Section
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public IEnumerable<SubSection> Contents { get; set; }


    }

    public class SubSection
    {
        public string Title { get; set; }

    }

    public sealed partial class MainPage : Page, INotifyPropertyChanged
    {
        private readonly string seed = "[{\"Title\":\"Heading A\",\"Description\":\"A really short description\",\"Contents\":[{\"Title\":\"Hey!\"},{\"Title\":\"Hey You\"},{\"Title\":\"Bye!\"},{\"Title\":\"Bye You\"}]},{\"Title\":\"Heading A\",\"Description\":\"A really short description\",\"Contents\":[{\"Title\":\"Foo!\"},{\"Title\":\"Foo You\"},{\"Title\":\"Bar!\"},{\"Title\":\"Bar You\"},{\"Title\":\"Hey!\"},{\"Title\":\"Hey You\"},{\"Title\":\"Bye!\"},{\"Title\":\"Bye You\"}]}]";

        public MainPage()
        {
            this.InitializeComponent();

            IEnumerable<Section> deserialized = JsonConvert.DeserializeObject<IEnumerable<Section>>(seed);
            this.Items = new ObservableCollection<Section>(deserialized);

        }

        private ObservableCollection<Section> items;

        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChanged(string name) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        public ObservableCollection<Section> Items
        {
            get { return items; }
            set
            {
                items = value;
                RaisePropertyChanged(nameof(Items));
            }
        }
    }
}
