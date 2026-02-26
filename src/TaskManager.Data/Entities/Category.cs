using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Data.Entities
{
    public class Category : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string ColorHex { get; set; } = "#FF0000";

        // Navigation property
        public int UserId { get; set; }
        public virtual User? User { get; set; }

        // Related tasks
        public virtual ICollection<TodoTask> Tasks { get; set; } = new List<TodoTask>();
    }
}
