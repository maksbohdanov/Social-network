﻿using AutoMapper;
using BuisnessLogicLayer.Models;
using BuisnessLogicLayer.Models.DTOs;
using DataAccessLayer.Entities;

namespace BuisnessLogicLayer
{
    public class AutomapperProfile: Profile
    {
        public AutomapperProfile()
        {
            CreateMap<Message, MessageDto>()
                .ForMember(dto => dto.Id, x => x.MapFrom(m => m.Id.ToString()))
                .ForMember(dto => dto.AuthorId, x => x.MapFrom(m => m.Author.Id.ToString()))
                .ForMember(dto => dto.ChatId, x => x.MapFrom(m => m.Chat.Id.ToString()))
                .ReverseMap();

            CreateMap<MessageModel, Message>();

            CreateMap<Chat, ChatDto>()
                .ForMember(dto => dto.Id, x => x.MapFrom(c => c.Id.ToString()))
                .ForMember(dto => dto.Users, x => x.MapFrom(c => c.Users.Select(u => u.UserId.ToString())))
                .ForMember(dto => dto.Messages, x => x.MapFrom(c => c.Messages.Select(m => m.ChatId.ToString())))
                .ReverseMap();

            CreateMap<User, UserDto>()
                .ForMember(dto => dto.Id, x => x.MapFrom(u => u.Id.ToString()))
                .ForMember(dto => dto.Chats, x => x.MapFrom(u => u.Chats.Select(c => c.ChatId.ToString())))
                .ReverseMap();

            CreateMap<UserRegistrationModel, User>();                
        }
    }
}
