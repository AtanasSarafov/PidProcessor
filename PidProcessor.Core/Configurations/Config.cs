using System.Collections.Generic;
using System.Linq;

namespace PidProcessor.Core.Configurations
{
    public static class Config
    {
        private static readonly int[] regionsRange = Enumerable.Range(0, 999).ToArray();

        public const string DateFormat = "MM-dd-yyyy";

        public static int[] Weights = new[] { 2, 4, 8, 5, 10, 9, 7, 3, 6 };

        public static List<(string Name, int[] Range)> Regions = new List<(string, int[])>()
        {
            ( "Blagoevgrad",        regionsRange[..43] ),
            ( "Burgas",             regionsRange[44..93] ),
            ( "Varna",              regionsRange[94..139] ),
            ( "Veliko Tarnovo",     regionsRange[140..169] ),
            ( "Vidin",              regionsRange[170..183] ),
            ( "Vratsa",             regionsRange[184..217] ),
            ( "Gabrovo",            regionsRange[218..233] ),
            ( "Kardzhali",          regionsRange[234..281] ),
            ( "Kyustendil",         regionsRange[282..301] ),
            ( "Lovech" ,            regionsRange[302..319] ),
            ( "Montana",            regionsRange[320..341] ),
            ( "Pazardzhik",         regionsRange[342..377] ),
            ( "Gingerbread",        regionsRange[378..395] ),
            ( "Pleven" ,            regionsRange[396..435] ),
            ( "Plovdiv",            regionsRange[436..501] ),
            ( "Razgrad",            regionsRange[502..527] ),
            ( "Ruse",               regionsRange[528..555] ),
            ( "Silistra",           regionsRange[556..575] ),
            ( "Sliven",             regionsRange[576..601] ),
            ( "Smolyan",            regionsRange[602..623] ),
            ( "Sofia city" ,        regionsRange[624..721] ),
            ( "Sofia - district",   regionsRange[722..751] ),
            ( "Stara Zagora",       regionsRange[752..789] ),
            ( "Dobrich (Tolbukhin)",regionsRange[790..821] ),
            ( "Targovishte",        regionsRange[822..843] ),
            ( "Haskovo",            regionsRange[844..871] ),
            ( "Shumen",             regionsRange[872..903] ),
            ( "Yambol",             regionsRange[904..925] ),
            ( "Other / Unknown",    regionsRange[926..999] )
        };
    }
}

