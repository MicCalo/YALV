using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using YALV.ThreadViewPlugin.ViewModels;

namespace YALV.ThreadViewPlugin.Views
{
    public class ThreadViewPanel : Panel
    {
        private static readonly Typeface threadsTypeface = new Typeface(new FontFamily("Segeo UI"), FontStyles.Normal, FontWeights.Bold, FontStretches.Normal);
        private static readonly Typeface itemTypeface = new Typeface("Segeo UI");
        protected override void OnRender(DrawingContext dc)
        {
            dc.DrawRectangle(Brushes.White, null, new Rect(10, 10, ActualWidth - 20, ActualHeight - 20));
            MainViewModel vm = DataContext as MainViewModel;
            if (vm != null)
            {
                Dictionary<ThreadViewModel, int> threadPositions = new Dictionary<ThreadViewModel, int>();
                int x = 100;
                foreach(ThreadViewModel tVm in vm.Threads)
                {
                    FormattedText text = new FormattedText(tVm.Name, CultureInfo.CurrentCulture, FlowDirection.LeftToRight, threadsTypeface, 15, Brushes.Black, null, 1.25);
                    dc.DrawText(text, new Point(x-text.Width/2, 0));
                    threadPositions.Add(tVm, x);
                    x += 150;
                }

                int y = 20;
                foreach(ItemViewModel item in vm.Items)
                {
                    FormattedText text = new FormattedText(item.ShortText, CultureInfo.CurrentCulture, FlowDirection.LeftToRight, itemTypeface, 12, Brushes.Black, null, 1.25);
                    dc.DrawText(text, new Point(threadPositions[item.Thread]-text.Width/2, y));
                    y += 15;
                }
            }
            
        }
    }
}
