/************************************************************************
 * BIBLIOTHEQUES
 ************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.IO.Ports;

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
        static string[] Cmnds = {"end of line \t---- \tSortie du programme",
                                        "get serial \t---- \tObtention des noms des ports Serie existants",
                                        "open serial \t---- \tOuverture d'un port série et intreprétation en int des caractères reçus",
                                        "set serial \t---- \tOuverture d'un port Série et interprétation en String des caractères reçus",
                                        "close serial \t---- \tFermeture du port série ouvert",
                                        "open excel \t---- \tRecherche et ouverture d'un classeur Excel",
                                        "close excel \t---- \tFermeture du classeur Excel",
                                        "folder search \t---- \tRecherche d'un dossier dans un dossier",
                                        "file search \t---- \tRecherche d'un fichier dans un dossier",
                                        "test keys \t---- \tTest des touches du clavier",
                                        "help \t\t---- \tAffichage des commandes existantes du programme"};


        //===============================
        //======== Fonction Main ========
        //===============================
        static void Main(string[] args)
        {
            Console.Write("*********************\n**** SerialSpeak ****\n*********************\n\n");
            Console.Write("Program written by Vincent RICCHI\nFree of rights\n2015\n\n");
            Console.WriteLine("Voici la liste des commandes du programme :");
            foreach (string cmnd in Cmnds)
            {
                Console.WriteLine(cmnd);
            }
            Console.Write("Bonjour!\n");                    //Petit mot de salutation
            ControlWithText.controlThroughLines();              //renvoi à la fonction permettant le controle...
                                                                //...du programme par ligne de texte
        }
    }   //close : "class Program"
}       //close : "namespace SerialSpeak"
