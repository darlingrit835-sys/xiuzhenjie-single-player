(() => {
  'use strict';

  const sheet = new Image();
  const scriptUrl = document.currentScript && document.currentScript.src;
  sheet.src = new URL(scriptUrl ? '../assets/void-realm/seal-frames.png' : 'assets/void-realm/seal-frames.png',
    scriptUrl || document.baseURI).href;

  const mounts = new WeakMap();
  const tau = Math.PI * 2;
  const tones = {
    jade: '#91dfbd', gold: '#f2c079', violet: '#c3abf5', blue: '#91ccef',
    ember: '#f19a6c', ink: '#9ccbc6', mint: '#91dfbd', green: '#8ec7a8',
    copper: '#df986b', cyan: '#83d5d5', indigo: '#b1adf3', crimson: '#e9978c'
  };
  const clamp = (value, low, high) => Math.min(high, Math.max(low, value));
  const tint = (rgb, alpha) => 'rgba(' + rgb.join(',') + ',' + alpha + ')';

  function color(input) {
    let hex = tones[input] || String(input || tones.jade);
    if (/^#[a-f\d]{3}$/i.test(hex)) hex = '#' + [...hex.slice(1)].map(n => n + n).join('');
    if (!/^#[a-f\d]{6}$/i.test(hex)) hex = tones.jade;
    return [1, 3, 5].map(start => parseInt(hex.slice(start, start + 2), 16));
  }

  function noise(id, salt) {
    const n = Math.sin(id * 41.263 + salt * 16.827) * 41976.173;
    return n - Math.floor(n);
  }

  function motes(count) {
    return Array.from({length: count}, (_, i) => ({
      angle: tau * noise(i, 1),
      phase: noise(i, 2),
      speed: 0.055 + 0.065 * noise(i, 3),
      size: 0.6 + 0.8 * noise(i, 4),
      curl: 0.35 + noise(i, 5) * 0.75,
      gold: i % 3 === 0
    }));
  }

  function mount(root, options = {}) {
    if (!root || !root.querySelector) return;
    const previous = mounts.get(root);
    if (previous) previous.destroy();
    const canvas = root.matches && root.matches('canvas[data-vr-fx]') ? root : root.querySelector('canvas[data-vr-fx]');
    if (!canvas) return;

    const previousCanvas = mounts.get(canvas);
    if (previousCanvas) previousCanvas.destroy();
    const ctx = canvas.getContext('2d');
    if (!ctx) return;
    const tier = clamp(Number(options.tier) || 1, 1, 9);
    const strength = (tier - 1) / 8;
    const mode = options.mode === 'room' || options.mode === 'reveal' ? options.mode : 'hall';
    const inHall = mode === 'hall';
    const reveal = mode === 'reveal';
    const rgb = color(options.tone);
    const gold = [255, 216, 144];
    const sealBand = tier >= 5 ? 3 : tier >= 3 ? 2 : 1;
    const ink = [115, 144, 133], jade = [129, 233, 194], darkJade = [34, 81, 68];
    const sealColor = [ink, jade, gold][sealBand - 1];
    const particleData = motes(inHall ? 12 : Math.round(16 + strength * 27));
    const preference = window.matchMedia('(prefers-reduced-motion: reduce)');
    const point = (value, fallback) => value === undefined || value === '' || !Number.isFinite(Number(value))
      ? fallback : clamp(Number(value), 0, 1);
    const xFraction = point(canvas.dataset.vrX, inHall ? 0.73 : reveal ? 0.5 : 0.7);
    const yFraction = point(canvas.dataset.vrY, inHall ? 0.43 : reveal ? 0.48 : 0.43);
    const artX = point(canvas.dataset.vrArtX, null);
    const artY = point(canvas.dataset.vrArtY, null);
    const oldStyle = canvas.getAttribute('style');
    canvas.setAttribute('aria-hidden', 'true');
    Object.assign(canvas.style, {position: 'absolute', inset: '0', width: '100%', height: '100%', pointerEvents: 'none'});
    let width = 0, height = 0, cx = 0, cy = 0, ratio = 1, elapsed = 0, lastFrame = null, raf = 0;
    let visible = true, disposed = false;

    function glow(x, y, radius, colors) {
      const g = ctx.createRadialGradient(x, y, 0, x, y, radius);
      colors.forEach(([stop, color]) => g.addColorStop(stop, color));
      ctx.fillStyle = g;
      ctx.fillRect(x - radius, y - radius, radius * 2, radius * 2);
    }

    function glitter(x, y, size, opacity) {
      ctx.fillStyle = tint(gold, opacity);
      ctx.beginPath();
      ctx.moveTo(x, y - size * 2.3);
      ctx.lineTo(x + size * 0.38, y - size * 0.38);
      ctx.lineTo(x + size * 1.65, y);
      ctx.lineTo(x + size * 0.38, y + size * 0.38);
      ctx.lineTo(x, y + size * 2.3);
      ctx.lineTo(x - size * 0.38, y + size * 0.38);
      ctx.lineTo(x - size * 1.65, y);
      ctx.lineTo(x - size * 0.38, y - size * 0.38);
      ctx.closePath();
      ctx.fill();
    }

    function ring(radius, color, weight, start = 0, end = tau) {
      ctx.beginPath();
      ctx.strokeStyle = color;
      ctx.lineWidth = weight;
      ctx.arc(0, 0, radius, start, end);
      ctx.stroke();
    }

    function cloudStud(radius, angle, size) {
      ctx.save();
      ctx.rotate(angle);
      ctx.translate(radius, 0);
      ctx.scale(size, size);
      // 两侧云瓣围住金钉，光束沿轮辐向外收尖。
      ctx.beginPath();
      ctx.moveTo(-0.6, -3.3);
      ctx.bezierCurveTo(2.5, -3.7, 3.3, -1.5, 1.9, -0.8);
      ctx.lineTo(5.5, 0);
      ctx.lineTo(1.9, 0.8);
      ctx.bezierCurveTo(3.3, 1.5, 2.5, 3.7, -0.6, 3.3);
      ctx.strokeStyle = tint(gold, 0.85);
      ctx.lineWidth = 0.8;
      ctx.stroke();
      glitter(0, 0, 1.4, 0.98);
      ctx.restore();
    }

    function drawRoomSeal(cx, cy, radius, time, pulse) {
      ctx.save();
      ctx.translate(cx, cy);
      const scale = clamp(radius / 72, 0.7, 1.65);
      const front = time * 0.13;
      ctx.lineCap = 'round';
      ctx.globalCompositeOperation = 'source-over';
      if (sealBand === 1) {
        ring(radius * 0.84, tint([37, 54, 47], 0.78), 2.7 * scale);
        ring(radius * 0.84, tint(ink, 0.86), 1.25 * scale);
        ring(radius * 0.84, tint([205, 218, 202], 0.68), 1.3 * scale, front - 0.43, front + 0.43);
      } else if (sealBand === 2) {
        [0.78, 0.98].forEach((factor, i) => {
          const r = radius * factor, angle = i ? -front : front;
          ring(r, tint(darkJade, 0.85), (i ? 3.9 : 2.5) * scale);
          ring(r, tint(jade, i ? 0.95 : 0.75), (i ? 2.1 : 1.25) * scale);
          ring(r, 'rgba(223,255,238,.92)', 2 * scale, angle + 1.2, angle + 1.67);
        });
      } else {
        [0.77, 0.95, 1.10].forEach((factor, i) => {
          const r = radius * factor;
          ring(r, 'rgba(92,58,24,.87)', (i === 2 ? 4.7 : 3.2) * scale);
          ring(r, tint(gold, i === 2 ? 0.98 : 0.79), (i === 2 ? 2.8 : 1.5) * scale);
        });
        const studs = tier >= 8 ? 12 : 8;
        for (let n = 0; n < studs; n++) {
          const a = tau * n / studs + front * 0.45;
          ring(radius * 1.076, tint(gold, 0.78), 0.9 * scale, a + 0.065, a + tau / studs - 0.065);
          cloudStud(radius * 1.10, a, scale * (1.10 + pulse * 0.28));
        }
      }
      // 内侧短弧沿用本馆职业色，金、玉、墨主轮仍由职业品阶决定。
      ring(radius * 0.72, tint(rgb, 0.82), 1.8 * scale, -front + 2.2, -front + 3.05);
      ctx.restore();
    }

    function draw(time) {
      if (!width || !height) return;
      ctx.setTransform(ratio, 0, 0, ratio, 0, 0);
      ctx.clearRect(0, 0, width, height);
      ctx.globalCompositeOperation = 'screen';
      const radius = Math.min(height * (reveal ? 0.38 : 0.35), width * (reveal ? 0.25 : 0.2));
      const breathe = (1 + Math.sin(time * 1.4)) * 0.5;
      const pulse = reveal ? Math.pow(1 - Math.min(time / 1.7, 1), 2) : 0;
      const haloSize = radius * (1.37 + strength * 0.28 + pulse * 0.22);

      glow(cx, cy, haloSize, [[0, tint(rgb, (inHall ? 0.05 : 0.10) + pulse * 0.17)],
        [0.47, tint(inHall ? rgb : sealColor, inHall ? 0.05 : 0.10 + strength * 0.06 + breathe * 0.025)],
        [0.64, tint(rgb, 0.05)], [1, tint(rgb, 0)]]);
      if (!inHall && (sealBand > 1 || reveal)) {
        const rays = 4 + Math.floor(strength * 5) + (reveal ? 2 : 0);
        for (let i = 0; i < rays; i++) {
          const angle = tau * i / rays + time * 0.045;
          const reach = radius * (1.2 + 0.45 * noise(i, 9));
          const gradient = ctx.createLinearGradient(cx, cy, cx + Math.cos(angle) * reach, cy + Math.sin(angle) * reach);
          gradient.addColorStop(0, tint(rgb, 0.01));
          gradient.addColorStop(0.54, tint(rgb, 0.06 + 0.06 * strength + pulse * 0.08));
          gradient.addColorStop(1, tint(rgb, 0));
          ctx.fillStyle = gradient;
          ctx.beginPath();
          ctx.moveTo(cx, cy);
          ctx.lineTo(cx + Math.cos(angle - 0.025) * reach, cy + Math.sin(angle - 0.025) * reach);
          ctx.lineTo(cx + Math.cos(angle + 0.025) * reach, cy + Math.sin(angle + 0.025) * reach);
          ctx.closePath();
          ctx.fill();
        }
      }

      if (sheet.complete && sheet.naturalWidth > 0) {
        const index = Math.floor(time * (reveal ? 8 : 5) + 1) % 16;
        const tile = sheet.naturalWidth / 4;
        const diameter = 2 * radius * (1 + pulse * 0.12);
        ctx.globalAlpha = inHall ? 0.33 : 0.27 + strength * 0.18;
        ctx.drawImage(sheet, (index % 4) * tile, Math.floor(index / 4) * tile,
          tile, tile, cx - diameter / 2, cy - diameter / 2, diameter, diameter);
        ctx.globalAlpha = 1;
      }
      if (!inHall) drawRoomSeal(cx, cy, radius, time, pulse);
      for (const p of particleData) {
        const progress = (p.phase + time * p.speed) % 1;
        const angle = p.angle + progress * p.curl;
        const distance = radius * (1.9 - progress * 1.34);
        const x = cx + Math.cos(angle) * distance, y = cy + Math.sin(angle) * distance * 0.85;
        const fade = Math.sin(Math.PI * progress);
        const size = p.size * clamp(radius / 68, 0.65, 1.6);
        ctx.fillStyle = tint(p.gold ? (inHall ? gold : sealColor) : rgb, fade * (inHall ? 0.38 : 0.52 + strength * 0.25));
        ctx.beginPath();
        ctx.arc(x, y, size, 0, tau);
        ctx.fill();
        if (p.gold && progress > 0.34 && progress < 0.66) glitter(x, y, size * 1.3, fade * (inHall ? 0.29 : 0.74));
      }
    }

    function positionFraction(position) {
      const first = position.split(',')[0].trim();
      if (first.endsWith('%')) return parseFloat(first) / 100;
      if (first === 'left' || first === 'top') return 0;
      if (first === 'right' || first === 'bottom') return 1;
      return 0.5;
    }

    function resize() {
      const rect = canvas.getBoundingClientRect();
      width = rect.width;
      height = rect.height;
      cx = width * xFraction;
      cy = height * yFraction;
      if (artX !== null && artY !== null && canvas.parentElement) {
        // 厅堂、书斋原画均为 1024×1536，以 CSS cover 后的画心定位。
        const artWidth = Math.max(width, height * 2 / 3);
        const artHeight = artWidth * 1.5;
        const style = getComputedStyle(canvas.parentElement);
        cx = artX * artWidth + (width - artWidth) * positionFraction(style.backgroundPositionX);
        cy = artY * artHeight + (height - artHeight) * positionFraction(style.backgroundPositionY);
      }
      ratio = Math.min(window.devicePixelRatio || 1, 2);
      const pixels = [Math.round(width * ratio), Math.round(height * ratio)];
      if (pixels[0] !== canvas.width || pixels[1] !== canvas.height) {
        canvas.width = pixels[0];
        canvas.height = pixels[1];
      }
      if (!document.hidden && visible) draw(preference.matches ? 2 : elapsed);
    }

    function tick(now) {
      raf = 0;
      if (!canvas.isConnected) { destroy(); return; }
      if (lastFrame === null) lastFrame = now;
      if (now - lastFrame >= 1000 / 24) {
        elapsed += Math.min(now - lastFrame, 100) * 0.001;
        lastFrame = now;
        draw(elapsed);
      }
      raf = requestAnimationFrame(tick);
    }

    function refresh() {
      if (raf) cancelAnimationFrame(raf);
      raf = 0;
      lastFrame = null;
      if (!canvas.isConnected) { destroy(); return; }
      if (disposed || !visible || document.hidden) return;
      resize();
      if (!preference.matches) raf = requestAnimationFrame(tick);
    }

    const sizeObserver = window.ResizeObserver ? new ResizeObserver(resize) : null;
    const viewObserver = window.IntersectionObserver ? new IntersectionObserver(entries => {
      visible = entries.some(entry => entry.isIntersecting);
      refresh();
    }) : null;
    function destroy() {
      if (disposed) return;
      disposed = true;
      cancelAnimationFrame(raf);
      if (sizeObserver) sizeObserver.disconnect();
      if (viewObserver) viewObserver.disconnect();
      window.removeEventListener('resize', resize);
      document.removeEventListener('visibilitychange', refresh);
      sheet.removeEventListener('load', refresh);
      if (preference.removeEventListener) preference.removeEventListener('change', refresh);
      else preference.removeListener(refresh);
      ctx.setTransform(1, 0, 0, 1, 0, 0);
      ctx.clearRect(0, 0, canvas.width, canvas.height);
      if (oldStyle === null) canvas.removeAttribute('style');
      else canvas.setAttribute('style', oldStyle);
      mounts.delete(root);
      mounts.delete(canvas);
    }
    const instance = {destroy};
    mounts.set(root, instance);
    mounts.set(canvas, instance);
    if (sizeObserver) sizeObserver.observe(canvas);
    if (viewObserver) viewObserver.observe(canvas);
    window.addEventListener('resize', resize);
    document.addEventListener('visibilitychange', refresh);
    sheet.addEventListener('load', refresh);
    if (preference.addEventListener) preference.addEventListener('change', refresh);
    else preference.addListener(refresh);
    refresh();
    return instance;
  }

  window.voidRealmFx = { mount, destroy(root) { const owner = mounts.get(root); if (owner) owner.destroy(); } };
})();
