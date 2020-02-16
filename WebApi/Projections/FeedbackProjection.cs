using System.ComponentModel.DataAnnotations;

namespace WebApi.Projections
{
    public class FeedbackProjection
    {
        public int Id { get; set; }
        
        public string Subject { get; set; }

        public string Details { get; set; }

        public string Parameters { get; set; }
    }
}