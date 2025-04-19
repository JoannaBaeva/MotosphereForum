document.addEventListener('DOMContentLoaded', function () {
    const slider = document.getElementById('priceSlider');
    if (!slider || typeof noUiSlider === 'undefined') return;

    const minInput = document.getElementById('minPriceInput');
    const maxInput = document.getElementById('maxPriceInput');
    const minHidden = document.getElementById('minPrice');
    const maxHidden = document.getElementById('maxPrice');

    const minInitial = parseFloat(minHidden.value) || 0;
    const maxInitial = parseFloat(maxHidden.value) || 100000;

    noUiSlider.create(slider, {
        start: [minInitial, maxInitial],
        connect: true,
        range: { min: 0, max: 100000 },
        step: 1,
        tooltips: true,
        format: {
            to: val => Math.round(val),
            from: val => Number(val)
        }
    });

    slider.noUiSlider.on('update', function (values) {
        minInput.value = minHidden.value = values[0];
        maxInput.value = maxHidden.value = values[1];
    });

    function updateSlider() {
        slider.noUiSlider.set([minInput.value, maxInput.value]);
    }

    minInput.addEventListener('change', updateSlider);
    maxInput.addEventListener('change', updateSlider);
});