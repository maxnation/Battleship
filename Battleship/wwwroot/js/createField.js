function CreateShipViewModel(size) {
    this.Size = size,
        this.Cells = []
}

function CreateShipCellViewModel(line, column) {
    this.LineNo = line,
        this.ColumnNo = column
}

function drawField() {
    var lines = 10;
    var columns = 10;

    var fieldContainer = document.getElementsByClassName("fieldContainer")[0];
    var table = document.createElement("table");

    for (var line = 0; line < lines; line++) {
        var tr = document.createElement("tr");
        for (var col = 0; col < columns; col++) {
            var td = document.createElement("td");
         
            var div = document.createElement("div");
            div.className = "freeCell";
            div.dataset.line = line;
            div.dataset.column = col;

            td.appendChild(div);
            tr.appendChild(td);
        }
        table.appendChild(tr);
    }
    fieldContainer.appendChild(table);
}

function drawShipsPanel() {
    var shipsPanel = document.getElementById("shipsPanel");

    var fourDeckShipsCount = 1;
    var threeDeckShipsCount = 2;
    var twoDeckShipsCount = 3;
    var singleDeckShipsCount = 4;

    var shipArr = [];

    for (let i = 1; i <= fourDeckShipsCount; i++) {
       let shipDiv = document.createElement("div");
        shipDiv.id = "ship4" + i;
        shipDiv.className = "ship fourdeck";
        shipArr.push(shipDiv);
        shipsPanel.appendChild(shipDiv)
    }
    

    for (let i = 1; i <= threeDeckShipsCount; i++) {
        let shipDiv = document.createElement("div");
        shipDiv.id = "ship3" + i;
        shipDiv.className = "ship threedeck";
        shipArr.push(shipDiv);
        shipsPanel.appendChild(shipDiv)
    }

    for (let i = 1; i <= twoDeckShipsCount; i++) {
        let shipDiv = document.createElement("div");
        shipDiv.id = "ship2" + i;
        shipDiv.className = "ship doubledeck";
        shipArr.push(shipDiv);
        shipsPanel.appendChild(shipDiv)
    }

    for (let i = 1; i <= singleDeckShipsCount; i++) {
        let shipDiv = document.createElement("div");
        shipDiv.id = "ship1" + i;
        shipDiv.className = "ship singledeck";
        shipArr.push(shipDiv);
        shipsPanel.appendChild(shipDiv)
    }
}

drawField();
drawShipsPanel();
var btn = document.getElementById("createField");
btn.addEventListener("click", OnSendClick);


function OnSendClick(e) {

    var ship41 = new CreateShipViewModel(4);
    ship41.Cells.push(new CreateShipCellViewModel(0, 0));
    ship41.Cells.push(new CreateShipCellViewModel(1, 0));
    ship41.Cells.push(new CreateShipCellViewModel(2, 0));
    ship41.Cells.push(new CreateShipCellViewModel(3, 0));

//////////////////////////////

    var ship31 = new CreateShipViewModel(3);
    ship31.Cells.push(new CreateShipCellViewModel(5,0));
    ship31.Cells.push(new CreateShipCellViewModel(6, 0));
    ship31.Cells.push(new CreateShipCellViewModel(7, 0));


    var ship32 = new CreateShipViewModel(3);
    ship32.Cells.push(new CreateShipCellViewModel(3, 5));
    ship32.Cells.push(new CreateShipCellViewModel(3, 6));
    ship32.Cells.push(new CreateShipCellViewModel(3, 7));

//////////////////////////////////

    var ship21 = new CreateShipViewModel(2);
    ship21.Cells.push(new CreateShipCellViewModel(5, 4));
    ship21.Cells.push(new CreateShipCellViewModel(6, 4));

    var ship22 = new CreateShipViewModel(2);
    ship22.Cells.push(new CreateShipCellViewModel(8, 4));
    ship22.Cells.push(new CreateShipCellViewModel(9, 4));

    var ship23 = new CreateShipViewModel(2);
    ship23.Cells.push(new CreateShipCellViewModel(7, 7));
    ship23.Cells.push(new CreateShipCellViewModel(7, 8));

  ////////////////////////////////////////////////

    var ship11 = new CreateShipViewModel(1);
    ship11.Cells.push(new CreateShipCellViewModel(1, 3));

    var ship12 = new CreateShipViewModel(1);
    ship12.Cells.push(new CreateShipCellViewModel(1, 8));

    var ship13 = new CreateShipViewModel(1);
    ship13.Cells.push(new CreateShipCellViewModel(3, 2));

    var ship14 = new CreateShipViewModel(1);
    ship14.Cells.push(new CreateShipCellViewModel(9, 7));

////////////////////////////////////////////////

    var ships = [ship41, ship31, ship32, ship21, ship22, ship23, ship11, ship12, ship13, ship14];
    var model = JSON.stringify(ships);

    $.ajax({
        type: "POST",
        url: '/Game/CreateField',
        data: model,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (arg) { //call successfull
            window.location.href = arg.redirectToUrl;

        },
        error: function (xhr) {
            console.log(xhr);
        }
    });
};