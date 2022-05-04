import React, { PureComponent } from "react";
import 'AnnouncementBlock.scss';
import '../../styles/styles.scss';

export default class AnnouncementBlock extends PureComponent {

    getTextOfAnnouncement = () =>{
        //Sem zadaj text oznamu
        var text = [
            "Od dňa 02.05.2022 sme úspešne implementovaly nový spôsob prihlasovania. Od tohto dňa využívame celouniverzitné prihlasovanie "
            +"do systému vďaka čomu je možné využívať aplikáciu všetkými študentmi Univerzity v Žiline.",
            "Pre užívateľov, ktorí už boli registrované netreba nič meniť, iba sa prihlásiť s použitím celouniverzitného hesla.",
            "V prípade akýchkoľvek výhrad alebo pripomienok nás neváhajte kontaktovať prostredníctvom formuláru Napíšte nám."
            ];
        return text;
    }

    doesAnnouncementTextExist = () =>{
        //Ak text nieje zadany nastav false, inak nastav true
        return true; 
    }

    render() {    
        if(this.doesAnnouncementTextExist)
            return (
                <div class="AnnouncementDiv">
                    <div class="InsideDiv">
                        
                    </div>
                </div>
            );
        else
            return ;
    }
}