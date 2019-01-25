/************************************************************************
 * BIBLIOTHEQUES
 ************************************************************************/
using System;

/************************************************************************
 * ESPACE DE NOM : SerialSpeak
 ************************************************************************/
namespace SerialSpeak
{
    /************************************************************************
    * CLASSE : Program
    *************************************************************************/
    class Program
    {
        //====================================
        //======== Variables globales ========
        //====================================
        static string[] Cmnds = 
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

        static string[] salutations =
        {
            "*********************\n",
            "**** SerialSpeak ****\n",
            "*********************\n\n",
            "Program written by Vincent RICCHI\n",
            "Published under Apache License 2.0\n",
            "2015, 2019\n\n"
        };

        //===============================
        //======== Fonction Main ========
        //===============================
        static void Main( string[] args )
        {


            foreach( string line in salutations )
            {
                Console.Write( line );
            }

            Console.WriteLine( "Voici la liste des commandes du programme :" );
            foreach ( string cmnd in Cmnds )
            {
                Console.WriteLine(cmnd);
            }

            ControlWithText.controlThroughLines( );  //renvoi à la fonction permettant le controle...
                                                     //...du programme par ligne de texte
        }
    }   //close : "class Program"
}       //close : "namespace SerialSpeak"
