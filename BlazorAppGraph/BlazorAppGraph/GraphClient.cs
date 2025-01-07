using Microsoft.Graph;
using Microsoft.Graph.Models;

namespace BlazorAppGraph
{
    public class GraphClient(GraphServiceClient graphServiceClient) : IGraphClient
    {
        private readonly GraphServiceClient _graphServiceClient = graphServiceClient;
        public async Task<IEnumerable<GroupInfo>> GetGroupsAsync(bool includeAppRoles, bool includeMembership)
        {
            var result = await _graphServiceClient.Groups.GetAsync((requestConfiguration) =>
            {
                if (includeAppRoles)
                    requestConfiguration.QueryParameters.Expand = new string[] { "AppRoleAssignments" };
                else if (includeMembership)
                    requestConfiguration.QueryParameters.Expand = new string[] { "Members" };
            });

            return result.Value.Select(g => new GroupInfo
            {
                Id = g.Id,
                DisplayName = g.DisplayName,
                Description = g.Description
            });
        }

        public async Task<IEnumerable<GroupInfo>> GetGroupsAsync(bool includeAppRoles, string appRoleAssignmentID)
        {
            var result = await _graphServiceClient.Groups.GetAsync((requestConfiguration) =>
            {
                if (includeAppRoles)
                    requestConfiguration.QueryParameters.Expand = new string[] { "AppRoleAssignments" };
            });

            // requestConfiguration.QueryParameters.Expand = new string[] { "owners" };

            return result.Value.Where(p => p.AppRoleAssignments.Any(
                c => c.ResourceId.Value.ToString() == appRoleAssignmentID )) .Select(g => new GroupInfo
            {
                Id = g.Id,
                DisplayName = g.DisplayName,
                Description = g.Description
            });
        }

        public async Task<GroupInfo> GetGroupAsync(string groupId, bool includeMembership)
        {
            var group = await _graphServiceClient.Groups[groupId].GetAsync((requestConfiguration) =>
            {
                if (includeMembership)
                    requestConfiguration.QueryParameters.Expand = new string[] { "Members" };
            });

            return new GroupInfo
            {
                Id = group.Id,
                DisplayName = group.DisplayName,
                Description = group.Description,
                Members = group.Members.Select(m => new UserInfo
                {
                    Id = m.Id
                }).ToList()
            };
        }

        public async Task<IEnumerable<UserInfo>> GetUsers()
        {
            var result = await _graphServiceClient.Users.GetAsync();

            return result.Value.Select(u => new UserInfo
            {
                Id = u.Id,
                DisplayName = u.DisplayName,
                Mail = u.Mail
            });
        }

        public async Task AddUserToGroupAsync(string userID, string groupId)
        {
            var requestBody = new ReferenceCreate
            {
                OdataId = $"https://graph.microsoft.com/v1.0/directoryObjects/{userID}",
            };

            await _graphServiceClient.Groups[groupId].Members.Ref.PostAsync(requestBody);
            return;
        }

        public async Task RemoveUserFromGroupAsync(string userID, string groupID)
        {
            await _graphServiceClient.Groups[groupID].Members[userID].Ref.DeleteAsync();
            return;
        }
    }

}
