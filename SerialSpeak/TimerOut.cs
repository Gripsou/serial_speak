/************************************************************************
 * BIBLIOTHEQUES
 ************************************************************************/
using System;

/************************************************************************
 * ESPACE DE NOM : checkSerial
 ************************************************************************/
namespace checkSerial
{
    /************************************************************************
    * CLASSE : TimerOut
    ************************************************************************/
    class TimerOut
    {
        //===============================================
        //======== Variable globale de la classe ========
        //===============================================
        private int timeOutCpt = 0;             //variable d'incrémentattion pour compteur TimeOut
        private bool nothingHappened = false;   //état de s'il s'est passé quelsque chose ou non
                                                //true -> rien ne s'est passé ; false -> il s'est passé quelque chose

        private static System.Timers.Timer aTimer;  //définition du timer sur lequel s'appuie notre objet

        //Définition des Delegate-Handles qui permettent d'intervenir sur...
        //...les méthodes des autre classe utilisées dans le programme
        public delegate void timerAction();
        timerAction t_handle = new timerAction(defAct); //init du handle pointant sur l'action par défaut

        //constantes qui permettent d'indiquer le mode d'action du compteur
        public const int TEN_TICK = 1;  //action tout les 10 coup d'horloge avec remise à zéro du du compte de période  
        public const int ONE_TICK = 0;  //action toute les fin de période de comptage

        //==============================
        //======== Constructeur ========
        //==============================
        public TimerOut( double per, timerAction t_act, int timerMode )
        {
            t_handle = t_act;           //attribution de la fonction à déclencher via des Delegate-Handle
            setTimer( per, timerMode );    //appel de la fonction de config
        }

        //==================================================================
        //======== Methodes relatives à la mise en place d'un timer ========
        //==================================================================

        //-----------------------------------
        //-------- Création du timer --------
        //-----------------------------------
        private void setTimer( double periode, int mode )
        {
            // Create a timer with a "periode" interval.
            aTimer = new System.Timers.Timer(periode);

            aTimer.Enabled = true;                                                  //Enable le timer
            aTimer.Interval = periode;                                              //attribution au timer de sa période de comptage

            //attribution de l'évènement de fin de période de comptage selon le mode de comptage
            if( mode == TEN_TICK )   //au bout de 10 périodes avec remise à 0 du compte de période possible
            {
                aTimer.Elapsed += new System.Timers.ElapsedEventHandler( timerBehavior );
            }
            else if( mode == ONE_TICK )  //chaque période
            {
                aTimer.Elapsed += new System.Timers.ElapsedEventHandler( timerEveryTickBehavior );
            }

        }

        //=============================================================
        //======== Methodes relatives au comportement du timer ========
        //=============================================================

        //---------------------------------------------------------------------------
        //-------- Comportement du timer à chaque fin de periode de comptage --------
        //---------------------------------------------------------------------------
        private void timerBehavior(object sender, EventArgs e)
        {
            if( true == nothingHappened )    //s'il ne s'est rien passé on effectue la séquence suivante
            {
                if( timeOutCpt >= 10 )   //s'il ne s'est rien passé depuis 10 periodes ou plus on ferme tout
                {
                    t_handle( );     //appel de la méthode sur laquelle pointe t_handle
                }

                timeOutCpt++;           //incrémentation du compteur
            }
            else    //s'il s'est passé quelque chose durant le cycle précédent, on remet le competeur de timeOut à l'état initial
            {
                nothingHappened = true;     //on dit qu'il ne se passe rien, s'il se passe quelque chose...
                //...d'ici la fin de cylcle on aura "nothingHappened = false"
                timeOutCpt = 0;             //on remet à 0 le compteur de timeout
            }
        }

        //-------------------------------------------------------------------------------------------------------
        //-------- Comportement d'un timer qui répète la même action à chaque fin de periode de comptage --------
        //-------------------------------------------------------------------------------------------------------
        private void timerEveryTickBehavior( object sender, EventArgs e )
        {
            t_handle( );     //appel de la méthode sur laquelle pointe t_handle
        }

        //----------------------------------------------------
        //-------- Comportement d'un timer par défaut --------
        //----------------------------------------------------
        private static void defAct( )
        {
            //action par défaut : ne rien faire
        }

        //--------------------------------------------------------------------------------------------
        //-------- Methode de remise à "false" de "nothingHappened" par les objets extérieurs --------
        //--------------------------------------------------------------------------------------------
        public void zeroTimeOut( )
        {
            nothingHappened = false;
        }

    }   //close : "class TimerOut"
}       //close : "namespace checkSerial"
