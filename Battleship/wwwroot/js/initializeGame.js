bus.on('initialize-game', function (e, data) {
    drawField("playerField", data, false);
    drawField("rivalField", data, true);

    setUsername("playerUsernameParagraph", data.player.username);
    setUsername("rivalUsernameParagraph", data.rival.username);
    this.switchControl(data.nextTurnPlayerId);
 
    bus.trigger('signalr-join-game', { gameId: data.gameId, username: data.player.username });
});

function drawField(fieldContainerId, data, isRivalField) {
    var lines = 10;
    var columns = 10;

    var fieldContainer = document.getElementById(fieldContainerId);
    var table = document.createElement("table");

    for (var line = 0; line < lines; line++) {
        var tr = document.createElement("tr");
        for (var col = 0; col < columns; col++) {
            var td = document.createElement("td");
            var div = document.createElement("div");
            div.className = "freeCell";
            div.dataset.line = line;
            div.dataset.column = col;

            div.addEventListener('click', event => {
                var stepVM = {
                    "playerId": data.player.playerId,
                    "rivalId": data.rival.playerId,
                    "lineNo": parseInt(event.srcElement.dataset.line),
                    "columnNo": parseInt(event.srcElement.dataset.column),
                    "isHit" : false
                }
                hubConnection.invoke("MakeStep", stepVM, data.gameId);
                });
            td.appendChild(div);
            tr.appendChild(td);
        }
        table.appendChild(tr);
    }
    fieldContainer.appendChild(table);

    if (isRivalField) {
        fillField(fieldContainerId, data.rival.field.cells, isRivalField);
    }
    else {
        fillField(fieldContainerId, data.player.field.cells, isRivalField);
    }
}

function fillField(fieldContainerId, fieldData, isRivalField) {
    for (let i = 0; i < fieldData.length; i++) {
        let cell = document.querySelector('#' + fieldContainerId + ' [data-line="' + fieldData[i].lineNo + '"][data-column="' + fieldData[i].columnNo + '"]');
        setCellClass(cell, fieldData[i].state, isRivalField);
    }
}

function setUsername(usernameParagraphId, username) {
    document.getElementById(usernameParagraphId).innerText = username;
}

function setCellClass(cell, state, isRivalField) {
    switch (state) {
        case "Free":
            cell.className = "freeCell";
            break;
        case "Occupied":
            if (isRivalField) {
                cell.className = "freeCell";
            }
            else {
                cell.className = "occupiedCell";
            }
            break;    
        case "Miss":
            cell.className = "missCell";
            break;
        case "Hit":
            if (isRivalField) {
                cell.className = "rivalHitCell";
            }
            else {
                cell.className = "playerHitCell";
            }            break;
    }
}

function switchControl(nextPlayerId, extraMessage) {
    let message;
    if (nextPlayerId == PLAYER_ID) {
         message = extraMessage == undefined ? "It's your turn!" : extraMessage
        unfreezeRivalField();
    }
    else {
        message = extraMessage == undefined ? "Your rival makes a move..." : extraMessage;
        freezeRivalField();
    }
    setStatusBarMessage(message);
}

function freezeRivalField() {
    document.getElementById("rivalField").style.pointerEvents = 'none';
}

function unfreezeRivalField() {
    document.getElementById("rivalField").style.pointerEvents = 'visible'; 
}

function setStatusBarMessage(message) {
    document.getElementById("statusBar").innerText = message == undefined ? "It's your turn!" : message;
}

function receiveStep(stepVM) {
    let newState;
    if (stepVM.isHit) {
        newState = "Hit";
    }
    else {
        newState = "Miss";
    }

    if (stepVM.playerId == PLAYER_ID) {
        let rivalCell = document.querySelector('#rivalField' + ' [data-line="' + stepVM.lineNo + '"][data-column="' + stepVM.columnNo + '"]');
        setCellClass(rivalCell, newState, true);

        if (newState == "Hit") {
            setStatusBarMessage("Hit! You go again!");
        }
        else if (newState == "Miss") {
            switchControl(RIVAL_ID, "You missed! Your rival makes a move...");
        }
    }
    else if (stepVM.playerId == RIVAL_ID) {
        let playerCell = document.querySelector('#playerField' + ' [data-line="' + stepVM.lineNo + '"][data-column="' + stepVM.columnNo + '"]');
        setCellClass(playerCell, newState, false);
        if (newState != "Hit") {
            switchControl(PLAYER_ID, "It's your turn");
        }
    }
}