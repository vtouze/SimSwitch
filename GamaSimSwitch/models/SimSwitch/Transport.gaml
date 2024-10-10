/**
* Name: Transport
* Based on the internal empty template. 
* Author: kevinchapuis
* Tags: 
*/


model Transport

global {
	
	// ONLY 3 MODES
	mode ACTIVE;
	mode PUBLICTRANSPORT;
	mode CAR;
	
	// INCOMES X MODES = cost
	matrix<float> MOBCOST; 
}

/*
 * Mode of transports
 */ 
species mode schedules:[] { map<string,float> criterias; }

/*
 * Public work modifying transport system
 */ 
species publicwork {}