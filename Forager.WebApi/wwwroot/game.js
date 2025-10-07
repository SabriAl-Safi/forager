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
    updateBoard(gameState.cells);
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
            cellDiv.id = `${row * size + col}`;
            boardDiv.appendChild(cellDiv);
        }
    }
}

function updateBoard(cells) {
    // Update cells
    for (let idx = 0; idx < cells.length; idx++) {
        const cell = cells[idx];
        const cellDiv = document.getElementById(`${cell.row * gameSettings.fieldSize + cell.col}`);
        cellDiv.innerHTML = '';
        const isClickable = (cell.state === 0 || cell.state === 5);
        cellDiv.className = isClickable ? 'shroom-cell' : 'grass-cell';
        const img = document.createElement('img');
        const fileName =
            cell.state === 0 ? `${cell.ctr % 10}.jpg` :
            cell.state === 4 ? 'hole.jpg' :
            cell.state === 1 ? 'forager.jpg' :
            cell.state === 5 ? 'flaghole.jpg' :
            'grass.jpeg';
        img.src = `/images/${fileName}`;
        img.alt = cell.image;
        img.style.width = "100%";
        img.style.height = "100%";
        cellDiv.appendChild(img);

        if (isClickable) {
            cellDiv.addEventListener('click', () => onCellClick(cell.row, cell.col));
        }
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