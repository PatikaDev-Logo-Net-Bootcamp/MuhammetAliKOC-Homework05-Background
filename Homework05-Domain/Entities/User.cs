using System.ComponentModel.DataAnnotations.Schema;

namespace Homework05_Domain.Entities
{
    public class User//:BaseEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
    }
}
