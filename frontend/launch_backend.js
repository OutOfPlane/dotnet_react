const { spawn } = require('child_process');
const path = require('path');


const backend = spawn("dotnet", ["run", "--project", "net_backend"], {
  stdio: 'inherit',
  cwd: path.join(__dirname, '../backend'),
  windowsHide: true,
});

backend.on('error', (err) => {
  console.error('Failed to start backend:', err);
});

process.on('exit', () => backend.kill());
process.on('SIGINT', () => process.exit());
process.on('SIGTERM', () => process.exit());