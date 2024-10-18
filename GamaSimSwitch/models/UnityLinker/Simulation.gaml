/**
* Name: Simulation
* Based on the internal empty template. 
* Author: stage
* Tags: 
*/


model Simulation

import "../SimSwitch/Population.gaml"
import "../SimSwitch/City.gaml"
import "../SimSwitch/Transport.gaml"

global {
	int test <- 2109;
	
	date starting_date <- #now; 
	float step <- 6#hours/*1#day*/;
	
	city thecity;
	
	city CITY_BUILDER {
		
		create district number:2;
		
		// Create city
		create city with:[q::list(district)];
		thecity <- first(city);
		
		return thecity;
	}
	
	action POP_SYNTH(city c) {
		
		loop hsdl over:["s","c"] {
			loop nbc over:[0,1,2,3] {
				loop inc over:["l","m","h"] {
					create household with:[householder::hsdl,number_of_child::nbc,incomes::inc] {
						size <- (householder="s" ? 1 : 2) + number_of_child;
					}
				}
			}
		}
		
		loop d over:c.q {
			loop hh over:household {
				d.pop[hh] <- rnd(1000);
			}
		}
	}
		
}
