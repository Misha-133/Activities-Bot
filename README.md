# Activities Bot

Discord bot for starting Discord Activities; Rewritten in C#

[Invite the bot to your server](https://discord.com/api/oauth2/authorize?client_id=897006159913975868&permissions=3073&scope=bot%20applications.commands)


## Run in docker
docker run -d -e ACTIVITIES_BotToken=''
-e ACTIVITIES_ConnectionStrings:ActivitiesBot=''
-e ACTIVITIES_InviteUrl=''
--name activities_bot ghcr.io/misha-133/activities-bot
