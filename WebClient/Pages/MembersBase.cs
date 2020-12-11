using Domain.ViewModel;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using WebClient.Abstractions;

namespace WebClient.Pages
{
    public class MembersBase : ComponentBase
    {
        protected List<MemberVm> members = new List<MemberVm>();
        protected List<MenuItem> leftMenuItem = new List<MenuItem>();

        protected bool showCreator;
        protected bool isLoaded;

        [Inject]
        public IMemberDataService MemberDataService { get; set; }

        protected override void OnInitialized()
        {
            UpdateMembers();
            ReloadMenu();

            MemberDataService.MembersChanged += MemberDataService_MembersChanged;
            showCreator = true;
            isLoaded = true;
        }

        private void MemberDataService_MembersChanged(object sender, EventArgs e)
        {
            UpdateMembers();
            ReloadMenu();

            showCreator = true;
            isLoaded = true;

            StateHasChanged();
        }

        private void ReloadMenu()
        {
            for (int i = 0; i < members.Count; i++)
            {
                leftMenuItem.Add(new MenuItem
                {
                    iconColor = members[i].Avatar,
                    label = members[i].FirstName,
                    referenceId = members[i].Id
                });
            }
        }

        protected void UpdateMembers()
        {
            var result = MemberDataService.Members;

            if (result.Any())
            {
                members = result.ToList();
            }
        }

    }
}
