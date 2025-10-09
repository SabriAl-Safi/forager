let currentGameId = null;
let gameSettings = {
    fieldSize: 10,
    numShrooms: 10
};
const apiBase = '/api/game';

function showModal(id) {
    document.getElementById(id).style.display = 'block';
}

function hideModal(id) {
    document.getElementById(id).style.display = 'none';
}

function showSettings() {
    showModal('settingsModal');
    document.getElementById('fieldSize').value = gameSettings.fieldSize;
    document.getElementById('numShrooms').value = gameSettings.numShrooms;
}

function saveSettings() {
    const size = parseInt(document.getElementById('fieldSize').value);
    if (size >= 3 && size <= 15) {
        gameSettings.fieldSize = size;
    } else {
        showMessage('Board size must be between 3 and 15', 'error');
        return;
    }

    const numShrooms = parseInt(document.getElementById('numShrooms').value);
    if (numShrooms >= 3 && numShrooms <= 15) {
        gameSettings.numShrooms = numShrooms;
        showMessage('Settings saved! Start a new game to apply changes.', 'success');
    } else {
        showMessage('Number of mushrooms must be between 3 and 15', 'error');
        return;
    }

    hideModal('settingsModal');
}


async function startNewGame() {
    try {
        const response = await fetch(`${apiBase}/new`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({
                FieldSize: gameSettings.fieldSize,
                NumShrooms: gameSettings.numShrooms
            })
        });

        if (response.ok) {
            const data = await response.json();
            currentGameId = data.gameId;
            initialiseBoard(gameSettings.fieldSize);
            updateGameDisplay(data.state);
        } else {
            showMessage(`${response.status}`, 'error');
        }
    } catch (error) {
        showMessage(`Error: ${error.message}`, 'error');
    }
}

window.onload = function () { startNewGame(); }

function updateGameDisplay(gameState) {
    document.getElementById('numShroomsFound').textContent = gameState.numShroomsFound;
    document.getElementById('currentDistance').textContent = gameState.currentDistance;
    document.getElementById('targetDistance').textContent = gameState.targetDistance;
    updateBoard(gameState.cells);

    if (!gameState.isFinished) {
        return;
    }

    if (gameState.numShroomsFound < gameSettings.numShrooms) {
        document.getElementById('tooFewModal').style.display = 'block';
        return;
    }

    if (gameState.currentDistance > gameState.targetDistance) {
        document.getElementById('tooFarModal').style.display = 'block';
        return;
    }

    document.getElementById('winModal').style.display = 'block';
}

function initialiseBoard(size) {
    const boardDiv = document.getElementById('boardDisplay');

    // Set grid template based on board size
    boardDiv.style.gridTemplateColumns = `repeat(${size}, 1fr)`;
    boardDiv.style.gridTemplateRows = `repeat(${size}, 1fr)`;

    // Clear existing cells
    boardDiv.innerHTML = '';

    for (let row = 0; row < size; row++) {
        for (let col = 0; col < size; col++) {
            const cellDiv = document.createElement('div');
            cellDiv.dataset.row = row;
            cellDiv.dataset.col = col;
            cellDiv.id = `${row},${col}`;
            boardDiv.appendChild(cellDiv);
        }
    }
}

function updateBoard(cells) {
    // Update cells
    for (let idx = 0; idx < cells.length; idx++) {
        const cell = cells[idx];
        const cellDiv = document.getElementById(`${cell.row},${cell.col}`);
        const isClickable = (cell.state === 0 || cell.state === 5);
        cellDiv.className = isClickable ? 'shroom-cell' : 'grass-cell';
        const img = document.createElement('img');
        img.src = `/images/${getCellFileName(cell)}`;
        img.style.width = "100%";
        img.style.height = "100%";
        cellDiv.replaceChildren(img);

        cellDiv.removeEventListener('click', onCellClick);
        if (isClickable) {
            cellDiv.addEventListener('click', onCellClick);
        }
    }
}

const getCellFileName = (cell) =>
    cell.state === 0 ? `${cell.ctr % 10}.jpg` :
        cell.state === 4 ? 'hole.jpg' :
            cell.state === 1 ? 'forager.jpg' :
                cell.state === 5 ? 'flaghole.jpg' :
                    cell.isTrodden ? 'troddengrass.jpg' :
                        'grass.jpg';

async function onCellClick(e) {
    try {
        const response = await fetch(`${apiBase}/${currentGameId}/action`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ Row: e.currentTarget.dataset.row, Col: e.currentTarget.dataset.col })
        });

        if (response.ok) {
            const data = await response.json();
            updateGameDisplay(data.state);
        } else {
            showMessage(`${response.status}`, 'error');
        }
    } catch (error) {
        showMessage(`Error: ${error.message}`, 'error');
    }
}

function showMessage(message, type) {
    const messagesDiv = document.getElementById('messages');
    const messageContent = document.getElementById('messageContent');

    messageContent.innerHTML = `<p class="${type}">${message}</p>`;
    messagesDiv.style.display = 'block';

    // Hide after 5 seconds
    setTimeout(() => {
        messagesDiv.style.display = 'none';
    }, 5000);
}

async function resetGame() {
    try {
        const response = await fetch(`${apiBase}/${currentGameId}/reset`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' }
        });

        if (response.ok) {
            const data = await response.json();
            updateGameDisplay(data.state);
        } else {
            showMessage(`${response.status}`, 'error');
        }
    } catch (error) {
        showMessage(`Error: ${error.message}`, 'error');
    }
}