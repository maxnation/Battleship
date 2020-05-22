bus.on('initialize-game', function (e, data) {
    drawField("playerField", data, false);
    drawField("rivalField", data, true);

    setUsername("playerUsernameParagraph", data.player.username);
    setUsername("rivalUsernameParagraph", data.rival.username);

    if (data.nextTurnPlayerId == data.player.playerId) {
        document.getElementById("statusBar").innerText = "It's your turn!";
    }
    else {
        document.getElementById("statusBar").innerText = "Your rival makes a move...";
    }

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
                    console.log('line: ' + event.srcElement.dataset.line + ' col: '
                        + event.srcElement.dataset.column + ' state: ' + event.srcElement.className
                        + ' playerId: ' + data.player.playerId + ' rivalId: ' + data.rival.playerId);
                });

            td.appendChild(div);
            tr.appendChild(td);
        }
        table.appendChild(tr);
    }
    fieldContainer.appendChild(table);
    console.log(data);
    if (isRivalField) {
        console.log(data.rival)
        fillField(fieldContainerId, data.rival.field.cells, isRivalField);
    }
    else {
        console.log(data.player)
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
    console.log("Email: " + username)
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