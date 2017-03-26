using Discord;
using DiscordBot;
using System;

namespace Code.SEL_Bot
{
    static class Pref
    {

        public static void Automove(MessageEventArgs e)
        {
            int i = Fcn.Discord.GetA_UsersIndex(e.User);
            bool b = !Program.AllUsers[i].AutoMove;
            Console.WriteLine("Command Found: 'Pref - Automove`");
            Program.AllUsers[i].AutoMove = b;
            Program.AllUsers[i].Update();
            if (b)
            {
                Fcn.Discord.TimedMsg(e.Channel.SendMessage(e.User.Mention + ", your AutoMove preference was changed to `Allow`"), 60000);
            }
            else
            {
                Fcn.Discord.TimedMsg(e.Channel.SendMessage(e.User.Mention + ", your AutoMove preference was changed to `Disabled`"), 60000);
            }
        }

        public static void AFKMove(MessageEventArgs e)
        {
            int i = Fcn.Discord.GetA_UsersIndex(e.User);
            bool b = !Program.AllUsers[i].AFKMove;
            Console.WriteLine("Command Found: 'Pref - AFK Move`");
            Program.AllUsers[i].AFKMove = b;
            Program.AllUsers[i].Update();
            if (b)
            {
                Fcn.Discord.TimedMsg(e.Channel.SendMessage(e.User.Mention + ", your AFK Move preference was changed to `Allow`"), 60000);
            }
            else
            {
                Fcn.Discord.TimedMsg(e.Channel.SendMessage(e.User.Mention + ", your AFK Move preference was changed to `Disabled`"), 60000);
            }
        }
    }
}
