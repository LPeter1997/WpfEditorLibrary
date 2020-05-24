using Nfh.EditorLibrary.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace Nfh.EditorLibrary.Controls
{
    /// <summary>
    /// A <see cref="Thumb"/> for moving <see cref="IItem"/>s.
    /// </summary>
    public class MoveThumb : Thumb
    {
        private IItem item;

        /// <summary>
        /// Initializes a new <see cref="MoveThumb"/>
        /// </summary>
        public MoveThumb()
        {
            DragStarted += MoveThumb_DragStarted;
            DragDelta += MoveThumb_DragDelta;
        }

        private void MoveThumb_DragStarted(object sender, DragStartedEventArgs e)
        {
            item = DataContext as IItem;
            item.Layer.Owner.SelectedLayer = item.Layer;
            item.Layer.SelectedItem = item;
        }

        private void MoveThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            if (item == null || !item.CanMove || item.Layer.IsLocked)
            {
                return;
            }

            item.X += e.HorizontalChange;
            item.Y += e.VerticalChange;

            // NOTE: Workaround for zooming
            if (SnapsToDevicePixels)
            {
                item.X = Math.Round(item.X);
                item.Y = Math.Round(item.Y);
            }
        }
    }
}
