using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApi.Models
{
    public class FeedbackModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Subject { get; set; }

        [Required]
        [StringLength(500)]
        public string Details { get; set; }

        public IDictionary<string, string> Parameters { get; set; }
    }
}