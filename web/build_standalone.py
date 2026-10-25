"""Wrap each game into a full web page that any browser can open directly,
including a VR headset's browser. Run it after changing a game:

    python3 web/build_standalone.py
"""
from pathlib import Path

here = Path(__file__).parent

# game file -> standalone page (served by GitHub Pages under /web/)
PAGES = {
    "paint-room.html": "index.html",
    "bonebreak.html": "bonebreak/index.html",
}

for source, target in PAGES.items():
    game = (here / source).read_text(encoding="utf-8")
    page = (
        "<!doctype html>\n"
        '<html lang="en">\n'
        "<head>\n"
        '<meta charset="utf-8">\n'
        '<meta name="viewport" content="width=device-width, initial-scale=1, viewport-fit=cover">\n'
        "<style>body { margin: 0; } img { max-width: 100%; } [hidden] { display: none !important; }</style>\n"
        "</head>\n"
        "<body>\n"
        f"<!-- Built from {source} by build_standalone.py. Edit {source}, not this file. -->\n"
        f"{game}\n"
        "</body>\n"
        "</html>\n"
    )
    out = here / target
    out.parent.mkdir(parents=True, exist_ok=True)
    out.write_text(page, encoding="utf-8")
    print("Wrote", out)
