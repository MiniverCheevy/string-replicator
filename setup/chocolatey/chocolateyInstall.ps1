$name = 'String Replicator'
$url  = 'https://cdn.rawgit.com/MiniverCheevy/string-replicator/be952b694164e86a1b7da161f26cff78ab8d23a3/setup/Output/setup.exe?raw=true'

Install-ChocolateyPackage $name 'EXE' '/VERYSILENT /SUPPRESSMSGBOXES /NORESTART' $url