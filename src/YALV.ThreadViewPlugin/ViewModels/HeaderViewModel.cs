namespace YALV.ThreadViewPlugin.ViewModels
{
    internal class HeaderViewModel : ViewModelBase
    {
        private double left = -1;
        private double width = -1;
        public double MinWith { get; private set; }
        public double With { get; private set; }
        public HeaderViewModel(string name, int minWidth, int width)
        {
            Name = name;
            MinWith = minWidth;
            Width = width;
        }

        public double Center { get { return left + width / 2; } }

        public double Left
        {
            get { return left; }
            set
            {
                left = value;
                FirePropertyChanged("Left");
            }
        }

        public double Width
        {
            get { return width; }
            set
            {
                width = value;
                FirePropertyChanged("Width");
            }
        }

        public string Name { get; private set; }
        public bool IsVisible { get; set; }
    }
}
