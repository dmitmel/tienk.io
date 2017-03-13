#!/usr/bin/env node

var Canvas = require('canvas'),
    fs = require('fs');

var CELL_WIDTH = 20,
    CELL_HEIGHT = 20,
    CELLS_HORIZONTAL_COUNT = 100,
    CELLS_VERTICAL_COUNT = 100,
    WIDTH = CELL_WIDTH * CELLS_HORIZONTAL_COUNT,
    HEIGHT = CELL_HEIGHT * CELLS_VERTICAL_COUNT,
    canvas = new Canvas(WIDTH, HEIGHT, 'png'),
    context = canvas.getContext('2d');

context.fillStyle = '#FFF';
context.strokeStyle = '#000';

context.fillRect(0, 0, WIDTH, HEIGHT);

context.strokeLine = function strokeLine(x1, y1, x2, y2) {
    this.beginPath();
    this.moveTo(x1, y1);
    this.lineTo(x2, y2);
    this.stroke();
};

for (var cellX = 0; cellX < CELLS_HORIZONTAL_COUNT + 1; cellX++) {
    var x = CELL_WIDTH * cellX;
    context.strokeLine(x, 0, x, HEIGHT);
    // context.strokeLine(x, 0, x, HEIGHT);
    // context.strokeLine(x, 0, x, HEIGHT);
    // context.strokeLine(x, 0, x, HEIGHT);
    // context.strokeLine(x, 0, x, HEIGHT);
    // context.strokeLine(x, 0, x, HEIGHT);
    // context.strokeLine(x, 0, x, HEIGHT);
    // context.strokeLine(x, 0, x, HEIGHT);
}

for (var cellY = 0; cellY < CELLS_VERTICAL_COUNT + 1; cellY++) {
    var y = CELL_HEIGHT * cellY;
    context.strokeLine(0, y, WIDTH, y);
    // context.strokeLine(0, y, WIDTH, y);
    // context.strokeLine(0, y, WIDTH, y);
    // context.strokeLine(0, y, WIDTH, y);
    // context.strokeLine(0, y, WIDTH, y);
    // context.strokeLine(0, y, WIDTH, y);
    // context.strokeLine(0, y, WIDTH, y);
    // context.strokeLine(0, y, WIDTH, y);
}

fs.writeFile('Assets/Textures/Background.png', canvas.toBuffer());
