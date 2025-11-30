import { Server } from "socket.io";

const io = new Server(3000, {
    cors: { origin: "*" }
});

console.log("Matchmaking server running on port 3000");

let waitingPlayer = null; // store single waiting player for 1v1 matchmaking
let roomIdCounter = 1;

io.on("connection", (socket) => {
    console.log("Player connected:", socket.id);

    // Player sends match data: Deck + Team
    socket.on("findMatch", (playerData) => {
        console.log("findMatch from:", socket.id, playerData);

        socket.playerData = playerData; // store player's match info on socket

        if (!waitingPlayer) {
            // No one is waiting → this player becomes the waiting player
            waitingPlayer = socket;
            socket.emit("waiting"); // optional
            console.log("Player waiting:", socket.id);
        } else {
            // Found a match!
            const player1 = waitingPlayer;
            const player2 = socket;

            const roomId = roomIdCounter++;
            const matchId = Date.now();

            // Create room
            player1.join(roomId.toString());
            player2.join(roomId.toString());

            // Assign enemy IDs (you can replace this with real user IDs)
            const enemyId1 = player2.playerData.name;
            const enemyId2 = player1.playerData.name;

            // Randomly pick who starts
            const activePlayerSocket = Math.random() < 0.5 ? player1 : player2;

            // Send matchFound to BOTH PLAYERS
            const matchPayload1 = {
                matchId,
                roomId,
                enemyName: enemyId1,
                enemyTeam: player2.playerData.team,
                enemyDeck: player2.playerData.deck,
                activePlayer: activePlayerSocket.playerData.name
            };

            const matchPayload2 = {
                matchId,
                roomId,
                enemyId: enemyId2,
                enemyTeam: player1.playerData.team,
                enemyDeck: player1.playerData.deck,
                activePlayer: activePlayerSocket.playerData.name
            };

            console.log("Match created:", { roomId, matchId });

            player1.emit("matchFound", matchPayload1);
            player2.emit("matchFound", matchPayload2);

            // Reset waiting player
            waitingPlayer = null;
        }
    });

    socket.on("leaveMatch", () => {
    console.log("Player left matchmaking:", socket.id);

    // If this player was waiting, remove from waiting
    if (waitingPlayer && waitingPlayer.id === socket.id) {
        waitingPlayer = null;
    }

    // Optional: disconnect socket
    socket.disconnect();
});
    socket.on("disconnect", () => {
        console.log("Player disconnected:", socket.id);

        if (waitingPlayer === socket) {
            waitingPlayer = null;
        }
    });
});
