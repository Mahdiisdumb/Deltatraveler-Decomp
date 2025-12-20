const fs = require('fs');
const path = require('path');

// Paths to clean
const projectRoot = path.resolve(__dirname);
const pluginDir = path.join(projectRoot, 'Assets', 'plugins');
const cacheDirs = ['Library', 'Temp', 'obj'];

// 1. Remove problematic DLLs
const dllsToRemove = [
  'Assembly-CSharp.dll',
  'FullSerializer.dll',
  'Unity.InputSystem.dll'
];

dllsToRemove.forEach(dll => {
  const filePath = path.join(pluginDir, dll);
  if (fs.existsSync(filePath)) {
    fs.unlinkSync(filePath);
    console.log(`Deleted DLL: ${dll}`);
  }
});

// 2. Remove Unity cache folders
cacheDirs.forEach(dir => {
  const dirPath = path.join(projectRoot, dir);
  if (fs.existsSync(dirPath)) {
    fs.rmSync(dirPath, { recursive: true, force: true });
    console.log(`Deleted folder: ${dir}`);
  }
});

// 3. Fix malformed [MovedFrom] attributes
function fixMovedFrom(filePath) {
  let content = fs.readFileSync(filePath, 'utf8');
  const regex = /\[MovedFrom\(.*?\)\]/g; // crude match for attributes
  content = content.replace(regex, ''); // remove them
  fs.writeFileSync(filePath, content, 'utf8');
  console.log(`Cleaned [MovedFrom] in: ${filePath}`);
}

function scanCsFiles(dir) {
  const files = fs.readdirSync(dir);
  files.forEach(f => {
    const fullPath = path.join(dir, f);
    const stat = fs.statSync(fullPath);
    if (stat.isDirectory()) scanCsFiles(fullPath);
    else if (f.endsWith('.cs')) fixMovedFrom(fullPath);
  });
}

// Run cleanup on Assets
scanCsFiles(path.join(projectRoot, 'Assets'));

console.log('✅ Harris cleanup complete. Reopen Unity after this.');
