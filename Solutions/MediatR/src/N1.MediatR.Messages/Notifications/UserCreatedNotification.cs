using MediatR;

namespace N1.MediatR.Messages.Notifications;

public class UserCreatedNotification : INotification
{
    public Guid UserId { get; set; }
}