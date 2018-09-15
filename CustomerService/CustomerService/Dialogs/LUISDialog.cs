using CustomerService.Models;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.FormFlow;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerService.Dialogs
{
    [LuisModel("app-key", "subscription-key", domain: "westus.api.cognitive.microsoft.com", Staging = true)]
    [Serializable]
    public class LUISDialog : LuisDialog<BugReport>
    {
        private readonly BuildFormDelegate<BugReport> _NewBugReport;

        public LUISDialog(BuildFormDelegate<BugReport> NewBugReport)
        {
            this._NewBugReport = NewBugReport;
        }

        [LuisIntent("")]
        public async Task None(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("Sorry , I dont understand what do you mean.");
            context.Wait(MessageReceived);
        }

        [LuisIntent("Greeting")]
        public async Task Greeting(IDialogContext context, LuisResult result)
        {
            context.Call(new GreetingDialog(), Callback);
        }

        [LuisIntent("NewBugReport")]
        public async Task NewBugReport(IDialogContext context, LuisResult result)
        {
            var enrolmentForm = new FormDialog<BugReport>(new BugReport(), this._NewBugReport, FormOptions.PromptInStart);
            context.Call<BugReport>(enrolmentForm, Callback);
        }

        [LuisIntent("QueryBugType")]
        public async Task QueryBugType(IDialogContext context, LuisResult result)
        {
            foreach(var entity in result.Entities.Where(entity => entity.Type == "BugType"))
            {
                var value = entity.Entity.ToLower();
                if (Enum.GetNames(typeof(BugType)).Where(a => a.ToLower().Equals(value)).Count() > 0)
                {
                    await context.PostAsync("Yes that is a bug type.");
                    context.Wait(MessageReceived);
                    return;
                }
                else
                {
                    await context.PostAsync("I am sorry that is not a bug type.");
                    context.Wait(MessageReceived);
                    return;
                }
            }
            await context.PostAsync("I am sorry that is not a bug type.");
            context.Wait(MessageReceived);
            return;
        }

        private async Task Callback(IDialogContext context, IAwaitable<object> result)
        {
            context.Wait(MessageReceived);
        }
    }
}