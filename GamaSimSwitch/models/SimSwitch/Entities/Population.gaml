/**
* Name: Population
* Based on the internal empty template. 
* Author: kevinchapuis
* Tags: 
*/


model Population

/*
 * Every transport related decision are made by households
 */
species household schedules:[] {
	
	// Demographics
	int size;
	string householder;
	int number_of_child;
	string incomes;
	
}