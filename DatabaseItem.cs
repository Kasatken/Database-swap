using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp7
{
    public class DatabaseItem
    {
        public string DisplayName { get; set; } // имя для отображения до |
        public string ConnectionString { get; set; } // параметр доступа в БД

        public override string ToString()
        {
            return DisplayName; // показываем только название в списке
        }
    }
}