model SendAndReceiveMessages

import "../UnityLinker/Simulation.gaml"
import "MessageTAGs.gaml"

species unity_linker parent: abstract_unity_linker {
	
	string player_species <- string(unity_player);
	bool do_send_world <- false;

	/**************************************
	 * DAILY INFORMATION FROM GAMA TO UNITY
	 */
	reflex daily {
		
		do send_message players: unity_player as list mes: [
			DAILYMESS::SIMDATE, // TODO : we have to change this so we recognise what it is
			_D::current_date.day,
			_DW::current_date.day_of_week,
			_M::current_date.month,
			_Y::current_date.year
		];
		
		do send_message players: unity_player as list mes: [
			DAILYMESS::SIMBUDGET, 
			_Budg::thecity.citymayor.budget
		];
	}

	action cc {
		city c <- world.CITY_BUILDER();
		ask world { do POP_SYNTH(c); }
		write "Population has been initialized";
		write "Send message: " + cycle + " / " + cycle;
		do send_message players: unity_player as list mes: ["city":: c, "district"::c.q, "households"::household];
	}

	action receive_message (string id, string mes) {
		write "Player " + id + " sent the message: " + mes;
	}

	action receive_experiment (string status, string exp_id) {
		write status + ": " + exp_id;
		do send_message players: unity_player as list mes: ["status"::status];
	}

    action increase_cycle_speed {
    	ask world{
    		if (minimum_cycle_duration < 3.0) {
            	minimum_cycle_duration <- min(minimum_cycle_duration * 2, 3.0);
            	write "Increased cycle duration (slower speed): " + minimum_cycle_duration;
        	}else {
            	write "Maximum cycle duration reached.";
    		}
        }
    }

    action slow_down_cycle_speed {
    	ask world{
    		if (minimum_cycle_duration > 0.025) {
            	minimum_cycle_duration <- max(minimum_cycle_duration / 2, 0.025);
            	write "Decreased cycle duration (faster speed): " + minimum_cycle_duration;
        	} else {
            	write "Minimum cycle duration reached.";
        	}
    	}

    }

	action test {
		write "test";
	}
}

species unity_player parent: abstract_unity_player;

experiment SimpleMessage type: gui ;

experiment unity_xp parent: SimpleMessage autorun: false type: unity {
	float minimum_cycle_duration <- 0.5;

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