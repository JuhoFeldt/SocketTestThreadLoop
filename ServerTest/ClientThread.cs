﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.IO;
using System.Threading;
namespace ServerTest
{
    // tässä luokassa on asiakkaan palveleminen
    // omassa säikeessään (sivu 34)
    class ClientThread
    {
        private TcpClient client;
        public ClientThread(TcpClient client)
        {
            this.client = client;
        }

        // ajetaan omassa säikeessään

        public void ServeClient()
        {
            // avataan yhteydet
            // avataan streamit
            NetworkStream ns = client.GetStream();
            StreamWriter sw = new StreamWriter(ns);
            StreamReader sr = new StreamReader(ns);

            sw.AutoFlush = true;

            bool jatka = true;
            while (jatka)
            {
                DateTime nyt = DateTime.Now;

                if (ns.DataAvailable)
                {
                    // luetaan ja käsitellään komennot

                    string komento = sr.ReadLine();
                    string vastaus = "";
                    DateTime nyt2 = DateTime.Now;
                    // päivitetään aika
                    switch (komento)
                    {
                        case "TIME":
                            vastaus = DateTime.Now.ToString();
                            break;
                        case "NUMBER_OF_CLIENTS":
                            vastaus = "1"; // TODO
                            break;
                        case "QUIT":
                            vastaus = "lopetus";
                            jatka = false;
                            break;
                    }
                    sw.WriteLine(vastaus);
                    Console.WriteLine(vastaus);
                }
                else
                {
                    DateTime nyt2 = DateTime.Now;
                    TimeSpan erotus = nyt2 - nyt;
                    if (erotus.TotalSeconds > 60)
                        jatka = false;
                    else
                        Thread.Sleep(500);
                }
            }
            // suljetaan yhteydet
            sw.Close();
            sr.Close();
            ns.Close();
            client.Close();
        }
    }
}
