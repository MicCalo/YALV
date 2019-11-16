namespace YALV.ThreadViewPlugin.ViewModels
{
    internal abstract class ItemViewModelBase
    {
        protected const string FullTimeString = "hh:MM:ss.fff";
        protected readonly string[] TimeStringOrder = new string[] { FullTimeString, "hh:MM:ss.ff", "hh:MM:ss.f", "hh:MM:ss", "hh:MM" };

        private readonly ThreadViewModel threadViewModel;

        protected ItemViewModelBase(ThreadViewModel threadViewModel)
        {
            this.threadViewModel = threadViewModel;
        }

        internal ThreadViewModel Thread { get { return threadViewModel; } }
        internal abstract string Time { get; }
        internal abstract string Id { get; }
        internal abstract string Text { get; }
        internal abstract void ToggleExpansion();
    }
}
