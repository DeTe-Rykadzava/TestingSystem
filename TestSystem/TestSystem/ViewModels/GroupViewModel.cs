using System;
using TestSystem.Models;

namespace TestSystem.ViewModels;

public class GroupViewModel
{
    public readonly Group Group;

    public int Id => Group.Id;

    public string GroupName => Group.GroupName;

    public GroupViewModel(Group group)
    {
        Group = group;
    }
}