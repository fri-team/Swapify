using System;
using FRITeam.Swapify.Entities.Enums;

namespace FRITeam.Swapify.Entities.Notifications
{
    public class SuccessfulExchangeNotification: Notification
    {
        private readonly Guid _successfulExchangeRequestId;

        public static SuccessfulExchangeNotification Create(BlockChangeRequest requestOfUserToNotify,
            BlockChangeRequest requestOfOtherUser)
        {
            return new SuccessfulExchangeNotification(requestOfUserToNotify.StudentId, requestOfUserToNotify.Id);
        }

        public SuccessfulExchangeNotification()
        {
        }        

        private SuccessfulExchangeNotification(Guid recipientId, Guid successfulExchangeRequestId)
        {
            Type = NotificationType.SuccessfulExchangeNotification;
            CreatedAt = DateTime.Now;
            Read = false;
            RecipientId = recipientId;
            _successfulExchangeRequestId = successfulExchangeRequestId;
        }

        public override string Text => $"Výmena {_successfulExchangeRequestId} bola vykonaná.";
    }
}
