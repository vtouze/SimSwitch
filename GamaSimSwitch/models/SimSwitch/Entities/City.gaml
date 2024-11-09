/**
* Name: City
* Based on the internal empty template. 
* Author: kevinchapuis
* Tags: 
*/


model City

import "Population.gaml"
import "Mayor.gaml"
import "../parameters.gaml"

species city {
	
	mayor citymayor;
	
	// ==============
	
	list<district> q;
	
	// ==============
	// Infrastructure
	
	list<publicwork> publicworks;
	
	// TODO infrastructure modification should lower or increase expected infrastructure usage ratio
	float car_infrastructure_dimension <- CARMOBEXP min:0.01 max:1.0;
	float pt_infrastructure_dimension <- PUBMOBEXP min:0.01 max:1.0;
	float bike_infrastructure_dimension <- BIKMOBEXP min:0.01 max:1.0;
	
	// ======= UTILITIES ======= //
	
	/*
	 * Return the total population of the city
	 */
	int total_population {
		int totalpop;
		loop d over:q { loop h over:d.pop.keys { totalpop <- totalpop + d.pop[h] * h.size;} }
		return totalpop;
	}
	
}

species district {
	
	map<household,int> pop;
	
}