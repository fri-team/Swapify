using FRITeam.Swapify.SwapifyBase.Entities.Enums;
using System;

namespace FRITeam.Swapify.SwapifyBase.Entities.Notifications
{
    public class SuccessfulExchangeNotification: Notification
    {
        public Guid ExchangeRequestId { get; set; }

        public static SuccessfulExchangeNotification Create(BlockChangeRequest requestOfUserToNotify,
            BlockChangeRequest requestOfOtherUser)
        {
            return new SuccessfulExchangeNotification(requestOfUserToNotify.TimetableId, requestOfUserToNotify.Id);
        }

        public SuccessfulExchangeNotification()
        {
        }        

        private SuccessfulExchangeNotification(Guid recipientId, Guid exchangeRequestId)
        {
            Type = NotificationType.SuccessfulExchangeNotification;
            CreatedAt = DateTime.Now;
            Read = false;
            RecipientId = recipientId;
            ExchangeRequestId = exchangeRequestId;
        }

        public override string Text => "Výmena bola vykonaná.";
    }
}
