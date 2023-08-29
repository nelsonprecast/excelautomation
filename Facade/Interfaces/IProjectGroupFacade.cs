namespace Facade.Interfaces
{
    public interface IProjectGroupFacade
    {
        bool EditGroup(int groupId, string groupName);

        bool DeleteGroup(int groupId);

        bool RemoveFromGroup(string projectDetailIds);
    }
}
