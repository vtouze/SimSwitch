model SendAndReceiveMessages

import "../UnityLinker/Simulation.gaml"
import "../SimSwitch/Population.gaml"

species unity_linker parent: abstract_unity_linker {
	string player_species <- string(unity_player);

	bool do_send_world <- false;
	
	/**************************************
	 * DAILY INFORMATION FROM GAMA TO UNITY
	 */
	reflex daily {
		do send_message players: unity_player as list mes: ["VIRGILE"::"DAILY",
			"_day"::current_date.day,
			"_dayOfWeek"::current_date.day_of_week,
			"_month"::current_date.month,
			"_year"::current_date.year
		];
		write "Daily date: " + current_date.day + " / " + current_date.month + " / " + current_date.year;
	}
		
	action cc {
		city c <- world.CITY_BUILDER();
    	ask world {do POP_SYNTH(c);}
		write "Population has been initialized";
    	write "Send message: "  + cycle + " / " + cycle;
		do send_message players: unity_player as list mes: ["city":: c, "district"::c.q, "households"::household];
		
	}
	
	action receive_message (string id, string mes) {
		write "Player " + id + " send the message: " + mes;
	}
	
	action receive_experiment (string status, string exp_id){
		write status + ": " + exp_id;
		do send_message players: unity_player as list mes: ["status"::status];
	}
}


species unity_player parent: abstract_unity_player;


experiment SimpleMessage type: gui ;


experiment unity_xp parent:SimpleMessage autorun: false type: unity {
	float minimum_cycle_duration <- 0.05;

	string unity_linker_species <- string(unity_linker);


	action create_player(string id) {
		ask unity_linker {
			do create_player(id);
		}
	}

	action remove_player(string id_input) {
		if (not empty(unity_player)) {
			ask first(unity_player where (each.name = id_input)) {
				do die;
			}
		}
	}
}