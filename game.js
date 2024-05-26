const canvas = document.getElementById('gameCanvas');
const ctx = canvas.getContext('2d');

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
