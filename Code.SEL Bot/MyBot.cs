using Discord;
using Discord.Commands;
using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using Code.SEL_Bot;
using Discord.Audio;

namespace DiscordBot
{
    class MyBot
    {
        /*
        bool Program.KickVoting = true, Program.BanVoting = true, Program.ListUsersWaiting = false, Program.ListUsersStopped = false;
        Kick[] Program.VoteKick; Ban[] Program.VoteBan; Exile[] Program.Exiles; TypeRace[] Program.TRs; TicTacToe[] Program.TTT; Hangman[] Program.Hang; Connect4[] Program.ConnectF;
        public static A_Users[] Program.AllUsers;       
        public static DiscordClient discord;
        CommandService commands;
        Random Program.rand;
        Channel Program.DefChan;
        List<Buddy> Program.Buddies = new List<Buddy>();
        List<UserJoined> Program.UserJoin = new List<UserJoined>();
        */
        public static DiscordClient discord;
        CommandService commands;
        public MyBot()
        {
            Program.rand = new Random();
            Array.Resize(ref Program.Hang, 1);
            Program.Hang[0] = new Hangman(null, null, null)
            { Finished = true };
            Array.Resize(ref Program.TTT, 1);
            Program.TTT[0] = new TicTacToe()
            { Finished = true };
            Array.Resize(ref Program.ConnectF, 1);
            Program.ConnectF[0] = new Connect4()
            { Finished = true };
            Array.Resize(ref Program.Exiles, 1);
            Program.Exiles[0] = new Exile(0);
            Array.Resize(ref Program.TRs, 1);
            Program.TRs[0] = new TypeRace();
            Array.Resize(ref Program.VoteKick, 1);
            Program.VoteKick[0] = new Kick();
            Array.Resize(ref Program.VoteBan, 1);
            Program.VoteBan[0] = new Ban();

            discord = new DiscordClient(x =>
            {
                x.LogLevel = LogSeverity.Info;
                x.LogHandler = Log;
            });
            discord.UsingAudio(x => // Opens an AudioConfigBuilder so we can configure our AudioService
            {
                x.Mode = AudioMode.Outgoing; // Tells the AudioService that we will only be sending audio
            });

            discord.UsingCommands(x => {
                x.PrefixChar = '-';
                x.AllowMentionPrefix = true;
            });

            commands = discord.GetService<CommandService>();
            {
                MsgDel();
                MsgLog();
                Data_Logging();
                OnConnect();
                Greet();
                UserEvents();
                BuddySelection();
            }

            try
            {
                discord.ExecuteAndWait(async () =>
                {
                    await discord.Connect("MjcyMzgzNzI2MzAwNjkyNTAw.C6vaeg.RW2y41Yg3ftp_4jojk7tpJF6lX4", TokenType.Bot);
                });
            }
            catch(System.Net.WebException)
            {
                System.Threading.Thread.Sleep(1000);
                try
                {
                    discord.ExecuteAndWait(async () =>
                    {
                        await discord.Connect("MjcyMzgzNzI2MzAwNjkyNTAw.C2VeHA.t1NbSsVElbtk8Di9ahwF0tGhm9M", TokenType.Bot);
                    });
                }
                catch (System.Net.WebException)
                {
                    discord.Disconnect();
                }
            }
        }

        public void Debug(string S)
        {
            Console.WriteLine("\nDEBUG: " + S + "\n");
        }

        protected virtual bool IsFileLocked(FileInfo file)
        {
            FileStream stream = null;

            try
            {
                stream = file.Open(FileMode.Open, FileAccess.Read, FileShare.None);
            }
            catch (IOException)
            {
                //the file is unavailable because it is:
                //still being written to
                //or being processed by another thread
                //or does not exist (has already been processed)
                return true;
            }
            finally
            {
                if (stream != null)
                {
                    stream.Close();
                }
            }

            //file is not locked
            return false;
        }

        public string GetMeme(string Type)
        {
            string[] MemeTriggers = { "meme", "mem", "memes", "mems" }, FunnyTriggers = { "funny", "fun", "picture", "pic" }, GifTriggers = { "gif", "gifs" },
            MemeLinks =
            {
                "http://i.imgur.com/47fO3yf.png",
                "http://i.imgur.com/tfhEJ3d.jpg",
                "http://i.imgur.com/kFqKKp7g.png",
                "http://i.imgur.com/Z1ux5ZC.png",
                "http://i.imgur.com/I8cEkjo.jpg",
                "../../meme/mem1.jpg",
                "../../meme/mem2.jpg",
                "../../meme/mem3.jpg",
                "../../meme/mem4.jpg",
                "../../meme/mem5.jpg",
                "../../meme/mem6.jpg",
                "../../meme/mem7.jpg",
                "../../meme/mem8.jpg",
                "../../meme/mem9.jpg",
                "../../meme/mem10.jpg",
                "../../meme/mem11.jpg",
                "../../meme/mem12.jpg",
                "../../meme/mem13.png",
                "../../meme/mem14.png",
                "../../meme/mem15.png",
                "../../meme/mem16.png",
                "../../meme/mem17.png",
                "../../meme/mem18.png",
                "../../meme/mem19.png",
                "../../meme/mem20.png"
            },
            FunnyLinks =
            {
                "http://i.imgur.com/qeUZrvX.jpg",
                "http://i.imgur.com/XsLq0We.png",
                "http://i.imgur.com/oPElr97.png",
                "http://i.imgur.com/Z9809Jeg.jpg",
                "http://i.imgur.com/x9UGQ8yg.jpg",
                "http://i.imgur.com/o9bU0V5.jpg",
                "http://i.imgur.com/3TUkxSx.jpg",
                "http://i.imgur.com/K54BV7b.jpg",
                "http://i.imgur.com/jooNikL.jpg",
                "http://i.imgur.com/ZnVUXCa.jpg",
                "http://i.imgur.com/iyHfcle.jpg",
                "http://i.imgur.com/iWe5T6f.jpg",
                "http://i.imgur.com/sVxsGgd.jpg",
                "http://i.imgur.com/77uULHZ.jpg",
                "http://i.imgur.com/dnVi3WX.jpg",
                "http://i.imgur.com/Mp8ZuJc.jpg",
                "http://i.imgur.com/Sx7aUJL.jpg",
                "http://i.imgur.com/wsFMxYn.jpg",
                "http://i.imgur.com/rmjAkj4g.jpg",
                "http://i.imgur.com/YsmMWN5.jpg",
                "http://i.imgur.com/BpPc4oJg.jpg",
                "http://i.imgur.com/LpeW976.jpg",
                "http://i.imgur.com/djR4Xw7g.jpg",
                "http://i.imgur.com/fVV4iljg.jpg",
                "http://i.imgur.com/tfhEJ3d.jpg",
                "http://i.imgur.com/6RaUAYY.jpg",
                "http://i.imgur.com/3SFZVCu.jpg",
                "http://i.imgur.com/Z1ux5ZC.png",
                "http://i.imgur.com/sO32fY4.jpg?1",
                "http://i.imgur.com/m20soU5.png",
                "http://i.imgur.com/j4k07C7g.jpg",
                "http://i.imgur.com/wfZq56Eg.jpg",
                "http://i.imgur.com/I8cEkjo.jpg"
            },
            GifLinks =
            {
                /*"http://i.imgur.com/gf1jAxK.gif",
                "https://media.giphy.com/media/90Vqtqrg7kS2s/giphy.gif",
                "https://media.giphy.com/media/12Xuickl5bdGec/giphy.gif",
                "https://media.giphy.com/media/12Xuickl5bdGec/giphy.gif",
                "https://media.giphy.com/media/14jkPsejZkOKuQ/giphy.gif",
                "https://media.giphy.com/media/zH33GE7Dwmwuc/giphy.gif",
                "https://media.giphy.com/media/kjDOCRbawN1PG/source.gif",
                "https://media.giphy.com/media/u6X58nPZbNU5y/giphy.gif",
                "https://media.giphy.com/media/PqAHfunnTPRyU/giphy.gif",
                "https://media.giphy.com/media/791BQu3yl3NHq/giphy.gif",*/
                "https://gfycat.com/SilentComfortableBighornedsheep",
                "https://gfycat.com/SpiritedMaleAustraliankestrel",
                "https://gfycat.com/FrequentOffbeatBarebirdbat"
            };
            if (Fcn.Msg.CommandCheck(Type, MemeTriggers, false))
            {
                int i = Program.rand.Next(MemeLinks.Length);
                Console.WriteLine("Action: Sending File /Type: Meme /ID: " + i);
                if (MemeLinks[i].StartsWith("http") && (MemeLinks[i][MemeLinks[i].Length - 4] == '.'))
                {
                    FileInfo I = new FileInfo("../../meme/meme" + i + "." + MemeLinks[i].Substring(MemeLinks[i].Length - 3));
                    using (WebClient webClient = new WebClient())
                    {
                        webClient.DownloadFileAsync(new Uri(MemeLinks[i]), "../../meme/meme" + i + "." + MemeLinks[i].Substring(MemeLinks[i].Length - 3));
                    }

                    while (IsFileLocked(I))
                    {
                        ;
                    }

                    System.Threading.Thread.Sleep(10);
                    return "../../meme/meme" + i + "." + MemeLinks[i].Substring(MemeLinks[i].Length - 3);
                }
                return MemeLinks[i];
            }
            else if (Fcn.Msg.CommandCheck(Type, FunnyTriggers, false))
            {
                int i = Program.rand.Next(FunnyLinks.Length);
                Console.WriteLine("Action: Sending File /Type: Funny /ID: " + i);
                if (FunnyLinks[i].StartsWith("http") && (FunnyLinks[i][FunnyLinks[i].Length - 4] == '.'))
                {
                    FileInfo I = new FileInfo("../../meme/Funny" + i + "." + FunnyLinks[i].Substring(FunnyLinks[i].Length - 3));
                    using (WebClient webClient = new WebClient())
                    {
                        webClient.DownloadFileAsync(new Uri(FunnyLinks[i]), "../../meme/Funny" + i + "." + FunnyLinks[i].Substring(FunnyLinks[i].Length - 3));
                    }

                    while (IsFileLocked(I))
                    {
                        ;
                    }

                    System.Threading.Thread.Sleep(10);
                    return "../../meme/Funny" + i + "." + FunnyLinks[i].Substring(FunnyLinks[i].Length - 3);
                }
                return FunnyLinks[i];
            }
            else if (Fcn.Msg.CommandCheck(Type, GifTriggers, false))
            {
                int i = Program.rand.Next(GifLinks.Length);
                Console.WriteLine("Action: Sending File /Type: Gif /ID: " + i);
                if (GifLinks[i].StartsWith("http") && (GifLinks[i][GifLinks[i].Length - 4] == '.'))
                {
                    FileInfo I = new FileInfo("../../meme/Gif" + i + "." + GifLinks[i].Substring(GifLinks[i].Length - 3));
                    using (WebClient webClient = new WebClient())
                    {
                        webClient.DownloadFileAsync(new Uri(GifLinks[i]), "../../meme/Gif" + i + "." + GifLinks[i].Substring(GifLinks[i].Length - 3));
                    }

                    while (IsFileLocked(I))
                    {
                        ;
                    }

                    System.Threading.Thread.Sleep(10);
                    return "../../meme/Gif" + i + "." + GifLinks[i].Substring(GifLinks[i].Length - 3);
                }
                return GifLinks[i];
            }
            return "../../meme/Error.png";
        }

        public int GetListCounter(IEnumerable<User> Users, char Startingwith, bool Online, bool OnlineOption)
        {
            bool include; int Counter = 0;
            foreach (User U in Users)
            {
                if (U != null)
                {
                    include = true;
                    if ((Startingwith != '~') && !(U.Name.ToString()[0].ToString().ToLower() == Startingwith.ToString().ToLower() || (U.Nickname != null && U.Nickname.ToString()[0].ToString().ToLower() == Startingwith.ToString().ToLower())))
                    {
                        include = false;
                    }

                    if (OnlineOption && Online && U.Status.Value.ToString() != "online")
                    {
                        include = false;
                    }
                    else if (OnlineOption && !Online && U.Status.Value.ToString() != "offline")
                    {
                        include = false;
                    }

                    if (include)
                    {
                        Counter++;
                    }
                }
            }
            return Counter;
        }

        public IEnumerable<User> SortUsers(IEnumerable<User> Users)
        {
            User[] U = null, Out = null;
            Array.Resize(ref U, 1); Array.Resize(ref Out, 1);
            U[0] = null; Out[0] = null;
            for (int p = 97; p < 123; p++)
            {
                foreach (User Q in Sort(Users, char.ConvertFromUtf32(p)[0], 1))
                {
                    Array.Resize(ref U, U.Length + 1);
                    U[U.Length - 1] = Q;
                }
            }
            for (int i = 97; i < 123; i++)
            {
                foreach (User Q in U)
                {
                    if (Q != null)
                    {
                        if (Q.Name.ToLower()[0] == char.ConvertFromUtf32(i)[0])
                        {
                            Array.Resize(ref Out, Out.Length + 1);
                            Out[Out.Length - 1] = Q;
                        }
                    }
                }
            }
            return Out;
        }

        public IEnumerable<User> Sort(IEnumerable<User> Users, char c, int index)
        {
            User[] U = { null };
            foreach (User u in Users)
            {
                if (u.Name.ToLower()[Math.Min(u.Name.Length - 1, index)] == c)
                {
                    Array.Resize(ref U, U.Length + 1);
                    U[U.Length - 1] = u;
                }
            }
            return U;
        }

        public void UpdateStats()
        {
            string[] XO_Data = File.ReadAllLines("../../XO_Data.txt"), Hang_Data = File.ReadAllLines("../../Hang_Data.txt"), Data = File.ReadAllLines("../../Data.txt");
            int XO_S_Wins = 0, XO_NS_Wins = 0, XO_Ties = 0, Hang_Wins = 0, Bot_Matches = 0, New_Matches = 0, Hang_GamesWithDesc = 0;
            double XO_Avg_Moves = 0, XO_Winrate_VS_Bot = 0, Hang_Avg_Letters = 0, Hang_Avg_Word_Length = 0, Hang_Avg_Helps = 0, Hang_Avg_Stage = 0;
            foreach (string XO in XO_Data)
            {
                if (!string.IsNullOrWhiteSpace(XO) && !XO.Contains("Incomplete"))
                {
                    if (XO[18] == 'T')
                    {
                        XO_Ties++;
                    }

                    if (XO[18] == XO[27])
                    {
                        XO_S_Wins++;
                    }
                    else if (XO[18] != 'T')
                    {
                        XO_NS_Wins++;
                    }

                    XO_Avg_Moves = XO_Avg_Moves + ((Convert.ToDouble(XO.Substring(36, 1))) / (Convert.ToDouble(Data[0].Substring(8))));
                    if (XO[43] == 'Y')
                    {
                        Bot_Matches++;
                    }

                    if (XO[18] == 'X' && XO[43] == 'Y')
                    {
                        XO_Winrate_VS_Bot++;
                    }

                    if (XO[18] == 'T' && XO[43] == 'Y')
                    {
                        XO_Winrate_VS_Bot = XO_Winrate_VS_Bot + 0.5;
                    }
                }
            }
            XO_Winrate_VS_Bot = XO_Winrate_VS_Bot * 100 / Bot_Matches;
            foreach (string H in Hang_Data)
            {
                if (!string.IsNullOrWhiteSpace(H) && !H.Contains("Incomplete"))
                {
                    New_Matches++;
                    if (H[19] == 'Y')
                    {
                        Hang_Wins++;
                    }
                    Hang_Avg_Stage += double.Parse(Fcn.Msg.Between(H, "$Stage\"", "\" "));
                    Hang_Avg_Helps += double.Parse(Fcn.Msg.Between(H, "$Helps\"", "\" "));
                    Hang_Avg_Letters += Fcn.Msg.Between(H, "$Letters\"", "\" ").Length;
                    Hang_Avg_Word_Length += H.Substring(H.IndexOf("$Word:") + 6).Length;
                    if (Fcn.Msg.Between(H, "$Description\"", "\" ") != "")
                        Hang_GamesWithDesc++;
                }
            }
            Hang_Avg_Stage = Hang_Avg_Stage / New_Matches;
            Hang_Avg_Helps = Hang_Avg_Helps / New_Matches;
            Hang_Avg_Letters = Hang_Avg_Letters / New_Matches;
            Hang_Avg_Word_Length = Hang_Avg_Word_Length / New_Matches;

            Data[2] = "XO_S_Wins = " + XO_S_Wins.ToString();
            Data[3] = "XO_NS_Wins = " + XO_NS_Wins.ToString();
            Data[4] = "XO_Ties = " + XO_Ties.ToString();
            Data[5] = "XO_Avg_Moves = " + XO_Avg_Moves.ToString().Substring(0, 3);
            Data[6] = "XO_Winrate_VS_Bot = " + XO_Winrate_VS_Bot.ToString().Substring(0, Math.Min(5, XO_Winrate_VS_Bot.ToString().Length));

            Data[10] = "Hang_Wins = " + Hang_Wins.ToString();
            Data[11] = "Hang_Avg_Letters = " + Hang_Avg_Letters.ToString().Substring(0, 5);
            Data[12] = "Hang_Avg_Word_Length = " + Hang_Avg_Word_Length.ToString().Substring(0, 2);
            Data[13] = "Hang_Avg_Helps = " + Hang_Avg_Helps.ToString().Substring(0, 4);
            Data[14] = "Hang_Avg_Stage = " + Hang_Avg_Stage.ToString().Substring(0, 1);
            Data[17] = "Hang_WithDesc = " + Hang_GamesWithDesc;
            Data[18] = "Hang_TotGames = " + New_Matches;
            File.WriteAllLines("../../Data.txt", Data);
            Console.WriteLine("Event: Data Updated");
        }

        public string GetStats(string Game)
        {
            string[] Data = File.ReadAllLines("../../Data.txt");
            string S = "";
            if (Game == "XO")
            {
                S = S + "Total XO Games played:           " + Data[0].Substring(8);
                S = S + "\nTotal Tied Games:                " + Data[4].Substring(10);
                S = S + "\nTotal Games Won by Starter:      " + Data[2].Substring(12);
                S = S + "\nTotal Games Won by non-Starter:  " + Data[3].Substring(13);
                S = S + "\nAverage Moves per Match:         " + Data[5].Substring(15);
                S = S + "\nAverage Winrate against the Bot: " + Data[6].Substring(20);
                return S;
            }
            else if (Game == "Program.Hang")
            {
                S = S + "Total Hangman Games played:      " + Data[1].Substring(13);
                S = S + "\nTotal Hangman Games Won:         " + Data[10].Substring(12);
                S = S + "\nTotal Games with a Description:  " + Data[17].Substring(16);
                S = S + "\nAverage Word Length:             " + Data[12].Substring(23);
                S = S + "\nAverage Letters used per Game:   " + Data[11].Substring(19);
                S = S + "\nAverage Stage at Game End:       " + Data[14].Substring(17);
                S = S + "\nAverage Helps per Game:         " + Data[13].Substring(16);
                return S;
            }
            return "";
        }
        
        public void Backup()
        {
            File.WriteAllLines("../../User_Data_BACKUP.txt", File.ReadAllLines("../../User_Data.txt"));
            File.WriteAllLines("../../XO_Data_BACKUP.txt", File.ReadAllLines("../../XO_Data.txt"));
            File.WriteAllLines("../../Hang_Data_BACKUP.txt", File.ReadAllLines("../../Hang_Data.txt"));
            File.WriteAllLines("../../Connect_Data_BACKUP.txt", File.ReadAllLines("../../Connect_Data.txt"));
            File.WriteAllLines("../../Data_BACKUP.txt", File.ReadAllLines("../../Data.txt"));
        }

        public void PlayError(User u)
        {
            return;
            /*if (u.VoiceChannel == null) return;
            IAudioClient IA = await discord.GetService<AudioService>() 
                .Join(u.VoiceChannel);

            System.Timers.Timer T = new System.Timers.Timer(6000);
            T.Start();
            T.Elapsed += (s, e) => { T.Stop(); IA.Disconnect(); };
            var channelCount = discord.GetService<AudioService>().Config.Channels; // Get the number of AudioChannels our AudioService has been configured to use.
            var OutFormat = new WaveFormat(48000, 16, channelCount); // Create a new Output Format, using the spec that Discord will accept, and with the number of channels that our client supports.
            using (var MP3Reader = new Mp3FileReader("../../MipMup.mp3")) // Create a new Disposable MP3FileReader, to read audio from the filePath parameter
            using (var resampler = new MediaFoundationResampler(MP3Reader, OutFormat)) // Create a Disposable Resampler, which will convert the read MP3 data to PCM, using our Output Format
            {
                resampler.ResamplerQuality = 60; // Set the quality of the resampler to 60, the highest quality
                int blockSize = OutFormat.AverageBytesPerSecond / 50; // Establish the size of our AudioBuffer
                byte[] buffer = new byte[blockSize];
                int byteCount;

                while ((byteCount = resampler.Read(buffer, 0, blockSize)) > 0) // Read audio into our buffer, and keep a loop open while data is present
                {
                    if (byteCount < blockSize)
                    {
                        // Incomplete Frame
                        for (int i = byteCount; i < blockSize; i++)
                            buffer[i] = 0;
                    }
                    IA.Send(buffer, 0, blockSize); // Send the buffer to Discord
                }
                await IA.Disconnect();
            }*/
        }

        /*********************************** ----- Bot Commands -----***********************************/

        private void MsgLog()
        {
            discord.MessageUpdated += async (s, e) =>
            {
                //---------------------Info Update---------------------//
                if (e.Channel.Id == 227865516260196352)
                {
                    Discord.Message MS = Fcn.Discord.WaitForMsg(await e.Server.DefaultChannel.SendTTSMessage("Server Informations were updated"));
                    Console.WriteLine("Action: Notified Default Channel about update of Server Informations");
                    await MS.Delete();
                }
            };
            discord.MessageReceived += async (s, e) =>
            {
                string msg = e.Message.RawText, output, S, M;
                string[] CmdTriggers;
                Discord.Message MSG;
                //---------------------Console---------------------//
                if (msg == "")
                {
                    msg = "File Sent";
                }

                if (msg.Length > 1)
                {
                    try
                    {
                        while (msg.Contains("<@!"))
                        {
                            msg = Fcn.Msg.Remove(msg.Insert(msg.IndexOf("<@!"), '@'+e.Server.GetUser(ulong.Parse(Fcn.Msg.Between(msg, "<@!", '>'))).Name), "<@!" + (Fcn.Msg.Between(msg, "<@!", '>') + ">"));
                        }
                    }
                    catch (FormatException) { }
                    try
                    {
                        while (msg.Contains("<@"))
                        {
                            msg = Fcn.Msg.Remove(msg.Insert(msg.IndexOf("<@"), '@' + e.Server.GetUser(ulong.Parse(Fcn.Msg.Between(msg, "<@", '>'))).Name), "<@" + (Fcn.Msg.Between(msg, "<@", '>') + ">"));
                        }
                    }
                    catch (FormatException) { } catch(NullReferenceException) { }
                    if (msg.Length > 200)
                    {
                        msg = "*Message too long to be displayed*";
                    }

                    if (msg == "")
                    {
                        msg = "File Sent";
                    }

                    Console.WriteLine("#" + e.Channel.Name + " | " + Fcn.Msg.ConditionalString("Music FredBoat", e.User.Id == 184405311681986560) + e.User.Name + ": " + msg);
                }
                //---------------------Announcements---------------------//
                if (e.Channel.Id == 218475383845027843)
                {
                    MSG = Fcn.Discord.WaitForMsg(await e.Server.DefaultChannel.SendTTSMessage("new server announcement"));
                    Console.WriteLine("Action: Notified Default Channel about new Server Announcement");
                    await MSG.Delete();
                }
                //------------------------------------------//
                if (!e.User.IsBot && msg.Length > 1)
                {
                    Console.Write("Action: Checking for Commands --- ");
                }
                //---------------------To Emoji---------------------//
                CmdTriggers = new string[] { "!e", "toemoji", "emoji" };
                if (!e.User.IsBot && Fcn.Msg.CommandCheck(e.Message.RawText, CmdTriggers))
                {
                    await e.Channel.SendIsTyping();
                    Console.WriteLine("Command Found: 'To Emoji'");
                    await e.Message.Delete();
                    M = Fcn.Msg.Decrypt(e.Message.RawText, CmdTriggers);
                    Console.WriteLine("Action: Proccessing Emoji Request by " + e.User.Name + " for " + M);
                    S = Fcn.Msg.ToEmoji(M);
                    await e.Channel.SendMessage(S);
                    return;
                }
                //---------------------Say---------------------//
                CmdTriggers = new string[] { "say " };
                if (!e.User.IsBot && Fcn.Msg.CommandCheck(e.Message.RawText, CmdTriggers))
                {
                    await e.Channel.SendIsTyping();
                    Console.WriteLine("Command Found: 'Say'");
                    await e.Message.Delete();
                    await e.Channel.SendMessage(Fcn.Msg.Decrypt(e.Message.RawText, CmdTriggers));
                    return;
                }
                //---------------------Server Info---------------------//
                CmdTriggers = new string[] { "server", "serverinfo" };
                if (!e.User.IsBot && Fcn.Msg.CommandCheck(e.Message.RawText, CmdTriggers))
                {
                    await e.Channel.SendIsTyping();
                    Console.WriteLine("Command Found: 'Server Info'");
                    output = "**Code.SEL Server Info:**";
                    Console.WriteLine("Action: Sending Server Information to #" + e.Channel.Name);
                    MSG = await e.Channel.SendMessage(":speech_balloon: ");
                    await e.Message.Delete();
                    FileInfo I = new FileInfo("../../meme/Server.jpg");
                    using (WebClient webClient = new WebClient())
                    {
                        webClient.DownloadFileAsync(new Uri(e.Server.IconUrl), "../../meme/Server.jpg");
                    }

                    while (IsFileLocked(I))
                    {
                        System.Threading.Thread.Sleep(10);
                    }

                    await e.Channel.SendFile("../../meme/Server.jpg");
                    await MSG.Delete();
                    output += "\n\nServer ID: `" + e.Server.Id.ToString() + "`\n";
                    output += "\n`" + Fcn.Discord.CountOnlineUsers(e.Server) + "/" + (e.Server.UserCount - Convert.ToInt32(Fcn.Discord.CountOnlineUsers(e.Server))) + "` Users Online right now\n";
                    output += "\nServer Owner: <@155955179852791809>\n\nDefault Channel: " + e.Server.DefaultChannel.Mention;
                    output += "\n\nServer Invite Links:\n";
                    IEnumerable<Discord.Invite> Invs = await e.Server.GetInvites();
                    foreach (Discord.Invite Inv in Invs)
                    {
                        output += "`https://discord.gg/" + Inv.Code + "` " + e.Server.GetChannel(Inv.Channel.Id).Mention + "\n";
                    }

                    IEnumerable<Discord.Role> Roles = e.Server.Roles;
                    output += "\nRoles:";
                    for (int P = Roles.Count(); P > 0; P--)
                    {
                        foreach (Discord.Role R in Roles)
                        {
                            if (R.IsMentionable && R.Position == P)
                            {
                                output += "\n`ID: " + R.Id + "` " + R.Mention + " `#" + -(P - Roles.Count()) + "` includes " + Fcn.Discord.CountUsersInRole(R, e.Server);
                            }
                        }
                    }
                    await e.Channel.SendMessage(output + "\n\n For more info, go to <#227865516260196352>");
                    return;
                }
                //---------------------Help---------------------//
                if (Fcn.Msg.CommandCheck(e.Message.RawText, "help", true))
                {
                    await e.Channel.SendIsTyping();
                    Console.WriteLine("Command Found: 'Help'");
                    if (e.User.Roles.Count() == 1)
                    {
                        Console.WriteLine("Event: Help triggered by " + e.User.Name + " without any role");Program.UserJoin.Add(new UserJoined());
                        Program.UserJoin.Last().Joined = true;
                        Program.UserJoin.Last().User = e.User;
                        Program.UserJoin.Last().Mess = await e.Server.DefaultChannel.SendMessage($"Hello, " + e.User.Mention + "!\n\nWelcome to Code.SEL, please specifiy what category you are in:\n      1. " + e.Server.FindRoles("Dota Buddy").FirstOrDefault().Mention + "\n      2. " + e.Server.FindRoles("Rocket League Buddy").FirstOrDefault().Mention + "\n      3. " + e.Server.FindRoles("GTA Buddy").FirstOrDefault().Mention + "\n      4. " + e.Server.FindRoles("Overwatch Buddy").FirstOrDefault().Mention + "\n      5. " + e.Server.FindRoles("CS GO Buddy").FirstOrDefault().Mention + "\n\n*send a message starting with a dash `-` and the number. Ex: `-1`*");
                        return;
                    }
                    else
                    {
                        Console.WriteLine("Event: " + e.User.Name + " requested Help");
                        string responce =
                            "Here are the commands you can use with me:\n" +
                            "```help\n" +
                            "hi - hey - sup - oy - wassup - hello\n" +
                            "spam - annoy - anoy <User>\n" +
                            "callbot - bot\n" +
                            "fuck - damn <Text>\n" +
                            "meme - mem\n" +
                            "funny - fun - pic - picture\n" +
                            "gif\n" +
                            "!e - toemoji - emoji <Text>\n" +
                            "clean - clear <Number>\n" +
                            "disconnect - bye\n" +
                            "serverinfo - server\n" +
                            "user - info <User>\n" +
                            "users - listusers <Arguments>\n" +
                            "votekick - kick <User>\n" +
                            "stopkick - cancelkick\n" +
                            "startkick - continuekick\n" +
                            "```For more info, check the PDF below..";
                        Fcn.Discord.WaitForMsg(await e.Channel.SendMessage(responce));
                        Console.WriteLine("Action: Sending PDF File");
                        await e.Channel.SendFile("D:/Desktop/Documents/Photoshop Projects/Code.SEL Reborn/Bot Commands.pdf");
                        return;
                    }
                }
                //---------------------Preferences---------------------//
                CmdTriggers = new string[] { "pref" };
                if (!e.User.IsBot && Fcn.Msg.CommandCheck(e.Message.RawText, CmdTriggers))
                {
                    msg = Fcn.Msg.Decrypt(e.Message.RawText, CmdTriggers).ToLower();
                    switch (msg)
                    {
                        case "automove": { await e.Message.Delete(); Pref.Automove(e); return; }
                        case "afkmove": { await e.Message.Delete(); Pref.AFKMove(e); return; }
                        default:
                            break;
                    }
                }
                //---------------------Invite---------------------//
                CmdTriggers = new string[] { "invite" };
                if (!e.User.IsBot && Fcn.Msg.CommandCheck(e.Message.RawText, CmdTriggers, false))
                {
                    await e.Channel.SendIsTyping();
                    Console.WriteLine("Command Found: 'Create Invite'");
                    Discord.Invite Inv = await e.Server.CreateInvite();
                    await e.Channel.SendMessage("Here's an invite for the Server: `https://discord.gg/" + Inv.Code + "`");
                    Console.WriteLine("Event: Created invite to the Server with URL: https://discord.gg/" + Inv.Code);
                    return;
                }
                //---------------------User Listing---------------------//
                CmdTriggers = new string[] { "listusers", "users" };
                if (!e.User.IsBot && Fcn.Msg.CommandCheck(e.Message.RawText, CmdTriggers))
                {
                    await e.Channel.SendIsTyping();
                    Console.WriteLine("Command Found: 'User Listing'"); output = "";
                    char Startingwith = '~';
                    msg = e.Message.RawText.ToLower();
                    int Counter = 0, Total;
                    bool Online = false, OnlineOption = false, include;
                    MSG = await e.Channel.SendMessage(":speech_balloon:");
                    await e.Channel.SendIsTyping();
                    if (msg.Contains("online"))
                    {
                        Online = true;
                        OnlineOption = true;
                        if (msg.Contains("#"))
                        {
                            Startingwith = msg[msg.IndexOf('#') + 1];
                            Console.WriteLine("Event: User Listing requested by " + e.User.Name + " with Online and Starting with" + Startingwith + " parameters");
                        }
                        else
                        {
                            Console.WriteLine("Event: User Listing requested by " + e.User.Name + " with Online parameter");
                        }
                    }
                    else if (msg.Contains("offline"))
                    {
                        Online = false;
                        OnlineOption = true;
                        if (msg.Contains("#"))
                        {
                            Startingwith = msg[msg.IndexOf('#') + 1];
                            Console.WriteLine("Event: User Listing requested by " + e.User.Name + " with Offline and Starting with" + Startingwith + " parameters");
                        }
                        else
                        {
                            Console.WriteLine("Event: User Listing requested by " + e.User.Name + " with Offline parameter");
                        }
                    }
                    else
                    {
                        if (msg.Contains("#"))
                        {
                            Startingwith = msg[msg.IndexOf('#') + 1];
                            Console.WriteLine("Event: User Listing requested by " + e.User.Name + " with Starting with" + Startingwith + " parameter");
                        }
                        else
                        {
                            Console.WriteLine("Event: User Listing requested by " + e.User.Name + " with no parameters");
                        }
                    }

                    Program.ListUsersStopped = false;
                    IEnumerable<User> Users = SortUsers(e.Server.Users);
                    Total = GetListCounter(Users, Startingwith, Online, OnlineOption);
                    if (msg.Contains("desc") || msg.Contains("dsc"))
                    {
                        Users = Users.Reverse();
                    }

                    foreach (User U in Users)
                    {
                        if (U != null)
                        {
                            if (!Program.ListUsersStopped)
                            {
                                include = true;
                                if ((Startingwith != '~') && !(U.Name.ToString()[0].ToString().ToLower() == Startingwith.ToString().ToLower() || (U.Nickname != null && U.Nickname.ToString()[0].ToString().ToLower() == Startingwith.ToString().ToLower())))
                                {
                                    include = false;
                                }

                                if (OnlineOption && Online && U.Status.Value.ToString() != "online")
                                {
                                    include = false;
                                }
                                else if (OnlineOption && !Online && U.Status.Value.ToString() != "offline")
                                {
                                    include = false;
                                }

                                if (include)
                                {
                                    Counter++;
                                    output += Fcn.Discord.GetUserInfo(U) + "\n\n";
                                }

                                if (U == Users.Last())
                                {
                                    if (Counter == 0)
                                    {
                                        await e.Channel.SendMessage("**:exclamation: No Results Found :exclamation: **"); output = "";
                                        Console.WriteLine("Event: User Listing did not find any Results with the specified parameters");
                                    }
                                    else
                                    {
                                        Counter = 0;
                                        output += ("-------------------------------\n**Listing Complete  :white_check_mark: **");
                                        await e.Channel.SendMessage(output); output = "";
                                        Console.WriteLine("Event: User Listing Completed with " + GetListCounter(Users, Startingwith, Online, OnlineOption) + " results");
                                        await MSG.Delete();
                                    }
                                }
                                else if (Counter > 4)
                                {
                                    if (Counter == Total)
                                    {
                                        Program.ListUsersStopped = true;
                                        output += ("-------------------------------\n**Listing Complete  :white_check_mark: **");
                                        Console.WriteLine("Event: User Listing Completed with " + GetListCounter(Users, Startingwith, Online, OnlineOption) + " results");
                                        await e.Channel.SendMessage(output); output = "";
                                        await MSG.Delete();
                                    }
                                    else
                                    {
                                        Total = Total - Counter;
                                        Counter = 0;
                                        output += ("Continue List?  >  Yes: `-continuelist`  >  No: `-stoplist`");
                                        Console.WriteLine("Event: User Listing Limit reached");
                                        await e.Channel.SendMessage(output); output = "";
                                        await MSG.Delete();
                                        Program.ListUsersWaiting = true;
                                        while (Program.ListUsersWaiting || Program.ListUsersStopped)
                                        {
                                            if (Program.ListUsersStopped)
                                            {
                                                break;
                                            }

                                            await Task.Delay(100);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    return;
                }
                //---------------------Info---------------------//
                CmdTriggers = new string[] { "info" };
                if (!e.User.IsBot && Fcn.Msg.CommandCheck(e.Message.RawText, CmdTriggers))
                {
                    await e.Message.Delete();
                    await e.Channel.SendIsTyping();
                    Console.WriteLine("Command Found: 'Info'");
                    msg = Fcn.Msg.Decrypt(e.Message.RawText, CmdTriggers);
                    User u = e.Message.MentionedUsers.FirstOrDefault();
                    if (u != null)
                    {
                        Console.WriteLine("Event: " + u.Name + "'s Info was requested by " + e.User.Name);
                        MSG = await e.Channel.SendMessage(":speech_balloon:");
                        if (u.AvatarUrl != null)
                        {
                            FileInfo I = new FileInfo("../../meme/avatar.jpg");
                            using (WebClient webClient = new WebClient())
                            {
                                webClient.DownloadFileAsync(new Uri(u.AvatarUrl), "../../meme/avatar.jpg");
                            }

                            while (IsFileLocked(I))
                            {
                                System.Threading.Thread.Sleep(10);
                            }

                            await e.Channel.SendFile("../../meme/avatar.jpg");
                        }
                        await e.Channel.SendIsTyping();
                        await MSG.Delete();
                        await e.Channel.SendMessage(Fcn.Discord.GetUserInfo(u));
                        System.Timers.Timer MainTimer = new System.Timers.Timer(5000);
                        MainTimer.Start();
                        MainTimer.Elapsed += (Se, E) =>
                        {
                            MainTimer.Stop();
                            File.Delete("../../meme/avatar.jpg");
                            return;
                        };
                    }
                    else if (msg.Contains("xo") || msg.Contains("tictactoe"))
                    {
                        if (msg.Contains("#"))
                        {
                            int ID = Convert.ToInt32(msg.Substring(msg.IndexOf('#') + 1));
                            string[] Sts = File.ReadAllLines("../../XO_Data.txt");
                            if (Sts[ID].Contains("Incomplete"))
                            {
                                Fcn.Discord.TimedMsg(await e.Channel.SendMessage(":grey_exclamation: Game was not completed"), 10000);
                                return;
                            }
                            string P1 = Fcn.Discord.MentionByName(Fcn.Msg.Between(Sts[ID], "Player[1](", ')'));
                            string P2 = Fcn.Discord.MentionByName(Fcn.Msg.Between(Sts[ID], "Player[2](", ')'));
                            string ss = "Tic Tac Toe `Game ID: " + ID + "`\nFcn.Msg.Between " + P1 + " <:TTT_X:285804765819174912> & " + P2 + " <:TTT_O:285804759951212544>\n";
                            if (Sts[ID][18] == 'T')
                            {
                                ss += "Game ended in a tie with ";
                            }

                            if (Sts[ID][18] == 'X')
                            {
                                ss += P1 + " Won the game in `" + Sts[ID][36] + "` moves with ";
                            }

                            if (Sts[ID][18] == 'O')
                            {
                                ss += P2 + " Won the game in `" + Sts[ID][36] + "` moves with ";
                            }

                            if (Sts[ID][27] == 'X')
                            {
                                ss += P1 + " starting first\n\n";
                            }

                            if (Sts[ID][27] == 'O')
                            {
                                ss += P2 + " starting first\n\n";
                            }

                            if (Sts[ID].Contains("#"))
                            {
                                ss += Fcn.Msg.ConditionalString("<:TTT_X:285804765819174912>", Sts[ID][Sts[ID].IndexOf('#') + 7] == '1') + Fcn.Msg.ConditionalString("<:TTT_O:285804759951212544>", Sts[ID][Sts[ID].IndexOf('#') + 7] == '2') + Fcn.Msg.ConditionalString("<:TTT_Empty:285804754171592704>", Sts[ID][Sts[ID].IndexOf('#') + 7] == '0') + "<:TTT_B:285804751424192512>";
                                ss += Fcn.Msg.ConditionalString("<:TTT_X:285804765819174912>", Sts[ID][Sts[ID].IndexOf('#') + 8] == '1') + Fcn.Msg.ConditionalString("<:TTT_O:285804759951212544>", Sts[ID][Sts[ID].IndexOf('#') + 8] == '2') + Fcn.Msg.ConditionalString("<:TTT_Empty:285804754171592704>", Sts[ID][Sts[ID].IndexOf('#') + 8] == '0') + "<:TTT_B:285804751424192512>";
                                ss += Fcn.Msg.ConditionalString("<:TTT_X:285804765819174912>", Sts[ID][Sts[ID].IndexOf('#') + 9] == '1') + Fcn.Msg.ConditionalString("<:TTT_O:285804759951212544>", Sts[ID][Sts[ID].IndexOf('#') + 9] == '2') + Fcn.Msg.ConditionalString("<:TTT_Empty:285804754171592704>", Sts[ID][Sts[ID].IndexOf('#') + 9] == '0') + "\n";
                                ss += "<:TTT_B:285804751424192512><:TTT_B:285804751424192512><:TTT_B:285804751424192512><:TTT_B:285804751424192512><:TTT_B:285804751424192512>\n";
                                ss += Fcn.Msg.ConditionalString("<:TTT_X:285804765819174912>", Sts[ID][Sts[ID].IndexOf('#') + 4] == '1') + Fcn.Msg.ConditionalString("<:TTT_O:285804759951212544>", Sts[ID][Sts[ID].IndexOf('#') + 4] == '2') + Fcn.Msg.ConditionalString("<:TTT_Empty:285804754171592704>", Sts[ID][Sts[ID].IndexOf('#') + 4] == '0') + "<:TTT_B:285804751424192512>";
                                ss += Fcn.Msg.ConditionalString("<:TTT_X:285804765819174912>", Sts[ID][Sts[ID].IndexOf('#') + 5] == '1') + Fcn.Msg.ConditionalString("<:TTT_O:285804759951212544>", Sts[ID][Sts[ID].IndexOf('#') + 5] == '2') + Fcn.Msg.ConditionalString("<:TTT_Empty:285804754171592704>", Sts[ID][Sts[ID].IndexOf('#') + 5] == '0') + "<:TTT_B:285804751424192512>";
                                ss += Fcn.Msg.ConditionalString("<:TTT_X:285804765819174912>", Sts[ID][Sts[ID].IndexOf('#') + 6] == '1') + Fcn.Msg.ConditionalString("<:TTT_O:285804759951212544>", Sts[ID][Sts[ID].IndexOf('#') + 6] == '2') + Fcn.Msg.ConditionalString("<:TTT_Empty:285804754171592704>", Sts[ID][Sts[ID].IndexOf('#') + 6] == '0') + "\n";
                                ss += "<:TTT_B:285804751424192512><:TTT_B:285804751424192512><:TTT_B:285804751424192512><:TTT_B:285804751424192512><:TTT_B:285804751424192512>\n";
                                ss += Fcn.Msg.ConditionalString("<:TTT_X:285804765819174912>", Sts[ID][Sts[ID].IndexOf('#') + 1] == '1') + Fcn.Msg.ConditionalString("<:TTT_O:285804759951212544>", Sts[ID][Sts[ID].IndexOf('#') + 1] == '2') + Fcn.Msg.ConditionalString("<:TTT_Empty:285804754171592704>", Sts[ID][Sts[ID].IndexOf('#') + 1] == '0') + "<:TTT_B:285804751424192512>";
                                ss += Fcn.Msg.ConditionalString("<:TTT_X:285804765819174912>", Sts[ID][Sts[ID].IndexOf('#') + 2] == '1') + Fcn.Msg.ConditionalString("<:TTT_O:285804759951212544>", Sts[ID][Sts[ID].IndexOf('#') + 2] == '2') + Fcn.Msg.ConditionalString("<:TTT_Empty:285804754171592704>", Sts[ID][Sts[ID].IndexOf('#') + 2] == '0') + "<:TTT_B:285804751424192512>";
                                ss += Fcn.Msg.ConditionalString("<:TTT_X:285804765819174912>", Sts[ID][Sts[ID].IndexOf('#') + 3] == '1') + Fcn.Msg.ConditionalString("<:TTT_O:285804759951212544>", Sts[ID][Sts[ID].IndexOf('#') + 3] == '2') + Fcn.Msg.ConditionalString("<:TTT_Empty:285804754171592704>", Sts[ID][Sts[ID].IndexOf('#') + 3] == '0');
                            }
                            await e.Channel.SendMessage(ss);
                            return;
                        }
                        else
                        {
                            UpdateStats();
                            await e.Channel.SendMessage("Tic <:TTT_X:285804765819174912> Tac <:TTT_O:285804759951212544> Toe Games Stats:\n```java\n" + GetStats("XO") + "```");
                            return;
                        }
                    }
                    else if (msg.Contains("hang"))
                    {
                        if (msg.Contains("#"))
                        {
                            int ID = 0;
                            try
                            {
                                 ID = Convert.ToInt32(msg.Substring(msg.IndexOf('#') + 1));
                            }
                            catch(FormatException) { Fcn.Discord.TimedMsg(await e.Channel.SendMessage("Syntax Error `Match ID Error`"), 10000); PlayError(e.User);  return; }
                            string[] Sts = File.ReadAllLines("../../Hang_Data.txt");
                            if (Fcn.Msg.Between(Sts[ID], "$Win\"", "\" ") == "O")
                            {
                                Fcn.Discord.TimedMsg(await e.Channel.SendMessage("This Match is not yet Finished use `-continuehang " + ID + "` to view the match"), 10000);
                                return;
                            }
                            if (Fcn.Msg.Between(Sts[ID], "$Win\"", "\" ") == "I")
                            {
                                Fcn.Discord.TimedMsg(await e.Channel.SendMessage("This Match is Incomplete or Contains no usable Data"), 10000);
                                return;
                            }
                            Array.Resize(ref Program.Hang, Program.Hang.Length + 1);
                            Program.Hang[Program.Hang.Length - 1] = new Hangman(null, null, null);
                            if (!e.Channel.IsPrivate)
                            {
                                Program.Hang[Program.Hang.Length - 1].Continue(ID, e.Channel);
                            }
                            else
                            {
                                Program.Hang[Program.Hang.Length - 1].Continue(ID, Program.DefChan);
                            }
                            Program.Hang[Program.Hang.Length - 1].Finished = true;
                            Program.Hang[Program.Hang.Length - 1].Start(true);
                            return;
                        }
                        else
                        {
                            UpdateStats();
                            await e.Channel.SendMessage("<:Hangman:285779439604727809> Hangman Games Stats:\n```java\n" + GetStats("Program.Hang") + "```");
                            return;
                        }
                    }
                    else if (msg.Contains("connect4") || msg.Contains("connect 4"))
                    {
                        if (msg.Contains("#"))
                        {
                            int ID = Convert.ToInt32(msg.Substring(msg.IndexOf('#') + 1));
                            string[] Sts = File.ReadAllLines("../../Connect_Data.txt");
                            string ss = "Connect <:Connect_4:285813760277610496> `Game ID: " + ID + "`\nFcn.Msg.Between :red_circle:" + Fcn.Discord.MentionByName(Fcn.Msg.Between(Sts[ID], "P1{", '}')) + " & :large_blue_circle:" + Fcn.Discord.MentionByName(Fcn.Msg.Between(Sts[ID], "P2{", '}'));
                            if (Sts[ID][18] == 'X')
                            {
                                ss += "\nGame was not completed\n";
                            }
                            else if (Sts[ID][18] == 'T')
                            {
                                ss += "\nGame ended in a tie in `42` moves with " + Fcn.Discord.MentionByName(Fcn.Msg.Between(Sts[ID], "P" + (Convert.ToInt32(Sts[ID][27]) - 47) + "{", '}')) + "\n";
                            }
                            else
                            {
                                ss += "\n:tada: " + Fcn.Discord.MentionByName(Fcn.Msg.Between(Sts[ID], "P" + (Convert.ToInt32(Sts[ID][18]) - 47) + "{", '}')) + " Won the Game in `" + Sts[ID][36] + Sts[ID][37] + "` moves with " + Fcn.Discord.MentionByName(Fcn.Msg.Between(Sts[ID], "P" + (Convert.ToInt32(Sts[ID][27]) - 47) + "{", '}')) + "\n";
                            }

                            int t = Sts[ID].IndexOf('#');
                            for (int y = 5; y >= 0; y--)
                            {
                                for (int x = 0; x < 7; x++)
                                {
                                    t++;
                                    if (Sts[ID][t] == '0')
                                    {
                                        ss += ":white_circle:";
                                    }

                                    if (Sts[ID][t] == '1')
                                    {
                                        ss += ":red_circle:";
                                    }

                                    if (Sts[ID][t] == '2')
                                    {
                                        ss += ":large_blue_circle:";
                                    }

                                    if (Sts[ID][t] == '3')
                                    {
                                        ss += "<:Gold_Red_Circle:285466048084312065>";
                                    }

                                    if (Sts[ID][t] == '4')
                                    {
                                        ss += "<:Gold_Blue_Circle:285466046494670848>";
                                    }
                                }
                                ss += "\n";
                            }
                            await e.Channel.SendMessage(ss);
                            return;
                        }
                        else
                        {
                            return;
                        }
                    }
                    else
                    {
                        Fcn.Discord.TimedMsg(await e.Channel.SendMessage(":anger: Syntax error in Info Command: `No User or Game Specified`"), 30000);
                        Console.WriteLine("Event: User Info Error #No User or Game Specified");
                        return;
                    }
                }
                //---------------------Continue List---------------------//
                if (Fcn.Msg.CommandCheck(e.Message.RawText, "continuelist", true))
                {
                    await e.Channel.SendIsTyping();
                    Console.WriteLine("Command Found: 'Continue List'");
                    Program.ListUsersWaiting = false;
                    Console.WriteLine("Action: Listing Continued by " + e.User.Name);
                    await e.Message.Delete();
                    return;
                }
                //---------------------Stop List---------------------//
                if (Fcn.Msg.CommandCheck(e.Message.RawText, "stoplist", true))
                {
                    await e.Channel.SendIsTyping();
                    Console.WriteLine("Command Found: 'Stop List'");
                    Program.ListUsersStopped = true;
                    await e.Channel.SendMessage("Listing Stopped  :no_entry: ");
                    Console.WriteLine("Action: Listing Stopped by " + e.User.Name);
                    await e.Message.Delete();
                    return;
                }
                //---------------------AutoMove---------------------//
                CmdTriggers = new string[] { "automove", "autmove" };
                if (!e.User.IsBot && Fcn.Msg.CommandCheck(e.Message.RawText, CmdTriggers))
                {
                    await e.Channel.SendIsTyping();
                    Console.WriteLine("Command Found: 'Automove'");
                    Console.WriteLine("Action: Moving all connected users to according channels");
                    await e.Message.Delete();
                    MSG = await e.Channel.SendMessage(":arrow_right: Moving all connected users to according channels");
                    output = ":arrow_right: Moving all connected users to according channels";
                    if (e.User.ServerPermissions.MoveMembers)
                    {
                        foreach (User U in e.Server.Users)
                        {
                            if (U.VoiceChannel != null && Program.AllUsers[Fcn.Discord.GetA_UsersIndex(U)].AutoMove)
                            {
                                if(!Program.AllUsers[Fcn.Discord.GetA_UsersIndex(U)].AFKMove &&U.VoiceChannel.Id == 155958790124994560)
                                { }
                                else if (U.CurrentGame != null && Fcn.Discord.GetChannel(U.CurrentGame.Value.Name) != null)
                                {
                                    await U.Edit(null, null, Fcn.Discord.GetChannel(U.CurrentGame.Value.Name));
                                    output += "\n" + U.Mention + " to " + Fcn.Discord.GetChannel(U.CurrentGame.Value.Name).Name;
                                    await MSG.Edit(output);
                                }
                                else if (U.VoiceChannel.Id != 155958790124994560)
                                {
                                    await U.Edit(null, null, Fcn.Discord.GetChannel("main"));
                                    output += "\n" + U.Mention + " to " + Fcn.Discord.GetChannel("main").Name;
                                    await MSG.Edit(output);
                                }
                            }
                        }
                        Fcn.Discord.TimedMsg(MSG, 30000);
                    }
                    else
                    {
                        Fcn.Discord.TimedMsg(await e.Channel.SendMessage(":anger: You do not have permissions to move members in the Server"), 30000);
                        Console.WriteLine("Event: User without move permissions executing Move command - Error");
                    }
                    return;
                }
                //---------------------Move---------------------//
                CmdTriggers = new string[] { "move", "go" };
                if (Fcn.Msg.CommandCheck(e.Message.RawText, CmdTriggers))
                {
                    Channel ToC, FromC;
                    await e.Channel.SendIsTyping();
                    Console.WriteLine("Command Found: 'Move'");
                    if (e.User.ServerPermissions.MoveMembers)
                    {
                        if (e.Message.RawText.Contains("to "))
                        {
                            await e.Message.Delete();
                            msg = Fcn.Msg.Decrypt(e.Message.RawText, CmdTriggers);
                            IEnumerable<User> U = e.Server.Users.Except(e.Server.AFKChannel.Users);
                            ToC = Fcn.Discord.GetChannel(msg.Substring(msg.IndexOf("to ") + 3));
                            if (ToC == null)
                            {
                                Fcn.Discord.TimedMsg(await e.Channel.SendMessage(":anger: Syntax Error in Move Command: `missing 'to' channel`"));
                                Console.WriteLine("Event: 'to' channel not specified in Move Command - Error");
                                return;
                            }
                            FromC = Fcn.Discord.GetChannel(msg.Substring(0, Math.Max(0, msg.IndexOf("to ") - 1)));
                            if (FromC != null)
                            {
                                U = FromC.Users;
                                Fcn.Discord.TimedMsg(await e.Channel.SendMessage(":arrow_right: Moved users from " + FromC.Name + " to " + ToC.Name), 30000);
                            }
                            else
                            {
                                Fcn.Discord.TimedMsg(MSG = await e.Channel.SendMessage(":arrow_right: Moved connected users to " + ToC.Name), 30000);
                            }

                            Console.WriteLine("Action: Moved specified users to " + ToC.Name);
                            foreach (User u in U)
                            {
                                if (u.VoiceChannel != null)
                                {
                                    if (!Program.AllUsers[Fcn.Discord.GetA_UsersIndex(u)].AFKMove && u.VoiceChannel.Id == 155958790124994560)
                                    { }
                                    else
                                        await u.Edit(null, null, ToC);
                                }
                            }
                        }
                        else
                        {
                            Fcn.Discord.TimedMsg(await e.Channel.SendMessage(":anger: Syntax Error in Move Command: `missing 'to'`"), 30000);
                            Console.WriteLine("Event: 'to' not specified in Move Command - Error");
                        }
                    }
                    else
                    {
                        Fcn.Discord.TimedMsg(await e.Channel.SendMessage(":anger: You do not have permissions to move members in the Server"), 30000);
                        Console.WriteLine("Event: User without move permissions executing Move command - Error");
                    }
                    return;
                }
                //---------------------Exile/Calm---------------------//
                CmdTriggers = new string[] { "calm", "restrain", "exile" };
                if (!e.User.IsBot && Fcn.Msg.CommandCheck(e.Message.RawText, CmdTriggers))
                {
                    await e.Channel.SendIsTyping();
                    await e.Message.Delete();
                    Console.WriteLine("Command Found: 'Exile'");
                    int Time = 0, min = 0, sec = 30;
                    msg = Fcn.Msg.Decrypt(e.Message.RawText, CmdTriggers);
                    if (msg.Contains("#"))
                    {
                        if (msg.Contains(":"))
                        {
                            if (msg.IndexOf('#') - msg.IndexOf(':') < -1)
                            {
                                min = Convert.ToInt32(Fcn.Msg.Between(msg, '#', ':'));
                            }

                            sec = Convert.ToInt32(msg.Substring(msg.IndexOf(':') + 1, Math.Min(msg.Length - 1, msg.IndexOf(':') + 2) - msg.IndexOf(':')));
                        }
                        else
                        {
                            min = Convert.ToInt32(msg.Substring(1 + msg.IndexOf('#'), Math.Min(msg.Length - 1, msg.IndexOf('#') + 2) - msg.IndexOf('#')));
                            sec = 0;
                        }
                        Time = min * 60000 + sec * 1000;
                    }
                    if (Time <= 0)
                    {
                        Time = 30000;
                    }

                    User u = e.Message.MentionedUsers.FirstOrDefault();
                    if (u != null)
                    {
                        if (u == e.User)
                        {
                            Fcn.Discord.TimedMsg(await e.Channel.SendMessage(":grey_exclamation: You can't exile yourself"), 10000);
                            Console.WriteLine("Event: User can not Exile himself - Error");
                        }
                        else if (u.HasRole(e.Server.GetRole(279279809237090304)))
                        {
                            foreach (Exile E in Program.Exiles)
                            {
                                if (E.U == u && E.Duration.Total > 0)
                                {
                                    E.Duration.AddMilliseconds(Time);
                                    E.ETimer.Stop();
                                    E.ETimer.Interval = E.Duration.Total;
                                    E.ETimer.Start();
                                    Time D = new Time(Time);
                                    Console.WriteLine($"Action: Added {D.Get()} seconds to {e.User.Name}'s Exile");
                                    Fcn.Discord.TimedMsg(await e.Channel.SendMessage($"{D.Get()} has been added to {u.Mention}'s Exile, a total of {E.Duration.Get()}"), Convert.ToInt32(E.Duration.Total));
                                    return;
                                }
                            }
                        }
                        else if (u.Id == 272383726300692500)
                        {
                            Fcn.Discord.TimedMsg(await e.Channel.SendMessage(":grey_exclamation: I can't Exile myself"), 10000);
                            Console.WriteLine("Event: User specified in Exile Command is beyond Bot's reach - Error");
                        }
                        else if (u.HasRole(e.Server.GetRole(218442977121402891)) || u.HasRole(e.Server.GetRole(276033405089480714)))
                        {
                            Fcn.Discord.TimedMsg(await e.Channel.SendMessage(":grey_exclamation: This Exile is beyond my reach :cry:"), 10000);
                            Console.WriteLine("Event: User specified in Exile Command is beyond Bot's reach - Error");
                        }
                        else
                        {
                            await e.Message.Delete();
                            IEnumerable<Discord.Role> Rs = new[] { e.Server.GetRole(279279809237090304) };
                            Array.Resize(ref Program.Exiles, Program.Exiles.Length + 1);
                            int arraysize = Program.Exiles.Length - 1;
                            Program.Exiles[arraysize] = new Exile(Time)
                            {
                                U = u,
                                Roles = u.Roles,
                                Chan = u.VoiceChannel
                            };
                            Console.Write("Action: Exiling " + u.Name + " - ");
                            await u.Edit(true, true, e.Server.AFKChannel, Rs);
                            Time D = new Time(Time);
                            MSG = await e.Server.DefaultChannel.SendMessage(":x: " + u.Mention + " was exiled for " + D.Get());

                            System.Timers.Timer MainTimer = new System.Timers.Timer();
                            Program.Exiles[arraysize].ETimer.Interval = Time;
                            Program.Exiles[arraysize].ETimer.Start();
                            Program.Exiles[arraysize].ETimer.Elapsed += async (Se, E) =>
                            {
                                Program.Exiles[arraysize].ETimer.Stop();
                                await MSG.Delete();
                                Console.WriteLine("Action: Restoring " + u.Name + "'s Roles back");
                                await u.Edit(false, false, Program.Exiles[arraysize].Chan, Program.Exiles[arraysize].Roles);
                                Fcn.Discord.TimedMsg(await e.Server.DefaultChannel.SendMessage(":o: " + u.Mention + " is no longer exiled"));
                            };
                        }
                    }
                    else
                    {
                        Fcn.Discord.TimedMsg(await e.Channel.SendMessage(":anger: Syntax Error in Exile Command: `user not specified`"), 10000);
                        Console.WriteLine("Event: User not specified in Calm Command - Error");
                    }
                    return;
                }
                //---------------------Unexile---------------------//
                CmdTriggers = new string[] { "unrestrain me", "unexile me" };
                if (!e.User.IsBot && Fcn.Msg.CommandCheck(e.Message.RawText, CmdTriggers))
                {
                    await e.Channel.SendIsTyping();
                    Console.WriteLine("Command Found: 'UnExile'");
                    if (e.User.HasRole(discord.Servers.FirstOrDefault().GetRole(279279809237090304)))
                    {
                        foreach (Exile E in Program.Exiles)
                        {
                            if (E.U == e.User && E.Duration.Total > 0)
                            {
                                E.Duration = new Time(E.Duration.Total / 4);
                                E.ETimer.Stop();
                                E.ETimer.Interval = E.Duration.Total;
                                E.ETimer.Start();
                                Console.WriteLine($"Action: Unexiling {e.User.Name} in {E.Duration.Get()}");
                                Fcn.Discord.TimedMsg(await e.Channel.SendMessage($"You will be unexiled in {E.Duration.Get()}"), Convert.ToInt32(E.Duration.Total));
                            }
                        }
                    }
                    else
                    {
                        Fcn.Discord.TimedMsg(await e.Channel.SendMessage(":anger: You are not Exiled"));
                        Console.WriteLine("Event: Unexile attempt while not being Exiled - Error");
                    }
                    return;
                }
                //---------------------Vote Kick---------------------//
                CmdTriggers = new string[] { "kick", "votekick" };
                if (!e.User.IsBot && Fcn.Msg.CommandCheck(e.Message.RawText, CmdTriggers) && !e.User.IsBot)
                {
                    await e.Channel.SendIsTyping();
                    bool exist = false;
                    Console.WriteLine("Command Found: 'Vote Kick'");
                    msg = Fcn.Msg.Decrypt(e.Message.RawText, CmdTriggers);
                    if (Program.KickVoting)
                    {
                        msg = Fcn.Msg.Decrypt(e.Message.RawText, CmdTriggers);
                        User u = e.Message.MentionedUsers.FirstOrDefault();
                        if (u == null)
                        {
                            Fcn.Discord.TimedMsg(await e.Channel.SendMessage(":anger: No user specified :grey_exclamation: "), 10000);
                            Console.WriteLine("Event: Kick Voting initiated without User parameter by " + e.User.Name);
                        }
                        else
                        {
                            foreach (Kick K in Program.VoteKick)
                            {
                                if (!K.Canceled)
                                {
                                    if (u == K.User)
                                    {
                                        exist = true;
                                        if (K.HasVoted(e.User.Id.ToString()))
                                        {
                                            Fcn.Discord.TimedMsg(await e.Channel.SendMessage(":x: You already voted, " + e.User.Mention + "  -  Error"), 30000);
                                            Console.WriteLine("Event: Kick Voting initiated by " + e.User.Name + " while having already voted  -  Error");
                                        }
                                        else if (K.User == e.User)
                                        {
                                            Fcn.Discord.TimedMsg(await e.Channel.SendMessage(":x: You can not vote to kick yourself, " + e.User.Mention), 10000);
                                            Console.WriteLine("Event: Kick Voting initiated by " + e.User.Name + " against himself  -  Error");
                                        }
                                        else
                                        {
                                            K.Vote(e.User, e.Server.DefaultChannel);
                                            if (K.Votes > 3)
                                            {
                                                await e.Server.DefaultChannel.SendMessage(":name_badge: Kicking Vote Passed, " + K.User.Mention + " will be kicked shortly :name_badge: ");
                                                System.Timers.Timer MainTimer = new System.Timers.Timer(5000);
                                                MainTimer.Start();
                                                MainTimer.Elapsed += async (Se, E) =>
                                                {
                                                    MainTimer.Stop();
                                                    if (Program.KickVoting)
                                                    {
                                                        await K.User.Kick();
                                                        Console.WriteLine("Event: " + K.User.Name + " was kicked from the Server");
                                                    }
                                                    else
                                                    {
                                                        Console.WriteLine("Event: " + K.User.Name + " was NOT kicked from the Server due to kicking being disabled");
                                                    }
                                                };
                                            }
                                            else
                                            {
                                                await e.Server.DefaultChannel.SendMessage(K.Votes + "/4 have voted to kick " + K.User.Mention + " :grey_exclamation: ");
                                            }
                                        }
                                    }
                                }
                            }
                            if (!exist)
                            {
                                Array.Resize(ref Program.VoteKick, Program.VoteKick.Length + 1);
                                Console.WriteLine("Event: Kick Voting initiated for the firest time on " + u.Name + " by " + e.User.Name);
                                Program.VoteKick[Program.VoteKick.Length - 1] = new Kick();
                                Program.VoteKick.Last().User = u;
                                Program.VoteKick.Last().Vote(e.User, e.Server.DefaultChannel);
                            }
                        }
                    }
                    else
                    {
                        Fcn.Discord.TimedMsg(await e.Server.DefaultChannel.SendMessage(":no_entry_sign: Kicking is disabled at the moment :no_entry_sign: "), 20000);
                        Console.WriteLine("Event: Kick Voting initiated by " + e.User.Name + " while Kicking is disabled  -  Error");
                    }
                    return;
                }
                //---------------------Stop Kick---------------------//
                CmdTriggers = new string[] { "stopkick", "cancelkick", "cancelvote", "stopvote" };
                if (!e.User.IsBot && Fcn.Msg.CommandCheck(e.Message.RawText, CmdTriggers) && e.User.Id == 155955179852791809)
                {
                    await e.Channel.SendIsTyping();
                    Console.WriteLine("Command Found: 'Stop Kick'");
                    Console.WriteLine("Action: Stopped Kicking Command");
                    Fcn.Discord.TimedMsg(await e.Server.DefaultChannel.SendMessage("Kick Voting has been stopped  :no_entry: "), 30000);
                    Program.KickVoting = false;
                    foreach (Kick K in Program.VoteKick)
                    {
                        K.Canceled = true;
                    }

                    await e.Message.Delete();
                }
                //---------------------Continue Kick---------------------//
                CmdTriggers = new string[] { "continuekick", "startkick", "continuevote", "startvote" };
                if (!e.User.IsBot && Fcn.Msg.CommandCheck(e.Message.RawText, CmdTriggers) && e.User.Id == 155955179852791809)
                {
                    await e.Channel.SendIsTyping();
                    Console.WriteLine("Command Found: 'Continue Kick'");
                    Console.WriteLine("Action: Continued Kicking Command");
                    Fcn.Discord.TimedMsg(await e.Server.DefaultChannel.SendMessage("Kick Voting has been continued  :white_check_mark: "), 30000);
                    Program.KickVoting = true;
                    await e.Message.Delete();
                }
                //---------------------Vote Ban---------------------//
                CmdTriggers = new string[] { "ban", "voteban" };
                if (!e.User.IsBot && Fcn.Msg.CommandCheck(e.Message.RawText, CmdTriggers) && !e.User.IsBot)
                {
                    await e.Channel.SendIsTyping();
                    bool exist = false;
                    Console.WriteLine("Command Found: 'Vote Ban'");
                    msg = Fcn.Msg.Decrypt(e.Message.RawText, CmdTriggers);
                    if (Program.BanVoting)
                    {
                        msg = Fcn.Msg.Decrypt(e.Message.RawText, CmdTriggers);
                        User u = e.Message.MentionedUsers.FirstOrDefault();
                        if (u == null)
                        {
                            Fcn.Discord.TimedMsg(await e.Channel.SendMessage(":anger: No user specified :exclamation: "), 10000);
                            Console.WriteLine("Event: Ban Voting initiated without User parameter by " + e.User.Name);
                        }
                        else
                        {
                            foreach (Ban K in Program.VoteBan)
                            {
                                if (!K.Canceled)
                                {
                                    if (u == K.User)
                                    {
                                        exist = true;
                                        if (K.HasVoted(e.User.Id.ToString()))
                                        {
                                            Fcn.Discord.TimedMsg(await e.Channel.SendMessage(":x: You already voted, " + e.User.Mention + "  -  Error"), 30000);
                                            Console.WriteLine("Event: Ban Voting initiated by " + e.User.Name + " while having already voted  -  Error");
                                        }
                                        else if (K.User == e.User)
                                        {
                                            Fcn.Discord.TimedMsg(await e.Channel.SendMessage(":x: You can not vote to Ban yourself, " + e.User.Mention), 10000);
                                            Console.WriteLine("Event: Ban Voting initiated by " + e.User.Name + " against himself  -  Error");
                                        }
                                        else
                                        {
                                            K.Vote(e.User, e.Server.DefaultChannel);
                                            if (K.Votes > 3)
                                            {
                                                await e.Server.DefaultChannel.SendMessage(":name_badge: Banning Vote Passed, " + K.User.Mention + " will be banned shortly :name_badge: ");
                                                System.Timers.Timer MainTimer = new System.Timers.Timer(5000);
                                                MainTimer.Start();
                                                MainTimer.Elapsed += async (Se, E) =>
                                                {
                                                    MainTimer.Stop();
                                                    if (Program.BanVoting)
                                                    {
                                                        await e.Server.Ban(K.User);
                                                        Console.WriteLine("Event: " + K.User.Name + " was banned from the Server");
                                                    }
                                                    else
                                                    {
                                                        Console.WriteLine("Event: " + K.User.Name + " was NOT banned from the Server due to Baning being disabled");
                                                    }
                                                };
                                            }
                                            else
                                            {
                                                await e.Server.DefaultChannel.SendMessage(K.Votes + "/6 have voted to Ban " + K.User.Mention + " :exclamation: ");
                                            }
                                        }
                                    }
                                }
                            }
                            if (!exist)
                            {
                                Array.Resize(ref Program.VoteBan, Program.VoteBan.Length + 1);
                                Console.WriteLine("Event: Ban Voting initiated for the firest time on " + u.Name + " by " + e.User.Name);
                                Program.VoteBan[Program.VoteBan.Length - 1] = new Ban();
                                Program.VoteBan.Last().User = u;
                                Program.VoteBan.Last().Vote(e.User, e.Server.DefaultChannel);
                                await e.Server.DefaultChannel.SendMessage(Fcn.Msg.ConditionalString("1", !e.User.HasRole(e.Server.GetRole(217294542020411392))) + Fcn.Msg.ConditionalString("2", e.User.HasRole(e.Server.GetRole(217294542020411392))) + "/6 have voted to Ban " + u.Mention + " :exclamation: ");
                            }
                        }
                    }
                    else
                    {
                        Fcn.Discord.TimedMsg(await e.Server.DefaultChannel.SendMessage(":no_entry_sign: Baning is disabled at the moment :no_entry_sign: "), 20000);
                        Console.WriteLine("Event: Ban Voting initiated by " + e.User.Name + " while Baning is disabled  -  Error");
                    }
                    return;
                }
                //---------------------Stop Ban---------------------//
                CmdTriggers = new string[] { "stopban", "cancelban", "cancelvote", "stopvote" };
                if (!e.User.IsBot && Fcn.Msg.CommandCheck(e.Message.RawText, CmdTriggers) && e.User.Id == 155955179852791809)
                {
                    await e.Channel.SendIsTyping();
                    Console.WriteLine("Command Found: 'Stop Ban'");
                    Console.WriteLine("Action: Stopped Baning Command");
                    Fcn.Discord.TimedMsg(await e.Server.DefaultChannel.SendMessage("Ban Voting has been stopped  :no_entry: "), 30000);
                    Program.BanVoting = false;
                    foreach (Ban K in Program.VoteBan)
                    {
                        K.Canceled = true;
                    }

                    await e.Message.Delete();
                }
                //---------------------Continue Ban---------------------//
                CmdTriggers = new string[] { "continueban", "startban", "continuevote", "startvote" };
                if (!e.User.IsBot && Fcn.Msg.CommandCheck(e.Message.RawText, CmdTriggers) && e.User.Id == 155955179852791809)
                {
                    await e.Channel.SendIsTyping();
                    Console.WriteLine("Command Found: 'Continue Ban'");
                    Console.WriteLine("Action: Continued Baning Command");
                    Fcn.Discord.TimedMsg(await e.Server.DefaultChannel.SendMessage("Ban Voting has been continued  :white_check_mark: "), 30000);
                    Program.BanVoting = true;
                    await e.Message.Delete();
                }
                //---------------------Spam---------------------//
                CmdTriggers = new string[] { "spam", "annoy", "anoy" };
                if (!e.User.IsBot && Fcn.Msg.CommandCheck(e.Message.RawText, CmdTriggers))
                {
                    await e.Channel.SendIsTyping();
                    Console.WriteLine("Command Found: 'Spam'");
                    msg = Fcn.Msg.Decrypt(e.Message.RawText, CmdTriggers);
                    User u = e.Server.FindUsers(msg).FirstOrDefault();
                    Console.WriteLine("Action: Spamming " + u.Name + " as Requested by " + e.User.Name);
                    await e.Channel.SendMessage("Time to have some fun! " + u.Mention + " :smiling_imp: ");
                    for (int j = 0; j < Program.rand.Next(5, 30); j++)
                    {
                        Fcn.Discord.Annoy(u);
                        System.Threading.Thread.Sleep(Program.rand.Next(100, 1000));
                    }
                    return;
                }
                //---------------------Clean Up---------------------//
                CmdTriggers = new string[] { "clean", "clear" };
                if (!e.User.IsBot && Fcn.Msg.CommandCheck(msg, CmdTriggers))
                {
                    int i; DateTime DT = new DateTime(0);
                    msg = e.Message.RawText;
                    await e.Channel.SendIsTyping();
                    await e.Message.Delete();
                    Console.WriteLine("Command Found: 'Clean Up'");
                    msg = Fcn.Msg.Decrypt(msg, CmdTriggers); msg = Fcn.Msg.Remove(msg, "your");
                    Fcn.Msg.RmvSpaces(ref msg);
                    Discord.Message[] messagesToDelete;
                    if(msg.Contains("#"))
                    {
                        try { i = Convert.ToInt32(msg.Substring(0, msg.IndexOf('#'))); }
                        catch(FormatException) { i = 15; }
                        try { DT = DateTime.Parse(msg.Substring(msg.IndexOf("#") + 1, msg.Length - msg.IndexOf("#") - 1)); }
                        catch(FormatException)
                        {
                            if (msg.Contains("today"))
                                DT = DateTime.Parse(DateTime.Now.Day.ToString() + "/" + DateTime.Now.Month.ToString() + "/" + DateTime.Now.Year.ToString());
                        }
                    }
                    else
                    {
                        if (msg == "")
                        {
                            i = 10;
                        }
                        else
                        {
                            i = Convert.ToInt32(msg);
                        }
                    }

                    i = Math.Min(99, i);
                    i = Math.Max(1, i);
                    i++;
                    List<Discord.Message> msgstodel2;
                    List<Discord.Message> msgstodel = msgstodel2 = (await e.Channel.DownloadMessages(i)).ToList();
                    foreach(Discord.Message D in msgstodel.ToArray())
                    {
                        if ((D.User.Id != 272383726300692500 && e.Message.RawText.Contains("your")) || DateTime.Compare(D.Timestamp, DT) < 0 )
                        {
                            msgstodel2.Remove(D);
                            i--;
                        }
                    }
                    msgstodel = msgstodel2;
                    foreach (Discord.Message mess in msgstodel)
                    {
                        foreach (TicTacToe T in Program.TTT)
                        {
                            if (!T.Finished)
                            {
                                if (mess.Id == T.Board.Id || mess.Id == T.Mess.Id)
                                {
                                    msgstodel2.Remove(mess);
                                    i--;
                                }
                            }
                        }
                        foreach (Hangman T in Program.Hang)
                        {
                            if (!T.Finished)
                            {
                                if (mess.Id == T.Mess.Id || mess.Id == T.Mess.Id)
                                {
                                    msgstodel2.Remove(mess);
                                    i--;
                                }
                            }
                        }
                    }
                    messagesToDelete = msgstodel2.ToArray();
                    try
                    {
                        Console.WriteLine("Action: " + e.User.Name + " is Clearing " + (i - 1) + " messages");
                        await e.Channel.DeleteMessages(messagesToDelete);
                        Fcn.Discord.TimedMsg(await e.Channel.SendMessage(":put_litter_in_its_place: " + Fcn.Msg.ToEmoji(msgstodel2.Count)));
                    }
                    catch (Discord.Net.HttpException)
                    {
                        Console.WriteLine("Event: Failed to clear messages correctly");
                        Fcn.Discord.TimedMsg(await e.Channel.SendMessage(":disappointed_relieved: Failed to clear messages correctly, it might take longer to delete them"));
                        System.Timers.Timer T = new System.Timers.Timer(500);
                        int index = 0;
                        T.Start();
                        T.Elapsed += async (SS, EE) =>
                        {
                            try
                            {
                                await msgstodel[index].Delete();
                            }
                            catch (ArgumentOutOfRangeException) { T.Dispose(); }
                            index++;
                        };
                    }
                    return;
                }
                //---------------------Program.Hang Answer---------------------//
                if (e.Message.RawText.Length == 1 && char.IsLetter(e.Message.RawText[0]))
                {
                    foreach (Hangman H in Program.Hang)
                    {
                        if (!H.Finished)
                        {
                            await e.Message.Delete();
                            Console.WriteLine($"Event: 'Program.Hang Answer' > '{e.Message.RawText}' by {e.User.Name}");
                            if (H.Starter != null && e.User.Id == H.Starter.Id)
                            {
                                if (H.Help >= 2)
                                {
                                    return;
                                }

                                H.Help++;
                                Fcn.Discord.TimedMsg(await e.Channel.SendMessage(e.User.Mention + " suggests using `" + e.Message.RawText + "`"), 10000);
                                return;
                            }
                            if(H.Play(e.Message.RawText[0], e.User))
                            {
                                Program.AllUsers[Fcn.Discord.GetA_UsersIndex(e.User)].HangSolved++;
                                Program.AllUsers[Fcn.Discord.GetA_UsersIndex(e.User)].Update();
                            }
                                
                            return;
                        }
                    }
                }
                //---------------------Help Hangman---------------------//
                CmdTriggers = new string[] { "hanghelp", "hang help" };
                if (!e.User.IsBot && Fcn.Msg.CommandCheck(e.Message.RawText, CmdTriggers))
                {
                    string alpha = "qwertyuiopasdfghjklzxcvbnm";
                    foreach (Hangman H in Program.Hang)
                    {
                        if (!H.Finished)
                        {
                            Console.WriteLine("Command Found: 'Help Hangman'");
                            int y = 0;
                            foreach (char c in H.Letters)
                            {
                                if (H.Word.Contains(c))
                                {
                                    y++;
                                }
                            }

                            if (H.Help != 2 && (H.TotalLetters - y > 2))
                            {
                                while (true)
                                {
                                    int p = new Random().Next(alpha.Length);
                                    if (!H.Letters.Contains(alpha[p]) && H.Word.Contains(alpha[p]))
                                    {
                                        await e.Message.Delete();
                                        H.Help++;
                                        Fcn.Discord.TimedMsg(await e.Channel.SendMessage($"Try using the letter `{alpha[p]}`"), 10000);
                                        return;
                                    }
                                }
                            }
                            else
                            {
                                await e.Message.Delete();
                                if(H.Help == 2)
                                    Fcn.Discord.TimedMsg(await e.Channel.SendMessage(":grey_exclamation: All helps were used for this Hangman game"));
                                else
                                    Fcn.Discord.TimedMsg(await e.Channel.SendMessage(":grey_exclamation: There are too few remaining letters to help you"));
                                return;
                            }
                        }
                    }
                    return;
                }
                //---------------------Continue Hangman---------------------//
                CmdTriggers = new string[] { "continuehangman", "continuehang", "continue hangman", "continue hang" };
                if (!e.User.IsBot && Fcn.Msg.CommandCheck(e.Message.RawText, CmdTriggers))
                {
                    await e.Channel.SendIsTyping();
                    await e.Message.Delete();
                    Console.WriteLine("Command Found: 'Continue Hangman'");
                    if (!e.Channel.IsPrivate)
                    {
                        string[] Sts = File.ReadAllLines("../../Hang_Data.txt");
                        msg = Fcn.Msg.Decrypt(e.Message.RawText, CmdTriggers).Trim('#');
                        int ID = 0;
                        try
                        {
                            ID = Convert.ToInt32(msg);
                        }
                        catch (FormatException)
                        {
                            Fcn.Discord.TimedMsg(await e.Channel.SendMessage(":anger: Error in Continue Program.Hang Command: `Match ID missing`"));
                            return;
                        }
                        if (Fcn.Msg.Between(Sts[ID], "$Win\"", "\" ") == "Y" || Fcn.Msg.Between(Sts[ID], "$Win\"", "\" ") == "N")
                        {
                            Fcn.Discord.TimedMsg(await e.Channel.SendMessage("This Match is already Finished use `-info hang #" + ID + "` to view the match"), 10000);
                            return;
                        }
                        if (Fcn.Msg.Between(Sts[ID], "$Win\"", "\" ") == "I")
                        {
                            Fcn.Discord.TimedMsg(await e.Channel.SendMessage("This Match is not compatible with this technique `Too Old` or `Incomplete`"), 10000);
                            return;
                        }
                        Array.Resize(ref Program.Hang, Program.Hang.Length + 1);
                        Program.Hang[Program.Hang.Length - 1] = new Hangman(null, null, null);
                            Program.Hang[Program.Hang.Length - 1].Continue(ID, e.Channel);
                        Program.Hang[Program.Hang.Length - 1].Start(true);
                    }
                    else
                    {
                        Fcn.Discord.TimedMsg(await e.Channel.SendMessage("You can not continue a match in a private channel"), 10000);
                    }
                    return;
                }
                //---------------------Hangman---------------------//
                CmdTriggers = new string[] { "hangman", "hang" };
                if (!e.User.IsBot && Fcn.Msg.CommandCheck(e.Message.RawText, CmdTriggers))
                {
                    await e.Channel.SendIsTyping();
                    await e.Message.Delete();
                    Console.WriteLine("Command Found: 'Hangman'"); msg = Fcn.Msg.Decrypt(e.Message.RawText, CmdTriggers);
                    foreach (Hangman H in Program.Hang)
                    {
                        if (!H.Finished)
                        {
                            if (msg == "me")
                            {
                                H.End();
                                return;
                            }
                            Fcn.Discord.TimedMsg(await e.Channel.SendMessage(":grey_exclamation: Another Hangman Game is still ongoing"));
                            return;
                        }
                    }
                    if (e.Message.RawText.Contains('#'))
                    {
                        try { msg = msg.Substring(0, msg.IndexOf('#') - 1); }
                        catch (ArgumentOutOfRangeException) { Fcn.Discord.TimedMsg(await e.Channel.SendMessage("Invalid Syntax for the `Description`"), 10000); return; }
                    }
                    if (msg.Contains("Description:")) { Fcn.Discord.TimedMsg(await e.Channel.SendMessage("Invalid Word `Word can not contain the sequence: 'Description:'`"), 10000); return; }
                    if (msg[0] == ' ')
                    {
                        msg = msg.Substring(1);
                    }

                    if (msg[msg.Length - 1] == ' ')
                    {
                        msg = msg.Substring(0, msg.Length - 2);
                    }

                    if (msg.Length < 4)
                    {
                        Fcn.Discord.TimedMsg(await e.Channel.SendMessage("Word must contain at least 3 letters`"));
                        return;
                    }
                    Array.Resize(ref Program.Hang, Program.Hang.Length + 1);
                    Program.Hang[Program.Hang.Length - 1] = new Hangman(e.User, Program.DefChan, msg);
                    if (e.Message.RawText.Contains('#'))
                    {
                        Program.Hang[Program.Hang.Length - 1].Description = e.Message.RawText.Substring(e.Message.RawText.IndexOf('#') + 1);
                    }
                    if (!e.Channel.IsPrivate)
                    {
                        Program.Hang[Program.Hang.Length - 1].Chan = e.Channel;
                    }

                    Fcn.Discord.TimedMsg(await e.Channel.SendMessage("Word `" + msg + "` registered, Hangman starting.."));
                    Program.Hang[Program.Hang.Length - 1].Start();
                    Program.AllUsers[Fcn.Discord.GetA_UsersIndex(e.User)].HangStarted++;
                    Program.AllUsers[Fcn.Discord.GetA_UsersIndex(e.User)].Update();
                    return;
                }
                //---------------------Tic Tac Toe---------------------//
                CmdTriggers = new string[] { "tictactoe", "xo" };
                if (!e.User.IsBot && Fcn.Msg.CommandCheck(e.Message.RawText, CmdTriggers))
                {
                    await e.Message.Delete();
                    await e.Channel.SendIsTyping();
                    Console.WriteLine("Command Found: 'TicTacToe'"); msg = e.Message.RawText;
                    msg = Fcn.Msg.Decrypt(e.Message.RawText, CmdTriggers);
                    User P2;
                    if (e.Channel.IsPrivate)
                    {
                        P2 = e.Channel.Users.FirstOrDefault();
                    }
                    else
                    {
                        P2 = e.Message.MentionedUsers.FirstOrDefault();
                    }

                    if (P2 == null || P2 == e.User)
                    {
                        Fcn.Discord.TimedMsg(await e.Channel.SendMessage("Syntax error in TicTacToe command: `Invalid User specified`"));
                        return;
                    }
                    foreach (TicTacToe T in Program.TTT)
                    {
                        if (!T.Finished)
                        {
                            if (e.User == T.Player[0] || e.User == T.Player[1])
                            {
                                Fcn.Discord.TimedMsg(await e.Channel.SendMessage(e.User.Mention + " is already in a Tic Tac Toe game"));
                                return;
                            }
                            else if (P2 == T.Player[0] || P2 == T.Player[1])
                            {
                                Fcn.Discord.TimedMsg(await e.Channel.SendMessage(P2.Mention + " is already in a Tic Tac Toe game"));
                                return;
                            }
                        }
                    }
                    Array.Resize(ref Program.TTT, Program.TTT.Length + 1);
                    Program.TTT[Program.TTT.Length - 1] = new TicTacToe();
                    Program.TTT[Program.TTT.Length - 1].Initialize(e.User, P2, e.Channel);
                    Program.TTT[Program.TTT.Length - 1].Start();
                    return;
                }
                //---------------------X / O---------------------//
                if (e.Message.RawText.Length == 3 && (e.Message.RawText[1] == '#' || e.Message.RawText[1] == '.'))
                {
                    msg = e.Message.RawText;
                    if ((msg[0] == '1' || msg[0] == '2' || msg[0] == '3') && (msg[2] == '1' || msg[2] == '2' || msg[2] == '3'))
                    {
                        Console.WriteLine("Command Found: 'X/O'");
                        foreach (TicTacToe T in Program.TTT)
                        {
                            if (!T.Finished)
                            {
                                if (e.User == T.Player[T.Turn])
                                {
                                    await e.Message.Delete();
                                    T.Play(Convert.ToInt32(msg[0]) - 48, Convert.ToInt32(msg[2]) - 48);
                                    return;
                                }
                                else if (e.User == T.Player[1] || e.User == T.Player[0])
                                {
                                    await e.Message.Delete();
                                    Fcn.Discord.TimedMsg(await e.Channel.SendMessage("It's not your turn to play"));
                                    return;
                                }
                            }
                        }
                        await e.Message.Delete();
                        Fcn.Discord.TimedMsg(await e.Channel.SendMessage("You are not in a Tic Tac Toe game"));
                        return;
                    }
                }
                //---------------------Connect 4---------------------//
                CmdTriggers = new string[] { "connect4", "connect 4" };
                if (!e.User.IsBot && Fcn.Msg.CommandCheck(e.Message.RawText, CmdTriggers))
                {
                    await e.Message.Delete();
                    await e.Channel.SendIsTyping();
                    Console.WriteLine("Command Found: 'Connect 4'");
                    msg = Fcn.Msg.Decrypt(e.Message.RawText, CmdTriggers);
                    User P2;
                    if (e.Channel.IsPrivate)
                    {
                        P2 = e.Channel.Users.FirstOrDefault();
                    }
                    else
                    {
                        P2 = e.Message.MentionedUsers.FirstOrDefault();
                    }

                    if (P2 == null || P2 == e.User)
                    {
                        Fcn.Discord.TimedMsg(await e.Channel.SendMessage("Syntax error in Connect4 command: `Invalid User specified`"));
                        return;
                    }
                    foreach (Connect4 C in Program.ConnectF)
                    {
                        if (!C.Finished)
                        {
                            if (e.User == C.Player[0] || e.User == C.Player[1])
                            {
                                Fcn.Discord.TimedMsg(await e.Channel.SendMessage(e.User.Mention + " is already in a Connect 4 game"));
                                return;
                            }
                            else if (P2 == C.Player[0] || P2 == C.Player[1])
                            {
                                Fcn.Discord.TimedMsg(await e.Channel.SendMessage(P2.Mention + " is already in a Connect 4 game"));
                                return;
                            }
                        }
                    }
                    Array.Resize(ref Program.ConnectF, Program.ConnectF.Length + 1);
                    Program.ConnectF[Program.ConnectF.Length - 1] = new Connect4();
                    Program.ConnectF[Program.ConnectF.Length - 1].Start(e.User, P2, e.Channel);
                    return;
                }
                //---------------------Connect 4 Play---------------------//
                if (e.Message.RawText.Length == 1 && char.IsNumber(e.Message.RawText[0]))
                {
                    bool b = false;
                    msg = e.Message.RawText;
                    if (Convert.ToInt32(e.Message.RawText) > 7 && Program.ConnectF.Length > 1)
                    {
                        await e.Message.Delete();
                        Fcn.Discord.TimedMsg(await e.Channel.SendMessage("Index is out of range `1 - 7`"));
                        return;
                    }
                    foreach (Connect4 C in Program.ConnectF)
                    {
                        if (!C.Finished)
                        {
                            b = true;
                            if (e.User == C.Player[C.Turn])
                            {
                                await e.Message.Delete();
                                C.Play(Convert.ToInt32(e.Message.RawText));
                                return;
                            }
                            else if (e.User == C.Player[1] || e.User == C.Player[0])
                            {
                                await e.Message.Delete();
                                Fcn.Discord.TimedMsg(await e.Channel.SendMessage(":anger: It's not your turn to play"));
                                return;
                            }
                        }
                    }
                    if (b)
                    {
                        await e.Message.Delete();
                        Fcn.Discord.TimedMsg(await e.Channel.SendMessage("You are not in a Connect 4 game"));
                    }
                    return;
                }
                //---------------------Typerace---------------------//
                CmdTriggers = new string[] { "typerace", "typeface" };
                if (!e.User.IsBot && Fcn.Msg.CommandCheck(e.Message.RawText, CmdTriggers))
                {
                    await e.Message.Delete();
                    Console.WriteLine("Command Found: 'Typerace'");
                    Array.Resize(ref Program.TRs, Program.TRs.Length + 1);
                    Program.TRs[Program.TRs.Length - 1] = new TypeRace();
                    Program.TRs[Program.TRs.Length - 1].Start(e);
                    return;
                }
                //---------------------Explain---------------------//
                CmdTriggers = new string[] { "explain", "gameinfo" };
                if (!e.User.IsBot && Fcn.Msg.CommandCheck(e.Message.RawText, CmdTriggers))
                {
                    S = "";
                    await e.Message.Delete();
                    msg = Fcn.Msg.Decrypt(e.Message.RawText, CmdTriggers);
                    if (msg.Contains("xo") || msg.Contains("tic tac toe"))
                    {
                        S = S + "Tic <:TTT_X:285804765819174912> Tac <:TTT_O:285804759951212544> Toe can be started by saying `-xo @User` or by DMing me `-xo`";
                        S = S + "\nI will then create the board in the channel with emojis! Remember you can play against me too!";
                        S = S + "\nTo play your turn, you will need to tell me what case you want to play, the board is made of 9 cases, lowest to the left being `1#1` and toppest right `3#3`";
                        S = S + "\nSend a message with ONLY the first number, hash (#) and second number to play, here are the IDs of each case:";
                        S = S + "\n`1#3  2#3  3#3`\n`1#2  2#2  3#2`\n`1#1  2#1  3#1`";
                    }
                    else if (msg.Contains("hang"))
                    {
                        S = S + "<:Hangman:285779439604727809> Hangman can be started by saying `-hang Word #description` though #description is not needed";
                        S = S + "\nYou can start it by private messaging me so no one can see the word/sentence you put";
                        S = S + "\nOther users can play your hangman by sending single-letter messages";
                        S = S + "\nletters sent by you will not count and will be shown as hints";
                        S = S + "\nPlayers are allowed 6 wrong answers and will loose on the 7th";
                        S = S + "\nAll non-letters are revealed thus only A to Z answers are taken";
                    }
                    else if (msg.Contains("typerace") || msg.Contains("typeface"))
                    {
                        S = S + "<:TypeRace:285808847254454272> Type Race can be started by saying `-typerace <difficulty>` *Difficulty is either hard, normal or easy and putting nothing is normal by default*";
                        S = S + "\nAfter starting, I will countdown from 3 and show a random sentence which you will need to retype as fast as possible";
                        S = S + "\nCapitals are not needed, you are allowed `1/3/5` errors for `Hard/Normal/Easy`";
                        S = S + "\nI am designed to catch cheaters, so beware!";
                        S = S + "\nAfter you send the Message I showed, you will get a star-based rating based on how fast and how correct you were";
                    }
                    else
                    {
                        return;
                    }
                    Fcn.Discord.TimedMsg(await e.Channel.SendMessage(S), 80000);
                    return;
                }
                //---------------------Fuck---------------------//
                CmdTriggers = new string[] { "fuck", "damn" };
                if (!e.User.IsBot && Fcn.Msg.CommandCheck(e.Message.RawText, CmdTriggers, false))
                {
                    await e.Channel.SendIsTyping();
                    Console.WriteLine("Command Found: 'Fuck'");
                    string[] fuckreply =
                    {
                        "YEAH! Fuck ",
                        "Hell yeah, fuck ",
                        ":knife: :knife: ",
                        "Damn right! Fuck ",
                        "FUCJ ",
                        "To hell with ",
                        "Damn ",
                        ":rage: DAMN ",
                        ":smiling_imp: See you in hell ",
                        ":skull_crossbones: ",
                        ":poop: on ", "Shit on "
                    },
                    MasterTriggers = { "t. d. w.", "t.d.w", "td", "jad" };
                    msg = Fcn.Msg.Decrypt(e.Message.RawText, CmdTriggers, false);
                    Console.WriteLine("Event: Ping Pong type 'Fuck' triggered by " + e.User.Name);
                    if (Fcn.Msg.CommandCheck(e.Message.RawText, MasterTriggers, false))
                    {
                        await e.Channel.SendMessage("Dobby doesn't want to be mean to master");
                    }
                    else if (msg == "" || msg == " ")
                    {
                        await e.Channel.SendMessage("Well Fuck you Too!");
                    }
                    else
                    {
                        await e.Channel.SendMessage(fuckreply[Program.rand.Next(fuckreply.Length)] + msg);
                    }

                    return;
                }
                //---------------------Meme---------------------//
                CmdTriggers = new string[] { "meme", "mem", "memes", "mems" };
                if (!e.User.IsBot && Fcn.Msg.CommandCheck(e.Message.RawText, CmdTriggers, false))
                {
                    await e.Channel.SendIsTyping();
                    Console.WriteLine("Command Found: 'Meme'");
                    string[] MemeResponce =
                    {
                        "Fresh meme comin up..",
                        "Prepare to get MEMEd! :grimacing: ",
                        "Meme is off the charts!! :chart_with_upwards_trend: ",
                        "All dem memes.. :upside_down: ",
                        "DID SOMEONE SAY memes?!",
                        "On it!"
                    };
                    Console.WriteLine("Action: Sending Meme as requested by " + e.User.Name);
                    await e.Channel.SendIsTyping();
                    MSG = await e.Channel.SendMessage(":speech_balloon:");
                    string R = GetMeme(e.Message.RawText);
                    await MSG.Delete();
                    Fcn.Discord.WaitForMsg(await e.Channel.SendMessage(MemeResponce[Program.rand.Next(MemeResponce.Length)]));
                    await e.Channel.SendIsTyping();
                    if (R.StartsWith("../../meme/"))
                    {
                        await e.Channel.SendFile(R);
                        System.Timers.Timer MainTimer = new System.Timers.Timer(10000);
                        MainTimer.Start();
                        MainTimer.Elapsed += (Se, E) =>
                        {
                            MainTimer.Stop();
                            File.Delete(R);
                        };
                    }
                    else
                    {
                        await e.Channel.SendMessage(R);
                    }

                    return;
                }
                //---------------------Picture---------------------//
                CmdTriggers = new string[] { "funny", "fun", "picture", "pic" };
                if (!e.User.IsBot && Fcn.Msg.CommandCheck(e.Message.RawText, CmdTriggers, false))
                {
                    await e.Channel.SendIsTyping();
                    Console.WriteLine("Command Found: 'Picture'");
                    string[] FunnyResponce =
                    {
                        "On it!",
                        "Some find it funny, others find it badass.. it's all about taste.. :thinking: ",
                        "Get Fun'ed :grimacing: ",
                        "Pictures pictures pictures. :frame_photo: ",
                        "So many pictures :frame_photo: ",
                        "Picture Delivery! :motor_scooter: ",
                        "Freshly Baked Pictures! Right Here :french_bread: "
                    };
                    Console.WriteLine("Action: Sending Picture as requested by " + e.User.Name);
                    await e.Channel.SendIsTyping();
                    MSG = await e.Channel.SendMessage(":speech_balloon:");
                    string R = GetMeme(e.Message.RawText);
                    await MSG.Delete();
                    Fcn.Discord.WaitForMsg(await e.Channel.SendMessage(FunnyResponce[Program.rand.Next(FunnyResponce.Length)]));
                    await e.Channel.SendIsTyping();
                    if (R.StartsWith("../../meme/"))
                    {
                        await e.Channel.SendFile(R);
                        System.Timers.Timer MainTimer = new System.Timers.Timer(10000);
                        MainTimer.Start();
                        MainTimer.Elapsed += (Se, E) =>
                        {
                            MainTimer.Stop();
                            File.Delete(R);
                        };
                    }
                    else
                    {
                        await e.Channel.SendMessage(R);
                    }

                    return;
                }
                //---------------------Gif---------------------//
                CmdTriggers = new string[] { "gif", "jif" };
                if (!e.User.IsBot && Fcn.Msg.CommandCheck(e.Message.RawText, CmdTriggers, false))
                {
                    await e.Channel.SendIsTyping();
                    Console.WriteLine("Command Found: 'Gif'");
                    string[] GifResponce =
                    {
                        "Amma giff you some.. :wink:",
                        "On it!",
                        "Gif.. Jiffs.. I mean who cares? right?",
                        "Here's for my Gif Lovers :blue_heart: ",
                        "This ought to be interestingiff..\n*bad joke, master knows*",
                        "Get Giff'ed :video_camera: "
                    };
                    Console.WriteLine("Action: Sending Gif as requested by " + e.User.Name);
                    await e.Channel.SendIsTyping();
                    MSG = await e.Channel.SendMessage(":speech_balloon:");
                    string R = GetMeme(e.Message.RawText);
                    await MSG.Delete();
                    Fcn.Discord.WaitForMsg(await e.Channel.SendMessage(GifResponce[Program.rand.Next(GifResponce.Length - 1)]));
                    await e.Channel.SendIsTyping();
                    if (R.StartsWith("../../meme/"))
                    {
                        await e.Channel.SendFile(R);
                        System.Timers.Timer MainTimer = new System.Timers.Timer(10000);
                        MainTimer.Start();
                        MainTimer.Elapsed += (Se, E) =>
                        {
                            MainTimer.Stop();
                            File.Delete(R);
                        };
                    }
                    else
                    {
                        await e.Channel.SendMessage(R);
                    }

                    return;
                }
                //---------------------Hello---------------------//
                CmdTriggers = new string[] { "hi", "hey", "hey", "sup", "wassup", "oy", "hello" };
                if (!e.User.IsBot && Fcn.Msg.CommandCheck(e.Message.RawText, CmdTriggers, false))
                {
                    await e.Channel.SendIsTyping();
                    string[] HelloResponses =
                    {
                        "Hey!", "Hey",
                        "Hello.", "Hello!", "`Print Msg(` Hello :robot: `)`",
                        "Hey there!", "Hey there :eyes:",
                        "Sup", "Sup :call_me:",
                        "Greetings",
                        "Ahoy there!", "Ahoy there! :sailboat: ",
                        "Herow",
                        "Kon'nichiwa",
                        "Hai",
                        "Yo :metal:"
                    };
                    Console.WriteLine("Command Found: 'Hello'");
                    Console.WriteLine("Event: Ping Pong type 'Hello' triggered by " + e.User.Name);
                    await e.Channel.SendMessage(HelloResponses[Program.rand.Next(HelloResponses.Length - 1)]);
                    return;
                }
                //---------------------Thanks---------------------//
                CmdTriggers = new string[] { "thank", "thx", "tank" };
                if (!e.User.IsBot && Fcn.Msg.CommandCheck(e.Message.RawText, CmdTriggers, false))
                {
                    await e.Channel.SendIsTyping();
                    string[] ThankResponses =
                    {
                        "You are Welcome","You're Welcome :grinning:",
                        "No Problem !","No Problem :hugging: ",
                        "Anytime","Anytime :v: ",
                        "Of course","Of course :grin: ",
                        "My Pleasure", "My Pleasure :smile: ",
                        "Sure", "Sure :wink: "
                    };
                    Console.WriteLine("Command Found: 'Thanks'");
                    Console.WriteLine("Event: Ping Pong type 'Thanks' triggered by " + e.User.Name);
                    await e.Channel.SendMessage(ThankResponses[Program.rand.Next(ThankResponses.Length - 1)]);
                    return;
                }
                //---------------------Call Bot---------------------//
                CmdTriggers = new string[] { "bot", "callbot" };
                if (!e.User.IsBot && Fcn.Msg.CommandCheck(e.Message.RawText, CmdTriggers, false))
                {
                    await e.Channel.SendIsTyping();
                    Console.WriteLine("Command Found: 'Call Bot'");
                    await e.Message.Delete();
                    Console.WriteLine("Event: " + e.User.Name + " called the Bot");
                    IEnumerable<Server.Emoji> Em = e.Server.CustomEmojis;
                    await e.Channel.SendMessage("Hey! I'm the <@272383726300692500> <:codesel:249266261928706048> \n\nI'm here to assist you with random things around this server!\nIf you need me, you can start your message with ` ' - ' ` or mention me!\nFor more help and a list of my commands, use the `-help` command");
                    return;
                }
                //---------------------Disconnect---------------------//
                CmdTriggers = new string[] { "disconnect", "bye" };
                if (!e.Channel.IsPrivate && !e.User.IsBot && (e.User.Roles.First().Name.ToString() == "Code.SEL Member") && Fcn.Msg.CommandCheck(e.Message.RawText, CmdTriggers, false))
                {
                    await e.Channel.SendIsTyping();
                    Console.WriteLine("Command Found: 'Disconnect'");
                    Console.WriteLine("Action: " + e.User.Name + " is Shutting Down the Bot");
                    await e.Channel.SendMessage(":wave:");
                    System.Timers.Timer MainTimer = new System.Timers.Timer(5000);
                    MainTimer.Start();
                    MainTimer.Elapsed += async (Se, E) =>
                    {
                        MainTimer.Stop();
                        await discord.Disconnect();
                        return;
                    };
                }
                //---------------------Update Data---------------------//
                CmdTriggers = new string[] { "updatedata" };
                if (Fcn.Msg.CommandCheck(e.Message.RawText, CmdTriggers))
                {
                    await e.Message.Delete();
                    await e.Channel.SendIsTyping();
                    Console.WriteLine("Command Found: 'Update Data'");
                    UpdateStats();
                    Fcn.Discord.TimedMsg(await e.Channel.SendMessage("Data has been updated"), 60000);
                    return;
                }
                //---------------------Restart---------------------//
                CmdTriggers = new string[] { "restart" };
                if (Fcn.Msg.CommandCheck(e.Message.RawText, CmdTriggers))
                {
                    Console.WriteLine("Command Found: 'Restart'");
                    await e.Channel.SendIsTyping();
                    await e.Message.Delete();
                    MSG = await e.Channel.SendMessage("Restarting.");
                    System.Timers.Timer MainTimer = new System.Timers.Timer(500);
                    MainTimer.Start();
                    MainTimer.Elapsed += async (Se, E) =>
                    {
                        MainTimer.Stop();
                        await MSG.Edit("Restarting..");
                        System.Timers.Timer MainTimer_2 = new System.Timers.Timer(500);
                        MainTimer_2.Start();
                        MainTimer_2.Elapsed += async (Se_2, E_2) =>
                        {
                            MainTimer_2.Stop();
                            await MSG.Edit("Restarting...");
                            System.Timers.Timer MainTimer_3 = new System.Timers.Timer(500);
                            MainTimer_3.Start();
                            MainTimer_3.Elapsed += async (Se_3, E_3) =>
                            {
                                MainTimer_3.Stop();
                                await MSG.Delete();
                                System.Timers.Timer MainTimer_4 = new System.Timers.Timer(500);
                                MainTimer_4.Start();
                                MainTimer_4.Elapsed += async (Se_4, E_4) =>
                                {
                                    MainTimer_4.Stop();
                                    Application.Restart();
                                    await discord.Disconnect();
                                };
                            };
                        };
                    };
                    return;
                }
                //---------------------Oxyl Skip---------------------//
                if(!e.Channel.IsPrivate && (e.Message.RawText == "skip" || (e.Message.RawText.Contains("oxyl") && e.Message.RawText.Contains("skip"))))
                {
                    await e.Message.Delete();
                    if(e.Message.RawText == "skip")
                        Fcn.Discord.TimedMsg(await e.Channel.SendMessage($"{e.User.Mention}, try: `oxyl skip`"));
                }
                if(e.User.Id == 255832257519026178)
                {
                    System.Timers.Timer OxTimer = new System.Timers.Timer(60000);
                    OxTimer.Start();
                    OxTimer.Elapsed += async (se, ee) => { await e.Message.Delete(); };
                }
                //---------------------Music Bot AutoMove---------------------//
                if (!e.Channel.IsPrivate && e.Message.RawText.Contains("oxyl") && e.Message.RawText.Contains("play"))
                {
                    Program.AllUsers[Fcn.Discord.GetA_UsersIndex(e.User)].SongsPlayed++;
                    Program.AllUsers[Fcn.Discord.GetA_UsersIndex(e.User)].Update();
                    await e.Message.Delete();
                    System.Timers.Timer T2 = new System.Timers.Timer(11500);
                    T2.Start();
                    T2.Elapsed += async (p, y) =>
                    {
                        T2.Stop();
                        if (e.User.VoiceChannel.Id == 235871481307987968)
                        {
                            foreach (User u in e.Server.GetChannel(235871481307987968).Users)
                            {
                                await u.Edit(null, null, e.Server.GetChannel(272736534418161664));
                            }

                            Fcn.Discord.TimedMsg(await e.Channel.SendMessage(":arrow_right: Moved users from :coffee: Main to :headphones: Music"), 10000);
                        }
                    };
                }
                //---------------------Typerace Answer---------------------//
                if (!e.User.IsBot && Program.TRs.Length != 1)
                {
                    bool b = false;
                    foreach (TypeRace TR in Program.TRs)
                    {
                        if (TR.Word != "")
                        { TR.Play(e.Message); b = true; }
                    }
                    if (b)
                    {
                        return;
                    }
                }
                //---------------------End of Line---------------------//
                if (!e.User.IsBot && e.Message.RawText.Length > 1)
                {
                    Console.WriteLine("No Commands Found");
                }
            };
        }

        private void Greet()
        {
            discord.UserJoined += async (s, e) =>
            {
                if (!e.User.IsBot)
                {
                    Console.WriteLine("Event: " + e.User.Name + " Joined the Server");
                    System.Threading.Thread.Sleep(1000);
                    Console.WriteLine("Action: " + e.User.Name + " was greeted and asked to choose his Role");
                    Program.UserJoin.Add(new UserJoined());
                    Program.UserJoin.Last().Joined = true;
                    Program.UserJoin.Last().User = e.User;
                    Program.UserJoin.Last().Mess = await e.Server.DefaultChannel.SendMessage($"Hello, " + e.User.Mention + "!\n\nWelcome to Code.SEL, please specifiy what category you are in:\n      1. " + e.Server.FindRoles("Dota Buddy").FirstOrDefault().Mention + "\n      2. " + e.Server.FindRoles("Rocket League Buddy").FirstOrDefault().Mention + "\n      3. " + e.Server.FindRoles("GTA Buddy").FirstOrDefault().Mention + "\n      4. " + e.Server.FindRoles("Overwatch Buddy").FirstOrDefault().Mention + "\n      5. " + e.Server.FindRoles("CS GO Buddy").FirstOrDefault().Mention + "\n\n*send a message starting with a dash `-` and the number. Ex: `-1`*");
                }
                else
                {
                    Console.WriteLine("Event: " + e.User.Name + " Joined the Server");
                    Console.WriteLine("Action: " + e.User.Name + " was granted BOTS Role");
                    await e.User.AddRoles(e.Server.GetRole(272385653420064768));
                }
            };
        }

        private void BuddySelection()
        {
            commands.CreateCommand("1")
            .Alias("2", "3", "4", "5")
            .Do(async (e) =>
            {
                foreach (UserJoined UJ in Program.UserJoin)
                {
                    if(UJ.Joined && UJ.User == e.User)
                    {
                        await UJ.Mess.Delete();
                        await e.Message.Delete();
                        await Program.Buddies[int.Parse(e.Message.Text[1].ToString()) - 1].Select(e.User);
                        UJ.Joined = false;
                    }
                }
            });
        }

        private void UserEvents()
        {
            discord.UserBanned += async (s, e) =>
            {
                await e.Server.DefaultChannel.SendMessage(e.User.Mention + " was Banned from the Server.");
                Console.WriteLine("Event: " + e.User.Name + " was Banned from the Server");
            };
            discord.UserLeft += async (s, e) =>
            {
                await e.Server.DefaultChannel.SendMessage(e.User.Mention + " left the Server.");
                Console.WriteLine("Event: " + e.User.Name + " was Kicked from/Left the Server");
            };
            discord.UserUnbanned += async (s, e) =>
            {
                await e.Server.DefaultChannel.SendMessage(e.User.Mention + " was UnBanned.");
                Console.WriteLine("Event: " + e.User.Name + " was Unbanned from the Server");
            };
        }

        private void MsgDel()
        {
            discord.MessageDeleted += async (s, e) =>
            {
                foreach (TicTacToe T in Program.TTT)
                {
                    try
                    {
                        if (!T.Finished && (e.Message.Id == T.Board.Id || e.Message.Id == T.Mess.Id))
                        {
                            await e.Channel.SendMessage("The Tic Tac Toe board Fcn.Msg.Between +" + T.Player[0].Name + " and " + T.Player[0].Name + " was deleted, game over");
                            T.Finished = true;
                        }
                    }
                    catch (NullReferenceException) { }
                }

                foreach (Hangman H in Program.Hang)
                {
                    try
                    {
                        if (!H.Finished && (e.Message.Id == H.Hang.Id || e.Message.Id == H.Mess.Id))
                        {
                            await e.Channel.SendMessage("Hangman board Fcn.Msg.Between was deleted, game over");
                            H.Finished = true;
                        }
                    }
                    catch (NullReferenceException) { }
                }
            };
        }

        private void Log(object sender, LogMessageEventArgs e)
        {
            Console.WriteLine(e.Message);
        }

        private void OnConnect()
        {
            if (!Program.Initialized)
            {
                System.Timers.Timer T = new System.Timers.Timer(5000);
                discord.ServerAvailable += (s, e) =>
                {
                    System.Timers.Timer T2 = new System.Timers.Timer(500);
                    T2.Start();
                    T2.Elapsed += (p, y) =>
                    {
                        Program.DefChan = discord.Servers.FirstOrDefault().DefaultChannel;
                        T2.Stop();
                        User[] Users = discord.Servers.FirstOrDefault().Users.ToArray();
                        Array.Resize(ref Program.AllUsers, discord.Servers.FirstOrDefault().UserCount);
                        for (int i = 0; i < Program.AllUsers.Length; i++)
                        {
                            Program.AllUsers[i] = new A_Users() { User = Users[i] };
                            Program.AllUsers[i].Read();
                            Program.AllUsers[i].Update();
                        }
                        foreach(A_Users a in Program.AllUsers)
                        if (Program.AllUsers == null)
                            return;
                        T.Start();
                        if ((DateTime.Now.DayOfWeek.ToString() == "Tuesday" || DateTime.Now.DayOfWeek.ToString() == "Friday") && DateTime.Now.Hour == 12)
                            Backup();
                        if (Program.Buddies.Count <= 1)
                        {
                            Program.Buddies.Add(new Buddy(1, discord.Servers.FirstOrDefault()));
                            Program.Buddies.Add(new Buddy(2, discord.Servers.FirstOrDefault()));
                            Program.Buddies.Add(new Buddy(3, discord.Servers.FirstOrDefault()));
                            Program.Buddies.Add(new Buddy(4, discord.Servers.FirstOrDefault()));
                            Program.Buddies.Add(new Buddy(5, discord.Servers.FirstOrDefault()));
                        }
                    /*
                    string[] dat = File.ReadAllLines("../../Hang_Data.txt");
                    foreach(string ss in dat)
                    {
                        if(discord.Servers.First().FindUsers(Fcn.Msg.Between(ss, "$Starter\"", "\" ")).FirstOrDefault() != null)
                        {
                            Program.AllUsers[Fcn.Discord.GetA_UsersIndex(discord.Servers.First().FindUsers(Fcn.Msg.Between(ss, "$Starter\"", "\" ")).FirstOrDefault())].HangStarted++;
                            Program.AllUsers[Fcn.Discord.GetA_UsersIndex(discord.Servers.First().FindUsers(Fcn.Msg.Between(ss, "$Starter\"", "\" ")).FirstOrDefault())].Update();
                        }
                        if (discord.Servers.First().FindUsers(Fcn.Msg.Between(ss, "$Solver\"", "\" ")).FirstOrDefault() != null)
                        {
                            Program.AllUsers[Fcn.Discord.GetA_UsersIndex(discord.Servers.First().FindUsers(Fcn.Msg.Between(ss, "$Solver\"", "\" ")).FirstOrDefault())].HangSolved++;
                            Program.AllUsers[Fcn.Discord.GetA_UsersIndex(discord.Servers.First().FindUsers(Fcn.Msg.Between(ss, "$Solver\"", "\" ")).FirstOrDefault())].Update();
                        }
                    }
                    */
                    };
                };
                T.Elapsed += Looper;
                Program.Initialized = true;
            }
        }

        private void Looper(object sender, System.Timers.ElapsedEventArgs e)
        {
            foreach (A_Users AU in Program.AllUsers)
            {
                if (AU.AutoMove && AU.User.Status.Value == "online" && AU.User.CurrentGame != null)
                {
                    if (AU.User.VoiceChannel != null)
                    {
                        if ((!AU.Moved || AU.GameName != AU.User.CurrentGame.Value.Name) && !AU.User.IsBot)
                        {
                            if (Fcn.Discord.GetChannel(AU.User.CurrentGame.Value.Name) != null && Fcn.Discord.GetChannel(AU.User.CurrentGame.Value.Name).Id != AU.User.VoiceChannel.Id)
                            {
                                if (!AU.AFKMove && AU.User.VoiceChannel.Id == 155958790124994560)
                                { }
                                else
                                    AU.User.Edit(null, null, Fcn.Discord.GetChannel(AU.User.CurrentGame.Value.Name));
                            }

                            AU.Moved = true;
                            AU.GameName = AU.User.CurrentGame.Value.Name;
                        }
                    }
                    if (AU.User.CurrentGame.Value.Name != "" && !AU.Games.Contains(AU.User.CurrentGame.Value.Name))
                    {
                        AU.Games.Add(AU.User.CurrentGame.Value.Name);
                    }
                }
                
                if(AU.User.Status.Value == "online")
                {
                    AU.LastOnline = DateTime.Now;
                    if (AU.OnlineSince.ToString() == "01-Jan-01 12:00:00 AM")
                    {
                        AU.OnlineSince = DateTime.Now;
                    }
                }
                else
                {
                    AU.OnlineSince = DateTime.Parse("01-Jan-01 12:00:00 AM");
                }

                AU.Update();
            }
            try
            {
                foreach (Channel C in discord.Servers.FirstOrDefault().VoiceChannels)
                {
                    if (C.Id != 235871481307987968 && C.Id != 272736534418161664 && C.Id != 155958790124994560)
                    {
                        bool b = C.Users.Count() > 0;
                        foreach (Discord.User U in C.Users)
                        {
                            if (U.CurrentGame != null || !Program.AllUsers[Fcn.Discord.GetA_UsersIndex(U)].AutoMove)
                            {
                                b = false;
                            }
                        }

                        if (b)
                        {
                            Fcn.Discord.TimedMsg(Program.DefChan.SendMessage($":arrow_right: Moved users from {C.Name} to :coffee: Main").Result, 10000);
                            foreach (Discord.User U in C.Users)
                            {
                                U.Edit(null, null, Fcn.Discord.GetChannel("main"));
                            }
                        }
                    }
                }
            }
            catch (NullReferenceException) { }
        }

        private void Data_Logging()
        {
            discord.MessageReceived += (s, e) =>
            {
                int i = Fcn.Discord.GetA_UsersIndex(e.User);
                try
                {
                    Program.AllUsers[i].MessagesSent++;
                    Program.AllUsers[i].CharSent += e.Message.RawText.Length;
                    Program.AllUsers[i].Update();
                }
                catch (NullReferenceException) { }
            };
        }
    }
}