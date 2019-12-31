/*slick slider*/
$('.quotes').slick({
  dots: false,
  infinite: true,
  autoplay: true,
  autoplaySpeed: 6000,
  speed: 800,
  slidesToShow: 1,
  adaptiveHeight: false
});
/*slick slider*/
/*scroll fix*/
$(window).scroll(function(){
    if ($(window).scrollTop() >= 80) {
       $('.fix-nav').addClass('fixed-header');
    }
    else {
       $('.fix-nav').removeClass('fixed-header');
    }
	
});
//modal
$(".for-1").on( "click", function() {
        $('#myModal').modal('hide');  
});

$(".for-1").on( "click", function() {
		$('#myModal2').modal('hide');  
});
/*scroll fix*/
/*product image zoom*/
function zoomAT(element) {
    var box = element;
    jq = jQuery;
    var lazy = jq(element).find('.box-zoom-lazy');
    var parentOffset = jq(box).offset();
    var boxWidth = 100 / jq(box).width();
    var boxHeight = 100 / jq(box).height();
    var spaceLeft = parentOffset.left;
    var spaceTop = parentOffset.top;
    
    var showZoom = true;
    
    this.showZoom = function(opt = true){
        showZoom = opt;
    }
        
    //initial
    var lazy_image = jq(lazy).attr('src');
    jq(box).css('background-image', "url('" + lazy_image + "')");
    jq(lazy).remove();

    //on page resize recalcule sizes fixeds
    $(window).resize(function() {
        var parentOffset = jq(box).offset();
        boxWidth = 100 / jq(box).width();
        boxHeight = 100 / jq(box).height();
        spaceLeft = parentOffset.left;
        spaceTop = parentOffset.top;
        console.log(spaceLeft);
    });

    this.setImage = function(urlImage) {
        jq(box).css('background-image', "url('" + urlImage + "')");
    }

    jq(box).click(function() {
        jq(this).toggleClass('zoom-plus');
    });

    jq(box).hover(function() {
        if(showZoom){
            jq(box).addClass('active');
            jq(this).mousemove(function(e) {
                var relX = boxWidth * (e.pageX - spaceLeft);
                var relY = boxHeight * (e.pageY - spaceTop);
                jq(this).css('background-position', relX + '% ' + relY + '%');
            });
         }
    }, function() {
        jq(box).removeClass('active');
        jq(box).css('background-position', '50% 50%');
    });
}

var zoom = new zoomAT($('.box-zoom'));
zoom.setImage('~/images/Slider-One.jpg');

//zoom.showZoom(false);
/*product image zoom*/
/*input effect*/

/*input effect*/
