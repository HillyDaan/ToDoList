using System;

namespace ToDolistVersion2.Models
{
    public class SubTaskModel
    {
        public string? Id { get; set; }
        public string? Title { get; set; }

        public string? ParentId { get; set; }

        public int? Points { get; set; }

        public Boolean IsChecked { get; set; }

    }
}
