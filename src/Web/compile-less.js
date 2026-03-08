const fs = require('fs');
const path = require('path');
const less = require('less');

// Function to recursively find all .less files
function findLessFiles(dir, fileList = []) {
    const files = fs.readdirSync(dir);

    files.forEach(file => {
        const filePath = path.join(dir, file);
        const stat = fs.statSync(filePath);

        if (stat.isDirectory()) {
            findLessFiles(filePath, fileList);
        } else if (path.extname(file) === '.less') {
            fileList.push(filePath);
        }
    });

    return fileList;
}

// Function to compile a LESS file to CSS
async function compileLessFile(lessFile) {
    try {
        const lessContent = fs.readFileSync(lessFile, 'utf8');
        const output = await less.render(lessContent, {
            filename: lessFile,
            compress: false,
            sourceMap: {
                sourceMapFileInline: false
            }
        });

        const cssFile = lessFile.replace('.less', '.css');
        fs.writeFileSync(cssFile, output.css, 'utf8');
        console.log(`Compiled: ${lessFile} -> ${cssFile}`);
    } catch (error) {
        console.error(`Error compiling ${lessFile}:`, error.message);
        process.exit(1);
    }
}

// Main compilation process
async function compileAll() {
    const contentDir = path.join(__dirname, 'Content');

    if (!fs.existsSync(contentDir)) {
        console.error('Content directory not found');
        process.exit(1);
    }

    const lessFiles = findLessFiles(contentDir);
    console.log(`Found ${lessFiles.length} LESS files to compile`);

    for (const lessFile of lessFiles) {
        await compileLessFile(lessFile);
    }

    console.log('LESS compilation complete!');
}

compileAll().catch(error => {
    console.error('Compilation failed:', error);
    process.exit(1);
});
