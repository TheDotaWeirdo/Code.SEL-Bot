using System.Threading.Tasks;
using Discord;

namespace Code.SEL_Bot
{
    public class UserJoined
    {
        public bool Joined = false;
        public User User;
        public Discord.Message Mess;
    }

    public class Buddy
    {
        int ID = 0;
        public Role Role;
        Server S;
        public string Description;
        public string Permissions;

        public Buddy(int Id, Server Se)
        {
            switch (Id)
            {
                case 1:
                    {
                        ID = Id; S = Se;
                        Role = S.GetRole(217704302892810250);
                        Description = "";
                        Permissions = "   • View any **Text Channel**\n"
                            + "   • Send messages in the <#155958788447404032> channel\n"
                            + "   • Connect to the **AFK, Main & Music** voice channels\n"
                            + "   • Connect to the **:sunrise_over_mountains: Dota 2** channel\n"
                            + "   • Can not **Invite** other people to the Server\n"
                            + "   • Can not change your **Nickname**\n"
                            + "   • Can not connect to other **Gaming** voice channels";
                        break;
                    }
                case 2:
                    {
                        ID = Id; S = Se;
                        Role = S.GetRole(279698883456794624);
                        Description = "";
                        Permissions = "   • View any **Text Channel**\n"
                            + "   • Send messages in the <#155958788447404032> channel\n"
                            + "   • Connect to the **AFK, Main & Music** voice channels\n"
                            + "   • Connect to the **:rocket: Rocket League** channel\n"
                            + "   • Can not **Invite** other people to the Server\n"
                            + "   • Can not change your **Nickname**\n"
                            + "   • Can not connect to other **Gaming** voice channels\n";
                        break;
                    }
                case 3:
                    {
                        ID = Id; S = Se;
                        Role = S.GetRole(248905559690969102);
                        Description = "";
                        Permissions = "   • View any **Text Channel**\n"
                            + "   • Send messages in the <#155958788447404032> channel\n"
                            + "   • Connect to the **AFK, Main & Music** voice channels\n"
                            + "   • Connect to the **:police_car: GTA V** channel\n"
                            + "   • Can not **Invite** other people to the Server\n"
                            + "   • Can not change your **Nickname**\n"
                            + "   • Can not connect to other **Gaming** voice channels";
                        break;
                    }
                case 4:
                    {
                        ID = Id; S = Se;
                        Role = S.GetRole(248905441185103873);
                        Description = "";
                        Permissions = "   • View any **Text Channel**\n"
                            + "   • Send messages in the <#155958788447404032> channel\n"
                            + "   • Connect to the **AFK, Main & Music** voice channels\n"
                            + "   • Connect to the **:open_hands: Overwatch** channel\n"
                            + "   • Can not **Invite** other people to the Server\n"
                            + "   • Can not change your **Nickname**\n"
                            + "   • Can not connect to other **Gaming** voice channels";
                        break;
                    }
                case 5:
                    {
                        ID = Id; S = Se;
                        Role = S.GetRole(265924787728023553);
                        Description = "";
                        Permissions = "   • View any **Text Channel**\n"
                            + "   • Send messages in the <#155958788447404032> channel\n"
                            + "   • Connect to the **AFK, Main & Music** voice channels\n"
                            + "   • Connect to the **:gun: CS GO** channel\n"
                            + "   • Can not **Invite** other people to the Server\n"
                            + "   • Can not change your **Nickname**\n"
                            + "   • Can not connect to other **Gaming** voice channels";
                        break;
                    }
                default:
                    break;
            }
        }

        internal async Task Select(User user)
        {
            await S.DefaultChannel.SendMessage($"Welcome to <:codesel:249266261928706048> Code.SEL, {user.Mention}, you are now a {Role.Mention}\nI sent you your Role's Permissions\nFor more info, check the <#227865516260196352> for more");
            await user.AddRoles(Role);
            await user.SendMessage($"Here are the permissions for **{Role.Name}**:\n\n{Permissions}");
        }
    }
}
