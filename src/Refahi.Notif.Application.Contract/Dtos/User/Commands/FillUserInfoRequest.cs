using AutoMapper;
using MediatR;
using Refahi.Notif.Messages.NotifCenter;

namespace Refahi.Notif.Application.Contract.Dtos.User.Commands;

[AutoMap(typeof(FillUserInfo), ReverseMap = true)]
public class FillUserInfoRequest : FillUserInfo, IRequest
{

}