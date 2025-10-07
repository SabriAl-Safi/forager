let currentGameId = null;
let gameSettings = {
    fieldSize: 10,
    numShrooms: 10
};
const apiBase = '/api/game';

function showSettings() {
    document.getElementById('settingsModal').style.display = 'block';
    document.getElementById('fieldSize').value = gameSettings.fieldSize;
    document.getElementById('numShrooms').value = gameSettings.numShrooms;
}

function hideSettings() {
    document.getElementById('settingsModal').style.display = 'none';
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

    hideSettings();
}

const showInstructions = () => document.getElementById('instructionsModal').style.display = 'block';
const hideInstructions = () => document.getElementById('instructionsModal').style.display = 'none';

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
            updateGameDisplay(data.state);
        } else {
            showMessage(`${response.status}`, 'error');
        }
    } catch (error) {
        showMessage(`Error: ${error.message}`, 'error');
    }
}

window.onload = function () { startNewGame(); }

async function endGame() {
    if (!currentGameId) return;

    try {
        await fetch(`${apiBase}/${currentGameId}`, { method: 'DELETE' });
        resetGame();
    } catch (error) {
        showMessage(`Error: ${error.message}`, 'error');
    }
}

function updateGameDisplay(gameState) {
    document.getElementById('numShroomsFound').textContent = gameState.numShroomsFound;
    document.getElementById('currentDistance').textContent = gameState.currentDistance;
    document.getElementById('targetDistance').textContent = gameState.targetDistance;
    renderBoard(gameState.cells);
}

function renderBoard(cells) {
    const boardDiv = document.getElementById('boardDisplay');

    if (!cells || !Array.isArray(cells) || cells.length === 0) {
        boardDiv.innerHTML = '<p>No board data available</p>';
        return;
    }

    const size = Math.sqrt(cells.length);

    // Set grid template based on board size
    boardDiv.style.gridTemplateColumns = `repeat(${size}, 1fr)`;
    boardDiv.style.gridTemplateRows = `repeat(${size}, 1fr)`;

    // Clear existing cells
    boardDiv.innerHTML = '';

    // Create cells
    let shroomCtr = 0;
    for (let idx = 0; idx < cells.length; idx++) {
        const cell = cells[idx];
        const cellDiv = document.createElement('div');
        cellDiv.className = cell.state === 0 ? 'shroom-cell' : 'grass-cell';
        cellDiv.dataset.row = cell.row;
        cellDiv.dataset.col = cell.col;
        const img = document.createElement('img');
        const fileName =
            cell.state === 0 ? `${shroomCtr % 10}.jpg` :
            cell.state === 4 ? 'hole.jpg' :
            cell.state == 1 ? 'forager.jpg' :
            'grass.jpeg';
        img.src = `/images/${fileName}`;
        img.alt = cell.image;
        img.style.width = "100%";
        img.style.height = "100%";
        cellDiv.appendChild(img);

        if (cell.state === 0) {
            // Add click handler for cell interaction
            cellDiv.addEventListener('click', () => onCellClick(cell.row, cell.col));
        }

        if (cell.state != 2) {
            shroomCtr++;
        }

        boardDiv.appendChild(cellDiv);
    }
}

async function onCellClick(row, col) {
    try {
        const response = await fetch(`${apiBase}/${currentGameId}/action`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ Row: row, Col: col })
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

function resetGame() {
    currentGameId = null;
    document.getElementById('startSection').style.display = 'block';
    document.getElementById('gameDisplay').style.display = 'none';
    document.getElementById('gameActions').style.display = 'block';
    showMessage('Game ended', 'success');
}