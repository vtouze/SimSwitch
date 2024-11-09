/**
* Name: Mayor
* Based on the internal empty template. 
* Author: kevinchapuis
* Tags: 
*/


model Mayor

import "City.gaml"
import "../parameters.gaml"

species mayor {
	
	city mycity;
	
	// The budget is made of credit (units) that represent a "token-of-action"
	// Renew of budget is based on recurrent costs and incomes from taxes
	float budget <- float(STARTING_BUDGET);
	
	// TAXES
	float __taxe_fuel <- 1.0 min:0.0 max:__MAXTF; float __MAXTF <- 2.0;
	float __parc_price <- 1.0 min:0.0 max:__MAXPP; float __MAXPP <- 2.0; 
	float __bus_price <- 1.0 min:0.0 max:__MAXBP; float __MAXBP <- 2.0;
	float __local_taxe <- 1.0 min:0.0 max:__MAXLT; float __MAXLT <- 2.0;// Adjustement criteria for budget equilibrium
	
	/**
	 * 
	 * BUDGET DYNAMIC
	 * --------------
	 * 
	 * 
	 */
	reflex mobility_budget when:cycle>1 and every(#day) {
		
		/* ALL INPUTS */
		
		// CAR = fuel taxes & parking
		// TODO : add parking price * park occupancey
		// TODO : add car ratio and infrastructure usage 
		float CAR_decisionpart <- __taxe_fuel; 
		float __car_input <- CAR_decisionpart * STARTING_BUDGET * FUEL_TAXE_RATIO_BALANCE * #day / #year;
		
		// PT = tikets
		// TODO : add bus ratio and infrastructure usage
		float PT_decisionpart <- __bus_price;
		float __pt_input <- PT_decisionpart * STARTING_BUDGET * (PT_TAXE_RATIO_BALANCE + PT_PAYMENT_RATIO) * #day / #year;
		
		// TAXES
		float __local_taxes <- __local_taxe * STARTING_BUDGET * LOCAL_TAXE_RATIO_BALANCE * #day / #year;
		
		/* ALL INVESTMENT */
		
		// 1 - Subsidies
		// 2 - Public works
		float __invest;
		
		budget <- budget + __car_input + __pt_input + __local_taxes - __invest;
		
	}
	
}