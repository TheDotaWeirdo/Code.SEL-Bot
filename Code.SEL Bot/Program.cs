using Discord;
using DiscordBot;
using System;
using Code.SEL_Bot;
using System.Collections.Generic;
using Discord.Commands;

namespace Code.SEL_Bot
{
    public class Program
    {

        static public bool KickVoting = true, BanVoting = true, ListUsersWaiting = false, ListUsersStopped = false;
        static public Kick[] VoteKick;
        static public Ban[] VoteBan;
        static public Exile[] Exiles;
        static public TypeRace[] TRs;
        static public TicTacToe[] TTT;
        static public Hangman[] Hang;
        static public Connect4[] ConnectF;
        public static A_Users[] AllUsers;
        static public Random rand;
        static public Channel DefChan;
        static public List<Buddy> Buddies = new List<Buddy>();
        static public List<UserJoined> UserJoin = new List<UserJoined>();
        internal static bool Initialized = false;

        static void Main(string[] args)
        {
            MyBot Bot = new MyBot();
        }
    }
}
