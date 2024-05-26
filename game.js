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

function update() {
    // Madár mozgása
    bird.speed += bird.gravity;
    bird.y += bird.speed;

    if (bird.y + bird.height > canvas.height || bird.y < 0) {
        resetGame();
    }

    // Akadályok frissítése
    if (frameCount % 180 === 0) {
        let pipeWidth = 80;
        let pipeHeight = 100;
    
        // Arányosan változtatjuk az akadály magasságát a szélességhez
        let pipeSize = pipeHeight / pipeWidth;
        
        let topHeight = Math.floor(Math.random() * (canvas.height - gap - pipeHeight))+50; // Véletlenszerű magasság a felső akadálynak
        let bottomHeight = canvas.height - topHeight - gap;
    
        pipes.push({
            x: canvas.width,
            topY: topHeight,
            bottomY: topHeight + gap,
            width: pipeWidth,
            height: pipeWidth * pipeSize // Arányosan változik a csövek magassága
        });
    }

    pipes.forEach((pipe, index) => {
        pipe.x -= 2;
        if (pipe.x + pipe.width < 0) {
            pipes.splice(index, 1);
            score++;
        }

        // Ütközés vizsgálata
        if (
            bird.x < pipe.x + pipe.width &&
            bird.x + bird.width > pipe.x &&
            (
                bird.y < pipe.topY || // Ütközés a felső csővel
                bird.y + bird.height > pipe.bottomY // Ütközés az alsó csővel
            )
        ) {
            resetGame();
        }
    });

    frameCount++;
}

// Játék fő ciklusa
function gameLoop() {
    update();
    positions();
    requestAnimationFrame(gameLoop);
}