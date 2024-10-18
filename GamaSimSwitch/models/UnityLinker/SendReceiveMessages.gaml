model SendAndReceiveMessages

import "../UnityLinker/Simulation.gaml"
import "../SimSwitch/Population.gaml"

//Species that will make the link between GAMA and Unity. It has to inherit from the built-in species asbtract_unity_linker
species unity_linker parent: abstract_unity_linker {
	//name of the species used to represent a Unity player
	string player_species <- string(unity_player);

	//in this model, no information will be automatically sent to the Player at every step, so we set do_info_world to false
	bool do_send_world <- false;
	
	/**************************************
	 * DAILY INFORMATION FROM GAMA TO UNITY
	 */
	reflex daily {
		do send_message players: unity_player as list mes: ["VIRGILE"::"DAILY",
			"day"::current_date.day,
			"month"::current_date.month,
			"year"::current_date.year
		];
	}
	
	
	//reflex activated only when there is at least one player and every 100 cycles
	action cc {
		city c <- world.CITY_BUILDER();
    	ask world {do POP_SYNTH(c);}
		write "Population has been initialized";
    	
    	//send a message to all players; the message should be a map (key: name of the attribute; value: value of this attribute)
		//the name of the attribute should be the same as the variable in the serialized class in Unity (c# script) 
		write "Send message: "  + cycle;
		do send_message players: unity_player as list mes: ["city":: c, "district"::c.q, "households"::household];
		
	}
	
	//action that will be called by the Unity player to send a message to the GAMA simulation
	action receive_message (string id, string mes) {
		write "Player " + id + " send the message: " + mes;
	}
	
	action receive_experiment (string status, string exp_id){
		write status + ": " + exp_id;
		do send_message players: unity_player as list mes: ["status"::status];
	}
}


//species used to represent an unity player, with the default attributes. It has to inherit from the built-in species asbtract_unity_player
species unity_player parent: abstract_unity_player;


//default experiment
experiment SimpleMessage type: gui ;


//The unity type allows to create at the initialization one unity_linker agent
experiment unity_xp parent:SimpleMessage autorun: false type: unity {
	//minimal time between two simulation step
	float minimum_cycle_duration <- 0.05;

	//name of the species used for the unity_linker
	string unity_linker_species <- string(unity_linker);


	//action called by the middleware when a player connects to the simulation
	action create_player(string id) {
		ask unity_linker {
			do create_player(id);
		}
	}

	//action called by the middleware when a player is remove from the simulation
	action remove_player(string id_input) {
		if (not empty(unity_player)) {
			ask first(unity_player where (each.name = id_input)) {
				do die;
			}
		}
	}
}