import React, { Component } from 'react';
import { connect } from 'react-redux';
import './PrivacyPolicyPage.css';

class PrivacyPolicyPage extends Component {

  render() {
    return (

        <html lang="en">
          <head>
            <meta charSet="UTF-8" />
            <meta name="viewport" content="width=device-width, initial-scale=1.0" />
            <meta httpEquiv="X-UA-Compatible" content="ie=edge" />
            <link
              rel="stylesheet"
              href="https://use.fontawesome.com/releases/v5.7.2/css/all.css"
              integrity="sha384-fnmOCqbTlWIlj8LyTjo7mOUStjsKC4pOpQbqyi7RrhN7udi9RwhKkMHpvLbHG9Sr"
              crossOrigin="anonymous"
            />
            <title>PDF Viewer</title>
          </head>
          <body>
            <div className="top-bar">
              <div className="top-bar-content">
                <h1>Zmluvné podmienky a Zásady ochrany osobných údajov</h1>
                {/*
                <button className="btn" id="prev-page">
                  <i className="fas fa-arrow-circle-left"></i> Predošlá strana
                </button>
                <button className="btn" id="next-page">
                  Nasledujúca strana <i className="fas fa-arrow-circle-right"></i>
                </button>
                <span className="page-info">
                  Strana <span id="page-num"></span> z <span id="page-count"></span>
                </span>
                */}
                
              </div>
            </div>

            <div className="pdf-container">
              <div className="pdf-context">

                <div className="cls_002"><span className="cls_002">Spoločenstvo študentov a učiteľov podieľajúcich sa na vytvorení webovej aplikácie na</span></div>
                <div className="cls_002"><span className="cls_002">zjednodušenie výmeny cvičení študentov Žilinskej univerzity s názvom Swapify</span></div>
                <div className="cls_002"><span className="cls_002">(</span><a HREF="http://swapify.fri.uniza.sk/">swapify.fri.uniza.sk</a> <span className="cls_002">), organizovaného a koučovaného súkromnou firmou GlobalLogic, s. r. o.</span></div>
                <div className="cls_002"><span className="cls_002">(</span><a HREF="http://www.globallogic.com/">www.globallogic.com</a> <span className="cls_002">), právne sídlo spoločnosti: Štúrova 27, Košice 040 01, zapísaná</span></div>
                <div className="cls_002"><span className="cls_002">Okresným súdom Košice I v Košiciach, ICO: 47782421, DIČ: SK2024098142, ako</span></div>
                <div className="cls_002"><span className="cls_002">spolu-prevádzkovateľ internetovej stránky </span><a HREF="http://www.swapify.fri.uniza.sk/">www.swapify.fri.uniza.sk</a>, vyhlasuje, že všetky</div>
                <div className="cls_002"><span className="cls_002">osobné údaje (ďalej aj „údaje“) sú považované za prísne dôverné a je s nimi nakladané v súlade</span></div>
                <div className="cls_002"><span className="cls_002">s platnými zákonnými ustanoveniami v oblasti ochrany osobných údajov.</span></div>
                <div className="cls_002"><span className="cls_002">Bezpečie vašich osobných údajov je pre nás prioritou. Osobným údajom a ich ochrane venujeme</span></div>
                <div className="cls_002"><span className="cls_002">preto náležitú pozornosť. V týchto Zásadách spracovania osobných údajov („Zásady“) by sme</span></div>
                <div className="cls_002"><span className="cls_002">vás chceli informovať o tom, aké osobné údaje o vás zhromažďujeme a akým spôsobom ich ďalej</span></div>
                <div className="cls_002"><span className="cls_002">používame.</span></div>
                <div className="cls_004"><span className="cls_004">1.   Osobné údaje a ich spracovanie</span></div>
                <div className="cls_005"><span className="cls_005">1.1.  Kategórie osobných údajov</span></div>
                <div className="cls_002"><span className="cls_002">Zhromažďujeme rôzne údaje v závislosti od toho, ktoré z našich služieb využívate.</span></div>
                <div className="cls_002"><span className="cls_002">Pokiaľ si u nás vyplníte všetky osobné informácie vo svojom profile, zhromažďujeme:</span></div>
                <div className="cls_006"><span className="cls_006">•</span><span className="cls_002">   Meno a kontaktné údaje. Meno a priezvisko, e-mailovú adresu, telefónne číslo, vami</span></div>
                <div className="cls_002"><span className="cls_002">zadaný odkaz na profil ľubovoľnej sociálnej siete, osobné číslo študenta a ľubovoľný</span></div>
                <div className="cls_002"><span className="cls_002">textový záznam v sekcií profilu „O mne“.</span></div>
                <div className="cls_006"><span className="cls_006">•</span><span className="cls_002">   Demografické údaje. Údaj o pohlaví, dátum narodenia, krajinu a uprednostňovaný jazyk.</span></div>
                <div className="cls_006"><span className="cls_006">•</span><span className="cls_002">   Údaje vzniknuté na základe trvania zmluvy o poskytnutí služby - zmeny vykonávané v</span></div>
                <div className="cls_002"><span className="cls_002">rozvrhu, požiadavky i nevykonané, objem poskytnutých služieb a komunikáciu s našou</span></div>
                <div className="cls_002"><span className="cls_002">podporou.</span></div>
                <div className="cls_006"><span className="cls_006">•</span><span className="cls_002">   Prihlasovacie údaje. Prihlasovacie mená a heslá. Nemáme prístup ku skutočnému heslu.</span></div>
                <div className="cls_002"><span className="cls_002">Pokiaľ si u nás nevyplníte žiadne z nepovinných polí vo svojom profile, zhromažďujeme:</span></div>
                <div className="cls_006"><span className="cls_006">•</span><span className="cls_002">   Meno a kontaktné údaje - e-mailová adresa a krstné meno spolu s priezviskom.</span></div>
                <div className="cls_006"><span className="cls_006">•</span><span className="cls_002">   Demografické údaje. Údaj o pohlaví, krajine a uprednostňovaný jazyk, dátum narodenia.</span></div>
                <div className="cls_006"><span className="cls_006">•</span><span className="cls_002">   Prihlasovacie údaje. Prihlasovacie mená a heslá. Nemáme prístup ku skutočnému heslu.</span></div>
                <div className="cls_002"><span className="cls_002">Ďalej spracovávame tieto osobné údaje:</span></div>
                <div className="cls_006"><span className="cls_006">•</span><span className="cls_002">   Údaje z komunikácie medzi našou používateľskou podporou a používateľom.</span></div>
                <div className="cls_006"><span className="cls_006">•</span><span className="cls_002">   Zmeny vykonávané v rozvrhu, požiadavky i nevykonané a objem poskytnutých služieb.</span></div>

                <div className="cls_006"><span className="cls_006">•</span><span className="cls_002">   Logy obsahujú adresu IP, údaje prehliadača (rozlíšenie, verzia, operačný systém, odtlačok</span></div>
                <div className="cls_002"><span className="cls_002">prsta), jazyk</span></div>
                <div className="cls_006"><span className="cls_006">•</span><span className="cls_002">   Záznamy o správaní na internetových stránkach spravovaných naším spoločenstvom</span></div>
                <div className="cls_005"><span className="cls_005">1.2.  Účely spracovania osobných údajov:</span></div>
                <div className="cls_006"><span className="cls_006">•</span></div>
                <div className="cls_007"><span className="cls_007">Poskytovanie služieb a ich zlepšovanie</span><span className="cls_002">. Aby sme mohli poskytovať ponúkané služby a</span></div>
                <div className="cls_002"><span className="cls_002">zlepšovať ich k vašej spokojnosti, spracovávame vaše osobné údaje. Konkrétne sem</span></div>
                <div className="cls_002"><span className="cls_002">patria:</span></div>
                <div className="cls_008"><span className="cls_008">o</span></div>
                <div className="cls_007"><span className="cls_007">Spracovanie požiadaviek na zmenu rozvrhu</span><span className="cls_002">, vykonaných či už prostredníctvom</span></div>
                <div className="cls_002"><span className="cls_002">našej webovej stránky, mobilnej aplikácie, alebo komunikácie s našou</span></div>
                <div className="cls_002"><span className="cls_002">používateľskou podporou. Dôvodom je tu nevyhnutnosť pre splnenie požiadaviek</span></div>
                <div className="cls_002"><span className="cls_002">a funkčnosť systému.</span></div>
                <div className="cls_008"><span className="cls_008">o</span></div>
                <div className="cls_007"><span className="cls_007">Upozornenie na vykonané zmeny alebo užitočné notifikácie</span><span className="cls_002">. V prípade, že</span></div>
                <div className="cls_002"><span className="cls_002">požiadate o zmenu rozvrhu, spracujeme vaše osobné údaje na základe vášho</span></div>
                <div className="cls_002"><span className="cls_002">súhlasu a ďalej Vám môžu byť zasielané informácie o priebehu spracovania alebo</span></div>
                <div className="cls_002"><span className="cls_002">Vás systém bude na základe vami zadaných údajov informovať prostredníctvom</span></div>
                <div className="cls_002"><span className="cls_002">notifikácií v prostredí web stránky.</span></div>
                <div className="cls_008"><span className="cls_008">o</span></div>
                <div className="cls_007"><span className="cls_007">Používateľská podpora. </span><span className="cls_002">Na zaistenie používateľského servisu a na odstránenie</span></div>
                <div className="cls_002"><span className="cls_002">prípadných problémov pri plnení nami poskytovaných služieb spracovávame vaše</span></div>
                <div className="cls_002"><span className="cls_002">osobné údaje na základe nevyhnutného splnenia požiadaviek alebo prípadných</span></div>
                <div className="cls_002"><span className="cls_002">problémov.</span></div>
                <div className="cls_008"><span className="cls_008">o</span></div>
                <div className="cls_007"><span className="cls_007">Komunikácia</span><span className="cls_002">. Zhromaždené údaje využívame s cieľom komunikácie s vami a jej</span></div>
                <div className="cls_002"><span className="cls_002">individuálneho prispôsobovania. Môžeme Vás napríklad kontaktovať telefonicky,</span></div>
                <div className="cls_002"><span className="cls_002">e-mailom, v aplikácii alebo inou formou, aby sme vám pripomenuli, vykonané</span></div>
                <div className="cls_002"><span className="cls_002">zmeny, pomohli vám s dokončením vašej požiadavky, oznámili vám aktuálny stav</span></div>
                <div className="cls_002"><span className="cls_002">vašej žiadosti o zmeny v rozvrhu alebo k takejto zmene od vás získali ďalšie</span></div>
                <div className="cls_002"><span className="cls_002">informácie. alebo vás upozornili, že musíte vykonať akciu nutnú na zachovanie</span></div>
                <div className="cls_002"><span className="cls_002">aktívneho stavu vášho účtu ako i akcie splnené s dokončením vami požadovanej</span></div>
                <div className="cls_002"><span className="cls_002">zmeny rozvrhu.</span></div>
                <div className="cls_008"><span className="cls_008">o</span></div>
                <div className="cls_007"><span className="cls_007">Zlepšovanie služieb</span><span className="cls_002">. Údaje používame na neustále zlepšovanie našich služieb a</span></div>
                <div className="cls_002"><span className="cls_002">systémov vrátane pridávania nových funkcií a zároveň s cieľom robiť</span></div>
                <div className="cls_002"><span className="cls_002">informované rozhodnutia za použitia súhrnných analýz a business inteligencie, to</span></div>
                <div className="cls_002"><span className="cls_002">všetko na základe nášho oprávneného záujmu odvíjajúceho sa od slobody</span></div>
                <div className="cls_002"><span className="cls_002">podnikania a spočívajúceho v nevyhnutnosti zlepšovania poskytovaných služieb</span></div>
                <div className="cls_002"><span className="cls_002">kvôli úspechu v hospodárskej súťaži. Aby sme Vašim právam a záujmom zaistili</span></div>
                <div className="cls_002"><span className="cls_002">dostatočnú ochranu, s cieľom zlepšovania používame osobné údaje, ktoré sú v čo</span></div>
                <div className="cls_002"><span className="cls_002">najväčšej možnej miere anonymizované.</span></div>
                <div className="cls_006"><span className="cls_006">•</span></div>
                <div className="cls_007"><span className="cls_007">Ochrana, bezpečnosť a riešenie sporov</span><span className="cls_002">. Údaje môžeme spracovávať aj z oprávneného</span></div>
                <div className="cls_002"><span className="cls_002">záujmu, ktorý spočíva v zaistení ochrany a bezpečnosti našich systémov a našich</span></div>
                <div className="cls_002"><span className="cls_002">používateľov, na zisťovanie a prevenciu podvodov, riešenie sporov a presadzovanie</span></div>
                <div className="cls_002"><span className="cls_002">našich dohôd na základe oprávneného záujmu.</span></div>

                <div className="cls_006"><span className="cls_006">•</span><span className="cls_007">   Notifikácie systému na požadované a prípadne vykonané zmeny</span><span className="cls_002">. Tieto oznámenia o</span></div>
                <div className="cls_002"><span className="cls_002">stave môžete vždy odmietnuť prostredníctvom odkazu pre odhlásenie, ktorý nájdete</span></div>
                <div className="cls_002"><span className="cls_002">v každom e-maile alebo vo vašom profile. V prípade, že odoberanie oznámení o stave</span></div>
                <div className="cls_002"><span className="cls_002">odhlásite, nebudeme naďalej vaše elektronické kontakty na tieto účely využívať. Začneme</span></div>
                <div className="cls_002"><span className="cls_002">ich opäť využívať, pokiaľ ich zaregistrujete alebo si ich výslovne vyžiadate.</span></div>
                <div className="cls_008"><span className="cls_008">o</span><span className="cls_002">  Pokiaľ nie ste naším používateľom, spracovávame na základe vášho súhlasu.</span></div>
                <div className="cls_008"><span className="cls_008">o</span><span className="cls_002">  Máte právo kedykoľvek bezplatne vzniesť námietku proti týmto spracovaniam.</span></div>
                <div className="cls_002"><span className="cls_002">Kontaktné údaje sú uvedené na konci tohto dokumentu.</span></div>
                <div className="cls_006"><span className="cls_006">•</span><span className="cls_002">   Spracovávanie </span><span className="cls_007">cookies z internetových stránok</span><span className="cls_002"> prevádzkovaných naším spoločenstvom</span></div>
                <div className="cls_008"><span className="cls_008">o</span><span className="cls_002">  V prípade, že máte vo svojom webovom prehliadači povolené cookies,</span></div>
                <div className="cls_002"><span className="cls_002">spracovávame záznamy správania z cookies umiestnených na internetových</span></div>
                <div className="cls_002"><span className="cls_002">stránkach prevádzkovaných naším spoločenstvom, a to na účely zaistenia lepšej</span></div>
                <div className="cls_002"><span className="cls_002">prevádzky internetových stránok Swapify. Viac informácií nájdete v samostatnej</span></div>
                <div className="cls_002"><span className="cls_002">kapitole tohto dokumentu.</span></div>
                <div className="cls_005"><span className="cls_005">1.3.  Odovzdanie osobných údajov tretím stranám</span></div>
                <div className="cls_002"><span className="cls_002">Vaše osobné údaje budú odovzdané tretím osobám či inak sprostredkované iba vtedy, ak je to</span></div>
                <div className="cls_002"><span className="cls_002">nevyhnutné v rámci plnenia zmluvných podmienok a nami poskytovaných služieb, na základe</span></div>
                <div className="cls_002"><span className="cls_002">oprávneného záujmu alebo pokiaľ ste s tým vopred vyslovili súhlas.</span></div>
                <div className="cls_006"><span className="cls_006">•</span><span className="cls_002">   a) sesterským spoločnostiam a spracovateľom na základe plnenia zmluvných podmienok</span></div>
                <div className="cls_002"><span className="cls_002">a nami poskytovaných služieb pre výkon interných procesov a postupov</span></div>
                <div className="cls_006"><span className="cls_006">•</span><span className="cls_002">   b) ďalším poskytovateľom služieb, tretím stranám zapojeným do spracovania dát</span></div>
                <div className="cls_006"><span className="cls_006">•</span><span className="cls_002">   c) tretím stranám, napr. právnym alebo finančným zástupcom, súdom za účelom</span></div>
                <div className="cls_002"><span className="cls_002">spracovania daňových dokladov, vymáhanie pohľadávok alebo ďalších dôvodov</span></div>
                <div className="cls_002"><span className="cls_002">vyplývajúcich z plnenia našich zákonných povinností</span></div>
                <div className="cls_006"><span className="cls_006">•</span><span className="cls_002">   d) verejným orgánom (napr. polícia)</span></div>
                <div className="cls_006"><span className="cls_006">•</span><span className="cls_002">   e) tretím stranám vykonávajúcim prieskumy medzi používateľmi</span></div>
                <div className="cls_002"><span className="cls_002">Pokiaľ tretie osoby použijú údaje v rámci ich oprávneného záujmu, nenesie za tieto spracovania</span></div>
                <div className="cls_002"><span className="cls_002">správca zodpovednosť. Tieto spracovania sa riadia zásadami spracovania osobných údajov</span></div>
                <div className="cls_002"><span className="cls_002">príslušných spoločností a osôb.</span></div>
                <div className="cls_004"><span className="cls_004">2.   Používateľské konto</span></div>
                <div className="cls_006"><span className="cls_006">•</span><span className="cls_002">   a) V rámci uzavretia zmluvy o nami poskytovaných službách vám zakladáme</span></div>
                <div className="cls_002"><span className="cls_002">používateľské konto, ktoré je zabezpečené heslom. V rámci používateľského konta</span></div>

                <div className="cls_002"><span className="cls_002">získavate priamy prístup k svojim údajom vrátane ich editácie a môžete takto nahliadať</span></div>
                <div className="cls_002"><span className="cls_002">do svojich údajov o dokončených aj nedokončených požiadavkách o zmenu rozvrhu a</span></div>
                <div className="cls_002"><span className="cls_002">upravovať zasielanie notifikácií. Prostredníctvom používateľského konta môžete takisto</span></div>
                <div className="cls_002"><span className="cls_002">spravovať svoje osobné údaje a zasielanie informatívnych emailov.</span></div>
                <div className="cls_006"><span className="cls_006">•</span><span className="cls_002">   b) Pokiaľ si neprajete založiť pre uskutočnenie zmien v rozvrhu používateľské konto,</span></div>
                <div className="cls_002"><span className="cls_002">nemôžete v našom v našom systéme bez registrácie, t. j. ako nečlen vykonávať žiadne</span></div>
                <div className="cls_002"><span className="cls_002">zmeny v rozvrhu a teda funkcionalita systému je neprístupná.</span></div>
                <div className="cls_006"><span className="cls_006">•</span><span className="cls_002">   c) Máte právo ukončiť zmluvu o nami poskytovaných službách v súlade s príslušnými</span></div>
                <div className="cls_002"><span className="cls_002">podmienkami.</span></div>
                <div className="cls_004"><span className="cls_004">3.   Zabezpečenie osobných údajov a obdobie uchovávania</span></div>
                <div className="cls_005"><span className="cls_005">3.1.  Zabezpečenie osobných údajov</span></div>
                <div className="cls_006"><span className="cls_006">•</span><span className="cls_002">   a) Vaše osobné údaje sa k nám prenášajú v zakódovanej podobe. Používame kódovací</span></div>
                <div className="cls_002"><span className="cls_002">systém SSL (secure socket layer). Zabezpečujeme naše webové stránky a ostatné systémy</span></div>
                <div className="cls_002"><span className="cls_002">pomocou technických a organizačných opatrení proti strate a zničeniu vašich údajov, proti</span></div>
                <div className="cls_002"><span className="cls_002">prístupu neoprávnených osôb k vašim údajom, ich pozmeňovaniu či rozširovaniu.</span></div>
                <div className="cls_006"><span className="cls_006">•</span><span className="cls_002">   b) U našich spracovateľov vyžadujeme preukázanie súladu ich systémov s nariadením</span></div>
                <div className="cls_002"><span className="cls_002">GDPR.</span></div>
                <div className="cls_006"><span className="cls_006">•</span><span className="cls_002">   c) Prístup do vášho používateľského konta je možný iba po zadaní vášho osobného hesla.</span></div>
                <div className="cls_002"><span className="cls_002">V tejto súvislosti by sme vás chceli upozorniť, že je nevyhnutné, aby ste vaše prístupové</span></div>
                <div className="cls_002"><span className="cls_002">údaje neoznamovali tretím osobám a po ukončení vašej činnosti v používateľskom konte</span></div>
                <div className="cls_002"><span className="cls_002">vždy zavreli okno svojho webového prehliadača, najmä vtedy, ak používate počítač spolu</span></div>
                <div className="cls_002"><span className="cls_002">s inými používateľmi. Swapify nepreberá zodpovednosť za zneužitie použitých hesiel,</span></div>
                <div className="cls_002"><span className="cls_002">ibaže by túto situáciu Swapify priamo spôsobil.</span></div>
                <div className="cls_005"><span className="cls_005">3.2.  Obdobie spracovania</span></div>
                <div className="cls_002"><span className="cls_002">Osobné údaje spracovávame a uchovávame</span></div>
                <div className="cls_006"><span className="cls_006">•</span><span className="cls_002">   v čase nevyhnutnom na zaistenie všetkých práv a povinností vyplývajúcich zo zmluvy</span></div>
                <div className="cls_002"><span className="cls_002">o nami poskytovaných službách</span></div>
                <div className="cls_006"><span className="cls_006">•</span><span className="cls_002">   počas trvania zmluvy o nami poskytovaných službách</span></div>
                <div className="cls_006"><span className="cls_006">•</span><span className="cls_002">   v čase, počas ktorého je Swapify povinné ako správca uchovávať podľa všeobecne</span></div>
                <div className="cls_002"><span className="cls_002">záväzných právnych predpisov. Účtovné doklady, napr. faktúry vystavené spoločnosťou,</span></div>
                <div className="cls_002"><span className="cls_002">sú v súlade so zákonom archivované 10 rokov od ich vystavenia.</span></div>
                <div className="cls_006"><span className="cls_006">•</span><span className="cls_002">   Súhlas pre upozornenie na zmeny v rozvrhu zostáva v platnosti do registrácie používateľa</span></div>
                <div className="cls_002"><span className="cls_002">do jeho odvolania.</span></div>
                <div className="cls_006"><span className="cls_006">•</span><span className="cls_002">   Súhlas s notifikáciami v rámci systému je platný do registrácie používateľa do odvolania</span></div>

                <div className="cls_006"><span className="cls_006">•</span><span className="cls_002">   Komunikácia po dobu neurčitú, však minimálne po dobu vyplývajúcu z účelu spracovania</span></div>
                <div className="cls_002"><span className="cls_002">jej obsahu a vyhovenie požiadavkám alebo prípadným problémom používateľa</span></div>
                <div className="cls_006"><span className="cls_006">•</span><span className="cls_002">   V ostatných prípadoch vyplýva obdobie spracovania z účelu spracovania alebo je dané</span></div>
                <div className="cls_002"><span className="cls_002">právnymi predpismi v oblasti ochrany osobných údajov.</span></div>
                <div className="cls_004"><span className="cls_004">4.   Práva subjektov údajov</span></div>
                <div className="cls_006"><span className="cls_006">•</span><span className="cls_002">   a) Pokiaľ spracovávame vaše osobné údaje, môžete kedykoľvek požadovať bezplatnú</span></div>
                <div className="cls_002"><span className="cls_002">informáciu o spracovávaní svojich osobných údajov.</span></div>
                <div className="cls_006"><span className="cls_006">•</span><span className="cls_002">   b) V prípade, že sa domnievate, že spracovanie osobných údajov robíme v rozpore</span></div>
                <div className="cls_002"><span className="cls_002">s ochranou vašich osobných údajov a zákonnými podmienkami ochrany osobných údajov,</span></div>
                <div className="cls_002"><span className="cls_002">môžete požiadať o vysvetlenie, požadovať, aby sme odstránili takto vzniknutý stav, najmä</span></div>
                <div className="cls_002"><span className="cls_002">môžete požadovať urobenie opravy, doplnenie, likvidáciu osobných údajov alebo</span></div>
                <div className="cls_002"><span className="cls_002">blokáciu osobných údajov.</span></div>
                <div className="cls_006"><span className="cls_006">•</span><span className="cls_002">   c) Pre uplatnenie svojich práv kontaktujte poverenca pre ochranu osobných údajov na</span></div>
                <div className="cls_002"><span className="cls_002">e-mailovej adrese </span><a HREF="mailto:oleg@swapify.com">oleg@swapify.com</a> <span className="cls_002">. Obrátiť sa môžete aj na Úrad na ochranu osobných</span></div>
                <div className="cls_002"><span className="cls_002">údajov.</span></div>
                <div className="cls_006"><span className="cls_006">•</span><span className="cls_002">   d) Svoj súhlas so spracovaním osobných údajov môžete kedykoľvek odvolať. Pokiaľ</span></div>
                <div className="cls_002"><span className="cls_002">odvoláte súhlas so spracovaním osobných údajov, budú vaše osobné údaje vymazané</span></div>
                <div className="cls_002"><span className="cls_002">alebo anonymizované; to sa však netýka tých osobných údajov, ktoré Swapify potrebuje</span></div>
                <div className="cls_002"><span className="cls_002">na splnenie zákonných povinností (napr. vybavenie už podanej požiadavky) či na ochranu</span></div>
                <div className="cls_002"><span className="cls_002">svojich oprávnených záujmov. K likvidácii osobných údajov dôjde takisto v prípade, že</span></div>
                <div className="cls_002"><span className="cls_002">osobné údaje nebudú potrebné pre stanovený účel alebo ak bude uloženie vašich údajov</span></div>
                <div className="cls_002"><span className="cls_002">neprípustné z iných zákonom stanovených dôvodov.</span></div>
                <div className="cls_004"><span className="cls_004">5.   Webové stránky</span></div>
                <div className="cls_005"><span className="cls_005">5.1.  Súbory cookies</span></div>
                <div className="cls_002"><span className="cls_002">Naše stránky používajú tzv. cookies, abybola zachovaná plná funkcionalita nami ponúkaných</span></div>
                <div className="cls_002"><span className="cls_002">služieb. Cookies sú malé textové súbory, ktoré sú uložené vo vašom počítači, smartfóne či inom</span></div>
                <div className="cls_002"><span className="cls_002">zariadení a ktoré sa používajú vo vašom prehliadači. Viac informácií o súboroch cookies môžete</span></div>
                <div className="cls_002"><span className="cls_002">nájsť </span><a HREF="https://en.wikipedia.org/wiki/HTTP_cookie">tu</a> <span className="cls_002">. Súbory cookies využívame napr. na:</span></div>
                <div className="cls_006"><span className="cls_006">•</span><span className="cls_002">   správnu funkčnosť systému tak, aby ste čo najjednoduchšie dokončili vykonávanie</span></div>
                <div className="cls_002"><span className="cls_002">svojich zmien v rozvrhu</span></div>
                <div className="cls_006"><span className="cls_006">•</span><span className="cls_002">   zapamätanie vašich prihlasovacích údajov, takže ich nemusíte opakovane zadávať</span></div>
                <div className="cls_006"><span className="cls_006">•</span><span className="cls_002">   čo najlepšie prispôsobenie našich stránok vašim požiadavkám vďaka sledovaniu</span></div>
                <div className="cls_002"><span className="cls_002">návštevnosti, vášho pohybu po stránkach a využitých funkciách</span></div>
                <div className="cls_006"><span className="cls_006">•</span><span className="cls_002">   zistenie informácií o prezeraní reklám, aby sme vám nezobrazovali reklamu, o ktorú</span></div>
                <div className="cls_002"><span className="cls_002">nemáte záujem</span></div>

                <div className="cls_002"><span className="cls_002">Niektoré súbory cookie môžu zhromažďovať informácie, ktoré sú následne využité tretími</span></div>
                <div className="cls_002"><span className="cls_002">stranami a ktoré napr. priamo podporujú naše reklamné aktivity (tzv. „cookie tretích strán“).</span></div>
                <div className="cls_002"><span className="cls_002">Napríklad informácie o využívaných službách na našich stránkach môžu byť zobrazené</span></div>
                <div className="cls_002"><span className="cls_002">reklamnou agentúrou v rámci zobrazenia a prispôsobenia internetových reklamných bannerov na</span></div>
                <div className="cls_002"><span className="cls_002">vami zobrazovaných webových stránkach. Podľa týchto údajov vás však nemožno identifikovať.</span></div>
                <div className="cls_005"><span className="cls_005">5.2.  Využitie súborov cookies</span></div>
                <div className="cls_002"><span className="cls_002">Súbory cookie používané na našich stránkach možno rozdeliť na dva základné typy. Krátkodobé,</span></div>
                <div className="cls_002"><span className="cls_002">tzv. „session cookie“, sú zmazané ihneď, len čo ukončíte návštevu našich stránok. Dlhodobé, tzv.</span></div>
                <div className="cls_002"><span className="cls_002">„persistent cookie“, zostávajú uložené vo vašom zariadení omnoho dlhšie alebo kým ich ručne</span></div>
                <div className="cls_002"><span className="cls_002">neodstránite (čas ponechania súborov cookie vo vašom zariadení závisí od nastavenia samotnej</span></div>
                <div className="cls_002"><span className="cls_002">cookie a nastavenia vášho prehliadača).</span></div>
                <div className="cls_002"><span className="cls_002">Cookie možno tiež rozdeliť podľa ich funkčnosti na:</span></div>
                <div className="cls_006"><span className="cls_006">•</span><span className="cls_002">   analytické, ktoré nám pomáhajú zvýšiť používateľské pohodlie nášho webu tým, že</span></div>
                <div className="cls_002"><span className="cls_002">pochopíme, ako ho používatelia používajú</span></div>
                <div className="cls_006"><span className="cls_006">•</span><span className="cls_002">   konverzné, ktoré nám umožňujú analyzovať výkon rôznych kanálov pre využívanie</span></div>
                <div className="cls_002"><span className="cls_002">služieb</span></div>
                <div className="cls_006"><span className="cls_006">•</span><span className="cls_002">   trackingové (sledovacie), ktoré v kombinácii s konverznými pomáhajú analyzovať výkon</span></div>
                <div className="cls_002"><span className="cls_002">rôznych kanálov pre využívanie služieb</span></div>
                <div className="cls_006"><span className="cls_006">•</span><span className="cls_002">   remarketingové, ktoré používame na personalizáciu obsahu reklám a ich správne</span></div>
                <div className="cls_002"><span className="cls_002">zacielenie</span></div>
                <div className="cls_006"><span className="cls_006">•</span><span className="cls_002">   esenciálne, ktoré sú dôležité pre základnú funkčnosť webu</span></div>
                <div className="cls_005"><span className="cls_005">5.3.  Odmietnutie súborov cookies</span></div>
                <div className="cls_002"><span className="cls_002">Nastavenie použitia súborov cookies je súčasťou vášho internetového prehliadača. Väčšina</span></div>
                <div className="cls_002"><span className="cls_002">prehliadačov súbory cookies vo východiskovom nastavení automaticky prijíma. Súbory cookies</span></div>
                <div className="cls_002"><span className="cls_002">možno pomocou vášho webového prehliadača odmietnuť alebo obmedziť ich na vami vybrané</span></div>
                <div className="cls_002"><span className="cls_002">typy.</span></div>
                <div className="cls_002"><span className="cls_002">Informácie o prehliadačoch a o spôsobe nastavenia predvolieb pre súbory cookie môžete nájsť na</span></div>
                <div className="cls_002"><span className="cls_002">nasledujúcich webových stránkach alebo v ďalšej dokumentácii internetových prehliadačov</span></div>
                <div className="cls_006"><span className="cls_006">•</span><span className="cls_003">   </span><a HREF="https://support.google.com/accounts/answer/61416?hl=sk&co=GENIE.Platform=Desktop&oco=1">Chrome</a> </div>
                <div className="cls_006"><span className="cls_006">•</span><span className="cls_003">   </span><a HREF="https://support.mozilla.org/sk/kb/povolenie-zakazanie-cookies">Firefox</a> </div>
                <div className="cls_006"><span className="cls_006">•</span><span className="cls_003">   </span><a HREF="https://support.microsoft.com/sk-sk/help/17442/windows-internet-explorer-delete-manage-cookies">Internet Explorer</a> </div>
                <div className="cls_006"><span className="cls_006">•</span><span className="cls_003">   </span><a HREF="https://support.google.com/accounts/answer/61416?hl=sk&co=GENIE.Platform=Android&oco=1">Android</a> </div>

                <div className="cls_002"><span className="cls_002">Účinný nástroj na správu súborov cookie je tiež k dispozícii na stránkach</span></div>
                <div className="cls_009"><span className="cls_009"> </span><a HREF="http://www.youronlinechoices.com/sk/">www.youronlinechoices.com/sk/</a> </div>
                <div className="cls_005"><span className="cls_005">5.4.  Odkazy</span></div>
                <div className="cls_002"><span className="cls_002">Naše webové stránky obsahujú odkazy na iné webové stránky, ktoré sú praktické a obsahujú</span></div>
                <div className="cls_002"><span className="cls_002">informácie. Upozorňujeme, že tieto stránky môžu byť vlastnené a prevádzkované ďalšími</span></div>
                <div className="cls_002"><span className="cls_002">spoločnosťami a organizáciami a majú iné zásady zabezpečenia a ochrany osobných údajov.</span></div>
                <div className="cls_002"><span className="cls_002">Naše spoločenstvo nemá žiadnu kontrolu a nenesie žiadnu zodpovednosť za akékoľvek</span></div>
                <div className="cls_002"><span className="cls_002">informácie, materiál, produkty alebo služby nachádzajúce sa na týchto webových stránkach alebo</span></div>
                <div className="cls_002"><span className="cls_002">prístupné prostredníctvom nich.</span></div>
                <div className="cls_004"><span className="cls_004">6.   Kontaktujte nás</span></div>
                <div className="cls_002"><span className="cls_002">V prípade akýchkoľvek otázok, komentárov a žiadostí, čo sa týka týchto Zásad, nás neváhajte</span></div>
                <div className="cls_002"><span className="cls_002">kontaktovať prostredníctvom e-mailovej adresy.</span></div>
                <div className="cls_002"><span className="cls_002">Zodpovedná osoba: </span><a HREF="mailto:oleg@swapify.com">oleg@swapify.com</a> </div>
                <div className="cls_002"><span className="cls_002">Kontakt: GlobalLogic, s.r.o.,</span></div>
                <div className="cls_002"><span className="cls_002">Štúrova 27,</span></div>
                <div className="cls_002"><span className="cls_002">040 01 Košice</span></div>
                <div className="cls_002"><span className="cls_002">Slovenská republika</span></div>
                <div className="cls_002"><span className="cls_002">Používateľský servis: </span><a HREF="mailto:info@swapify.sk">info@swapify.sk</a> </div>
                <div className="cls_004"><span className="cls_004">7.   Účinnosť</span></div>
                <div className="cls_002"><span className="cls_002">Tieto Zásady ochrany osobných údajov platia od 1.1. 2015.</span></div>

              </div>
            </div>

          </body>
        </html>

    );
  }
}

export default connect()(PrivacyPolicyPage);
