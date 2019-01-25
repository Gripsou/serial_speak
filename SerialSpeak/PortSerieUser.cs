/************************************************************************
 * BIBLIOTHEQUES
 ************************************************************************/
using System;
using System.Windows.Forms;
using System.IO.Ports;
using System.Threading;

/************************************************************************
 * ESPACE DE NOM : SerialSpeak
 ************************************************************************/
namespace SerialSpeak
{
    /************************************************************************
     * CLASSE : PortSerieUser
     ************************************************************************/
    class PortSerieUser
    {
        //===============================================
        //======== Variable globale de la classe ========
        //===============================================
        public SerialPort serie = null;    //définition d'un port Série sur lequel s'appuie notre objet

        //définition des Delegate-Handles pour cette classe du programme
        public delegate void serialAction( );
        public static serialAction serHandle = new serialAction( defAct ); //init du handle pointant sur l'action par défaut

        //==============================
        //======== Constructeur ========
        //==============================
        public PortSerieUser( string givenPortName, serialAction serAct )
        {
            serHandle = serAct;         //attribution au Delegate-Handle de la methode dont on a passé le pointeur en argument
            serie = setSerialPort( givenPortName );    //mise en place du port Série
        }

        //==============================================================
        //======== Méthodes liées au comportement du port Série ========
        //==============================================================

        //------------------------------------------
        //-------- Création d'un Port Série --------
        //------------------------------------------
        public static SerialPort setSerialPort( string givenPortName )
        {
            try
            {
                SerialPort mySerial = new SerialPort( givenPortName );    //nouvelle instance d'objet SerialPort
                                                                          //...avec le nom passé en argument
                configureSerialPort( mySerial );      //appel à la fonction permettand de configurer le port nouvellement créé
                return mySerial;
            }
            catch( Exception ex )
            {
                MessageBox.Show( "ERREUR : " + ex.Message, "Erreur à l'ouverture du Port Série" );
                return null;
            }
        }

        //------------------------------------------------------
        //-------- Recherche du port Série et ouverture --------
        //------------------------------------------------------
        public static String lookForSerialPort( )
        {
            string[] ports = SerialPort.GetPortNames( ); //méthode permétant de retourner une liste de...
                                                         //...String contenant les noms des ports Série disponibles
            string portString = "";     //chaine de caractère permettant le retour de tous les ports serie visibles en console

            if (ports != null)   //s'il existe des ports Série (chaine de String retournée non nulle)
            {
                //on converti le tableau "ports" en une chaine de caractère unique avec saut de ligne entre chaque nom de port
                foreach( string port in ports )
                {
                    portString = portString + port + "\n";
                }

                return portString;     //on retourne les noms des ports série qui ont été détectés
            }
            else    //si le port n'est pas ouvert 
            {
                MessageBox.Show( "Port Série indisponible" );    //on l'indique à l'utilisateur par une boite de message
                return null;
            }
        }

        private static void configureSerialPort( SerialPort portToConfigure )
        {
            //configuration du port Série
            portToConfigure.BaudRate = 9600;
            portToConfigure.Parity = Parity.None;
            portToConfigure.StopBits = StopBits.One;
            portToConfigure.DataBits = 8;
            portToConfigure.Handshake = Handshake.None;

            //ajout d'un DataReceiveHandler à l'évènement de réception des données
            portToConfigure.DataReceived += new SerialDataReceivedEventHandler( dataReceivedHandler );

            portToConfigure.Open( ); //ouverture du port série
        }

        //---------------------------------------------------------------
        //-------- Methode pour une cloture "safe" du Port Série --------
        //---------------------------------------------------------------
        public static void closeSerial( SerialPort portToClose )
        {
            if( portToClose != null )  //s'il existe un port Série on effectue sa cloture
            {
                try     //comportement en temps normal : cloture
                {
                    portToClose.Close( );
                }
                catch( Exception ex )   //si pour une quelconque raison le port a été ouvert...
                                        //...et l'objet existe mais le hardware absent
                {
                    MessageBox.Show( "ERREUR : " + ex.Message, "Erreur à la fermeture du port Série" );
                }
            }
        }

        //------------------------------------------------------------------
        //-------- Envoit des valeurs en argument sur le port Série --------
        //------------------------------------------------------------------
        public static void sendToSerial( string toSerial, SerialPort targetPort )
        {
            byte[] val = { 0 };

            try
            {
                targetPort.Write( toSerial );
                Thread.Sleep( 15 );
            }
            catch( Exception ex )
            {
                MessageBox.Show( "ERREUR : " + ex.Message, "Erreur envoi de valeur sur Serial" );
            }
        }

        //--------------------------------------------------------------------------------------
        //-------- Evènement "triggered" à chaque réception de donnée sur le Port Série --------
        //--------------------------------------------------------------------------------------
        private static void dataReceivedHandler( object sender, System.IO.Ports.SerialDataReceivedEventArgs e )
        {
            serHandle( );    //appel à la méthode passée en argument à la création de l'objet
            //dataViewer.Text = portSerie.ReadExisting();
        }


        //--------------------------------------------
        //-------- Delegate Handle par défaut --------
        //--------------------------------------------
        private static void defAct( )
        {
            //action par défaut : ne rien faire
        }

    }   //close : "class PortSerieUser"
}       //close : "namespace SerialSpeek"
