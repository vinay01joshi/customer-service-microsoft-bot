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
        public async Task StartAsync(IDialogContext context)
        {
            await context.PostAsync("Hi I am Vin-Bot");
            await Respond(context);

            context.Wait(MessageRecievedAsync);          
        }

        public static async Task Respond(IDialogContext context)
        {
            var username = string.Empty;
            context.UserData.TryGetValue<string>("Name", out username);

            if (string.IsNullOrEmpty(username))
            {
                await context.PostAsync("What is your name?");
                context.UserData.SetValue("GetName", true);
            }
            else
            {
                await context.PostAsync(string.Format("Hi {0}. How are you today?", username));
            }
        }

        private async Task MessageRecievedAsync(IDialogContext context, IAwaitable<IMessageActivity> argument)
        {
            var message = await argument;
            var username = string.Empty;
            var getName = false;

            context.UserData.TryGetValue<string>("Name", out username);
            context.UserData.TryGetValue<bool>("GetName", out getName);

            if (getName)
            {
                username = message.Text ;
                context.UserData.SetValue<string>("Name", username);
                context.UserData.SetValue<bool>("GetName", false);
                await Respond(context);
                context.Wait(MessageRecievedAsync);
            }          
            else
            {
                context.Done(message);
            }           
        }
    }
}