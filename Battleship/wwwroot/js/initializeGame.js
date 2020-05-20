function InitializeGame(data) {
    drawField("playerField", data.player.field.cells, false);
    drawField("rivalField", data.rival.field.cells, true);

    setUsername("playerUsernameParagraph", data.player.username);
    setUsername("rivalUsernameParagraph", data.rival.username);

    if (data.nextturnplayerid == data.player.playerId) {
        document.getElementById("statusBar").innerText = "It's your turn!";
    }
    else {
        document.getElementById("statusBar").innerText = "Your rival makes a move...";
    }
}

function drawField(fieldContainerId, fieldData, isRivalField) {
    var lines = 10;
    var columns = 10;

    var fieldContainer = document.getElementById(fieldContainerId);
    var table = document.createElement("table");

    for (var line = 0; line < lines; line++) {
        var tr = document.createElement("tr");
        for (var col = 0; col < columns; col++) {
            var td = document.createElement("td");
            td.dataset.line = line;
            td.dataset.column = col;
            var div = document.createElement("div");
            div.className = "freeCell";
            td.appendChild(div);
            tr.appendChild(td);
        }
        table.appendChild(tr);
    }
    fieldContainer.appendChild(table);

    fillField(fieldContainerId, fieldData, isRivalField);
}

function fillField(fieldContainerId, fieldData, isRivalField) {
    for (let i = 0; i < fieldData.length; i++) {
        let cell = document.querySelector('#' + fieldContainerId + ' [data-line="' + fieldData[i].lineNo + '"][data-column="' + fieldData[i].columnNo + '"]');
        setCellClass(cell.firstChild, fieldData[i].state, isRivalField);
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