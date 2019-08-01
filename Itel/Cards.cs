using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Itel
{
    public static class CardPrices
    {

    }

    public static class DenominationID
    {
        public const int TouchSmall = 80011;
        public const int TouchMedium = 80017;
        public const int Touch1Month = 80022;
        public const int Touch2Month = 80045;
        public const int Touch3Month = 80068;

        public const int StartSOS = 80040;
        public const int Start = 80010;
        public const int Smart = 80016;
        public const int Super = 80030;

        public const int AlfaSmall = 81010;
        public const int AlfaMedium = 81016;
        public const int Alfa1Month = 81025;
        public const int AlfaSOS = 81004;


        public const int PSN_USA_10 = 85010;
        public const int PSN_USA_20 = 85020;
        public const int PSN_USA_25 = 85025;
        public const int PSN_LEB_5 = 85005;
        public const int PSN_LEB_10 = 85008;
        public const int PSN_LEB_20 = 85022;
        public const int PSN_UAE_5 = 85002;
        public const int PSN_UAE_10 = 85009;
        public const int PSN_UAE_20 = 85019;


        public const int GOOGLE_10 = 18010;
        public const int GOOGLE_15 = 18015;
        public const int GOOGLE_25 = 18025;
        public const int GOOGLE_50 = 18050;
    }

    class RechargeCards
    {
        int TouchSmall, TouchMedium, Touch1Month, Touch2Month, Touch3Month;
        int AlfaSmall, Alfa1Month;
        int Start, SOS;
    }

    class PlayAndValidityCards
    {
        int Google10, Google15, Google25, Google50;
        int PSN5, PSN10, PSN20;
        int ValidityTouch;
    }

    class Day
    {
        DateTime Date;
        double Cash, ExtraCash, Debts, Loss;
        RechargeCards PurchasedCardsPaper, PurchasedCardsProgram, CardsPaper, CardsProgram;
        PlayAndValidityCards OtherCards;
        double TouchDollars, AlfaDollars;

        double Total;
    }
}
