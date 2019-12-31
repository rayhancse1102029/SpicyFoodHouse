
$(document).ready(function () {
    $('.owl-carousel').owlCarousel({
        loop: true,
        margin: 10,
        nav: true,
        navText: ['<span><i style="font-size:25px" class="fas fa-angle-left"></i></span>', ' <span><i style="font-size:25px" class="fas fa-angle-right"></i></span>'],
        responsive: {
            0: {
                items: 1
            },
            600: {
                items: 3
            },
            1000: {
                items: 5
            }
        }
    });
});