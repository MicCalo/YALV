using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using YALV.ThreadViewPlugin.ViewModels;

namespace YALV.ThreadViewPlugin.Views
{
    internal class ItemView : Panel
    {
        private static readonly Pen pen = new Pen(Brushes.Gray, 0.5);
        private static readonly Pen grpPen = new Pen(Brushes.Black, 1);
        private static readonly Typeface itemTypeface = new Typeface("Segeo UI");

        private static double size = 12;

        public ItemView()
        {
            Width = 1000;
            Height = size + 2;
            Margin = new Thickness(0);
        }

        protected override void OnRender(DrawingContext dc)
        {
            ItemViewModel vm = DataContext as ItemViewModel;
            if (vm != null)
            {
                HeaderViewModel timeColumn = vm.Thread.MainViewModel.TimeHeader;
                FormattedText time = GetText(vm.Time, timeColumn.Width);
                dc.DrawText(time, new Point(timeColumn.Left, 0));

                HeaderViewModel idColumn = vm.Thread.MainViewModel.IdHeader;
                FormattedText id = GetText(vm.Id, idColumn.Width);
                dc.DrawText(id, new Point(idColumn.Left, 0));

                FormattedText msg = GetText(vm.Text, vm.Thread.Width);
                double x = vm.Thread.Left;// Math.Max(vm.Thread.Left + (vm.Thread.Width - text.Width) / 2, vm.Thread.Left);
                if (true)
                {
                    x = Math.Max(vm.Thread.Left + (vm.Thread.Width - msg.Width) / 2, vm.Thread.Left);

                }
                dc.DrawText(msg, new Point(x, 0));

                foreach(ThreadViewModel other in vm.Thread.MainViewModel.Threads)
                {
                    if (other != vm.Thread)
                    {
                        dc.DrawLine(pen, new Point(other.Center, -2), new Point(other.Center, size + 4));
                    }
                }

                bool changeBefore = vm.Previous == null || vm.Thread != vm.Previous.Thread;
                bool changeAfter = vm.Next == null || vm.Thread != vm.Next.Thread;
                if (!(changeBefore && changeAfter))
                {
                    HeaderViewModel grp = vm.Thread.MainViewModel.GrpHeader;
                    double center = grp.Center;
                    double top = -2;
                    double bottom = size + 4;
                    if (changeBefore)
                    {
                        top += 4;
                        dc.DrawLine(grpPen, new Point(center - 2, top), new Point(center + 2, top));
                    }
                    if (changeAfter)
                    {
                        bottom -= 4;
                        dc.DrawLine(grpPen, new Point(center - 2, bottom), new Point(center + 2, bottom));
                    }
                    dc.DrawLine(grpPen, new Point(center-2, top), new Point(center-2, bottom));
                }
            }
        }

        private FormattedText GetText(string s, double maxWidth)
        {
            FormattedText text = new FormattedText(s, CultureInfo.CurrentCulture, FlowDirection.LeftToRight, itemTypeface, size, Brushes.Black, null, 1);
            text.MaxTextWidth = maxWidth;
            text.MaxTextHeight = size * 1.3;
            return text;
        }
    }
}
