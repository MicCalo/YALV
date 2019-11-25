using System;
using System.Collections.Generic;
using System.Text;

namespace YALV.ThreadViewPlugin.ViewModels
{
    internal enum GroupMembership
    {
        None,
        First,
        Last,
        InBetween
    }

    internal class GroupedItemViewModel : ItemViewModelBase
    {
        private readonly List<ItemViewModel> items = new List<ItemViewModel>();
        private readonly Lazy<string> timeString;

        internal GroupedItemViewModel(ThreadViewModel threadVm):base(threadVm)
        {
            timeString = new Lazy<string>(CreateTimeString);
        }

        internal override string Time
        {
            get { return timeString.Value; }
        }

        private string CreateTimeString()
        {
            string a = "";
            string b = "";
            foreach (string ts in TimeStringOrder)
            {
                a = First.GetTime(ts);
                b = Last.GetTime(ts);
                if (a.Equals(b))
                {
                    return a;
                }
            }

            return a + "-" + b;
        }

        internal override string Id
        {
            get { return First.Id + "-" + Last.Id; }
        }

        internal override string Text
        {
            get { return Count+" messages"; }
        }

        internal override void ToggleExpansion()
        {
            Thread.MainViewModel.Expand(this);
        }

        internal void Add(ItemViewModel i)
        {
            items.Add(i);
        }

        internal void Collapse()
        {
            Thread.MainViewModel.Collapse(this);
        }

        public GroupMembership GetMembership(ItemViewModel i)
        {
            int pos = items.IndexOf(i);
            if (pos == -1)
            {
                return GroupMembership.None;
            }
            if (pos == 0)
            {
                return GroupMembership.First;
            }

            if (pos == Count - 1)
            {
                return GroupMembership.Last;
            }

            return GroupMembership.InBetween;
        }

        internal bool IsCenter(ItemViewModel i)
        {
            int pos = items.IndexOf(i);
            return pos == Count / 2;
        }

        internal ItemViewModel First { get { return items[0]; } }
        internal ItemViewModel Last { get { return items[Count-1]; } }
        internal int Count { get { return items.Count; } }
        internal IReadOnlyList<ItemViewModel> Items { get { return items; } }
    }
}
