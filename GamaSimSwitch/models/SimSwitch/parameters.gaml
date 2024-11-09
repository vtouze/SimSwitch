/**
* Name: parameters
* Based on the internal empty template. 
* Author: kevinchapuis
* Tags: 
*/


model parameters

import "Entities/Transport.gaml"
import "Entities/City.gaml"

global {
	
	// **************************** //
	//								//
	//			  BUDGET			//
	//								//
	// **************************** //
	
	// VARIABLE INPUTS
	int STARTING_BUDGET <- 100;
	
	// ##############################################################################
	// https://www.economie.gouv.fr/cedef/chiffres-cles-budgets-collectivites-locales
	// https://www.ecologie.gouv.fr/sites/default/files/bis_141_budget_du_maire.pdf
	// https://www.collectivites-locales.gouv.fr/collectivites-locales-chiffres-2023
	// ##############################################################################
	
	int INVEST_BUDGET_PER_INHABITANT <- 331; 
	
	// Part des dépenses d'investissement dans les équipement de transport 
	
	// Métropole de montpellier : investissement équipement transport / budget d'investissement
	// 1er budget d'investissement, 6* suppérieur au 2nd !!!!
	// Rapport : https://www.google.com/url?sa=t&rct=j&q=&esrc=s&source=web&cd=&ved=2ahUKEwj1m-zqmYOFAxUJRaQEHZXYDRQQFnoECBkQAQ&url=https%3A%2F%2Fwww.montpellier.fr%2Finclude%2FviewFile.php%3Fidtf%3D41603%26path%3D39%252F41603_694_Rapport-BP-2022-Ville-VF.pdf&usg=AOvVaw02g20wR3krcQR3i-H_0zop&opi=89978449
	float BUDGET_RATIO_EQUIPEMENT_TRANSPORT <- 146.4/640.0;
	
	// !!!!!!!!!!!!!!!!!!!!!!!
	// !!!!!!!!!!!!!!!!!!!!!!!
	// L’Île-de-France concentre 75 % de la demande de transport collectif urbain de France métropolitaine
	// https://www.statistiques.developpement-durable.gouv.fr/sites/default/files/2018-11/datalab-essentiel-150-transport-collectif-urbain-septembre2018.pdf
	
	// bis_141_budget_du_maire.pdf
	float BUDGET_BALANCE_FUNCTIONING <- (37.6+16.7+8.6+4.6) / (52.3+14.1+13.4);
	float LOCAL_TAXE_RATIO_BALANCE <- 52.3 / (52.3+14.1+13.4) * 12.3 / (7.8+3.4); // recette de fonctionnement into excedent en recette d'investissement
	
	// -------------------------------------------------
	// PART DU BUDGET INVESTISSEMENT (RECURENT) PAR MODE
	
	// ratio of TICPE input in budget balance see Chapitre 1 - Les chiffres clés des collectivités 2023
	float FUEL_TAXE_RATIO_BALANCE <- 0.07;
	
	// Ratio of "Versement mobilité" over budget input
	float PT_TAXE_RATIO_BALANCE <- 0.03;
	// Ratio of public transport cost payed with tickets
	// https://www.statistiques.developpement-durable.gouv.fr/sites/default/files/2018-11/datalab-essentiel-150-transport-collectif-urbain-septembre2018.pdf
	float PT_PAYMENT_RATIO <- 0.18;   
	
	// ======= UTILS
	
	/*
	 * Donne le montant des investissements annuels dans les équipements de transports
	 */
	int __actual_base_annual_budget(city c) { 
		return INVEST_BUDGET_PER_INHABITANT*c.total_population()*BUDGET_RATIO_EQUIPEMENT_TRANSPORT;
	}
	
	// **************************** //
	//								//
	//			TRANSPORT			//
	//								//
	// **************************** //
	
	// Overall mode ratio
	// https://www.statistiques.developpement-durable.gouv.fr/edition-numerique/chiffres-cles-transports-2022/12-transport-interieur-de-voyageurs
	// https://www.statistiques.developpement-durable.gouv.fr/la-mobilite-locale-et-longue-distance-des-francais-enquete-nationale-sur-la-mobilite-des-0#:~:text=—%20La%20mobilité%20locale%20des%20Français,de%20plus%20qu%27en%202008.
	
	map<string,float> MODE_EXPECTED_RATIO <- [
		CARMODE::CARMOBEXP, ACTIVEMODE::BIKMOBEXP+WALMOBEXP, PUBLICTRANSPORTMODE::PUBMOBEXP
	];
	
	float CARMOBEXP <- 0.628;
	float PUBMOBEXP <- 0.091;
	float BIKMOBEXP <- 0.027;
	float WALMOBEXP <- 0.237; // TODO add walk
	
}