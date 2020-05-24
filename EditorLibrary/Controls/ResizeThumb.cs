using Nfh.EditorLibrary.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls.Primitives;

namespace Nfh.EditorLibrary.Controls
{
    /// <summary>
    /// A <see cref="Thumb"/> for resizing <see cref="IItem"/>s.
    /// </summary>
    public class ResizeThumb : Thumb
    {
        private IItem item;

        /// <summary>
        /// Initializes a new <see cref="ResizeThumb"/>
        /// </summary>
        public ResizeThumb()
        {
            DragStarted += ResizeThumb_DragStarted;
            DragDelta += ResizeThumb_DragDelta;
        }

        private void ResizeThumb_DragStarted(object sender, DragStartedEventArgs e)
        {
            item = DataContext as IItem;
            item.Layer.Owner.SelectedLayer = item.Layer;
            item.Layer.SelectedItem = item;
        }

        private void ResizeThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            if (item == null || !item.CanResize || item.Layer.IsLocked)
            {
                return;
            }

            switch (HorizontalAlignment)
            {
            case HorizontalAlignment.Left:
                item.X += e.HorizontalChange;
                item.Width -= e.HorizontalChange;
                break;
            case HorizontalAlignment.Right:
                item.Width += e.HorizontalChange;
                break;
            }

            switch (VerticalAlignment)
            {
            case VerticalAlignment.Top:
                item.Y += e.VerticalChange;
                item.Height -= e.VerticalChange;
                break;
            case VerticalAlignment.Bottom:
                item.Height += e.VerticalChange;
                break;
            }

            item.Width = Math.Max(item.Width, 0.0);
            item.Height = Math.Max(item.Height, 0.0);

            // NOTE: Workaround for zooming
            if (SnapsToDevicePixels)
            {
                item.Width = Math.Round(item.Width);
                item.Height = Math.Round(item.Height);
            }

            e.Handled = true;
        }
    }
}
