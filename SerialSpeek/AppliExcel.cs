/************************************************************************
 * BIBLIOTHEQUES
 ************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.IO.Ports;
//Package pour pouvoir utiliser Excel :
using Excel = Microsoft.Office.Interop.Excel;
using Microsoft.Office.Tools.Excel;
using Microsoft.Office.Core;

/************************************************************************
 * ESPACE DE NOM : SerialSpeak
 ************************************************************************/
namespace SerialSpeak
{
    /************************************************************************
     * CLASSE : AppliExcel
     ************************************************************************/
    class AppliExcel
    {
        //====================================
        //======== Variables globales ========
        //====================================
        static Microsoft.Office.Interop.Excel.Application appli;   //definition de l'appli
        static public Microsoft.Office.Interop.Excel._Workbook classeur;  //definition d'un objet classeur pour faciliter sa manipulation
        static Microsoft.Office.Interop.Excel._Worksheet feuille;  //definition d'un objet feuille pour faciliter sa manipulation

        static object M = System.Reflection.Missing.Value;     //objet M pour "missing value"
        static object FileName = "D:/";    //Filename est l'objet représentant le nom de notre fichier

        static string tempString = "Ceci est une chaine tampon";    //chaine permettant d'obtenir ce qu'il se passe dans les methodes statique
        string prevTempString = tempString;                         //chaine de comparaison avec tempString

        static public int lineNumber = 1, columnNumber = 1;    //indice des lignes et colonnes du tableau Excel

        //==============================
        //======== Constructeur ========
        //==============================
        public AppliExcel()
        {
            //rien pour le moment
        }

        //==========================================================================
        //======== Fonctions réalisant la lecture/écriture du fichier Excel ========
        //==========================================================================

        //-------------------------------------------------------
        //-------- Ouverture d'un fichier Excel spécifié --------
        //-------------------------------------------------------
        public static void openExcelFile(string givenFileName)
        {
            String nomFichier = convertThing(FileName);
            try
            {
                //definition de l'application avec laquelle nous interragissons
                appli = new Microsoft.Office.Interop.Excel.Application();
                appli.Visible = true;  //on veut afficher à l'écran la fenètre Excel

                //conversion de FileName d'Oject vers String
                nomFichier = givenFileName + ".xls";

                //création du classeur
                // WARNING : IL FAUT QUE LE FICHIER EXISTE => ECRIRE LA PARTIE CREATION D'UN NOUVEAU FICHIER
                classeur = (Microsoft.Office.Interop.Excel._Workbook)(appli.Workbooks.Open(nomFichier, M, M, M, M, M, M, M, M, M, M, M, M, M, M));
                //if (classeur == null) classeur = appli.Workbooks.Add();
                //activer la feuille
                feuille = (Microsoft.Office.Interop.Excel._Worksheet)classeur.ActiveSheet;
            }
            catch (Exception ex)
            {
                MessageBox.Show("ERREUR : " + ex.Message);
            }
        }

        //--------------------------------------------------
        //-------- On ferme le fichier Excel ouvert --------
        //--------------------------------------------------
        //WARNING : NE PAS OUBLIER CETTE FONCTION SINON ON NE FERME JAMAIS LE CLASSEUR EXCEL
        public static void closeExcel()
        {
            if (classeur != null || feuille != null || appli != null)
            {
                try
                {
                    //reset des numeros de ligne et de colonne
                    lineNumber = 1;
                    columnNumber = 1;

                    classeur.Close(true, M, M); //fermeture du classeur avec Auto-Save
                    feuille = null;
                    classeur = null;
                    appli.Quit();               //Fermeture de Excel
                    appli = null;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("ERREUR : " + ex.Message);
                }
            }
        }
        
        //------------------------------------------------
        //-------- Ecriture dans un fichier Excel --------
        //------------------------------------------------
        public static void writeInWorksheet(string chaineAEcrire)
        {
            try
            {
                feuille.Cells[lineNumber, columnNumber] = chaineAEcrire;
            }
            catch (Exception ex)
            {
                MessageBox.Show("ERREUR : " + ex.Message, "Problème d'écriture fichier Excel");
            }
        }


        //=================================================================
        //======== FONCTIONS COMPLEMENTAIRES POUR LE TABLEUR EXCEL ========
        //=================================================================

        //-----------------------------------------------------------
        //-------- Fonctions pour les tests divers et variés --------
        //-----------------------------------------------------------
        public static void testThing(object thingToTest)
        {
            MessageBox.Show(thingToTest.ToString(), "TEST MESSAGE");
        }

        //------------------------------------------------------------
        //-------- Conversion du contenu d'un objet en String --------
        //------------------------------------------------------------
        private static String convertThing(object thingToTest)
        {
            return thingToTest.ToString();
        }

        //------------------------------------------------------------
        //-------- Conversion d'une String en tableau de Char --------
        //------------------------------------------------------------
        private byte[] convertStringToChar(String stringToConvert)
        {
            byte[] valConvertie = { 0 };      //buffer d'un seul Octet
            char[] charTab = new char[3];   //tableau de caractères pour conversion

            charTab = stringToConvert.ToCharArray();    //conversion de la "String" entrante en tableau de "char"

            //conversion des valeurs du tableau sur un seul octet (valeurs de 0 à 255)
            valConvertie[0] = (byte)((charTab[0] - 0x30) * 0x64 + (charTab[1] - 0x30) * 0xA + charTab[2] - 0x30);

            return valConvertie;
        }
     
    }   //close : "class AppliExcel"
}       //close : "namespace SerialSpeak"
