/**
* Name: City
* Based on the internal empty template. 
* Author: kevinchapuis
* Tags: 
*/


model City

import "Population.gaml"

species city {
	list<district> q;
}

species district {
	map<household,int> pop;
}