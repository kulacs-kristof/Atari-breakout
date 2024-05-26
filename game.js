const canvas = document.getElementById('gameCanvas');
const ctx = canvas.getContext('2d');

let bird = { x: 50, y: 150, width: 40, height: 40, gravity: 0.8, lift: -8, speed: 0 };
let pipes = [];
let frameCount = 0;
let score = 0;
let gap = 210; // rés a csövek között

// Kép betöltése
const birdImg = new Image();
birdImg.src = 'bird.png';
const backgroundImg = new Image();
backgroundImg.src = 'background.webp';

const topPipeImage = new Image();
topPipeImage.src = 'pipe_top.png';
const bottomPipeImage = new Image();
bottomPipeImage.src = 'pipe_bottom.png';

let imagesLoaded = 0;
const totalImages = 4;

function imageLoaded() {
    imagesLoaded++;
    if (imagesLoaded === totalImages) {
        gameLoop();
    }
}

birdImg.onload = imageLoaded;
backgroundImg.onload = imageLoaded;
topPipeImage.onload = imageLoaded;
bottomPipeImage.onload = imageLoaded;

// Irányítás
document.addEventListener('keydown', () => {
    bird.speed = bird.lift;
});
document.addEventListener('mousedown', () => {
    bird.speed = bird.lift;
});

// Játék fő ciklusa
function gameLoop() {
    update();
    positions();
    requestAnimationFrame(gameLoop);
}