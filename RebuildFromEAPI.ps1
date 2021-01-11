ssotme -build

$pkg = cat .\package.json|convertfrom-json
$pkg.version = Get-Date -format 'vyyyy.MM.dd.01'
$pkg|convertto-json|out-file package.json
npm i -g .