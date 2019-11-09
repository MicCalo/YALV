using YALV.Core.Domain;

namespace YALV.ThreadViewPlugin.ViewModels
{
    internal class ItemViewModel
    {
        private string shortText;
        private readonly LogItem model;

        public ItemViewModel Previous { get; private set; }
        public ItemViewModel Next { get; private set; }
        public ThreadViewModel Thread { get; private set; }
        public string Time { get { return model.TimeStamp.ToString("hh:MM:ss.fff"); } }
        public string Id { get { return model.Id.ToString(); } }
        public string Text { get { return model.Message; } }

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

        public ItemViewModel(LogItem model, ItemViewModel previous, ThreadViewModel thread)
        {
            this.model = model;
            Thread = thread;
            this.Previous = previous;
            if (this.Previous != null)
            {
                this.Previous.Next = this;
            }
        }
    }
}
