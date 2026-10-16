"""Gera os ZIPs de instalação; não copia arquivos para o jogo."""
from pathlib import Path
from zipfile import ZIP_DEFLATED, ZipFile
import hashlib
import json
from validate import ROOT, validate

validate()
dist = ROOT / 'dist'
dist.mkdir(exist_ok=True)
main_dll = ROOT / 'src/Collectibles/bin/Release/net9.0/Maestro.BrainiacCollectibles.dll'
assert main_dll.is_file(), 'Compile o mod primeiro.'

main_zip = dist / 'Brainiac-Collectibles-1.4.0-alpha.1-SPT-4.0.13-Experimental.zip'
with ZipFile(main_zip, 'w', ZIP_DEFLATED) as archive:
    base = 'SPT_Runtime/user/mods/Maestro-BrainiacCollectibles/'
    archive.write(main_dll, base + main_dll.name)
    for file in (ROOT / 'content').rglob('*'):
        if file.is_file():
            archive.write(file, base + file.relative_to(ROOT / 'content').as_posix())
    archive.write(ROOT / 'CREDITS.md', base + 'CREDITS.md')
    archive.write(ROOT / 'LICENSE', 'LICENSE.txt')
    archive.write(ROOT / 'README.md', 'README.md')
manifest = {file.name: hashlib.sha256(file.read_bytes()).hexdigest() for file in [main_zip]}
(dist / 'checksums.json').write_text(json.dumps(manifest, indent=2) + '\n', encoding='utf-8')
for file in manifest:
    print(dist / file)
