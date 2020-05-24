using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace Nfh.EditorLibrary.ViewModel
{
    /// <summary>
    /// Interface for layer stacks.
    /// </summary>
    public interface ILayerStack : INotifyPropertyChanged
    {
        /// <summary>
        /// The layers in this layer stack.
        /// </summary>
        ObservableCollection<ILayer> Layers { get; }
        /// <summary>
        /// The layers in the stack in reverse order.
        /// </summary>
        ReadOnlyObservableCollection<ILayer> LayersReverse { get; }
        /// <summary>
        /// The currently selected layer.
        /// </summary>
        ILayer SelectedLayer { get; set; }

        /// <summary>
        /// Adds an <see cref="IItem"/> to the correct layer.
        /// </summary>
        /// <param name="item">The <see cref="IItem"/> to add.</param>
        void Add(IItem item);
    }

    /// <summary>
    /// A default implementation of <see cref="ILayerStack"/>.
    /// </summary>
    public class LayerStack : ILayerStack
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<ILayer> Layers { get; } = new ObservableCollection<ILayer>();

        private ILayer selectedLayer;
        public ILayer SelectedLayer 
        { 
            get => selectedLayer; 
            set 
            {
                selectedLayer?.SetIsSelectedInternal(false);
                value?.SetIsSelectedInternal(true);
                selectedLayer = value; 
                OnPropertyChanged(); 
            }
        }

        private ObservableCollection<ILayer> layersReverse = new ObservableCollection<ILayer>();
        public ReadOnlyObservableCollection<ILayer> LayersReverse { get; }

        public LayerStack()
        {
            LayersReverse = new ReadOnlyObservableCollection<ILayer>(layersReverse);
            Layers.CollectionChanged += Layers_CollectionChanged;
        }

        public void Add(IItem item)
        {
            item.Layer.Items.Add(item);
        }

        private void Layers_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            // TODO: We could optimize this
            layersReverse.Clear();
            for (int i = Layers.Count - 1; i >= 0; --i)
            {
                layersReverse.Add(Layers[i]);
            }
        }

        private void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
