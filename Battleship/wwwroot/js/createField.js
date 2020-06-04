let fieldSideLength = 10;
let shipMap = new Map();
let lastShipHeadsPositions = new Map();
let field = document.getElementsByClassName("fieldContainer")[0];
let shipArr = [];

function CreateShipViewModel(size) {
    this.Size = size,
        this.Cells = []
}

function CreateShipCellViewModel(line, column) {
    this.LineNo = line,
        this.ColumnNo = column
}

function drawField() {
    let table = document.createElement("table");

    for (let line = 0; line < fieldSideLength; line++) {
        var tr = document.createElement("tr");

        for (let col = 0; col < fieldSideLength; col++) {
            let td = document.createElement("td");

            let div = document.createElement("div");
            div.className = "cell freeCell";
            div.dataset.line = line;
            div.dataset.column = col;

            td.appendChild(div);
            tr.appendChild(td);
        }
        table.appendChild(tr);
    }
    field.appendChild(table);
}

function drawShipsPanel() {
    var shipsPanel = document.getElementById("shipsPanel");

    var fourDeckShipsCount = 1;
    var threeDeckShipsCount = 2;
    var twoDeckShipsCount = 3;
    var singleDeckShipsCount = 4;

    for (let i = 1; i <= fourDeckShipsCount; i++) {
        let shipDiv = document.createElement("div");
        shipDiv.id = "ship4" + i;
        shipDiv.className = "ship fourdeck";
        shipDiv.dataset.orientation = "horizontal";
        shipDiv.dataset.length = 4;
        shipArr.push(shipDiv);
        shipsPanel.appendChild(shipDiv)
    }

    for (let i = 1; i <= threeDeckShipsCount; i++) {
        let shipDiv = document.createElement("div");
        shipDiv.id = "ship3" + i;
        shipDiv.className = "ship threedeck";
        shipDiv.dataset.orientation = "horizontal";
        shipDiv.dataset.length = 3;
        shipArr.push(shipDiv);
        shipsPanel.appendChild(shipDiv)
    }

    for (let i = 1; i <= twoDeckShipsCount; i++) {
        let shipDiv = document.createElement("div");
        shipDiv.id = "ship2" + i;
        shipDiv.className = "ship doubledeck";
        shipDiv.dataset.orientation = "horizontal";
        shipDiv.dataset.length = 2;
        shipArr.push(shipDiv);
        shipsPanel.appendChild(shipDiv)
    }

    for (let i = 1; i <= singleDeckShipsCount; i++) {
        let shipDiv = document.createElement("div");
        shipDiv.id = "ship1" + i;
        shipDiv.className = "ship singledeck";
        shipDiv.dataset.orientation = "horizontal";
        shipDiv.dataset.length = 1;
        shipArr.push(shipDiv);
        shipsPanel.appendChild(shipDiv)
    }

}

function setShipMovementListeners() {
    shipArr.forEach(ship => ship.onmousedown = function (event) {
        let startPositionLeft = ship.getBoundingClientRect().left;
        let startPositionTop = ship.getBoundingClientRect().top;

        if (shipMap.has(ship.id)) {
            let lastPosition = lastShipHeadsPositions.get(ship.id);
            resetOccupiedCells(lastPosition.leftTopLine, lastPosition.leftTopColumn, ship);
        }

        let shiftX = event.clientX - startPositionLeft;
        let shiftY = event.clientY - startPositionTop;

        ship.style.position = 'absolute';
        ship.style.zIndex = 1000;
        document.body.append(ship);

        moveAt(event.pageX, event.pageY);     

        ship.onmouseup = onMouseUp;
        document.addEventListener('mousemove', onMouseMove);

        ship.ondragstart = function () {
            return false;
        };

        function moveAt(pageX, pageY) {
            ship.style.left = pageX - shiftX + 'px';
            ship.style.top = pageY - shiftY + 'px';
        }

        function onMouseMove(event) {
            moveAt(event.pageX, event.pageY);
        }

        function onMouseUp(event) {
            if (event.which == 3) {
                rotateShip();
            }

            let downElem = getCellBelow();  
            let isInvalidlyPlaced = checkPositionValidity(downElem);
            
            if (isInvalidlyPlaced) {
                ship.classList.add("invalidlyPlaced");

                if (shipMap.has(ship.id)) {
                    shipMap.delete(ship.id);
                }
            }
            else {
                if (ship.classList.contains("invalidlyPlaced")) {
                    ship.classList.remove("invalidlyPlaced");
                }
                let line = parseInt(downElem.dataset.line);
                let column = parseInt(downElem.dataset.column);
                let shipLength = parseInt(ship.dataset.length);
                let shipVM = new CreateShipViewModel(shipLength);

                if (ship.dataset.orientation == "horizontal") {
                    for (let i = 0; i < shipLength; i++) {
                        shipVM.Cells.push(new CreateShipCellViewModel(line, column + i));
                    }
                }
                else {
                    for (let i = 0; i < shipLength; i++) {
                        shipVM.Cells.push(new CreateShipCellViewModel(line + i, column));
                    }
                }
                shipMap.set(ship.id, shipVM);
                setOccupiedCells(downElem.dataset.line, downElem.dataset.column, ship);
            }
            document.removeEventListener('mousemove', onMouseMove);
            ship.onmouseup = null;

            function checkPositionValidity(downElem) {
                let isInvalidlyPlaced = false;

                // If ship head is placed not on cell, set isInvalidlyPlaced True
                if (downElem == null) {
                    ship.style.left = startPositionLeft;
                    ship.style.top = startPositionTop;
                    isInvalidlyPlaced = true;
                }
                else {
                    // Align the head of the ship in the upper left corner of the cell
                    let downElemLeft = downElem.getBoundingClientRect().left;
                    let downElemTop = downElem.getBoundingClientRect().top;
                    ship.style.left = downElemLeft + 'px';
                    ship.style.top = downElemTop + 'px';

                    let shipLength = parseInt(ship.dataset.length);
                    let shipWidth = 1;
                    let shipHeight = 1;
                    let headColumn = parseInt(downElem.dataset.column);
                    let headLine = parseInt(downElem.dataset.line);

                    // Check if out of bounds
                    if (ship.dataset.orientation == "horizontal") {
                        if (headColumn + shipLength > fieldSideLength) {
                            isInvalidlyPlaced = true;
                        }
                        shipWidth = shipLength;
                    }
                    else {
                        if (headLine + shipLength > fieldSideLength) {
                            isInvalidlyPlaced = true;
                        }
                        shipHeight = shipLength;
                    }

                    if (!isInvalidlyPlaced) {
                        // Check if placed on occupied or neighbouring cells
                        for (let columnShift = 0; columnShift < shipWidth; columnShift++) {
                            for (let lineShift = 0; lineShift < shipHeight; lineShift++) {
                                let cell = document.querySelector(`[data-line='${headLine + lineShift}'][data-column='${headColumn + columnShift}']`);
                                if (cell.classList.contains("unavailableAsNeighbouringCell")) {
                                    isInvalidlyPlaced = true;
                                    break;
                                }
                            }
                        }
                    }
                }
                return isInvalidlyPlaced;
            }

            function getCellBelow() {
                let left = ship.getBoundingClientRect().left;
                let top = ship.getBoundingClientRect().top;

                ship.hidden = true;
                let elemBelow = document.elementFromPoint(left, top);
                let downElem = elemBelow.closest('.cell');
                ship.hidden = false;
                return downElem;
            }
        }

        function rotateShip() {
            let offsetWidth = ship.offsetWidth;
            ship.style.width = ship.offsetHeight + 'px';
            ship.style.height = offsetWidth + 'px';
            ship.dataset.orientation = ship.dataset.orientation == "horizontal" ? "vertical" : "horizontal";
            document.onmousemove = null;
        }
    });
}

function setOccupiedCells(leftTopLine, leftTopColumn, ship) {
    handleOccupiedCells(leftTopLine, leftTopColumn, ship.dataset.length, ship.dataset.orientation, true);
    lastShipHeadsPositions.set(ship.id, {
        leftTopLine: leftTopLine,
        leftTopColumn: leftTopColumn,
        shipLength: ship.dataset.length,
        orientation: ship.dataset.orientation
    });
}

function resetOccupiedCells(leftTopLine, leftTopColumn, ship) {
    handleOccupiedCells(leftTopLine, leftTopColumn, ship.dataset.length, ship.dataset.orientation, false);
    lastShipHeadsPositions.delete(ship.id);
}

function handleOccupiedCells(leftTopLine, leftTopColumn, shipLength, orientation, toSet) {
    leftTopLine = parseInt(leftTopLine) -1;
    leftTopColumn = parseInt(leftTopColumn) -1;
    shipLength = parseInt(shipLength);

    let width = 1;
    let height = 1;
    orientation == "horizontal" ? width = shipLength : height = shipLength;

    for (let colShift = 0; colShift < width + 2; colShift++) {
        for (let lineShift = 0; lineShift < height + 2; lineShift++) {
            let cell = field.querySelector(`[data-line='${leftTopLine + lineShift}'][data-column='${leftTopColumn + colShift}']`);
            if (cell != null)
            {
                if (toSet == true) {
                    if (!cell.classList.contains("unavailableAsNeighbouringCell")) {
                        cell.classList.add("unavailableAsNeighbouringCell");
                    }
                    else {
                        cell.classList.add("mutuallyNeighboured");
                    }
                }
                else {
                    if (cell.classList.contains("mutuallyNeighboured")) {
                        cell.classList.remove("mutuallyNeighboured")
                    }
                    else {
                        cell.classList.remove("unavailableAsNeighbouringCell");
                    }
                }                
            }
        }
    }
}

function onSendClick(e) {
    if (shipMap.size != 10) {
        alert("Place all ships in correct positions!");
        return;
    }
    let ships = [...shipMap.values()];
    var model = JSON.stringify(ships);

    $.ajax({
        type: "POST",
        url: '/Game/CreateField',
        data: model,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (arg) {
            window.location.href = arg.redirectToUrl;
        }
    });
};

document.body.addEventListener("contextmenu", function (evt) { evt.preventDefault(); return false; });
drawField();
drawShipsPanel();
setShipMovementListeners();
document.getElementById("createField").addEventListener("click", onSendClick);