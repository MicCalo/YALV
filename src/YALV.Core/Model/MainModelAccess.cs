using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YALV.Core.Model
{
    public class MainModelAccess
    {
        private static MainModelAccess _instance;

        public static MainModelAccess Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new MainModelAccess();
                }
                return _instance;
            }
        }

        public IMainModel MainModel
        {
            get { return _mainModel; }
        }

        private readonly IMainModel _mainModel;

        private MainModelAccess()
        {
            _mainModel = new MainModel();
        }
    }
}
