# Serverless bots

ok, long story short, I just code a bot (BotFramework-v4) that works with Azure Functions.

To run the projects you'll need :
- [Azure Functions Core Tools](https://github.com/Azure/azure-functions-core-tools): `npm i -g azure-functions-core-tools --unsafe-perm true`
- [BotFramework Emulator](https://github.com/microsoft/BotFramework-Emulator/releases)

For any bot, just go to the bot's project, run it with `func start`, and copy the function's URL in the BotFramework Emulator

## Projects
**FunctionApp:** This project has the initial template for an Azure Function App.

**BotFunctionApp:** This was the first iteration. A simple bot that lives inside an azure function.

**BotFunctionAppWithAdapter:** The second iteration. This bot lives inside an azure function AND uses a custom adapter called `BotFrameworkFunctionsAdapter` [(implementation here)](https://github.com/enzocanoo/vOpen2019/blob/master/code/Microsoft.Bot.Builder.Integration.Functions/BotFrameworkFunctionsAdapter.cs) that has direct integration with azure function's model.

**BotFunctionAppWithAdapterAndDI:** Take three!! Same than above AND uses Azure Functions's built-in dependency injection.

**MainProject:**

Sample bot with three responsibilities:
1. Say _hello_ to new users.
2. Tell a joke (yes, just one).
3. Fortune guesser.
Also, it has a _custom_ recognizer that uses regex.

**SecretProject:** Self-explanatory.

**SuperSecretProject:** Super Self-explanatory.

And that's all.

Twitter: @enz_cano

