/************************************************************************
 * BIBLIOTHEQUES
 ************************************************************************/
using System;
using System.Windows.Forms;
using System.IO.Ports;

/************************************************************************
 * ESPACE DE NOM : SerialSpeak
 ************************************************************************/
namespace SerialSpeak
{
    /************************************************************************
    * CLASSE : ControlWithTex
    ************************************************************************/
    class ControlWithText
    {
        //====================================
        //======== Variables globales ========
        //====================================
        static SerialPort serie = null;     //port série que l'on va utiliser
        static ConsoleKeyInfo charRead;     //pour récupération des informations relatives aux tpouches enfoncées
        static string previousCmnd = "";

        static string[] prgrmCmnd =
        {
            "end of line \t---- \tSortie du programme",
            "get serial \t---- \tObtention des noms des ports Serie existants",
            "open serial \t---- \tOuverture d'un port série et intreprétation en int des caractères reçus",
            "set serial \t---- \tOuverture d'un port Série et interprétation en String des caractères reçus",
            "close serial \t---- \tFermeture du port série ouvert",
            "open excel \t---- \tRecherche et ouverture d'un classeur Excel",
            "close excel \t---- \tFermeture du classeur Excel",
            "folder search \t---- \tRecherche d'un dossier dans un dossier",
            "file search \t---- \tRecherche d'un fichier dans un dossier",
            "test keys \t---- \tTest des touches du clavier",
            "help \t\t---- \tAffichage des commandes existantes du programme"
        };
        static string[] serialInfo = 
        {
            "Les caractèristiques du port série seront :",
            "\tBaud rate : \t9600",
            "\tParity : \tnone",
            "\tStop bits : \tOne",
            "\tData bits : \t8",
            "\tHand shake : \tnone"
        };

        //==============================
        //======== Constructeur ========
        //==============================

        public ControlWithText( )
        {
            //rien pour l'instant
        }

        //===========================================================================
        //======== Methodes d'interpretation des entrées du texte en console ========
        //===========================================================================

        //-----------------------------------------------------------------
        //-------- Fonction de décode destouches tapées au clavier --------
        //-----------------------------------------------------------------
        private static string acquireKeyboard( )
        {
            string resultLine = "";                //chaine de caractères à retourner en fin d'acquisition des caractères au clavier
            string[] command = new string[ 100 ];     //tableau de récupération des caractère que l'on rentre par le clavier
            int i = 0;                             //indice du tableau de caractère

            //on tient compte de la casse
            Console.TreatControlCAsInput = true;

            //boucle infinie dont on ne sort que pour retourner la chaine de caractère validée (avec la touche entrée)
            while( true )
            {
                charRead = Console.ReadKey( );   //acquisition d'une touche enfoncée

                //Si la touche enfoncée est la touche "Enter"
                if( charRead.Key == ConsoleKey.Enter )
                {
                    //on complète "commandLine" a partir du tableau de caractère "command[]"
                    for( int j = 0 ; j < i ; j++ )
                    {
                        resultLine += command[ j ];
                    }

                    previousCmnd = resultLine;
                    Console.Write( resultLine + "\n" );  //on "printf" commandLine avec un saut à la ligne

                    return resultLine; //on retourne à la fonction qui nous a appelé la valeur de la chaine "commandLine"
                }
                //Si la touche enfoncée est Echap on renvoi la valeur la chaine de caractère de sortie du programme
                else if( charRead.Key == ConsoleKey.Escape )
                {
                    return "end of line";
                }
                //Si la touche enfoncée est backspace, on revient en arrière de 1 dans le tableau de caractères...
                //...tant que le compteur du tableau est positif
                else if( ( charRead.Key == ConsoleKey.Backspace ) && 
                         ( i > 0 )                                  )
                {
                    i--;
                }
                //Si la touche enfoncée est la flèche vers le haut
                else if( charRead.Key == ConsoleKey.UpArrow )
                {
                    return previousCmnd;
                }
                //Gestion des touches qui ne sont pas Echap, Enter ou BackSpace
                else if( ( charRead.Key != ConsoleKey.Enter )    && 
                         ( charRead.Key != ConsoleKey.Backspace )  )
                {
                    command[ i ] = charRead.KeyChar.ToString( );
                    i++;
                }
            }
        }

        //---------------------------------------------------------------------------------
        //-------- interpretation de la chaine de caractère obtenue via le clavier --------
        //---------------------------------------------------------------------------------
        public static void controlThroughLines( )
        {
            string commandLine = "";    //chaine de caractère correspondant à la commande que l'on va interpreter

            Console.Write( "->" );
            commandLine = acquireKeyboard( ); //aquisition de la "commandLine"

            switch( commandLine )
            {
                //commande de sortie du programme
                case "end of line":
                    PortSerieUser.closeSerial( serie );         //cloture du port Serie ouvert
                    AppliExcel.closeExcel( );                   //fermeture de l'appli Excel
                    Console.WriteLine( "Time to say goodye" );  //on dit qu'il est temps de dire au revoir
                break;

                //recherche et affichage des nom des ports Série existants disponibles
                case "get serial":
                    Console.WriteLine( "Les Ports Serie trouvés sont :" );
                    Console.WriteLine( PortSerieUser.lookForSerialPort( ) );
                    controlThroughLines( );  //relance de l'acquisition d'une commande dans la console
                break;

                //permet d'afficher les valeurs en int reçue sur le port Serie
                case "open serial":
                {
                    foreach( string serInf in serialInfo )
                    {
                        Console.WriteLine( serInf );
                    }

                    //Acquisition du nom du port par la console
                    string nomDuPort = "";
                    Console.WriteLine( "Quel Port Serie initialiser?" );
                    nomDuPort = acquireKeyboard( );
                    //appel au fonctions de création du port dont on vient d'indiquer le nom
                    PortSerieUser.serHandle = sortieSerialSurConsole;
                    //initialisation du port dont le nom est indiqué sauf si l'on souhaite sortir (nomDuPort = "out")
                    if( nomDuPort != "out" )
                    {
                        serie = PortSerieUser.setSerialPort( nomDuPort );
                    }

                    controlThroughLines( );
                }
                break;

                //pour afficher les caractères reçu sur le port Série
                case "set serial":
                {
                    foreach( string serInf in serialInfo )
                    {
                        Console.WriteLine( serInf );
                    }
                    
                    //Acquisition du nom du port par la console
                    string nameOfPort = "";
                    Console.WriteLine( "Quel Port Serie initialiser?" );
                    nameOfPort = acquireKeyboard( );
                    //appel au fonctions de création du port dont on vient d'indiquer le nom
                    PortSerieUser.serHandle = verifSerialSurConsole;
                    //initialisation du port dont le nom est indiqué sauf si l'on souhaite sortir (nameOfPort = "out")
                    if( nameOfPort != "out" )
                    {
                        serie = PortSerieUser.setSerialPort( nameOfPort );
                    }

                    controlThroughLines( );
                }
                break;

                //cloture du port Série
                case "close serial":
                    PortSerieUser.closeSerial( serie );
                    controlThroughLines( );
                break;

                //écriture dans le port série
                case "write serial":
                    //Acquisition d'une chaine de caractère à envoyer par la console
                    string chaineEnvoyee = "";
                    Console.WriteLine( "Que vas-t-on ecrire?" );
                    chaineEnvoyee = acquireKeyboard( ) + '\n';
                    //envoi de la chaine sur le port serie
                    PortSerieUser.sendToSerial( chaineEnvoyee, serie );
                    controlThroughLines( );
                break;

                //ouverture d'un classeur excel
                case "open excel":
                {
                    string nomDuClasseur = "";
                    string chemin = "";
                    string userAnswer = "";

                lookForFiles:  //balise de retour
                    Console.WriteLine( "Quel est le nom du fichier à ouvrir?" );
                    chemin = acquireKeyboard( );             //acquisition de l'adresse du fichier à ouvrir
                    SearchBox.callSearch_files( chemin );     //recherche des docs à l'adresse indiquée
                    Console.WriteLine( "Cela convient-il ?" );
                    userAnswer = acquireKeyboard( );         //acquisition de la réponse de l'utilisateur
                    if( userAnswer != "oui" )
                    {
                        goto lookForFiles; //si la réponse est différente de oui (on ne trouve pas ce...
                                           //...que l'on souhaite) on renvoie à la balise de retour
                    }
                    else if( userAnswer == "out" )
                    {
                        controlThroughLines( );
                    }

                    Console.WriteLine( "Entrer le nom du fichier" );
                    nomDuClasseur = acquireKeyboard( );          //acquisition du nom du fichier à ouvrir
                    AppliExcel.openExcelFile( chemin + "/" + nomDuClasseur );   //ouverture du fichier
                    controlThroughLines( );
                }
                break;

                //cloture de l'appli Excel et retour à l'acquisition d'une commande dans la console
                case "close excel":
                    AppliExcel.closeExcel( );
                    controlThroughLines( );
                break;

                //recherche dans les dossiers
                case "folder search":
                    string diskName = acquireKeyboard( );
                    SearchBox.callSearch_folder( diskName );
                    controlThroughLines( );
                break;

                //recherche de fichier
                case "file search":
                    string filName = acquireKeyboard( );
                    SearchBox.callSearch_files( filName );
                    controlThroughLines( );
                break;

                //test des touches du clavier
                case "test keys":
                    testKey( );
                    controlThroughLines( );
                break;

                //affichage de la liste des commandes
                case "help":
                {
                    Console.WriteLine( "Voici la liste des commandes du programme :" );
                    foreach( string cmnd in prgrmCmnd )
                    {
                        Console.WriteLine( cmnd );
                    }

                    controlThroughLines( );
                }
                break;

                //default case : si la commande est inconnue (ou contient des fautes de frappe) on retourne au début
                default:
                    Console.WriteLine( "Commande inconnue !" );
                    controlThroughLines( );
                break;
            }
        }

        //==========================================
        //======== Méthodes complémentaires ========
        //==========================================

        //--------------------------------------------------------------------------------------------------------------------
        //-------- Serial Event que l'on trigger à chaque reception de caractères - Interprétation en valeur d'entier --------
        //--------------------------------------------------------------------------------------------------------------------
        private static void sortieSerialSurConsole( )
        {
            int chaineSortie;

            try
            {
                chaineSortie = serie.ReadChar( );          //actualisation de la valeur de chaineSortie par le caractère reçu
                fillExcelFile( chaineSortie.ToString( ) );   //conversion de chaineSortie de int vers String...
                                                             //...et écriture dans le tableur Excel de cette valeur
                Console.Write( chaineSortie );              //écriture de la valeur de chaineSortie en console
            }
            catch( Exception ex )
            {
                MessageBox.Show( "ERREUR : " + ex.Message, "Erreur sur le port serie" );
            }
        }

        //------------------------------------------------------------------------------------------------------------------------
        //-------- Serial Event que l'on trigger à chaque reception de caractères - Interprétation en chaine de caractère --------
        //------------------------------------------------------------------------------------------------------------------------
        private static void verifSerialSurConsole( )
        {
            string chaineSortie = "";

            try
            {
                chaineSortie = serie.ReadExisting( );    //actualisation de la valeur de chaineSortie par le caractère reçu
                fillExcelFile( chaineSortie );            //écriture de ce caractère dans le tableur Excel
                Console.Write( chaineSortie );            //écriture de ce caractère dans la console
            }
            catch( Exception ex )
            {
                MessageBox.Show( "ERREUR : " + ex.Message, "Erreur sur le port serie" );
            }
        }

        //-----------------------------------------------------------------------------
        //-------- Gestion de la façon dont on va écrire dans un fichier Excel --------
        //-----------------------------------------------------------------------------
        private static void fillExcelFile( string inputExcel )
        {
            if( AppliExcel.classeur != null )
            {
                try
                {
                    if( AppliExcel.lineNumber == 1 )
                    {
                        AppliExcel.writeInWorksheet( "N°" );     //on imprime le numéro de la ligne dans la 1ère colonne
                        AppliExcel.columnNumber = 2;             //on change de colonne
                        AppliExcel.writeInWorksheet( "INPUT" );  //on imprimme le contenu du message
                        //on descend d'une ligne et onn revient colonne 1
                        AppliExcel.lineNumber++;
                        AppliExcel.columnNumber = 1;
                    }
                    else
                    {
                        AppliExcel.writeInWorksheet( AppliExcel.lineNumber.ToString( ) );  //on imprime le numéro de la ligne dans la 1ère colonne
                        AppliExcel.columnNumber = 2;                                       //on change de colonne
                        AppliExcel.writeInWorksheet( inputExcel );                         //on imprimme le contenu du message
                        //on descend d'une ligne et onn revient colonne 1
                        AppliExcel.lineNumber++;
                        AppliExcel.columnNumber = 1;
                    }
                }
                catch( Exception ex )
                {
                    MessageBox.Show( "ERREUR : " + ex.Message, "Erreur fichier Excel" );
                }
            }
        }

        //-----------------------------------------------------------------
        //-------- Fonction de test des touches entrées au clavier --------
        //-----------------------------------------------------------------
        private static void testKey( )
        {
            ConsoleKeyInfo cki;
            // Prevent example from ending if CTL+C is pressed.
            Console.TreatControlCAsInput = true;

            Console.WriteLine( "Press any combination of CTL, ALT, and SHIFT, and a console key." );
            Console.WriteLine( "Press the Escape (Esc) key to quit: \n" );
            do
            {
                cki = Console.ReadKey( );
                Console.Write( " --- You pressed " );
                if (( cki.Modifiers & ConsoleModifiers.Alt ) != 0)
                {
                    Console.Write( "ALT+" );
                }

                if( ( cki.Modifiers & ConsoleModifiers.Shift ) != 0 )
                {
                    Console.Write( "SHIFT+" );
                }

                if( ( cki.Modifiers & ConsoleModifiers.Control ) != 0 )
                {
                    Console.Write( "CTL+" );
                }

                Console.WriteLine( cki.Key.ToString( ) );
            } while( cki.Key != ConsoleKey.Escape ); //sortie du programme de test si appuis sur la touche Echap
        }

    }   //close : "class ControlWithTex"
}       //close : "namespace SerialSpeak"
