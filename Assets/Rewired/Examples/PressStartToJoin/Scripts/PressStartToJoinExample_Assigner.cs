// Copyright (c) 2017 Augie R. Maddox, Guavaman Enterprises. All rights reserved.

/* To recreate the same Rewired Input Manager settings as shown in this demo:
 * 1. Create "JoinGame" Action.
 * 2. Create Map Categories "Assignment" and "Default".
 * 3. Create Joystick and Keyboard Maps in the "Assignment" category that map "JoinGame"
 *    to some buttons/keys such as "Start" on in the Dual Analog Gamepad Template and "Enter" in the Keyboard Map.
 * 4. Create Joystick and Keyboard Maps in the "Default" category.
 * 5. Assign the "Assignment" maps to each Player (assign the Keyboard Map to only 1 Player unless you've
 *    created multiple in different layouts for each Player to use). Ensure they are set to be enabled on start.
 * 6. Assign the "Default" maps to each Player setting them to be disabled on start.
 * 7. Leave joystick auto-assignment enabled.
 */

namespace Rewired.Demos {

    using UnityEngine;
    using System.Collections.Generic;
    using Rewired;

    [AddComponentMenu("")]
    public class PressStartToJoinExample_Assigner : MonoBehaviour {

        // Static

        private static PressStartToJoinExample_Assigner instance;

        public static Rewired.Player GetRewiredPlayer(int gamePlayerId) {
            if(!Rewired.ReInput.isReady) return null;
            if(instance == null) {
                Debug.LogError("Not initialized. Do you have a PressStartToJoinPlayerSelector in your scehe?");
                return null;
            }
            for(int i = 0; i < instance.playerMap.Count; i++) {
                if(instance.playerMap[i].gamePlayerId == gamePlayerId) return ReInput.players.GetPlayer(instance.playerMap[i].rewiredPlayerId);
            }
            return null;
        }

        // Instance

        public int maxPlayers = 4;

        private List<PlayerMap> playerMap; // Maps Rewired Player ids to game player ids
        private int gamePlayerIdCounter = 0;

        void Awake() {
            playerMap = new List<PlayerMap>();
            instance = this; // set up the singleton
        }

        void Update() {

            // Watch for JoinGame action in each Player
            for(int i = 0; i < ReInput.players.playerCount; i++) {
                if(ReInput.players.GetPlayer(i).GetButtonDown("JoinGame")) {
                    AssignNextPlayer(i);
                }
            }
        }

        void AssignNextPlayer(int rewiredPlayerId) {
            if(playerMap.Count >= maxPlayers) {
                Debug.LogError("Max player limit already reached!");
                return;
            }

            int gamePlayerId = GetNextGamePlayerId();

            // Add the Rewired Player as the next open game player slot
            playerMap.Add(new PlayerMap(rewiredPlayerId, gamePlayerId));

            Player rewiredPlayer = ReInput.players.GetPlayer(rewiredPlayerId);

            // Disable the Assignment map category in Player so no more JoinGame Actions return
            rewiredPlayer.controllers.maps.SetMapsEnabled(false, "Assignment");

            // Enable UI control for this Player now that he has joined
            rewiredPlayer.controllers.maps.SetMapsEnabled(true, "Default");

            Debug.Log("Added Rewired Player id " + rewiredPlayerId + " to game player " + gamePlayerId);
        }

        private int GetNextGamePlayerId() {
            return gamePlayerIdCounter++;
        }

        // This class is used to map the Rewired Player Id to your game player id
        private class PlayerMap {
            public int rewiredPlayerId;
            public int gamePlayerId;

            public PlayerMap(int rewiredPlayerId, int gamePlayerId) {
                this.rewiredPlayerId = rewiredPlayerId;
                this.gamePlayerId = gamePlayerId;
            }
        }
    }
}