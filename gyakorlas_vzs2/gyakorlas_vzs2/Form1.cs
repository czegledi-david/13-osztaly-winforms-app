using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace gyakorlas_vzs2
{
    public partial class Form1 : Form
    {
        public class Tranzakcio
        {
            public int Nap { get; private set; }
            public string Idopont { get; private set; }
            public string Termek { get; private set; }
            public int DolgozoId { get; private set; }
            public int Mennyiseg { get; private set; }
            public string Muvelet { get; private set; } // S vagy E
            
            public Tranzakcio(int nap, string idopont, string termek, int dolgozoid, int mennyiseg, char m)
            {
                Nap = nap;
                Idopont = idopont;
                Termek = termek;
                DolgozoId = dolgozoid;
                Mennyiseg = mennyiseg;
                Muvelet = (m == 'S') ? "Sütés" : "Eladás";
            }


        }

        private List<Tranzakcio> mozgasok = new List<Tranzakcio>();

        private void AdatokBeolvasasa(string fajlNev)
        {
            try
            {
                string[] sorok = File.ReadAllLines(fajlNev);
                foreach (string s in sorok) 
                {
                    if (string.IsNullOrWhiteSpace(s)) continue;

                    string[] adatok = s.Split(' ');
                    Tranzakcio t = new Tranzakcio(
                        int.Parse(adatok[0]),
                        adatok[1],
                        adatok[2],
                        int.Parse(adatok[3]),
                        int.Parse(adatok[4]),
                        char.Parse(adatok[5])
                    );
                    mozgasok.Add(t);
                }
                //Betölti az összes adatot a táblázatba induláskor
                dataGridView1.DataSource = mozgasok;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hiba a fájl beolvasásakor: " + ex.Message, "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        public Form1()
        {
            InitializeComponent();
            AdatokBeolvasasa("pekseg.txt");
        }

        //Utolsó eladott gomb
        private void button1_Click(object sender, EventArgs e)
        {
            for(int i = mozgasok.Count -1; i >= 0; i--)
            {
                if (mozgasok[i].Muvelet == "Eladás")
                {
                    label1.Text = "Utolsó eladott pékáru: " + mozgasok[i].Termek;
                    return; //Megtaláltuk, befejezzük a futást
                }
            }
            label1.Text = "Nem volt eladás.";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //Csak a nap kell
            int valasztottnap = dateTimePicker1.Value.Day;

            List<Tranzakcio> szurtlista = new List<Tranzakcio>();

            foreach(Tranzakcio t in mozgasok)
            {
                if (t.Nap == valasztottnap)
                {
                    szurtlista.Add(t);
                } 
            }
            
            //Ha nincs adat akkor kiürítjük a táblázatot
            if(szurtlista.Count == 0)
            {
                dataGridView1.DataSource = null;
                MessageBox.Show("Ezen a napon nem volt forgalom.", "Információ", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                //Kicseréljük a szűrt listára
                dataGridView1.DataSource = szurtlista;
            }



        }
    }
}
