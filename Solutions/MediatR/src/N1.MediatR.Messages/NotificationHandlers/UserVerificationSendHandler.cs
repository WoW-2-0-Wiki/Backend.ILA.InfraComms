using MediatR;
using N1.MediatR.Messages.Notifications;

namespace N1.MediatR.Messages.NotificationHandlers;

public class UserVerificationSendHandler : INotificationHandler<UserCreatedNotification>
{
    public Task Handle(UserCreatedNotification notification, CancellationToken cancellationToken)
    {
        Console.WriteLine($"Sending verification email to user {notification.UserId}");
        return Task.CompletedTask;
    }
}
