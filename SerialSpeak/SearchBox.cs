/************************************************************************
 * BIBLIOTHEQUES
 ************************************************************************/
using System;
using System.Windows.Forms;
using System.IO;

/************************************************************************
 * ESPACE DE NOM : SerialSpeak
 ************************************************************************/
namespace SerialSpeak
{
    /************************************************************************
    * CLASSE : SearchBox
    *************************************************************************/
    class SearchBox
    {
        //=======================================
        //======== Méthodes de recherche ========
        //=======================================

        //------------------------------------------------------
        //-------- Recherche de dossier dans un dossier --------
        //------------------------------------------------------
        public static void callSearch_folder( string fol )
        {
            DirectoryInfo di = new DirectoryInfo( @fol );
            try
            {
                //écriture dans la console de tous les dossiers trouvé dans à l'adresse indiquée
                foreach ( var dir in di.GetDirectories( ) )
                {
                    Console.WriteLine( dir.Name );
                }
            }
            catch( Exception ex )
            {
                MessageBox.Show( "ERREUR : " + ex.Message, "Erreur dans la procédure de recherche" );
            }

        }

        //--------------------------------------------------------
        //-------- Recherche d'un fichier dans un dossier --------
        //--------------------------------------------------------
        public static void callSearch_files( string fol )
        {
            DirectoryInfo di = new DirectoryInfo( @fol );
            try
            {
                foreach( var fi in di.GetFiles( ) )
                {
                    Console.WriteLine( fi.Name );
                }
            }
            catch( Exception ex )
            {
                MessageBox.Show( "ERREUR : " + ex.Message, "Erreur dans la procédure de recherche" );
            }
        }

    }   //close : "class SearchBox"
}       //close : "namespace SerialSpeak"

