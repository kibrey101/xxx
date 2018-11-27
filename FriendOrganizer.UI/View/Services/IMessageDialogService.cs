namespace FriendOrganizer.UI.View.Services
{
    public interface IMessageDialogService
    {
        MessageDialogresult ShowOkCancelDialog(string text, string title);
    }
}