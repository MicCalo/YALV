namespace YALV.ThreadViewPlugin.ViewModels
{
    internal class ThreadViewModel : HeaderViewModel
    {
        public MainViewModel MainViewModel { get; private set; }

        public ThreadViewModel(string name, MainViewModel main) : base(name, 50, 200)
        {
            MainViewModel = main;
        }
    }
}
