using System;
using System.Collections.ObjectModel;
using System.Linq;
using YALV.Core.Domain;

namespace YALV.Core.Model
{
    public class MainModel : IMainModel
    {
        private ObservableCollection<LogItem> _items = new ObservableCollection<LogItem>();
        public ObservableCollection<LogItem> Items
        {
            get { return _items; }
            set
            {
                _items.Clear();
                _items = value;
            }
        }

        public void RemoveItemsWithPath(string path)
        {
            var selectedItems = from it in _items
                                where (!it.Path.Equals(path, StringComparison.OrdinalIgnoreCase))
                                select it;
            _items = new ObservableCollection<LogItem>(selectedItems);

            int itemId = 1;
            foreach (LogItem item in _items)
                item.Id = itemId++;
        }

    }
}
