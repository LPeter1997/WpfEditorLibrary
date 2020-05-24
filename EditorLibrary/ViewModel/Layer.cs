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
    /// The interface for layers in the editor software.
    /// </summary>
    public interface ILayer : INotifyPropertyChanged
    {
        /// <summary>
        /// True, if the layer can be renamed.
        /// </summary>
        bool CanRename { get; }
        /// <summary>
        /// True, if the layer can have multiple items selected.
        /// </summary>
        bool CanSelectMany { get; }
        /// <summary>
        /// True, if the layer can be locked, so it's not editable.
        /// </summary>
        bool CanLock { get; }
        /// <summary>
        /// True, if the layer can be hidden, so it's items aren't visible.
        /// </summary>
        bool CanHide { get; }

        /// <summary>
        /// The owning layer stack.
        /// </summary>
        ILayerStack Owner { get; }

        /// <summary>
        /// True, if this is the currently selected layer.
        /// </summary>
        bool IsSelected { get; set; }

        /// <summary>
        /// The name of the layer.
        /// </summary>
        string Name { get; set; }
        /// <summary>
        /// True, if the layer is currently locked.
        /// </summary>
        bool IsLocked { get; set; }
        /// <summary>
        /// True, if the layer is currently visible.
        /// </summary>
        bool IsVisible { get; set; }

        /// <summary>
        /// The <see cref="IItem"/>s in the current layer.
        /// </summary>
        public ObservableCollection<IItem> Items { get; }
        /// <summary>
        /// The currently selected item in this layer. This should be null, if multiple items
        /// are selected.
        /// </summary>
        public IItem SelectedItem { get; set; }
        /// <summary>
        /// The collection of selected items in this layer.
        /// </summary>
        public ObservableCollection<IItem> SelectedItems { get; }

        /// <summary>
        /// Sets the <see cref="IsSelected"/> property without firing events.
        /// For internal usage.
        /// </summary>
        /// <param name="selected">The value to set the property to.</param>
        void SetIsSelectedInternal(bool selected);
    }

    /// <summary>
    /// A base-class that implement some basic logic for <see cref="ILayer"/>.
    /// </summary>
    public abstract class LayerBase : ILayer
    {
        public event PropertyChangedEventHandler PropertyChanged;

        // Editability

        private bool canRename = true;
        public bool CanRename
        {
            get => canRename;
            set { canRename = value; OnPropertyChanged(); }
        }

        private bool canSelectMany = true;
        public bool CanSelectMany
        {
            get => canSelectMany;
            set { canSelectMany = value; OnPropertyChanged(); }
        }

        private bool canLock = true;
        public bool CanLock
        {
            get => canLock;
            set { canLock = value; OnPropertyChanged(); }
        }

        private bool canHide = true;
        public bool CanHide
        {
            get => canHide;
            set { canHide = value; OnPropertyChanged(); }
        }

        public ILayerStack Owner { get; }

        private bool isSelected;
        public bool IsSelected
        {
            get => isSelected;
            set 
            {
                Owner.SelectedLayer = value ? this : null;
                SetIsSelectedInternal(value);
            }
        }

        // Layer properties

        private string name;
        public string Name 
        { 
            get => name; 
            set { name = value; OnPropertyChanged(); }
        }

        private bool isLocked;
        public bool IsLocked 
        {
            get => isLocked;
            set { isLocked = value; OnPropertyChanged(); } 
        }

        private bool isVisible = true;
        public bool IsVisible 
        { 
            get => isVisible; 
            set { isVisible = value; OnPropertyChanged(); }
        }

        public ObservableCollection<IItem> Items { get; } = new ObservableCollection<IItem>();

        public IItem SelectedItem 
        { 
            get => SelectedItems.Count == 1 ? SelectedItems[0] : null; 
            set 
            {
                RemoveAll(SelectedItems);
                if (value != null)
                {
                    SelectedItems.Add(value);
                }
            }
        }

        public ObservableCollection<IItem> SelectedItems { get; } = new ObservableCollection<IItem>();

        public LayerBase(ILayerStack owner)
        {
            Owner = owner;
            SelectedItems.CollectionChanged += SelectedItems_CollectionChanged;
        }

        private void SelectedItems_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged(nameof(SelectedItem));

            if (e.OldItems != null)
            {
                foreach (var i in e.OldItems)
                {
                    (i as IItem).SetIsSelectedInternal(false);
                }
            }
            if (e.NewItems != null)
            {
                foreach (var i in e.NewItems)
                {
                    (i as IItem).SetIsSelectedInternal(true);
                }
            }
        }

        public void SetIsSelectedInternal(bool selected)
        {
            isSelected = selected;
            OnPropertyChanged(nameof(IsSelected));
        }

        private void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private static void RemoveAll<T>(IList<T> list)
        {
            while (list.Count > 0)
            {
                list.RemoveAt(list.Count - 1);
            }
        }
    }
}
