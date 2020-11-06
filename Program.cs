using System;

namespace AutoProjekt
{
    class Program
    {
        static void Main(string[] args)
        {

            Auto meinAuto = new Auto();
            Ferrari meinFerrari = new Ferrari();
            Delorean meinDelorean = new Delorean();
            /*
            meinAuto.TuerenAusgeben();
            meinAuto.TuerenBenutzen(1);
            meinAuto.TuerenAusgeben();
            meinAuto.TuerenBenutzen(1);
            meinAuto.TuerenAusgeben();
            meinAuto.TuerenBenutzen(1);
            meinAuto.TuerenAusgeben();
            meinAuto.TuerenBenutzen(1);
            meinAuto.TuerenAusgeben();
            meinAuto.TuerenBenutzen(3);
            meinAuto.TuerenAusgeben();
            meinAuto.TuerenBenutzen(1);
            meinAuto.TuerenBenutzen(1);
            */

            meinAuto.TuerenAusgeben();
            meinAuto.TuerenBenutzen(AutoTuer.VornLinks);
            meinAuto.TuerenAusgeben();
            meinAuto.TuerenBenutzen(AutoTuer.VornLinks);
            meinAuto.TuerenAusgeben();

            Auto MeinZweitwagen = new Auto((byte)(AutoTuer.HintenLinks | AutoTuer.HintenRechts | AutoTuer.TankDeckel));

        }

    }
    class Auto
    {
        protected byte _tueren; // enthält den zustand von 8 türen
        protected byte _aktivierbareTueren;

        public Auto()
        {
            _aktivierbareTueren = 255;
        }
        public Auto(byte ErlaubteTueren)
        {
            _aktivierbareTueren = ErlaubteTueren;
        }

        public void TuerenBenutzen(byte TuerNummer)
        {
            if ((_aktivierbareTueren & 1 << TuerNummer) == 0) //early exit wenn nicht erlaubte tür benutzt wird
            {
                Console.WriteLine("Tür ist verboten");
                return;
            }


            byte Lesekopf = 0b_0000_0001;
            Lesekopf = (byte)(Lesekopf << TuerNummer); // schiebt das lesebit bei jedem durchlauf eins weiter nach links

            // 0000 1011   // Dez 11 zustand der türen
            // 0000 0010   // Dez  2 lesekopf wenn nach tür 1 gefragt wird
            // 0000 0010   // Dez  2 ergebnis der UND verknüpfung

            if ((_tueren & Lesekopf) > 0)
            {
                // das gesetzte bit muss auf 0 zurück
                _tueren = (byte)(_tueren ^ Lesekopf);
                Console.WriteLine("Tür " + TuerNummer + " ist jetzt zu");
            }
            else
            {
                // das leere bit muss gesetzt werden
                _tueren += Lesekopf;
                Console.WriteLine("Tür " + TuerNummer + " ist jetzt auf");
            }
        }

        public void TuerenBenutzen(AutoTuer Tuer)
        {
            if ((_aktivierbareTueren & (byte)Tuer) == 0) //early exit wenn nicht erlaubte tür benutzt wird
            {
                Console.WriteLine("Tür ist verboten");
                return; // beendet vorzeitig die methode
            }

            if ((_tueren & (byte)Tuer) > 0)
            {
                // das gesetzte bit muss auf 0 zurück
                _tueren = (byte)(_tueren ^ (byte)Tuer);
                Console.WriteLine("Tür " + Tuer + " ist jetzt zu");
            }
            else
            {
                // das leere bit muss gesetzt werden
                _tueren += (byte)Tuer;
                Console.WriteLine("Tür " + Tuer + " ist jetzt auf");
            }




        }

        public void TuerenAusgeben()
        {
            byte Lesekopf = 0b_1000_0000;
            for (int i = 0; i < 8; i++) //Hack: geht fest von 8 möglichen türen aus
            {
                Console.Write(((_tueren & Lesekopf) > 0 ? "  true" : " false"));
                Lesekopf = (byte)(Lesekopf >> 1);
            }
            Console.WriteLine();
        }
    }

    class Ferrari : Auto
    {
        public Ferrari(byte erlaubteTueren) : base(erlaubteTueren) { }

        public Ferrari()
        {
            _aktivierbareTueren = (byte)(AutoTuer.MotorHaube | AutoTuer.SchiebeDach | AutoTuer.TankDeckel | AutoTuer.VornLinks | AutoTuer.VornRechts);
        }
    }

    class Delorean : Auto
    {
        public Delorean()
        {
            _aktivierbareTueren = 255;
        }
    }

    enum AutoTuer
    {
        VornLinks = 0b_0000_0001, // 1   0x01
        VornRechts = 0b_0000_0010, // 2
        HintenLinks = 0b_0000_0100, // 4
        HintenRechts = 0b_0000_1000, // 8
        KofferRaum = 0b_0001_0000, // 16
        MotorHaube = 0b_0010_0000, // 32
        SchiebeDach = 0b_0100_0000, //64
        TankDeckel = 0b_1000_0000 //128
    }
}
