using Nfh.EditorLibrary.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EditorLibrary.Example
{
    public class Layer : LayerBase
    {
        public Layer(ILayerStack owner) : base(owner)
        {
        }
    }

    public class Item : ItemBase
    {
        public Item(ILayer layer) : base(layer)
        {
        }
    }

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public LayerStack LayerStack { get; } = new LayerStack();

        public MainWindow()
        {
            InitializeComponent();

            DataContext = LayerStack;

            LayerStack.Layers.Add(new Layer(LayerStack) { Name = "Layer 0" });
            LayerStack.Layers.Add(new Layer(LayerStack) { Name = "Layer 1" });
            LayerStack.Layers.Add(new Layer(LayerStack) { Name = "Layer 2" });
            LayerStack.Layers.Add(new Layer(LayerStack) { Name = "Layer 3" });
            LayerStack.Layers.Add(new Layer(LayerStack) { Name = "Layer 4" });
        }

        private int cnt = 0;
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (LayerStack.SelectedLayer == null)
            {
                return;
            }

            var it = new Item(LayerStack.SelectedLayer)
            {
                Name = $"foo{cnt++}",
                X = 100,
                Y = 100,
                Width = 50,
                Height = 50,
                Visual = new Rectangle
                {
                    Stroke = Brushes.Green,
                    StrokeThickness = 1,
                    Fill = Brushes.Transparent,
                }
            };
            LayerStack.SelectedLayer.Items.Add(it);
            LayerStack.SelectedLayer.SelectedItem = it;
        }
    }
}
