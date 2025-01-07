using System.ComponentModel.DataAnnotations.Schema;

namespace BlazorAppGraph
{
    public class UserInfo
    {
        public string Id { get; set; }
        public string GroupId { get; set; }
        public string DisplayName { get; set; }
        public string Mail { get; set; }
        [NotMapped]
        public bool IsSelected { get; set; }
    }
}
