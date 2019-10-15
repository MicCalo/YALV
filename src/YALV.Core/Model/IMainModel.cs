using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YALV.Core.Domain;

namespace YALV.Core.Model
{
    public interface IMainModel
    {
        ObservableCollection<LogItem> Items { get; set; }
        void RemoveItemsWithPath(string path);
    }
}
