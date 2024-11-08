//СКИБИДИ ТУАЛЕТ ------------------------------------
var vehicleswanted = [];
var attachedObjects = [];


global.localplayer.setVisible(true, false);

mp.events.add('attachObject', attachObject);
mp.events.add('detachObject', function (player) {
    try {
        if (player && mp.players.exists(player)) {
            if (attachedObjects[player.id] != undefined) attachedObjects[player.id].destroy();
            attachedObjects[player.id] = undefined;
        }
    } catch (e) { } 
});

function attachObject(player) {
    try {
        if (player && mp.players.exists(player)) {
            if (attachedObjects[player.id] != undefined) attachedObjects[player.id].destroy();

            if (player.getVariable('attachedObject') == null) return;
            let data = JSON.parse(player.getVariable('attachedObject'));
            let boneID = player.getBoneIndex(data.Bone);
            var object = mp.objects.new(data.Model, player.position,
                {
                    rotation: new mp.Vector3(0, 0, 0),
                    alpha: 255,
                    dimension: player.dimension
                });

            waitEntity(object).then(() => {
                object.attachTo(player.handle, boneID, data.PosOffset.x, data.PosOffset.y, data.PosOffset.z, data.RotOffset.x, data.RotOffset.y, data.RotOffset.z, true, true, false, false, 0, true);
                attachedObjects[player.id] = object;
            });
        }
    } catch (e) { } 
}
function waitEntity(entity){
	return new Promise(resolve => {
		let wait = setInterval(() => {
			if(mp.game.entity.isAnEntity(entity.handle)){
				clearInterval(wait);
				resolve();
			}
		}, 1);
	});
}

mp._events.add("playerQuit", (player) => {
try {
if (attachedObjects[player.id] != undefined) {
	attachedObjects[player.id].destroy();
	attachedObjects[player.id] = undefined;
}
} catch (e) { }
});
mp.events.add('entityStreamOut', function (entity) {
try {
if (entity.type != 'player') return;
if (attachedObjects[entity.id] != undefined) {
	attachedObjects[entity.id].destroy();
	attachedObjects[entity.id] = undefined;
}
} catch (e) { } 
});
//СКИБИДИ ТУАЛЕТ ------------------------------------


var vehicleswanted = [];

global.localplayer.setVisible(true, false);

gm.events.add("payday", (exp, level, cash) => {
	mp.events.call('client.charStore.EXP', exp);
	mp.events.call('client.charStore.LVL', level);
	mp.gui.emmit(`window.updateLevel()`);
	if (cash > 0 && !global.menuCheck())
		mp.gui.emmit(`window.PayDay(${cash})`);
	/*if (wasexp === 999999) {
		wasexp = 0;
		nextexp = 0;
	}
    if(!mp.game.graphics.hasHudScaleformLoaded(19)) 
	{
		mp.game.graphics.requestHudScaleform(19);
		while (!mp.game.graphics.hasHudScaleformLoaded(19)) mp.game.wait(0);
	}
	mp.game.graphics.pushScaleformMovieFunctionFromHudComponent(19, "SET_COLOUR");
	mp.game.graphics.pushScaleformMovieFunctionParameterInt(116);
	mp.game.graphics.pushScaleformMovieFunctionParameterInt(123);
	mp.game.graphics.popScaleformMovieFunctionVoid();
	mp.game.graphics.pushScaleformMovieFunctionFromHudComponent(19, "SET_RANK_SCORES");
	mp.game.graphics.pushScaleformMovieFunctionParameterInt(0); 
	mp.game.graphics.pushScaleformMovieFunctionParameterInt(reqexp);
	mp.game.graphics.pushScaleformMovieFunctionParameterInt(wasexp);
	mp.game.graphics.pushScaleformMovieFunctionParameterInt(nextexp);
	mp.game.graphics.pushScaleformMovieFunctionParameterInt(level);
	mp.game.graphics.popScaleformMovieFunctionVoid();
	mp.game.graphics.pushScaleformMovieFunctionFromHudComponent(19, "OVERRIDE_ANIMATION_SPEED");
	mp.game.graphics.pushScaleformMovieFunctionParameterInt(5000);
	mp.game.graphics.popScaleformMovieFunctionVoid();
	mp.game.audio.playSoundFrontend(-1, "RANK_UP", "HUD_AWARDS", true);*/
});

gm.events.add("pedStreamIn", (entity) => {
	//entity.taskLookAt(global.localplayer.handle, -1, 2048, 3);

	if (entity.PedId == 1 && entity.getModel() == mp.game.joaat("s_f_y_ranger_01")) { // Ketrin Kellerman
		entity.taskPlayAnim("anim@mp_player_intuppersalute", "idle_a", 8.0, 1.0, -1, 49, 0.0, false, false, false);
	}
});

gm.events.add('setVehiclesWanted', function (numbers) {
	vehicleswanted[numbers] = true;
});

gm.events.add('clearVehicleWanted', function (numbers) {
	if (vehicleswanted[numbers] !== undefined) delete vehicleswanted[numbers];
});

const PlayerHash = mp.game.joaat("PLAYER");
const NonFriendlyHash = mp.game.joaat("FRIENDLY_PLAYER");
const FriendlyHash = mp.game.joaat("NON_FRIENDLY_PLAYER");

global.localplayer.setRelationshipGroupHash(PlayerHash);

mp.game.ped.addRelationshipGroup("FRIENDLY_PLAYER", 0);
mp.game.ped.addRelationshipGroup("NON_FRIENDLY_PLAYER", 0);

mp.game.ped.setRelationshipBetweenGroups(0, PlayerHash, FriendlyHash);

mp.game.ped.setRelationshipBetweenGroups(5, PlayerHash, NonFriendlyHash);
mp.game.ped.setRelationshipBetweenGroups(5, NonFriendlyHash, PlayerHash);

global.dmgdisabled = false;

gm.events.add('disabledmg', function (player, toggle) {
	try {
		if (player !== undefined && player != null) {
			if (global.localplayer == player) {
				global.dmgdisabled = toggle;
				if (toggle) {
					mp.players.forEachInStreamRange((entity) => {
						if (entity != global.localplayer) entity.setRelationshipGroupHash(FriendlyHash);
					});
				} else {
					mp.players.forEachInStreamRange((entity) => {
						if (entity != global.localplayer) entity.setRelationshipGroupHash(NonFriendlyHash);
					});
				}
			} else {
				if (toggle) {
					mp.players.forEachInStreamRange((entity) => {
						if (entity == player) entity.setRelationshipGroupHash(FriendlyHash);
					});
				} else {
					mp.players.forEachInStreamRange((entity) => {
						if (entity == player) entity.setRelationshipGroupHash(NonFriendlyHash);
					});
				}
			}
		}
	} catch (e) {
		mp.events.callRemote("client_trycatch", "player/basicsync", "disabledmg", e.toString());
	}
});

global.playersBlips = {};

gm.events.add("render", () => {
	mp.players.forEachInStreamRange((player) => {

		if (mp.players.local === player) {
			return;
		}

		let needBlip = player['IS_MASK'] == false;

		if(player.blipId != undefined && !nativeInvoke("DOES_BLIP_EXIST", player.blipId)){
			delete player.blipId;
		}

		if (needBlip) {
			if (player.blipId == undefined) {
				gm.createPlayerBlip(player)
			}
		} else {
			deletePlayerBlip(player)
		}
	});
})

gm.events.add("playerStreamIn", (entity) => {
	if (entity['DMGDisable']) entity.setRelationshipGroupHash(FriendlyHash);
	else entity.setRelationshipGroupHash(NonFriendlyHash);

	if (entity['INVISIBLE']) {
		entity.setVisible(false, false);
		entity.setAlpha(0);
	} else if (entity["REDNAME"])
		entity.setAlpha(100);
	else
		entity.setAlpha(255);
});

const deletePlayerBlip = (player) => {
	if (player) {
		if (player.blipId) {
			mp.game.ui.removeBlip(player.blipId);
			delete player.blipId;
		}
	}
}

gm.createPlayerBlip = (player) => {
	if (player && mp.players.exists(player)) {
	  if (player['ALVL']){
		return;
	  }
  
	  let color = 0; // цвет для всех
  
	  //if (global.fractionId == player['fraction']) {
	  if ((global.fractionId !== 0 && global.fractionId === player['fraction']) || (global.organizationId !== 0 && global.organizationId === player['organization'])) {
		color = 2; // цвет для тиммейтов
	  }
  
	  if (player['InDeath']) {
		color = 70; // цвет для мертвых
	  }
  
	  if (player.blipId) {
		nativeInvoke("SET_BLIP_COLOUR", player.blipId, color);
	  } else {
		let playerBlip = nativeInvoke("ADD_BLIP_FOR_ENTITY", player.handle);
		nativeInvoke("SET_BLIP_COLOUR", playerBlip, color);
		nativeInvoke("SET_BLIP_CATEGORY", playerBlip, 7);
		nativeInvoke("_SET_BLIP_SHOW_HEADING_INDICATOR", playerBlip, true);
		player.blipId = playerBlip;
	  }
  
	}
  }

gm.events.add("playerStreamOut", (entity) => {
	deletePlayerBlip(entity);
});

gm.events.add("vehicleStreamIn", (entity) => {
	if ([7, 9].includes(global.fractionId)) {
		const myvehicle = global.localplayer.vehicle;
		if (myvehicle && myvehicle.getPedInSeat(-1) == global.localplayer.handle && myvehicle.getClass() == 18) {
			var vehnum = Natives.GET_VEHICLE_NUMBER_PLATE_TEXT(entity.handle).replace(/\s+/g, '');
			if (vehicleswanted[vehnum] !== undefined) {
				//TODO
				mp.events.call('notify', 3, 9, translateText("Транспортное средство с номерами ") + vehnum + translateText(" было замечено неподалёку!"), 3000);
			}
		}
	}
});