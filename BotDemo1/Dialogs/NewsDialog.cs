using BotDemo1.Models;
using BotDemo1.Services;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Choices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace BotDemo1.Dialogs
{
    public class NewsDialog:ComponentDialog
    {
        private readonly BotStateService _botStateService;
        public NewsDialog(string dialogId,BotStateService botStateService):base(dialogId)
        {
            _botStateService = botStateService ?? throw new System.ArgumentNullException(nameof(botStateService));

            InitializeWaterfallDialog();
        }

        private void InitializeWaterfallDialog()
        {
            var waterfallSteps = new WaterfallStep[]
            {
                InitialStepAsync,
                FinalStepAsync
            };

            AddDialog(new WaterfallDialog($"{nameof(NewsDialog)}.mainflow", waterfallSteps));
            AddDialog(new TextPrompt($"{nameof(NewsDialog)}.description"));

            InitialDialogId = $"{nameof(NewsDialog)}.mainflow";
        }

        private async Task<DialogTurnResult> InitialStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {

            if (Regex.Match(stepContext.Context.Activity.Text.ToLower(), "weather").Success)
            {
                await stepContext.Context.SendActivityAsync(MessageFactory.Text($"The Weather is likely to by sunny with temperature of 30 C in your location."), cancellationToken);
                return await stepContext.NextAsync(null, cancellationToken);
            }
            else if(Regex.Match(stepContext.Context.Activity.Text.ToLower(), "sports").Success)
            {
                await stepContext.Context.SendActivityAsync(MessageFactory.Text($"Here are some of highlights : India lost to Australia in t20 finals"), cancellationToken);
                return await stepContext.NextAsync(null, cancellationToken);
            }
            else
            {
                await stepContext.Context.SendActivityAsync(MessageFactory.Text($"Sorry, The news you are looking for is not present now. please try categories like weather and sports."), cancellationToken);
                return await stepContext.NextAsync(null, cancellationToken);
            }
        }

        private async Task<DialogTurnResult> FinalStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            return await stepContext.EndDialogAsync(null, cancellationToken);
        }
    }
}
