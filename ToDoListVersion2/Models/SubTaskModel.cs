using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDolistVersion2.Models
{
    public class SubTaskModel
    {
        public string? Id { get; set; }
        public string? Title { get; set; }

        public Boolean isChecked { get; set; }

    }
}
