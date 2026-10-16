window.offgrid = (() => {
    let io = null;
    const ensure = () => io ??= new IntersectionObserver((entries) => {
        entries.forEach((e) => {
            if (e.isIntersecting) { e.target.classList.add('is-visible'); io.unobserve(e.target); }
        });
    }, { threshold: 0.12, rootMargin: '0px 0px -6% 0px' });

    return {
        ready() { window.__ogReady = true; },
        observe(el) { if (el) ensure().observe(el); },
        unobserve(el) { if (el && io) io.unobserve(el); },
        scrollTop() { window.scrollTo({ top: 0, behavior: 'instant' }); },
        scrollToId(id) {
            const reduce = matchMedia('(prefers-reduced-motion: reduce)').matches;
            document.getElementById(id)?.scrollIntoView({ behavior: reduce ? 'auto' : 'smooth' });
        },
    };
})();

// If Blazor never connects, don't leave reveal sections hidden.
window.addEventListener('load', () => setTimeout(() => {
    if (!window.__ogReady) document.documentElement.classList.remove('js');
}, 5000));