using System;
using Windows.UI.Popups;

namespace Movie_App.Util
{
    /// <summary>
    ///     Class for displaying error messages.
    /// </summary>
    public static class ErrorMessage
    {
        /// <summary>
        ///     Show simple error message in UI.
        /// </summary>
        /// <param name="message">message text</param>
        /// <param name="buttontext">button text</param>
        public static async void Show(string title, string message, string buttontext)
        {
            var msg = new MessageDialog(message);
            msg.Title = title;
            msg.Commands.Add(new UICommand(buttontext));
            msg.DefaultCommandIndex = 0;
            msg.CancelCommandIndex = 1;

            await msg.ShowAsync();
        }
    }
}