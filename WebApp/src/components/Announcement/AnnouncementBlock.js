import React from 'react';
import './AnnouncementBlock.scss';
import '../../styles/styles.scss';

    const AnnouncementBlock = () => {  
        //Zmen na hodnotu false - ak sa nema oznam zobrazovat
        var show = true;
        //+Zmen text oznamu v pripade potreby  
        if(show){
            return (
                <div className="AnnouncementDiv">
                    <div className="InsideDiv">
                        <p className="AnnouncementText">
                        Od dňa 02.05.2022 sme úspešne implementovaly nový spôsob prihlasovania.
                        Od tohto dňa využívame celouniverzitné prihlasovanie do systému vďaka 
                        čomu je možné využívať aplikáciu všetkými študentmi Univerzity v Žiline.
                         Pre užívateľov, ktorí už boli registrované netreba nič meniť, iba sa 
                         prihlásiť s použitím celouniverzitného hesla. V prípade akýchkoľvek 
                         výhrad alebo pripomienok nás neváhajte kontaktovať prostredníctvom 
                         formuláru Napíšte nám.
                        </p>
                    </div>
                </div>
            )
        }else{
            return (<nothing></nothing>);
        }
    };


export default AnnouncementBlock;