let currentGameId = null;
let gameSettings = {
    fieldSize: 10,
    numShrooms: 10
};
const apiBase = 'http://localhost:5270/api/game'; // Adjust this to your API base URL

function showSettings() {
    document.getElementById('settingsSection').style.display = 'block';
    document.getElementById('fieldSize').value = gameSettings.fieldSize;
}

function hideSettings() {
    document.getElementById('settingsSection').style.display = 'none';
}

function saveSettings() {
    const size = parseInt(document.getElementById('fieldSize').value);
    if (size >= 3 && size <= 20) {
        gameSettings.fieldSize = size;
        hideSettings();
        showMessage('Settings saved! Start a new game to apply changes.', 'success');
    } else {
        showMessage('Board size must be between 3 and 20', 'error');
    }
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
        }
        );

        if (response.ok) {
            const data = await response.json();
            currentGameId = data.GameId;
            updateGameDisplay(data.state);
            showMessage('Game started successfully!', 'success');
            document.getElementById('gameDisplay').style.display = 'block';
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
    document.getElementById('gameId').textContent = currentGameId;
    //document.getElementById('turnNumber').textContent = gameState.turn;
    //document.getElementById('currentPlayer').textContent = gameState.currentPlayer;
    //document.getElementById('gameStatus').textContent = gameState.gameStatus;
    renderBoard(gameState.cells);

    //// Show game over message if applicable
    //if (gameState.IsGameOver) {
    //    showMessage(`Game Over! Winner: ${gameState.Winner}`, 'success');
    //    document.getElementById('gameActions').style.display = 'none';
    //}
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
    for (let idx = 0; idx < cells.length; idx++) {
        const cell = cells[idx];
        const cellDiv = document.createElement('div');
        cellDiv.className = 'board-cell';
        cellDiv.dataset.row = cell.row;
        cellDiv.dataset.col = cell.col;
        const img = document.createElement('img');
        img.src = `/images/grass.jpeg`;
        img.alt = cell.image;
        img.style.width = "100%";
        img.style.height = "100%";
        cellDiv.appendChild(img);

        // Add click handler for cell interaction
        cellDiv.addEventListener('click', () => onCellClick(row, col));

        boardDiv.appendChild(cellDiv);
    }
}

function onCellClick(row, col) {
    // Handle cell click - you can customize this based on your game
    console.log(`Clicked cell: (${row}, ${col})`);

    // Example: Set the clicked position for the next action
    const actionType = document.getElementById('actionType').value;
    if (actionType) {
        // Auto-fill position parameter if there's an input for it
        const positionInput = document.querySelector('input[name="position"]');
        if (positionInput) {
            positionInput.value = `${row},${col}`;
        }
    }
}

function updateActionForm() {
    const actionType = document.getElementById('actionType').value;
    const parametersDiv = document.getElementById('actionParameters');

    // Clear previous parameters
    parametersDiv.innerHTML = '';

    // Add parameter fields based on action type
    // Customize this based on your game's actions
    switch (actionType) {
        case 'move':
            parametersDiv.innerHTML = `
                                <label>Position:</label>
                                <input type="text" name="position" placeholder="e.g., A1">
                            `;
            break;
        case 'attack':
            parametersDiv.innerHTML = `
                                <label>Target:</label>
                                <input type="text" name="target" placeholder="Target position">
                            `;
            break;
        // Add more cases for your specific actions
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