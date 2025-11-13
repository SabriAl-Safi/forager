let currentGameId = null;
let gameSettings = { fieldSize: 10, numShrooms: 10, pcStone: 10 };
const apiBase = '/api/game';
const setDisp = (id, val) => document.getElementById(id).style.display = val;
const showModal = id => setDisp(id, 'block');
const hideModal = id => setDisp(id, 'none');
const postJson = (url, body) => fetch(url, jsonRequest(body));
function jsonRequest(body) { return { method: 'POST', headers: { 'Content-Type': 'application/json' }, body: JSON.stringify(body) } };

async function startNewGame() {
    const response = await postJson(`${apiBase}/new`, gameSettings);
    if (response.ok) {
        const data = await response.json();
        currentGameId = data.gameId;
        initialiseBoard(gameSettings.fieldSize);
        updateGameDisplay(data.state);
    }
}

window.onload = function () { startNewGame(); }

function initialiseBoard(size) {
    const boardDiv = document.getElementById('boardDisplay');
    boardDiv.style.gridTemplateColumns = `repeat(${size}, 1fr)`;
    boardDiv.style.gridTemplateRows = `repeat(${size}, 1fr)`;
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

function updateGameDisplay(gameState) {
    document.getElementById('numShroomsFound').textContent = gameState.numShroomsFound;
    document.getElementById('currentDistance').textContent = gameState.currentDistance;
    document.getElementById('targetDistance').textContent = gameState.targetDistance;
    updateBoard(gameState.cells);
    if (!gameState.isFinished) {
        return;
    }
    const modal =
        gameState.numShroomsFound < gameSettings.numShrooms ? 'tooFewModal' :
        gameState.currentDistance > gameState.targetDistance ? 'tooFarModal' :
        'winModal';
    showModal(modal);
}

function updateBoard(cells) {
    for (let idx = 0; idx < cells.length; idx++) {
        const cell = cells[idx];
        const cellDiv = document.getElementById(`${cell.row},${cell.col}`);
        const isClickable = (cell.state === 0 || cell.state === 5);
        cellDiv.className = isClickable ? 'shroom-cell' : 'grass-cell';

        let img = cellDiv.querySelector('img');
        if (!img) {
            img = document.createElement('img');
            img.style.width = "100%";
            img.style.height = "100%";
            cellDiv.appendChild(img);
        }

        img.src = `/images/${getCellFileName(cell)}`;

        cellDiv.removeEventListener('click', onCellClick);
        if (isClickable) {
            cellDiv.addEventListener('click', onCellClick);
        }
    }
}

const cellFiles = { 1: 'forager-80x80.png', 2: 'grass-80x80.png', 4: 'hole-80x80.png', 5: 'flaghole-80x80.png', 6: 'stone.jpg' };
const getCellFileName = (cell) => cell.state === 0 ? `${cell.ctr % 10}.jpg` : isTroddenGrass(cell) ? `troddengrass-80x80.png` : cellFiles[cell.state];
const isTroddenGrass = (cell) => (cell.state === 2 && cell.isTrodden);


async function onCellClick(e) {
    const response = await postJson(`${apiBase}/${currentGameId}/action`, { Row: e.currentTarget.dataset.row, Col: e.currentTarget.dataset.col });
    if (response.ok) {
        updateGameDisplay((await response.json()).state);
    }
}

async function resetGame() {
    const response = await postJson(`${apiBase}/${currentGameId}/reset`, {});
    if (response.ok) {
        updateGameDisplay((await response.json()).state);
    }
}

function showSettings() {
    showModal('settingsModal');
    document.getElementById('fieldSize').value = gameSettings.fieldSize;
    document.getElementById('numShrooms').value = gameSettings.numShrooms;
    document.getElementById('pcStone').value = gameSettings.pcStone;
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
    } else {
        showMessage('Number of mushrooms must be between 3 and 15', 'error');
        return;
    }

    const pcStone = parseInt(document.getElementById('pcStone').value);
    if (pcStone >= 0 && pcStone <= 25) {
        gameSettings.pcStone = pcStone;
    } else {
        showMessage('Percentage stone wall must be between 0 and 25', 'error');
        return;
    }

    showMessage('Settings saved! Start a new game to apply changes.', 'success');
    hideModal('settingsModal');
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