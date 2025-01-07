namespace BlazorAppGraph
{
    public class GroupInfo
    {
        public string? Id { get; set; }
        public string? DisplayName { get; set; }
        public string? Description { get; set; }
        public List<UserInfo> Members { get; set; }
    }
}
