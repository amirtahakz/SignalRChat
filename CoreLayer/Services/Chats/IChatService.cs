﻿using CoreLayer.ViewModels.Chats;
using DataLayer.Entities.Chats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLayer.Services.Chats
{
    public interface IChatService
    {
         Task SendMessage(Chat chat);
        Task<List<ChatViewModel>> GetChatGroup(long groupId);
    }
}
