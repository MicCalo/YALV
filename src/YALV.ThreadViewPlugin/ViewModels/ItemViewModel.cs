using System;
using YALV.Core.Domain;

namespace YALV.ThreadViewPlugin.ViewModels
{
    internal class ItemViewModel : ItemViewModelBase
    {
        private string shortText;
        private readonly LogItem model;

        public ItemViewModel Previous { get; private set; }
        public ItemViewModel Next { get; private set; }

        public GroupedItemViewModel Group { get; set; }
        internal override string Time { get { return model.TimeStamp.ToString(FullTimeString); } }
        internal override string Id { get { return model.Id.ToString(); } }
        internal override string Text { get { return model.Message; } }

        internal string GetTime(string format)
        {
            return model.TimeStamp.ToString(format);
        }

        internal override void ToggleExpansion()
        {
            Group.Collapse();
        }

        public ItemViewModel(LogItem model, ItemViewModel previous, ThreadViewModel thread):base(thread)
        {
            this.model = model;
            Previous = previous;
            if (Previous != null)
            {
                Previous.Next = this;
            }
        }

        public string ShortText { get {
                if (shortText == null)
                {
                    shortText = model.Message;
                    if (shortText.Length > 30)
                    {
                        shortText = shortText.Substring(0, 30);
                    }
                }
                return shortText;
            }
        }        
    }
}
