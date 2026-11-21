"""Bake registered Unity animation cells from the existing city painting (Pillow, NumPy)."""
import json
from pathlib import Path

import numpy as np
from PIL import Image

ART = Path(__file__).resolve().parents[1] / "Assets/Resources/InkInterface"
layout = json.loads((ART / "CityLandmarkClips.json").read_text())
source = np.asarray(Image.open(ART / "CityLandmarks.jpg").convert("RGB"), dtype=float)
size = layout["cell"]
frames = layout["frames"]
v, u = (np.indices((size, size)) + 0.5) / size
tau = 2 * np.pi


def sat(a):
    return np.clip(a, 0, 1)


def spot(x, y, cx, cy, rx, ry):
    return np.exp(-2 * (((x - cx) / rx) ** 2 + ((y - cy) / ry) ** 2))


def sample(x, y):
    sx = np.clip(x / layout["width"] * source.shape[1] - 0.5, 0, source.shape[1] - 1.001)
    sy = np.clip(y / layout["height"] * source.shape[0] - 0.5, 0, source.shape[0] - 1.001)
    ix, iy = sx.astype(int), sy.astype(int)
    fx, fy = (sx - ix)[..., None], (sy - iy)[..., None]
    return ((1-fx) * source[iy, ix] + fx * source[iy, ix+1]) * (1-fy) + (
        (1-fx) * source[iy+1, ix] + fx * source[iy+1, ix+1]) * fy


def rgba(rgb, alpha):
    return Image.fromarray(np.uint8(np.clip(np.dstack((rgb, 255 * sat(alpha))), 0, 255)))


def room_cell(clip, phase):
    x = clip["left"] + u * (clip["right"] - clip["left"])
    y = clip["top"] + v * (clip["bottom"] - clip["top"])
    theta = phase * tau
    dx, dy, light, shade = (np.zeros_like(x) for _ in range(4))
    color = np.array([255, 225, 154])
    kind = clip["name"]

    if kind == "GatePennant":
        # The knot stays on the crossbar. Motion increases towards the tassels.
        cloth = sat(1 - abs(x - (50.5 + 0.12 * (y - 122))) / 13)
        weight = cloth * sat((y - 128) / 30) * sat((174 - y) / 7)
        dx = 5.2 * weight * np.sin(theta - (y - 126) / 13)
        dy = 1.0 * weight * np.cos(theta - (y - 126) / 13)
        shade = .12 * weight * np.sin(theta + (y - 126) / 9)
    elif kind == "PavilionLamps":
        for cx in (457, 565):
            lamp = spot(x, y, cx, 105, 7, 13)
            dx += lamp * 2.8 * np.sin(theta) * sat((y - 95) / 8)
            light += lamp * (.20 + .19 * np.cos(theta))
        for cx, cy in ((502, 114), (526, 114), (509, 157), (529, 158)):
            light += spot(x, y, cx, cy, 9, 8) * (.18 + .17 * np.sin(theta + cx / 15))
    elif kind == "MarketCanopy":
        hem = 347 - .0036 * (x - 84) ** 2
        cloth = sat((x - 26) / 8) * sat((145 - x) / 9) * sat((y - 308) / 13) * sat((hem + 6 - y) / 5)
        wave = np.sin((x - 33) / 16 - theta)
        dy = 3.3 * wave * cloth
        shade = .073 * cloth * wave
    elif kind == "TavernWheel":
        # Re-project the front face. Preserve the fixed axle and the stone pedestal.
        nx, ny = (x - 586.5) / 32.7, (y - 377.4) / 47.2
        radius = np.hypot(nx, ny)
        stone = sat((y - 374) / 4) * sat((x - (588 - .30 * (y - 374))) / 3)
        wheel = sat((1.03 - radius) / .13) * sat((radius - .13) / .12) * (1-stone)
        def rotated(angle):
            cs, sn = np.cos(angle), np.sin(angle)
            return sample(586.5 + 32.7 * (nx * cs - ny * sn), 377.4 + 47.2 * (ny * cs + nx * sn))
        angle = phase * tau / 12
        # Adjacent spoke images cross-fade; the last cell joins directly to the first.
        spinning = rotated(-angle) * (1-phase) + rotated(tau / 12 - angle) * phase
        water = spot(x, y, 541.5, 415, 15, 32)
        dy = water * 3.4 * np.sin((y - 390) * .45 - theta * 2)
        dx = water * 1.1 * np.sin(y * .28 - theta)
        light = water * np.maximum(0, np.cos((y - 387) * .45 - theta * 2)) * .32
        # A bead of reflected sky runs down the wet timber beside the spill.
        wet_rim = np.exp(-((np.hypot((x-584)/42, (y-378)/51) - 1) / .07)**2)
        wet_rim *= spot(x, y, 546, 396, 11, 26)
        light += wet_rim * (.30 + .28 * np.cos(theta - (y-395)/7))
        color = np.array([213, 249, 239])
    elif kind == "SpiritLanterns":
        color = np.array([255, 223, 255])
        for cx, cy in ((125, 557), (225, 549)):
            light += (.31 + .28 * np.sin(theta + cx / 48)) * spot(x, y, cx, cy, 13, 15)
            light += .45 * spot(x, y, cx + 3 * np.cos(theta), cy - 11 - 4 * np.sin(theta), 2.4, 3.2)
    elif kind == "TransitRunes":
        color = np.array([215, 255, 233])
        nx, ny = (x - 494) / 43, (y - 556) / 12.5
        r, a = np.hypot(nx, ny), np.arctan2(ny, nx)
        ring = np.exp(-((r - .85) / .13) ** 2)
        light = ring * (.19 + .61 * ((1 + np.cos(a - theta)) / 2) ** 3)
        for offset in (0, np.pi):
            light += .75 * spot(x, y, 494 + 35 * np.cos(theta + offset),
                                556 + 10.3 * np.sin(theta + offset), 4, 2.5)
        light += (.15 + .13 * np.sin(theta)) * spot(x, y, 494, 544, 13, 22)
    elif kind == "NoticePaper":
        corner = sat((x - 337) / 38) * sat((y - 768) / 34)
        corner *= sat((382-x)/5) * sat((810-y)/5)
        lift = (.5 - .5 * np.cos(theta)) ** 2
        dx = -2 * corner * lift
        dy = 4.4 * corner * lift
        shade = -.22 * corner * lift

    rgb = sample(x-dx, y-dy)
    active = np.maximum.reduce([sat(abs(dx) + abs(dy)), sat(abs(shade) * 15), sat(light * 9)])
    if kind == "TavernWheel":
        rgb += (spinning - rgb) * wheel[..., None]
        active = np.maximum(active, wheel)
    rgb = rgb * (1 + shade[..., None])
    rgb += (color - rgb) * sat(light)[..., None]
    edge = sat(np.minimum.reduce((u, v, 1-u, 1-v)) * 48)
    return rgba(rgb, active * edge)


def touch_cell(frame):
    # A split, gold-rimmed ink ripple. The dark rim remains visible over light paper.
    progress = frame / (frames-1)
    x, y = u - .5, v - .5
    r, a = np.hypot(x, y), np.arctan2(y, x)
    radius = .15 + .31 * (1 - (1-progress)**2)
    wobble = .005 * np.sin(5*a + .5)
    outer = np.exp(-((r-radius-wobble) / .019)**2)
    ink = np.exp(-((r-radius-wobble-.013) / .025)**2)
    arc = .40 + .60 * (.5 + .5 * np.cos(2*a-.7))**.6
    inner = np.exp(-((r-radius*.70) / .014)**2) * (.5 + .5*np.cos(3*a+1))
    glow = .26 * np.exp(-(r/(radius*.84))**2)
    alpha = ((ink + outer) * arc + inner*.68 + glow) * (1-progress*.92)
    rgb = np.broadcast_to([57., 105., 85.], (size,size,3)).copy()
    rgb += (np.array([255.,227.,161.])-rgb) * sat(outer*1.45 + inner)[...,None]
    return rgba(rgb, alpha)


# Traced highlight paths in the painting. Preserve the breaks between sunlit tiles.
RIDGES = {
    "GateRoofGlint": [[(42,126),(71,117),(83,95),(96,108),(177,110),(186,100)],
                      [(86,153),(122,142),(181,142),(206,149),(229,145)]],
    "PavilionGlaze": [[(447,88),(472,92),(494,81),(514,65),(535,82),(553,91),(571,85)],
                      [(444,130),(475,128),(495,122),(530,122),(555,128),(576,126)]],
    "MarketBrass": [[(25,325),(54,340),(80,347),(109,341),(143,326)],
                    [(74,356),(86,357),(103,355)]],
    "TavernEaves": [[(420,298),(452,292),(494,287),(541,270),(550,265),(564,283),(584,295)],
                   [(423,345),(474,345),(514,337)]],
    "ShrineGlaze": [[(55,472),(79,478),(99,469),(116,451),(125,454),(145,474),(166,478),(189,458)],
                    [(72,503),(94,509),(117,500),(143,487),(165,502),(184,509),(214,491)]],
    "TransitStones": [[(425,515),(430,482),(437,469),(444,485),(445,514)],
                      [(540,514),(542,485),(550,471),(557,490),(559,516)],
                      [(582,555),(582,526),(590,511),(597,531)]],
    "NoticeTacks": [[(z, w),(z+1, w)] for z in (241,299,320,373) for w in (739,802)]
}


def sunlight_cell(clip, phase):
    x = clip["left"] + u * (clip["right"] - clip["left"])
    y = clip["top"] + v * (clip["bottom"] - clip["top"])
    color = np.array([255., 235., 172.])
    envelope = np.sin(phase * np.pi)**.80
    if clip["name"] == "RiverReflection":
        # Broken horizontal reflections follow the visible strip of water under the bridge.
        mask = spot(x, y, 342, 641, 57, 28) + .55 * spot(x, y, 307, 652, 29, 20)
        ripples = (.5 + .5 * np.cos(.76 * y + .56*np.sin(x*.12 - phase*tau) - phase*tau*1.4)) ** 9
        flicker = (.5 + .5*np.cos(.21*x + .42*y - phase*tau))**3
        painted = sample(x, y)
        water = sat((painted[:,:,1]-painted[:,:,0]+8)/25)
        alpha = mask * ripples * flicker * water * envelope * .78
        color = np.array([255., 245., 201.])
    else:
        distances = []
        for ridge in RIDGES[clip["name"]]:
            for (x1,y1),(x2,y2) in zip(ridge, ridge[1:]):
                sx, sy = x2-x1, y2-y1
                t = sat(((x-x1)*sx+(y-y1)*sy)/(sx*sx+sy*sy))
                distances.append(np.hypot(x-x1-t*sx, y-y1-t*sy))
        distance = np.minimum.reduce(distances)
        band = np.exp(-((u - (-.04 + 1.08*phase)) / .14)**2)
        luminance = sample(x, y).mean(axis=2)
        glaze = .22 + .78 * sat((luminance - 55)/124)
        alpha = (.93*np.exp(-(distance/1.6)**2) + .24*np.exp(-(distance/4.2)**2)) * band * envelope * glaze
        if clip["name"] == "PavilionGlaze":
            for cx,cy in ((500,111),(526,111),(512,157),(529,157)):
                alpha += .50 * spot(x,y,cx,cy,7,7) * band * envelope
    return rgba(np.broadcast_to(color, (size,size,3)), alpha)


sheet = Image.new("RGBA", (frames*size, (len(layout["rooms"])+1+len(layout["sunlight"]))*size))
for row, clip in enumerate(layout["rooms"]):
    for frame in range(frames):
        sheet.paste(room_cell(clip, frame/frames), (frame*size, row*size))
for frame in range(frames):
    sheet.paste(touch_cell(frame), (frame*size, len(layout["rooms"])*size))
for row, clip in enumerate(layout["sunlight"], len(layout["rooms"])+1):
    for frame in range(frames):
        sheet.paste(sunlight_cell(clip, frame/(frames-1)), (frame*size, row*size))
sheet.save(ART / "CityLandmarkFrames.png", optimize=True)
