namespace BlazorAppGraph
{
    public interface IGraphClient
    {
        Task<IEnumerable<GroupInfo>> GetGroupsAsync(bool includeAppRoles, bool includeMembership);
        Task<IEnumerable<GroupInfo>> GetGroupsAsync(bool includeAppRoles, string appRoleAssignmentID);
        Task<GroupInfo> GetGroupAsync(string groupId, bool includeMembership);
        Task<IEnumerable<UserInfo>> GetUsers();
        Task AddUserToGroupAsync(string userID, string groupID);
        Task RemoveUserFromGroupAsync(string userID, string groupID);
    }
}