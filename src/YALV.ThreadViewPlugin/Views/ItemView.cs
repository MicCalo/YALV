using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using YALV.ThreadViewPlugin.ViewModels;

namespace YALV.ThreadViewPlugin.Views
{
    internal class ItemView : Panel
    {
        private static readonly Pen pen = new Pen(Brushes.Gray, 0.5);
        private static readonly Pen grpPen = new Pen(Brushes.Black, 1);
        private static readonly Typeface itemTypeface = new Typeface("Segeo UI");
        private Rect btnRect;

        private static double size = 9;

        public ItemView()
        {
            Width = 1000;
            Height = size + 2;
            Margin = new Thickness(0);
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            ItemViewModelBase vm = DataContext as ItemViewModelBase;
            if (vm != null)
            {
                if (btnRect.Contains(e.GetPosition(this)))
                {
                    vm.ToggleExpansion();
                }
            }
        }

        private void DrawColumnText(DrawingContext dc, string text, HeaderViewModel column, Brush color, bool isCentered = false)
        {
            FormattedText ft = GetText(text, column.Width, color);
            double x = column.Left;
            if (isCentered)
            {
                x = Math.Max(column.Left + (column.Width - ft.Width) / 2, column.Left);
            }
            dc.DrawText(ft, new Point(x, 0));
        }

        protected override void OnRender(DrawingContext dc)
        {
            btnRect = Rect.Empty;
            if (DataContext == null)
            {
                return;
            }

            ItemViewModel itemVm = DataContext as ItemViewModel;
            GroupedItemViewModel grpVm = DataContext as GroupedItemViewModel;
            ItemViewModelBase vm;
            Brush color;
            if (itemVm != null)
            {
                if (itemVm.Group != null)
                {
                    DrawGroupingVisualization(dc, itemVm);
                }
                vm = itemVm;
                color = Brushes.Black;
            }
            else if (grpVm != null)
            {

                DrawBtn(dc, grpVm.Thread.MainViewModel.GrpHeader.Center, size / 2 - 5, true);
                vm = grpVm;
                color = Brushes.Gray;
            }
            else
            {
                return;
            }

            HeaderViewModel timeColumn = vm.Thread.MainViewModel.TimeHeader;
            DrawColumnText(dc, vm.Time, timeColumn, color);

            HeaderViewModel idColumn = vm.Thread.MainViewModel.IdHeader;
            DrawColumnText(dc, vm.Id, idColumn, color);

            DrawColumnText(dc, vm.Text, vm.Thread,color, true);


            foreach (ThreadViewModel other in vm.Thread.MainViewModel.Threads)
            {
                if (other != vm.Thread)
                {
                    dc.DrawLine(pen, new Point(other.Center, -2), new Point(other.Center, size + 4));
                }
            }
        }

        private void DrawGroupingVisualization(DrawingContext dc, ItemViewModel vm)
        {
            HeaderViewModel grp = vm.Thread.MainViewModel.GrpHeader;
            double center = grp.Center;
            double top = -2;
            double bottom = size + 4;
            GroupMembership membership = vm.Group.GetMembership(vm);
            if (membership == GroupMembership.First)
            {
                top += 2;
                dc.DrawLine(grpPen, new Point(center - 2, top), new Point(center + 2, top));
            }
            else if (membership == GroupMembership.Last)
            {
                bottom -= 2;
                dc.DrawLine(grpPen, new Point(center - 2, bottom), new Point(center + 2, bottom));
            }
            dc.DrawLine(grpPen, new Point(center - 2, top), new Point(center - 2, bottom));

            if (vm.Group.IsCenter(vm))
            {
                double y = size / 2 - 5;
                if ((vm.Group.Count % 2) == 0)
                {
                    y = -size / 2 - 3;
                }
                DrawBtn(dc, center, y, false);
            }
        }

        private void DrawBtn(DrawingContext dc, double center, double y, bool plus)
        {
            btnRect = new Rect(center - 8, y, 12, 12);
            dc.DrawRectangle(Brushes.White, grpPen, btnRect);
            if (plus)
            {
                dc.DrawLine(grpPen, new Point(center - 2, y + 2), new Point(center - 2, y + 10));
            }
            dc.DrawLine(grpPen, new Point(center - 6, y + 6), new Point(center + 2, y + 6));
        }

        private FormattedText GetText(string s, double maxWidth, Brush color)
        {
            FormattedText text = new FormattedText(s, CultureInfo.CurrentCulture, FlowDirection.LeftToRight, itemTypeface, size, color, null, 1);
            text.MaxTextWidth = maxWidth;
            text.MaxTextHeight = size * 1.3;
            return text;
        }
    }
}
