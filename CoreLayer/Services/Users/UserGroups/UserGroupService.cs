﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreLayer.ViewModels.Chats;
using DataLayer.Context;
using DataLayer.Entities.Users;
using Microsoft.EntityFrameworkCore;

namespace CoreLayer.Services.Users.UserGroups
{
    public class UserGroupService : BaseService, IUserGroupService
    {
        public UserGroupService(TestChatContext context) : base(context)
        {
        }

        public async Task<List<UserGroupViewModel>> GetUserGroups(long userId)
        {
            var result = await Table<UserGroup>()
                .Include(c => c.ChatGroup.Chats)
                .Include(c => c.ChatGroup.Receiver)
                .Include(c => c.ChatGroup.User)
                .Where(g => g.UserId == userId && !g.ChatGroup.IsPrivate).ToListAsync();

            var model = new List<UserGroupViewModel>();

            foreach (var userGroup in result)
            {
                var chatGroup = userGroup.ChatGroup;
                if (chatGroup.ReceiverId != null)
                    if (userGroup.ChatGroup.ReceiverId == userId)
                        model.Add(new UserGroupViewModel()
                        {
                            ImageName = chatGroup.User.Avatar,
                            GroupName = chatGroup.User.UserName,
                            Token = chatGroup.GroupToken,
                            LastChat = chatGroup.Chats.OrderByDescending(d => d.Id).FirstOrDefault()
                        });
                    else
                        model.Add(new UserGroupViewModel()
                        {
                            ImageName = chatGroup.Receiver.Avatar,
                            GroupName = chatGroup.Receiver.UserName,
                            Token = chatGroup.GroupToken,
                            LastChat = chatGroup.Chats.OrderByDescending(d => d.Id).FirstOrDefault()
                        });
                else
                    model.Add(new UserGroupViewModel()
                    {
                        ImageName = chatGroup.ImageName,
                        GroupName = chatGroup.GroupTitle,
                        Token = chatGroup.GroupToken,
                        LastChat = chatGroup.Chats.OrderByDescending(d => d.Id).FirstOrDefault()
                    });
            }

            return model;
        }

        public async Task JoinGroup(long userId, long groupId)
        {
            var model = new UserGroup()
            {
                CreateDate = DateTime.Now,
                GroupId = groupId,
                UserId = userId
            };
            Insert(model);
            await Save();
        }


        public async Task<bool> IsUserInGroup(long userId, long groupId)
        {
            return await Table<UserGroup>().AnyAsync(g => g.GroupId == groupId && g.UserId == userId);
        }

        public async Task<bool> IsUserInGroup(long userId, string token)
        {
            return await Table<UserGroup>()
                .Include(c => c.ChatGroup)
                .AnyAsync(g => g.ChatGroup.GroupToken == token && g.UserId == userId);

        }
        public async Task<List<string>> GetUserIds(long groupId)
        {
            return await Table<UserGroup>().Where(g => g.GroupId == groupId)
                .Select(s => s.UserId.ToString()).ToListAsync();
        }

        public async Task JoinGroup(List<long> userIds, long groupId)
        {
            foreach (var userId in userIds)
            {
                var model = new UserGroup()
                {
                    CreateDate = DateTime.Now,
                    GroupId = groupId,
                    UserId = userId
                };
                Insert(model);
            }
            await Save();
        }
    }
}