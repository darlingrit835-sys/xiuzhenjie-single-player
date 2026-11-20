// Render a single transparent book sprite per stable icon ID. Requires local Node.js and sharp.
'use strict';
const fs = require('node:fs');
const path = require('node:path');
const sharp = require('sharp');

const root = path.resolve(__dirname, '../..');
const dataDir = path.join(root, 'server/data/config');
const defaultOut = path.join(root, 'UnityProject/Assets/Resources/CatalogIcons/Methods');
const study = fs.readFileSync(path.join(root, 'art/catalog/method-emblems.svg'), 'utf8');
const defs = study.slice(study.indexOf('<defs>') + 6, study.indexOf('</defs>'));
const motif = (name, x = 0, y = 0, scale = 1, rotation = 0) =>
  `<use xlink:href="#${name}" transform="translate(${x} ${y}) scale(${scale}) rotate(${rotation} 50 50)"/>`;

// Physical book pattern and emblem ID are independent of the current localized title.
// Each composition has its own scale, overlap and silhouette. Foreground subjects render last.
const scenes = {
  71001: [motif('moon', 15, -5, .7, 24), motif('eye', 0, 21, 1.00), motif('surf', 25, 70, .34)],
  71002: [motif('mountain', 15, -3, .84), motif('monk', -4, 29, .73), motif('grass', 69, 55, .35)],
  71003: [motif('surf', -1, 54, 1), motif('lotus', 3, -4, .98)],
  71004: [motif('cloudbank', 11, 48, .85), motif('bell', -1, 0, 1.0)],
  71005: [motif('moon', 10, -6, .54), motif('wave', 0, 7, .99)],
  71006: [motif('cloudbank', 2, 53, .97), motif('pagoda', 5, -5, .90)],
  71007: [motif('flame', -1, -5, 1.02), motif('pearl', 24, 53, .49)],
  71008: [motif('moon', -1, 1, .83, -25), motif('mask', 31, 25, .66, 5)],
  71009: [motif('stars', 48, 1, .52), motif('planet', 1, 10, .99, -13)],
  71010: [motif('fish', 2, 9, .78, -26), motif('fish', 24, 31, .75, 149), motif('pearl', 44, 44, .25)],
  71011: [motif('stars', 0, 5, 1.0), motif('comet', 19, 14, .87), motif('mountain', -2, 62, .46)],
  71012: [motif('spiral', -1, 0, .94, -17), motif('eye', 30, 32, .55, -10)],
  71013: [motif('cloudbank', 22, 56, .73), motif('heron', 0, -1, 1)],
  71014: [motif('moon', 40, -6, .54), motif('pine', -3, 7, .98), motif('surf', 44, 72, .42)],
  71015: [motif('shield', 14, 8, .75), motif('flame', -3, 48, .51), motif('flame', 54, 55, .43)],
  71016: [motif('stars', -1, 4, .68), motif('cloudbank', 4, 55, .98), motif('eagle', 4, -4, .95)],
  71017: [motif('surf', 5, 54, .93), motif('mirror', 4, -6, .98, -13)],
  72001: [motif('bones', -4, 13, .91, -16), motif('bones', 46, -6, .56, 20)],
  72002: [motif('flame', -5, 29, .63), motif('anvil', -4, 21, .95), motif('hammer', 27, -9, .73, -10)],
  72003: [motif('crystal', 44, -5, .51), motif('shield', -1, 8, .92)],
  72004: [motif('dragon', 39, -10, .59), motif('elephant', -3, 14, 1.0)],
  72005: [motif('surf', 3, 62, .91), motif('shell', 0, 3, .98), motif('pearl', 69, 53, .26)],
  72006: [motif('grass', 42, 47, .51), motif('tiger', -1, 7, 1.04)],
  72007: [motif('pearl', 18, -12, .67), motif('mantle', 3, 9, .98)],
  72008: [motif('cloudbank', -2, 46, 1), motif('crescentBlade', 1, -4, 1.03)],
  72009: [motif('stars', 0, -4, .97), motif('mountain', 6, 24, .92), motif('comet', 51, -9, .48)],
  72010: [motif('comet', 3, -5, .58), motif('shield', 20, 19, .81, -12), motif('comet', 64, 56, .35)],
  72011: [motif('mountain', 28, 12, .68), motif('horns', -3, -1, .99)],
  72012: [motif('flame', -2, 14, .98), motif('mask', 20, 3, .68, -8)],
  72013: [motif('crystal', -3, -5, 1.02), motif('grass', 60, 57, .42)],
  72014: [motif('stars', 44, -2, .55), motif('mantle', -3, 13, .88), motif('feather', 58, 49, .44, -22)],
  72015: [motif('cloudbank', -4, 52, .9), motif('pine', 7, -6, 1.02), motif('lotusbud', 64, 57, .37)],
  72016: [motif('moon', 13, -9, .77, 45), motif('monk', 8, 6, .95)],
  73001: [motif('grass', -8, 49, .59), motif('seedling', 9, -4, 1.0), motif('pearl', 63, 59, .29)],
  73002: [motif('pearl', 21, -9, .53), motif('vessel', 0, 11, .93)],
  73003: [motif('leaf', 40, -5, .57), motif('pine', -4, 7, .97)],
  73004: [motif('waterfall', 3, -6, 1.01), motif('fish', 52, 64, .43, -17)],
  73005: [motif('flame', -6, -6, 1.1), motif('bird', 61, -7, .38)],
  73006: [motif('mountain', -3, 1, 1.03), motif('grass', 41, 62, .48)],
  73007: [motif('pearl', 43, -7, .56), motif('cloud', -3, 5, .98), motif('cloudbank', 14, 51, .81)],
  73008: [motif('stars', 4, -6, .98), motif('moon', 3, 22, .78, -27), motif('pearl', 56, 41, .4)],
  73009: [motif('spiral', 7, 4, .84, -45), motif('leaf', -6, 57, .41), motif('leaf', 59, -5, .39, 108)],
  73010: [motif('cauldron', 4, 12, .94), motif('cloud', 14, -9, .65)],
  73011: [motif('bamboo', -3, 0, .98), motif('bamboo', 49, 27, .62, 14)],
  73012: [motif('mountaingate', 3, 20, .93), motif('bird', -4, -8, .59), motif('bird', 59, -8, .40)],
  73013: [motif('moon', 51, -5, .44), motif('dragon', 0, 3, 1.02)],
  73014: [motif('wave', 23, 13, .76, 11), motif('shell', -5, 27, .74)],
  73015: [motif('moon', 1, -5, .78, -40), motif('moon', 34, 25, .72, 139), motif('pearl', 26, 34, .39)],
  73016: [motif('mountain', 2, 40, .91), motif('lotusbud', 25, -7, .66), motif('cloudbank', 47, 63, .52)],
  73017: [motif('moon', 40, -8, .64), motif('leaf', -4, -2, .94, -22), motif('seedling', 53, 45, .49)],
};

// Cover pigments and block-print inks, indexed by the actual profile's element.
const palettes = {
  1: ['#363c3d','#616766','#9caaa3','#ddd8bd','#f4e4b8','#b69765'],
  2: ['#263c31','#4c6350','#879270','#c5c7a2','#e0d9ad','#9fa879'],
  3: ['#253848','#436275','#8aa1a8','#bdcbd0','#ece0b5','#9baab3'],
  4: ['#492e2a','#815044','#ab8065','#d7b792','#f2d4a1','#bb7654'],
  5: ['#463b2a','#746448','#ab9a76','#d1c19b','#ede1af','#aa9065'],
};

function randomSequence(seed) {
  let state = seed >>> 0;
  return () => ((state = (Math.imul(1664525, state) + 1013904223) >>> 0) / 4294967296);
}
function coverSurface(id, theme, pathId) {
  const [shade, tone, worn] = theme;
  const rand = randomSequence(id);
  const dots = [];
  // Small clusters, broken pigment and diagonal linen fibres give the three bindings varied material.
  for (let i=0; i<570; i++) {
    const x=(32+rand()*194).toFixed(1), y=(10+rand()*220).toFixed(1);
    const radius=(.22+rand()*(pathId===2?1.25:.86)).toFixed(2);
    dots.push(`<circle cx="${x}" cy="${y}" r="${radius}" fill="${i%3 ? shade : worn}" opacity="${(.08+rand()*.18).toFixed(2)}"/>`);
  }
  for (let i=0; i<215; i++) {
    const x=(34+rand()*189).toFixed(1), y=(15+rand()*205).toFixed(1);
    const length=pathId===3 ? 1.3+rand()*12 : .6+rand()*5;
    const dy=(rand()-.5)*(pathId===2?3:2);
    dots.push(`<path d="M${x} ${y}q${(length*.4).toFixed(1)} ${(dy*.1).toFixed(1)} ${length.toFixed(1)} ${dy.toFixed(1)}" stroke="${i%2 ? worn : shade}" stroke-width="${(.3+rand()*.45).toFixed(2)}" opacity=".29" fill="none"/>`);
  }
  return `<defs><linearGradient id="cloth" x1=".15" x2=".98" y1="0" y2="1"><stop stop-color="${tone}"/><stop offset=".43" stop-color="${tone}"/><stop offset="1" stop-color="${shade}"/></linearGradient>
    <radialGradient id="stain"><stop stop-color="${worn}" stop-opacity=".27"/><stop offset="1" stop-color="${worn}" stop-opacity="0"/></radialGradient>
    <clipPath id="cut"><path d="M42 23Q45 18 53 19L201 16Q209 16 209 21L204 213Q191 215 178 212L47 217Q39 215 39 208Z"/></clipPath></defs>
    <path d="M42 23Q45 18 53 19L201 16Q209 16 209 21L204 213Q191 215 178 212L47 217Q39 215 39 208Z" fill="url(#cloth)" stroke="#332f29" stroke-width="3.5"/>
    <g clip-path="url(#cut)"><ellipse cx="98" cy="79" rx="114" ry="117" fill="url(#stain)"/><ellipse cx="199" cy="173" rx="40" ry="103" fill="url(#stain)"/>${dots.join('')}</g>
    <path d="M49 24l148-4m9 5-5 180-5 3-143 4" fill="none" stroke="${worn}" stroke-width="1.2" opacity=".57"/>`;
}

function stitch(y, pathId, tone) {
  const hole = `<ellipse cx="61" cy="${y+1}" rx="3.7" ry="4.3" fill="#332d26"/>`;
  if (pathId===2)
    return `${hole}<path d="M39 ${y-5}q10 0 23 3l-1 8q-14 0-22-3Z" fill="#393329" stroke="#927653"/><path d="M40 ${y-2}l19 3" fill="none" stroke="#b69a71" stroke-width="1.2"/><circle cx="57" cy="${y+1}" r="2" fill="#c3a26c"/>`;
  const string = pathId===3?`M40 ${y+5}Q52 ${y+7} 61 ${y}M41 ${y-4}q7 1 20 4`:`M40 ${y+1}q11 3 21-1`;
  const bead=pathId===1?`<ellipse cx="49" cy="${y+2}" rx="3" ry="2.7" fill="${tone}" stroke="#5f6958" stroke-width=".7"/>`:'';
  return `${hole}<path d="${string}" fill="none" stroke="#3b342b" stroke-width="4.7"/><path d="${string}" fill="none" stroke="#cbbd9c" stroke-width="2.2"/>${bead}`;
}
function binding(pathId, theme) {
  const [, ,worn,,light] = theme;
  const stations = pathId===3 ? [43,70,120,169,195] : pathId===2 ? [51,113,178] : [45,82,150,191];
  const background = {
    1: '<path d="M42 23l12-1-6 191-9 1Z" fill="#284038" opacity=".65"/><path d="m51 24-5 182" stroke="#a8b493" stroke-width="1" opacity=".7"/><ellipse cx="136" cy="105" rx="62" ry="58" fill="url(#stain)"/><path d="M85 139a63 63 0 0 1 76-99m-59 132a70 70 0 0 0 93-62" fill="none" stroke="'+worn+'" opacity=".44" stroke-width="1.1"/>',
    2: '<path d="M172 19l34-2-1 34-5-15q-8-12-28-15Zm29 163 2 27-31 2q15-5 22-14Z" fill="#a68a5d" stroke="#594c37" stroke-width="1"/><circle cx="200" cy="24" r="1.9" fill="'+light+'"/><circle cx="198" cy="205" r="1.8" fill="'+light+'"/>',
    3: '<path d="M42 23l12-2-6 192-8 1Z" fill="#9a8255" opacity=".44"/><path d="M43 25 39 207m12-181-4 181" fill="none" stroke="#d4bf93" stroke-width=".8"/><path d="M78 165q40-24 89-23 18 0 29-8m-105 41q29-17 59-17m24-7 20-7M80 71q18-16 54-20" fill="none" stroke="'+worn+'" opacity=".42" stroke-width="1.35"/>',
  };
  return `${background[pathId]}<path d="M46 22l15-1-6 192-13 1Z" fill="#302f29" opacity=".23"/><path d="M63 22 57 212" stroke="${worn}" stroke-width=".85" opacity=".65"/>${stations.map(y=>stitch(y,pathId,worn)).join('')}`;
}

function renderCover(id, pathId, elementId) {
  const theme=palettes[elementId], marks=scenes[id];
  if (!marks || !theme) throw Error(`Missing drawing or element pigment for icon ${id}`);
  const bindingId=[1,2,3].includes(pathId) ? pathId : 3;
  const [shade,,,ink,light,accent]=theme;
  const strokes=`<style>.ink{fill:${ink}}.deep{fill:${shade}}.light{fill:${light}}.accent{fill:${accent}}
    .outline,.engrave,.darkline{fill:none;stroke-linecap:round;stroke-linejoin:round}
    .outline{stroke:${ink};stroke-width:2}.fine{stroke-width:1}.bold{stroke-width:3.4}
    .engrave,.darkline{stroke:${shade};stroke-width:2}</style>`;
  const page=`<path d="M39 24q4-5 15-5l149-2q8 0 9 5l2 5-3 192-9 11-156 7-9-6Z" fill="#3d3730" stroke="#2b2925" stroke-width="3"/>
    <path d="M44 215 204 208l5-6-2 15-9 10-147 6-8-3Z" fill="#d7c8a9" stroke="#786a53" stroke-width="1.4"/>
    <path d="m46 223 151-6 9-7m-151 16 138-6m-142 10 94-3 52-4" fill="none" stroke="#93846e" stroke-width=".9"/>
    <path d="m45 231 9 2 144-6 10-9-4 8-7 5-146 7-9-4Z" fill="#554a39"/>`;
  return `<svg xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" viewBox="0 0 252 252" width="256" height="256">
    ${strokes}<defs>${defs}</defs><g transform="rotate(-6 125 127)">${page}${coverSurface(id,theme,bindingId)}${binding(bindingId,theme)}
    <g transform="translate(66 44) scale(1.33)">${marks.join('')}</g>
    <path d="m49 214 8 1m142-10 3-14M42 25l8-3m148-5 7 1" fill="none" stroke="${light}" stroke-width="1.5" opacity=".62"/></g></svg>`;
}

async function main() {
  const options=process.argv.slice(2);
  function arg(flag, fallback) {
    const i=options.indexOf(flag);
    if(i<0)return fallback;
    if(!options[i+1]||options[i+1].startsWith('--'))throw Error('Missing argument for '+flag);
    return options[i+1];
  }
  const configPath=arg('--config',path.join(dataDir,'cultivation_affinities.json'));
  const config=JSON.parse(fs.readFileSync(configPath));
  const localePath=arg('--locale',configPath.replace(/\.json$/,'.zh-CN.json'));
  const locale=JSON.parse(fs.readFileSync(localePath));
  if(!config.methods?.templates || !config.catalogVersion || config.catalogVersion!==locale.catalogVersion)
    throw Error('Export matching catalog and localized data before rendering.');
  const output=arg('--output',defaultOut), proof=arg('--proof',null);
  const ids=Object.entries(config.methods.templates).sort(([a],[b])=>Number(a)-Number(b));
  fs.mkdirSync(output,{recursive:true});
  const sprites=[];
  for (const [key,method] of ids) {
    const id=method.iconId;
    if (!locale.profiles[method.profileId]?.name) throw Error('Missing localized profile for '+key);
    const element=config.profiles[method.profileId].primaryElementId;
    const sprite=await sharp(Buffer.from(renderCover(id,method.pathId,element)),{density:192})
      .resize(256,256,{kernel:'lanczos3'}).png().toBuffer();
    fs.writeFileSync(path.join(output,id+'.png'),sprite);
    sprites.push([key,sprite]);
  }
  if (proof) {
    const size=70, step=76, columns=10;
    const inputs=[];
    for (let i=0;i<sprites.length;i++) inputs.push({
      input:await sharp(sprites[i][1]).resize(size,size).png().toBuffer(),
      left:(i%columns)*step+3,top:Math.floor(i/columns)*step+3,
    });
    await sharp({create:{width:columns*step,height:Math.ceil(sprites.length/columns)*step,channels:3,background:'#d4c7af'}})
      .composite(inputs).png().toFile(proof);
  }
  console.log('Rendered '+ids.length+' method sprites.');
}
main().catch(error=>{console.error(error.message);process.exitCode=1;});
