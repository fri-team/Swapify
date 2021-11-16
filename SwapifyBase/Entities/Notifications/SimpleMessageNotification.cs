namespace FRITeam.Swapify.SwapifyBase.Entities.Notifications
{
    public class SimpleMessageNotification: Notification
    {
        public string Message { get; set; }
        public override string Text => Message;
    }
}
