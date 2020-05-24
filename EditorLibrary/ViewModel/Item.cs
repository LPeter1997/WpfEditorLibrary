using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;

namespace Nfh.EditorLibrary.ViewModel
{
    /// <summary>
    /// The interface for every interactive item in the editor software.
    /// </summary>
    public interface IItem : INotifyPropertyChanged
    {
        /// <summary>
        /// True, if the item can be renamed, if the owning layer isn't locked.
        /// </summary>
        bool CanRename { get; }
        /// <summary>
        /// True, if the item can be moved around, if the owning layer isn't locked.
        /// </summary>
        bool CanMove { get; }
        /// <summary>
        /// True, if the item can be resized, if the owning layer isn't locked.
        /// </summary>
        bool CanResize { get; }

        /// <summary>
        /// The owning layer.
        /// </summary>
        ILayer Layer { get; }

        /// <summary>
        /// True, if this item is currently selected.
        /// </summary>
        bool IsSelected { get; set; }

        /// <summary>
        /// The name of this item.
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// The X position of this item on the editor's canvas.
        /// </summary>
        double X { get; set; }
        /// <summary>
        /// The Y position of this item on the editor's canvas.
        /// </summary>
        double Y { get; set; }

        /// <summary>
        /// The width of this item.
        /// </summary>
        double Width { get; set; }
        /// <summary>
        /// The height of this item.
        /// </summary>
        double Height { get; set; }

        /// <summary>
        /// The visual representation of this item.
        /// </summary>
        UIElement Visual { get; set; }

        /// <summary>
        /// Sets the <see cref="IsSelected"/> property without firing events.
        /// For internal usage.
        /// </summary>
        /// <param name="selected">The value to set the property to.</param>
        void SetIsSelectedInternal(bool selected);
    }

    /// <summary>
    /// A base-class that implement some basic logic for <see cref="IItem"/>.
    /// </summary>
    public abstract class ItemBase : IItem
    {
        public event PropertyChangedEventHandler PropertyChanged;

        // Editability

        private bool canRename = true;
        public bool CanRename
        {
            get => canRename;
            set { canRename = value; OnPropertyChanged(); }
        }

        private bool canMove = true;
        public bool CanMove
        {
            get => canMove;
            set { canMove = value; OnPropertyChanged(); }
        }

        private bool canResize = true;
        public bool CanResize
        {
            get => canResize;
            set { canResize = value; OnPropertyChanged(); }
        }

        public ILayer Layer { get; }

        // The only unique property

        private bool isSelected;
        public bool IsSelected
        {
            get => isSelected;
            set
            {
                if (value)
                {
                    Layer.SelectedItems.Add(this);
                }
                else
                {
                    Layer.SelectedItems.Remove(this);
                }
                SetIsSelectedInternal(value);
            }
        }

        // Rest of the properties

        private string name;
        public string Name
        {
            get => name;
            set { name = value; OnPropertyChanged(); }
        }

        private double x;
        public double X
        {
            get => x;
            set { x = value; OnPropertyChanged(); }
        }

        private double y;
        public double Y
        {
            get => y;
            set { y = value; OnPropertyChanged(); }
        }

        private double width;
        public double Width
        {
            get => width;
            set { width = value; OnPropertyChanged(); }
        }

        private double height;
        public double Height
        {
            get => height;
            set { height = value; OnPropertyChanged(); }
        }

        private UIElement visual;
        public UIElement Visual
        {
            get => visual;
            set { visual = value; OnPropertyChanged(); }
        }

        public ItemBase(ILayer layer)
        {
            Layer = layer;
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
    }
}
