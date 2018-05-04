/************************************************************************
 * BIBLIOTHEQUES
 ************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;
using System.Threading;

/************************************************************************
 * ESPACE DE NOM : checkSerial
 ************************************************************************/
namespace checkSerial
{
    /************************************************************************
     * CLASSE : Behavior
     ************************************************************************/
    public class Behavior
    {
        //====================================
        //======== Variables globales ========
        //====================================

        private Thread threadSafeCall = null;               //définition du Thread que l'on va utiliser pour les appel de méthode Thread-Safe
        public delegate void delThread();                   //définition d'un type d'objet Delegate permettant de déclarer des Delegate-Handle...
                                                            //...qui pointent sur les méthodes que l'on souhaite appeler en mode Thread-Safe
        delThread delSafeCall = new delThread(defCall);     //definition d'un Delegate-Handle du type "delTtread" défini précédemment...
                                                            //...que l'on fait pointer sur une méthode par défaut (defCall = default call)

        PortSerieUser ser = null;                           //déclaration d'un objet "PortSerieUser" qui est le port série que l'on va utiliser
        //TimerOut tim = null;                                //déclaration d'un objet "TimerOut pour" qui va généré l'évenement de sortie du programme
        //TimerOut zeroChaineRetour = null;                   //timer utilisé pouréffacer le texte à l'écran

        static string chaineRetour = "";                    //chaine de caractère de retour lors des appel de fonction inter-Thread

        //==============================
        //======== Constructeur ========
        //==============================
        public Behavior()
        {
            //initialisation des composantes faisant appel à un thread parallèle pour les "safe call"
            this.threadSafeCall = new Thread(new ThreadStart(safeCall));
            this.threadSafeCall.Start();

            //mise en place du timer et du port Série
            //tim = new TimerOut(1000, timAct, TimerOut.TEN_TICK);  //paramètre 1 : periode Timer; paramètre 2 : méthode à exécuter lors du timeOut
                                                                    //paramètre 3 : mode d'exécution de la méthode de sortie (après 1 ou 10 périodes)
            ser = new PortSerieUser(updateText);    //paramètre : méthode à exécuter sur reception de mesure sur le port Série
            //zeroChaineRetour = new TimerOut(1000, zeroText, TimerOut.ONE_TICK);  //timer pour effacer le texte à l'écran
        }

        //===================================================================
        //======== Methode des événements liés aux objets graphiques ========
        //===================================================================

        //-------------------------------------------------------------------------
        //-------- Comportement de la fenètre sur appui du bouton "fermer" --------
        //-------------------------------------------------------------------------
        private void onCloseBehavior(object sender, FormClosedEventArgs e)
        {
            ser.closeSerial();
        }

        //----------------------------------------------------------------------
        //-------- Méthode appelée à chaque évènement sur le port Série --------
        //----------------------------------------------------------------------
        private void updateText()
        {
            //tim.zeroTimeOut();  //remise à "false" du "nothingHappened" du timer "tim"

            chaineRetour += ser.serie.ReadExisting();   //lecture du port Série et mise des caractères dans la chaine tampon

            //éciture Thread-Safe du texte à l'écran (passage par un autre thread pour l'appel à lobjet "dataViewer")
            delSafeCall = setScreenText;
            safeCall();

            Thread.Sleep(1050);
            zeroText();
        }

        //---------------------------------------------
        //-------- Ecriture du texte à l'écran --------
        //---------------------------------------------
        private void setScreenText()
        {
            //dataViewer.Text = chaineRetour;
        }

        //===========================================================
        //======== Méthodes liées au comportement des Timers ========
        //===========================================================

        //----------------------------------------------------------------------------------------------
        //-------- Evènement "triggered" après 10 periodes du compteur pour sortie du programme --------
        //----------------------------------------------------------------------------------------------
        private void timAct()
        {
            delSafeCall = ser.closeSerial;  //attribution du Delegate-Handle à la méthode "closeSerial()" de l'objet "ser"
            safeCall();                     //appel Thread-Safe de "closeSerial()" issu de la classe PortSerieUser
            //delSafeCall = this.Close;       //attribution du Delegate-Handle pour Safe-Call à la méthode "this.Close()"
            safeCall();                     //appel Thread-Safe de "this.Close()"
        }

        //---------------------------------------------------------------------
        //-------- Methode de remise à 0 de chaineRetour régulièrement --------
        //---------------------------------------------------------------------
        private void zeroText()
        {
            chaineRetour = "";
            delSafeCall = setScreenText;
            safeCall();
        }

        //=====================================================================
        //======== Fonctions assurant les communications inter-threads ========
        //=====================================================================

        //--------------------------------------------
        //-------- Appel au thread par dédaut --------
        //--------------------------------------------
        private static void defCall()
        {
            //appel au Thread par défaut :  rien ne se passe
        }

        //-----------------------------------------------------------------
        //-------- Description du comportement du Thread parallèle --------
        //-----------------------------------------------------------------
        private void safeCall()
        {
        /*
            try     //dans les cas habituels
            {
                if (this.InvokeRequired)    //si l'on requier un Invoke sur l'objet actuel si la méthode appelée se situe sur un thread différent
                {
                    delThread sfcll = new delThread(safeCall);    //on crée un nouveau Delegate-Handle qui pointe sur la fonction actuelle
                    this.Invoke(sfcll);                           //on effectue un Invoke du Delegate-Handle créé juste avant
                }
                else    //tous les cas où l'objet et la méthode appelée sont sur le meme Thread (pas besoin de Invoke)  
                {
                    this.delSafeCall();     //on effectue l'action/exécute la méthode à laquelle se réfère le Delegate-Handle delSafeCall...
                    //...que l'on aura pris soin d'attribuer avant l'appel à la méthode "safeCall()"
                }
            }
            catch (Exception ex)    //dans les cas d'exception/erreur => information de l'utilisateur
            {
                MessageBox.Show("ERREUR : " + ex.Message);  //=>Sortie du message d'erreur dans une TextBox
            }
        */ 
        }

        //----------------------------------------------------------------------------------------
        //-------- Fonction de décode des commandes envoyée par ligne de texte en console --------
        //----------------------------------------------------------------------------------------
        /*
        public static void controlThroughLines()
        {
            ConsoleKeyInfo charRead;    //pour récupération des informations relatives aux tpouches enfoncées
            string commandLine = "";    //chiane de caractère qui servira à transmettre les commandes
            
            //boucle infinie dont on ne sort que pour acceder à d'autres fonctions du programme
            while (true)
            {
                charRead = Console.ReadKey();   //acquisition d'une touche enfoncée
                if (charRead.Key == ConsoleKey.Escape) goto end;    //si c'est Echap on saute hors de la boucle
                if (charRead.Key == ConsoleKey.Enter)               //comportement si la touche est Entrée
                {
                    Console.Write(commandLine + "\n");  //on "printf" commandLine avec un saut à la ligne

                    switch (commandLine)
                    {
                        case "END OF LINE" :
                            Console.Write("time to say goodye\n");  //on dit qu'il est temps de dire au revoir
                            goto end;                               //on saute hors de la boucle infinie
                            //break;
                        
                        case "GET SERIAL" :
                            string portString = "";
                            string[] ports = SerialPort.GetPortNames();
                            foreach (string port in ports)
                            {
                            portString = portString + port + "\n";
                            }
                            Console.Write(portString+"\n");
                            break;

                        case "SET PORT" :
                            string nomDuPort = "";
                            while (charRead.Key != ConsoleKey.Enter)
                            {
                                nomDuPort += charRead.Key.ToString();
                            }
                            //PortSerieUser serial = new PortSerieUser();
                            break;
                        
                        default :
                            controlThroughLines();
                            break;
                    }
                    commandLine = "";   //remise à "zéro" de la chaine de caractères après l'avoir "printf"
                }

                //Si la touche enfoncée est "espace" on ajoute un espace dans commandLine
                if (charRead.Key == ConsoleKey.Spacebar)
                {
                    commandLine += " ";
                }
                //Si la touche enfoncée n'est pas "Entrée" ni "Backspace" on incrémente...
                //... la chaine "commandLine" du caractère correspondant à la touche
                else if (charRead.Key != ConsoleKey.Enter && charRead.Key != ConsoleKey.Backspace)
                {
                    commandLine += charRead.Key.ToString();
                }
            }
        //label auquel on se renvoie pour sortir de la boucle infinie
        end:
            Console.Write("Goodbye\n"); //on dit "au revoir"
        }
        */ 

    }   //close : "class Behavior"
}       //close : "namespace checkSerial"
