using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDolistVersion2.Models
{
    public class TaskModel
    {
        /// <summary>
        /// Gets or sets unique identifier of the task
        /// </summary>
        public string? Id { get; set; }

        /// <summary>
        /// Gets or sets title of the task
        /// </summary>
        public string? Title { get; set; }

        /// <summary>
        ///  Gets or sets points of task.
        ///  1 point == 1 hour
        /// </summary>
        public int? Points { get; set; }

        /// <summary>
        /// Gets or sets creation date of task
        /// </summary>
        public DateTime? Created { get; set; }

        /// <summary>
        /// Gets or sets deadline of task
        /// </summary>
        public DateTime? Deadline { get; set; }

        /// <summary>
        /// Gets or sets checked status of each task
        /// </summary>
        public bool IsChecked { get; set; }

    }
}
