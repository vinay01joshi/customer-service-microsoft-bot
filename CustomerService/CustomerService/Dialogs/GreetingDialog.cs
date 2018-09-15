using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace CustomerService.Dialogs
{
    [Serializable]
    public class GreetingDialog : IDialog
    {
        public Task StartAsync(IDialogContext context)
        {
            context.PostAsync("Hi I am Vin-Bot");
            context.Wait(MessageRecievedAsync);
            return Task.CompletedTask;
        }

        private async Task MessageRecievedAsync(IDialogContext context, IAwaitable<object> result)
        {
            var message = await result as Activity;
            var username = string.Empty;
            var getName = false;

            context.UserData.TryGetValue<string>("Name", out username);
            context.UserData.TryGetValue<bool>("GetName", out getName);

            if (getName)
            {
                username = message.Text ;
                context.UserData.SetValue<string>("Name", username);
                context.UserData.SetValue<bool>("GetName", false);
            }

            if (string.IsNullOrEmpty(username))
            {
                await context.PostAsync("What is your name ?");
                context.UserData.SetValue<bool>("GetName", true);
            }
            else
            {
                await context.PostAsync(string.Format("Hi {0}. How can i help you today ?", username));
            }

            context.Wait(MessageRecievedAsync);
        }
    }
}