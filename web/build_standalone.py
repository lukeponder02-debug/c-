"""Wrap paint-room.html into a full web page (index.html) that any browser can open directly,
including a VR headset's browser. Run it after changing paint-room.html:

    python3 web/build_standalone.py
"""
from pathlib import Path

here = Path(__file__).parent
game = (here / "paint-room.html").read_text(encoding="utf-8")

page = (
    "<!doctype html>\n"
    '<html lang="en">\n'
    "<head>\n"
    '<meta charset="utf-8">\n'
    '<meta name="viewport" content="width=device-width, initial-scale=1, viewport-fit=cover">\n'
    "<style>body { margin: 0; } img { max-width: 100%; } [hidden] { display: none !important; }</style>\n"
    "</head>\n"
    "<body>\n"
    "<!-- Built from paint-room.html by build_standalone.py. Edit paint-room.html, not this file. -->\n"
    f"{game}\n"
    "</body>\n"
    "</html>\n"
)

(here / "index.html").write_text(page, encoding="utf-8")
print("Wrote", here / "index.html")
